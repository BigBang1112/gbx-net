using GBX.NET;
using GBX.NET.Engines.Game;
using GBX.NET.LZO;
using KellermanSoftware.CompareNetObjects;

if (args.Length < 2)
{
    Console.WriteLine("Usage: GbxDiff <file1> <file2>");
    return;
}

Console.WriteLine("Expected: " + Path.GetFileName(args[0]));
Console.WriteLine("Actual: " + Path.GetFileName(args[1]));
Console.WriteLine();

Gbx.LZO = new Lzo();

var gbx1 = Gbx.Parse(args[0]);
var gbx2 = Gbx.Parse(args[1]);

var compareLogic = new CompareLogic(new ComparisonConfig() { MaxDifferences = 100 });

var result = compareLogic.Compare(gbx1, gbx2);

if (!result.AreEqual)
{
    Console.WriteLine("Files are not equal:");
    Console.WriteLine();

    foreach (var difference in result.Differences)
    {
        Console.WriteLine(difference.ToString());
    }
}
else
{
    Console.WriteLine("Files are equal.");
}

Console.ReadKey();