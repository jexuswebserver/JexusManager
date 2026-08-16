// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Features.TraceFailedRequests
{
    using JexusManager.Features.TraceFailedRequests.Wizards.AddTraceWizard;
    using Microsoft.Web.Administration;
    using System;
    using System.Collections.Generic;

    [Serializable]
    internal class TraceFailedRequestsItem : IItem<TraceFailedRequestsItem>
    {
        public TraceFailedRequestsItem()
        {
            Path = string.Empty;
            Flag = "Local";
            Providers = new List<Provider>();
        }

        public string Path { get; set; }

        internal string OriginalKey { get; set; }

        public long Verbosity { get; set; }

        public string Flag { get; set; }

        public bool Equals(TraceFailedRequestsItem other)
        {
            // all properties
            return Match(other);
        }

        public bool Match(TraceFailedRequestsItem other)
        {
            // match combined keys.
            return other != null && other.Path == Path;
        }

        public List<Provider> Providers { get; set; }

        public string Codes { get; set; }

        public TimeSpan TimeTaken { get; set; }
    }
}
