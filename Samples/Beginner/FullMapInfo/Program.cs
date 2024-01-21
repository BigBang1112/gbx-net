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

using var ms = new MemoryStream();
using var fs = File.OpenRead(args[0]);
fs.CopyTo(ms);
ms.Position = 0;

watch = Stopwatch.StartNew();

var mapAgain = Gbx.ParseNode<CGameCtnChallenge>(ms);

watch.Stop();

Console.WriteLine("Block count per model:");

foreach (var model in mapAgain.Blocks.GroupBy(x => x.BlockModel))
{
    Console.WriteLine($"- {model.Key.Id}: {model.Count()}");
}

Console.WriteLine();
Console.WriteLine($"Map loaded in {watch.Elapsed.TotalMilliseconds}ms");