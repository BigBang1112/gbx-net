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
var packlist = await PakList.ParseAsync(packlistFileName, game);

var pakFileNames = isDirectory ? Directory.GetFiles(pakFileNameOrDirectory, "*.pak", SearchOption.AllDirectories) : [pakFileNameOrDirectory];

foreach (var pakFileName in pakFileNames)
{
    var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(pakFileName);
    var extractFolderPath = Path.Combine(directoryPath, fileNameWithoutExtension);
    var key = packlist[fileNameWithoutExtension.ToLowerInvariant()].Key;

    await using var fs = File.OpenRead(pakFileName);
    await using var pak = await Pak.ParseAsync(fs, key);

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
        {
            Console.WriteLine(ex);
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