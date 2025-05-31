using GBX.NET.Components;
using GBX.NET.Exceptions;
using GBX.NET.PAK;

var game = PakListGame.TM;
var pakFilePaths = new List<string>();
var keys = new Dictionary<string, PakKeyInfo?>(StringComparer.OrdinalIgnoreCase);
var hashes = new Dictionary<string, string?>();

var keysTxtPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "keys.txt");

if (File.Exists(keysTxtPath))
{
    foreach (var (name, keyInfo) in ParseKeysFromTxt(keysTxtPath))
    {
        keys[name] = keyInfo;
    }
}

var hashesTxtPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "hashes.txt");

if (File.Exists(hashesTxtPath))
{
    foreach (var (hash, fileName) in ParseHashesFromTxt(hashesTxtPath))
    {
        hashes[hash] = fileName;
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

        var hashesFilePath = argsEnumerator.Current;

        if (!File.Exists(hashesFilePath))
        {
            throw new Exception("Hashes file does not exist.");
        }

        foreach (var (hash, fileName) in ParseHashesFromTxt(hashesFilePath))
        {
            hashes[hash] = fileName;
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
        pakFilePaths.AddRange(Directory.GetFiles(arg, "*.pak", SearchOption.AllDirectories));
        pakFilePaths.AddRange(Directory.GetFiles(arg, "*.Pack.Gbx", SearchOption.AllDirectories));
        continue;
    }

    if (File.Exists(arg))
    {
        pakFilePaths.Add(arg);
        continue;
    }
}

var checkedDirectories = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

foreach (var pakFilePath in pakFilePaths)
{
    var directoryPath = Path.GetDirectoryName(pakFilePath)!;

    if (checkedDirectories.Add(directoryPath))
    {
        var keysFilePath = Path.Combine(directoryPath, "keys.txt");
        var pakListFilePath = Path.Combine(directoryPath, "packlist.dat");

        if (File.Exists(keysFilePath))
        {
            foreach (var (name, keyInfo) in ParseKeysFromTxt(keysFilePath))
            {
                keys[name] = keyInfo;
            }
        }

        if (File.Exists(pakListFilePath))
        {
            foreach (var (name, keyInfo) in (await PakList.ParseAsync(pakListFilePath, game)).ToKeyInfoDictionary())
            {
                keys[name] = keyInfo;
            }

            Console.WriteLine("Bruteforcing possible file names from hashes...");

            foreach (var (hash, fileName) in await Pak.BruteforceFileHashesAsync(directoryPath, keys, onlyUsedHashes: false))
            {
                hashes[hash] = fileName;
            }

            Console.WriteLine("Bruteforcing completed.");
        }
    }

    var pakId = Path.GetFileNameWithoutExtension(pakFilePath);
    var extractFolderPath = Path.Combine(directoryPath, pakId);

    await using var pak = keys.TryGetValue(pakId, out var keyData)
        ? await Pak.ParseAsync(pakFilePath, keyData?.PrimaryKey, keyData?.FileKey)
        : await Pak.ParseAsync(pakFilePath);

    Console.WriteLine($"Pak: {pakFilePath}");

    foreach (var file in pak.Files.Values)
    {
        var fileName = hashes.GetValueOrDefault(file.Name)?.Replace('\\', Path.DirectorySeparatorChar) ?? file.Name;
        var fullPath = Path.Combine(extractFolderPath, file.FolderPath, fileName);

        Console.WriteLine(fullPath);

        Directory.CreateDirectory(Path.GetDirectoryName(fullPath)!);

        await using var stream = File.Create(fullPath);

        try
        {
            var gbx = await pak.OpenGbxFileAsync(file);

            if (gbx.Header is GbxHeaderUnknown)
            {
                CopyFileToStream(pak, file, stream);
            }
            else
            {
                gbx.Save(stream);
            }
        }
        catch (NotAGbxException)
        {
            CopyFileToStream(pak, file, stream);
        }
        catch
        {
            try
            {
                CopyFileToStream(pak, file, stream);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}

static void CopyFileToStream(Pak pak, PakFile file, Stream stream)
{
    var pakItemFileStream = pak.OpenFile(file, out _);
    var data = new byte[file.UncompressedSize];
    var count = pakItemFileStream.Read(data);
    stream.Write(data, 0, count);
}

static IEnumerable<(string, PakKeyInfo?)> ParseKeysFromTxt(string keysFileName)
{
    foreach (var line in File.ReadLines(keysFileName))
    {
        var parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);

        if (parts.Length < 2)
            continue;

        var pak = parts[0];
        var key = parts[1] != "null" ? Convert.FromHexString(parts[1]) : null;
        var secondKey = parts.Length > 2 && parts[2] != "null" ? Convert.FromHexString(parts[2]) : null;

        yield return (pak, new PakKeyInfo(key, secondKey));
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