[CmdletBinding()]
param(
    [Parameter(Position = 0)]
    [string] $Configuration = 'Release'
)

Import-Module PowerShellGet

$msBuild = "msbuild"
try
{
    & $msBuild /version
    Write-Host "Likely on Linux/macOS."
}
catch
{
    Write-Host "MSBuild doesn't exist. Use VSSetup instead."

    Install-Module VSSetup -Scope CurrentUser -Force
    $instance = Get-VSSetupInstance -All -Prerelease | Select-VSSetupInstance -Product * -Latest
    $installDir = $instance.installationPath
    $msBuild = $installDir + '\MSBuild\Current\Bin\MSBuild.exe'
    if (![System.IO.File]::Exists($msBuild))
    {
        Write-Host "MSBuild 16 doesn't exist."
        $msBuild = $installDir + '\MSBuild\15.0\Bin\MSBuild.exe'
        if (![System.IO.File]::Exists($msBuild))
        {
            Write-Host "MSBuild 15 doesn't exist. Exit."
            exit 1
        }
        else
        {
            Write-Host "Likely on Windows with VS2017."
        }
    }
    else
    {
        Write-Host "Likely on Windows with VS2019."
    }
}

Write-Host "MSBuild found. Compile the projects."

& $msBuild JexusManager.sln /p:Configuration=$Configuration /t:restore
& $msBuild JexusManager.sln /p:Configuration=$Configuration /t:clean

Remove-Item .\bin -Recurse
New-Item .\bin -ItemType Directory > $null
Set-Location .\JexusManager
dotnet publish -c $Configuration -r win-x64 --self-contained -o ..\bin\x64
dotnet publish -c $Configuration -r win-x86 --self-contained -o ..\bin\x86
dotnet publish -c $Configuration -r win-arm64 --self-contained -o ..\bin\arm64
Copy-Item .\ThirdPartyNotices.txt ..\bin
Set-Location ..

Set-Location .\CertificateInstaller
dotnet publish -c $Configuration -r win-x64 --self-contained -o ..\bin\x64
dotnet publish -c $Configuration -r win-x86 --self-contained -o ..\bin\x86
dotnet publish -c $Configuration -r win-arm64 --self-contained -o ..\bin\arm64
dotnet publish -c $Configuration -r win-x64 --self-contained -p:PublishSingleFile=true -p:PublishTrimmed=true -o ..\bin\temp
Copy-Item ..\bin\temp\CertificateInstaller.exe ..\bin\arm64\CertificateInstaller.x64.exe
Remove-Item ..\bin\temp -Recurse
Set-Location ..

.\lib\Paraffin.exe -regExExclude "JexusManager\.exe" -NoRootDirectory -dir .\bin\x64 -GroupName Files64 .\Setup\Files64.wxs
.\lib\Paraffin.exe -regExExclude "JexusManager\.exe" -NoRootDirectory -dir .\bin\x86 -GroupName Files86 .\Setup\Files86.wxs
.\lib\Paraffin.exe -regExExclude "JexusManager\.exe" -NoRootDirectory -dir .\bin\arm64 -GroupName FilesARM64 .\Setup\FilesARM64.wxs

Write-Host "Compilation finished."
