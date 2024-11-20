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

using var stream = pak.OpenFile(pak.Files.Values.First(x => x.FolderPath.EndsWith("Solid\\")));

var ok = Gbx.Parse(stream, new() { IgnoreExceptionsInBody = true });

Console.WriteLine();