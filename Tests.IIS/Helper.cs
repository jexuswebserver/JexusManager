// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.IO;
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
        
        public static readonly string FileNameMachineConfig = IsRunningOnMono()
            ? "/Library/Frameworks/Mono.framework/Versions/Current/etc/mono/4.5/machine.config"
            : Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows),
                "Microsoft.NET",
                IntPtr.Size == 2 ? "Framework" : "Framework64",
                "v4.0.30319",
                "CONFIG",
                "machine.config");

        public static readonly string FileNameWebConfig = IsRunningOnMono()
            ? "/Library/Frameworks/Mono.framework/Versions/Current/etc/mono/4.5/web.config"
            : Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.Windows),
                "Microsoft.NET",
                IntPtr.Size == 2 ? "Framework" : "Framework64",
                "v4.0.30319",
                "CONFIG",
                "web.config");
    }
}
