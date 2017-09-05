rmdir /S /Q bin
call release.bat
call package.bat
@IF %ERRORLEVEL% NEQ 0 PAUSE