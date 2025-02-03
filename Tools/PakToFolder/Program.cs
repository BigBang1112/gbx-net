using GBX.NET.Components;
using GBX.NET.Exceptions;
using GBX.NET.PAK;

var pakFileNameOrDirectory = args[0];
var isDirectory = Directory.Exists(pakFileNameOrDirectory);

var directoryPath = isDirectory ? pakFileNameOrDirectory : Path.GetDirectoryName(pakFileNameOrDirectory)!;

var game = PakListGame.TM;

if (args.Length > 1 && args[1].Equals("vsk5", StringComparison.InvariantCultureIgnoreCase))
{
    game = PakListGame.Vsk5;
}

Console.WriteLine("Bruteforcing possible file names from hashes...");

var hashes = await Pak.BruteforceFileHashesAsync(directoryPath, game, onlyUsedHashes: false);

var packlistFileName = Path.Combine(directoryPath, "packlist.dat");
var keysFileName = "keys.txt";

PakList? packlist = null;
Dictionary<string, (string? Key, string? SecondKey)>? keys = null;

if (File.Exists(packlistFileName))
{
    packlist = await PakList.ParseAsync(packlistFileName, game);
}
else if (File.Exists(keysFileName))
{
    keys = GetKeysFromTxt(keysFileName);
}
else
{
    Console.WriteLine("No packlist.dat or keys.txt files found.");
    return;
}

var pakFileNames = isDirectory ? 
    Directory.GetFiles(pakFileNameOrDirectory, "*.*", SearchOption.AllDirectories)
    .Where(file => file.EndsWith(".pak", StringComparison.OrdinalIgnoreCase) ||
                   file.EndsWith(".Pack.Gbx", StringComparison.OrdinalIgnoreCase))
    .ToArray() 
    : [pakFileNameOrDirectory];

if (pakFileNames.Length == 0)
{
    Console.WriteLine($"No files found in directory {pakFileNameOrDirectory}");
}

foreach (var pakFileName in pakFileNames)
{
    var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(pakFileName);
    var extractFolderPath = Path.Combine(directoryPath, fileNameWithoutExtension);

    Pak? pak = null;

    await using var fs = File.OpenRead(pakFileName);

    if (packlist != null)
    {
        var key = packlist[fileNameWithoutExtension.ToLowerInvariant()].Key;

        pak = await Pak.ParseAsync(fs, key);
    }
    else if (keys != null)
    {
        if (keys.TryGetValue(fileNameWithoutExtension, out var keyData))
        {
            var key = keyData.Key != null ? Convert.FromHexString(keyData.Key) : null;
            var secondKey = keyData.SecondKey != null ? Convert.FromHexString(keyData.SecondKey) : null;

            pak = await Pak.ParseAsync(fs, key, secondKey);
        }
    }

        continue;

    foreach (var file in pak.Files.Values)
    {
        var fileName = hashes.GetValueOrDefault(file.Name)?.Replace('\\', Path.DirectorySeparatorChar) ?? file.Name;
        var fullPath = Path.Combine(extractFolderPath, file.FolderPath, fileName);

        Console.WriteLine(fullPath);

        Directory.CreateDirectory(Path.GetDirectoryName(fullPath)!);

        if (File.Exists(fullPath))
        {
            File.Delete(fullPath);
        }

        using var stream = File.OpenWrite(fullPath);

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
        catch (Exception ex)
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

static Dictionary<string, (string? Key, string? SecondKey)> GetKeysFromTxt(string keysFileName)
{
    Dictionary<string, (string? Key, string? SecondKey)> keys = [];
    var keys = new Dictionary<string, (string? Key, string? SecondKey)>(StringComparer.OrdinalIgnoreCase);

    foreach (var line in File.ReadLines(keysFileName))
    {
        var parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);

        if (parts.Length < 2) 
            continue;

        string pak = parts[0];
        string? key = parts[1] != "null" ? parts[1] : null;
        string? secondKey = parts.Length > 2 && parts[2] != "null" ? parts[2] : null;

        keys[pak] = (key, secondKey);
    }

    return keys;
}
