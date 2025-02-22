using GBX.NET;
using GBX.NET.Components;
using GBX.NET.Exceptions;
using GBX.NET.PAK;
using System.IO.Compression;

var pakFileName = args[0];
var directoryPath = Path.GetDirectoryName(pakFileName)!;

var game = PakListGame.TM;

if (args.Length > 1 && args[1].Equals("vsk5", StringComparison.InvariantCultureIgnoreCase))
{
    game = PakListGame.Vsk5;
}

var pakListFileName = Path.Combine(directoryPath, "packlist.dat");
var keysFileName = "keys.txt";

Dictionary<string, PakKeyInfo?> keys;

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

var hashes = await Pak.BruteforceFileHashesAsync(directoryPath, keys, onlyUsedHashes: false);

await using var pak = keys.TryGetValue(Path.GetFileNameWithoutExtension(pakFileName), out var keyData)
    ? await Pak.ParseAsync(pakFileName, keyData.PrimaryKey, keyData.FileKey)
    : await Pak.ParseAsync(pakFileName);

File.Delete(Path.ChangeExtension(pakFileName, ".zip"));

using var zip = ZipFile.Open(Path.ChangeExtension(pakFileName, ".zip"), ZipArchiveMode.Create);

foreach (var file in pak.Files.Values)
{
    var fileName = hashes.GetValueOrDefault(file.Name)?.Replace('\\', Path.DirectorySeparatorChar) ?? file.Name;
    var fullPath = Path.Combine(file.FolderPath, fileName);

    Console.WriteLine(fullPath);

    var entry = zip.CreateEntry(fullPath);

    try
    {
        var gbx = await pak.OpenGbxFileAsync(file, fileHashes: hashes);

        using var stream = entry.Open();

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
        await using var stream = entry.Open();
        CopyFileToStream(pak, file, stream);
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex);
    }
}

static void CopyFileToStream(Pak pak, PakFile file, Stream stream)
{
    var pakItemFileStream = pak.OpenFile(file, out _);
    var data = new byte[file.UncompressedSize];
    var count = pakItemFileStream.Read(data);
    stream.Write(data, 0, count);
}

static async Task<Dictionary<string, PakKeyInfo?>> ParseKeysFromTxtAsync(string keysFileName)
{
    var keys = new Dictionary<string, PakKeyInfo?>(StringComparer.OrdinalIgnoreCase);

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