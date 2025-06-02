﻿using ChunkL.Structure;
using GBX.NET.Generators.Extensions;
using GBX.NET.Generators.Models;
using Microsoft.CodeAnalysis;
using System.Collections.Immutable;
using System.Text;
using System.Text.RegularExpressions;

namespace GBX.NET.Generators.ChunkL;

internal sealed class MemberSerializationWriter
{
    private readonly StringBuilder sb;
    private readonly SerializationType serializationType;
    private readonly ArchiveDataModel? archive;
    private readonly bool self;
    private readonly ImmutableDictionary<string, IFieldSymbol> existingFields;
    private readonly ImmutableDictionary<string, IPropertySymbol> existingProperties;
    private readonly ClassDataModel classInfo;
    private readonly ImmutableDictionary<string, ClassDataModel> classes;
    private readonly ImmutableDictionary<string, ArchiveDataModel> archives;
    private readonly bool autoProperty;
    private readonly SourceProductionContext context;

    private int unknownCounter;

    public MemberSerializationWriter(
        StringBuilder sb, 
        SerializationType serializationType,
        ArchiveDataModel? archive,
        ImmutableDictionary<string, IFieldSymbol> existingFields,
        ImmutableDictionary<string, IPropertySymbol> existingProperties,
        ClassDataModel classInfo,
        ImmutableDictionary<string, ClassDataModel> classes,
        ImmutableDictionary<string, ArchiveDataModel> archives,
        bool autoProperty,
        SourceProductionContext context)
    {
        this.sb = sb;
        this.serializationType = serializationType;
        this.archive = archive;
        self = archive is not null;
        this.existingFields = existingFields;
        this.existingProperties = existingProperties;
        this.classInfo = classInfo;
        this.classes = classes;
        this.archives = archives;
        this.autoProperty = autoProperty;
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
                        sb.Append("v");
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

                    var enumDataModel = default(EnumDataModel);
                    var enumValueNext = false;

                    foreach (var part in ifStatement.Condition)
                    {
                        if (enumDataModel is not null && part == "::")
                        {
                            sb.Append('.');
                            enumValueNext = true;
                            continue;
                        }

                        if (enumValueNext)
                        {
                            sb.Append(part);
                            enumValueNext = false;
                            enumDataModel = null;
                            continue;
                        }

                        var isWord = Regex.IsMatch(part, @"^[^0-9]\w+$"); // does not start with a number and is azAZ09_

                        if (isWord)
                        {
                            if (!self && !IsUnknown(part) && part is not "null")
                            {
                                if (!classInfo.Enums.TryGetValue(part, out enumDataModel))
                                {
                                    sb.Append("n.");
                                }
                            }
                        }

                        sb.Append(part);
                    }

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
                case "base":
                    sb.Append(indent, "base.");

                    switch (serializationType)
                    {
                        case SerializationType.Read:
                            if (archive is null)
                            {
                                sb.Append("Read(n, r);");
                            }
                            else
                            {
                                sb.Append("Read(r, ");

                                if (archive.ChunkLDefinition?.Properties.ContainsKey("contextual") == true)
                                {
                                    sb.Append("n, ");
                                }

                                sb.Append("v);");
                            }
                            break;
                        case SerializationType.Write:
                            if (archive is null)
                            {
                                sb.Append("Write(n, w);");
                            }
                            else
                            {
                                sb.Append("Write(w, ");

                                if (archive.ChunkLDefinition?.Properties.ContainsKey("contextual") == true)
                                {
                                    sb.Append("n, ");
                                }

                                sb.Append("v);");
                            }
                            break;
                        case SerializationType.ReadWrite:
                            if (archive is null)
                            {
                                sb.Append("ReadWrite(n, rw);");
                            }
                            else
                            {
                                sb.Append("ReadWrite(rw, ");

                                if (archive.ChunkLDefinition?.Properties.ContainsKey("contextual") == true)
                                {
                                    sb.Append("n, ");
                                }

                                sb.Append("v);");
                            }
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

            return;
        }

        if (member is ChunkThrow chunkThrow)
        {
            sb.Append(indent, "throw new ");
            sb.Append(chunkThrow.Exception);
            sb.Append("(\"");
            sb.Append(chunkThrow.Message);
            sb.Append("\");");

            if (!string.IsNullOrWhiteSpace(chunkThrow.Description))
            {
                sb.Append(" // ");
                sb.Append(chunkThrow.Description);
            }

            sb.AppendLine();
        }
    }

