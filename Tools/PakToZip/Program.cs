using GBX.NET;
using GBX.NET.PAK;
using GBX.NET.ZLib;

Gbx.ZLib = new ZLib();

var fileName = args[0];
var directoryPath = Path.GetDirectoryName(fileName)!;

var hashes = await Pak.BruteforceHashFileNamesAsync(directoryPath);

var packlistFileName = Path.Combine(directoryPath, "packlist.dat");
var packlist = await PakList.ParseAsync(packlistFileName);

var key = packlist[Path.GetFileNameWithoutExtension(fileName).ToLowerInvariant()].Key;

using var fs = File.OpenRead(fileName);
using var pak = await Pak.ParseAsync(fs, key);

var file = pak.Files.Values.First(x => x.Name == "B2FC497BF7F81AB02D01FE8FB2F707CD8F");
var fileItemName = hashes[file.Name];

var gbx = await pak.OpenGbxFileAsync(file);

Console.WriteLine();