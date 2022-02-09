using GBX.NET;
using Microsoft.Extensions.Logging;

if (args.Length == 0) return;

var logger = LoggerFactory.Create(builder =>
{
    builder.AddSimpleConsole(options =>
    {
        options.IncludeScopes = true;
        options.SingleLine = true;
        options.TimestampFormat = "yyyy-MM-dd HH:mm:ss ";
    });
    builder.SetMinimumLevel(LogLevel.Debug);
}).CreateLogger<Program>();

var fileName = args[0];

var rootPath = Path.GetDirectoryName(typeof(Program).Assembly.Location) + "/";

GameBox.Decompress(fileName, rootPath + Path.GetFileNameWithoutExtension(fileName) + "-decompressed" + Path.GetExtension(fileName));
var node = GameBox.ParseNode(fileName, logger: logger);

if (node is null)
{
    return;
}

using var ms = new MemoryStream();

node.Save(ms);

ms.Position = 0;

GameBox.Decompress(ms, rootPath + Path.GetFileNameWithoutExtension(fileName) + "-saved-decompressed" + Path.GetExtension(fileName));