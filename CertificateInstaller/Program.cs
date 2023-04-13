// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Web.Administration;
using Mono.Options;
using Serilog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;

var folder = Environment.ExpandEnvironmentVariables(@"%APPDATA%\JexusManager");
if (!Directory.Exists(folder))
{
    Directory.CreateDirectory(folder);
}

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .Enrich.FromLogContext()
    .Enrich.WithProcessId()
    .Enrich.WithProcessName()
    .WriteTo.File(
        Path.Combine(folder, "log.txt"),
        rollingInterval: RollingInterval.Day,
        retainedFileCountLimit: 5,
        shared: true)
    .CreateLogger();

Log.Information($"Process {System.Runtime.InteropServices.RuntimeInformation.ProcessArchitecture} started.");

string p12File = null;
string p12Pwd = null;
string friendlyName = null;
string store = null;
string hash = null;
string address = null;
string port = null;
string id = null;
string host = null;
string url = null;
string descriptor = null;
string config = null;
string siteId = null;
string launcher = null;
string resultFile = null;
bool kill = false;
bool restart = false;
string verb = null;
string input = null;

OptionSet p =
    new OptionSet()
        .Add("verb:", "Verb", delegate (string v) { if (v != null) verb = v; })
        .Add("input:", "Appcmd input string", delegate (string v) { if (v != null) input = v; })
        .Add("f:", "File name", delegate (string v) { if (v != null) p12File = v; })
        .Add("p:", "Password", delegate (string v) { if (v != null) p12Pwd = v; })
        .Add("n:", "Friendly name", delegate (string v) { if (v != null) friendlyName = v; })
        .Add("s:", "Store name", delegate (string v) { if (v != null) store = v; })
        .Add("h:", "Certificate hash (not required when adding certificates)", delegate (string v) { if (v != null) hash = v; })
        .Add("a:", "IP address", delegate (string v) { if (v != null) address = v; })
        .Add("o:", "Port number", delegate (string v) { if (v != null) port = v; })
        .Add("i:", "Application ID", delegate (string v) { if (v != null) id = v; })
        .Add("x:", "SNI host name (not required when managing IP based bindings)", delegate (string v) { if (v != null) host = v; })
        .Add("u:", "Reserved URL", delegate (string v) { if (v != null) url = v; })
        .Add("d:", "Security descriptor", delegate (string v) { if (v != null) descriptor = v; })
        .Add("config:", "Config file path", delegate (string v)
        {
            if (v != null) config = v;
        })
        .Add("siteId:", "Site ID", delegate (string v)
        {
            if (v != null) siteId = v;
        })
        .Add("resultFile:", "Result File", delegate (string v)
        {
            if (v != null) resultFile = v;
        })
        .Add("launcher:", "IIS Express path", delegate (string v)
        {
            if (v != null)
                launcher = v;
        })
        .Add("k", "Kill Process", delegate (string v)
        {
            if (v != null) kill = true;
        })
        .Add("r", "Restart Site", delegate (string v)
        {
            if (v != null) restart = true;
        });

if (args.Length == 0)
{
    ShowHelp(p);
    Log.Information("No arguments.");
    return -1;
}

List<string> extra;
try
{
    extra = p.Parse(args);
}
catch (OptionException ex)
{
    Log.Fatal(ex, "Option exception.");
    return -2;
}

if (extra.Count > 0)
{
    ShowHelp(p);
    Log.Fatal("Unknown arguments.", extra);
    return -3;
}

try
{
    if (verb == "appcmd")
    {
        if (input == null || launcher == null)
        {
            ShowHelp(p);
            Log.Fatal("Calling appcmd without arguments.");
            return -4;
        }

        return RunProcess(launcher, resultFile, input);
    }

    if (config != null)
    {
        return HandleSite(config, siteId, launcher, resultFile, kill, restart);
    }

    if (url != null)
    {
        return HandleReservedUrl(url, descriptor);
    }

    if (store == null)
    {
        return DeleteMapping(address, port, host);
    }

    using var personal = new X509Store(store, StoreLocation.LocalMachine);
    try
    {
        personal.Open(OpenFlags.ReadWrite);
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex);
        Log.Fatal(ex, "Certificate store open exception.");
        return -5;
    }

    if (hash == null)
    {
        return AddCertificate(p12File, p12Pwd, friendlyName, personal);
    }

    if (string.IsNullOrEmpty(hash))
    {
        if (address == null && host != null)
        {
            NativeMethods.DeleteSniBinding(new Tuple<string, int>(host, int.Parse(port)));
            return 0;
        }

        ShowHelp(p);
        Log.Fatal("Should never reach here");
        return -6;
    }

    var selectedItem = personal.Certificates.Find(X509FindType.FindByThumbprint, hash, false);
    if (selectedItem.Count > 0)
    {
        X509Certificate2 cert = selectedItem[0];
        if (address == null)
        {
            return RemoveCertificate(port, host, personal, cert);
        }
        else
        {
            return CreateMapping(store, address, port, id, host, cert);
        }
    }
}
catch (Exception ex)
{
    Console.WriteLine(ex);
    Log.Fatal(ex, "Unknown exception.");
    return -7;
}

Console.WriteLine("Not supported path");
Log.Fatal("Should never hit this");
return -8;

int AddCertificate(string p12File, string p12Pwd, string friendlyName, X509Store personal)
{
    // add certificate
    // http://paulstovell.com/blog/x509certificate2
    var x509 = new X509Certificate2(
        p12File,
        p12Pwd,
        X509KeyStorageFlags.Exportable | X509KeyStorageFlags.PersistKeySet
        | X509KeyStorageFlags.MachineKeySet)
    { FriendlyName = friendlyName };
    personal.Add(x509);
    return 0;
}

