// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Microsoft.Web.Administration
{
    [Obfuscation(Exclude = true, ApplyToMembers = false)]
    internal class SiteNameValidator : ConfigurationValidatorBase
    {
        public override void Validate(object value)
        {
            var data = (string)value;
            if (string.IsNullOrWhiteSpace(data))
            {
                throw new COMException("Invalid site name\r\n");
            }

            foreach (var ch in SiteCollection.InvalidSiteNameCharacters())
            {
                if (data.Contains(ch))
                {
                    throw new COMException("Invalid site name\r\n");
                }
            }
        }
    }
}
