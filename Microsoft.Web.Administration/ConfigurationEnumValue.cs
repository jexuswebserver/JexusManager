// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Web.Administration
{
    public sealed class ConfigurationEnumValue
    {
        internal ConfigurationEnumValue()
        { }

        public string Name { get; internal set; }
        public long Value { get; internal set; }
    }
}
