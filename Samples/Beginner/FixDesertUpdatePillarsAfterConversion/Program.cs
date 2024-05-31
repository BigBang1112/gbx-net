using GBX.NET;
using GBX.NET.Engines.Game;
using GBX.NET.LZO;

Gbx.LZO = new MiniLZO();

var gbxMap = Gbx.Parse<CGameCtnChallenge>(args[0]);

foreach (var block in gbxMap.Node.GetBlocks().Where(x => x.Name == "DecoWallBasePillar"))
{
    block.Skin = null;
}

Directory.CreateDirectory("fixed");

gbxMap.Save(Path.Combine("fixed", gbxMap.GetFileNameWithoutExtension() + ".Map.Gbx"));