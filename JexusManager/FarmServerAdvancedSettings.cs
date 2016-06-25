// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager
{
    using System.ComponentModel;

    public class FarmServerAdvancedSettings
    {
        [Browsable(true)]
        [Category("Advanced Settings")]
        [DisplayName("httpPort")]
        [DefaultValue(80)]
        public int HttpPort { get; set; }

        [Browsable(true)]
        [Category("Advanced Settings")]
        [DisplayName("httpsPort")]
        [DefaultValue(443)]
        public int HttpsPort { get; set; }

        [Browsable(true)]
        [Category("Advanced Settings")]
        [DisplayName("weight")]
        [DefaultValue(100)]
        public int Weight { get; set; }

        [Browsable(false)]
        public string Name { get; set; }
    }
}
