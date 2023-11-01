del bin\Release\en-us\JexusManager.msi
%WINDIR%\Microsoft.NET\Framework\v4.0.30319\msbuild .\Setup\JexusManager.wixproj /t:Rebuild /property:Configuration=Release /property:Platform=arm64
copy bin\Release\en-us\JexusManager.msi .\JexusManager_arm64.msi
del bin\Release\en-us\JexusManager.msi
%WINDIR%\Microsoft.NET\Framework\v4.0.30319\msbuild .\Setup\JexusManager.wixproj /t:Rebuild /property:Configuration=Release /property:Platform=x64
copy bin\Release\en-us\JexusManager.msi .\JexusManager_x64.msi
