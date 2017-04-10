// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Reflection;
using System.Runtime.InteropServices;

namespace Microsoft.Web.Administration
{
    [Obfuscation(Exclude = true, ApplyToMembers = false)]
    internal class NonEmptyStringValidator : ConfigurationValidatorBase
    {
        public override void Validate(object value)
        {
            var data = (string)value;
            if (string.IsNullOrEmpty(data))
            {
                throw new COMException("String must not be empty\r\n");
            }
        }
    }
}