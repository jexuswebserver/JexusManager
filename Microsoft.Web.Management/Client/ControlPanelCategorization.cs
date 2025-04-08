// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Web.Management.Client
{
    public sealed class ControlPanelCategorization
    {
        public ControlPanelCategorization(
            string key,
            string displayName
            )
        {
            Key = key;
            DisplayName = displayName;
        }

        public static readonly string AreaCategorization;
        public static readonly string CategoryCategorization;

        public override bool Equals(
            Object obj
            )
        {
            return false;
        }

        public override int GetHashCode()
        {
            return 0;
        }

        public string DisplayName { get; }
        public string Key { get; }
    }
}
