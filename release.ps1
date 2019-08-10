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
    $instance = Get-VSSetupInstance -All | Select-VSSetupInstance -Latest
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

& $msBuild JexusManager.sln /p:Configuration=Release /t:restore
& $msBuild JexusManager.sln /p:Configuration=Release /t:clean
& $msBuild JexusManager.sln /p:Configuration=Release

Set-Location .\JexusManager
dotnet publish -c Release -r win-x64 /p:PublishSignelFile=true -o ..\bin\x64 /p:PublishSingleFile=true
dotnet publish -c Release -r win-x86 /p:PublishSignelFile=true -o ..\bin\x86 /p:PublishSingleFile=true
Copy-Item .\ThirdPartyNotices.txt ..\bin
Set-Location ..

Set-Location .\CertificateInstaller
dotnet publish -c Release -r win-x64 /p:PublishSignelFile=true -o ..\bin\x64 /p:PublishSingleFile=true
dotnet publish -c Release -r win-x86 /p:PublishSignelFile=true -o ..\bin\x86 /p:PublishSingleFile=true
Set-Location ..

Write-Host "Compilation finished."
