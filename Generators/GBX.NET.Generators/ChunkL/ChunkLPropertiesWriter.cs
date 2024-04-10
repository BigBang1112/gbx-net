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
    private readonly SourceProductionContext context;

    public ChunkLPropertiesWriter(
        StringBuilder sb,
        ClassDataModel? classInfo,
        ArchiveDataModel? archiveInfo,
        Dictionary<string, IPropertySymbol> alreadyExistingProperties,
        int indent,
        bool autoProperty,
        SourceProductionContext context)
    {
        this.sb = sb;
        this.classInfo = classInfo;
        this.archiveInfo = archiveInfo;
        this.alreadyExistingProperties = alreadyExistingProperties;
        this.indent = indent;
        this.autoProperty = autoProperty;
        this.context = context;
    }

    internal void Append()
    {
        var unknownCounter = 0;
        var alreadyAddedProps = new HashSet<string>();

        if (classInfo is not null)
        {
            foreach (var item in classInfo.HeaderChunks.Concat(classInfo.Chunks))
            {
                if (item.Value.ChunkLDefinition is null || item.Value.ChunkLDefinition.Properties.ContainsKey("demonstration"))
                {
                    continue;
                }

                AppendPropertiesRecursive(item.Value.ChunkLDefinition.Members, alreadyAddedProps, includeUnknown: false, ref unknownCounter);
            }
        }

        if (archiveInfo?.ChunkLDefinition is not null)
        {
            AppendPropertiesRecursive(archiveInfo.ChunkLDefinition.Members, alreadyAddedProps, includeUnknown: true, ref unknownCounter);
        }
    }

    private void AppendPropertiesRecursive(List<IChunkMember> members, HashSet<string> alreadyAddedProps, bool includeUnknown, ref int unknownCounter)
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

            var mappedType = prop is ChunkEnum enumProp ? PropertyTypeExtensions.MapType(enumProp.EnumType) : prop.Type.ToCSharpType();

            var nullable = prop.IsNullable || (prop.Type.IsReferenceType() && string.IsNullOrEmpty(prop.DefaultValue));
            var fieldName = GetFieldName(propName);

            var isExternal = prop.Properties?.ContainsKey("external") ?? false;

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
                sb.AppendLine(" { get; set; }");
            }
            else
            {
                sb.Append(" { get => ");

                if (isExternal)
                {
                    sb.Append(fieldName);
                    sb.Append("File?.GetNode(ref ");
                    sb.Append(fieldName);
                    sb.Append(") ?? ");
                }

                sb.Append(fieldName);
                sb.Append("; set => ");
                sb.Append(fieldName);
                sb.AppendLine(" = value; }");
            }

            if (isExternal)
            {
                sb.Append(indent, "    private Components.GbxRefTableFile? ");
                sb.Append(fieldName);
                sb.AppendLine("File;");
                sb.Append(indent, "    public Components.GbxRefTableFile? ");
                sb.Append(propName);
                sb.Append("File { get => ");
                sb.Append(fieldName);
                sb.Append("File; set => ");
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
                sb.Append("(GbxReadSettings settings = default) => ");
                sb.Append(fieldName);
                sb.Append("File?.GetNode(ref ");
                sb.Append(fieldName);
                sb.Append(", settings) ?? ");
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

        if (fieldName is "params" or "class")
        {
            return '@' + fieldName;
        }

        return fieldName;
    }
}
