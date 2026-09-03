$conn = New-Object System.Data.SqlClient.SqlConnection("Server=(localdb)\mssqllocaldb;Database=AutoPartsERP_DB;Integrated Security=True;TrustServerCertificate=True;")
$conn.Open()

$cmd = $conn.CreateCommand()
$cmd.CommandText = "SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE = 'BASE TABLE' ORDER BY TABLE_NAME;"
$reader = $cmd.ExecuteReader()
$tables = New-Object System.Collections.Generic.List[string]
while ($reader.Read()) {
    $tables.Add($reader.GetString(0))
}
$reader.Close()

Write-Host "==========================================" -ForegroundColor Cyan
Write-Host " Total Tables in AutoPartsERP_DB: $($tables.Count)" -ForegroundColor Green
Write-Host "==========================================" -ForegroundColor Cyan

foreach ($tbl in $tables) {
    $countCmd = $conn.CreateCommand()
    $countCmd.CommandText = "SELECT COUNT(*) FROM [$tbl];"
    $count = $countCmd.ExecuteScalar()
    Write-Host " - $tbl : $count rows" -ForegroundColor Yellow
}

$conn.Close()
