using GBX.NET;
using GBX.NET.PAK;
using GBX.NET.ZLib;

Gbx.ZLib = new ZLib();

var fileName = args[0];

var packlistFileName = Path.Combine(Path.GetDirectoryName(fileName)!, "packlist.dat");
var packlist = await PakList.ParseAsync(packlistFileName);

var key = packlist[Path.GetFileNameWithoutExtension(fileName).ToLowerInvariant()].Key;

using var fs = File.OpenRead(fileName);
using var pak = await Pak.ParseAsync(fs, key);

var file = pak.Files.Values.First(x => x.FolderPath.EndsWith("Solid\\"));

using var stream = pak.OpenFile(file, out var encryptionInitializer);

var ok = Gbx.Parse(stream, new() { EncryptionInitializer = encryptionInitializer });

Console.WriteLine();