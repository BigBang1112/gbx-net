using GBX.NET.PAK;

var hashes = new Dictionary<string, string>();

foreach (var filePath in Directory.GetFiles("../../../../../Resources", "FileHashes_*.txt"))
{
    foreach (var line in File.ReadLines(filePath))
    {
        var parts = line.Split(' ', 2, StringSplitOptions.RemoveEmptyEntries);

        if (parts.Length != 2)
        {
            continue;
        }

        var hash = parts[0];
        var fileName = parts[1];

        hashes[hash] = fileName;
    }
}

using (var writer = File.CreateText("../../../../../Resources/FileHashes.txt"))
{
    foreach (var (hash, name) in hashes.OrderBy(x => x.Value).ThenBy(x => x.Key))
    {
        await writer.WriteLineAsync($"{hash} {name}");
    }
}

var keyLookup = ParseKeysFromTxt("../../../../../Resources/BaseKeys.txt").ToLookup(x => x.Item1, x => x.Item2);

Directory.CreateDirectory("../../../../../Resources/FileHashes");

var directoryPath = args[0];

foreach (var pakFilePath in Directory.EnumerateFiles(directoryPath, "*.pak").Concat(Directory.EnumerateFiles(directoryPath, "*.Pack.Gbx")))
{
    foreach (var key in keyLookup[Path.GetFileNameWithoutExtension(pakFilePath)])
    {
        try
        {
            using var pak = await Pak.ParseAsync(pakFilePath, key);

            var perPakHashes = new Dictionary<string, string>();
            var perPakHashesPath = $"../../../../../Resources/FileHashes/{Path.GetFileNameWithoutExtension(pakFilePath)}.txt";

            if (File.Exists(perPakHashesPath))
            {
                foreach (var line in File.ReadLines(perPakHashesPath))
                {
                    var parts = line.Split(' ', 2, StringSplitOptions.RemoveEmptyEntries);

                    if (parts.Length != 2)
                    {
                        continue;
                    }

                    var hash = parts[0];
                    var fileName = parts[1];

                    perPakHashes[hash] = fileName;
                }
            }

            foreach (var (_, file) in pak.Files)
            {
                if (hashes.TryGetValue(file.Name, out var fileName))
                {
                    perPakHashes[file.Name] = fileName;
                }
            }

            if (perPakHashes.Count > 0)
            {
                using var writer = File.CreateText(perPakHashesPath);

                foreach (var (hash, name) in perPakHashes.OrderBy(x => x.Value).ThenBy(x => x.Key))
                {
                    await writer.WriteLineAsync($"{hash} {name}");
                }
            }
        }
        catch
        {

        }
    }
}

static IEnumerable<(string, byte[])> ParseKeysFromTxt(string keysFileName)
{
    foreach (var line in File.ReadLines(keysFileName))
    {
        var parts = line.Split(' ', 2, StringSplitOptions.RemoveEmptyEntries);

        if (parts.Length != 2)
        {
            continue;
        }

        var pak = parts[0];
        var key = Convert.FromHexString(parts[1]);

        yield return (pak, key);
    }
}