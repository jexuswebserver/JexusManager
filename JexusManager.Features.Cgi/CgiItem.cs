// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Features.Cgi
{
    using System;
    using System.ComponentModel;

    using Microsoft.Web.Administration;

    internal class CgiItem
    {
        public ConfigurationElement Element { get; set; }

        public CgiItem(ConfigurationElement element)
        {
            this.Element = element;
            CreateCgiWithNewConsole = (bool)element["createCGIWithNewConsole"];
            CreateProcessAsUser = (bool)element["createProcessAsUser"];
            Timeout = (TimeSpan)element["timeout"];
        }

        public void Apply()
        {
            Element["createCGIWithNewConsole"] = CreateCgiWithNewConsole;
            Element["createProcessAsUser"] = CreateProcessAsUser;
            Element["timeout"] = Timeout;
        }

        [Browsable(true)]
        [Category("Behavior")]
        [Description("Indicates whether a CGI application runs in its own console. If the value is set to true, each CGI application creates a new console when started. A value of false indicaes that CGI applications should run without a console.")]
        [DisplayName("Use New Console For Each Invocation")]
        [DefaultValue(false)]
        public bool CreateCgiWithNewConsole { get; set; }

        [Browsable(true)]
        [Category("Security")]
        [Description("Specifies whether a CGI process is created in the system context or in the context of the requesting user.")]
        [DisplayName("Impersonate User")]
        [DefaultValue(true)]
        public bool CreateProcessAsUser { get; set; }

        [Browsable(true)]
        [Category("Behavior")]
        [Description("Specifies the time-out value for CGI applications.")]
        [DisplayName("Timeout (hh:mm:ss)")]
        [DefaultValue(typeof(TimeSpan), "0:15:00")]
        public TimeSpan Timeout { get; set; }
    }
}