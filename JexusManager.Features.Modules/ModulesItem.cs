// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Features.Modules
{
    using System;
    using System.Collections.Generic;

    using Microsoft.Web.Administration;

    [Serializable]
    internal class ModulesItem : IItem<ModulesItem>
    {
        public ModulesItem()
        {
            PreConditions = new List<string>();
            Type = string.Empty;
            Flag = "Local";
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

        internal string OriginalKey { get; set; }

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
