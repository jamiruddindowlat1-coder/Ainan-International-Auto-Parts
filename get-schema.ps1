# Step 1: Auto-detect DB name from appsettings.json
$appsettings = Get-ChildItem -Path . -Recurse -Filter "appsettings.json" | Select-Object -First 1
$content = Get-Content $appsettings.FullName -Raw
if ($content -match 'Initial Catalog=([^;]+)') {
    $dbName = $matches[1]
} elseif ($content -match 'Database=([^;]+)') {
    $dbName = $matches[1]
} else {
    Write-Host "Database name auto-detect failed. Please check appsettings.json manually."
    exit
}
Write-Host "Detected Database: $dbName"

# Step 2: Load SMO (SQL Server Management Objects)
[System.Reflection.Assembly]::LoadWithPartialName("Microsoft.SqlServer.Smo") | Out-Null
[System.Reflection.Assembly]::LoadWithPartialName("Microsoft.SqlServer.SmoExtended") | Out-Null

$serverName = ".\SQLEXPRESS"
$server = New-Object Microsoft.SqlServer.Management.Smo.Server($serverName)
$db = $server.Databases[$dbName]

if ($null -eq $db) {
    Write-Host "Could not find database '$dbName' on $serverName. Check the name and try again."
    exit
}

# Step 3: Configure the scripter
$scripter = New-Object Microsoft.SqlServer.Management.Smo.Scripter($server)
$scripter.Options.ScriptDrops = $false
$scripter.Options.WithDependencies = $true
$scripter.Options.IncludeIfNotExists = $true
$scripter.Options.Indexes = $true
$scripter.Options.DriPrimaryKey = $true
$scripter.Options.DriForeignKeys = $true
$scripter.Options.DriDefaults = $true
$scripter.Options.NoCollation = $true
$scripter.Options.ToFileOnly = $true
$scripter.Options.FileName = "D:\AutoPartsERP\schema.sql"
$scripter.Options.Encoding = New-Object System.Text.UTF8Encoding $false

# Step 4: Script all user tables (skips system tables)
$tables = $db.Tables | Where-Object { -not $_.IsSystemObject }
$scripter.Script($tables)

Write-Host "Done. Schema exported to: D:\AutoPartsERP\schema.sql"