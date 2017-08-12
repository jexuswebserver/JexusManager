// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Xml.Linq;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace Microsoft.Web.Administration
{
    [DebuggerDisplay("{Name}[{Type}]")]
    public sealed class ConfigurationAttributeSchema
    {
        private ConfigurationEnumValueCollection _collection;

        private ConfigurationValidatorBase _validator;

        internal ConfigurationAttributeSchema()
        { }

        public ConfigurationEnumValueCollection GetEnumValues()
        {
            return _collection ?? (_collection = new ConfigurationEnumValueCollection());
        }

        public object GetMetadata(string metadataType)
        {
            return null;
        }

        public bool AllowInfinite { get; internal set; }
        public object DefaultValue { get; private set; }
        public bool IsCaseSensitive { get; internal set; }
        public bool IsCombinedKey { get; internal set; }
        public bool IsEncrypted { get; internal set; }
        public bool IsExpanded { get; internal set; }
        public bool IsRequired { get; internal set; }
        public bool IsUniqueKey { get; internal set; }
        public string Name { get; internal set; }
        public string TimeSpanFormat { get; internal set; }
        public string Type { get; internal set; }

        internal object Parse(string value)
        {
            if (value == null)
            {
                return null;
            }

            object result = null;

            // bool|enum|flags|uint|int|int64|string|timeSpan
            if (Type == "bool")
            {
                bool b;
                bool.TryParse(value, out b);
                result = b;
            }
            else if (Type == "uint")
            {
                uint u;
                uint.TryParse(value, out u);
                result = u;
            }
            else if (Type == "int")
            {
                int i;
                int.TryParse(value, out i);
                result = i;
            }
            else if (Type == "int64")
            {
                long l;
                long.TryParse(value, out l);
                result = l;
            }
            else if (Type == "string")
            {
                result = value;
            }
            else if (Type == "enum")
            {
                var enum1 = GetEnumValues()[value];
                if (enum1 == null)
                {
                    throw new COMException(string.Format("Enum must be one of {0}\r\n", this.GetEnumValues().FormattedString));
                }

                result = enum1.Value;
            }
            else if (Type == "flags")
            {
                if (value == "None" || value == string.Empty)
                {
                    // IMPORTANT: workaround for Ftp_Schema.xml bug on validationFlags defaultValue=string.Empty.
                    result = 0L;
                }
                else
                {
                    var parts = value.Split(',');
                    long flags = 0;
                    foreach (var part in parts)
                    {
                        var values = GetEnumValues();
                        var item = values[part.Trim()];
                        if (item == null)
                        {
                            throw new COMException(string.Format("Flags must be some combination of {0}\r\n", this.GetEnumValues().FormattedString));
                        }

                        flags |= item.Value;
                    }

                    result = flags;
                }
            }
            else if (Type == "timeSpan")
            {
                if (AllowInfinite && value == "Infinite")
                {
                    result = Timeout.InfiniteTimeSpan;
                }
                else if (TimeSpanFormat == "string")
                {
                    TimeSpan t;
                    TimeSpan.TryParse(value, out t);
                    result = t;
                }
                else if (TimeSpanFormat == "minutes")
                {
                    result = new TimeSpan(0, int.Parse(value), 0);
                }
                else if (TimeSpanFormat == "seconds")
                {
                    result = new TimeSpan(0, 0, int.Parse(value));
                }
            }

            Validate(result);

            return result;
        }

        private void Validate(object result)
        {
            _validator.Validate(result);
        }

        public string ValidationParameter { get; internal set; }
        public string ValidationType { get; internal set; }

        public string DefaultValueByType
        {
            get
            {
                // bool|enum|flags|uint|int|int64|string|timeSpan
                //if (Type == "bool")
                //{
                //    return "false";
                //}

                //if (this.Type == "uint")
                //{
                //    return "0";
                //}

                //if (this.Type == "int")
                //{
                //    return "0";
                //}

                //if (this.Type == "int64")
                //{
                //    return "0";
                //}

                if (this.Type == "string")
                {
                    if (ValidationType != "nonEmptyString")
                    {
                        // IMPORTANT: httpErrors / path
                        return string.Empty;
                    }
                }

                //if (this.Type == "enum")
                //{
                //    return "0";
                //}

                //if (this.Type == "flags")
                //{
                //    return "0";
                //}

                //if (this.Type == "timeSpan")
                //{
                //    return "0:00:00";
                //}

                return null;
            }
        }

        internal void SetEnumValues(ConfigurationEnumValueCollection enums)
        {
            _collection = enums;
        }

        internal void LoadEnums(XElement item)
        {
            var enums = new ConfigurationEnumValueCollection();
            foreach (var node in item.Nodes())
            {
                var element = node as XElement;
                if (element == null)
                {
                    continue;
                }

                enums.Add(new ConfigurationEnumValue { Name = element.Attribute("name").Value, Value = long.Parse(element.Attribute("value").Value) });
            }

            SetEnumValues(enums);
        }

        internal void SetDefaultValue(string value)
        {
            DefaultValue = Parse(value);
        }

        public void CreateValidator()
        {
            if (ValidationType == null)
            {
                _validator = ValidatorRegistry.DefaultValidator;
                return;
            }

            var type = ValidatorRegistry.GetValidator(this.ValidationType);
            _validator = (ConfigurationValidatorBase)Activator.CreateInstance(
                type,
                BindingFlags.CreateInstance | BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public,
                null,
                ValidationParameter == null ? null : new object[] { this.ValidationParameter },
                CultureInfo.InvariantCulture);
        }

        public object CheckType(object value)
        {
            var result = this.CheckRawType(value);
            Validate(result);
            return result;
        }

        private object CheckRawType(object value)
        {
            // bool|enum|flags|uint|int|int64|string|timeSpan
            if (this.Type == "bool" && value is bool)
            {
                return value;
            }

            var valueType = value.GetType();
            if (Type == "enum" && valueType.BaseType.FullName == "System.Enum")
            {
                var enumType = Enum.GetUnderlyingType(valueType);
                if (enumType == typeof(long))
                {
                    return (long)value;
                }

                if (enumType == typeof(int))
                {
                    return (long)(int)value;
                }
            }

            if (this.Type == "enum" && value is long)
            {
                return value;
            }

            if (this.Type == "enum" && value is int)
            {
                return Convert.ToInt64(value);
            }

            if (this.Type == "enum" && value is string)
            {
                return Parse((string)value);
            }

            // TODO: is this needed?
            if (this.Type == "flags" && value is long)
            {
                return value;
            }

            if (this.Type == "flags" && value is string)
            {
                return Parse((string)value);
            }

            if (this.Type == "uint" && value is uint)
            {
                return value;
            }

            if (this.Type == "uint" && value is int)
            {
                return Convert.ToUInt32(value);
            }

            // IMPORTANT: site id
            if (this.Type == "uint" && value is long)
            {
                return Convert.ToUInt32(value);
            }

            if (this.Type == "int" && value is int)
            {
                return value;
            }

            if (this.Type == "int64" && value is long)
            {
                return value;
            }

            if (this.Type == "string" && value is string)
            {
                return value;
            }

            if (this.Type == "timeSpan" && value is TimeSpan)
            {
                return value;
            }

            throw new InvalidCastException();
        }
    }
}
