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
            Element = element;
            Flag = element == null || element.IsLocallyStored ? "Local" : "Inhertied";
            if (element == null)
            {
                PreConditions = new List<string>();
                Type = string.Empty;
                return;
            }

            Name = (string)element["name"];
            Type = (string)element["type"];
            var content = (string)element["preCondition"];
            PreConditions = content.Split(',').ToList();

            IsLocked = element.GetIsLocked();
            if (!string.IsNullOrWhiteSpace(Type))
            {
                IsManaged = true;
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
                if (item.Name == Name)
                {
                    Type = item.Image;
                    item.Loaded = true;
                    GlobalModule = item;
                    IsManaged = false;
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
                return PreConditions.Contains("managedHandler");
            }

            set
            {
                var current = PreConditions.Contains("managedHandler");
                if (value == current)
                {
                    return;
                }

                if (value)
                {
                    PreConditions.Add("managedHandler");
                }
                else
                {
                    PreConditions.Remove("managedHandler");
                }
            }
        }

        public string ModuleName
        {
            get
            {
                return IsManaged 
                    ? Type 
                    : GlobalModule == null 
                        ? string.Empty 
                        : GlobalModule.Image;
            }
        }

        public bool Equals(ModulesItem other)
        {
            // all properties
            return Match(other) && other.Type == Type;
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
            return other != null && other.Name == Name;
        }

        public void Unload()
        {
            if (GlobalModule == null)
            {
                return;
            }

            GlobalModule.Loaded = false;
            GlobalModule = null;
        }
    }
}
