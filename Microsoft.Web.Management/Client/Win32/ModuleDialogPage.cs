// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections;
using System.ComponentModel;
using System.Reflection;
using System.Windows.Forms;
using Microsoft.Web.Management.Properties;

namespace Microsoft.Web.Management.Client.Win32
{
#if DESIGN
    public class ModuleDialogPage : ModulePage
#else
    public abstract class ModuleDialogPage : ModulePage
#endif
    {
        private sealed class DialogTaskList : TaskList
        {
            private readonly ModuleDialogPage _owner;

            public DialogTaskList(ModuleDialogPage owner)
            {
                _owner = owner;
            }

            public bool AppliedChanges { get; set; }

            public override ICollection GetTaskItems()
            {
                const MethodTaskItemUsages usage = MethodTaskItemUsages.ContextMenu | MethodTaskItemUsages.TaskList;
                var result = new ArrayList
                {
                    new MethodTaskItem("ApplyChanges", "Apply", string.Empty, string.Empty, Resources.apply_16)
                    {
                        Usage = usage
                    },
                    new MethodTaskItem("CancelChanges", "Cancel", string.Empty, string.Empty, Resources.cancel_16)
                    {
                        Usage = usage
                    }
                };
                foreach (TaskItem item in result)
                {
                    item.Enabled = _owner.HasChanges;
                }

                return result.ToArray(typeof(TaskItem)) as TaskItem[];
            }

            [Obfuscation(Exclude = true)]
            public void ApplyChanges()
            {
                _owner.ApplyChanges();
            }

            [Obfuscation(Exclude = true)]
            public void CancelChanges()
            {
                _owner.CancelChanges();
            }
        }

        private bool _showDirtyPageAlert;
        private TaskList _dialogTaskList;
        private DialogTaskList _taskList;

        protected ModuleDialogPage()
        {
        }

#if DESIGN
        protected internal virtual bool ApplyChanges()
        {
            throw new NotImplementedException();
        }

        protected internal virtual void CancelChanges()
        {
            throw new NotImplementedException();
        }

#else
        internal protected abstract bool ApplyChanges();
        internal protected abstract void CancelChanges();
#endif
        protected override void OnDeactivating(CancelEventArgs e)
        {
        }

        protected virtual void OnRefresh()
        {
            throw new NotImplementedException();
        }

        protected override sealed void Refresh()
        {
            if (_dialogTaskList != null)
            {
                ((DialogTaskList)_dialogTaskList).AppliedChanges = false;
            }

            _showDirtyPageAlert = false;
            OnRefresh();
        }

#if DESIGN
        internal protected virtual bool CanApplyChanges { get; }
#else
        internal protected abstract bool CanApplyChanges { get; }
#endif
        protected virtual string ChangesSavedMessage { get; }

        protected override TaskListCollection Tasks
        {
            get
            {
                if (_taskList == null)
                {
                    _taskList = new DialogTaskList(this);
                }

                base.Tasks.Add(_taskList);
                return base.Tasks;
            }
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // ModuleDialogPage
            // 
            this.Name = "ModuleDialogPage";
            this.Size = new System.Drawing.Size(648, 516);
            this.ResumeLayout(false);
        }
    }
}