@echo off
:: Add the paths for the F# SDK 3.x (from higher version to lower)
set FSHARPSDK=^
C:\Program Files (x86)\Microsoft SDKs\F#\3.1\Framework\v4.0\;^
C:\Program Files (x86)\Microsoft SDKs\F#\3.0\Framework\v4.0\

cls
:: Execute the script "only" with the first "fsianycpu.exe" found
for %%i in (fsianycpu.exe) do "%%~$FSHARPSDK:i" rename.fsx %*

pause