using GBX.NET;
using GBX.NET.Engines.Plug;

var files = new[] { "*.Solid.Gbx", "*.Solid2.Gbx" }
    .SelectMany(x => Directory.EnumerateFiles(args[0], x, SearchOption.AllDirectories));

var gameDataFolder = args.ElementAtOrDefault(1);

foreach (var filePath in files)
{
    try
    {
        var node = GameBox.ParseNode(filePath);

        switch (node)
        {
            case CPlugSolid solid:
                solid.ExportToObj(filePath, gameDataFolder, corruptedMaterials: true);
                (solid.Tree as CPlugTree)?.Surface?.ExportToObj(filePath + ".Surface");
                break;
            case CPlugSolid2Model solid2:
                solid2.ExportToObj(filePath, gameDataFolder, corruptedMaterials: true);
                break;
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex);
    }
}