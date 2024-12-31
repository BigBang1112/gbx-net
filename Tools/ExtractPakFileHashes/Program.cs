using GBX.NET.PAK;

var directoryPath = args[0];

Console.WriteLine("Bruteforcing possible file names from hashes...");

var hashes = await Pak.BruteforceFileHashesAsync(directoryPath);

using var writer = new StreamWriter("hashes.txt");
foreach (var (hash, name) in hashes.OrderBy(x => x.Value))
{
    await writer.WriteLineAsync($"{hash:X16} {name}");
}