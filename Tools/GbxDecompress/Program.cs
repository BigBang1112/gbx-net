using GBX.NET;
using GBX.NET.LZO;

if (args.Length == 0)
{
    Console.WriteLine("Usage: GbxDecompress <filename> [<filename> ...]");
    Console.Write("Press any key to continue...");
    Console.ReadKey(true);
    Console.WriteLine();
    return;
}

Gbx.LZO = new Lzo();

var folderName = "decompressed";

foreach (var fileName in args)
{
    Directory.CreateDirectory(folderName);

    Gbx.Decompress(fileName, Path.Combine(folderName, Path.GetFileName(fileName)));

    Console.WriteLine($"{fileName} successfully decompressed.");
}