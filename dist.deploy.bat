rmdir /S /Q bin
call release.bat
call sign.bat
call package.bat
@IF %ERRORLEVEL% NEQ 0 PAUSE