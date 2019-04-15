@echo off
cls

dotnet restore build.proj

IF NOT EXIST build.fsx (
  fake.cmd run init.fsx
)
fake.cmd build %*
