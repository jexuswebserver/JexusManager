using System;
using System.Collections.Generic;
using Microsoft.Web.Administration;
using Microsoft.Web.Management.Server;

namespace JexusManager.Features.ResponseHeaders
{
    internal sealed class ResponseHeadersService : ModuleService
    {
        [ModuleServiceMethod]
        public ResponseHeadersItem[] GetItems()
        {
            var result = new List<ResponseHeadersItem>();
            foreach (ConfigurationElement element in Collection())
            {
                result.Add(new ResponseHeadersItem
                {
                    Name = (string)element["name"],
                    Value = (string)element["value"],
                    Flag = element.IsLocallyStored ? "Local" : "Inherited"
                });
            }

            return result.ToArray();
        }

        [ModuleServiceMethod]
        public void Add(ResponseHeadersItem item)
        {
            AddItem(item);
            ManagementUnit.Update();
        }

        [ModuleServiceMethod]
        public void Update(ResponseHeadersItem oldItem, ResponseHeadersItem item)
        {
            var existing = Find(oldItem);
            if (existing != null)
            {
                Collection().Remove(existing);
            }

            AddItem(item);
            ManagementUnit.Update();
        }

        [ModuleServiceMethod]
        public void Remove(ResponseHeadersItem item)
        {
            var existing = Find(item) ?? throw new InvalidOperationException("Response header was not found.");
            Collection().Remove(existing);
            ManagementUnit.Update();
        }

        private ConfigurationElementCollection Collection()
        {
            return ManagementUnit.Configuration.GetSection("system.webServer/httpProtocol").GetCollection("customHeaders");
        }

        private ConfigurationElement Find(ResponseHeadersItem item)
        {
            foreach (ConfigurationElement element in Collection())
            {
                if ((string)element["name"] == item.Name)
                {
                    return element;
                }
            }

            return null;
        }

        private void AddItem(ResponseHeadersItem item)
        {
            if (item == null || string.IsNullOrWhiteSpace(item.Name))
            {
                throw new ArgumentException("A response header name is required.");
            }

            var element = Collection().CreateElement();
            element["name"] = item.Name;
            element["value"] = item.Value;
            Collection().Add(element);
        }
    }
}