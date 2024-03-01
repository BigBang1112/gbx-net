using System.Text.Json.Serialization;
using System.Text.Json;
using GBX.NET.Serialization.Chunking;

namespace GBX.NET.Json.Converters;

public class JsonChunkConverter : JsonConverter<IChunk>
{
    public override IChunk? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }

    public override void Write(Utf8JsonWriter writer, IChunk value, JsonSerializerOptions options)
    {
        var type = value.GetType();
        JsonSerializer.Serialize(writer, value, type, options);
    }
}