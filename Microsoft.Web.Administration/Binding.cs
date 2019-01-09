// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.ComponentModel;
using System.Net;
using System.Net.Sockets;
using System.Windows.Forms;

namespace Microsoft.Web.Administration
{
    public sealed class Binding : ConfigurationElement
    {
        private IPEndPoint _endPoint;
        private string _host;

        private bool _initialized;

        public Binding(ConfigurationElement element, BindingCollection parent)
            : base(element, "binding", null, parent, null, null)
        {
            Parent = parent;
        }

        internal Binding(string protocol, string bindingInformation, byte[] hash, string store, SslFlags flags, BindingCollection parent)
            : base(null, "binding", null, parent, null, null)
        {
            BindingInformation = bindingInformation;
            Protocol = protocol;
            CertificateHash = hash;
            CertificateStoreName = store;
            if (parent.Parent.Server.SupportsSni)
            {
                SslFlags = flags;
            }

            Parent = parent;
        }

        public override string ToString()
        {
            return EndPoint == null ? BindingInformation : $"{EndPoint.Address.AddressToDisplay()}:{EndPoint.Port}:{Host.HostToDisplay()}";
        }

        public string BindingInformation
        {
            get
            {
                Initialize();
                return (string)this["bindingInformation"];
            }

            set
            {
                this["bindingInformation"] = value;
                _initialized = false;
            }
        }

        private void Initialize()
        {
            if (_initialized)
            {
                return;
            }

            _initialized = true;
            if (!CanBrowse)
            {
                _host = string.Empty;
                _endPoint = null;
                return;
            }

            var value = (string)this["bindingInformation"];
            var last = value.LastIndexOf(':');
            if (last > 0)
            {
                _host = value.Substring(last + 1);
                var next = value.LastIndexOf(':', last - 1);
                var port = value.Substring(next + 1, last - next - 1);
                if (next > -1)
                {
                    var address = value.Substring(0, next);
                    if (PortIsValid(port, out int number, null, false))
                    {
                        try
                        {
                            _endPoint = new IPEndPoint(address.DisplayToAddress(), number);
                        }
                        catch (FormatException)
                        {
                        }
                    }
                }
            }

            if (Protocol != "https" || CertificateHash != null)
            {
                return;
            }

            if (Helper.IsRunningOnMono())
            {
                // TODO: how to do it on Mono?
                return;
            }

            RefreshCertificate();
        }

        public byte[] CertificateHash { get; set; }
        public string CertificateStoreName { get; set; }

        public IPEndPoint EndPoint
        {
            get
            {
                Initialize();
                return _endPoint;
            }
        }

        public string Host
        {
            get
            {
                Initialize();
                return _host;
            }
        }

        // ReSharper disable once InconsistentNaming
        public bool IsIPPortHostBinding { get; internal set; }

        public string Protocol
        {
            get
            {
                Initialize();
                return (string)this["protocol"];
            }

            set
            {
                this["protocol"] = value;
                _initialized = false;
            }
        }

        public SslFlags SslFlags
        {
            get
            {
                Initialize();
                if (ContainsAttribute("sslFlags"))
                {
                    return (SslFlags)Enum.ToObject(typeof(SslFlags), this["sslFlags"]);
                }

                return SslFlags.None;
            }

            set
            {
                if (ContainsAttribute("sslFlags"))
                {
                    this["sslFlags"] = (uint)value;
                    _initialized = false;
                }
            }
        }

        public bool UseDsMapper { get; set; }

        internal BindingCollection Parent { get; }

