// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Web.Management.Client.Win32
{
#if DESIGN
    public class TaskForm : BaseTaskForm
#else
    public abstract class TaskForm : BaseTaskForm
#endif
    {
#if DESIGN
        public TaskForm() { }
#endif
        protected TaskForm(IServiceProvider serviceProvider)
            : base(serviceProvider)
        {
        }
#if DESIGN
        protected virtual void OnAccept()
        {
            throw new NotImplementedException();
        }
#else
        protected abstract void OnAccept();
#endif
        protected virtual void OnCancel()
        {
            Close();
        }

        protected override void StartTaskProgress()
        {
        }

        protected override void StopTaskProgress()
        {
        }

        protected override sealed void Update()
        {
        }

        protected void UpdateTaskForm()
        {
        }

        protected virtual bool CanAccept
        {
            get { return true; }
        }

        protected virtual bool CanCancel
        {
            get { return true; }
        }

        protected virtual bool IsCancellable
        {
            get { return false; }
        }
    }
}
