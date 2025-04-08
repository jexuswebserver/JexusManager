// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Web.Management.Client
{
    [Serializable]
    public sealed class ModuleListPageGrouping
    {
        public ModuleListPageGrouping(string name, string text)
        {
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }

            if (text == null)
            {
                throw new ArgumentNullException("text");
            }

            Name = name;
            Text = text;
        }

        public override bool Equals(Object obj)
        {
            var second = obj as ModuleListPageGrouping;
            return second != null && (string.Equals(Name, second.Name) && string.Equals(Text, second.Text));
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode() + Text.GetHashCode();
        }

        public override string ToString()
        {
            return Text;
        }

        public string Name { get; }
        public string Text { get; }
    }
}
