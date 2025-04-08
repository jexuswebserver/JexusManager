// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Microsoft.Extensions.Logging;
using JexusManager;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Security.Cryptography;

namespace Microsoft.Web.Administration
{
    [DebuggerDisplay("{Name}")]
    public sealed class ConfigurationAttribute
    {
        private static readonly ILogger _logger = LogHelper.GetLogger("ConfigurationAttribute");

        private object _value;
        private readonly ConfigurationElement _element;

        /// <summary>
        /// Constructs instances from config file.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="schema"></param>
        /// <param name="value"></param>
        /// <param name="element"></param>
        internal ConfigurationAttribute(string name, ConfigurationAttributeSchema schema, string value, ConfigurationElement element)
        {
            _element = element;
            Name = name;
            Schema = schema;
            IsProtected = schema?.IsEncrypted ?? false;
            var clear = Decrypt(value).ToString();
            var raw = Schema == null ? clear : Schema.Parse(clear);
            var result = TypeMatch(raw);
            IsInheritedFromDefaultValue = (Schema == null || !Schema.IsRequired)
                                                    && result.Equals(ExtractDefaultValueFromSchema());
            SetValue(raw);
            _element.InnerEntity.SetAttributeValue(Name, value);
        }

        /// <summary>
        /// Constructs instances when the external queries.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="schema"></param>
        /// <param name="element"></param>
        internal ConfigurationAttribute(string name, ConfigurationAttributeSchema schema, ConfigurationElement element)
        {
            _element = element;
            Name = name;
            Schema = schema;
            IsProtected = schema?.IsEncrypted ?? false;
            ResetValue();
        }

        private void ResetValue()
        {
            if (_element.HasSiteDefaults)
            {
                SetValue(_element.GetSiteDefaults(Name));
                return;
            }

            var clear = Decrypt(ExtractDefaultValueFromSchema());
            IsInheritedFromDefaultValue = true;
            SetValue(clear);
        }

        internal object ExtractDefaultValueFromSchema()
        {
            object defaultValue;
            if (_element.FileContext.Parent != null)
            {
                // web.config
                var parentElementInFile = _element.GetParentElement();
                defaultValue = parentElementInFile == null ? Schema?.DefaultValue : parentElementInFile.Attributes[Name].Value;
            }
            else if (_element.Section?.Location == null)
            {
                // root config
                defaultValue = Decrypt(Schema?.DefaultValue);
            }
            else
            {
                // location tags in applicationHost.config.
                var parentElement = _element.GetElementAtParentLocationInFileContext(_element.FileContext);
                defaultValue = parentElement == null ? Schema?.DefaultValue : parentElement.Attributes[Name].Value;
            }

            if (defaultValue == null)
            {
                // IMPORTANT: to set default values so that in configuration files they do not appear.
                if (Schema != null)
                {
                    if (Schema.DefaultValue == null)
                    {
                        try
                        {
                            Schema.SetDefaultValue(Schema.DefaultValueByType);
                        }
                        catch (COMException)
                        {
                            // IMPORTANT: If validators do not like default value, simply ignore.
                            return defaultValue = Schema.DefaultValueByType;
                        }

                        defaultValue = Schema.DefaultValue;
                    }
                }
            }

            return defaultValue;
        }

        public void Delete()
        {
            if (_element == null)
            {
                throw new InvalidOperationException("null element");
            }

            var innerEntity = _element.InnerEntity;
            if (innerEntity == null)
            {
                return;
            }

            innerEntity.SetAttributeValue(Name, null);
            if (!innerEntity.HasAttributes && !innerEntity.HasElements)
            {
                innerEntity.Remove();
            }

            ResetValue();
        }

        public object GetMetadata(string metadataType)
        {
            throw new NotImplementedException();
        }

        public void SetMetadata(string metadataType, object value)
        {
            throw new NotImplementedException();
        }

        public bool IsInheritedFromDefaultValue { get; internal set; }

        public bool IsProtected { get; }
        public string Name { get; internal set; }
        public ConfigurationAttributeSchema Schema { get; internal set; }

        public object Value
        {
            get
            {
                return _value;
            }

            set
            {
                SetValue(_element.SetAttributeValueInner(this.Name, value));
            }
        }

        internal void SetValue(object value)
        {
            _value = value;
        }

        internal object TypeMatch(object value)
        {
            return Schema != null ? Schema.CheckType(value) : value;
        }

        internal string Format(object value)
        {
            return this.Encrypt(this.Schema == null ? value.ToString() : this.Schema.Format(value));
        }

        private string Encrypt(string value)
        {
            if (IsProtected)
            {
                var protectedInfo = _element.FileContext.ProtectedConfiguration;

                // Get all available providers
                var providers = protectedInfo.Providers.Items;

                if (providers.Count > 0)
                {
                    string selectedProvider = null;

                    // Determine which provider to use based on the element context
                    bool isAppPoolPassword = _element.ParentElement != null &&
                                            _element.ParentElement.ElementTagName == "processModel" &&
                                            Name == "password";

                    if (isAppPoolPassword)
                    {
                        // For application pool passwords, prefer IISWASOnlyCngProvider
                        if (providers.ContainsKey("IISWASOnlyCngProvider"))
                        {
                            selectedProvider = "IISWASOnlyCngProvider";
                        }
                        else if (providers.ContainsKey("IISCngProvider"))
                        {
                            selectedProvider = "IISCngProvider";
                        }
                    }
                    else
                    {
                        // For other passwords (virtual directories, etc.), prefer IISCngProvider
                        if (providers.ContainsKey("IISCngProvider"))
                        {
                            selectedProvider = "IISCngProvider";
                        }
                        else if (providers.ContainsKey("IISWASOnlyCngProvider"))
                        {
                            selectedProvider = "IISWASOnlyCngProvider";
                        }
                    }

                    // If no preferred provider is found, fall back to any available provider
                    if (selectedProvider == null)
                    {
                        foreach (var key in providers.Keys)
                        {
                            selectedProvider = key;
                            break;
                        }
                    }

                    if (selectedProvider != null)
                    {
                        try
                        {
                            var provider = protectedInfo.GetProvider(selectedProvider, false);
                            var secret = provider.Encrypt(value);
                            return string.Format("[enc:{0}:{1}:enc]", selectedProvider, secret);
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, "Error encrypting with provider {Provider}", selectedProvider);

                            // If all encryption attempts fail, return the original value
                            return value;
                        }
                    }
                }
            }

            return value;
        }

        private object Decrypt(object value)
        {
            if (value is string && IsProtected)
            {
                var full = value.ToString();
                if (string.IsNullOrWhiteSpace(full))
                {
                    // IMPORTANT: workaround IIS 7 Express anonymous authentication default password is empty in schema issue.
                    return value;
                }

                var length = full.Length - 10;
                if (length < 0)
                {
                    return value;
                }

                var inner = full.Substring(5, length);
                var point = inner.IndexOf(':');
                if (point < 0)
                {
                    return value;
                }

                var name = inner.Substring(0, point);
                var data = inner.Substring(point + 1);
                var protectedInfo = _element.FileContext.ProtectedConfiguration;
                var provider = protectedInfo.GetProvider(name, false);
                try
                {
                    return data.Length == 0 ? data : provider.Decrypt(data);
                }
                catch (CryptographicException)
                {
                    return "**********************";
                }
                catch (DllNotFoundException)
                {
                    return "";
                }
            }

            return value;
        }
    }
}

