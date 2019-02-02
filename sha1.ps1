$files = Get-ChildItem .\* -Include ('*.msi') -File
Write-Host "SHA1 code for the downloads are:"
$files | ForEach-Object {
    $hash = Get-FileHash -Path $_.FullName -Algorithm SHA1
    Write-Host $_.Name - $hash.Hash
}
