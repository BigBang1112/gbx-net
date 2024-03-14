using ChunkL.Structure;
using GBX.NET.Generators.Extensions;
using GBX.NET.Generators.Models;
using Microsoft.CodeAnalysis;
using System.Collections.Immutable;
using System.Text;

namespace GBX.NET.Generators.ChunkL;

internal sealed class ChunkLFieldsWriter
{
    private readonly StringBuilder sb;
    private readonly ChunkDataModel chunk;
    private readonly ImmutableArray<ISymbol> existingChunkMembers;
    private readonly SourceProductionContext context;

    private readonly int indent = 2;

    public ChunkLFieldsWriter(StringBuilder sb, ChunkDataModel chunk, ImmutableArray<ISymbol> existingChunkMembers, SourceProductionContext context)
    {
        this.sb = sb;
        this.chunk = chunk;
        this.existingChunkMembers = existingChunkMembers;
        this.context = context;
    }

    internal void Append()
    {
        var alreadyAddedProps = new HashSet<string>();

        if (chunk.ChunkLDefinition is not null)
        {
            var unknownCounter = 0;
            AppendFieldsRecursive(chunk.ChunkLDefinition.Members, alreadyAddedProps, ref unknownCounter);
        }
    }

    private void AppendFieldsRecursive(List<IChunkMember> members, HashSet<string> alreadyAddedFields, ref int unknownCounter)
    {
        foreach (var member in members)
        {
            if (member is IChunkMemberBlock block)
            {
                AppendFieldsRecursive(block.Members, alreadyAddedFields, ref unknownCounter);
                continue;
            }
            
            if (member is ChunkProperty prop)
            {
                if (!IsUnknownProperty(prop.Name) || prop.Type.IsKeyword() || alreadyAddedFields.Contains(prop.Name))
                {
                    continue;
                }

                ++unknownCounter;
                var fieldName = IsExplicitUnknownProperty(prop.Name) ? prop.Name : $"U{unknownCounter:00}";

                alreadyAddedFields.Add(fieldName);

                var mappedType = prop is ChunkEnum enumProp ? PropertyTypeExtensions.MapType(enumProp.EnumType) : prop.Type.ToCSharpType();

                if (!string.IsNullOrWhiteSpace(prop.Description))
                {
                    sb.AppendLine(indent, "/// <summary>");
                    sb.Append(indent, "/// ");
                    sb.AppendLine(prop.Description);
                    sb.AppendLine(indent, "/// </summary>");
                }

                sb.Append(indent, "public ");
                sb.Append(mappedType);

                var nullable = prop.IsNullable || (prop.Type.IsReferenceType() && string.IsNullOrEmpty(prop.DefaultValue));

                if (nullable)
                {
                    sb.Append('?');
                }

                sb.Append(' ');
                sb.Append(fieldName);

                if (!string.IsNullOrWhiteSpace(prop.DefaultValue))
                {
                    sb.Append(" = ");

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

                sb.AppendLine(";");
            }
        }
    }

    private bool IsUnknownProperty(string? name)
    {
        return string.IsNullOrWhiteSpace(name) || IsExplicitUnknownProperty(name);
    }

    private bool IsExplicitUnknownProperty(string? name)
    {
        return name?.Length == 3 && name[0] == 'U' && char.IsDigit(name[1]) && char.IsDigit(name[2]);
    }
}
