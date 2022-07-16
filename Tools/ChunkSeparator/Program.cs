using GBX.NET;
using GBX.NET.Engines.MwFoundations;
using Microsoft.Extensions.Logging;
using System.IO.Compression;

if (args.Length == 0) return;

var fileName = args[0];

var logger = LoggerFactory.Create(builder =>
{
    builder.AddSimpleConsole(options =>
    {
        options.IncludeScopes = true;
        options.SingleLine = true;
    });
    builder.SetMinimumLevel(LogLevel.Debug);
}).CreateLogger<Program>();

GameBox.SeekForRawChunkData = true;

var node = GameBox.ParseNode(fileName, logger: logger);

if (node is null)
{
    return;
}

var zipDict = new Dictionary<Type, ZipArchive>();
var nodeType = node.GetType();

while (nodeType is not null && nodeType != typeof(CMwNod))
{
    File.Delete($"{nodeType.Name}.zip");
    zipDict.Add(nodeType, ZipFile.Open($"{nodeType.Name}.zip", ZipArchiveMode.Create));
    nodeType = nodeType.BaseType;
}

if (node is INodeHeader nodeHeader)
{
    foreach (var chunk in nodeHeader.HeaderChunks)
    {
        ExportChunkData(chunk);
    }
}

foreach (var chunk in node.Chunks)
{
    ExportChunkData(chunk);
}

foreach (var (type, zip) in zipDict)
{
    zip.Dispose();
}

void ExportChunkData(Chunk chunk)
{
    if (chunk.RawData is null)
    {
        return;
    }

    var type = chunk.GetType();

    if (type.DeclaringType is null)
    {
        return;
    }

    var zip = zipDict[type.DeclaringType];
    var entry = zip.CreateEntry($"{type.Name}.dat");

    using var stream = entry.Open();
    using var ms = new MemoryStream(chunk.RawData);
    ms.CopyTo(stream);
}