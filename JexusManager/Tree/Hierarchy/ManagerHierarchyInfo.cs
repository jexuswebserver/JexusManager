// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections;
using System.ComponentModel;
using System.Collections.Generic;
using System.Windows.Forms;
using Microsoft.Web.Management.Client;

namespace JexusManager.Tree.Hierarchy
{
    /// <summary>
    /// HierarchyInfo implementation for ManagerTreeNode objects.
    /// </summary>
    internal class ManagerHierarchyInfo : HierarchyInfo
    {
        private readonly ManagerTreeNode _treeNode;
        private TaskListCollection _tasks;
        private Hashtable _properties;

        public ManagerHierarchyInfo(ManagerTreeNode treeNode)
            : base(treeNode.ServiceProvider)
        {
            _treeNode = treeNode ?? throw new ArgumentNullException(nameof(treeNode));
        }

        /// <summary>
        /// Gets the manager tree node associated with this hierarchy info.
        /// </summary>
        public ManagerTreeNode TreeNode => _treeNode;

        public override string NodeType => _treeNode.GetType().Name;

        public override string Text => _treeNode.Text;

        public override string ToolTip => _treeNode.ToolTipText;

        public override object Image => _treeNode.ImageIndex;

        public override IDictionary Properties
        {
            get
            {
                if (_properties == null)
                {
                    _properties = new Hashtable();
                }
                return _properties;
            }
        }

        public override bool SupportsChildren => _treeNode.Nodes.Count > 0;

        protected override bool SupportsDelete => false;

        protected override bool SupportsRename => false;

        public override HierarchyPriority Priority => HierarchyPriority.Normal;

        public override HierarchyVisibility Visibility => HierarchyVisibility.All;

        public override bool IsExtendable => false;

        protected override Connection Connection
        {
            get
            {
                // This would need to be implemented based on the actual connection type
                // in the application, but for now returning null as a placeholder
                return null;
            }
        }

        protected override HierarchyInfo[] GetChildren()
        {
            if (!SupportsChildren)
            {
                return null;
            }

            var children = new List<HierarchyInfo>();
            foreach (TreeNode node in _treeNode.Nodes)
            {
                if (node is ManagerTreeNode managerNode && managerNode.HierarchyInfo != null)
                {
                    children.Add(managerNode.HierarchyInfo);
                }
            }

            return children.Count > 0 ? children.ToArray() : null;
        }

        public override TaskListCollection Tasks
        {
            get
            {
                if (_tasks == null)
                {
                    _tasks = new TaskListCollection();
                }
                return _tasks;
            }
        }

        protected override bool OnSelected()
        {
            // When this hierarchy info is selected, select the corresponding tree node
            _treeNode.TreeView.SelectedNode = _treeNode;
            return true;
        }

        public override void Refresh()
        {
            // Refresh the tree node
            _treeNode.Nodes.Clear();

            if (_treeNode.TreeView != null && _treeNode.TreeView.IsHandleCreated)
            {
                // Add a temporary node to ensure the node can be expanded
                _treeNode.Nodes.Add(ManagerTreeNode.TempNodeName);
            }
        }
    }
}
