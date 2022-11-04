using GBX.NET;
using GBX.NET.Engines.Game;
using GBX.NET.Exceptions;
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

var files = directory is null ? null : Directory.EnumerateFiles(directory, pattern, SearchOption.AllDirectories);
var entries = zipFile is null ? null : archive?.Entries.Where(x => x.Name != "").ToList();
var exceptionMessages = new List<string>();

var length = entries?.Count ?? 0;
var successful = 0;

if (files is not null)
{
    var counter = 0;

    foreach (var fileName in files)
    {
        counter++;

        try
        {
            var node = GameBox.ParseNode(fileName, logger: logger);

            if (node is null)
            {
                Console.WriteLine(fileName + " returns null!");
            }
            else
            {
                successful++;
            }
        }
        catch (NotAGbxException)
        {
            counter--;
        }
        catch (Exception ex)
        {
            if (!exceptionMessages.Contains(ex.Message))
            {
                logger.LogError(ex, fileName ?? "Unknown file");

                exceptionMessages.Add(ex.Message);
            }
        }

        Console.Write("Progress: {0}/{1} ({2})", successful, counter, (successful / (float)counter).ToString("P"));
        Console.CursorLeft = 0;
    }
}

if (entries is not null)
{
    var notAGbxs = 0;

    for (int i = 0; i < entries.Count; i++)
    {
        var entry = entries[i];
        
        try
        {
            using var stream = entry.Open();
            var node = GameBox.ParseNode(stream, logger: logger);

            if (node is null)
            {
                Console.WriteLine(entry.FullName + " returns null!");
            }
            else
            {
                successful++;
            }
        }
        catch (NotAGbxException)
        {
            length--;
            notAGbxs++;
        }
        catch (Exception ex)
        {
            if (!exceptionMessages.Contains(ex.Message))
            {
                logger.LogError(ex, entry.FullName ?? "Unknown file");

                exceptionMessages.Add(ex.Message);
            }
        }

        Console.Write("Progress: {0}/{1}/{2} ({3})", successful, i + 1 - notAGbxs, length, (successful / (float)(i + 1 - notAGbxs)).ToString("P"));
        Console.CursorLeft = 0;
    }
}

Console.WriteLine("Complete!");