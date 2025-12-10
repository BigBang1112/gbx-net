using GBX.NET;
using GBX.NET.Engines.Game;
using GBX.NET.LZO;
using GBX.NET.ZLib;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

if (args.Length == 0)
{
    Console.WriteLine("Usage: ParseRepeater <filename>");
    Console.WriteLine("Press any key to continue...");
    Console.ReadKey(true);
    return;
}

var fileName = args[0];
Gbx.LZO = new Lzo();
Gbx.ZLib = new ZLib();

var logger = LoggerFactory.Create(builder =>
{
    builder.AddSimpleConsole(options =>
    {
        options.IncludeScopes = true;
        options.SingleLine = true;
    });
    builder.SetMinimumLevel(LogLevel.Trace);
}).CreateLogger<Program>();

while (true)
{
    /*try
    {*/
        var stopwatch = Stopwatch.StartNew();
        var gbx = Gbx.ParseNode(fileName, new() { Logger = logger });
        stopwatch.Stop();
        logger.LogInformation("Parsed in {time}ms", stopwatch.Elapsed.TotalMilliseconds);
    /*}
    catch (Exception ex)
    {
        logger.LogError(ex, "Exception during parse.");
    }*/

    Console.WriteLine();
    Console.ReadKey(true);
}