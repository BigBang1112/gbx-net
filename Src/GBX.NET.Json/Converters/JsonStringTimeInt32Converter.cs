using System.Text.Json.Serialization;
using System.Text.Json;
using TmEssentials;

namespace GBX.NET.Json.Converters;

public class JsonStringTimeInt32Converter : JsonConverter<TimeInt32>
{
    public override TimeInt32 Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }

    public override void Write(Utf8JsonWriter writer, TimeInt32 value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString());
    }
}