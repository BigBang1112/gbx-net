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
    private readonly SourceProductionContext context;

    public ChunkLPropertiesWriter(
        StringBuilder sb,
        ClassDataModel? classInfo,
        ArchiveDataModel? archiveInfo,
        Dictionary<string, IPropertySymbol> alreadyExistingProperties,
        int indent,
        SourceProductionContext context)
    {
        this.sb = sb;
        this.classInfo = classInfo;
        this.archiveInfo = archiveInfo;
        this.alreadyExistingProperties = alreadyExistingProperties;
        this.indent = indent;
        this.context = context;
    }

    internal void Append()
    {
        var alreadyAddedProps = new HashSet<string>();

        if (classInfo is not null)
        {
            foreach (var item in classInfo.HeaderChunks.Concat(classInfo.Chunks))
            {
                if (item.Value.ChunkLDefinition is null)
                {
                    continue;
                }

                AppendPropertiesRecursive(item.Value.ChunkLDefinition.Members, alreadyAddedProps);
            }
        }

        if (archiveInfo?.ChunkLDefinition is not null)
        {
            AppendPropertiesRecursive(archiveInfo.ChunkLDefinition.Members, alreadyAddedProps);
        }
    }

    private void AppendPropertiesRecursive(List<IChunkMember> members, HashSet<string> alreadyAddedProps)
    {
        foreach (var member in members)
        {
            if (member is IChunkMemberBlock block)
            {
                AppendPropertiesRecursive(block.Members, alreadyAddedProps);
                continue;
            }
            
            if (member is ChunkProperty prop)
            {
                if (IsUnknownProperty(prop.Name) || alreadyExistingProperties.ContainsKey(prop.Name) || alreadyAddedProps.Contains(prop.Name))
                {
                    continue;
                }

                alreadyAddedProps.Add(prop.Name);

                var mappedType = prop.Type.ToCSharpType();
                var fieldName = char.ToLowerInvariant(prop.Name[0]) + prop.Name.Substring(1);

                sb.AppendLine();
                sb.Append(indent, "    private ");
                sb.Append(mappedType);

                if (!prop.Type.IsReferenceType() && prop.IsNullable)
                {
                    sb.Append('?');
                }

                sb.Append(' ');
                sb.Append(fieldName);
                sb.AppendLine(";");

                if (!string.IsNullOrWhiteSpace(prop.Description))
                {
                    sb.AppendLine(indent, "    /// <summary>");
                    sb.Append(indent, "    /// ");
                    sb.AppendLine(prop.Description);
                    sb.AppendLine(indent, "    /// </summary>");
                }

                sb.Append(indent, "    public ");
                sb.Append(mappedType);

                if (prop.IsNullable)
                {
                    sb.Append('?');
                }

                sb.Append(' ');
                sb.Append(prop.Name);

                sb.AppendLine();
                sb.AppendLine(indent, "    {");
                sb.Append(indent, "        get => ");
                sb.Append(fieldName);
                sb.AppendLine(";");
                sb.Append(indent, "        set => ");
                sb.Append(fieldName);
                sb.AppendLine(" = value;");
                sb.AppendLine(indent, "    }");
            }
        }
    }

    private bool IsUnknownProperty(string? name)
    {
        return string.IsNullOrWhiteSpace(name) || (name?.Length == 3 && name[0] == 'U' && char.IsDigit(name[1]) && char.IsDigit(name[2]));
    }
}
