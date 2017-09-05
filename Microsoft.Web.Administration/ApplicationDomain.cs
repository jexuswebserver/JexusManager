// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Web.Administration
{
    public class ApplicationDomain : ConfigurationElement
    {
        internal ApplicationDomain(ConfigurationElement parent)
            : base(null, "applicationDomain", null, parent, null, null)
        { }

        public void Unload()
        { }
        public string Id { get; private set; }
        public int Idle { get; private set; }
        public string PhysicalPath { get; private set; }
        public string VirtualPath { get; private set; }
        public WorkerProcess WorkerProcess { get; private set; }
    }
}
