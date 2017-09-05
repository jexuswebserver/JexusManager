// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Text;

namespace Microsoft.Web.Administration
{
    internal static class ConfigurationAttributeSchemaExtensions
    {
        public static string Format(this ConfigurationAttributeSchema schema, object value)
        {
            if (value == null)
            {
                return null;
            }

            if (schema.Type == "enum")
            {
                return schema.GetEnumValues().GetName((long)value);
            }

            if (schema.Type == "flags")
            {
                var longValue = (long)value;
                var result = new StringBuilder();
                foreach (ConfigurationEnumValue item in schema.GetEnumValues())
                {
                    if (item.Value == 0)
                    {
                        if (longValue == 0)
                        {
                            return item.Name;
                        }

                        continue;
                    }

                    if ((longValue & item.Value) == item.Value)
                    {
                        result.AppendFormat("{0},", item.Name);
                    }
                }

                if (result[result.Length - 1] == ',')
                {
                    result.Length--;
                }

                return result.ToString();
            }

            if (schema.Type == "bool")
            {
                return (bool)value ? "true" : "false";
            }

            return value.ToString();
        }
    }
}
