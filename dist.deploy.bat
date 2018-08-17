rmdir /S /Q bin
call release.bat
IF %ERRORLEVEL% NEQ 0 goto failed
powershell -file sign.ps1
IF %ERRORLEVEL% NEQ 0 goto failed
call package.bat
IF %ERRORLEVEL% NEQ 0 goto failed

echo succeeded.
exit /b 0

:failed
echo failed.
pause