using EmmyLua.LanguageServer.Framework.Server;
using IIS.LanguageServer.Handlers;
using IIS.LanguageServer.Schema;

try
{
    Console.Error.WriteLine("IIS Language Server initializing...");

    // Initialize schema cache
    var schemaCache = new SchemaCache();

    // Create handlers (for future use)
    var completionHandler = new CompletionHandler(schemaCache);
    var hoverHandler = new HoverHandler(schemaCache);
    var textSyncHandler = new TextDocumentSyncHandler(schemaCache);

    // Create language server
    var server = LanguageServer.From(Console.OpenStandardInput(), Console.OpenStandardOutput());

    // Register initialization callbacks
    server.OnInitialize((request, info) =>
    {
        info.Name = "IIS Configuration Language Server";
        info.Version = "1.0.0";
        return Task.CompletedTask;
    });

    server.OnInitialized(request =>
    {
        Console.Error.WriteLine("IIS Language Server initialized");
        return Task.CompletedTask;
    });

    Console.Error.WriteLine("IIS Language Server started");
    await server.Run();
}
catch (Exception ex)
{
    Console.Error.WriteLine($"LSP Server error: {ex}");
}