int HandleSite(string config, string siteId, string launcher, string resultFile, bool kill, bool restart)
{
    if (resultFile != null)
    {
        return StartSite(config, siteId, launcher, resultFile, restart);
    }

    if (kill)
    {
        return KillSite(config, siteId);
    }

    return QuerySite(config, siteId);
}

int RunProcess(string launcher, string resultFile, string input)
{
    var process = new Process
    {
        StartInfo =
                            {
                                FileName = launcher,
                                Arguments = input,
                                CreateNoWindow = true,
                                WindowStyle = ProcessWindowStyle.Hidden,
                                RedirectStandardError = true,
                                RedirectStandardOutput = true,
                                UseShellExecute = false
                            }
    };
    process.Start();
    process.WaitForExit();
    var error = process.StandardError.ReadToEnd();
    var output = process.StandardOutput.ReadToEnd();
    if (process.ExitCode != 0)
    {
        File.AppendAllText(resultFile, error);
        File.AppendAllText(resultFile, output);
    }

    return process.ExitCode;
}

int StartSite(string config, string siteId, string launcher, string resultFile, bool restart)
{
    if (restart)
    {
        var toKill = $"/config:\"{config}\" /siteid:{siteId} /systray:false /trace:error";
        var items = Process.GetProcessesByName("iisexpress");
        var found = items.Where(item =>
        {
            var command = item.GetCommandLine();
            return command != null && command.TrimEnd().EndsWith(toKill, StringComparison.Ordinal);
        });
        foreach (var item in found)
        {
            item.Kill();
            item.WaitForExit();
        }
    }

    // start a site.
    var process = new Process
    {
        StartInfo =
                            {
                                FileName = launcher,
                                Arguments = $"/config:\"{config}\" /siteid:{siteId} /systray:false /trace:error",
                                CreateNoWindow = true,
                                WindowStyle = ProcessWindowStyle.Hidden,
                                RedirectStandardOutput = true,
                                UseShellExecute = false
                            }
    };
    process.Start();
    process.WaitForExit(5000);
    if (process.HasExited)
    {
        File.WriteAllText(resultFile, process.StandardOutput.ReadToEnd());
        return 1;
    }

    return 0;
}

int QuerySite(string config, string siteId)
{
    var toQuery = $"/config:\"{config}\" /siteid:{siteId} /systray:false /trace:error";
    var items = Process.GetProcessesByName("iisexpress");
    var found = items.Any(item =>
    {
        var command = item.GetCommandLine();
        return command != null && command.TrimEnd().EndsWith(toQuery, StringComparison.Ordinal);
    });
    return found ? 1 : 0;
}

int KillSite(string config, string siteId)
{
    var toKill = $"/config:\"{config}\" /siteid:{siteId} /systray:false /trace:error";
    var items = Process.GetProcessesByName("iisexpress");
    var found = items.Where(item =>
    {
        var command = item.GetCommandLine();
        return command != null && command.TrimEnd().EndsWith(toKill, StringComparison.Ordinal);
    });
    foreach (var item in found)
    {
        item.Kill();
        item.WaitForExit();
    }

    return 0;
}

int DeleteMapping(string address, string port, string host)
{
    if (host == null)
    {
        NativeMethods.DeleteCertificateBinding(
            new IPEndPoint(IPAddress.Parse(address), int.Parse(port)));
    }
    else
    {
        NativeMethods.DeleteSniBinding(new Tuple<string, int>(host, int.Parse(port)));
    }

    return 0;
}

int HandleReservedUrl(string url, string descriptor)
{
    if (descriptor != null)
    {
        NativeMethods.DeleteHttpNamespaceAcl(url, descriptor);
    }
    else
    {
        NativeMethods.BindHttpNamespaceAcl(url, "D:(A;;GX;;;WD)");
    }

    return 0;
}

int CreateMapping(string store, string address, string port, string id, string host, X509Certificate2 selectedItem)
{
    if (host == null)
    {
        // register mapping
        var endpoint = new IPEndPoint(IPAddress.Parse(address), int.Parse(port));
        NativeMethods.BindCertificate(endpoint, selectedItem.GetCertHash(), store, Guid.Parse(id));
    }
    else
    {
        NativeMethods.BindSni(new Tuple<string, int>(host, int.Parse(port)), selectedItem.GetCertHash(), store, Guid.Parse(id));
    }

    return 0;
}

int RemoveCertificate(string port, string host, X509Store personal, X509Certificate2 cert)
{
    // remove IP based mapping
    var mappings = NativeMethods.QuerySslCertificateInfo();
    foreach (var mapping in mappings)
    {
        if (mapping.Hash.SequenceEqual(cert.GetCertHash()))
        {
            if (port != null && host == null)
            {
                NativeMethods.DeleteCertificateBinding(mapping.IpPort);
            }
        }
    }

    // remove SNI mapping.
    var mappings1 = NativeMethods.QuerySslSniInfo();
    foreach (var mapping in mappings1)
    {
        if (mapping.Hash.SequenceEqual(cert.GetCertHash()))
        {
            if (port != null && host != null)
            {
                NativeMethods.DeleteSniBinding(new Tuple<string, int>(mapping.Host, mapping.Port));
            }
        }
    }

    if (port == null)
    {
        // IMPORTANT: only delete the certificate if no port is binded.
        personal.Remove(cert);
    }

    personal.Close();
    return 0;
}

void ShowHelp(OptionSet optionSet)
{
    Console.WriteLine("Jexus Manager is available at https://www.jexusmanager.com");
    Console.WriteLine("CertificateInstaller.exe [Options]");
    Console.WriteLine("Options:");
    optionSet.WriteOptionDescriptions(Console.Out);
}
