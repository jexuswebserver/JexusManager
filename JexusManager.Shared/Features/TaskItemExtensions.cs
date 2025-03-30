// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Reflection;

namespace JexusManager.Features
{
    using System.Collections;
    using System.Diagnostics;
    using System.Drawing;
    using System.Windows.Forms;

    using JexusManager.Properties;

    using Microsoft.Web.Management.Client;
    using Microsoft.Web.Management.Client.Win32;

    public static class TaskItemExtensions
    {
        public static void Fill(this TaskListCollection tasks, ToolStrip actionPanel, ContextMenuStrip actionMenu, TaskList extra = null)
        {
            actionPanel.Items.Clear();
            actionMenu?.Items.Clear();
            if (extra != null)
            {
                TaskListFill(actionPanel, actionMenu, extra, extra.GetTaskItems());
            }

            foreach (var item in tasks.GetTaskListItems(null))
            {
                TaskListFill(actionPanel, actionMenu, item.Key, item.Value);
            }

            actionPanel.Items.RemoveAt(actionPanel.Items.Count - 1);
            actionMenu?.Items.RemoveAt(actionMenu.Items.Count - 1);
        }

        private static void TaskListFill(ToolStrip actionPanel, ContextMenuStrip actionMenu, TaskList extra, ICollection items)
        {
            if (items.Count == 0)
            {
                return;
            }

            foreach (TaskItem task in items)
            {
                task.TaskItemFill(extra, actionPanel, actionMenu, null);
            }

            actionPanel.Items.Add(new ToolStripSeparator());
            actionMenu?.Items.Add(new ToolStripSeparator());
        }

        public static MethodTaskItem SetUsage(this MethodTaskItem method, bool enabled = true, MethodTaskItemUsages usage = MethodTaskItemUsages.ContextMenu | MethodTaskItemUsages.TaskList)
        {
            method.Enabled = enabled;
            method.Usage = usage;
            return method;
        }

        private static void TaskItemFill(this TaskItem item, TaskList list, ToolStrip actionPanel, ContextMenuStrip actionMenu, ToolStripMenuItem parent)
        {
            var method = item as MethodTaskItem;
            if (method != null)
            {
                if (method.Text == "-")
                {
                    actionPanel.Items.Add(new ToolStripSeparator());
                    if ((method.Usage & MethodTaskItemUsages.ContextMenu) == MethodTaskItemUsages.ContextMenu)
                    {
                        if (parent == null)
                        {
                            actionMenu.Items.Add(new ToolStripSeparator());
                        }
                        else
                        {
                            parent.DropDownItems.Add(new ToolStripSeparator());
                        }
                    }

                    return;
                }

                var result = new ToolStripButton(method.Text, method.Image ?? Resources.transparent_16,
                    (o, args) =>
                    {
                        try
                        {
                            list.InvokeMethod(method.MethodName, method.UserData);
                        }
                        catch (TargetInvocationException ex)
                        {
                            if (ex.InnerException is UnauthorizedAccessException)
                            {
                                MessageBox.Show($"{ex.InnerException.Message}. Running Jexus Manager as administrator might resolve it.", "Jexus Manager", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                Debug.WriteLine(ex.InnerException);
                                return;
                            }

                            throw new InvalidOperationException($"menu item {method.Text} function {method.MethodName}", ex);
                        }
                    })
                {
                    ImageAlign = ContentAlignment.MiddleLeft,
                    ImageTransparentColor = Color.Magenta,
                    TextAlign = ContentAlignment.MiddleLeft,
                    Enabled = method.Enabled,
                    AutoToolTip = false
                };
                actionPanel.Items.Add(result);
                if ((method.Usage & MethodTaskItemUsages.ContextMenu) == MethodTaskItemUsages.ContextMenu)
                {
                    var result2 = new ToolStripMenuItem(method.Text, method.Image ?? Resources.transparent_16,
                        (o, args) =>
                        {
                            try
                            {
                                list.InvokeMethod(method.MethodName, method.UserData);
                            }
                            catch (TargetInvocationException ex)
                            {
                                if (ex.InnerException is UnauthorizedAccessException)
                                {
                                    MessageBox.Show($"{ex.InnerException.Message}. Running Jexus Manager as administrator might resolve it.", "Jexus Manager", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    Debug.WriteLine(ex.InnerException);
                                    return;
                                }

                                throw new InvalidOperationException($"menu item {method.Text} function {method.MethodName}", ex);
                            }
                        })
                    {
                        ImageTransparentColor = Color.Magenta,
                        Enabled = method.Enabled,
                        AutoToolTip = false
                    };
                    if (parent == null)
                    {
                        actionMenu.Items.Add(result2);
                    }
                    else
                    {
                        parent.DropDownItems.Add(result2);
                    }
                }

                return;
            }

            var text = item as TextTaskItem;
            if (text != null)
            {
                var result = new ToolStripLabel(text.Text, text.Image ?? Resources.transparent_16)
                {
                    ImageAlign = ContentAlignment.MiddleLeft,
                    ImageTransparentColor = Color.Magenta,
                    TextAlign = ContentAlignment.MiddleLeft,
                    Enabled = text.Enabled
                };
                if (text.IsHeading)
                {
                    result.Font = new Font(result.Font, FontStyle.Bold);
                }

                actionPanel.Items.Add(result);
                return;
            }

            var group = item as GroupTaskItem;
            if (group != null)
            {
                var result = new ToolStripLabel(group.Text)
                {
                    ImageAlign = ContentAlignment.MiddleLeft,
                    ImageTransparentColor = Color.Magenta,
                    TextAlign = ContentAlignment.MiddleLeft,
                    Enabled = group.Enabled
                };
                if (group.IsHeading)
                {
                    result.Font = new Font(result.Font, FontStyle.Bold);
                    result.BackColor = SystemColors.ControlDarkDark;
                }

                actionPanel.Items.Add(result);
                ToolStripMenuItem result2 = null;
                if (actionMenu != null)
                {
                    result2 = new ToolStripMenuItem(group.Text);
                    if (parent == null)
                    {
                        actionMenu.Items.Add(result2);
                    }
                    else
                    {
                        parent.DropDownItems.Add(result2);
                    }
                }

                foreach (TaskItem subItem in group.Items)
                {
                    TaskItemFill(subItem, list, actionPanel, actionMenu, result2);
                }
            }
        }
    }
}
