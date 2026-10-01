@echo off
setlocal
cd /d "%~dp0"

echo [1/6] Docker check...
docker info >nul 2>&1
if errorlevel 1 (
    echo Docker Desktop chalu nei. Age Docker Desktop chalu korun.
    pause
    exit /b 1
)

echo [2/6] SQL Server container...
docker ps -a --format "{{.Names}}" | findstr /x "sqlserver" >nul
if errorlevel 1 (
    docker run -d --name sqlserver -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=YourStrong@Pass123" -p 1435:1433 -v sqldata:/var/opt/mssql mcr.microsoft.com/mssql/server:2022-latest
) else (
    docker start sqlserver >nul
)

echo [3/6] JWT secret...
if not exist jwt-secret.txt (
    powershell -NoProfile -Command "$b = New-Object byte[] 48; [System.Security.Cryptography.RandomNumberGenerator]::Create().GetBytes($b); [Convert]::ToBase64String($b)" > jwt-secret.txt
)
set /p JWT=<jwt-secret.txt

echo [4/6] API container (Docker)...
docker rm -f autopartserp-api >nul 2>&1
docker run -d --name autopartserp-api -p 5000:8080 -e "ASPNETCORE_ENVIRONMENT=Development" -e "Google__ClientId=521386348099-qvkp613ih614tojdssg26b773ptaai37.apps.googleusercontent.com" -e "JwtSettings__Secret=%JWT%" -e "ConnectionStrings__DefaultConnection=Server=host.docker.internal,1435;Database=AutoPartsERP_DB;User Id=sa;Password=YourStrong@Pass123;TrustServerCertificate=True;MultipleActiveResultSets=true" autopartserp-api

echo [5/6] Frontend...
start "AutoPartsERP Frontend" cmd /k "cd /d D:\AutoPartsERP\frontend && npm run dev"

echo [6/6] API chalu hoyar jonno opekkha...
set tries=0
:wait
curl -s -o nul -f http://localhost:5000/swagger/v1/swagger.json && goto ready
set /a tries+=1
if %tries% geq 30 goto ready
timeout /t 2 /nobreak >nul
goto wait

:ready
start "" http://localhost:5000/swagger
start "" http://localhost:5173/

echo Shob chalu. Ei window bondho korte jekono key chapun.
pause >nul