// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Web.Management.Server
{
    public abstract class ConfigurationModuleProvider : SimpleDelegatedModuleProvider
    {
        public override DelegationState GetChildDelegationState(string path)
        {
            return null;
        }

        public override DelegationState[] GetSupportedChildDelegationStates(string path)
        {
            return null;
        }

        public override void SetChildDelegationState(string path, DelegationState state)
        { }

        protected abstract string ConfigurationSectionName { get; }
    }
}