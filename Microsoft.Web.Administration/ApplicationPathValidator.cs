// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Reflection;
using System.Runtime.InteropServices;

namespace Microsoft.Web.Administration
{
    [Obfuscation(Exclude = true, ApplyToMembers = false)]
    internal class ApplicationPathValidator : ConfigurationValidatorBase
    {
        public override void Validate(object value)
        {
            var data = (string)value;
            string message = $"Invalid application path {data}\r\n";
            if (string.IsNullOrWhiteSpace(data))
            {
                throw new COMException(message);
            }

            if (data[0] != '/')
            {
                throw new COMException(message);
            }

            if (data.Length > 1 && data[data.Length - 1] == '/')
            {
                throw new COMException(message);
            }

            var items = data.Split('/');
            foreach (var item in items)
            {
                if (item == "." || item == "..")
                {
                    throw new COMException(message);
                }
            }
        }
    }
}
