using GBX.NET.Engines.MwFoundations;
using GBX.NET.NewtonsoftJson.Converters;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace GBX.NET.NewtonsoftJson;

public static class JsonExtensions
{
    private static readonly JsonSerializerSettings settings = new()
    {
        TypeNameHandling = TypeNameHandling.Auto,
        NullValueHandling = NullValueHandling.Ignore
    };

    static JsonExtensions()
    {
        settings.Converters.Add(new TimeInt32JsonConverter());
        settings.Converters.Add(new NullableTimeInt32JsonConverter());
        settings.Converters.Add(new StringEnumConverter());
    }

    public static string ToJson(this Gbx gbx, bool indented = true)
    {
        return JsonConvert.SerializeObject(gbx, indented ? Formatting.Indented : Formatting.None, settings);
    }

    public static string ToJson(this CMwNod node, bool indented = true)
    {
        return JsonConvert.SerializeObject(node, indented ? Formatting.Indented : Formatting.None, settings);
    }

    public static void ToJson(this Gbx gbx, TextWriter writer, bool indented = true)
    {
        CreateSerializer(indented).Serialize(writer, gbx);
    }

    public static void ToJson(this CMwNod node, TextWriter writer, bool indented = true)
    {
        CreateSerializer(indented).Serialize(writer, node);
    }

    public static void ToJson(this Gbx gbx, Stream stream, bool indented = true)
    {
        using var writer = new StreamWriter(stream);
        CreateSerializer(indented).Serialize(writer, gbx);
    }

    public static void ToJson(this CMwNod node, Stream stream, bool indented = true)
    {
        using var writer = new StreamWriter(stream);
        CreateSerializer(indented).Serialize(writer, node);
    }

    private static JsonSerializer CreateSerializer(bool indented)
    {
        var serializer = JsonSerializer.Create(settings);
        serializer.Formatting = indented ? Formatting.Indented : Formatting.None;
        return serializer;
    }
}
