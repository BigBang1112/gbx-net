using GBX.NET;
using GBX.NET.Engines.Game;
using GBX.NET.LZO;
using System.Diagnostics;

if (args.Length == 0)
{
    Console.WriteLine("Usage: dotnet run <path to map>");
    return;
}

var watch = Stopwatch.StartNew();

Gbx.LZO = new MiniLZO();
var map = Gbx.ParseNode<CGameCtnChallenge>(args[0]);

watch.Stop();

Console.WriteLine("Block count per model:");

foreach (var model in map.GetBlocks().GroupBy(x => x.BlockModel))
{
    Console.WriteLine($"- {model.Key.Id}: {model.Count()}");
}

Console.WriteLine();
Console.WriteLine($"Map loaded in {watch.Elapsed.TotalMilliseconds}ms");
