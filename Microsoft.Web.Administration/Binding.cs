// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Net;
using System.Net.Sockets;

namespace Microsoft.Web.Administration
{
    public sealed class Binding : ConfigurationElement
    {
        private IPEndPoint _endPoint;
        private string _host;

        private bool _initialized;

        public Binding(ConfigurationElement element, BindingCollection parent)
            : base(element, null, null, parent, null, null)
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
            SslFlags = flags;
            Parent = parent;
        }

        public override string ToString()
        {
            return $"{EndPoint.Address.AddressToDisplay()}:{EndPoint.Port}:{Host.HostToDisplay()}";
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
            if (last > -1)
            {
                _host = value.Substring(last + 1);
                var next = value.LastIndexOf(':', last - 1);
                var port = value.Substring(next + 1, last - next - 1);
                if (next > -1)
                {
                    var address = value.Substring(0, next);
                    _endPoint = new IPEndPoint(address.DisplayToAddress(), Int32.Parse(port));
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

            if (this.GetIsSni())
            {
                var sni = NativeMethods.QuerySslSniInfo(new Tuple<string, int>(_host, _endPoint.Port));
                if (sni != null)
                {
                    CertificateHash = sni.Hash;
                    CertificateStoreName = sni.StoreName;
                    return;
                }
            }

            var certificate = NativeMethods.QuerySslCertificateInfo(_endPoint);
            if (certificate != null)
            {
                CertificateHash = certificate.Hash;
                CertificateStoreName = certificate.StoreName;
            }
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
                return (SslFlags)Enum.ToObject(typeof(SslFlags), this["sslFlags"]);
            }

            set
            {
                this["sslFlags"] = (uint)value;
                _initialized = false;
            }
        }

        public bool UseDsMapper { get; set; }

        internal BindingCollection Parent { get; }

        internal string ToUri()
        {
            var address = EndPoint.Address.Equals(IPAddress.Any)
                ? Parent.Parent.Parent.Parent.HostName.ExtractName()
                : EndPoint.AddressFamily == AddressFamily.InterNetwork
                    ? EndPoint.Address.ToString()
                    : $"[{EndPoint.Address}]";
            return IsDefaultPort
                ? $"{Protocol}://{address}"
                : $"{Protocol}://{address}:{EndPoint.Port}";
        }

        internal string ToIisUrl()
        {
            return ToUri() + "/";
        }

        private bool IsDefaultPort
        {
            get
            {
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
    }
}
