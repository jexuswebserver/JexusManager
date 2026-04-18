using IIS.LanguageServer.Schema;

namespace IIS.LanguageServer.Handlers;

public class HoverHandler
{
    private readonly SchemaCache _schemaCache;

    public HoverHandler(SchemaCache schemaCache)
    {
        _schemaCache = schemaCache;
    }

    public string? GetHoverInfo(string elementPath, string? attributeName)
    {
        if (!string.IsNullOrEmpty(attributeName))
        {
            var attrType = _schemaCache.GetAttributeType(elementPath, attributeName);
            if (!string.IsNullOrEmpty(attrType))
            {
                return $"**{attributeName}** : `{attrType}`";
            }
        }

        if (!string.IsNullOrEmpty(elementPath))
        {
            return $"**Element:** `{elementPath}`";
        }

        return null;
    }
}
