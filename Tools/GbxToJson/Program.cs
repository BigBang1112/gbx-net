using GBX.NET;
using GBX.NET.LZO;
using System.Text.Json;
using System.Text.Json.Serialization;

if (args.Length == 0)
{
    Console.WriteLine("Usage: GbxToJson <filename>");
    Console.WriteLine("Press any key to continue...");
    Console.ReadKey(true);
}

Gbx.LZO = new MiniLZO();

var gbx = Gbx.Parse(args[0]);

using var stream = Console.OpenStandardOutput();

JsonSerializer.Serialize(stream, gbx, gbx.GetType(), new JsonSerializerOptions
{
    WriteIndented = true,
    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
});