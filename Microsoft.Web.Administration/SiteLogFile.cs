// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Web.Administration
{

    public sealed class SiteLogFile : ConfigurationElement
    {
        private CustomLogFieldCollection _collection;
        private readonly SiteDefaultProcessor<SiteLogFile> _processor;

        internal SiteLogFile(ConfigurationElement parent)
            : this(null, parent)
        { }

        internal SiteLogFile(ConfigurationElement element, ConfigurationElement parent)
            : base(element, "logFile", null, parent, null, null)
        {
            var server = (parent as Site)?.Server;
            var defaults = server?.SiteDefaults.LogFile;
            _processor = new SiteDefaultProcessor<SiteLogFile>(defaults);
            SetSiteDefaults(defaults);
            element.SetSiteDefaults(defaults);
        }

        // TODO: how to read default custom fields?
        public CustomLogFieldCollection CustomLogFields => _collection ?? (_collection = Schema.ChildElementSchemas["customFields"] == null ? null : new CustomLogFieldCollection(ChildElements["customFields"], this));

        public Guid CustomLogPluginClsid
        {
            get
            {
                return _processor.Get("customLogPluginClsid",
                    () => _processor.SiteDefault.CustomLogPluginClsid,
                    attribute =>
                    {
                        Guid guid;
                        Guid.TryParse(attribute.Value.ToString(), out guid);
                        return guid;
                    }, 
                    this);
            }

            set
            {
                _processor.Set("customLogPluginClsid", 
                    value,
                    () => _processor.SiteDefault.CustomLogPluginClsid,
                    attribute => attribute.Value = value.ToString(),
                    this);
            }
        }

        public string Directory
        {
            get
            {
                return _processor.Get("directory",
                    () => _processor.SiteDefault.Directory,
                    attribute => (string) attribute.Value,
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
                    attribute => (bool) attribute.Value,
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

        public bool LocalTimeRollover
        {
            get
            {
                return _processor.Get("localTimeRollover",
                    () => _processor.SiteDefault.LocalTimeRollover,
                    attribute => (bool)attribute.Value,
                    this);
            }

            set
            {
                _processor.Set("localTimeRollover",
                    value,
                    () => _processor.SiteDefault.LocalTimeRollover,
                    attribute => attribute.Value = value,
                    this);
            }
        }

        public LogExtFileFlags LogExtFileFlags
        {
            get
            {
                return _processor.Get("logExtFileFlags",
                    () => _processor.SiteDefault.LogExtFileFlags,
                    attribute => (LogExtFileFlags)Enum.ToObject(typeof(LogExtFileFlags), attribute.Value),
                    this);
            }

            set
            {
                _processor.Set("logExtFileFlags",
                    value,
                    () => _processor.SiteDefault.LogExtFileFlags,
                    attribute => attribute.Value = (long)value,
                    this);
            }
        }

        public LogFormat LogFormat
        {
            get
            {
                return _processor.Get("logFormat",
                    () => _processor.SiteDefault.LogFormat,
                    attribute => (LogFormat)Enum.ToObject(typeof(LogFormat), attribute.Value),
                    this);
            }

            set
            {
                _processor.Set("logFormat",
                    value,
                    () => _processor.SiteDefault.LogFormat,
                    attribute => attribute.Value = (long)value,
                    this);
            }
        }

        public LogTargetW3C LogTargetW3C
        {
            get
            {
                return _processor.Get("logTargetW3C",
                    () => _processor.SiteDefault.LogTargetW3C,
                    attribute => (LogTargetW3C)Enum.ToObject(typeof(LogTargetW3C), attribute.Value),
                    this);
            }

            set
            {
                _processor.Set("logTargetW3C",
                    value,
                    () => _processor.SiteDefault.LogTargetW3C,
                    attribute => attribute.Value = (long)value,
                    this);
            }
        }

        public LoggingRolloverPeriod Period
        {
            get
            {
                return _processor.Get("period",
                    () => _processor.SiteDefault.Period,
                    attribute => (LoggingRolloverPeriod)Enum.ToObject(typeof(LoggingRolloverPeriod), attribute.Value),
                    this);
            }

            set
            {
                _processor.Set("period",
                    value,
                    () => _processor.SiteDefault.Period,
                    attribute => attribute.Value = (long)value,
                    this);
            }
        }

        public long TruncateSize
        {
            get
            {
                return _processor.Get("truncateSize",
                    () => _processor.SiteDefault.TruncateSize,
                    attribute => (long)attribute.Value,
                    this);
            }

            set
            {
                _processor.Set("truncateSize",
                    value,
                    () => _processor.SiteDefault.TruncateSize,
                    attribute => attribute.Value = value,
                    this);
            }
        }
    }
}
