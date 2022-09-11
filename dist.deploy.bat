CALL dist.ci.bat
IF %ERRORLEVEL% NEQ 0 goto failed

pwsh -ExecutionPolicy Bypass -file sign.ps1
IF %ERRORLEVEL% NEQ 0 goto failed

CALL build.installer.bat
IF %ERRORLEVEL% NEQ 0 goto failed

pwsh -ExecutionPolicy Bypass -file sign.installers.ps1
IF %ERRORLEVEL% NEQ 0 goto failed

pwsh -ExecutionPolicy Bypass -file sha1.ps1

echo succeeded.
exit /b 0

:failed
echo failed.
exit /b 1
