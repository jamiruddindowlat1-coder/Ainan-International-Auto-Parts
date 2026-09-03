[System.Reflection.Assembly]::LoadWithPartialName("Microsoft.SqlServer.Smo") | Out-Null
[System.Reflection.Assembly]::LoadWithPartialName("Microsoft.SqlServer.SmoExtended") | Out-Null
$dbName = "InternationalERPDb"
$serverName = ".\SQLEXPRESS"
$server = New-Object Microsoft.SqlServer.Management.Smo.Server($serverName)
$db = $server.Databases[$dbName]
if ($null -eq $db) {
    Write-Host "Still not found. Check exact name from the list you got earlier."
    exit
}
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
$tables = $db.Tables | Where-Object { -not $_.IsSystemObject }
$scripter.Script($tables)
Write-Host "Done. Schema exported to: D:\AutoPartsERP\schema.sql"
