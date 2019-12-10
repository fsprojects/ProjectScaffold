@echo off
cls

.paket\paket.exe restore -s
if errorlevel 1 (
  exit /b %errorlevel%
)

IF EXIST init.fsproj (
  yarn add webpack-cli@3.1.2 --dev
  yarn add webpack-cli --dev
  dotnet run --project .\init.fsproj %*
)

dotnet restore
dotnet run --project --configuration Release --project .\.build\Build.fsproj %*
