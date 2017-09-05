// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Reflection;
using System.Runtime.InteropServices;

namespace Microsoft.Web.Administration
{
    [Obfuscation(Exclude = true, ApplyToMembers = false)]
    internal class VirtualDirectoryPathValidator : ConfigurationValidatorBase
    {
        public override void Validate(object value)
        {
            var data = (string)value;
            if (string.IsNullOrWhiteSpace(data))
            {
                throw new COMException("Invalid virtual directory path\r\n");
            }

            if (data[0] != '/')
            {
                throw new COMException("Invalid virtual directory path\r\n");
            }

            if (data.Length > 1 && data[data.Length - 1] == '/')
            {
                throw new COMException("Invalid virtual directory path\r\n");
            }

            var items = data.Split('/');
            foreach (var item in items)
            {
                if (item == "." || item == "..")
                {
                    throw new COMException("Invalid virtual directory path\r\n");
                }
            }
        }
    }
}
