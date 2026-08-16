// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Features.Handlers
{
    using System;
    using System.Collections.Generic;

    using Microsoft.Web.Administration;

    [Serializable]
    internal class HandlersItem : IItem<HandlersItem>
    {
        public HandlersItem()
        {
            PreConditions = new List<string>(0);
            Verb = "*";
            Path = Modules = ScriptProcessor = Type = string.Empty;
            ResponseBufferLimit = 4194304;
            RequireAccess = 3;
            Flag = "Local";
        }

        public bool AllowPathInfo { get; set; }

        public uint ResponseBufferLimit { get; set; }

        public string ScriptProcessor { get; set; }

        public long RequireAccess { get; set; }

        public string Verb { get; set; }

        public long ResourceType { get; set; }

        public string Path { get; set; }

        public List<string> PreConditions { get; set; }

        public string Type { get; set; }

        public string Modules { get; set; }

        public string Name { get; set; }

        public string Flag { get; set; }

        internal string OriginalKey { get; set; }

        public string GetState(long accessPolicy)
        {
            if (this.RequireAccess == 0L)
            {
                return "Enabled";
            }

            if (this.RequireAccess == 1L && (accessPolicy & 1L) == 1L)
            {
                return "Enabled";
            }

            if (this.RequireAccess == 2L && (accessPolicy & 2L) == 2L)
            {
                return "Enabled";
            }

            if (this.RequireAccess == 3L && (accessPolicy & 512L) == 512L)
            {
                return "Enabled";
            }

            if (this.RequireAccess == 4L && (accessPolicy & 4L) == 4L)
            {
                return "Enabled";
            }

            return "Disabled";
        }

        public string PathType
        {
            get
            {
                switch (this.ResourceType)
                {
                    case 0:
                        return "File";
                    case 1:
                        return "Folder";
                    case 2:
                        return "File or Folder";
                    case 3:
                        return "Unspecified";
                }

                return "Unspecified";
            }
        }

        public string TypeString
        {
            get
            {
                return !string.IsNullOrWhiteSpace(this.Type) ? this.Type : this.Modules;
            }
        }

        public bool Equals(HandlersItem other)
        {
            // all properties
            return this.Match(other) && other.Type == this.Type;
        }

        public bool Match(HandlersItem other)
        {
            // match combined keys.
            return other != null && other.Name == this.Name;
        }
    }
}
