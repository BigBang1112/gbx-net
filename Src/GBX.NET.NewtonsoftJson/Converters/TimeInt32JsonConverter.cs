using Newtonsoft.Json;
using TmEssentials;

namespace GBX.NET.NewtonsoftJson.Converters;

public sealed class TimeInt32JsonConverter : JsonConverter
{
    public override bool CanConvert(Type objectType)
    {
        return objectType == typeof(TimeInt32);
    }

    public override object ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
    {
        return new TimeInt32(Convert.ToInt32(reader.Value));
    }

    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
    {
        writer.WriteValue(((TimeInt32?)value)?.TotalMilliseconds);
    }
}
