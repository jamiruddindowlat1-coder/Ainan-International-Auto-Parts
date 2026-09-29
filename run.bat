@echo off
echo Starting AutoPartsERP...

start "AutoPartsERP Backend" cmd /k "cd /d D:\AutoPartsERP\backend\AutoPartsERP.API && dotnet run"

start "AutoPartsERP Frontend" cmd /k "cd /d D:\AutoPartsERP\frontend && npm run dev"

timeout /t 6 /nobreak > nul

start http://localhost:5272/scalar/v1
start http://localhost:3000/

echo Both servers started.
 