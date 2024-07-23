using GBX.NET.Engines.MwFoundations;
using Newtonsoft.Json;

#if NET6_0_OR_GREATER
using System.Diagnostics.CodeAnalysis;
#endif

namespace GBX.NET.NewtonsoftJson;

public static class GbxJson
{
#if NET6_0_OR_GREATER
	[RequiresUnreferencedCode(JsonExtensions.JsonSerializationRequiresUnreferencedCodeMessage)]
#endif
	public static string Serialize(this Gbx gbx, bool indented = true)
	{
		return JsonExtensions.ToJson(gbx, indented);
	}

#if NET6_0_OR_GREATER
	[RequiresUnreferencedCode(JsonExtensions.JsonSerializationRequiresUnreferencedCodeMessage)]
#endif
	public static string Serialize(this CMwNod node, bool indented = true)
	{
		return JsonExtensions.ToJson(node, indented);
	}

#if NET6_0_OR_GREATER
	[RequiresUnreferencedCode(JsonExtensions.JsonSerializationRequiresUnreferencedCodeMessage)]
#endif
	public static void Serialize(this Gbx gbx, TextWriter writer, bool indented = true)
	{
		JsonExtensions.ToJson(gbx, writer, indented);
	}

#if NET6_0_OR_GREATER
	[RequiresUnreferencedCode(JsonExtensions.JsonSerializationRequiresUnreferencedCodeMessage)]
#endif
	public static void Serialize(this CMwNod node, TextWriter writer, bool indented = true)
	{
		JsonExtensions.ToJson(node, writer, indented);
	}

#if NET6_0_OR_GREATER
	[RequiresUnreferencedCode(JsonExtensions.JsonSerializationRequiresUnreferencedCodeMessage)]
#endif
	public static void Serialize(this Gbx gbx, Stream stream, bool indented = true)
	{
		JsonExtensions.ToJson(gbx, stream, indented);
	}

#if NET6_0_OR_GREATER
	[RequiresUnreferencedCode(JsonExtensions.JsonSerializationRequiresUnreferencedCodeMessage)]
#endif
	public static void Serialize(this CMwNod node, Stream stream, bool indented = true)
	{
		JsonExtensions.ToJson(node, stream, indented);
	}

#if NET6_0_OR_GREATER
	[RequiresUnreferencedCode(JsonExtensions.JsonSerializationRequiresUnreferencedCodeMessage)]
#endif
	internal static Gbx<T>? DeserializeGbx<T>(string json) where T : CMwNod
	{
		return JsonConvert.DeserializeObject<Gbx<T>>(json);
	}
}
