powershell -file download.dotnet.releases.ps1
call dist.deploy.bat
powershell -file sha1.ps1
pause