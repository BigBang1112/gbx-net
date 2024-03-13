﻿using ChunkL.Structure;
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
            var fieldName = char.ToLowerInvariant(propName[0]) + propName.Substring(1);

            sb.AppendLine();
            sb.Append(indent, "    private ");
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
            sb.Append(" { get => ");
            sb.Append(fieldName);
            sb.Append("; set => ");
            sb.Append(fieldName);
            sb.AppendLine(" = value; }");
        }
    }

    private bool IsUnknownProperty(string? name)
    {
        return string.IsNullOrWhiteSpace(name) || (name?.Length == 3 && name[0] == 'U' && char.IsDigit(name[1]) && char.IsDigit(name[2]));
    }
}