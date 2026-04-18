using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace IIS.LanguageServer.Schema;

public class SchemaLoader
{
    public static List<string> FindSchemaFiles()
    {
        var files = new List<string>();

        // Primary location: IIS Express
        var iisExpressPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles),
            "IIS Express",
            "config",
            "schema");

        if (Directory.Exists(iisExpressPath))
        {
            files.AddRange(Directory.GetFiles(iisExpressPath, "*_schema.xml"));
        }

        // Fallback 1: IIS Express x86
        var iisExpressX86Path = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86),
            "IIS Express",
            "config",
            "schema");

        if (Directory.Exists(iisExpressX86Path))
        {
            var x86Files = Directory.GetFiles(iisExpressX86Path, "*_schema.xml");
            files.AddRange(x86Files.Where(f => !files.Contains(f)));
        }

        // Fallback 2: Full IIS (System32)
        var iisPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.System),
            "inetsrv",
            "config",
            "schema");

        if (Directory.Exists(iisPath))
        {
            var iisFiles = Directory.GetFiles(iisPath, "*_schema.xml");
            files.AddRange(iisFiles.Where(f => !files.Contains(f)));
        }

        return files;
    }

    public static Dictionary<string, XElement> LoadAllSchemas()
    {
        var schemas = new Dictionary<string, XElement>();
        var schemaFiles = FindSchemaFiles();

        foreach (var file in schemaFiles)
        {
            try
            {
                var doc = XDocument.Load(file);
                if (doc.Root != null)
                {
                    foreach (var node in doc.Root.Nodes())
                    {
                        if (node is XElement element && element.Name.LocalName == "sectionSchema")
                        {
                            var name = element.Attribute("name")?.Value;
                            if (!string.IsNullOrEmpty(name) && !schemas.ContainsKey(name))
                            {
                                schemas[name] = element;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to load schema file {file}: {ex.Message}");
            }
        }

        return schemas;
    }
}
