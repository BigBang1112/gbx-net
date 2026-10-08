using ChunkL.Syntax;

namespace GBX.NET.Generators.Generation;

internal static class WireTypes
{
    private static readonly Dictionary<string, string> Types = new(StringComparer.OrdinalIgnoreCase)
    {
        ["int8"] = "sbyte",
        ["int16"] = "short",
        ["int32"] = "int",
        ["int64"] = "long",
        ["uint8"] = "byte",
        ["uint16"] = "ushort",
        ["uint32"] = "uint",
        ["uint64"] = "ulong",
        ["int128"] = "Int128",
        ["uint128"] = "UInt128",
        ["uint256"] = "UInt256",
        ["checksum128"] = "Checksum128",
        ["checksum256"] = "Checksum256",
        ["vec2"] = "Vec2",
        ["vec3"] = "Vec3",
        ["vec3_6"] = "Vec3",
        ["vec4"] = "Vec4",
        ["int2"] = "Int2",
        ["int3"] = "Int3",
        ["int4"] = "Int4",
        ["byte3"] = "Byte3",
        ["mat3"] = "Mat3",
        ["mat4"] = "Mat4",
        ["iso4"] = "Iso4",
        ["boxaligned"] = "BoxAligned",
        ["box"] = "BoxAligned",
        ["boxint3"] = "BoxInt3",
        ["quat"] = "Quat",
        ["color"] = "Color",
        ["rect"] = "Rect",
        ["transquat"] = "TransQuat",
        ["timeofday"] = "TimeSpan",
        ["filetime"] = "DateTime",
        ["systemtime"] = "DateTime",
        ["unixtime"] = "DateTimeOffset",
        ["ipv4"] = "global::System.Net.IPAddress",
        ["ident"] = "Ident",
        ["meta"] = "Ident",
        ["id"] = "string",
        ["lookbackstring"] = "string",
        ["packdesc"] = "PackDesc",
        ["fileref"] = "PackDesc",
        ["data"] = "byte[]",
        ["datauint"] = "uint",
        ["datauint32"] = "uint",
        ["dataint"] = "int",
        ["dataint32"] = "int",
        ["dataulong"] = "ulong",
        ["datauint64"] = "ulong",
        ["datalong"] = "long",
        ["dataint64"] = "long",
        ["boolbyte"] = "bool",
        ["booltext"] = "bool",
        ["optimizedint"] = "int",
        ["node"] = "CMwNod",
        ["version"] = "int",
        ["versionb"] = "int"
    };

    private static readonly Dictionary<string, string> Methods = new(StringComparer.OrdinalIgnoreCase)
    {
        ["int"] = "Int32",
        ["uint"] = "UInt32",
        ["short"] = "Int16",
        ["ushort"] = "UInt16",
        ["byte"] = "Byte",
        ["sbyte"] = "SByte",
        ["long"] = "Int64",
        ["ulong"] = "UInt64",
        ["float"] = "Single",
        ["double"] = "Double",
        ["bool"] = "Boolean",
        ["string"] = "String",
        ["id"] = "Id",
        ["lookbackstring"] = "Id",
        ["data"] = "Data",
        ["vec3_6"] = "Vec3_6",
        ["timeofday"] = "TimeOfDay",
        ["filetime"] = "FileTime",
        ["systemtime"] = "SystemTime",
        ["unixtime"] = "UnixTime",
        ["ipv4"] = "IPv4",
        ["datauint"] = "DataUInt32",
        ["datauint32"] = "DataUInt32",
        ["dataint"] = "DataInt32",
        ["dataint32"] = "DataInt32",
        ["dataulong"] = "DataUInt64",
        ["datauint64"] = "DataUInt64",
        ["datalong"] = "DataInt64",
        ["dataint64"] = "DataInt64",
        ["optimizedint"] = "OptimizedInt"
    };

    public static string Map(string name, AttributeList? attributes = null)
    {
        var mapped = Types.TryGetValue(name, out var type) ? type : name;
        if (LayoutModel.Has(attributes, "time"))
        {
            return mapped switch
            {
                "int" => "TimeInt32",
                "float" => "TimeSingle",
                _ => throw new NotSupportedException("The time attribute requires an int or float field.")
            };
        }

        return mapped;
    }

    public static bool Primitive(string name)
        => Types.ContainsKey(name) || Methods.ContainsKey(name);

    public static bool Value(string name)
    {
        return Map(name) is not ("string" or "Ident" or "PackDesc" or "byte[]" or "CMwNod" or "global::System.Net.IPAddress") && Primitive(name);
    }

    public static string Method(string name, AttributeList? attributes = null)
    {
        if (LayoutModel.Has(attributes, "time"))
        {
            return Map(name, attributes);
        }

        return Methods.TryGetValue(name, out var method) ? method :
            Methods.TryGetValue(Map(name), out method) ? method : Map(name);
    }

    public static string CSharp(FieldDeclaration field)
        => CSharp(field.Type, field.Attributes);

    public static string CSharp(TypeReference type, AttributeList? attributes = null)
    {
        if (type.Name == "data")
        {
            return "byte[]";
        }

        var element = Cast(type) ?? Map(type.Name, attributes);
        var array = type.ArrayDimensions > 0;

        if (array && LayoutModel.Has(attributes, "external"))
        {
            element = "External<" + element + ">";
        }

        if (array)
        {
            if (type.IsNullable)
            {
                element += "?";
            }

            return LayoutModel.Has(attributes, "list") ? "List<" + element + ">" :
                element + string.Concat(Enumerable.Repeat("[]", type.ArrayDimensions));
        }

        return element;
    }

    public static bool Nullable(FieldModel field)
    {
        return field.Occurrences.Any(static x => x.Type.IsNullable) ||
            field.Declaration.Type.Name is "systemtime" or "filetime" or "unixtime" or "timeofday" ||
            ((field.Declaration.Type.ArrayDimensions > 0 || (!Value(field.Declaration.Type.Name) && field.Declaration.Type.CastTarget is null)) &&
                field.Declaration.DefaultValue is null);
    }

    public static string? Cast(TypeReference type)
    {
        return type.CastTarget is null ? null :
            (string.IsNullOrEmpty(type.CastTarget.QualifyingType) ? "" : type.CastTarget.QualifyingType + ".") + type.CastTarget.Name;
    }
}
