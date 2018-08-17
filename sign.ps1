if(![System.IO.File]::Exists('F:\sign.txt'))
{
    exit
}

$signtool = "C:\Program Files (x86)\Windows Kits\10\bin\10.0.17134.0\x64\signtool.exe"
foreach ($line in Get-Content .\sign.txt) {
    & $signtool sign /tr http://timestamp.digicert.com /td sha256 /fd sha256 /a .\bin\$line | Write-Debug
}

$files = Get-ChildItem .\bin\* -Include ('*.dll', "*.exe") -File
$files | ForEach-Object {
    & $signtool verify /pa /q $_.FullName 2>&1 | Write-Debug
    if ($LASTEXITCODE -ne 0)
    {
        Write-Host $_.FullName is not signed.
        exit $LASTEXITCODE
    }
}