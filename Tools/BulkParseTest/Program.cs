using GBX.NET;
using GBX.NET.Engines.Game;
using Microsoft.Extensions.Logging;
using System.IO.Compression;

if (args.Length == 0)
    return;

var pattern = "*.Gbx";
var directory = default(string?);
var zipFile = default(string?);

var argsEnumerator = args.GetEnumerator();

while (argsEnumerator.MoveNext())
{
    switch (argsEnumerator.Current)
    {
        case "-pattern":
            if (argsEnumerator.MoveNext())
                pattern = $"*.{argsEnumerator.Current}.Gbx";
            break;
        default:
            var inputDirectory = (string)argsEnumerator.Current;
            if (Directory.Exists(inputDirectory))
                directory = inputDirectory;
            if (File.Exists(inputDirectory))
                zipFile = inputDirectory;
            break;
    }
}

var archive = default(ZipArchive);

if (directory is null)
{
    if (zipFile is null)
    {
        return;
    }

    archive = ZipFile.OpenRead(zipFile);
}

var logger = LoggerFactory.Create(builder =>
{
    builder.AddSimpleConsole(options =>
    {
        options.IncludeScopes = true;
        options.SingleLine = true;
    });
    builder.SetMinimumLevel(LogLevel.Information);
}).CreateLogger<Program>();

var files = directory is null ? null : Directory.GetFiles(directory, pattern, SearchOption.AllDirectories);
var entries = zipFile is null ? null : archive?.Entries.Where(x => x.Name != "").ToList();
var exceptionMessages = new List<string>();

var length = files?.Length ?? entries?.Count;
var successful = 0;

for (var i = 0; i < length; i++)
{
    var fileName = files?[i];
    var entry = entries?[i];

    try
    {
        //logger.LogInformation("{fileName}", fileName);

        var node = default(Node);

        if (fileName is not null)
        {
            node = GameBox.ParseNode(fileName, logger: logger);
        }
        else if (entry is not null)
        {
            using var stream = entry.Open();
            node = GameBox.ParseNode(stream, logger: logger);
        }

        node.DiscoverChunk<CGameCtnChallenge.Chunk03043040>();

        if (node is null)
        {
            Console.WriteLine(fileName + " returns null!");
        }
        else
        {
            successful++;
        }
    }
    catch (Exception ex)
    {
        if (!exceptionMessages.Contains(ex.Message))
        {
            logger.LogError(ex, fileName ?? entry?.Name ?? "Unknown file");

            exceptionMessages.Add(ex.Message);
        }
    }

    Console.Write("Progress: {0}/{1}/{2} ({3})", successful, i + 1, length, (successful / (float)(i + 1)).ToString("P"));
    Console.CursorLeft = 0;
}

Console.WriteLine("Complete!");