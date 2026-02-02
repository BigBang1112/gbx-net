using GBX.NET.PAK;

var pakFileNameOrDirectory = args[0];
var isDirectory = Directory.Exists(pakFileNameOrDirectory);

var directoryPath = isDirectory ? pakFileNameOrDirectory : Path.GetDirectoryName(pakFileNameOrDirectory)!;

var game = PakListGame.TM;

if (args.Length > 1 && args[1].Equals("vsk5", StringComparison.InvariantCultureIgnoreCase))
{
    game = PakListGame.Vsk5;
}

var pakListFileName = Path.Combine(directoryPath, "packlist.dat");
var keysFileName = "keys.txt";

Dictionary<string, byte[]?> keys;

if (File.Exists(pakListFileName))
{
    keys = (await PakList.ParseAsync(pakListFileName, game)).ToKeyInfoDictionary();
}
else if (File.Exists(keysFileName))
{
    keys = await ParseKeysFromTxtAsync(keysFileName);
}
else
{
    keys = [];
}

Console.WriteLine("Bruteforcing possible file names from hashes...");

var hashes = await Pak.BruteforceFileHashesAsync(directoryPath, keys, keepUnresolvedHashes: true);

Console.WriteLine($"Resolved {hashes.Count(x => !string.IsNullOrEmpty(x.Value))}/{hashes.Count} hashes.");

using var writer = new StreamWriter("hashes.txt");
foreach (var (hash, name) in hashes.OrderBy(x => x.Value).ThenBy(x => x.Key))
{
    await writer.WriteLineAsync($"{hash:X16} {name}");
}

static async Task<Dictionary<string, byte[]?>> ParseKeysFromTxtAsync(string keysFileName)
{
    var keys = new Dictionary<string, byte[]?>(StringComparer.OrdinalIgnoreCase);

    using var reader = new StreamReader(keysFileName);

    while (await reader.ReadLineAsync() is string line)
    {
        var parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);

        if (parts.Length == 0)
        {
            continue;
        }

        var pak = parts[0];
        var key = parts.Length > 1 ? Convert.FromHexString(parts[1]) : null;

        keys[pak] = key;
    }

    return keys;
}