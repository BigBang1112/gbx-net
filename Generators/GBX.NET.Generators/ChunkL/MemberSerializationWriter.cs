using ChunkL.Structure;
using GBX.NET.Generators.Extensions;
using Microsoft.CodeAnalysis;
using System.Text;

namespace GBX.NET.Generators.ChunkL;

internal sealed class MemberSerializationWriter
{
    private readonly StringBuilder sb;
    private readonly SerializationType serializationType;
    private readonly bool self;
    private readonly SourceProductionContext context;

    private int unknownCounter;

    public MemberSerializationWriter(
        StringBuilder sb, 
        SerializationType serializationType,
        bool self,
        SourceProductionContext context)
    {
        this.sb = sb;
        this.serializationType = serializationType;
        this.self = self;
        this.context = context;
    }

    public void Append(int indent, IEnumerable<IChunkMember> members)
    {
        foreach (var member in members)
        {
            AppendMember(indent, member);
        }
    }

    private void AppendMember(int indent, IChunkMember member)
    {
        if (member is IChunkMemberBlock memberBlock)
        {
            switch (memberBlock)
            {
                case ChunkVersion version:
                    sb.Append(indent, "if (");

                    if (self)
                    {
                        sb.Append("version");
                    }
                    else
                    {
                        sb.Append("Version");
                    }

                    sb.Append(' ');

                    switch (version.Operator)
                    {
                        case "=":
                            sb.Append("==");
                            break;
                        case "+":
                            sb.Append(">=");
                            break;
                        case "-":
                            sb.Append("<=");
                            break;
                        default:
                            throw new NotImplementedException();
                    }

                    sb.Append(' ');
                    sb.Append(version.Number);

                    sb.AppendLine(")");
                    break;
                case ChunkIfStatement ifStatement:
                    sb.Append(indent, "if (");

                    /*if (!self && !isUnknown)
                    {
                        sb.Append("n.");
                    }*/

                    sb.Append(ifStatement.Left);
                    sb.Append(' ');
                    sb.Append(ifStatement.Operator);
                    sb.Append(' ');
                    sb.Append(ifStatement.Right);
                    sb.AppendLine(")");
                    break;
                default:
                    throw new NotImplementedException();
            }

            sb.AppendLine(indent, "{");
            Append(indent + 1, memberBlock.Members);
            sb.AppendLine(indent, "}");

            return;
        }

        if (member is ChunkProperty chunkProperty)
        {
            switch (chunkProperty.Type.PrimaryType)
            {
                case "return":
                    sb.Append(indent, "return;");
                    break;
                case "throw":
                    sb.Append(indent, "throw new Exception();");
                    break;
                case "base":
                    sb.Append(indent, "base.");

                    switch (serializationType)
                    {
                        case SerializationType.Read:
                            sb.Append("Read(n, r);");
                            break;
                        case SerializationType.Write:
                            sb.Append("Write(n, w);");
                            break;
                        case SerializationType.ReadWrite:
                            sb.Append("ReadWrite(n, rw);");
                            break;
                        default:
                            throw new NotImplementedException();
                    }

                    break;
                default:
                    switch (serializationType)
                    {
                        case SerializationType.Read:
                            AppendRead(indent, chunkProperty);
                            break;
                        case SerializationType.Write:
                            AppendWrite(indent, chunkProperty);
                            break;
                        case SerializationType.ReadWrite:
                            AppendReadWrite(indent, chunkProperty);
                            break;
                        default:
                            throw new NotImplementedException();
                    }
                    break;
            }

            if (!string.IsNullOrWhiteSpace(chunkProperty.Description))
            {
                sb.Append(" // ");
                sb.Append(chunkProperty.Description);
            }

            sb.AppendLine();
        }
    }

