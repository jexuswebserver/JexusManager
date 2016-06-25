// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Features.FastCgi
{
    using System.ComponentModel;

    internal class EnvironmentVariables
    {
        [Category("Misc")]
        [DisplayName("Name")]
        [DefaultValue("Name")]
        public string Name { get; set; }

        [Category("Misc")]
        [DisplayName("Value")]
        public string Value { get; set; }
    }
}
