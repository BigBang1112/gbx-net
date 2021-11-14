using GBX.NET;

if (args.Length == 0) return;

var fileName = args[0];

var rootPath = Path.GetDirectoryName(typeof(Program).Assembly.Location) + "/";

GameBox.Decompress(fileName, rootPath + Path.GetFileNameWithoutExtension(fileName) + "-decompressed" + Path.GetExtension(fileName));