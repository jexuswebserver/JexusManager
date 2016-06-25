// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Web.Management.Client
{
    public sealed class ModulePageInfo
    {
        public ModulePageInfo(Module associatedModule, Type pageType, string title)
            : this(associatedModule, pageType, title, string.Empty)
        {
        }

        public ModulePageInfo(Module associatedModule, Type pageType, string title, string description)
            : this(associatedModule, pageType, title, description, null, null)
        {
        }

        public ModulePageInfo(Module associatedModule, Type pageType, string title, string description, object smallImage, object largeImage)
            : this(associatedModule, pageType, title, description, smallImage, largeImage, string.Empty)
        {
        }

        public ModulePageInfo(Module associatedModule, Type pageType, string title, string description, object smallImage, object largeImage, string longDescription)
        {
            AssociatedModule = associatedModule;
            PageType = pageType;
            Title = title;
            Description = description;
            SmallImage = smallImage;
            LargeImage = largeImage;
            LongDescription = longDescription;
        }

        public override bool Equals(object obj)
        {
            var second = obj as ModulePageInfo;
            return second != null && (PageType == second.PageType && AssociatedModule == second.AssociatedModule);
        }

        public override int GetHashCode()
        {
            return PageType.GetHashCode();
        }

        public Module AssociatedModule { get; }
        public string Description { get; }

        public bool IsEnabled
        {
            get { return true; }
        }

        public object LargeImage { get; }
        public string LongDescription { get; }
        public Type PageType { get; }
        public object SmallImage { get; }
        public string Title { get; }
    }
}