    private void AppendRead(int indent, ChunkProperty chunkProperty)
    {
        var isUnknown = IsUnknown(chunkProperty.Name);
        var name = GetPropName(chunkProperty, isUnknown, isField: !autoProperty);

        sb.AppendIndent(indent);

        if (chunkProperty.Type.PrimaryType is "version" or "versionb")
        {
            sb.Append("Version");
        }
        else
        {
            if (!self && !isUnknown)
            {
                sb.Append("n.");
            }

            sb.Append(name);
        }

        sb.Append(" = ");

        if (chunkProperty is ChunkEnum chunkEnum)
        {
            sb.Append('(');
            sb.Append(chunkEnum.EnumType);
            sb.Append(')');
        }

        sb.Append("r.Read");

        if (chunkProperty.Type.PrimaryType == "version")
        {
            sb.Append("Int32();");
            return;
        }

        if (chunkProperty.Type.PrimaryType == "versionb")
        {
            sb.Append("Byte();");
            return;
        }

        if (chunkProperty.Type.IsArray)
        {
            sb.Append("Array");

            if (chunkProperty.Properties?.ContainsKey("external") == true)
            {
                sb.Append("External");
            }
        }

        AppendAnyRead(chunkProperty, onlyReadable: true);

        sb.Append("(");

        var contextual = archives.TryGetValue(chunkProperty.Type.PrimaryType, out var archiveModel)
            && archiveModel.ChunkLDefinition?.Properties.ContainsKey("contextual") == true;

        var comma = false;

        if (contextual)
        {
            sb.Append('n');
            comma = true;
        }

        if (chunkProperty.Properties?.TryGetValue("version", out var version) == true)
        {
            if (comma)
            {
                sb.Append(", ");
            }

            sb.Append("version: ");
            sb.Append(version);

            comma = true;
        }

        if (chunkProperty.Properties?.ContainsKey("external") == true && !chunkProperty.Type.IsArray)
        {
            if (comma)
            {
                sb.Append(", ");
            }

            sb.Append("out ");

            if (!self && !isUnknown)
            {
                sb.Append("n.");
            }

            sb.Append(char.ToLowerInvariant(name[0]) + name.Substring(1));
            sb.Append("File");
        }

        sb.Append(");");
    }

    private static bool IsUnknown(string name)
    {
        return string.IsNullOrEmpty(name) || (name[0] == 'U' && char.IsDigit(name[1]) && char.IsDigit(name[2]));
    }

