Remove-Item .\JexusManager.zip
$files = Get-Content .\list.txt
if (![System.IO.Directory]::Exists(".\bin"))
{
    New-Item .\bin -ItemType Directory
}

Set-Location .\bin
Compress-Archive $files -CompressionLevel Optimal -DestinationPath ..\JexusManager.zip
Set-Location ..

Write-Host "Package is ready."