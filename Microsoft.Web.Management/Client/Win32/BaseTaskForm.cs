// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.ComponentModel;
using System.Resources;
using System.Windows.Forms;

namespace Microsoft.Web.Management.Client.Win32
{
#if DESIGN
    public class BaseTaskForm : DialogForm
#else
    public abstract class BaseTaskForm : DialogForm
#endif
    {
#if DESIGN
        public BaseTaskForm() { }
#endif
        protected BaseTaskForm(IServiceProvider serviceProvider)
            : base(serviceProvider)
        {
        }

        protected void CancelAsyncTask()
        {
        }

        protected override void DisplayErrorMessage(Exception ex, ResourceManager resourceManager)
        {
        }

        protected void InvalidateTask(bool progressGradientOnly)
        {
        }

        protected override void OnClosing(CancelEventArgs e)
        {
        }

        protected void SetButtonsPanel(Control buttons)
        {
        }

        protected void SetContent(Control content)
        {
        }

        protected void StartAsyncTask(DoWorkEventHandler doWorkHandler,
            RunWorkerCompletedEventHandler workCompletedHandler)
        {
            StartAsyncTask(doWorkHandler, workCompletedHandler, null, null);
        }

        protected void StartAsyncTask(
            DoWorkEventHandler doWorkHandler,
            RunWorkerCompletedEventHandler workCompletedHandler,
            MethodInvoker cancelTaskHandler)
        {
            StartAsyncTask(doWorkHandler, workCompletedHandler, cancelTaskHandler, null);
        }

        protected void StartAsyncTask(
            DoWorkEventHandler doWorkHandler,
            RunWorkerCompletedEventHandler workCompletedHandler,
            MethodInvoker cancelTaskHandler,
            Object argument)
        {
            var worker = new BackgroundWorker();
            worker.DoWork += doWorkHandler;
            worker.RunWorkerCompleted += workCompletedHandler;
            worker.RunWorkerAsync(argument);
        }

        protected virtual void StartTaskProgress()
        {
        }

        protected virtual void StopTaskProgress()
        {
        }

        protected bool BackgroundJobRunning
        {
            get { return false; }
        }

        protected virtual int BorderMargin
        {
            get { return 0; }
        }
    }
}
