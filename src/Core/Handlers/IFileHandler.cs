using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Core.Handlers;

public interface IFileHandler
{
    Task<(string content, string customer)> HandleFileAsync(Stream fileStream);
}

public class FileHandlerFactory
{
    public bool TryCreateHandler(string fileType, out IFileHandler handler)
    {
        handler = fileType.ToLower() switch
        {
            ".txt" => new TxtFileHandler(),
            ".xml" => new XmlFileHandler(),
            ".json" => new JsonFileHandler(),
            _ => null
        };

        return handler is not null;
    }
}

public class TxtFileHandler : IFileHandler
{
    public async Task<(string content, string customer)> HandleFileAsync(Stream fileStream)
    {
        using var reader = new StreamReader(fileStream);
        var content = await reader.ReadToEndAsync();
        return (content, null);
    }
}

public class XmlFileHandler : IFileHandler
{
    public async Task<(string content, string customer)> HandleFileAsync(Stream fileStream)
    {
        using var reader = new StreamReader(fileStream);
        var xmlContent = await reader.ReadToEndAsync();
        var xdoc = XDocument.Parse(xmlContent);

        var content = xdoc.Root.Element("Content")?.Value.Trim();
        var customer = xdoc.Root.Element("Customer")?.Value.Trim();

        return (content, customer);
    }
}

public class JsonFileHandler : IFileHandler
{
    public async Task<(string content, string customer)> HandleFileAsync(Stream fileStream)
    {
        using var reader = new StreamReader(fileStream);
        var jsonContent = await reader.ReadToEndAsync();

        var token = JToken.Parse(jsonContent);

        var content = token.Value<string>("content");
        var customer = token.Value<string>("customer");
        return (content, customer);
    }
}

