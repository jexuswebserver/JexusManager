// Copyright (c) Lex Li. All rights reserved.
//
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using Microsoft.Web.Administration;
using Microsoft.Web.Management.Server;

namespace JexusManager.Features.FastCgi
{
    internal sealed class FastCgiService : ModuleService
    {
        [ModuleServiceMethod]
        public FastCgiItem[] GetApplications()
        {
            var result = new List<FastCgiItem>();
            var collection = ManagementUnit.ServerManager.GetApplicationHostConfiguration().GetSection("system.webServer/fastCgi").GetCollection();
            foreach (ConfigurationElement element in collection)
            {
                result.Add(CreateItem(element));
            }

            return result.ToArray();
        }

        [ModuleServiceMethod]
        public void Add(FastCgiItem item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            var collection = GetCollection();
            var element = collection.CreateElement();
            ApplyItem(element, item);
            collection.Add(element);
            ManagementUnit.Update();
        }

        [ModuleServiceMethod]
        public void Update(FastCgiItem original, FastCgiItem item)
        {
            if (original == null || item == null)
            {
                throw new ArgumentNullException(original == null ? nameof(original) : nameof(item));
            }

            var collection = GetCollection();
            var existing = Find(collection, original);
            if (existing == null)
            {
                throw new InvalidOperationException("FastCGI application was not found.");
            }

            if (existing.IsLocallyStored)
            {
                ApplyItem(existing, item);
            }
            else
            {
                collection.Remove(existing);
                var element = collection.CreateElement();
                ApplyItem(element, item);
                collection.Add(element);
            }

            ManagementUnit.Update();
        }

        [ModuleServiceMethod]
        public void Remove(FastCgiItem item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            var collection = GetCollection();
            var existing = Find(collection, item);
            if (existing == null)
            {
                throw new InvalidOperationException("FastCGI application was not found.");
            }

            collection.Remove(existing);
            ManagementUnit.Update();
        }

        private ConfigurationElementCollection GetCollection()
        {
            return ManagementUnit.ServerManager.GetApplicationHostConfiguration().GetSection("system.webServer/fastCgi").GetCollection();
        }

        private static FastCgiItem CreateItem(ConfigurationElement element)
        {
            var item = new FastCgiItem
            {
                OriginalKey = (string)element["fullPath"] + "|" + (string)element["arguments"],
                Path = (string)element["fullPath"],
                Arguments = (string)element["arguments"],
                MonitorChangesTo = (string)element["monitorChangesTo"],
                ErrorMode = (ErrorMode)element["stderrMode"],
                MaxInstances = (uint)element["maxInstances"],
                IdleTimeout = (uint)element["idleTimeout"],
                ActivityTimeout = (uint)element["activityTimeout"],
                RequestTimeout = (uint)element["requestTimeout"],
                InstanceMaxRequests = (uint)element["instanceMaxRequests"],
                SignalBeforeTerminateSeconds = (uint)element["signalBeforeTerminateSeconds"],
                QueueLength = (uint)element["queueLength"],
                RapidFailsPerMinute = (uint)element["rapidFailsPerMinute"],
                Flag = element.IsLocallyStored ? "Local" : "Inhertied"
            };
            item.AdvancedSettings.Protocol = (Protocol)element["protocol"];
            item.AdvancedSettings.FlushNamedPipe = (bool)element["flushNamedPipe"];

            foreach (ConfigurationElement child in element.GetCollection("environmentVariables"))
            {
                item.EnvironmentVariables.Add(
                    new EnvironmentVariables { Name = (string)child["name"], Value = (string)child["value"] });
            }

            return item;
        }

        private static ConfigurationElement Find(ConfigurationElementCollection collection, FastCgiItem item)
        {
            var key = string.IsNullOrEmpty(item.OriginalKey) ? item.Path + "|" + item.Arguments : item.OriginalKey;
            foreach (ConfigurationElement element in collection)
            {
                if ((string)element["fullPath"] + "|" + (string)element["arguments"] == key)
                {
                    return element;
                }
            }

            return null;
        }

        private static void ApplyItem(ConfigurationElement element, FastCgiItem item)
        {
            element["fullPath"] = item.Path;
            element["arguments"] = item.Arguments;
            element["monitorChangesTo"] = item.MonitorChangesTo;
            element["stderrMode"] = item.ErrorMode;
            element["maxInstances"] = item.MaxInstances;
            element["idleTimeout"] = item.IdleTimeout;
            element["activityTimeout"] = item.ActivityTimeout;
            element["requestTimeout"] = item.RequestTimeout;
            element["instanceMaxRequests"] = item.InstanceMaxRequests;
            element["signalBeforeTerminateSeconds"] = item.SignalBeforeTerminateSeconds;
            element["protocol"] = item.AdvancedSettings.Protocol;
            element["queueLength"] = item.QueueLength;
            element["flushNamedPipe"] = item.AdvancedSettings.FlushNamedPipe;
            element["rapidFailsPerMinute"] = item.RapidFailsPerMinute;

            var collection = element.GetCollection("environmentVariables");
            collection.Clear();
            foreach (EnvironmentVariables environmentVariable in item.EnvironmentVariables)
            {
                var newElement = collection.CreateElement();
                newElement["name"] = environmentVariable.Name;
                newElement["value"] = environmentVariable.Value;
                collection.Add(newElement);
            }
        }
    }
}
