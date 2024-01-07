using GBX.NET;
using GBX.NET.Engines.Game;
using GBX.NET.LZO;

if (args.Length == 0)
{
    Console.WriteLine("Usage: dotnet run <path to map>");
    return;
}

Gbx.LZO = new MiniLZO();
var map = Gbx.ParseNode<CGameCtnChallenge>(args[0]);

Console.WriteLine($"Map name: {map.MapName}");