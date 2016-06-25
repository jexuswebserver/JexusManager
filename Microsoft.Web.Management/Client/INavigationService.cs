// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.ObjectModel;
using Microsoft.Web.Management.Server;

namespace Microsoft.Web.Management.Client
{
    public interface INavigationService
    {
        bool Navigate(Connection connection, ManagementConfigurationPath configurationPath, Type pageType, object navigationData);

        bool NavigateBack(int steps);

        bool NavigateForward();

        bool CanNavigateBack { get; }

        bool CanNavigateForward { get; }

        NavigationItem CurrentItem { get; }

        ReadOnlyCollection<NavigationItem> History { get; }


        event NavigationEventHandler NavigationPerformed;
    }
}