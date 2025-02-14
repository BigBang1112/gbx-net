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

PakList? pakList = null;
Dictionary<string, (byte[]? Key, byte[]? SecondKey)>? keys = null;

if (File.Exists(pakListFileName))
{
    pakList = await PakList.ParseAsync(pakListFileName, game);
}
else if (File.Exists(keysFileName))
{
    keys = await PakList.ParseKeysFromTxtAsync(keysFileName);
}
else
{
    Console.WriteLine($"No {pakListFileName} or {keysFileName} files found.");
    return;
}

Console.WriteLine("Bruteforcing possible file names from hashes...");

Dictionary<string, string> hashes = [];

if (pakList is not null)
{
    hashes = await Pak.BruteforceFileHashesAsync(directoryPath, pakList, onlyUsedHashes: false);
}
else if (keys is not null)
{
    hashes = await Pak.BruteforceFileHashesAsync(directoryPath, keys, onlyUsedHashes: false);
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

    if (pakList != null)
    {
        var key = pakList[fileNameWithoutExtension.ToLowerInvariant()].Key;

        pak = await Pak.ParseAsync(fs, key);
    }
    else if (keys != null)
    {
        if (keys.TryGetValue(fileNameWithoutExtension, out var keyData))
        {
            pak = await Pak.ParseAsync(fs, keyData.Key, keyData.SecondKey);
        }
        else
        {
            Console.WriteLine($"No key found for pak {pakFileName} {fileNameWithoutExtension}");
            continue;
        }
    }

    Console.WriteLine($"Pak: {pakFileName}");

    foreach (var file in pak!.Files.Values)
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
}

static void CopyFileToStream(Pak pak, PakFile file, Stream stream)
{
    var pakItemFileStream = pak.OpenFile(file, out _);
    var data = new byte[file.UncompressedSize];
    var count = pakItemFileStream.Read(data);
    stream.Write(data, 0, count);
}
