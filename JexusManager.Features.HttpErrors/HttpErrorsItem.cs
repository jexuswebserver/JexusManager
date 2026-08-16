// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Features.HttpErrors
{
    using System;

    using Microsoft.Web.Administration;

    [Serializable]
    internal class HttpErrorsItem : IItem<HttpErrorsItem>
    {
        public HttpErrorsItem()
        {
            Prefix = Path = string.Empty;
            Response = "File";
            Substatus = -1;
            Flag = "Local";
        }

        public string Prefix { get; set; }

        public uint Status { get; set; }
        public int Substatus { get; set; }
        public string Path { get; set; }
        public string Response { get; set; }
        public string Flag { get; set; }

        internal string OriginalKey { get; set; }

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
    }
}
