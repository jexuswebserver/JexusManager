rmdir /S /Q bin
powershell -file release.ps1
IF %ERRORLEVEL% NEQ 0 goto failed

powershell -file sign.ps1
IF %ERRORLEVEL% NEQ 0 goto failed

CALL build.installer.bat
IF %ERRORLEVEL% NEQ 0 goto failed

powershell -file sign.installers.ps1
IF %ERRORLEVEL% NEQ 0 goto failed

echo succeeded.
exit /b 0

:failed
echo failed.
exit /b 1