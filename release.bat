set EnableNuGetPackageRestore=true
set msBuildDir=C:\Program Files (x86)\MSBuild\14.0\Bin
call .nuget\nuget.exe restore jexusmanager.sln
call "%MSBuildDir%\msbuild" jexusmanager.sln /p:Configuration=Release
@IF %ERRORLEVEL% NEQ 0 PAUSE