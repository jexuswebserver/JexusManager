// Copyright (c) Lex Li. All rights reserved.
//
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using JexusManager.Features.TraceFailedRequests.Wizards.AddTraceWizard;
using Microsoft.Web.Management.Client;

namespace JexusManager.Features.TraceFailedRequests
{
    internal sealed class TraceFailedRequestsModuleProxy : ModuleServiceProxy
    {
        internal TraceFailedRequestsItem[] GetItems()
        {
            return (TraceFailedRequestsItem[])Invoke(nameof(GetItems));
        }

        internal Provider[] GetProviderDefinitions()
        {
            return (Provider[])Invoke(nameof(GetProviderDefinitions));
        }

        internal void Add(TraceFailedRequestsItem item)
        {
            Invoke(nameof(Add), item);
        }

        internal void Update(TraceFailedRequestsItem original, TraceFailedRequestsItem item)
        {
            Invoke(nameof(Update), original, item);
        }

        internal void Remove(TraceFailedRequestsItem item)
        {
            Invoke(nameof(Remove), item);
        }

        internal void MoveUp(TraceFailedRequestsItem item)
        {
            Invoke(nameof(MoveUp), item);
        }

        internal void MoveDown(TraceFailedRequestsItem item)
        {
            Invoke(nameof(MoveDown), item);
        }

        internal void Revert()
        {
            Invoke(nameof(Revert));
        }

        internal TraceFailedRequestsSettings GetSettings()
        {
            return (TraceFailedRequestsSettings)Invoke(nameof(GetSettings));
        }

        internal void ApplySettings(TraceFailedRequestsSettings settings)
        {
            Invoke(nameof(ApplySettings), settings);
        }
    }
}
