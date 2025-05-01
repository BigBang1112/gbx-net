using ChunkL.Structure;
using GBX.NET.Generators.Extensions;
using GBX.NET.Generators.Models;
using Microsoft.CodeAnalysis;
using System.Text;

namespace GBX.NET.Generators.ChunkL;

internal class ChunkLPropertiesWriter
{
    private readonly StringBuilder sb;
    private readonly ClassDataModel? classInfo;
    private readonly ArchiveDataModel? archiveInfo;
    private readonly Dictionary<string, IPropertySymbol> alreadyExistingProperties;
    private readonly int indent;
    private readonly bool autoProperty;
    private readonly bool privateSet;
    private readonly SourceProductionContext context;
    private readonly Dictionary<string, List<string>> appliedWithChunkDictionary = [];

    public ChunkLPropertiesWriter(
        StringBuilder sb,
        ClassDataModel? classInfo,
        ArchiveDataModel? archiveInfo,
        Dictionary<string, IPropertySymbol> alreadyExistingProperties,
        int indent,
        bool autoProperty,
        bool privateSet,
        SourceProductionContext context)
    {
        this.sb = sb;
        this.classInfo = classInfo;
        this.archiveInfo = archiveInfo;
        this.alreadyExistingProperties = alreadyExistingProperties;
        this.indent = indent;
        this.autoProperty = autoProperty;
        this.privateSet = privateSet;
        this.context = context;

        PopulateAppliedWithChunkDictionary(appliedWithChunkDictionary, GetChunkLChunkMembers());
    }

    private void PopulateAppliedWithChunkDictionary(IDictionary<string, List<string>> appliedWithChunkDict, IEnumerable<(IChunkMember, ChunkDefinition)> members)
    {
        foreach (var pair in members)
        {
            var member = pair.Item1;
            var chunk = pair.Item2;

            if (member is IChunkMemberBlock block)
            {
                PopulateAppliedWithChunkDictionary(appliedWithChunkDict, block.Members.Select(x => (x, chunk)));
                continue;
            }

            if (member is not ChunkProperty prop || string.IsNullOrEmpty(prop.Name))
            {
                continue;
            }

            var chunkId = chunk.Id > 0xFFF
                ? chunk.Id
                : ((classInfo?.Id ?? 0) + chunk.Id);

            var chunkClass = (chunk.Properties.ContainsKey("header") ? "HeaderChunk" : "Chunk") + chunkId.ToString("X8");

            if (appliedWithChunkDict.ContainsKey(prop.Name))
            {
                appliedWithChunkDict[prop.Name].Add(chunkClass);
            }
            else
            {
                appliedWithChunkDict[prop.Name] = [chunkClass];
            }
        }
    }

    private IEnumerable<(IChunkMember, ChunkDefinition)> GetChunkLChunkMembers()
    {
        if (classInfo is null)
        {
            yield break;
        }

        foreach (var item in classInfo.HeaderChunks.Concat(classInfo.Chunks))
        {
            if (item.Value.ChunkLDefinition is not null && !item.Value.ChunkLDefinition.Properties.ContainsKey("demonstration"))
            {
                foreach (var member in item.Value.ChunkLDefinition.Members)
                {
                    yield return (member, item.Value.ChunkLDefinition);
                }
            }
        }
    }

    private IEnumerable<IChunkMember> GetChunkLArchiveMembers()
    {
        if (archiveInfo?.ChunkLDefinition is not null)
        {
            foreach (var member in archiveInfo.ChunkLDefinition.Members)
            {
                yield return member;
            }
        }
    }

    internal void Append()
    {
        var unknownCounter = 0;
        var alreadyAddedProps = new HashSet<string>();

        AppendPropertiesRecursive(GetChunkLChunkMembers().Select(x => x.Item1), alreadyAddedProps, includeUnknown: false, ref unknownCounter);
        AppendPropertiesRecursive(GetChunkLArchiveMembers(), alreadyAddedProps, includeUnknown: true, ref unknownCounter);
    }

