using System;
using System.Collections.Generic;
using Microsoft.Web.Administration;
using Microsoft.Web.Management.Server;

namespace JexusManager.Features.Authorization
{
    internal sealed class AuthorizationService : ModuleService
    {
        private const string SectionPath = "system.webServer/security/authorization";

        [ModuleServiceMethod]
        public AuthorizationRule[] GetRules()
        {
            var rules = new List<AuthorizationRule>();
            foreach (ConfigurationElement element in GetCollection()) rules.Add(CreateRule(element));
            return rules.ToArray();
        }

        [ModuleServiceMethod]
        public void Add(AuthorizationRule rule)
        {
            AddRule(GetCollection(), rule);
            ManagementUnit.Update();
        }

        [ModuleServiceMethod]
        public void Update(AuthorizationRule original, AuthorizationRule rule)
        {
            if (original == null || rule == null) throw new ArgumentNullException(original == null ? nameof(original) : nameof(rule));
            var collection = GetCollection();
            var existing = Find(collection, original);
            if (existing == null) throw new InvalidOperationException("Authorization rule was not found.");
            if (existing.IsLocallyStored)
            {
                ApplyRule(existing, rule);
            }
            else
            {
                collection.Remove(existing);
                AddRule(collection, rule);
            }

            ManagementUnit.Update();
        }

        [ModuleServiceMethod]
        public void Remove(AuthorizationRule rule)
        {
            if (rule == null) throw new ArgumentNullException(nameof(rule));
            var collection = GetCollection();
            var existing = Find(collection, rule);
            if (existing == null) throw new InvalidOperationException("Authorization rule was not found.");
            collection.Remove(existing);
            ManagementUnit.Update();
        }

        private ConfigurationElementCollection GetCollection() => ManagementUnit.Configuration.GetSection(SectionPath).GetCollection();
        private static AuthorizationRule CreateRule(ConfigurationElement e) => new AuthorizationRule { OriginalKey = (string)e["users"] + "|" + (string)e["roles"] + "|" + (string)e["verbs"], AccessType = (long)e["accessType"], Users = (string)e["users"], Roles = (string)e["roles"], Verbs = (string)e["verbs"], Flag = e.IsLocallyStored ? "Local" : "Inhertied" };
        private static ConfigurationElement Find(ConfigurationElementCollection collection, AuthorizationRule rule)
        {
            var key = string.IsNullOrEmpty(rule.OriginalKey) ? rule.Users + "|" + rule.Roles + "|" + rule.Verbs : rule.OriginalKey;
            foreach (ConfigurationElement e in collection) if ((string)e["users"] + "|" + (string)e["roles"] + "|" + (string)e["verbs"] == key) return e;
            return null;
        }
        private static void AddRule(ConfigurationElementCollection collection, AuthorizationRule rule)
        {
            if (rule == null) throw new ArgumentNullException(nameof(rule));
            var e = collection.CreateElement();
            ApplyRule(e, rule);
            collection.Add(e);
        }

        private static void ApplyRule(ConfigurationElement e, AuthorizationRule rule)
        {
            e["accessType"] = rule.AccessType; e["users"] = rule.Users; e["roles"] = rule.Roles; e["verbs"] = rule.Verbs;
        }
    }
}
