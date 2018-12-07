Get-ChildItem * -Include *.zip | Remove-Item
$files = Get-Content .\list.txt
$sourceFolder = Get-Location
$releaseFolder = ".\JexusManager\bin\Release\netcoreapp3.0"
if (![System.IO.Directory]::Exists($releaseFolder))
{
    New-Item $releaseFolder -ItemType Directory
}

Set-Location $releaseFolder
Compress-Archive $files -CompressionLevel Optimal -DestinationPath $sourceFolder\JexusManager.zip
Set-Location $sourceFolder

Write-Host "Package is ready."