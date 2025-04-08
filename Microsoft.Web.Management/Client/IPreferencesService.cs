// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Web.Management.Client
{
    public interface IPreferencesService
    {
        PreferencesStore GetPreferencesStore(
            Guid storeIdentifier
            );

        bool GetPreferencesStore(
            Guid storeIdentifier,
            out PreferencesStore store
            );

        void ResetPreferencesStore(
            Guid storeIdentifier
            );

        void Save();
    }
}
