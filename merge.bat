mkdir bin
cd bin
echo merge JexusManager.exe
..\packages\ILRepack.2.0.10\tools\ILRepack.exe /keyfile:..\JexusManager\JexusManager.snk /t:winexe /out:JexusManager.exe JexusManager.exe JexusManager.Shared.dll JexusManager.Features.Access.dll JexusManager.Features.Authentication.dll JexusManager.Features.Authorization.dll JexusManager.Features.Caching.dll JexusManager.Features.Certificates.dll JexusManager.Features.Cgi.dll JexusManager.Features.Compression.dll JexusManager.Features.DefaultDocument.dll JexusManager.Features.DirectoryBrowse.dll JexusManager.Features.FastCgi.dll JexusManager.Features.Handlers.dll JexusManager.Features.HttpApi.dll JexusManager.Features.HttpErrors.dll JexusManager.Features.HttpRedirect.dll JexusManager.Features.IpSecurity.dll JexusManager.Features.IsapiCgiRestriction.dll JexusManager.Features.IsapiFilters.dll JexusManager.Features.Jexus.dll JexusManager.Features.Logging.dll JexusManager.Features.MimeMap.dll JexusManager.Features.Modules.dll JexusManager.Features.RequestFiltering.dll JexusManager.Features.ResponseHeaders.dll JexusManager.Features.Rewrite.dll Ookii.Dialogs.dll MakarovDev.ExpandCollapsePanel.dll CheckBoxComboBox.dll Microsoft.Web.Configuration.AppHostFileProvider.dll Microsoft.Web.Management.dll Microsoft.Web.Administration.dll Crad.Windows.Forms.Actions.dll  System.Net.Http.Formatting.dll BouncyCastle.Crypto.dll Mono.Security.dll Vista.Controls.BreadcrumbBar.dll Newtonsoft.Json.dll
echo System.Reactive.Core.dll System.Reactive.Interfaces.dll System.Reactive.Linq.dll System.Reactive.PlatformServices.dll
@IF %ERRORLEVEL% NEQ 0 PAUSE
del JexusManager.Shared.dll JexusManager.Features.*.dll Ookii.Dialogs.dll System.Net.Http.dll Microsoft.Web.*.dll Crad.Windows.Forms.Actions.dll System.Net.Http.Formatting.dll BouncyCastle.Crypto.dll Mono.Security.dll Vista.Controls.BreadcrumbBar.dll Newtonsoft.Json.dll CheckBoxComboBox.dll MakarovDev.ExpandCollapsePanel.dll
echo System.Reactive.*.dll

echo merge CertificateInstaller.exe
..\packages\ILRepack.2.0.10\tools\ILRepack.exe /keyfile:..\JexusManager\JexusManager.snk /t:exe /out:CertificateInstaller.exe CertificateInstaller.exe Mono.Options.dll
@IF %ERRORLEVEL% NEQ 0 PAUSE
del Mono.Options.dll

cd ..