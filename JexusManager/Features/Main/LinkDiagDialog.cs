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

    public partial class LinkDiagDialog : DialogForm
    {
        public LinkDiagDialog(IServiceProvider provider, Site site)
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
                        Debug($"System Time: {DateTime.Now}");
                        Debug($"Processor Architecture: {Environment.GetEnvironmentVariable("PROCESSOR_ARCHITECTURE")}");
                        Debug($"OS: {Environment.OSVersion}");
                        Debug($"{site.Server.Type}");
                        Debug("-----");

                        Debug($"[W3SVC/{site.Id}]");
                        Debug($"ServerComment  : {site.Name}");
                        Debug($"ServerAutoStart: {site.ServerAutoStart}");
                        Debug($"ServerState    : {site.State}");
                        Debug(string.Empty);
                        foreach (Binding binding in site.Bindings)
                        {
                            Info($"BINDING: {binding.Protocol} {binding}");
                            if (binding.Protocol == "https" || binding.Protocol == "http")
                            {
                                Debug($"This site can be accessed if,");
                                Debug($" * TCP port {binding.EndPoint.Port} must be opened on Windows Firewall (or any other equivalent products) so as to receive requests from other machines.");

                                if (binding.EndPoint.Address == IPAddress.Any)
                                {
                                    Debug($" * Requests from web browsers must be routed to following end points on this machine,");
                                    foreach (IPAddress address in Dns.GetHostEntry(string.Empty).AddressList.Where(address => !address.IsIPv6LinkLocal))
                                    {
                                        Debug($"   * {address}:{binding.EndPoint.Port}.");
                                    }

                                    Debug($"   * 127.0.0.1:{binding.EndPoint.Port}.");
                                }
                                else
                                {
                                    Debug($" * The networking must be properly set up to forward requests from web browsers to {binding.EndPoint} on this machine.");
                                }

                                if (binding.Host == "*" || binding.Host == string.Empty)
                                {
                                    if (binding.EndPoint.Address == IPAddress.Any)
                                    {
                                        Debug($" * Web browsers can use several URLs, such as");
                                        Debug($"   * {binding.Protocol}://localhost:{binding.EndPoint.Port}.");
                                        foreach (IPAddress address in Dns.GetHostEntry(string.Empty).AddressList.Where(address => !address.IsIPv6LinkLocal))
                                        {
                                            Debug($"   * {binding.Protocol}://{address}:{binding.EndPoint.Port}.");
                                        }
                                    }
                                    else
                                    {
                                        Debug($" * Web browsers should use URL {binding.Protocol}://{binding.EndPoint.Address}:{binding.EndPoint.Port}.");
                                    }
                                }
                                else
                                {
                                    Debug($" * Requests must have Host header {binding.Host}.");
                                    Debug($" * Web browsers should use URL {binding.Protocol}://{binding.Host}:{binding.EndPoint.Port}.");
                                }

                                if (binding.Protocol == "https")
                                {
                                    Warn($"Please run SSL Diagnostics to analyze SSL configuration at server level.");
                                }
                            }

                            Debug(string.Empty);
                        }
                    }
                    catch (CryptographicException ex)
                    {
                        Debug(ex.ToString());
                        RollbarDotNet.Rollbar.Report(ex, custom: new Dictionary<string, object>{ { "hResult", ex.HResult } });
                    }
                    catch (Exception ex)
                    {
                        Debug(ex.ToString());
                        RollbarDotNet.Rollbar.Report(ex);
                    }
                }));

           container.Add(
                Observable.FromEventPattern<EventArgs>(btnSave, "Click")
                .ObserveOn(System.Threading.SynchronizationContext.Current)
                .Subscribe(evt =>
                {
                    var fileName = DialogHelper.ShowSaveFileDialog(null, "Text Files|*.txt|All Files|*.*");
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
            DialogHelper.ProcessStart("http://www.jexusmanager.com/en/latest/tutorials/binding-diagnostics.html");
        }

        private void SslDiagDialogHelpButtonClicked(object sender, System.ComponentModel.CancelEventArgs e)
        {
            BtnHelpClick(null, EventArgs.Empty);
        }
    }
}
