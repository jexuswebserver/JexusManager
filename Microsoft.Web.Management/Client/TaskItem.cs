// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections;

namespace Microsoft.Web.Management.Client
{
    public abstract class TaskItem
    {
        protected TaskItem(string text, string category)
            : this(text, category, string.Empty)
        { }

        protected TaskItem(string text, string category, string description)
        {
            Text = text;
            Category = category;
            Description = description;
        }

        public string Category { get; }
        public string Description { get; }
        public bool Enabled { get; set; }
        public IDictionary Properties { get; }
        public string Text { get; }
    }
}
