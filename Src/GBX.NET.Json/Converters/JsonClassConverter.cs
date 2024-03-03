using System.Text.Json.Serialization;
using System.Text.Json;
using System.Diagnostics.CodeAnalysis;
using GBX.NET.Engines.MwFoundations;

namespace GBX.NET.Json.Converters;

#if NET7_0_OR_GREATER
[RequiresUnreferencedCode("Uses JsonSerializer.Serialize behind the scenes.")]
[RequiresDynamicCode("Uses JsonSerializer.Serialize behind the scenes.")]
#endif
public class JsonClassConverter : JsonConverter<CMwNod>
{
    public override CMwNod? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }

    public override void Write(Utf8JsonWriter writer, CMwNod value, JsonSerializerOptions options)
    {
        JsonSerializer.Serialize(writer, value, value.GetType(), options);
    }
}