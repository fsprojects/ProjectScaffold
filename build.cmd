@echo off
cls

.\paket.bootstrapper.exe
if errorlevel 1 (
  exit /b %errorlevel%
)

.\paket.exe restore
if errorlevel 1 (
  exit /b %errorlevel%
)

IF NOT EXIST build.fsx (
  .\paket.exe update
  packages\build\FAKE\tools\FAKE.exe init.fsx
)
packages\build\FAKE\tools\FAKE.exe build.fsx %*
