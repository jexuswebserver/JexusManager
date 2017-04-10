// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Web.Administration
{
    public sealed class CustomLogField : ConfigurationElement
    {
        public CustomLogField()
            : this(null, null)
        { }

        internal CustomLogField(ConfigurationElement element, ConfigurationElement parent)
            : base(element, "add", null, parent, null, null)
        {
        }

        public string LogFieldName
        {
            get { return (string)this["logFieldName"]; }
            set { this["logFieldName"] = value; }
        }

        public string SourceName
        {
            get { return (string)this["sourceName"]; }
            set { this["sourceName"] = value; }
        }

        public CustomLogFieldSourceType SourceType
        {
            get { return (CustomLogFieldSourceType)Enum.ToObject(typeof(CustomLogFieldSourceType), this["sourceType"]); }
            set { this["sourceType"] = (long)value; }
        }
    }
}
