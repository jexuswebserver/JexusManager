// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using JexusManager.Properties;
using Microsoft.Web.Management.Client;
using Microsoft.Web.Management.Client.Win32;
using System.Drawing;
using System.Reflection;

namespace JexusManager.Features
{
    public abstract class DefaultTaskList : TaskList
    {
        public static Image BasicSettingsImage
        {
            get { return Resources.basic_settings_16; }
        }

        public static Image RemoveImage
        {
            // TODO: remove this property some day.
            get { return Resources.remove_16; }
        }

        public static Image ShowAllImage
        {
            get { return Resources.show_all_16; }
        }

        public static Image GoImage
        {
            get { return Resources.go_16; }
        }

        public static Image ViewImage
        {
            get { return Resources.view_16; }
        }

        public MethodTaskItem RemoveTaskItem
        {
            get
            {
                return GetRemoveTaskItem("Remove");
            }
        }

        public MethodTaskItem HelpTaskItem
        {
            get
            {
                return new MethodTaskItem("ShowHelp", "Help", string.Empty, string.Empty, Resources.help_16).SetUsage();
            }
        }

        public MethodTaskItem GetBackTaskItem(string methodName, string text)
        {
            return new MethodTaskItem(methodName, text, string.Empty, string.Empty, Resources.back_16).SetUsage();
        }

        public MethodTaskItem GetMoveUpTaskItem(bool enabled)
        {
            return GetMoveUpTaskItem("MoveUp", enabled);
        }

        public MethodTaskItem GetMoveDownTaskItem(bool enabled)
        {
            return GetMoveDownTaskItem("MoveDown", enabled);
        }

        public MethodTaskItem GetRemoveTaskItem(string methodName)
        {
            return new MethodTaskItem(methodName, "Remove", string.Empty, string.Empty, Resources.remove_16).SetUsage();
        }

        public MethodTaskItem GetMoveUpTaskItem(string methodName, bool enabled)
        {
            return new MethodTaskItem(methodName, "Move Up", string.Empty, string.Empty,
                    Resources.move_up_16).SetUsage(enabled);
        }

        public MethodTaskItem GetMoveDownTaskItem(string methodName, bool enabled)
        {
            return new MethodTaskItem(methodName, "Move Down", string.Empty, string.Empty,
                        Resources.move_down_16).SetUsage(enabled);
        }

        [Obfuscation(Exclude = true)]
        public virtual void Remove()
        {
        }

        [Obfuscation(Exclude = true)]
        public virtual void MoveUp()
        {
        }

        [Obfuscation(Exclude = true)]
        public virtual void MoveDown()
        {
        }
    }
}
