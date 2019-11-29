$foundCert = Test-Certificate -Cert Cert:\CurrentUser\my\43eb601ecc35ed5263141d4dc4aff9c77858451c -User
if(!$foundCert)
{
    Write-Host "Certificate doesn't exist. Exit."
    exit
}

Write-Host "Certificate found. Sign the assemblies."
$signtool = "C:\Program Files (x86)\Microsoft SDKs\ClickOnce\SignTool\signtool.exe"
foreach ($line in Get-Content .\sign.txt) {
    & $signtool sign /tr http://timestamp.digicert.com /td sha256 /fd sha256 /a .\bin\$line | Write-Debug
    if ($LASTEXITCODE -ne 0)
    {
        Write-Host ".\bin\$line is not signed. Exit."
        exit $LASTEXITCODE
    }
}

Write-Host "Verify digital signature."
$files = Get-ChildItem .\bin\* -Include ('*.dll', "*.exe") -File
$files | ForEach-Object {
    & $signtool verify /pa /q $_.FullName 2>&1 | Write-Debug
    if ($LASTEXITCODE -ne 0)
    {
        Write-Host "$_.FullName is not signed. Exit."
        exit $LASTEXITCODE
    }
}

Write-Host "Verification finished."
