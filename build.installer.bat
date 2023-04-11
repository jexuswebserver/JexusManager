del bin\Release\en-us\JexusManager.msi
%WINDIR%\Microsoft.NET\Framework\v3.5\msbuild .\Setup\JexusManager.wixproj /t:Rebuild /property:Configuration=Release /property:Platform=x86
copy bin\Release\en-us\JexusManager.msi .\JexusManager_x86.msi
del bin\Release\en-us\JexusManager.msi
%WINDIR%\Microsoft.NET\Framework\v3.5\msbuild .\Setup\JexusManager.wixproj /t:Rebuild /property:Configuration=Release /property:Platform=x64
copy bin\Release\en-us\JexusManager.msi .\JexusManager_x64.msi
del bin\Release\en-us\JexusManager.msi
%WINDIR%\Microsoft.NET\Framework\v3.5\msbuild .\Setup\JexusManager.wixproj /t:Rebuild /property:Configuration=Release /property:Platform=ARM64
copy bin\Release\en-us\JexusManager.msi .\JexusManager_arm64.msi
