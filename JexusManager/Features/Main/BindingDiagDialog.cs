// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Web.Administration;
using Microsoft.Web.Management.Client.Win32;
using System;
using System.Linq;
using System.Security.Cryptography;

namespace JexusManager.Features.Main
{
    using System.Drawing;
    using System.IO;
    using Microsoft.Win32;

    using System.Reactive.Disposables;
    using System.Reactive.Linq;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Sockets;
    using EnumsNET;
    using JexusManager.Features.HttpApi;
    using Microsoft.Web.Management.Client;

    public partial class BindingDiagDialog : DialogForm
    {
        public BindingDiagDialog(IServiceProvider provider, Site site)
            : base(provider)
        {
            InitializeComponent();

            var container = new CompositeDisposable();
            FormClosed += (sender, args) => container.Dispose();

            container.Add(
                Observable.FromEventPattern<EventArgs>(btnGenerate, "Click")
                .ObserveOn(System.Threading.SynchronizationContext.Current)
                .Subscribe(evt =>
                {
                    txtResult.Clear();
                    try
                    {
                        Warn("IMPORTANT: This report might contain confidential information. Mask such before sharing with others.");
                        Warn("-----");
                        Debug($"System Time: {DateTime.Now}");
                        Debug($"Processor Architecture: {Environment.GetEnvironmentVariable("PROCESSOR_ARCHITECTURE")}");
                        Debug($"OS: {Environment.OSVersion}");
                        Debug($"Server Type: {site.Server.Mode.AsString(EnumFormat.Description)}");
                        Debug("-----");

                        var adapters = Dns.GetHostEntry(string.Empty).AddressList.Where(address => !address.IsIPv6LinkLocal).ToList();
                        if (adapters.Count == 0)
                        {
                            Warn("This machine has no suitable IP address to accept external traffic.");
                        }
                        else
                        {
                            Info($"This machine has {adapters.Count} IP addresses to take external traffic.");
                            foreach (IPAddress address in adapters)
                            {
                                Info(address.AddressFamily == AddressFamily.InterNetworkV6
                                    ? $"* [{address}]."
                                    : $"* {address}.");
                            }
                        }

                        Debug("-----");

                        Debug($"[W3SVC/{site.Id}]");
                        Debug($"ServerComment  : {site.Name}");
                        Debug($"ServerAutoStart: {site.ServerAutoStart}");
                        Debug($"ServerState    : {site.State}");
                        Debug(string.Empty);
                        var feature = new ReservedUrlsFeature((Module)provider);
                        feature.Load();
                        foreach (Binding binding in site.Bindings)
                        {
                            Debug($"BINDING: {binding.Protocol} {binding}");
                            if (binding.Protocol != "https" && binding.Protocol != "http")
                            {
                                Warn("This prototol is not analyzed.");
                                Debug(string.Empty);
                                continue;
                            }

                            if (PublicNativeMethods.IsProcessElevated)
                            {
                                var found = Microsoft.Web.Administration.NativeMethods.FoundReserved((ushort)binding.EndPoint.Port);
                                if (found)
                                {
                                    Error("Found a conflicting TCP reserved port range. Please run \"netsh int ipv4 show excludedportrange protocol=tcp\" at command prompt to troubleshoot.");
                                }
                                else
                                {
                                    Info("No conflicting TCP reserved port range is found.");
                                }
                            }
                            else
                            {
                                Warn("Jexus Manager is not running as administrator, so TCP reserved port range is not verified. Please run \"netsh int ipv4 show excludedportrange protocol=tcp\" at command prompt to see if any conflict exists.");
                            }

                            if (binding.Host != "localhost")
                            {
                                if (site.Server.Mode == WorkingMode.IisExpress)
                                {
                                    var reservation = binding.ToUrlPrefix();
                                    if (!feature.Items.Any(item => item.UrlPrefix == reservation))
                                    {
                                        Warn($"URL reservation {reservation} is missing. So this binding only works if IIS Express runs as administrator.");
                                    }
                                }

                                Info($"This site can take external traffic if,");
                                Info($" * TCP port {binding.EndPoint.Port} must be opened on Windows Firewall (or any other installed firewall products).");
                            }

                            if (binding.EndPoint.Address.Equals(IPAddress.Any))
                            {
                                if (binding.Host != "localhost")
                                {
                                    Info($" * Requests from web browsers must be routed to following end points on this machine,");
                                    foreach (IPAddress address in adapters)
                                    {
                                        Info(address.AddressFamily == AddressFamily.InterNetworkV6
                                            ? $"   * [{address}]:{binding.EndPoint.Port}."
                                            : $"   * {address}:{binding.EndPoint.Port}.");
                                    }
                                }

                                if (Socket.OSSupportsIPv4)
                                {
                                    Debug($"This site can take local traffic at {IPAddress.Loopback}:{binding.EndPoint.Port}.");
                                }

                                if (Socket.OSSupportsIPv6)
                                {
                                    Debug($"This site can take local traffic at [{IPAddress.IPv6Loopback}]:{binding.EndPoint.Port}.");
                                }
                            }
                            else
                            {
                                Info($" * The networking must be properly set up to forward requests from web browsers to {binding.EndPoint} on this machine.");
                            }

                            if (binding.Host == "*" || binding.Host == string.Empty)
                            {
                                if (binding.EndPoint.Address.Equals(IPAddress.Any))
                                {
                                    Info($" * Web browsers can use several URLs, such as");
                                    foreach (IPAddress address in adapters)
                                    {
                                        Debug(address.AddressFamily == AddressFamily.InterNetworkV6
                                            ? $"   * {binding.Protocol}://[{address}]:{binding.EndPoint.Port}."
                                            : $"   * {binding.Protocol}://{address}:{binding.EndPoint.Port}.");
                                    }

                                    Info($"   * {binding.Protocol}://localhost:{binding.EndPoint.Port}.");
                                    Info($"   * {binding.Protocol}://{IPAddress.Loopback}:{binding.EndPoint.Port}.");
                                    Info($"   * {binding.Protocol}://[{IPAddress.IPv6Loopback}]:{binding.EndPoint.Port}.");
                                }
                                else
                                {
                                    Info($" * Web browsers should use URL {binding.Protocol}://{binding.EndPoint.Address}:{binding.EndPoint.Port}.");
                                }
                            }
                            else
                            {
                                Info($" * Web browsers should use URL {binding.Protocol}://{binding.Host}:{binding.EndPoint.Port}. Requests must have Host header of \"{binding.Host}\".");
                                if (!binding.Host.IsWildcard())
                                {
                                    Info($"   Start DNS query for {binding.Host}.");
                                    // IMPORTANT: wildcard host is not supported.
                                    try
                                    {
                                        var entry = Dns.GetHostEntry(binding.Host);
                                        var list = entry.AddressList;
                                        Info($"   DNS Query returns {list.Length} result(s).");
                                        var found = false;
                                        foreach (var address in list)
                                        {
                                            Info(address.AddressFamily == AddressFamily.InterNetworkV6
                                                ? $"    * [{address}]"
                                                : $"    * {address}");
                                            if (adapters.Any(item => address.Equals(item)))
                                            {
                                                found = true;
                                                break;
                                            }

                                            if (address.Equals(IPAddress.Loopback))
                                            {
                                                found = true;
                                            }
                                        }

                                        if (!found)
                                        {
                                            Warn($"   DNS query of \"{binding.Host}\" does not return a known IP address for any network adapter of this machine.");
                                            Warn("   The server usally uses private IP addresses, and DNS query returns public IP addresses.");
                                            Warn("   If packets are forwarded from public IP to private IP properly, this warning can be ignored.");
                                            Warn("   Otherwise, please review DNS settings (or modify the hosts file to emulate DNS).");
                                        }
                                    }
                                    catch (SocketException ex)
                                    {
                                        Error($"DNS query failed: {ex.Message}.");
                                        Error($"Please review the host name {binding.Host}.");
                                    }
                                }
                            }

                            if (binding.Protocol == "https")
                            {
                                Warn("Binding Diagnostics does not verify certificates and other SSL/TLS related settings.");
                                Warn($"Please run SSL Diagnostics at server level to analyze SSL/TLS configuration. More information can be found at https://docs.jexusmanager.com/tutorials/ssl-diagnostics.html.");
                            }

                            Debug(string.Empty);
                        }
                    }
                    catch (CryptographicException ex)
                    {
                        Debug(ex.ToString());
                        Rollbar.RollbarLocator.RollbarInstance.Error(ex, custom: new Dictionary<string, object>{ { "hResult", ex.HResult } });
                    }
                    catch (Exception ex)
                    {
                        Debug(ex.ToString());
                        Rollbar.RollbarLocator.RollbarInstance.Error(ex);
                    }
                }));

           container.Add(
                Observable.FromEventPattern<EventArgs>(btnSave, "Click")
                .ObserveOn(System.Threading.SynchronizationContext.Current)
                .Subscribe(evt =>
                {
                    var fileName = DialogHelper.ShowSaveFileDialog(null, "Text Files|*.txt|All Files|*.*", null);
                    if (string.IsNullOrEmpty(fileName))
                    {
                        return;
                    }
                    
                    File.WriteAllText(fileName, txtResult.Text);
                }));
        }

