@echo off
SETLOCAL EnableExtensions

set EXE1=ConsoleLog.exe
set EXE2=ConsoleSystemBG.exe

echo open file name less 20 char :::::::::::::::::::::::::::::::::::::::::::::::::::::

start cmd /k "E:\BackgroundSpaces\ConsoleLog\ConsoleLog.exe"
start cmd /k "E:\BackgroundSpaces\ConsoleSystemBG\ConsoleSystemBG.exe"

echo remove more :::::::::::::::::::::::::::::::::::::::::::::::::::::

FOR /F "tokens=2 skip=2" %%x IN ('tasklist /NH /FI "IMAGENAME eq %EXE1%"') DO taskkill /pid %%x /t /f 
FOR /F "tokens=2 skip=2" %%x IN ('tasklist /NH /FI "IMAGENAME eq %EXE2%"') DO taskkill /pid %%x /t /f 

exit