// Copyright (c) Lex Li. All rights reserved.
//
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using Microsoft.Web.Administration;
using Microsoft.Web.Management.Server;

namespace JexusManager.Features.Authentication
{
    internal sealed class AuthenticationService : ModuleService
    {
        private const string AnonymousSectionPath = "system.webServer/security/authentication/anonymousAuthentication";
        private const string BasicSectionPath = "system.webServer/security/authentication/basicAuthentication";
        private const string ClientCertificateSectionPath = "system.webServer/security/authentication/clientCertificateMappingAuthentication";
        private const string DigestSectionPath = "system.webServer/security/authentication/digestAuthentication";
        private const string FormsSectionPath = "system.web/authentication";
        private const string ImpersonationSectionPath = "system.web/identity";
        private const string WindowsSectionPath = "system.webServer/security/authentication/windowsAuthentication";

        [ModuleServiceMethod]
        public bool GetAnonymousEnabled()
        {
            return (bool)GetAnonymousSection()["enabled"];
        }

        [ModuleServiceMethod]
        public void SetAnonymousEnabled(bool enabled)
        {
            GetAnonymousSection()["enabled"] = enabled;
            ManagementUnit.Update();
        }

        [ModuleServiceMethod]
        public AnonymousItem GetAnonymousSettings()
        {
            var section = GetAnonymousSection();
            return new AnonymousItem
            {
                Name = (string)section["userName"],
                Password = (string)section["password"]
            };
        }

        [ModuleServiceMethod]
        public void ApplyAnonymous(AnonymousItem settings)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            var section = GetAnonymousSection();
            section["userName"] = settings.Name;
            section["password"] = settings.Password;
            ManagementUnit.Update();
        }

        [ModuleServiceMethod]
        public bool GetBasicEnabled()
        {
            return (bool)GetSection(BasicSectionPath)["enabled"];
        }

        [ModuleServiceMethod]
        public void SetBasicEnabled(bool enabled)
        {
            GetSection(BasicSectionPath)["enabled"] = enabled;
            ManagementUnit.Update();
        }

        [ModuleServiceMethod]
        public BasicItem GetBasicSettings()
        {
            var section = GetSection(BasicSectionPath);
            return new BasicItem
            {
                Domain = (string)section["defaultLogonDomain"],
                Realm = (string)section["realm"]
            };
        }