        internal string ToUri()
        {
            var value = BindingInformation;
            var last = value.LastIndexOf(':');
            string host = null;
            string address = null;
            string port = null;
            if (last > 0)
            {
                host = value.Substring(last + 1);
                var next = value.LastIndexOf(':', last - 1);
                port = value.Substring(next + 1, last - next - 1);
                if (next > -1)
                {
                    address = value.Substring(0, next);
                }
            }

            string domain;
            if (EndPoint == null)
            {
                if (string.IsNullOrWhiteSpace(port))
                {
                    domain = "localhost";
                }
                else if (PortIsValid(port, out int validPort, null, false))
                {
                    domain = $"localhost:{validPort}";
                }
                else
                {
                    throw new ArgumentException("Value does not fall within the expected range.");
                }

                return $"{Protocol}://{domain}";
            }

            domain = address == "0.0.0.0"
                ? Parent.Parent.Parent.Parent.HostName.ExtractName()
                : string.IsNullOrWhiteSpace(host)
                    ? EndPoint.AddressFamily == AddressFamily.InterNetwork
                        ? address
                        : $"[{address}]"
                    : Host;
            return IsDefaultPort
                ? $"{Protocol}://{domain}"
                : $"{Protocol}://{domain}:{EndPoint.Port}";
        }

        internal string ToIisUrl()
        {
            return ToUri() + "/";
        }

        private bool IsDefaultPort
        {
            get
            {
                if (EndPoint == null)
                {
                    return true;
                }

                if (Protocol == "http")
                {
                    return EndPoint.Port == 80;
                }

                if (Protocol == "https")
                {
                    return EndPoint.Port == 443;
                }

                return false;
            }
        }

        internal bool CanBrowse => Protocol == "http" || Protocol == "https";

        internal bool DetectConflicts()
        {
            if (SslFlags == SslFlags.Sni)
            {
                var sni = NativeMethods.QuerySslSniInfo(new Tuple<string, int>(_host, _endPoint.Port));
                return sni != null; // true if detect existing SNI mapping.
            }

            var certificate = NativeMethods.QuerySslCertificateInfo(_endPoint);
            return certificate != null; // true if detect existing IP mapping.
        }

        internal bool GetIsSni()
        {
            if (!ContainsAttribute("sslFlags"))
            {
                return false;
            }

            var value = this["sslFlags"];
            return ((uint)value & 1U) == 1U;
        }

        public void RefreshCertificate()
        {
            if (Parent.Parent.Server.SupportsSni)
            {
                if (GetIsSni())
                {
                    try
                    {
                        var sni = NativeMethods.QuerySslSniInfo(new Tuple<string, int>(_host, _endPoint.Port));
                        if (sni == null)
                        {
                            CertificateHash = null;
                            CertificateStoreName = string.Empty;
                            SslFlags = SslFlags.Sni;
                            return;
                        }
                        else
                        {
                            CertificateHash = sni.Hash;
                            CertificateStoreName = sni.StoreName;
                            SslFlags = SslFlags.Sni;
                            return;
                        }
                    }
                    catch (Win32Exception)
                    {
                        CertificateHash = null;
                        CertificateStoreName = string.Empty;
                        SslFlags = SslFlags.Sni;
                        return;
                    }
                }
            }

            if (_endPoint == null)
            {
                CertificateHash = null;
                CertificateStoreName = string.Empty;
                return;
            }

            var certificate = NativeMethods.QuerySslCertificateInfo(_endPoint);
            if (certificate == null)
            {
                CertificateHash = null;
                CertificateStoreName = string.Empty;
                return;
            }
            
            CertificateHash = certificate.Hash;
            CertificateStoreName = certificate.StoreName;
        }

        internal static bool PortIsValid(string portText, out int port, string text, bool showDialog = true)
        {
            const string theServerPortNumberMustBeAPositiveIntegerBetweenAnd = "The server port number must be a positive integer between 1 and 65535";
            try
            {
                port = int.Parse(portText);
            }
            catch (Exception)
            {
                if (showDialog)
                {
                    MessageBox.Show(theServerPortNumberMustBeAPositiveIntegerBetweenAnd, text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }

                port = 0;
                return false;
            }

            if (port < 1 || port > 65535)
            {
                if (showDialog)
                {
                    MessageBox.Show(theServerPortNumberMustBeAPositiveIntegerBetweenAnd, text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }

                return false;
            }

            return true;
        }
    }
}
