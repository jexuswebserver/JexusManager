using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Web.Administration.Properties;

namespace IIS.LanguageServer.Schema;

public class SchemaLoader
{
    public static List<string> FindSchemaFiles()
    {
        var files = new List<string>();

        // Primary location: IIS Express (x64)
        var iisExpressPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles),
            "IIS Express", "config", "schema");

        if (Directory.Exists(iisExpressPath))
        {
            var foundFiles = Directory.GetFiles(iisExpressPath, "*_schema.xml");
            Console.Error.WriteLine($"[IIS LS] Found {foundFiles.Length} schemas in: {iisExpressPath}");
            files.AddRange(foundFiles);
        }

        // Fallback 1: IIS Express (x86)
        var iisExpressX86Path = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86),
            "IIS Express", "config", "schema");

        if (Directory.Exists(iisExpressX86Path))
        {
            var x86Files = Directory.GetFiles(iisExpressX86Path, "*_schema.xml");
            var newFiles = x86Files.Where(f => !files.Contains(f)).ToList();
            if (newFiles.Count > 0)
            {
                Console.Error.WriteLine($"[IIS LS] Found {newFiles.Count} additional schemas in: {iisExpressX86Path}");
                files.AddRange(newFiles);
            }
        }

        // Fallback 2: Full IIS (System32)
        var iisPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.System),
            "inetsrv", "config", "schema");

        if (Directory.Exists(iisPath))
        {
            var iisFiles = Directory.GetFiles(iisPath, "*_schema.xml");
            var newFiles = iisFiles.Where(f => !files.Contains(f)).ToList();
            if (newFiles.Count > 0)
            {
                Console.Error.WriteLine($"[IIS LS] Found {newFiles.Count} additional schemas in: {iisPath}");
                files.AddRange(newFiles);
            }
        }

        return files;
    }

    // Writes embedded schemas from Microsoft.Web.Administration resources to temp files
    // and returns their paths. Used when no IIS installation is found on disk.
    public static List<string> GetEmbeddedSchemaFiles()
    {
        var tempDir = Path.Combine(Path.GetTempPath(), "iis-language-server-schemas");
        Directory.CreateDirectory(tempDir);

        var files = new List<string>();
        var embedded = new[]
        {
            ("IIS_schema.xml", Resources.IIS_schema),
            ("FX_schema.xml", Resources.FX_schema),
            ("rewrite_schema.xml", Resources.rewrite_schema)
        };

        foreach (var (name, content) in embedded)
        {
            var path = Path.Combine(tempDir, name);
            File.WriteAllText(path, content);
            files.Add(path);
        }

        Console.Error.WriteLine($"[IIS LS] Using {files.Count} embedded schemas from resources");
        return files;
    }
}
