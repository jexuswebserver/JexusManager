// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Drawing;

namespace Microsoft.Web.Management.Client.Win32
{
    public sealed class MethodTaskItem : TaskItem
    {
        public MethodTaskItem(string methodName, string text, string category)
            : this(methodName, text, category, string.Empty)
        { }

        public MethodTaskItem(string methodName, string text, string category, string description)
            : this(methodName, text, category, description, null)
        { }

        public MethodTaskItem(string methodName, string text, string category, string description, Image image)
            : this(methodName, text, category, description, image, null)
        { }

        public MethodTaskItem(string methodName, string text, string category, string description, Image image, object userData)
            : base(text, category, description)
        {
            MethodName = methodName;
            Image = image;
            UserData = userData;
            Usage = MethodTaskItemUsages.TaskList;
        }

        public bool CausesNavigation { get; set; }
        public Image Image { get; }
        public string MethodName { get; }
        public MethodTaskItemUsages Usage { get; set; }
        public object UserData { get; }

        public static MethodTaskItem CreateSeparator()
        {
            return new MethodTaskItem(string.Empty, "-", string.Empty);
        }
    }
}
