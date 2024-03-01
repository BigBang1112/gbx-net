using System.Text.Json.Serialization;
using System.Text.Json;
using TmEssentials;

namespace GBX.NET.Json.Converters;

public class JsonNumberTimeSingleConverter : JsonConverter<TimeSingle>
{
    public override TimeSingle Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return new(reader.GetSingle());
    }

    public override void Write(Utf8JsonWriter writer, TimeSingle value, JsonSerializerOptions options)
    {
        writer.WriteNumberValue(value.TotalSeconds);
    }
}