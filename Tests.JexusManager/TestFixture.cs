// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Tests.JexusManager
{
    using global::JexusManager.Features.Rewrite.Outbound;

    using Xunit;

    using PresentationControls;

    internal class TestFixture
    {
        [Fact]
        public void TestFlags()
        {
            var box = new CheckBoxComboBox();
            box.Items.Add("A");
            box.Items.Add("Area");
            box.Items.Add("Use Custom Tags");
            Assert.Equal(0, OutboundRulePage.GetFilter(box));
            box.CheckBoxItems[0].Checked = true;
            Assert.Equal(1, OutboundRulePage.GetFilter(box));
            box.CheckBoxItems[1].Checked = true;
            Assert.Equal(3, OutboundRulePage.GetFilter(box));
            box.CheckBoxItems[2].Checked = true;
            Assert.Equal(32771, OutboundRulePage.GetFilter(box));

            OutboundRulePage.SetFilter(32771, box);
            Assert.True(box.CheckBoxItems[2].Checked);
            Assert.True(box.CheckBoxItems[1].Checked);
            Assert.True(box.CheckBoxItems[0].Checked);
            OutboundRulePage.SetFilter(3, box);
            Assert.False(box.CheckBoxItems[2].Checked);
            Assert.True(box.CheckBoxItems[1].Checked);
            Assert.True(box.CheckBoxItems[0].Checked);
            OutboundRulePage.SetFilter(1, box);
            Assert.False(box.CheckBoxItems[2].Checked);
            Assert.False(box.CheckBoxItems[1].Checked);
            Assert.True(box.CheckBoxItems[0].Checked);
            OutboundRulePage.SetFilter(0, box);
            Assert.False(box.CheckBoxItems[2].Checked);
            Assert.False(box.CheckBoxItems[1].Checked);
            Assert.False(box.CheckBoxItems[0].Checked);
        }
    }
}
