// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Features.Rewrite
{
    using System.Windows.Forms;

    internal class ConditionListViewItem : ListViewItem
    {
        private readonly ListViewSubItem _matchType;

        private readonly ListViewSubItem _pattern;

        public ConditionListViewItem(ConditionItem condition)
            : base(condition.Input)
        {
            _matchType = new ListViewSubItem(this, GetText(condition.MatchType));
            this.SubItems.Add(_matchType);
            _pattern = new ListViewSubItem(this, condition.MatchType > 3 ? condition.Pattern : "N/A");
            this.SubItems.Add(_pattern);
            this.Item = condition;
        }

        public ConditionItem Item { get; }

        public void Update()
        {
            this.Text = this.Item.Input;
            _matchType.Text = GetText(this.Item.MatchType);
            _pattern.Text = this.Item.MatchType > 3 ? this.Item.Pattern : "N/A";
        }

        private static string GetText(int matchType)
        {
            switch (matchType)
            {
                case 0:
                    return "Is File";
                case 1:
                    return "Is Not File";
                case 2:
                    return "Is Directory";
                case 3:
                    return "Is Not Directory";
                case 4:
                    return "Matches the Pattern";
                case 5:
                    return "Does Not Match the Pattern";
            }

            return string.Empty;
        }
    }
}
