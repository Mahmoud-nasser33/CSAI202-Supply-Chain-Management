@echo off
echo ========================================
echo   Starting Silsila Frontend Server
echo ========================================
echo.
echo Server will start on: http://localhost:5200
echo Press Ctrl+C to stop the server
echo.

cd /d "%~dp0"
dotnet run

pause