        [ModuleServiceMethod]
        public void ApplyBasic(BasicItem settings)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            var section = GetSection(BasicSectionPath);
            section["defaultLogonDomain"] = settings.Domain;
            section["realm"] = settings.Realm;
            ManagementUnit.Update();
        }

        [ModuleServiceMethod]
        public bool GetClientCertificateEnabled()
        {
            return (bool)GetSection(ClientCertificateSectionPath)["enabled"];
        }

        [ModuleServiceMethod]
        public void SetClientCertificateEnabled(bool enabled)
        {
            GetSection(ClientCertificateSectionPath)["enabled"] = enabled;
            ManagementUnit.Update();
        }

        [ModuleServiceMethod]
        public bool GetDigestEnabled()
        {
            return (bool)GetSection(DigestSectionPath)["enabled"];
        }

        [ModuleServiceMethod]
        public void SetDigestEnabled(bool enabled)
        {
            GetSection(DigestSectionPath)["enabled"] = enabled;
            ManagementUnit.Update();
        }

        [ModuleServiceMethod]
        public DigestItem GetDigestSettings()
        {
            return new DigestItem { Realm = (string)GetSection(DigestSectionPath)["realm"] };
        }

        [ModuleServiceMethod]
        public void ApplyDigest(DigestItem settings)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            GetSection(DigestSectionPath)["realm"] = settings.Realm;
            ManagementUnit.Update();
        }

        [ModuleServiceMethod]
        public bool GetFormsEnabled()
        {
            return (long)GetSection(FormsSectionPath)["mode"] == 3L;
        }

        [ModuleServiceMethod]
        public void SetFormsEnabled(bool enabled)
        {
            GetSection(FormsSectionPath)["mode"] = enabled ? "Forms" : "Windows";
            ManagementUnit.Update();
        }

        [ModuleServiceMethod]
        public FormsItem GetFormsSettings()
        {
            var forms = GetSection(FormsSectionPath).GetChildElement("forms");
            return new FormsItem
            {
                LoginUrl = (string)forms["loginUrl"],
                Timeout = (TimeSpan)forms["timeout"],
                Mode = (long)forms["cookieless"],
                Name = (string)forms["name"],
                ProtectedMode = (long)forms["protection"],
                RequireSsl = (bool)forms["requireSSL"],
                SlidinngExpiration = (bool)forms["slidingExpiration"]
            };
        }

        [ModuleServiceMethod]
        public void ApplyForms(FormsItem settings)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            var forms = GetSection(FormsSectionPath).GetChildElement("forms");
            forms["loginUrl"] = settings.LoginUrl;
            forms["timeout"] = settings.Timeout;
            forms["cookieless"] = settings.Mode;
            forms["name"] = settings.Name;
            forms["protection"] = settings.ProtectedMode;
            forms["requireSSL"] = settings.RequireSsl;
            forms["slidingExpiration"] = settings.SlidinngExpiration;
            ManagementUnit.Update();
        }

        [ModuleServiceMethod]
        public bool GetImpersonationEnabled()
        {
            return (bool)GetSection(ImpersonationSectionPath)["impersonate"];
        }

        [ModuleServiceMethod]
        public void SetImpersonationEnabled(bool enabled)
        {
            GetSection(ImpersonationSectionPath)["impersonate"] = enabled;
            ManagementUnit.Update();
        }

        [ModuleServiceMethod]
        public ImpersonationItem GetImpersonationSettings()
        {
            var section = GetSection(ImpersonationSectionPath);
            return new ImpersonationItem
            {
                Name = (string)section["userName"],
                Password = (string)section["password"]
            };
        }

        [ModuleServiceMethod]
        public void ApplyImpersonation(ImpersonationItem settings)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            var section = GetSection(ImpersonationSectionPath);
            section["userName"] = settings.Name;
            section["password"] = settings.Password;
            ManagementUnit.Update();
        }

        [ModuleServiceMethod]
        public bool GetWindowsEnabled()
        {
            return (bool)GetSection(WindowsSectionPath)["enabled"];
        }

        [ModuleServiceMethod]
        public void SetWindowsEnabled(bool enabled)
        {
            GetSection(WindowsSectionPath)["enabled"] = enabled;
            ManagementUnit.Update();
        }

        [ModuleServiceMethod]
        public WindowsItem GetWindowsSettings()
        {
            var section = GetSection(WindowsSectionPath);
            var providers = section.GetCollection("providers");
            var result = new WindowsItem
            {
                TokenChecking = Convert.ToInt32((long)section.ChildElements["extendedProtection"]["tokenChecking"]),
                UseKernelMode = (bool)section["useKernelMode"]
            };

            foreach (ConfigurationElement provider in providers)
            {
                result.Providers.Add(new ProviderItem { Value = (string)provider["value"] });
            }

            return result;
        }

        [ModuleServiceMethod]
        public void ApplyWindows(WindowsItem settings)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            var section = GetSection(WindowsSectionPath);
            section["useKernelMode"] = settings.UseKernelMode;
            section.ChildElements["extendedProtection"]["tokenChecking"] = (long)settings.TokenChecking;
            var providers = section.GetCollection("providers");
            providers.Clear();
            foreach (var item in settings.Providers ?? new List<ProviderItem>())
            {
                if (string.IsNullOrWhiteSpace(item?.Value))
                {
                    throw new ArgumentException("A Windows Authentication provider value is required.", nameof(settings));
                }

                var provider = providers.CreateElement();
                provider["value"] = item.Value;
                providers.Add(provider);
            }

            ManagementUnit.Update();
        }

        private ConfigurationSection GetAnonymousSection()
        {
            return GetSection(AnonymousSectionPath);
        }

        private ConfigurationSection GetSection(string sectionPath)
        {
            if (ManagementUnit.Scope == ManagementScope.Server)
            {
                return ManagementUnit.Configuration.GetSection(sectionPath);
            }

            var locationPath = ManagementUnit.ConfigurationPath.GetEffectiveConfigurationPath(ManagementUnit.Scope);
            return ManagementUnit.ServerManager.GetApplicationHostConfiguration().GetSection(sectionPath, locationPath);
        }
    }
}