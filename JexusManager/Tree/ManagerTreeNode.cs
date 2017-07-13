// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Tree
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.Design;
    using System.Diagnostics;
    using System.IO;
    using System.Threading.Tasks;
    using System.Windows.Forms;

    using Microsoft.Web.Administration;
    using Microsoft.Web.Management.Server;

    using Application = Microsoft.Web.Administration.Application;

    internal abstract class ManagerTreeNode : TreeNode
    {
        public IServiceProvider ServiceProvider { get; set; }

        protected ManagerTreeNode(string text, IServiceProvider serviceProvider) : base(text)
        {
            ServiceProvider = serviceProvider;
        }

        public abstract string PathToSite { get; }
        public abstract string Folder { get; }

        public abstract string Uri { get; }
        public abstract ServerManager ServerManager { get; set; }

        public abstract void LoadPanels(MainForm mainForm, ServiceContainer serviceContainer, List<ModuleProvider> moduleProviders);

        public abstract Task HandleDoubleClick(MainForm mainForm);

        protected void LoadChildren(Application rootApp, int rootLevel, string rootFolder, string pathToSite, ContextMenuStrip phyMenu, ContextMenuStrip vDirMenu, ContextMenuStrip appMenu)
        {
            var treeNodes = new List<TreeNode>();
            foreach (VirtualDirectory virtualDirectory in rootApp.VirtualDirectories)
            {
                var path = virtualDirectory.PathToSite();
                if (!path.StartsWith(pathToSite))
                {
                    continue;
                }

                if (GetLevel(path) != rootLevel + 1)
                {
                    continue;
                }

                // IMPORTANT: only create level+1 vDir nodes.
                var virtualDirectoryNode = new VirtualDirectoryTreeNode(ServiceProvider, virtualDirectory) { ContextMenuStrip = vDirMenu };
                treeNodes.Add(virtualDirectoryNode);
            }

            var loaded = new HashSet<string>();
            if (Directory.Exists(rootFolder))
            {
                // IMPORTANT: only create level+1 physical nodes.
                foreach (var folder in new DirectoryInfo(rootFolder).GetDirectories())
                {
                    var path = folder.Name;
                    var isApp = false;
                    foreach (Application app in rootApp.Site.Applications)
                    {
                        if (!app.Path.StartsWith(pathToSite))
                        {
                            continue;
                        }

                        if (app.Path != pathToSite + '/' + path)
                        {
                            continue;
                        }

                        loaded.Add(app.Path);
                        var appNode = new ApplicationTreeNode(ServiceProvider, app) { ContextMenuStrip = appMenu };
                        treeNodes.Add(appNode);
                        isApp = true;
                    }

                    if (isApp)
                    {
                        continue;
                    }

                    var directory = new PhysicalDirectoryTreeNode(ServiceProvider, new PhysicalDirectory(folder, path, rootApp))
                    {
                        ContextMenuStrip = phyMenu
                    };
                    treeNodes.Add(directory);
                }
            }

            foreach (Application application in rootApp.Site.Applications)
            {
                if (application.IsRoot())
                {
                    continue;
                }

                if (!application.Path.StartsWith(pathToSite))
                {
                    continue;
                }

                if (loaded.Contains(application.Path))
                {
                    continue;
                }

                if (GetLevel(application.Path) != rootLevel + 1)
                {
                    continue;
                }

                // IMPORTANT: only create level+1 physical nodes.
                var appNode = new ApplicationTreeNode(ServiceProvider, application) { ContextMenuStrip = appMenu };
                treeNodes.Add(appNode);
            }

            treeNodes.Sort(s_comparer);
            Nodes.AddRange(treeNodes.ToArray());
        }

        private static readonly TreeNodeComparer s_comparer = new TreeNodeComparer();

        private sealed class TreeNodeComparer : IComparer<TreeNode>
        {
            public int Compare(TreeNode x, TreeNode y)
            {
                return String.Compare(x.Text, y.Text, StringComparison.OrdinalIgnoreCase);
            }
        }

        public abstract Task Expand(MainForm mainForm);
        public abstract Task AddApplication(ContextMenuStrip appMenu);
        public abstract void AddVirtualDirectory(ContextMenuStrip vDirMenu);

        public void Explore()
        {
            DialogHelper.Explore(Folder);
        }

        public void EditPermissions()
        {
            NativeMethods.ShowFileProperties(Folder);
        }

        public void Browse()
        {
            Process.Start(Uri);
        }

        protected static int GetLevel(string pathToSite)
        {
            return pathToSite.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries).Length;
        }

        internal static void AddToParent(TreeNode parentNode, ManagerTreeNode appNode)
        {
            parentNode.Nodes.Add(appNode);
            parentNode.TreeView.SelectedNode = appNode;
        }
    }
}