// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Diagnostics;
using System.IO;
using System.Xml;
using Microsoft.XmlDiffPatch;
using Xunit;

namespace Tests
{
    internal static class XmlAssert
    {
        internal static void Equal(string file1, string file2)
        {
            var diffFile = @"diff.xml";
            XmlTextWriter tw = new XmlTextWriter(new StreamWriter(diffFile));
            tw.Formatting = Formatting.Indented;
            var diff = new XmlDiff();
            diff.Options = XmlDiffOptions.IgnoreWhitespace | XmlDiffOptions.IgnoreComments | XmlDiffOptions.IgnoreXmlDecl;
            var result = diff.Compare(file1, file2, false, tw);
            tw.Close();
            if (!result)
            {
                //Files were not equal, so construct XmlDiffView.
                XmlDiffView dv = new XmlDiffView();

                //Load the original file again and the diff file.
                XmlTextReader orig = new XmlTextReader(file1);
                XmlTextReader diffGram = new XmlTextReader(diffFile);
                dv.Load(orig, diffGram);
                string tempFile = "test.htm";

                StreamWriter sw1 = new StreamWriter(tempFile);
                //Wrapping
                sw1.Write("<html><body><table>");
                sw1.Write("<tr><td><b>");
                sw1.Write(file1);
                sw1.Write("</b></td><td><b>");
                sw1.Write(file2);
                sw1.Write("</b></td></tr>");

                //This gets the differences but just has the 
                //rows and columns of an HTML table
                dv.GetHtml(sw1);

                //Finish wrapping up the generated HTML and 
                //complete the file by putting legend in the end just like the 
                //online tool.

                sw1.Write("<tr><td><b>Legend:</b> <font style='background-color: yellow'" +
                " color='black'>added</font>&nbsp;&nbsp;<font style='background-color: red'" +
                "color='black'>removed</font>&nbsp;&nbsp;<font style='background-color: " +
                "lightgreen' color='black'>changed</font>&nbsp;&nbsp;" +
                "<font style='background-color: red' color='blue'>moved from</font>" +
                "&nbsp;&nbsp;<font style='background-color: yellow' color='blue'>moved to" +
                "</font>&nbsp;&nbsp;<font style='background-color: white' color='#AAAAAA'>" + "ignored</font></td></tr>");

                sw1.Write("</table></body></html>");

                //HouseKeeping...close everything we dont want to lock.
                sw1.Close();
                orig.Close();
                diffGram.Close();
                Process.Start("explorer.exe", "test.htm");
            }

            Assert.True(result);
        }
    }
}
