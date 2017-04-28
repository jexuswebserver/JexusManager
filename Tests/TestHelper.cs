// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.IO;
using System.Xml.Linq;
using System.Xml.XPath;

using Microsoft.Web.Administration;

namespace Tests
{
    public static class TestHelper
    {
        internal static void FixPhysicalPathMono(string fileName)
        {
            if (!Helper.IsRunningOnMono())
            {
                return;
            }

            var file = XDocument.Load(fileName, LoadOptions.PreserveWhitespace);
            var root = file.Root;
            if (root == null)
            {
                return;
            }

            var pool =
                root.XPathSelectElement(
                    "/configuration/system.applicationHost/sites/site[@id='1']/application/virtualDirectory");
            pool.SetAttributeValue("physicalPath", "%JEXUS_TEST_HOME%/WebSite1");
            file.Save(fileName, SaveOptions.DisableFormatting);
        }

        public static string CopySiteConfig(string folder, string originalConfig)
        {
            var filePath = Path.Combine(folder, "WebSite1", "web.config");
            File.Copy(Path.Combine(folder, "WebSite1", originalConfig), filePath, true);
            return filePath;
        }

        public static void AssertSiteConfig(string folder, string expectedConfig)
        {
            XmlAssert.Equal(Path.Combine(folder, "WebSite1", expectedConfig), Path.Combine(folder, "WebSite1", "web.config"));
        }

        public static string GetSiteConfig(string folder)
        {
            return Path.Combine(folder, "WebSite1", "web.config");
        }
    }
}
