// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;

namespace Microsoft.Web.Administration
{
    internal static class ValidatorRegistry
    {
        private static readonly IDictionary<string, Type> s_validators;
        public static readonly ConfigurationValidatorBase DefaultValidator = new DefaultValidator();

        static ValidatorRegistry()
        {
            s_validators = new Dictionary<string, Type>();
            foreach (var type in Assembly.GetExecutingAssembly().GetTypes())
            {
                var found = type.IsSubclassOf(typeof(ConfigurationValidatorBase));
                if (!found)
                {
                    continue;
                }

                s_validators.Add(type.Name, type);
            }
        }

        public static Type GetValidator(string name)
        {
            var key = PascalCase(name) + "Validator";
            return s_validators.ContainsKey(key) ? s_validators[key] : null;
        }

        // convert name to PascalCase
        internal static string PascalCase(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return string.Empty;
            }

            if (name.Length == 1)
            {
                return name.ToUpperInvariant();
            }

            int index = IndexOfFirstCorrectChar(name);
            return char.ToUpperInvariant(name[index]).ToString(CultureInfo.InvariantCulture) + name.Substring(index + 1);
        }

        private static int IndexOfFirstCorrectChar(string s)
        {
            int index = 0;
            while ((index < s.Length) && (s[index] == '_'))
            {
                index++;
            }

            // it's possible that we won't find one, e.g. something called "_"
            return (index == s.Length) ? 0 : index;
        }
    }
}
