@echo off

echo kill :::::::::::::::::::::::::::::::::::::::::::::::::::::

taskkill /im "rabbit_execute.exe" /t /f
taskkill /im "Logdata.exe" /t /f

echo open :::::::::::::::::::::::::::::::::::::::::::::::::::::

cd "C:\Workings\rabbit_execute\"
start cmd /k "rabbit_execute.exe"

cd "C:\Workings\Logdata\"
start cmd /k "Logdata.exe"

exit