    private void AppendPropertiesRecursive(IEnumerable<IChunkMember> members, HashSet<string> alreadyAddedProps, bool includeUnknown, ref int unknownCounter)
    {
        foreach (var member in members)
        {
            if (member is IChunkMemberBlock block)
            {
                AppendPropertiesRecursive(block.Members, alreadyAddedProps, includeUnknown, ref unknownCounter);
                continue;
            }

            if (member is not ChunkProperty prop)
            {
                continue;
            }

            if ((!includeUnknown && IsUnknownProperty(prop.Name)) || prop.Type.IsKeyword() || alreadyExistingProperties.ContainsKey(prop.Name) || alreadyAddedProps.Contains(prop.Name))
            {
                continue;
            }

            var propName = string.IsNullOrEmpty(prop.Name) ? $"U{++unknownCounter:00}" : prop.Name;

            alreadyAddedProps.Add(propName);

            var isExternal = prop.Properties?.ContainsKey("external") ?? false;

            var mappedType = prop is ChunkEnum enumProp ? PropertyTypeExtensions.MapType(enumProp.EnumType) : prop.Type.ToCSharpType(isExternal);

            var nullable = prop.IsNullable || (prop.Type.IsReferenceType() && string.IsNullOrEmpty(prop.DefaultValue));
            var fieldName = GetFieldName(propName);

            // full property is forced for external for "behind the scenes" loading
            if (!autoProperty || isExternal)
            {
                sb.AppendLine();
                sb.Append(indent, "    private ");
                sb.Append(mappedType);

                if (nullable)
                {
                    sb.Append('?');
                }

                sb.Append(' ');
                sb.Append(fieldName);

                if (!string.IsNullOrWhiteSpace(prop.DefaultValue))
                {
                    AppendDefaultValue(prop, mappedType);
                }

                sb.AppendLine(";");
            }

            if (!string.IsNullOrWhiteSpace(prop.Description))
            {
                sb.AppendLine(indent, "    /// <summary>");
                sb.Append(indent, "    /// ");
                sb.AppendLine(prop.Description);
                sb.AppendLine(indent, "    /// </summary>");
            }

            if (classInfo?.Id is not null && appliedWithChunkDictionary.TryGetValue(propName, out var appliedWithChunks))
            {
                foreach (var chunkClass in appliedWithChunks)
                {
                    sb.Append(indent, "    [AppliedWithChunk<");
                    sb.Append(chunkClass);
                    sb.AppendLine(">]");
                }
            }

            sb.Append(indent, "    public ");
            sb.Append(mappedType);

            if (nullable)
            {
                sb.Append('?');
            }

            sb.Append(' ');
            sb.Append(propName);

            if (autoProperty && !isExternal)
            {
                sb.Append(" { get; ");

                if (privateSet)
                {
                    sb.Append("private ");
                }

                sb.Append("set; }");

                if (!string.IsNullOrWhiteSpace(prop.DefaultValue))
                {
                    AppendDefaultValue(prop, mappedType);
                    sb.Append(";");
                }

                sb.AppendLine();
            }
            else
            {
                sb.Append(" { get => ");

                if (isExternal && !prop.Type.IsArray)
                {
                    sb.Append(fieldName);
                    sb.Append("File?.GetNode(ref ");
                    sb.Append(fieldName);
                    sb.Append(") ?? ");
                }

                sb.Append(fieldName);
                sb.Append("; ");

                if (privateSet)
                {
                    sb.Append("private ");
                }

                sb.Append("set => ");
                sb.Append(fieldName);
                sb.AppendLine(" = value; }");
            }

            if (isExternal && !prop.Type.IsArray)
            {
                sb.Append(indent, "    private Components.GbxRefTableFile? ");
                sb.Append(fieldName);
                sb.AppendLine("File;");
                sb.Append(indent, "    public Components.GbxRefTableFile? ");
                sb.Append(propName);
                sb.Append("File { get => ");
                sb.Append(fieldName);
                sb.Append("File; ");

                if (privateSet)
                {
                    sb.Append("private ");
                }

                sb.Append("set => ");
                sb.Append(fieldName);
                sb.AppendLine("File = value; }");

                sb.Append(indent, "    public ");
                sb.Append(mappedType);

                if (nullable)
                {
                    sb.Append('?');
                }

                sb.Append(" Get");
                sb.Append(propName);
                sb.Append("(GbxReadSettings settings = default, bool exceptions = false) => ");
                sb.Append(fieldName);
                sb.Append("File?.GetNode(ref ");
                sb.Append(fieldName);
                sb.Append(", settings, exceptions) ?? ");
                sb.Append(fieldName);
                sb.AppendLine(";");
            }
        }
    }

    private void AppendDefaultValue(ChunkProperty prop, string mappedType)
    {
        sb.Append(" = ");

        if (prop.Type.PrimaryType == "list")
        {
            sb.Append("new List<");
            sb.Append(prop.Type.GenericType);
            sb.Append(">()");
        }
        else
        {
            switch (prop.DefaultValue)
            {
                case "empty":
                    sb.Append(mappedType);
                    sb.Append(".Empty");
                    break;
                default:
                    sb.Append(prop.DefaultValue);
                    break;
            }
        }
    }

    private bool IsUnknownProperty(string? name)
    {
        return string.IsNullOrWhiteSpace(name) || (name?.Length == 3 && name[0] == 'U' && char.IsDigit(name[1]) && char.IsDigit(name[2]));
    }

    private static string GetFieldName(string propName)
    {
        var fieldName = char.ToLowerInvariant(propName[0]) + propName.Substring(1);

        if (fieldName is "params" or "class" or "base" or "object")
        {
            return '@' + fieldName;
        }

        return fieldName;
    }
}
