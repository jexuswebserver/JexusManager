// Copyright (c) Lex Li. All rights reserved.
//
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.Web.Administration;
using Microsoft.Web.Management.Server;

namespace JexusManager.Features.Jexus
{
    internal sealed class JexusService : ModuleService
    {
        [ModuleServiceMethod]
        public JexusSettings GetSettings()
        {
            if (ManagementUnit.ServerManager.Mode != WorkingMode.Jexus)
            {
                return new JexusSettings { IsAvailable = false, Contents = string.Empty };
            }

            var text = new StringBuilder();
            foreach (var pair in ManagementUnit.ServerManager.GetExtra())
            {
                foreach (var value in pair.Value)
                {
                    text.AppendFormat("{0}={1}", pair.Key, value).AppendLine();
                }
            }

            return new JexusSettings { IsAvailable = true, Contents = text.ToString() };
        }

        [ModuleServiceMethod]
        public void Apply(string contents)
        {
            if (ManagementUnit.ServerManager.Mode != WorkingMode.Jexus)
            {
                throw new InvalidOperationException("Jexus-specific settings are unavailable for this server.");
            }

            var settings = ManagementUnit.ServerManager.GetExtra();
            settings.Clear();
            using var reader = new StringReader(contents ?? string.Empty);
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                var separator = line.IndexOf('=');
                if (separator < 0)
                {
                    continue;
                }

                var key = line.Substring(0, separator).Trim();
                if (key.Length == 0)
                {
                    continue;
                }

                var value = line.Substring(separator + 1).Trim();
                if (!settings.TryGetValue(key, out List<string> values))
                {
                    values = new List<string>();
                    settings.Add(key, values);
                }

                values.Add(value);
            }

            ManagementUnit.Update();
        }
    }
}