    private void AppendWrite(int indent, ChunkProperty chunkProperty)
    {
        sb.Append(indent, "w.Write");

        if (chunkProperty.Type.PrimaryType == "version")
        {
            sb.Append("(Version);");
            return;
        }

        if (chunkProperty.Type.PrimaryType == "versionb")
        {
            sb.Append("((byte)Version);");
            return;
        }

        if (chunkProperty.Type.IsArray && chunkProperty.Type.PrimaryType != "data")
        {
            sb.Append("Array");

            if (chunkProperty.Properties?.ContainsKey("external") == true)
            {
                sb.Append("External");
            }
        }


        if (chunkProperty.Type.PrimaryType == "data")
        {
            sb.Append("Data");
        }

        var mappedType = MapType(chunkProperty.Type.PrimaryType, out var noMatch);

        if (noMatch)
        {
            AppendNodeRefOrArchive(mappedType, chunkProperty.Type.PrimaryTypeMarker,
                chunkProperty.Properties?.ContainsKey("direct") == true,
                chunkProperty.Properties?.ContainsKey("meta") == true);

            if (chunkProperty.Type.IsDeprec)
            {
                sb.Append("_deprec");
            }

            sb.Append('<');
            sb.Append(mappedType);

            if (archives.TryGetValue(chunkProperty.Type.PrimaryType, out var a)
                && a.ChunkLDefinition?.Properties.ContainsKey("contextual") == true)
            {
                sb.Append(", ");
                sb.Append(classInfo.Name);
            }

            sb.Append('>');
        }
        else if (chunkProperty.Type.IsArray && PropertyTypeExtensions.IsValueType(chunkProperty.Type.PrimaryType))
        {
            if (chunkProperty.Type.IsDeprec)
            {
                sb.Append("_deprec");
            }

            sb.Append('<');
            sb.Append(PropertyTypeExtensions.MapType(chunkProperty.Type.PrimaryType));
            sb.Append('>');
        }
        else
        {
            if (mappedType == "IdAsString")
            {
                sb.Append("IdAsString");
            }

            if (mappedType == "List")
            {
                sb.Append("List");
                AppendList(chunkProperty);
            }
            else if (chunkProperty.Type.IsDeprec)
            {
                sb.Append("_deprec");
            }
        }

        sb.Append('(');

        var isUnknown = IsUnknown(chunkProperty.Name);
        var name = GetPropName(chunkProperty, isUnknown, isField: !autoProperty);

        var csharpType = PropertyTypeExtensions.MapType(chunkProperty.Type.PrimaryType);

        // enum or replaced existing field type
        if (chunkProperty is ChunkEnum || ExistingFieldOrPropertyMatchesType(name, csharpType))
        {
            sb.Append('(');
            sb.Append(csharpType);
            sb.Append(')');
        }

        if (!self && !isUnknown)
        {
            sb.Append("n.");
        }

        sb.Append(name);

        if (archives.TryGetValue(chunkProperty.Type.PrimaryType, out var archiveModel)
            && archiveModel.ChunkLDefinition?.Properties.ContainsKey("contextual") == true)
        {
            sb.Append(", n");
        }

        if (chunkProperty.Type.IsArray && !string.IsNullOrEmpty(chunkProperty.Type.ArrayLength))
        {
            sb.Append(", ");
            sb.Append(chunkProperty.Type.ArrayLength);
        }

        if (chunkProperty.Properties?.TryGetValue("version", out var version) == true)
        {
            sb.Append(", version: ");
            sb.Append(version);
        }

        if (chunkProperty.Type.PrimaryType.ToLowerInvariant() == "boolbyte")
        {
            sb.Append(", asByte: true");
        }

        if (chunkProperty.Type.PrimaryType.ToLowerInvariant() == "booltext")
        {
            sb.Append(", type: BoolType.Text");
        }

        if (chunkProperty.Properties?.TryGetValue("prefix", out var prefix) == true)
        {
            if (prefix == "byte")
            {
                sb.Append(", byteLengthPrefix: true");
            }
        }

        if (chunkProperty.Properties?.ContainsKey("external") == true && !chunkProperty.Type.IsArray)
        {
            sb.Append(", ");

            if (!self && !isUnknown)
            {
                sb.Append("n.");
            }

            sb.Append(name);
            sb.Append("File");
        }

        sb.Append(");");
    }

    // weird hacky method
    private bool ExistingFieldOrPropertyMatchesType(string fieldOrPropName, string csharpType)
    {
        if (string.IsNullOrEmpty(fieldOrPropName))
        {
            return false;
        }

        ITypeSymbol type;

        if (existingFields.TryGetValue(fieldOrPropName, out var fieldSymbol))
        {
            type = fieldSymbol.Type;
        }
        else if (existingProperties.TryGetValue(fieldOrPropName, out var propSymbol))
        {
            type = propSymbol.Type;
        }
        else
        {
            return false;
        }

        var fieldType = type.ToDisplayString(new SymbolDisplayFormat(
            SymbolDisplayGlobalNamespaceStyle.OmittedAsContaining,
            miscellaneousOptions: SymbolDisplayMiscellaneousOptions.UseSpecialTypes));

        if (fieldType.Length > 0 && fieldType[fieldType.Length - 1] == '?')
        {
            fieldType = fieldType.Substring(0, fieldType.Length - 1);
        }

        return fieldType != csharpType;
    }

