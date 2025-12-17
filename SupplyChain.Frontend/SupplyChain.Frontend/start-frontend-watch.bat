@echo off
echo ========================================
echo   Starting Silsila Frontend (Hot Reload)
echo ========================================
echo.
echo Server will start on: http://localhost:5200
echo Hot reload enabled - changes will auto-refresh
echo Press Ctrl+C to stop the server
echo.

cd /d "%~dp0"
dotnet watch run

pause
