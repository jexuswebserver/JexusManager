// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Web.Administration
{
    public sealed class SiteLogFile : ConfigurationElement
    {
        private CustomLogFieldCollection _collection;
        private readonly SiteLogFile _defaultLogFile;

        internal SiteLogFile(ConfigurationElement parent)
            : this(null, parent)
        { }

        internal SiteLogFile(ConfigurationElement element, ConfigurationElement parent)
            : base(element, "logFile", null, parent, null, null)
        {
            var server = (parent as Site)?.Server;
            _defaultLogFile = server?.SiteDefaults.LogFile;
        }

        // TODO: how to read default custom fields?
        public CustomLogFieldCollection CustomLogFields => _collection ?? (_collection = Schema.ChildElementSchemas["customFields"] == null ? null : new CustomLogFieldCollection(ChildElements["customFields"], this));

        private T Get<T>(string name, Func<T> defaultGetter, Func<ConfigurationAttribute, T> mainGetter)
        {
            var attribute = GetAttribute(name);
            if (attribute.IsInheritedFromDefaultValue && _defaultLogFile != null)
            {
                return defaultGetter();
            }

            return mainGetter(attribute);
        }

        private void Set<T>(string name, T value, Func<T> defaultGetter, Action<ConfigurationAttribute> mainSetter)
        {
            var attribute = GetAttribute(name);
            if (_defaultLogFile != null && value.Equals(defaultGetter()))
            {
                attribute.Delete();
                return;
            }

            mainSetter(attribute);
        }

        public Guid CustomLogPluginClsid
        {
            get
            {
                return Get("customLogPluginClsid",
                    () => _defaultLogFile.CustomLogPluginClsid,
                    attribute =>
                    {
                        Guid guid;
                        Guid.TryParse(attribute.Value.ToString(), out guid);
                        return guid;
                    });
            }

            set
            {
                Set("customLogPluginClsid", 
                    value,
                    () => _defaultLogFile.CustomLogPluginClsid,
                    attribute => attribute.Value = value.ToString());
            }
        }

        public string Directory
        {
            get
            {
                return Get("directory",
                    () => _defaultLogFile.Directory,
                    attribute => (string) attribute.Value);
            }

            set
            {
                Set("directory",
                    value,
                    () => _defaultLogFile.Directory,
                    attribute => attribute.Value = value);
            }
        }

        public bool Enabled
        {
            get
            {
                return Get("enabled",
                    () => _defaultLogFile.Enabled,
                    attribute => (bool) attribute.Value);
            }

            set
            {
                Set("enabled",
                    value,
                    () => _defaultLogFile.Enabled,
                    attribute => attribute.Value = value);
            }
        }

        public bool LocalTimeRollover
        {
            get
            {
                return Get("localTimeRollover",
                    () => _defaultLogFile.LocalTimeRollover,
                    attribute => (bool)attribute.Value);
            }

            set
            {
                Set("localTimeRollover",
                    value,
                    () => _defaultLogFile.LocalTimeRollover,
                    attribute => attribute.Value = value);
            }
        }

        public LogExtFileFlags LogExtFileFlags
        {
            get
            {
                return Get("logExtFileFlags",
                    () => _defaultLogFile.LogExtFileFlags,
                    attribute => (LogExtFileFlags)Enum.ToObject(typeof(LogExtFileFlags), attribute.Value));
            }

            set
            {
                Set("logExtFileFlags",
                    value,
                    () => _defaultLogFile.LogExtFileFlags,
                    attribute => attribute.Value = (long)value);
            }
        }

        public LogFormat LogFormat
        {
            get
            {
                return Get("logFormat",
                    () => _defaultLogFile.LogFormat,
                    attribute => (LogFormat)Enum.ToObject(typeof(LogFormat), attribute.Value));
            }

            set
            {
                Set("logFormat",
                    value,
                    () => _defaultLogFile.LogFormat,
                    attribute => attribute.Value = (long)value);
            }
        }

        public LogTargetW3C LogTargetW3C
        {
            get
            {
                return Get("logTargetW3C",
                    () => _defaultLogFile.LogTargetW3C,
                    attribute => (LogTargetW3C)Enum.ToObject(typeof(LogTargetW3C), attribute.Value));
            }

            set
            {
                Set("logTargetW3C",
                    value,
                    () => _defaultLogFile.LogTargetW3C,
                    attribute => attribute.Value = (long)value);
            }
        }

        public LoggingRolloverPeriod Period
        {
            get
            {
                return Get("period",
                    () => _defaultLogFile.Period,
                    attribute => (LoggingRolloverPeriod)Enum.ToObject(typeof(LoggingRolloverPeriod), attribute.Value));
            }

            set
            {
                Set("period",
                    value,
                    () => _defaultLogFile.Period,
                    attribute => attribute.Value = (long)value);
            }
        }

        public long TruncateSize
        {
            get
            {
                return Get("truncateSize",
                    () => _defaultLogFile.TruncateSize,
                    attribute => (long)attribute.Value);
            }

            set
            {
                Set("truncateSize",
                    value,
                    () => _defaultLogFile.TruncateSize,
                    attribute => attribute.Value = value);
            }
        }
    }
}
