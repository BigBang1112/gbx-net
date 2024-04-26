using GBX.NET;
using GBX.NET.Engines.Game;
using GBX.NET.LZO;

if (args.Length == 0)
{
    Console.WriteLine("Please provide a path to a Map.Gbx file.");
    return;
}

Gbx.LZO = new MiniLZO();

var map = Gbx.ParseNode<CGameCtnChallenge>(args[0]);

if (!map.IsGameVersion(GameVersion.TM2020))
{
    Console.WriteLine("This is not a Trackmania (2020) map.");
    return;
}

var random = new Random();
var colors = Enum.GetValues<DifficultyColor>().AsSpan().Slice(1);

foreach (var block in map.GetBlocks())
{
    block.Color = colors[random.Next(colors.Length)];
}

map.Save(GbxPath.GetFileNameWithoutExtension(args[0]) + "-lego.Map.Gbx");
