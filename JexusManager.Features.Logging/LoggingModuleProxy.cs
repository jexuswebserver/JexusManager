// Copyright (c) Lex Li. All rights reserved.
//
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Web.Management.Client;

namespace JexusManager.Features.Logging
{
    public sealed class LoggingModuleProxy : ModuleServiceProxy
    {
        public LoggingSettings GetSettings()
        {
            return (LoggingSettings)Invoke(nameof(GetSettings));
        }

        public void Apply(LoggingSettings settings)
        {
            Invoke(nameof(Apply), settings);
        }

        public void SetEnabled(bool enabled)
        {
            Invoke(nameof(SetEnabled), enabled);
        }
    }
}