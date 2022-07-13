using GBX.NET;
using GBX.NET.Engines.GameData;
using GBX.NET.Imaging;

if (args.Length == 0)
{
    return;
}

var fileName = args[0];

string[] fileNames;

if (File.Exists(fileName))
{
    fileNames = new[] { fileName };
}
else
{
    fileNames = Directory.GetFiles(Path.GetDirectoryName(fileName)!, "*.Gbx", SearchOption.AllDirectories);
}

foreach (var filePath in fileNames)
{
    var node = GameBox.ParseNodeHeader(filePath);

    if (node is not CGameCtnCollector collector)
    {
        continue;
    }

    var pngFileName = filePath + ".png";

    if (collector.ExportIcon(pngFileName))
    {
        Console.WriteLine("{0} exported!", pngFileName);
    }
}
