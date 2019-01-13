@echo off
cls

.paket\paket.exe restore -s
if errorlevel 1 (
  exit /b %errorlevel%
)

IF EXIST init.fsx (
  fake -v run init.fsx
)
dotnet run --project src\Build\Build.fsproj %*
