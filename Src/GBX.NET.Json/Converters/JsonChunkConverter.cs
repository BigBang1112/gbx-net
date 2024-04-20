using System.Text.Json.Serialization;
using System.Text.Json;
using GBX.NET.Serialization.Chunking;
using System.Diagnostics.CodeAnalysis;

namespace GBX.NET.Json.Converters;

#if NET7_0_OR_GREATER
[RequiresUnreferencedCode("Uses JsonSerializer.Serialize behind the scenes.")]
[RequiresDynamicCode("Uses JsonSerializer.Serialize behind the scenes.")]
#endif
public class JsonChunkConverter : JsonConverter<IChunk>
{
    public override IChunk? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }

    public override void Write(Utf8JsonWriter writer, IChunk value, JsonSerializerOptions options)
    {
        JsonSerializer.Serialize(writer, value, value.GetType(), options);
    }
}
