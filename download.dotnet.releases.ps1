$file = "./JexusManager/Resources/releases-index.json"
Invoke-WebRequest https://builds.dotnet.microsoft.com/dotnet/release-metadata/releases-index.json -OutFile $file

if (-not (Test-Path $file)) {
    Write-Error "Couldn't find releases-index.json. Exit."
    exit 1
}

$content = Get-Content $file | Out-String | ConvertFrom-Json
$releases = $content.'releases-index'
foreach ($release in $releases) {
    $version = $release.'channel-version'
    $link = $release.'releases.json'
    $releaseFile = "./JexusManager/Resources/$version-release.json"
    Write-Host "Download" $version
    Invoke-WebRequest $link -OutFile $releaseFile
}

Write-Host "Finished."
