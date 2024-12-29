using GBX.NET;
using GBX.NET.Components;
using GBX.NET.Exceptions;
using GBX.NET.PAK;
using System.IO.Compression;

var pakFileName = args[0];
var directoryPath = Path.GetDirectoryName(pakFileName)!;

Console.WriteLine("Bruteforcing possible file names from hashes...");

var hashes = await Pak.BruteforceFileHashesAsync(directoryPath, onlyUsedHashes: false);

var packlistFileName = Path.Combine(directoryPath, "packlist.dat");
var packlist = await PakList.ParseAsync(packlistFileName);

var key = packlist[Path.GetFileNameWithoutExtension(pakFileName).ToLowerInvariant()].Key;

using var fs = File.OpenRead(pakFileName);
using var pak = await Pak.ParseAsync(fs, key);

using var zip = ZipFile.Open(Path.ChangeExtension(pakFileName, ".zip"), ZipArchiveMode.Create);

foreach (var file in pak.Files.Values)
{
    var fileName = hashes.GetValueOrDefault(file.Name)?.Replace('\\', Path.DirectorySeparatorChar) ?? file.Name;
    var fullPath = Path.Combine(file.FolderPath, fileName);

    Console.WriteLine(fullPath);

    try
    {
        var gbx = await pak.OpenGbxFileAsync(file);

        var entry = zip.CreateEntry(fullPath);
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
        var entry = zip.CreateEntry(fullPath);
        using var stream = entry.Open();
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