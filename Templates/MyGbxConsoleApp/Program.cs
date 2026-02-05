using GBX.NET;
using GBX.NET.Engines.Game;

#if (EnableLzo)
Gbx.LZO = new GBX.NET.LZO.Lzo();
#endif
#if (EnableZlib)
Gbx.ZLib = new GBX.NET.ZLib.ZLib();
#endif
#if (EnableCrc32)
Gbx.CRC32 = new GBX.NET.Hashing.CRC32();
#endif

if (args.Length == 0)
{
    Console.WriteLine("Usage: MyGbxConsoleApp <path to file>");
    Console.WriteLine("Press any key to continue...");
    Console.ReadKey(true);
    return;
}

#if (SaveGbx)
var gbx = Gbx.Parse(args[0]);
var node = gbx.Node;
#else
var node = Gbx.ParseNode(args[0]);
#endif

if (node is not CGameCtnChallenge map)
{
    Console.WriteLine("The Gbx file is not a map.");
    Console.WriteLine("Press any key to continue...");
    Console.ReadKey(true);
    return;
}

Console.WriteLine($"MapName: {map.MapName}");

#if (SaveGbx)
map.MapName += $"$z - {DateTime.Now:yyyy-MM-dd HH:mm:ss}";

var newFileName = $"{gbx.GetFileNameWithoutExtension()}-{DateTime.Now:yyyyMMddTHHmmss}{GbxPath.GetExtension(gbx.FilePath)}";
gbx.Save(newFileName);
#endif