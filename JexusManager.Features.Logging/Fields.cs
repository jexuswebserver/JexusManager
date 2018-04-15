using Microsoft.Web.Administration;
using System.Collections.Generic;

namespace JexusManager.Features.Logging
{
    class Fields
    {
        public SiteLogFile Element { get; }
        public List<CustomLogField> CustomLogFields { get; internal set; }
        public LogExtFileFlags LogExtFileFlags { get; internal set; }


        public Fields(SiteLogFile file)
        {
            Element = file;
            CustomLogFields = new List<CustomLogField>();
            foreach (CustomLogField item in file.CustomLogFields)
            {
                CustomLogFields.Add(item);
            }

            LogExtFileFlags = file.LogExtFileFlags;
        }
    }
}
