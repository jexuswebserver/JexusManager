// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;

namespace Microsoft.Web.Administration
{
    internal static class Helper
    {
        internal static bool IsRunningOnMono()
        {
            return Type.GetType("Mono.Runtime") != null;
        }

        internal static bool GetIsSni(this Binding binding)
        {
            var value = binding["sslFlags"];
            return ((uint)value & 1U) == 1U;
        }

        internal static void GetAllDefinitions(this SectionGroup group, IList<SectionDefinition> result)
        {
            foreach (SectionDefinition item in group.Sections)
            {
                result.Add(item);
            }

            foreach (SectionGroup child in group.SectionGroups)
            {
                child.GetAllDefinitions(result);
            }
        }
    }
}