        private void Debug(string text)
        {
            txtResult.AppendText(text);
            txtResult.AppendText(Environment.NewLine);
        }

        private void Error(string text)
        {
            var defaultColor = txtResult.SelectionColor;
            txtResult.SelectionColor = Color.Red;
            txtResult.AppendText(text);
            txtResult.SelectionColor = defaultColor;
            txtResult.AppendText(Environment.NewLine);
        }

        private void Warn(string text)
        {
            var defaultColor = txtResult.SelectionColor;
            txtResult.SelectionColor = Color.Green;
            txtResult.AppendText(text);
            txtResult.SelectionColor = defaultColor;
            txtResult.AppendText(Environment.NewLine);
        }

        private void Info(string text)
        {
            var defaultColor = txtResult.SelectionColor;
            txtResult.SelectionColor = Color.Blue;
            txtResult.AppendText(text);
            txtResult.SelectionColor = defaultColor;
            txtResult.AppendText(Environment.NewLine);
        }

        private int GetEventLogging()
        {
            if (Helper.IsRunningOnMono())
            {
                return -1;
            }

            // https://support.microsoft.com/en-us/kb/260729
            var key = Registry.LocalMachine.OpenSubKey(@"System\CurrentControlSet\Control\SecurityProviders\SCHANNEL");
            if (key == null)
            {
                return 1;
            }

            var value = (int)key.GetValue("EventLogging", 1);
            return value;
        }

        private static bool GetProtocol(string protocol)
        {
            if (Helper.IsRunningOnMono())
            {
                return false;
            }

            // https://support.microsoft.com/en-us/kb/187498
            var key =
                Registry.LocalMachine.OpenSubKey(
                    $@"SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Protocols\{protocol}\Server");
            if (key == null)
            {
                return true;
            }

            var value = (int)key.GetValue("Enabled", 1);
            var enabled = value == 1;
            return enabled;
        }

        private void BtnHelpClick(object sender, EventArgs e)
        {
            DialogHelper.ProcessStart("https://docs.jexusmanager.com/tutorials/binding-diagnostics.html");
        }

        private void BindingDiagDialogHelpButtonClicked(object sender, System.ComponentModel.CancelEventArgs e)
        {
            BtnHelpClick(null, EventArgs.Empty);
        }
    }
}
