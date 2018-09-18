// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace JexusManager.Tree
{
    using System.Collections.Generic;
    using System.ComponentModel.Design;
    using System.Windows.Forms;

    using JexusManager.Features.Main;

    using Microsoft.Web.Administration;
    using Microsoft.Web.Management.Server;

    internal sealed class PlaceholderTreeNode : ManagerTreeNode
    {
        public PlaceholderTreeNode(string name, int image)
            : base(name, null)
        {
            SelectedImageIndex = image;
            ImageIndex = image;
        }

        public override string PathToSite => string.Empty;

        public override string Folder => string.Empty;

        public override string Uri => string.Empty;

        public override ServerManager ServerManager { get; set; }

        public override ServerTreeNode ServerNode => throw new InvalidOperationException("Server node access is invalid for home page node");

        public override void LoadPanels(MainForm mainForm, ServiceContainer serviceContainer, List<ModuleProvider> moduleProviders)
        {
            mainForm.LoadInner(new HomePage());
        }

        public override void HandleDoubleClick(MainForm mainForm)
        {
        }

        public override void Expand(MainForm mainForm)
        {
        }

        public override void AddApplication(ContextMenuStrip appMenu)
        {
        }

        public override void AddVirtualDirectory(ContextMenuStrip vDirMenu)
        {
        }
    }
}
