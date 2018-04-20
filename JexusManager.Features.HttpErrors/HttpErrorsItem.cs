// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Features.HttpErrors
{
    using Microsoft.Web.Administration;

    internal class HttpErrorsItem : IItem<HttpErrorsItem>
    {
        public HttpErrorsItem(ConfigurationElement element)
        {
            Flag = element == null || element.IsLocallyStored ? "Local" : "Inherited";
            Element = element;
            if (element == null)
            {
                Prefix = Path = string.Empty;
                Response = "File";
                Substatus = -1;
                return;
            }

            Status = (uint)element["statusCode"];
            Substatus = (int)element["subStatusCode"];
            Path = (string)element["path"];
            Prefix = (string)element["prefixLanguageFilePath"];
            Response = element.Schema.AttributeSchemas["responseMode"].Format(element["responseMode"]);
        }

        public string Prefix { get; set; }

        public uint Status { get; set; }
        public int Substatus { get; set; }
        public string Path { get; set; }
        public string Response { get; set; }
        public string Flag { get; set; }
        public ConfigurationElement Element { get; set; }

        public string FullPath
        {
            get { return string.IsNullOrEmpty(Prefix) ? Path : string.Format("{0}\\<LANGUAGE-TAG>\\{1}", Prefix, Path); }
        }

        public string Code
        {
            get { return Substatus == -1 || Substatus == 0 ? Status.ToString() : string.Format("{0}.{1}", Status, Substatus); }
        }

        public bool Equals(HttpErrorsItem other)
        {
            return Match(other);
        }

        public bool Match(HttpErrorsItem other)
        {
            return other != null && other.Status == Status && other.Substatus == Substatus;
        }

        public void Apply()
        {
            Element["statusCode"] = Status;
            Element["subStatusCode"] = Substatus;
            Element["prefixLanguageFilePath"] = Prefix;
            Element["path"] = Path;
            Element["responseMode"] = Response;
        }
    }
}
