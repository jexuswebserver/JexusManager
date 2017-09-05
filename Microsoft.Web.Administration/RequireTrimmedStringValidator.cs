// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Reflection;
using System.Runtime.InteropServices;

namespace Microsoft.Web.Administration
{
    [Obfuscation(Exclude = true, ApplyToMembers = false)]
    internal class RequireTrimmedStringValidator : ConfigurationValidatorBase
    {
        public override void Validate(object value)
        {
            var data = (string)value;
            if (data.StartsWith(" ", System.StringComparison.Ordinal) || data.EndsWith(" ", System.StringComparison.Ordinal))
            {
                throw new COMException("TODO");
            }
        }
    }
}
