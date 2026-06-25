using GBX.NET.PAK;

var game = PakListGame.TM;
var keys = new Dictionary<string, byte[]?>(StringComparer.OrdinalIgnoreCase);
var hashes = new Dictionary<string, string?>();
var pakDirectoryPaths = new List<string>();

var hashesFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "hashes.txt");
var keysTxtPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "keys.txt");

if (File.Exists(keysTxtPath))
{
    foreach (var (name, keyInfo) in ParseKeysFromTxt(keysTxtPath))
    {
        keys[name] = keyInfo;
    }
}

var argsEnumerator = args.AsEnumerable().GetEnumerator();

while (argsEnumerator.MoveNext())
{
    var arg = argsEnumerator.Current;
    var argLower = arg.ToLowerInvariant();

    if (argLower == "-k" || argLower == "--keys")
    {
        if (!argsEnumerator.MoveNext())
        {
            throw new Exception("Missing keys file.");
        }

        var keysFilePath = argsEnumerator.Current;

        if (!File.Exists(keysFilePath))
        {
            throw new Exception("Keys file does not exist.");
        }

        foreach (var (name, keyInfo) in ParseKeysFromTxt(keysFilePath))
        {
            keys[name] = keyInfo;
        }

        continue;
    }

    if (argLower == "-h" || argLower == "--hashes")
    {
        if (!argsEnumerator.MoveNext())
        {
            throw new Exception("Missing hashes file.");
        }

        hashesFilePath = argsEnumerator.Current;

        if (File.Exists(hashesFilePath))
        {
            foreach (var (hash, fileName) in ParseHashesFromTxt(hashesFilePath))
            {
                hashes[hash] = fileName;
            }
        }

        continue;
    }

    if (argLower == "--vsk5")
    {
        game = PakListGame.Vsk5;
        continue;
    }

    if (Directory.Exists(arg))
    {
        pakDirectoryPaths.Add(arg);
        continue;
    }

    if (File.Exists(arg))
    {
        pakDirectoryPaths.Add(Path.GetDirectoryName(arg)!);
        continue;
    }
}

foreach (var pakDirectoryPath in pakDirectoryPaths)
{
    var pakListFilePath = Path.Combine(pakDirectoryPath, "packlist.dat");

    if (File.Exists(pakListFilePath))
    {
        foreach (var (name, keyInfo) in (await PakList.ParseAsync(pakListFilePath, game)).ToKeyInfoDictionary())
        {
            keys[name] = keyInfo;
        }
    }

    Console.WriteLine("Bruteforcing possible file names from hashes...");

    var newHashes = await Pak.BruteforceFileHashesAsync(pakDirectoryPath, keys, keepUnresolvedHashes: true, additionalFileHashes: hashes.Keys);

    Console.WriteLine($"Resolved {hashes.Count(x => !string.IsNullOrEmpty(x.Value))}/{hashes.Count} hashes.");

    foreach (var (hash, name) in newHashes)
    {
        if (!hashes.TryGetValue(hash, out var existingName) || string.IsNullOrEmpty(existingName))
        {
            hashes[hash] = name;
        }
    }
}

using var writer = File.CreateText(hashesFilePath);
foreach (var (hash, name) in hashes.OrderBy(x => x.Value ?? "").ThenBy(x => x.Key))
{
    await writer.WriteLineAsync($"{hash:X16} {name}");
}

static IEnumerable<(string, byte[]?)> ParseKeysFromTxt(string keysFileName)
{
    foreach (var line in File.ReadLines(keysFileName))
    {
        var parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);

        if (parts.Length < 2)
            continue;

        var pak = parts[0];
        var key = parts[1] != "null" ? Convert.FromHexString(parts[1]) : null;

        yield return (pak, key);
    }
}

static IEnumerable<(string, string?)> ParseHashesFromTxt(string hashesFileName)
{
    foreach (var line in File.ReadLines(hashesFileName))
    {
        var firstSpace = line.IndexOf(' ');

        if (firstSpace == -1)
        {
            continue;
        }

        var hash = line[..firstSpace];
        var path = line[(firstSpace + 1)..];

        yield return (hash, string.IsNullOrEmpty(path) ? null : path);
    }
}