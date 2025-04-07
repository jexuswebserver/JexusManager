// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Features.Rewrite
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Reactive.Disposables;
    using System.Reactive.Linq;
    using System.Windows.Forms;

    using Properties;

    using Microsoft.Web.Management.Client.Win32;

    public partial class NewRewriteRuleDialog : DialogForm
    {
        public NewRewriteRuleDialog(IServiceProvider serviceProvider)
            : base(serviceProvider)
        {
            InitializeComponent();
            btnBig.Image = DefaultTaskList.BasicSettingsImage;
            btnSmall.Image = DefaultTaskList.ViewImage;

            ilLarge.Images.Add(Resources.blank_32);
            ilLarge.Images.Add(Resources.map_32);
            ilLarge.Images.Add(Resources.blocking_32);
            ilLarge.Images.Add(Resources.blankout_32);
            ilLarge.Images.Add(Resources.lowercase_32);

            ilSmall.Images.Add(Resources.blank_16);
            ilSmall.Images.Add(Resources.map_16);
            ilSmall.Images.Add(Resources.blocking_16);
            ilSmall.Images.Add(Resources.blankout_16);
            ilSmall.Images.Add(Resources.lowercase_16);
            listView1.LargeImageList = ilLarge;
            listView1.SmallImageList = ilSmall;
            ListViewGroup listViewGroup1 = new ListViewGroup("Inbound rules", HorizontalAlignment.Left);
            ListViewGroup listViewGroup2 = new ListViewGroup("Inbound and Outbound Rules", HorizontalAlignment.Left);
            ListViewGroup listViewGroup3 = new ListViewGroup("Outbound rules", HorizontalAlignment.Left);
            ListViewGroup listViewGroup4 = new ListViewGroup(
                "Search Engine Optimization (SEO)",
                HorizontalAlignment.Left);
            ListViewItem listViewItem1 = new ListViewItem("Blank rule");
            listViewItem1.ImageIndex = 0;
            ListViewItem listViewItem2 = new ListViewItem("Rule with rewrite map");
            listViewItem2.ImageIndex = 1;
            ListViewItem listViewItem3 = new ListViewItem("Request blocking");
            listViewItem3.ImageIndex = 2;
            ListViewItem listViewItem4 = new ListViewItem("User-friendly URL");
            listViewItem4.ImageIndex = 1;
            ListViewItem listViewItem5 = new ListViewItem("Blank rule");
            listViewItem5.ImageIndex = 3;
            ListViewItem listViewItem6 = new ListViewItem("Enforce lowercase URLs");
            listViewItem6.ImageIndex = 4;

            listViewGroup1.Header = "Inbound rules";
            listViewGroup1.Name = "lvgIn";
            listViewGroup2.Header = "Inbound and Outbound Rules";
            listViewGroup2.Name = "lvgInOut";
            listViewGroup3.Header = "Outbound rules";
            listViewGroup3.Name = "lvgOut";
            listViewGroup4.Header = "Search Engine Optimization (SEO)";
            listViewGroup4.Name = "lvgSeo";
            listView1.Groups.AddRange(new[] { listViewGroup1, listViewGroup2, listViewGroup3, listViewGroup4 });
            listViewItem1.Group = listViewGroup1;
            listViewItem1.ToolTipText =
                "Select this template to create a new inbound rule without any preset values. This template opens the \"Edit Rule\" page that you can use to define a new rewrite rule for changing the requested URL address.";
            listViewItem2.Group = listViewGroup1;
            listViewItem2.ToolTipText =
                "Select this template to create a rewrite or redirect rule that uses a rewrite map. The rewrite map for this rule can contain a large number of static mappings between original URLs and rewritten URLs or redirection URLs, depending on whether the rule is rewriting or redirecting requests.";
            listViewItem3.Group = listViewGroup1;
            listViewItem3.ToolTipText =
                "Select this template to create a rule that will block client requests based on ce"
                + "rtain text patterns in the URL path, query string, HTTP headers, and server vari" + "ables.";
            listViewItem4.Group = listViewGroup2;
            listViewItem4.ToolTipText =
                "Select this template to create rules for enabling user-friendly URLs for your dynamic Web applications. This template creates rules that rewrite user-friendly URLs to internal URLs that your Web application understands.";
            listViewItem5.Group = listViewGroup3;
            listViewItem5.ToolTipText =
                "Select this template to create a new outbound rule without any preset values. This template opens the \"Edit Rule\" page that you can use to define a new rewrite rule for changing the content of an HTTP response.";
            listViewItem6.Group = listViewGroup4;
            listViewItem6.ToolTipText =
                "Use this template to create a rule that will enforce the use of lowercase letters" + " in the URL.";
            listView1.Items.AddRange(
                new[] { listViewItem1, listViewItem2, listViewItem3, listViewItem4, listViewItem5, listViewItem6 });

            var container = new CompositeDisposable();
            FormClosed += (sender, args) => container.Dispose();

            container.Add(
                Observable.FromEventPattern<EventArgs>(listView1, "SelectedIndexChanged")
                .ObserveOn(System.Threading.SynchronizationContext.Current)
                .Subscribe(evt =>
                {
                    txtDescription.Text = listView1.SelectedItems.Count == 0
                        ? string.Empty
                        : listView1.SelectedItems[0].ToolTipText;
                    btnOK.Enabled = listView1.SelectedItems.Count > 0;
                }));

            var doubleClick = Observable.FromEventPattern<EventArgs>(listView1, "DoubleClick");
            container.Add(
                doubleClick.ObserveOn(System.Threading.SynchronizationContext.Current)
                .Subscribe(evt =>
                {
                    if (listView1.SelectedItems.Count == 0)
                    {
                        return;
                    }
                }));

            container.Add(
                Observable.FromEventPattern<EventArgs>(btnOK, "Click")
                .Merge(doubleClick)
                .ObserveOn(System.Threading.SynchronizationContext.Current)
                .Subscribe(evt =>
                {
                    SelectedIndex = listView1.SelectedIndices[0];
                    DialogResult = DialogResult.OK;
                }));

            var big = Observable.FromEventPattern<EventArgs>(btnBig, "Click");
            container.Add(
                big.ObserveOn(System.Threading.SynchronizationContext.Current)
                .Subscribe(evt =>
                {
                    btnSmall.Checked = !btnBig.Checked;
                }));

            var small = Observable.FromEventPattern<EventArgs>(btnSmall, "Click");
            container.Add(
                small.ObserveOn(System.Threading.SynchronizationContext.Current)
                .Subscribe(evt =>
                {
                    btnBig.Checked = !btnSmall.Checked;
                }));

            container.Add(
                big.Merge(small)
                .ObserveOn(System.Threading.SynchronizationContext.Current)
                .Subscribe(evt =>
                {
                    listView1.View = btnBig.Checked ? View.LargeIcon : View.SmallIcon;
                }));
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int SelectedIndex { get; private set; }

        private void NewRewriteRuleDialogHelpButtonClicked(object sender, CancelEventArgs e)
        {
            DialogHelper.ProcessStart("http://go.microsoft.com/fwlink/?LinkID=130421&amp;clcid=0x409");
        }
    }
}
