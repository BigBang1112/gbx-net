using GBX.NET;
using Microsoft.Extensions.Logging;

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

var folderName = Path.GetFileNameWithoutExtension(Path.GetFileNameWithoutExtension(fileName));

Directory.CreateDirectory(folderName);

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

    var fullName = $"{type.DeclaringType.Name}+{type.Name}";
    File.WriteAllBytes(Path.Combine(folderName, fullName + ".dat"), chunk.RawData);
}