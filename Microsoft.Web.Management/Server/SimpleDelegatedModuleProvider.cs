// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Web.Management.Server
{
    public abstract class SimpleDelegatedModuleProvider : ModuleProvider
    {
        public static readonly DelegationState NoneDelegationState;
        public static readonly DelegationState ParentDelegationState;
        public static readonly DelegationState ReadOnlyDelegationState;
        public static readonly DelegationState ReadWriteDelegationState;

        public override DelegationState GetChildDelegationState(
            string path
            )
        {
            return null;
        }

        public override DelegationState[] GetSupportedChildDelegationStates(
            string path
            )
        {
            return null;
        }

        public override void SetChildDelegationState(
            string path,
            DelegationState state
            )
        {
        }

        public override bool SupportsDelegation { get; }
    }
}
