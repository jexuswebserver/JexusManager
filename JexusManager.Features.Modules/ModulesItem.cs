// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Features.Modules
{
    using System.Collections.Generic;
    using System.Linq;

    using Microsoft.Web.Administration;

    internal class ModulesItem : IItem<ModulesItem>
    {
        public ModulesItem(ConfigurationElement element)
        {
            this.Element = element;
            this.Flag = element == null || element.IsLocallyStored ? "Local" : "Inhertied";
            if (element == null)
            {
                PreConditions = new List<string>();
                Type = string.Empty;
                return;
            }

            this.Name = (string)element["name"];
            this.Type = (string)element["type"];
            var content = (string)element["preCondition"];
            this.PreConditions = content.Split(',').ToList();

            IsLocked = element.GetIsLocked();
            if (!string.IsNullOrWhiteSpace(this.Type))
            {
                this.IsManaged = true;
            }
        }

        public ModulesItem Load(ModulesFeature feature)
        {
            if (IsManaged)
            {
                return this;
            }

            foreach (var item in feature.GlobalModules)
            {
                if (item.Name == this.Name)
                {
                    this.Type = item.Image;
                    item.Loaded = true;
                    this.GlobalModule = item;
                    this.IsManaged = false;
                    break;
                }
            }

            return this;
        }

        public string IsLocked { get; set; }

        public GlobalModule GlobalModule { get; set; }

        public List<string> PreConditions { get; set; }

        public string Type { get; set; }

        public string Name { get; set; }

        public ConfigurationElement Element { get; set; }

        public string Flag { get; set; }

        public bool IsManaged { get; set; }

        public bool ForManagedOnly
        {
            get
            {
                return this.PreConditions.Contains("managedHandler");
            }

            set
            {
                var current = this.PreConditions.Contains("managedHandler");
                if (value == current)
                {
                    return;
                }

                if (value)
                {
                    this.PreConditions.Add("managedHandler");
                }
                else
                {
                    this.PreConditions.Remove("managedHandler");
                }
            }
        }

        public bool Equals(ModulesItem other)
        {
            // all properties
            return this.Match(other) && other.Type == this.Type;
        }

        public void Apply()
        {
            Element["name"] = Name;
            if (IsManaged)
            {
                Element["type"] = Type;
            }

            Element["preCondition"] = PreConditions.Combine(",");
            Element.SetIsLocked(IsLocked);
        }

        public bool Match(ModulesItem other)
        {
            // match combined keys.
            return other != null && other.Name == this.Name;
        }

        public void Unload()
        {
            if (GlobalModule == null)
            {
                return;
            }

            this.GlobalModule.Loaded = false;
            this.GlobalModule = null;
        }
    }
}
