@echo off
cls

dotnet restore build.proj

IF NOT EXIST build.fsx (
  dotnet fake init.fsx
)
dotnet fake build %*