    private void AppendRead(int indent, ChunkProperty chunkProperty)
    {
        var isUnknown = string.IsNullOrEmpty(chunkProperty.Name);
        var name = isUnknown
            ? $"U{++unknownCounter:00}"
            : char.ToLowerInvariant(chunkProperty.Name[0]) + chunkProperty.Name.Substring(1);

        sb.AppendIndent(indent);

        if (!self && !isUnknown)
        {
            sb.Append("n.");
        }

        sb.Append(name);
        sb.Append(" = ");

        if (chunkProperty is ChunkEnum chunkEnum)
        {
            sb.Append('(');
            sb.Append(chunkEnum.EnumType);
            sb.Append(')');
        }

        sb.Append("r.Read");

        if (chunkProperty.Type.IsArray)
        {
            sb.Append("Array");
        }

        var mappedType = MapType(chunkProperty.Type.PrimaryType, out var noMatch);

        if (noMatch)
        {
            sb.Append("NodeRef<");
            sb.Append(mappedType);
            sb.Append(">");
        }
        else
        {
            sb.Append(mappedType);
        }

        sb.Append("()");
        sb.Append(";");
    }

    private void AppendWrite(int indent, ChunkProperty chunkProperty)
    {

    }

    private void AppendReadWrite(int indent, ChunkProperty chunkProperty)
    {
        sb.Append(indent, "rw.");

        if (chunkProperty.Type.PrimaryType == "version")
        {
            sb.Append("VersionInt32(this);");
            return;
        }

        if (chunkProperty.Type.PrimaryType == "versionb")
        {
            sb.Append("VersionByte(this);");
            return;
        }

        if (chunkProperty.Type.IsArray)
        {
            sb.Append("Array");
        }

        if (chunkProperty is ChunkEnum)
        {
            sb.Append("Enum");
        }

        var mappedType = MapType(chunkProperty.Type.PrimaryType, out var noMatch);

        if (noMatch)
        {
            sb.Append("NodeRef<");
            sb.Append(mappedType);
            sb.Append(">");
        }
        else if (chunkProperty.Type.IsArray && PropertyTypeExtensions.IsValueType(chunkProperty.Type.PrimaryType))
        {
            sb.Append("<");
            sb.Append(PropertyTypeExtensions.MapType(chunkProperty.Type.PrimaryType));
            sb.Append(">");
        }
        else
        {
            sb.Append(mappedType);
        }

        if (chunkProperty is ChunkEnum chunkEnum)
        {
            sb.Append('<');
            sb.Append(chunkEnum.EnumType);
            sb.Append('>');
        }

        sb.Append("(ref ");

        var isUnknown = string.IsNullOrEmpty(chunkProperty.Name);

        if (!self && !isUnknown)
        {
            sb.Append("n.");
        }

        var name = isUnknown
            ? $"U{++unknownCounter:00}"
            : char.ToLowerInvariant(chunkProperty.Name[0]) + chunkProperty.Name.Substring(1);
        sb.Append(name);

        sb.Append(")");
        sb.Append(";");
    }

    public static string MapType(string type, out bool noMatch)
    {
        noMatch = false;

        return type.ToLowerInvariant() switch
        {
            "int64" or "long" => nameof(Int64),
            "uint64" or "ulong" => nameof(UInt64),
            "uint32" or "uint" => nameof(UInt32),
            "int32" or "int" => nameof(Int32),
            "uint16" or "ushort" => nameof(UInt16),
            "int16" or "short" => nameof(Int16),
            "uint8" or "byte" => nameof(Byte),
            "int8" or "sbyte" => nameof(SByte),
            "float" => nameof(Single),
            "bool" => nameof(Boolean),
            "string" => nameof(String),
            "vec2" => "Vec2",
            "vec3" => "Vec3",
            "vec4" => "Vec4",
            "int2" => "Int2",
            "int3" => "Int3",
            "int4" => "Int4",
            "byte3" => "Byte3",
            "box" => "Box",
            "quat" => "Quat",
            "color" => "Color",
            "timeint" or "timeint32" => "TimeInt32",
            "timefloat" or "timesingle" => "TimeSingle",
            "timeofday" => "TimeOfDay",
            "ident" or "meta" => "Ident",
            "id" or "lookbackstring" => "IdAsString",
            "packdesc" or "fileref" => "PackDesc",
            "list" => "List",
            "data" => "Data",
            _ => Default(type, out noMatch),
        };

        static string Default(string type, out bool noMatch)
        {
            noMatch = true;
            return type;
        }
    }
}
