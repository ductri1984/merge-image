@echo off
SETLOCAL EnableExtensions

set EXE1=Logdata.exe
set EXE2=rabbit_execute.exe

FOR /F %%x IN ('tasklist /NH /FI "IMAGENAME eq %EXE1%"') DO IF %%x == %EXE1% goto ProcessFound1

goto ProcessNotFound

:ProcessFound1
FOR /F %%x IN ('tasklist /NH /FI "IMAGENAME eq %EXE2%"') DO IF %%x == %EXE2% goto ProcessFound2

goto ProcessNotFound

:ProcessFound2
echo true
goto END

:ProcessNotFound
echo false
goto END

:END
exit