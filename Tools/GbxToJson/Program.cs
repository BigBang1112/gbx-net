using GBX.NET;
using GBX.NET.NewtonsoftJson;
using GBX.NET.LZO;
using GBX.NET.ZLib;

if (args.Length == 0)
{
    Console.WriteLine("Usage: GbxToJson <filename>");
    Console.WriteLine("Press any key to continue...");
    Console.ReadKey(true);
}

Gbx.LZO = new Lzo();
Gbx.ZLib = new ZLib();

var gbx = Gbx.Parse(args[0]);

gbx.ToJson(Console.Out);