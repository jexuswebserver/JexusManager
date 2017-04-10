// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Web.Administration
{
    public class WebConfigurationMap
    {
        public WebConfigurationMap()
        {
        }

        public WebConfigurationMap(string machineConfigurationPath, string rootWebConfigurationPath)
        {
            MachineConfigurationPath = machineConfigurationPath;
            RootWebConfigurationPath = rootWebConfigurationPath;
        }

        public string MachineConfigurationPath { get; private set; }

        public string RootWebConfigurationPath { get; private set; }
    }
}