    private void AppendReadWrite(int indent, ChunkProperty chunkProperty)
    {
        var isFieldlessProperty = existingProperties.ContainsKey(chunkProperty.Name)
            && !existingFields.ContainsKey(char.ToLowerInvariant(chunkProperty.Name[0]) + chunkProperty.Name.Substring(1));

        sb.AppendIndent(indent);

        if (isFieldlessProperty)
        {
            if (!self)
            {
                sb.Append("n.");
            }

            sb.Append(chunkProperty.Name);
            sb.Append(" = ");
        }

        sb.Append("rw.");

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

        if (chunkProperty.Type.IsArray && chunkProperty.Type.PrimaryType != "data")
        {
            sb.Append("Array");
        }

        if (chunkProperty is ChunkEnum)
        {
            sb.Append("Enum");
        }

        AppendAnyRead(chunkProperty, onlyReadable: false);

        sb.Append("(");

        if (!isFieldlessProperty)
        {
            sb.Append("ref ");
        }

        var isUnknown = IsUnknown(chunkProperty.Name);
        var name = GetPropName(chunkProperty, isUnknown, isField: true);

        if (!self && !isUnknown)
        {
            sb.Append("n.");
        }
        else if (self && name == "version")
        {
            sb.Append("this.");
        }

        if (isFieldlessProperty)
        {
            sb.Append(chunkProperty.Name);
        }
        else
        {
            sb.Append(name);
        }

        if ((chunkProperty.Type.IsArray && !chunkProperty.Type.PrimaryTypeNullable)
         || (chunkProperty.Type.PrimaryType == "list" && !chunkProperty.Type.GenericTypeNullable))
        {
            sb.Append('!');
        }

        if (chunkProperty.Type.IsArray && !string.IsNullOrEmpty(chunkProperty.Type.ArrayLength))
        {
            sb.Append(", ");
            sb.Append(chunkProperty.Type.ArrayLength);
        }

        if (chunkProperty.Properties?.TryGetValue("version", out var version) == true)
        {
            sb.Append(", version: ");
            sb.Append(version);
        }

        if (chunkProperty.Type.PrimaryType.ToLowerInvariant() == "boolbyte")
        {
            sb.Append(", asByte: true");
        }

        if (chunkProperty.Type.PrimaryType.ToLowerInvariant() == "booltext")
        {
            sb.Append(", type: BoolType.Text");
        }

        if (chunkProperty.Properties?.TryGetValue("prefix", out var prefix) == true)
        {
            if (prefix == "byte")
            {
                sb.Append(", byteLengthPrefix: true");
            }
        }

        if (chunkProperty.Properties?.ContainsKey("external") == true && !chunkProperty.Type.IsArray)
        {
            sb.Append(", ref ");

            if (!self && !isUnknown)
            {
                sb.Append("n.");
            }

            sb.Append(name);
            sb.Append("File");
        }

        sb.Append(")");
        sb.Append(";");
    }

    private void AppendAnyRead(ChunkProperty chunkProperty, bool onlyReadable)
    {
        var mappedType = MapType(chunkProperty.Type.PrimaryType, out var noMatch);

        if (noMatch)
        {
            AppendNodeRefOrArchive(mappedType, chunkProperty.Type.PrimaryTypeMarker,
                chunkProperty.Properties?.ContainsKey("direct") == true,
                chunkProperty.Properties?.ContainsKey("meta") == true);

            if (chunkProperty.Type.IsDeprec)
            {
                sb.Append("_deprec");
            }

            sb.Append('<');
            sb.Append(mappedType);

            if (archives.TryGetValue(chunkProperty.Type.PrimaryType, out var a)
                && a.ChunkLDefinition?.Properties.ContainsKey("contextual") == true)
            {
                sb.Append(", ");
                sb.Append(classInfo.Name);
            }

            sb.Append('>');
        }
        else if (chunkProperty.Type.IsArray
            && PropertyTypeExtensions.IsValueType(chunkProperty.Type.PrimaryType)
            && chunkProperty.Type.PrimaryType != "optimizedint")
        {
            if (chunkProperty.Type.IsDeprec)
            {
                sb.Append("_deprec");
            }

            sb.Append('<');
            sb.Append(PropertyTypeExtensions.MapType(chunkProperty.Type.PrimaryType));
            sb.Append('>');
        }
        else
        {
            if (mappedType == "IdAsString")
            {
                sb.Append("Id");
            }
            else
            {
                sb.Append(mappedType);
            }

            if (mappedType is "TimeInt32" or "TimeSingle" && chunkProperty.IsNullable)
            {
                sb.Append("Nullable");
            }

            if (mappedType == "List")
            {
                AppendList(chunkProperty);
            }
            else if (chunkProperty.Type.IsDeprec)
            {
                sb.Append("_deprec");
            }
        }

        if (onlyReadable)
        {
            return;
        }

        if (chunkProperty is ChunkEnum chunkEnum)
        {
            sb.Append('<');
            sb.Append(chunkEnum.EnumType);
            sb.Append('>');
        }
    }

