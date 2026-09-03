$sqlFilePath = "d:\AutoPartsERP\backend\database\schema_and_seed.sql"
$masterConnStr = "Server=(localdb)\mssqllocaldb;Integrated Security=True;TrustServerCertificate=True;"

try {
    Write-Host "Connecting to SQL Server LocalDB..." -ForegroundColor Cyan
    $conn = New-Object System.Data.SqlClient.SqlConnection($masterConnStr)
    $conn.Open()
    Write-Host "Successfully connected to SQL Server LocalDB!" -ForegroundColor Green

    # Create Database if not exists
    $createDbSql = @"
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'AutoPartsERP_DB')
BEGIN
    CREATE DATABASE AutoPartsERP_DB;
END
"@
    $cmd = $conn.CreateCommand()
    $cmd.CommandText = $createDbSql
    $cmd.ExecuteNonQuery() | Out-Null
    Write-Host "Database 'AutoPartsERP_DB' is ready!" -ForegroundColor Green
    $conn.Close()

    # Now connect to AutoPartsERP_DB and run schema & seed
    $dbConnStr = "Server=(localdb)\mssqllocaldb;Database=AutoPartsERP_DB;Integrated Security=True;TrustServerCertificate=True;"
    $dbConn = New-Object System.Data.SqlClient.SqlConnection($dbConnStr)
    $dbConn.Open()

    $sqlContent = Get-Content $sqlFilePath -Raw -Encoding UTF8
    
    # Split by GO statements
    $batches = $sqlContent -split "(?i)\r?\n\s*GO\s*\r?\n"
    
    $batchCount = 0
    foreach ($batch in $batches) {
        $trimmed = $batch.Trim()
        # Skip USE and CREATE DATABASE statements if present in batch
        if ($trimmed -and -not ($trimmed -match "^CREATE DATABASE" -or $trimmed -match "^USE ")) {
            try {
                $batchCmd = $dbConn.CreateCommand()
                $batchCmd.CommandText = $trimmed
                $batchCmd.CommandTimeout = 120
                $batchCmd.ExecuteNonQuery() | Out-Null
                $batchCount++
            } catch {
                Write-Host "Batch error (ignoring if tables already exist): $($_.Exception.Message)" -ForegroundColor Yellow
            }
        }
    }
    
    Write-Host "Executed $batchCount SQL batches successfully into AutoPartsERP_DB!" -ForegroundColor Green
    $dbConn.Close()
} catch {
    Write-Host "Error: $($_.Exception.Message)" -ForegroundColor Red
}
