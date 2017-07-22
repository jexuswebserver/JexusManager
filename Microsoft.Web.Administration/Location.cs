// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Xml.Linq;

namespace Microsoft.Web.Administration
{
    internal class Location
    {
        public string OverrideMode { get; private set; }
        public XElement Entity { get; set; }
        public string Path { get; internal set; }
        public bool FromTag { get; private set; }

        public Location(string path, string overrideMode, XElement entity, bool fromTag = false)
        {
            Path = path;
            OverrideMode = overrideMode;
            Entity = entity;
            FromTag = fromTag;
        }
    }
}