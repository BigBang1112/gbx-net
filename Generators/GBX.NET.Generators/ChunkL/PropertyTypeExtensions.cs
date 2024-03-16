using ChunkL.Structure;
using System.Collections;
using System.Collections.Immutable;
using System.Text;

namespace GBX.NET.Generators.ChunkL;

internal static class PropertyTypeExtensions
{
    private static readonly ImmutableHashSet<string> valueTypes =
    [
        "bool",
        "byte",
        "sbyte",
        "decimal",
        "double",
        "float",
        "int",
        "uint",
        "long",
        "ulong",
        "short",
        "ushort",
        "TimeSpan",
        "Color",
        "Vec2",
        "Vec3",
        "Vec4",
        "Int2",
        "Int3",
        "Int4",
        "Int128",
        "UInt128",
        "UInt256",
        "Byte3",
        "BoxAligned",
        "BoxInt3",
        "Quat",
        "Id",
        "TimeInt32",
        "TimeSingle",
        "Iso4",
        "Rect"
    ];

    private static readonly ImmutableHashSet<string> keywords =
    [
        "return",
        "throw",
        "version",
        "versionb",
        "base"
    ];

    public static bool IsKeyword(this PropertyType propertyType)
    {
        return keywords.Contains(propertyType.PrimaryType);
    }

    public static bool IsSimpleType(this PropertyType propertyType)
    {
        return !propertyType.IsArray
            && !propertyType.IsDeprec
            && string.IsNullOrEmpty(propertyType.GenericType)
            && string.IsNullOrEmpty(propertyType.PrimaryTypeMarker)
            && string.IsNullOrEmpty(propertyType.GenericTypeMarker)
            && string.IsNullOrEmpty(propertyType.ArrayLength);
    }

    public static string ToCSharpType(this PropertyType propertyType)
    {
        var sb = new StringBuilder(MapType(propertyType.PrimaryType));
        if (!string.IsNullOrEmpty(propertyType.GenericType))
        {
            sb.Append('<');
            sb.Append(MapType(propertyType.GenericType));
            sb.Append('>');
        }

        if (propertyType.IsArray && propertyType.PrimaryType != "data")
        {
            sb.Append("[]");
        }

        return sb.ToString();
    }

    public static bool IsReferenceType(this PropertyType propertyType)
    {
        return propertyType.IsArray || !valueTypes.Contains(MapType(propertyType.PrimaryType));
    }

    public static bool IsValueType(string type)
    {
        return valueTypes.Contains(MapType(type));
    }

    public static string MapType(string type)
    {
        return type.ToLowerInvariant() switch
        {
            "int128" => "Int128",
            "uint128" => "UInt128",
            "uint256" => "Int256",
            "int64" => "long",
            "uint64" => "ulong",
            "uint32" => "uint",
            "int32" => "int",
            "uint16" => "ushort",
            "int16" => "short",
            "uint8" => "byte",
            "int8" => "sbyte",
            "vec2" => "Vec2",
            "vec3" => "Vec3",
            "vec4" => "Vec4",
            "int2" => "Int2",
            "int3" => "Int3",
            "int4" => "Int4",
            "byte3" => "Byte3",
            "mat3" => "Mat3",
            "mat4" => "Mat4",
            "iso4" => "Iso4",
            "boxaligned" or "box" => "BoxAligned",
            "boxint3" => "BoxInt3",
            "quat" => "Quat",
            "color" => "Color",
            "rect" => "Rect",
            "timeint" or "timeint32" => "TimeInt32",
            "timefloat" or "timesingle" => "TimeSingle",
            "timeofday" => nameof(TimeSpan),
            "filetime" => nameof(DateTime),
            "ident" or "meta" => "Ident",
            "id" or "lookbackstring" => "string",
            "packdesc" or "fileref" => "PackDesc",
            "list" => nameof(IList),
            "data" => "byte[]",
            "boolbyte" => "bool",
            _ => type
        };
    }
}
