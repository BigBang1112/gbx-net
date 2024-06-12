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

Gbx.LZO = new MiniLZO();

foreach (var fileName in args)
{
    Directory.CreateDirectory("decompressed");
    var outputFilePath = Path.GetDirectoryName(fileName) is string dir
        ? Path.Combine(dir, "decompressed", Path.GetFileName(fileName))
        : Path.Combine("decompressed", Path.GetFileName(fileName));

    var decompressed = Gbx.Decompress(fileName, outputFilePath);

    if (decompressed)
    {
        Console.WriteLine($"{fileName} successfully decompressed.");
    }
    else
    {
        Console.WriteLine($"{fileName} is already decompressed.");
    }
}