powershell -ExecutionPolicy Bypass -file download.dotnet.releases.ps1
call dist.deploy.bat
powershell -ExecutionPolicy Bypass -file sha1.ps1
pause