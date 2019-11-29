// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Dialogs
{
    using System;
    using System.Windows.Forms;

    public partial class ExceptionDialog : Form
    {
        private ExceptionDialog()
        {
            InitializeComponent();
        }

        public static void Report(string userName, string exception)
        {
            using var dialog = new ExceptionDialog();
            dialog.txtInfo.Text = $"{userName}{Environment.NewLine}{Environment.NewLine}{exception}";
            dialog.ShowDialog();
        }

        private void btnReport_Click(object sender, EventArgs e)
        {
            DialogHelper.ProcessStart("https://github.com/jexuswebserver/JexusManager/issues/new");
        }

        private void btnCopy_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(txtInfo.Text);
            MessageBox.Show("Copied");
        }
    }
}
