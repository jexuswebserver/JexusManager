// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Web.Management.Server
{
    public abstract class ModuleProvider
    {
        public virtual DelegationState GetChildDelegationState(string path)
        {
            throw new NotImplementedException();
        }

        public abstract ModuleDefinition GetModuleDefinition(IManagementContext context);

        public virtual DelegationState[] GetSupportedChildDelegationStates(string path)
        { throw new NotImplementedException(); }

        public virtual void Initialize(string name)
        {
            Name = name;
        }

        public virtual void SetChildDelegationState(string path, DelegationState delegationState)
        {
        }

        public abstract bool SupportsScope(ManagementScope scope);

        public virtual string FriendlyName { get; }
        protected ManagementUnit ManagementUnit { get; }
        public string Name { get; set; }
        public abstract Type ServiceType { get; }
        public virtual bool SupportsDelegation { get; }
    }
}
