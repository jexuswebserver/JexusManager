// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Features.Handlers
{
    using System.Collections.Generic;
    using System.Linq;

    using Microsoft.Web.Administration;

    internal class HandlersItem : IItem<HandlersItem>
    {
        public HandlersItem(ConfigurationElement element)
        {
            this.Element = element;
            this.Flag = element == null || element.IsLocallyStored ? "Local" : "Inhertied";
            if (element == null)
            {
                this.PreConditions = new List<string>(0);
                Verb = "*";
                Path = Modules = ScriptProcessor = Type = string.Empty;
                ResponseBufferLimit = 4194304;
                RequireAccess = 3;
                return;
            }

            this.Name = (string)element["name"];
            this.Path = (string)element["path"];
            this.ResourceType = (long)element["resourceType"];
            this.Verb = (string)element["verb"];
            this.RequireAccess = (long)element["requireAccess"];
            this.Modules = (string)element["modules"];
            this.ScriptProcessor = (string)element["scriptProcessor"];
            this.Type = (string)element["type"];
            var content = (string)element["preCondition"];
            this.PreConditions = content.Split(',').ToList();
            ResponseBufferLimit = (uint)element["responseBufferLimit"];
            AllowPathInfo = (bool)element["allowPathInfo"];
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

        public ConfigurationElement Element { get; set; }

        public string Flag { get; set; }

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

        public void Apply()
        {
            Element["name"] = Name;
            Element["path"] = Path;
            Element["resourceType"] = ResourceType;
            Element["verb"] = Verb;
            Element["requireAccess"] = RequireAccess;
            Element["modules"] = Modules;
            Element["scriptProcessor"] = ScriptProcessor;
            Element["type"] = Type;
            Element["preCondition"] = PreConditions.Combine(",");
            Element["responseBufferLimit"] = ResponseBufferLimit;
            Element["allowPathInfo"] = AllowPathInfo;
        }

        public bool Match(HandlersItem other)
        {
            // match combined keys.
            return other != null && other.Name == this.Name;
        }
    }
}
