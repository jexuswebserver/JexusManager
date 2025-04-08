// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Drawing;

namespace Microsoft.Web.Management.Client.Win32
{
    public sealed class TextTaskItem : TaskItem
    {
        public TextTaskItem(string text, string category)
            : this(text, category, false)
        { }

        public TextTaskItem(string text, string category, bool isHeading)
            : this(text, category, isHeading, null)
        { }

        public TextTaskItem(string text, string category, bool isHeading, Image image)
            : base(text, category)
        {
            IsHeading = isHeading;
            Image = image;
        }

        public Image Image { get; }
        public bool IsHeading { get; }
    }
}
