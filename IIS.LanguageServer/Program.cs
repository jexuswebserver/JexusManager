using IIS.LanguageServer.Handlers;
using IIS.LanguageServer.Schema;
using LspServer = EmmyLua.LanguageServer.Framework.Server.LanguageServer;

try
{
    Console.Error.WriteLine("IIS Language Server initializing...");

    // Initialize schema cache
    var schemaCache = new SchemaCache();

    // Create handlers
    var textSyncHandler = new TextDocumentSyncHandler(schemaCache);
    var completionHandler = new IISCompletionHandler(schemaCache, textSyncHandler);
    var hoverHandler = new IISHoverHandler(schemaCache, textSyncHandler);
    var definitionHandler = new IISDefinitionHandler(schemaCache, textSyncHandler);

    // Create language server
    var server = LspServer.From(Console.OpenStandardInput(), Console.OpenStandardOutput());

    // Wire diagnostics handler to the server for pushing notifications
    textSyncHandler.SetServer(server);

    // Register handlers with the server
    server.AddHandler(textSyncHandler);
    server.AddHandler(completionHandler);
    server.AddHandler(hoverHandler);
    server.AddHandler(definitionHandler);

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
