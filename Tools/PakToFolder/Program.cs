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

    Pak pak;

    await using var fs = File.OpenRead(pakFileName);

    if (keys.TryGetValue(fileNameWithoutExtension, out var keyData))
    {
        pak = await Pak.ParseAsync(fs, keyData?.PrimaryKey, keyData?.FileKey);
    }
    else
    {
        Console.WriteLine($"No key found for pak {fileNameWithoutExtension}, reading only metadata...");
        pak = await Pak.ParseAsync(fs);
    }

    Console.WriteLine($"Pak: {pakFileName}");

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

    await pak.DisposeAsync();
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