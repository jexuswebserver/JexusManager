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
    $instance = Get-VSSetupInstance -All -Prerelease | Select-VSSetupInstance -Require 'Microsoft.Component.MSBuild' -Latest
    $installDir = $instance.installationPath
    Write-Host "Visual Studio is found at $installDir"
    $msBuild = $installDir + '\MSBuild\Current\Bin\MSBuild.exe' # VS2019
    if (![System.IO.File]::Exists($msBuild))
    {
        $msBuild = $installDir + '\MSBuild\15.0\Bin\MSBuild.exe' # VS2017
        if (![System.IO.File]::Exists($msBuild))
        {
            Write-Host "MSBuild doesn't exist. Exit."
            exit 1
        }
    }

    Write-Host "Likely on Windows."
}

Write-Host "MSBuild found. Compile the projects."

& $msBuild JexusManager.sln /p:Configuration=Release /t:restore
& $msBuild JexusManager.sln /p:Configuration=Release /t:clean
& $msBuild JexusManager.sln /p:Configuration=Release

Write-Host "Compilation finished."
