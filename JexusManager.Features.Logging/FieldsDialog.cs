// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Features.Logging
{
    using System;
    using System.ComponentModel;
    using System.Reflection;
    using System.Reactive.Disposables;
    using System.Reactive.Linq;
    using System.Windows.Forms;

    using Microsoft.Web.Administration;
    using Microsoft.Web.Management.Client.Win32;

    internal partial class FieldsDialog : DialogForm
    {
        private class StandardListViewItem : ListViewItem
        {
            public StandardListViewItem(LogExtFileFlags flag)
            {
                Text = ToString(flag);
                Flag = flag;
            }

            public LogExtFileFlags Flag { get; }

            private static string ToString(LogExtFileFlags flag)
            {
                FieldInfo fi = typeof(LogExtFileFlags).GetField(flag.ToString());
                DescriptionAttribute dna =
                    (DescriptionAttribute)Attribute.GetCustomAttribute(
                        fi, typeof(DescriptionAttribute));
                return dna.Description;
            }
        }

        private class CustomListViewItem : ListViewItem
        {
            public CustomLogField Custom { get; }

            public CustomListViewItem(CustomLogField custom)
            {
                Custom = custom;
                Text = custom.LogFieldName;
                SubItems.Add(ToString(custom.SourceType));
                SubItems.Add(custom.SourceName);
            }

            private static string ToString(CustomLogFieldSourceType type)
            {
                switch (type)
                {
                    case CustomLogFieldSourceType.RequestHeader:
                        return "Request Header";
                    case CustomLogFieldSourceType.ResponseHeader:
                        return "Response Header";
                    case CustomLogFieldSourceType.ServerVariable:
                        return "Server Variable";
                    default:
                        throw new ArgumentOutOfRangeException(nameof(type), type, null);
                }
            }

            public void Update()
            {
                Text = Custom.LogFieldName;
                SubItems[0].Text = ToString(Custom.SourceType);
                SubItems[1].Text = Custom.SourceName;
            }
        }

        private bool enabled;

        public FieldsDialog(IServiceProvider serviceProvider, Fields logFile)
            : base(serviceProvider)
        {
            InitializeComponent();
            if (logFile.CustomLogFields != null)
            {
                foreach (CustomLogField custom in logFile.CustomLogFields)
                {
                    lvCustom.Items.Add(new CustomListViewItem(custom));
                }
            }

            foreach (LogExtFileFlags flag in Enum.GetValues(typeof(LogExtFileFlags)))
            {
                lvStandard.Items.Add(new StandardListViewItem(flag)
                {
                    Checked = (logFile.LogExtFileFlags & flag) == flag
                });
            }

            btnAdd.Enabled = logFile.CustomLogFields != null;

            var container = new CompositeDisposable();
            FormClosed += (sender, args) => container.Dispose();

            container.Add(
                Observable.FromEventPattern<EventArgs>(lvCustom, "SelectedIndexChanged")
                .ObserveOn(System.Threading.SynchronizationContext.Current)
                .Subscribe(evt =>
                {
                    btnRemove.Enabled = lvCustom.SelectedItems.Count > 0;
                    btnEdit.Enabled = lvCustom.SelectedItems.Count == 1;
                }));

            container.Add(
                Observable.FromEventPattern<EventArgs>(lvStandard, "ItemChecked")
                .ObserveOn(System.Threading.SynchronizationContext.Current)
                .Subscribe(evt =>
                {
                    if (enabled)
                    {
                        btnOK.Enabled = true;
                    }
                }));

            container.Add(
                Observable.FromEventPattern<EventArgs>(btnAdd, "Click")
                .ObserveOn(System.Threading.SynchronizationContext.Current)
                .Subscribe(evt =>
                {
                    using (var dialog = new AddFieldDialog(ServiceProvider, null, logFile))
                    {
                        if (dialog.ShowDialog() != DialogResult.OK)
                        {
                            return;
                        }

                        lvCustom.Items.Add(new CustomListViewItem(dialog.Custom));
                    }
                    btnOK.Enabled = true;
                }));

            container.Add(
                Observable.FromEventPattern<EventArgs>(btnEdit, "Click")
                .ObserveOn(System.Threading.SynchronizationContext.Current)
                .Subscribe(evt =>
                {
                    var selected = (CustomListViewItem)lvCustom.SelectedItems[0];
                    using (var dialog = new AddFieldDialog(ServiceProvider, selected.Custom, logFile))
                    {
                        if (dialog.ShowDialog() != DialogResult.OK)
                        {
                            return;
                        }
                    }

                    selected.Update();
                    btnOK.Enabled = true;
                }));

            container.Add(
                Observable.FromEventPattern<EventArgs>(btnRemove, "Click")
                .ObserveOn(System.Threading.SynchronizationContext.Current)
                .Subscribe(evt =>
                {
                    foreach (CustomListViewItem item in lvCustom.SelectedItems)
                    {
                        item.Remove();
                        item.Custom.Delete();
                        logFile.CustomLogFields.Remove(item.Custom);
                    }

                    btnOK.Enabled = true;
                }));

            container.Add(
                Observable.FromEventPattern<EventArgs>(btnOK, "Click")
                .ObserveOn(System.Threading.SynchronizationContext.Current)
                .Subscribe(evt =>
                {
                    LogExtFileFlags flags = 0;
                    foreach (StandardListViewItem item in lvStandard.Items)
                    {
                        if (item.Checked)
                        {
                            flags |= item.Flag;
                        }
                    }

                    logFile.LogExtFileFlags = flags;
                    DialogResult = DialogResult.OK;
                }));
        }

        private void FieldsDialog_HelpButtonClicked(object sender, CancelEventArgs e)
        {
            DialogHelper.ProcessStart("http://go.microsoft.com/fwlink/?LinkId=210517#W3CLoggingFields");
        }

        private void FieldsDialog_Shown(object sender, EventArgs e)
        {
            enabled = true;
        }
    }
}
