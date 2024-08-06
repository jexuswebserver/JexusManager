if ($env:CI -eq "true") {
    exit 0
}

$cert = Get-ChildItem -Path Cert:\CurrentUser\My -CodeSigningCert | Select-Object -First 1
if ($null -eq $cert) {
    Write-Host "No code signing certificate found in MY store. Exit."
    exit 1
}

Write-Host "Certificate found. Sign the assemblies."
$signtool = Get-ChildItem -Path "${env:ProgramFiles(x86)}\Windows Kits" -Recurse -Filter "signtool.exe" | Select-Object -First 1 -ExpandProperty FullName
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
