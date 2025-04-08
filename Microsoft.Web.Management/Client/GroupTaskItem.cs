// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections;

namespace Microsoft.Web.Management.Client
{
    public sealed class GroupTaskItem : TaskItem
    {
        public GroupTaskItem(string memberName, string text, string category)
            : this(memberName, text, category, false)
        { }

        public GroupTaskItem(string memberName, string text, string category, bool isHeading)
            : base(text, category)
        {
            MemberName = memberName;
            IsHeading = isHeading;
            Items = new ArrayList();
        }

        public bool IsHeading { get; }
        public IList Items { get; }
        public string MemberName { get; }
    }
}
