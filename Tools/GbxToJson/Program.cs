using GBX.NET;
using GBX.NET.Json.Converters;
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

var options = new JsonSerializerOptions
{
    WriteIndented = true,
    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
};
options.Converters.Add(new JsonStringTimeInt32Converter());
options.Converters.Add(new JsonNumberTimeSingleConverter());
options.Converters.Add(new JsonInt3Converter());
options.Converters.Add(new JsonStringEnumConverter());
options.Converters.Add(new JsonChunkConverter());
options.Converters.Add(new JsonClassConverter());

JsonSerializer.Serialize(stream, gbx, gbx.GetType(), options);