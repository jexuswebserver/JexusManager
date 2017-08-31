// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Features.TraceFailedRequests.Wizards.AddTraceWizard
{
    using System.Collections.Generic;

    internal class Provider
    {
        public string Name { get; set; }
        public List<string> Areas { get; set; } = new List<string>();
        public List<string> SelectedAreas { get; set; } = new List<string>();
        public int Verbosity { get; set; } = 5;

        public bool Selected { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
