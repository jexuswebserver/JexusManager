// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Drawing;

namespace Microsoft.Web.Management.Client.Win32
{
    public abstract class ManagementUIColorTable

    {
        public abstract Color CommandBarBorder { get; }
        public abstract Color CommandBarButtonPressed { get; }
        public abstract Color CommandBarButtonSelected { get; }
        public abstract Color CommandBarDragHandle { get; }
        public abstract Color CommandBarDragHandleShadow { get; }
        public abstract Color CommandBarGradientBegin { get; }
        public abstract Color CommandBarGradientEnd { get; }
        public abstract Color CommandBarGradientMiddle { get; }
        public abstract Color CommandBarSeparatorDark { get; }
        public abstract Color CommandBarSeparatorLight { get; }
        public abstract Color ContentBackgroundGradientBegin { get; }
        public abstract Color ContentBackgroundGradientEnd { get; }
        public abstract Color ContentBorder { get; }
        public abstract Color Hyperlink { get; }
        public abstract Color HyperlinkPressed { get; }
        public abstract Color ImageMarginGradientBegin { get; }
        public abstract Color ImageMarginGradientEnd { get; }
        public abstract Color ImageMarginGradientMiddle { get; }
        public abstract Color MenuBarGradientBegin { get; }
        public abstract Color MenuBarGradientEnd { get; }
        public abstract Color MenuBorder { get; }
        public abstract Color OverflowButtonGradientBegin { get; }
        public abstract Color OverflowButtonGradientEnd { get; }
        public abstract Color PaddingBackColor { get; }
        public abstract Color TaskFormProgressDark { get; }
        public abstract Color TaskFormProgressLight { get; }
        public abstract Color TaskPanelDisabled { get; }
        public abstract Color TaskSectionHeader { get; }
    }
}
