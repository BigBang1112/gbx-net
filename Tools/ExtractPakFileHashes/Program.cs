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

Dictionary<string, PakKeyInfo> keys;

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

var hashes = await Pak.BruteforceFileHashesAsync(directoryPath, keys);

using var writer = new StreamWriter("hashes.txt");
foreach (var (hash, name) in hashes.OrderBy(x => x.Value))
{
    await writer.WriteLineAsync($"{hash:X16} {name}");
}

static async Task<Dictionary<string, PakKeyInfo>> ParseKeysFromTxtAsync(string keysFileName)
{
    var keys = new Dictionary<string, PakKeyInfo>(StringComparer.OrdinalIgnoreCase);

    using var reader = new StreamReader(keysFileName);

    while (await reader.ReadLineAsync() is string line)
    {
        var parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);

        if (parts.Length < 2)
            continue;

        var pak = parts[0];
        var key = parts[1] != "null" ? Convert.FromHexString(parts[1]) : null;
        var secondKey = parts.Length > 2 && parts[2] != "null" ? Convert.FromHexString(parts[2]) : null;

        keys[pak] = new PakKeyInfo(key, secondKey);
    }

    return keys;
}