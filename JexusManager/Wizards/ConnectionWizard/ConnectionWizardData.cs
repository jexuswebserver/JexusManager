// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Wizards.ConnectionWizard
{
    using System.Linq;

    using Microsoft.Web.Administration;

    internal class ConnectionWizardData
    {
        private readonly string[] _names;

        public ConnectionWizardData(string[] names)
        {
            _names = names;
        }

        public string Name;
        public string UserName;
        public string Password;
        public string HostName;
        public ServerManager Server { get; set; }
        public string CertificateHash { get; set; }
        public WorkingMode Mode { get; set; }
        public bool UseVisualStudio { get; set; }
        public string FileName { get; set; }

        public bool VerifyName(string text)
        {
            return !_names.Contains(text);
        }
    }
}
