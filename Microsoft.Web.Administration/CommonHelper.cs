// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Microsoft.Web.Administration
{
    internal static class CommonHelper
    {
        internal static bool GetIsSni(this Binding binding)
        {
            var value = binding["sslFlags"];
            return ((uint)value & 1U) == 1U;
        }

        internal static List<string> Load(this SortedDictionary<string, List<string>> variables, List<string> defaultValue, params string[] names)
        {
            foreach (var name in names)
            {
                if (variables.ContainsKey(name))
                {
                    var result = variables[name];
                    variables.Remove(name);
                    return result;
                }
            }

            return defaultValue;
        }

        internal static string ToPath(this string fileName, out string applicationName)
        {
            // TODO: merge the code
            var appName = new StringBuilder();
            var sections = fileName.Split('_');
            for (int i = 1; i < sections.Length; i++)
            {
                appName.AppendFormat("/{0}", sections[i]);
            }

            applicationName = sections.LastOrDefault();
            return appName.ToString();
        }

        internal static string PathToName(this string path)
        {
            var last = path.LastIndexOf('/');
            return path.Substring(last + 1);
        }

        internal static string ExtractName(this string hostname)
        {
            if (hostname == null)
            {
                // IMPORTANT: for IIS Express.
                return "localhost";
            }

            var index = hostname.IndexOf(':');
            return index >= 0 ? hostname.Substring(0, index) : hostname;
        }

        internal static string ExtractUser(this string credential)
        {
            var index = credential.IndexOf('|');
            return index >= 0 ? credential.Substring(0, index) : credential;
        }

        internal static string GetPath(this XElement element)
        {
            var result = new StringBuilder(element.Name.LocalName);
            var parent = element.Parent;
            while (parent?.Name.LocalName != "configuration" && parent?.Name.LocalName != "location")
            {
                result.Insert(0, parent?.Name.LocalName + "/");
                parent = parent?.Parent;
            }

            return result.ToString();
        }

        internal static bool LoadBoolean(this XAttribute attribute, bool defaultValue)
        {
            return attribute == null ? defaultValue : bool.Parse(attribute.Value);
        }

        internal static string LoadString(this XAttribute attribute, string defaultValue)
        {
            return attribute?.Value ?? defaultValue;
        }

        internal static string ObjectToString(this object setting)
        {
            return setting?.ToString() ?? string.Empty;
        }

        internal static string PathToSite(this VirtualDirectory virtualDirectory)
        {
            return virtualDirectory.Application.IsRoot()
                ? virtualDirectory.Path
                : virtualDirectory.Application.Path + virtualDirectory.Path;
        }

        internal static string LocationPath(this VirtualDirectory virtualDirectory)
        {
            return virtualDirectory.Application.Site.Name + virtualDirectory.PathToSite();
        }

        internal static string LocationPath(this Application application)
        {
            return application.IsRoot() ? application.Site.Name : application.Site.Name + application.Path;
        }

        internal static bool IsJexus(ServerManager server, Application application)
        {
            if (server != null)
            {
                return server.Mode == WorkingMode.Jexus;
            }

            if (application == null)
            {
                throw new ArgumentException("both server and applicatioin null");
            }

            return application.Server.Mode == WorkingMode.Jexus;
        }

        internal static string GetParentPath(this string rootPath)
        {
            if (rootPath == null)
            {
                throw new InvalidOperationException("invalid app");
            }

            var parentEnd = rootPath.LastIndexOf('/');
            if (parentEnd == -1)
            {
                return rootPath.Length == 0 ? null : string.Empty;
            }

            var parentPath = parentEnd == 0 ? Application.RootPath : rootPath.Substring(0, parentEnd);
            return parentPath;
        }

        internal static string ToString(ObjectState state)
        {
            switch (state)
            {
                case ObjectState.Starting:
                    return "Starting";
                case ObjectState.Started:
                    return "Started";
                case ObjectState.Stopping:
                    return "Stopping";
                case ObjectState.Stopped:
                    return "Stopped";
                case ObjectState.Unknown:
                    return "Unknown";
                default:
                    throw new ArgumentOutOfRangeException(nameof(state), state, null);
            }
        }

        internal static string ToString(ManagedPipelineMode managedPipelineMode)
        {
            switch (managedPipelineMode)
            {
                case ManagedPipelineMode.Integrated:
                    return "Integrated";
                case ManagedPipelineMode.Classic:
                    return "Classic";
                default:
                    throw new ArgumentOutOfRangeException(nameof(managedPipelineMode), managedPipelineMode, null);
            }
        }
    }
}
