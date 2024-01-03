using GBX.NET;
using GBX.NET.Engines.Game;

if (args.Length == 0)
{
    Console.WriteLine("Please provide a path to a map.");
    return;
}

var map = (CGameCtnChallenge)Gbx.ParseHeaderNode(args[0])!;

Console.WriteLine($"Map name: {map.MapName}");