// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using JexusManager.Properties;
using Microsoft.Web.Management.Client;
using Microsoft.Web.Management.Client.Win32;
using System.Collections;
using System.Reflection;

namespace JexusManager.Features
{
    public abstract class ShowHelpTaskList : DefaultTaskList
    {
        public override ICollection GetTaskItems()
        {
            return new TaskItem[]
            {
                HelpTaskItem
            };
        }

        [Obfuscation(Exclude = true)]
        public abstract void ShowHelp();
    }
}
