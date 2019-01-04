// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Web.Administration
{
    public sealed class SiteTraceFailedRequestsLogging : ConfigurationElement
    {
        private readonly SiteDefaultProcessor<SiteTraceFailedRequestsLogging> _processor;

        internal SiteTraceFailedRequestsLogging(ConfigurationElement element, ConfigurationElement parent)
            : base(element, "traceFailedRequestsLogging", null, parent, null, null)
        {
            var server = (parent as Site)?.Server;
            _processor = new SiteDefaultProcessor<SiteTraceFailedRequestsLogging>(server?.SiteDefaults.TraceFailedRequestsLogging);
        }

        public string Directory
        {
            get
            {
                return _processor.Get("directory",
                    () => _processor.SiteDefault.Directory,
                    attribute => (string)attribute.Value,
                    this);
            }

            set
            {
                _processor.Set("directory",
                    value,
                    () => _processor.SiteDefault.Directory,
                    attribute => attribute.Value = value,
                    this);
            }
        }

        public bool Enabled
        {
            get
            {
                return _processor.Get("enabled",
                    () => _processor.SiteDefault.Enabled,
                    attribute => (bool)attribute.Value,
                    this);
            }

            set
            {
                _processor.Set("enabled",
                    value,
                    () => _processor.SiteDefault.Enabled,
                    attribute => attribute.Value = value,
                    this);
            }
        }

        public long MaxLogFiles
        {
            get
            {
                return _processor.Get("maxLogFiles",
                    () => _processor.SiteDefault.MaxLogFiles,
                    attribute => Convert.ToInt64(attribute.Value),
                    this);
            }

            set
            {
                _processor.Set("maxLogFiles",
                    value,
                    () => _processor.SiteDefault.MaxLogFiles,
                    attribute => attribute.Value = Convert.ToUInt32(value),
                    this);
            }
        }
    }
}
