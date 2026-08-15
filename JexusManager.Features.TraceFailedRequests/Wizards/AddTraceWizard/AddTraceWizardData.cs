// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;

namespace JexusManager.Features.TraceFailedRequests.Wizards.AddTraceWizard
{
    internal class AddTraceWizardData
    {
        public AddTraceWizardData(Provider[] providerDefinitions, TraceFailedRequestsItem existing)
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

            foreach (var provider in providerDefinitions ?? Array.Empty<Provider>())
            {
                Providers.Add(new Provider { Name = provider.Name, Areas = new List<string>(provider.Areas) });
            }

            if (existing != null)
            {
                foreach (var selection in existing.Providers)
                {
                    foreach (var provider in Providers)
                    {
                        if (provider.Name == selection.Name)
                        {
                            provider.Selected = true;
                            provider.SelectedAreas.AddRange(selection.SelectedAreas);
                            provider.Verbosity = selection.Verbosity;
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
            item.Providers = new List<Provider>();
            foreach (var provider in Providers)
            {
                item.Providers.Add(new Provider
                {
                    Name = provider.Name,
                    Selected = provider.Selected,
                    Verbosity = provider.Verbosity,
                    SelectedAreas = new List<string>(provider.SelectedAreas)
                });
            }
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
