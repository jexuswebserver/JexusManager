// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Web.Administration;

namespace JexusManager.Features.TraceFailedRequests.Wizards.AddTraceWizard
{
    internal class AddTraceWizardData
    {
        public AddTraceWizardData(ConfigurationElement config, TraceFailedRequestsItem existing)
        {
            Editing = existing != null;
            if (existing == null)
            {
                FileName = "*";
            }
            else
            {
                FileName = existing.Path;
                Codes = existing.Codes;
                Time = existing.TimeTaken.GetTotalSeconds();
                Verbosity = existing.Verbosity;
            }

            var collection = config.GetCollection();
            foreach (ConfigurationElement item in collection)
            {
                var name = item.GetAttribute("name").Value.ToString();
                var areas = item.ChildElements["areas"].GetCollection();
                var provider = new Provider { Name = name, Areas = new List<string>(areas.Count) };
                foreach (ConfigurationElement area in areas)
                {
                    provider.Areas.Add(area["name"].ToString());
                }

                Providers.Add(provider);
            }

            if (existing != null)
            {
                var selection = existing.Element.GetCollection("traceAreas");
                foreach (ConfigurationElement item in selection)
                {
                    var name = item["provider"].ToString();
                    var areas = item["areas"].ToString();
                    foreach (var provider in Providers)
                    {
                        if (provider.Name == name)
                        {
                            provider.Selected = true;
                            foreach (var area in areas.Split(','))
                            {
                                provider.SelectedAreas.Add(area);
                            }
                        }
                    }
                }
            }
        }

        internal void Apply(TraceFailedRequestsItem item)
        {
            item.Codes = Codes;
            item.Path = FileName;
            item.TimeTaken = TimeSpan.FromSeconds(Time);
            item.Verbosity = Verbosity;
            item.SetProviders(Providers);
        }

        public string FileName { get; set; }

        public string Codes { get; set; }

        public long Time { get; set; }

        public long Verbosity { get; set; }

        public bool Editing { get; }

        public IList<Provider> Providers { get; } = new List<Provider>();

        public bool IsValid
        {
            get
            {
                return Providers.Any(item => item.Selected);
            }
        }
    }
}
