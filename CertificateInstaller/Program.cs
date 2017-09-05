// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.ComponentModel;
using System.Diagnostics;

namespace CertificateInstaller
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Security.Cryptography.X509Certificates;
    using Microsoft.Web.Administration;
    using Mono.Options;
    using System.IO;

    internal class Program
    {
        private static int Main(string[] args)
        {
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

            OptionSet p =
                new OptionSet().Add("f:", "File name", delegate (string v) { if (v != null) p12File = v; })
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
                    .Add("config:", "Config file path", delegate(string v)
                    {
                        if (v != null) config = v;
                    })
                    .Add("siteId:", "Site ID", delegate(string v)
                    {
                        if (v != null) siteId = v;
                    })
                    .Add("resultFile:", "Result File", delegate(string v)
                    {
                        if (v != null) resultFile = v;
                    })
                    .Add("launcher:", "IIS Express path", delegate (string v)
                    {
                        if (v != null)
                            launcher = v;
                    })
                    .Add("k", "Kill Process", delegate(string v)
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
                return -1;
            }

            List<string> extra;
            try
            {
                extra = p.Parse(args);
            }
            catch (OptionException)
            {
                return -1;
            }

            if (extra.Count > 0)
            {
                ShowHelp(p);
                return -1;
            }

            try
            {
                if (config != null)
                {
                    if (resultFile != null)
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
                        var process = new Process();
                        process.StartInfo.FileName = launcher;
                        process.StartInfo.Arguments = $"/config:\"{config}\" /siteid:{siteId} /systray:false /trace:error";
                        process.StartInfo.CreateNoWindow = true;
                        process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                        process.StartInfo.RedirectStandardOutput = true;
                        process.StartInfo.UseShellExecute = false;
                        process.Start();
                        process.WaitForExit(5000);
                        if (process.HasExited)
                        {
                            File.WriteAllText(resultFile, process.StandardOutput.ReadToEnd());
                            return 1;
                        }

                        return 0;
                    }

                    if (kill)
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
                    else
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
                }

                if (url != null)
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

                if (store == null)
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

                var personal = new X509Store(store, StoreLocation.LocalMachine);
                try
                {
                    personal.Open(OpenFlags.ReadWrite);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    return -1;
                }

                if (hash == null)
                {
                    // add certificate
                    // http://paulstovell.com/blog/x509certificate2
                    var x509 = new X509Certificate2(
                        p12File,
                        p12Pwd,
                        X509KeyStorageFlags.Exportable | X509KeyStorageFlags.PersistKeySet
                        | X509KeyStorageFlags.MachineKeySet) {FriendlyName = friendlyName};
                    personal.Add(x509);
                    personal.Close();
                    return 0;
                }

                var selectedItem = personal.Certificates.Find(X509FindType.FindByThumbprint, hash, false);
                if (selectedItem.Count > 0)
                {
                    if (address == null)
                    {
                        if (host == null)
                        {
                            // remove certificate and mapping
                            var mappings = NativeMethods.QuerySslCertificateInfo();
                            foreach (var mapping in mappings)
                            {
                                if (mapping.Hash.SequenceEqual(selectedItem[0].GetCertHash()))
                                {
                                    NativeMethods.DeleteCertificateBinding(mapping.IpPort);
                                }
                            }

                            personal.Remove(selectedItem[0]);
                        }
                        else
                        {
                            var mappings = NativeMethods.QuerySslSniInfo();
                            foreach (var mapping in mappings)
                            {
                                if (mapping.Hash.SequenceEqual(selectedItem[0].GetCertHash()))
                                {
                                    NativeMethods.DeleteSniBinding(new Tuple<string, int>(mapping.Host, mapping.Port));
                                }
                            }
                        }

                        personal.Close();
                        return 0;
                    }

                    if (host == null)
                    {
                        // register mapping
                        var endpoint = new IPEndPoint(IPAddress.Parse(address), int.Parse(port));
                        NativeMethods.BindCertificate(endpoint, selectedItem[0].GetCertHash(), store, Guid.Parse(id));
                    }
                    else
                    {
                        NativeMethods.BindSni(new Tuple<string, int>(host, int.Parse(port)), selectedItem[0].GetCertHash(), store, Guid.Parse(id));
                    }

                    personal.Close();
                    return 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return -1;
            }

            Console.WriteLine("Not supported path");
            return -1;
        }

        private static void ShowHelp(OptionSet optionSet)
        {
            Console.WriteLine("Jexus Manager is available at https://www.jexusmanager.com");
            Console.WriteLine("CertificateInstaller.exe [Options]");
            Console.WriteLine("Options:");
            optionSet.WriteOptionDescriptions(Console.Out);
        }
    }
}