    private void AppendList(ChunkProperty chunkProperty)
    {
        var genericType = chunkProperty.Type.GenericType;
        var mappedGenericType = MapType(genericType, out var genericNoMatch);

        if (PropertyTypeExtensions.IsValueType(genericType) || genericNoMatch)
        {
            if (genericNoMatch)
            {
                AppendNodeRefOrArchive(genericType, chunkProperty.Type.GenericTypeMarker,
                    chunkProperty.Properties?.ContainsKey("direct") == true,
                    chunkProperty.Properties?.ContainsKey("meta") == true);
            }

            if (chunkProperty.Type.IsDeprec)
            {
                sb.Append("_deprec");
            }

            sb.Append('<');
            sb.Append(mappedGenericType);
            sb.Append('>');
        }
        else
        {
            sb.Append(mappedGenericType);

            if (chunkProperty.Type.IsDeprec)
            {
                sb.Append("_deprec");
            }
        }
    }

    private void AppendNodeRefOrArchive(string type, string typeMarker, bool direct, bool meta)
    {
        var shouldApplyReadableWritable = archives.ContainsKey(type)
            || (classes.TryGetValue(type, out var classData)
                && classData.NamelessArchive is not null
                && typeMarker != "*");

        if (shouldApplyReadableWritable)
        {
            sb.Append(serializationType switch
            {
                SerializationType.Read => "Readable",
                SerializationType.Write => "Writable",
                SerializationType.ReadWrite => "ReadableWritable",
                _ => throw new NotImplementedException()
            });
        }
        else if (meta)
        {
            sb.Append("MetaRef");
        }
        else if (direct)
        {
            sb.Append("Node");
        }
        else
        {
            sb.Append("NodeRef");
        }
    }

    private string GetPropName(ChunkProperty chunkProperty, bool isUnknown, bool isField)
    {
        if (isUnknown)
        {
            ++unknownCounter;
            return IsExplicitUnknownProperty(chunkProperty.Name)
                ? (self && isField ? char.ToLowerInvariant(chunkProperty.Name[0]) + chunkProperty.Name.Substring(1) : chunkProperty.Name)
                : $"{(self && isField ? 'u' : 'U')}{unknownCounter:00}";
        }
        
        if (isField)
        {
            var fieldName = char.ToLowerInvariant(chunkProperty.Name[0]) + chunkProperty.Name.Substring(1);

            if (fieldName is "class" or "params" or "base" or "object")
            {
                return '@' + fieldName;
            }

            return fieldName;
        }
        
        return chunkProperty.Name;
    }

    private bool IsExplicitUnknownProperty(string? name)
    {
        return name?.Length == 3 && name[0] == 'U' && char.IsDigit(name[1]) && char.IsDigit(name[2]);
    }

    public static string MapType(string type, out bool noMatch)
    {
        noMatch = false;

        return type.ToLowerInvariant() switch
        {
            "int128" => "Int128",
            "uint128" => "UInt128",
            "uint256" => "UInt256",
            "int64" or "long" => nameof(Int64),
            "uint64" or "ulong" => nameof(UInt64),
            "uint32" or "uint" => nameof(UInt32),
            "int32" or "int" => nameof(Int32),
            "uint16" or "ushort" => nameof(UInt16),
            "int16" or "short" => nameof(Int16),
            "uint8" or "byte" => nameof(Byte),
            "int8" or "sbyte" => nameof(SByte),
            "float" => nameof(Single),
            "bool" or "boolbyte" or "booltext" => nameof(Boolean),
            "string" => nameof(String),
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
            "transquat" => "TransQuat",
            "timeint" or "timeint32" => "TimeInt32",
            "timefloat" or "timesingle" => "TimeSingle",
            "timeofday" => "TimeOfDay",
            "filetime" => "FileTime",
            "systemtime" => "SystemTime",
            "ident" or "meta" => "Ident",
            "id" or "lookbackstring" => "IdAsString",
            "packdesc" or "fileref" => "PackDesc",
            "list" => "List",
            "data" => "Data",
            "datauint32" or "datauint" => "DataUInt32",
            "dataint32" or "dataint" => "DataInt32",
            "datauint64" or "dataulong" => "DataUInt64",
            "dataint64" or "datalong" => "DataInt64",
            "optimizedint" => "OptimizedInt",
            "node" => "NodeRef",
            _ => Default(type, out noMatch),
        };

        static string Default(string type, out bool noMatch)
        {
            noMatch = true;
            return type;
        }
    }
}
