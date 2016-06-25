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
            this.Flag = element == null || element.IsLocallyStored ? "Local" : "Inherited";
            this.Element = element;
            if (element == null)
            {
                Prefix = Path = string.Empty;
                Response = "File";
                Substatus = -1;
                return;
            }

            this.Status = (uint)element["statusCode"];
            this.Substatus = (int)element["subStatusCode"];
            this.Path = (string)element["path"];
            this.Prefix = (string)element["prefixLanguageFilePath"];
            this.Response = element.Schema.AttributeSchemas["responseMode"].Format(element["responseMode"]);
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
            get { return string.IsNullOrEmpty(this.Prefix) ? this.Path : string.Format("{0}\\<LANGUAGE-TAG>\\{1}", this.Prefix, this.Path); }
        }

        public string Code
        {
            get { return this.Substatus == -1 || this.Substatus == 0 ? this.Status.ToString() : string.Format("{0}.{1}", this.Status, this.Substatus); }
        }

        public bool Equals(HttpErrorsItem other)
        {
            return Match(other);
        }

        public bool Match(HttpErrorsItem other)
        {
            return other != null && other.Status == this.Status && other.Substatus == this.Substatus;
        }

        public void Apply()
        {
            this.Element["statusCode"] = this.Status;
            this.Element["subStatusCode"] = this.Substatus;
            this.Element["prefixLanguageFilePath"] = this.Prefix;
            this.Element["path"] = this.Path;
            this.Element["responseMode"] = this.Response;
        }
    }
}
