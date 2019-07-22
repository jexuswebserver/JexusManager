$foundCert = Test-Certificate -Cert Cert:\CurrentUser\my\43eb601ecc35ed5263141d4dc4aff9c77858451c -User
if(!$foundCert)
{
    Write-Host "Certificate doesn't exist. Exit."
    exit
}

Write-Host "Certificate found. Sign the assemblies."
$signtool = "C:\Program Files (x86)\Windows Kits\10\bin\10.0.17134.0\x64\signtool.exe"

Write-Host "Verify digital signature."
$files = Get-ChildItem .\* -Include ('*.msi') -File
$files | ForEach-Object {
    & $signtool sign /tr http://timestamp.digicert.com /td sha256 /fd sha256 /d "Jexus Manager" /a $_.FullName 2>&1 | Write-Debug
    & $signtool verify /pa /q $_.FullName 2>&1 | Write-Debug
    if ($LASTEXITCODE -ne 0)
    {
        Write-Host "$_.FullName is not signed. Exit."
        exit $LASTEXITCODE
    }
}

Write-Host "Verification finished."
