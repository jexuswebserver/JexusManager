// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Web.Management.Client
{
    public sealed class ControlPanelCategoryInfo
    {
        public ControlPanelCategoryInfo(
            string name,
            string text,
            string description,
            ControlPanelCategorization categorization
            )
        {
            Name = name;
            Text = text;
            Description = description;
            Categorization = categorization;
        }

        public static readonly string ApplicationDevelopment;
        public static readonly string AspNet;
        public static readonly string CommonHttp;
        public static readonly string HealthAndDiagnostics;
        public static readonly string Iis;
        public static readonly string Management;
        public static readonly string Performance;
        public static readonly string Security;
        public static readonly string Server;

        public override string ToString()
        {
            return null;
        }

        public ControlPanelCategorization Categorization { get; }
        public string Description { get; }
        public string Name { get; }
        public string Text { get; }
    }
}
