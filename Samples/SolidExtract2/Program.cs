using GBX.NET;
using GBX.NET.Engines.Plug;

IEnumerable<string> files;
var gameDataFolder = default(string);

if (args.Length == 0)
{
    Console.WriteLine("Please drag and drop a file or folder onto this executable.");
    return;
}

if (File.Exists(args[0]))
{
    files = new[] { args[0] };
}
else
{
    files = new[] { "*.Solid.Gbx", "*.Solid2.Gbx" }
        .SelectMany(x => Directory.EnumerateFiles(args[0], x, SearchOption.AllDirectories));

    gameDataFolder = args.ElementAtOrDefault(1);
}


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