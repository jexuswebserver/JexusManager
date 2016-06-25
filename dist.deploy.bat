rmdir /S /Q bin
call release.bat
call merge.bat
call obfuscate.bat
call package.bat
@IF %ERRORLEVEL% NEQ 0 PAUSE