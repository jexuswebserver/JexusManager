set signtool="C:\Program Files (x86)\Windows Kits\10\bin\10.0.17134.0\x64\signtool.exe"
if exist F:\sign.txt (
mkdir bin
cd bin
for /F "tokens=*" %%A in (..\sign.txt) do (%signtool% sign /tr http://timestamp.digicert.com /td sha256 /fd sha256 /a "%%A")
for /r %%i in (*.exe *.dll) do (
    %signtool% verify /pa /q "%%i"
    @IF %ERRORLEVEL% NEQ 0 PAUSE
)
cd ..
)