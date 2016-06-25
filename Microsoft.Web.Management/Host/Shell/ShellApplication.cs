// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Web.Management.Host.Shell
{
    public abstract class ShellApplication : IServiceProvider
    {
        public abstract ShellComponents CreateComponents();

        public void Execute(
            bool localDevelopmentMode,
            bool resetPreferences,
            bool resetPreferencesNoLaunch
            )
        { }

        protected virtual Object GetService(
    Type serviceType
)
        {
            return null;
        }

        object IServiceProvider.GetService(Type serviceType)
        {
            throw new NotImplementedException();
        }
    }
}
