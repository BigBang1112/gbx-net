using GBX.NET;
using GBX.NET.LZO;
using System.Diagnostics;

if (args.Length == 0)
{
    Console.WriteLine("Usage: BulkParseTest <dirpath>");
    Console.WriteLine("Press any key to continue...");
    Console.ReadKey(true);
}

var dirPath = args[0];

Gbx.LZO = new MiniLZO();

var existingExceptions = new HashSet<string>();
var counter = 0;
var successCounter = 0;

foreach (var fileName in Directory.EnumerateFiles(dirPath, "*.gbx", SearchOption.AllDirectories))
{
    counter++;

    try
    {
        var stopwatch = Stopwatch.StartNew();
        var gbx = Gbx.ParseNode(fileName);
        stopwatch.Stop();
        successCounter++;
        Console.WriteLine($"{Path.GetFileName(fileName)} - {stopwatch.Elapsed.TotalMilliseconds}ms");
    }
    catch (Exception ex)
    {
        var exStr = ex.ToString();

        if (!existingExceptions.Contains(exStr))
        {
            Console.WriteLine($"{Path.GetFileName(fileName)} - Exception during parse: {exStr}");
            existingExceptions.Add(exStr);
        }
    }

    Console.WriteLine($"Success: {successCounter}/{counter} ({(double)successCounter / counter:P})");
}

Console.ReadKey(true);