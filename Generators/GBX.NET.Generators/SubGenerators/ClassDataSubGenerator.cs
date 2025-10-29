﻿using ChunkL.Structure;
using GBX.NET.Generators.ChunkL;
using GBX.NET.Generators.Extensions;
using GBX.NET.Generators.Models;
using Microsoft.CodeAnalysis;
using System.Collections.Immutable;
using System.Globalization;
using System.Text;

namespace GBX.NET.Generators.SubGenerators;

internal class ClassDataSubGenerator
{
    public static void GenerateSource(SourceProductionContext context, ImmutableDictionary<string, ClassDataModel> classInfos)
    {
        var inheritanceInverted = new Dictionary<string, Dictionary<uint, ClassDataModel>>();

        foreach (var classInfoPair in classInfos)
        {
            var classInfo = classInfoPair.Value;

            if (classInfo.Inherits is null)
            {
                continue;
            }

            if (!inheritanceInverted.ContainsKey(classInfo.Inherits))
            {
                inheritanceInverted[classInfo.Inherits] = [];
            }

            inheritanceInverted[classInfo.Inherits].Add(classInfo.Id.GetValueOrDefault(), classInfo);
        }

        foreach (var classInfoPair in classInfos)
        {
            try
            {
                CreateClassCode(context, classInfoPair.Value, classInfos, inheritanceInverted);
            }
            catch (Exception ex)
            {
                context.ReportDiagnostic(Diagnostic.Create(
                    new DiagnosticDescriptor(
                    "GBXNETGEN010",
                    "Error while generating class",
                    "Error while generating class {0}: {1}",
                    "GBX.NET.Generators",
                    DiagnosticSeverity.Error,
                    isEnabledByDefault: true),
                    classInfoPair.Value.TypeSymbol?.Locations.FirstOrDefault(),
                    classInfoPair.Value.Name,
                    ex));
            }
        }
    }

    private static void CreateClassCode(
        SourceProductionContext context,
        ClassDataModel classInfo,
        ImmutableDictionary<string, ClassDataModel> classInfos,
        Dictionary<string, Dictionary<uint, ClassDataModel>> inheritanceInverted)
    {
        var sb = new StringBuilder();

        sb.Append("namespace GBX.NET.Engines.");
        sb.Append(classInfo.Engine);
        sb.AppendLine(";");

        sb.AppendLine();

        if (classInfo.Id is null)
        {
            context.ReportDiagnostic(Diagnostic.Create(
                new DiagnosticDescriptor(
                "GBXNETGEN001",
                "Class has no ID",
                "{0} has no ID defined by ClassAttribute",
                "GBX.NET.Generators",
                DiagnosticSeverity.Error,
                isEnabledByDefault: true),
                classInfo.TypeSymbol?.Locations.FirstOrDefault(),
                classInfo.Name));
        }

        var archiveStructureKind = default(int?);
        var privateSet = false;

        if (classInfo.NamelessArchive is not null)
        {
            var archiveGenOptionsAtt = classInfo.TypeSymbol?
                .GetAttributes()
                .FirstOrDefault(x => x.AttributeClass?.Name == "ArchiveGenerationOptionsAttribute");

            archiveStructureKind = (archiveGenOptionsAtt?.NamedArguments
                .FirstOrDefault(x => x.Key == "StructureKind").Value.Value as int?).GetValueOrDefault();

            privateSet = (archiveGenOptionsAtt?.NamedArguments
                .FirstOrDefault(x => x.Key == "PrivateSet").Value.Value as bool?).GetValueOrDefault();
        }

        AppendClassDefinitionLine(sb, classInfo, archiveStructureKind, classInfos, out var additionalCodePieces);

        sb.AppendLine("{");

        var existingMembers = classInfo.TypeSymbol?.GetMembers() ?? [];

        if (classInfo.Id.HasValue)
        {
            AppendStaticIdMemberLine(sb, classInfo.Id.Value, existingMembers, context);
        }

        sb.AppendLine();
        foreach (var additionalCodePiece in additionalCodePieces)
        {
            sb.AppendLine(indent: 1, additionalCodePiece);
        }

        sb.AppendLine();
        var alreadyExistingProperties = existingMembers.OfType<IPropertySymbol>()
            .ToDictionary(x => x.Name) ?? [];

        sb.AppendLine();

        var propWriter = new ChunkLPropertiesWriter(sb, classInfo, classInfo.NamelessArchive, alreadyExistingProperties, indent: 0, archiveStructureKind == 1, privateSet, context);
        propWriter.Append();

        if (classInfo.TypeSymbol is null || !classInfo.TypeSymbol.Constructors.Any(x => x.Parameters.Length == 0))
        {
            sb.AppendLine();
            AppendDefaultCtorLine(sb, classInfo.Name);
        }

        if (classInfo.NamelessArchive?.ChunkLDefinition?.Members.Count > 0)
        {
            AppendArchiveMethodsLine(sb, classInfo, classInfo.NamelessArchive, existingMembers, classInfos, classInfo.Archives, context, archiveStructureKind, indent: 0, null);
        }

        AppendChunksLine(sb, classInfo, existingMembers, classInfos, classInfo.Archives, context);

        foreach (var chunkInfo in classInfo.ChunksWithNoId)
        {
            context.ReportDiagnostic(Diagnostic.Create(
                new DiagnosticDescriptor(
                "GBXNETGEN002",
                "Chunk has no ID",
                "{0} has no ID defined by ChunkAttribute",
                "GBX.NET.Generators",
                DiagnosticSeverity.Error,
                isEnabledByDefault: true),
                chunkInfo.Locations.FirstOrDefault(),
                chunkInfo.Name));
        }

        sb.AppendLine();
        AppendArchivesLine(sb, classInfo, existingMembers, classInfos, classInfo.Archives, context);

        sb.AppendLine();
        AppendEnumsLine(sb, classInfo, context);

        sb.AppendLine();
        sb.Append("    public static ");

        if (classInfo.Name != "CMwNod")
        {
            sb.Append("new ");
        }

        sb.AppendLine("IClass? New(uint classId) => classId switch");
        sb.AppendLine("    {");

        if (!classInfo.IsAbstract)
        {
            sb.Append("        0x");
            sb.Append(classInfo.Id.GetValueOrDefault().ToString("X8"));
            sb.Append(" => new ");
            sb.Append(classInfo.Name);
            sb.AppendLine("(),");
        }

        RecurseInheritanceInverted(classInfo.Name, []);

        void RecurseInheritanceInverted(string name, HashSet<uint> alreadyAddedInheritance)
        {
            if (!inheritanceInverted.TryGetValue(name, out var classes))
            {
                return;
            }

            foreach (var classIdAndDataModel in classes)
            {
                if (alreadyAddedInheritance.Contains(classIdAndDataModel.Key))
                {
                    continue;
                }

                if (!classIdAndDataModel.Value.IsAbstract)
                {
                    sb.Append("        0x");
                    sb.Append(classIdAndDataModel.Key.ToString("X8"));
                    sb.Append(" => new ");
                    sb.Append(classIdAndDataModel.Value.Name);
                    sb.AppendLine("(),");
                }

                RecurseInheritanceInverted(classIdAndDataModel.Value.Name, alreadyAddedInheritance);
            }
        }


        sb.AppendLine("        _ => null");

        sb.AppendLine("    };");

        sb.AppendLine();

        AppendChunkMethodsLine(sb, classInfo, existingMembers);

        sb.AppendLine("}");

        context.AddSource($"Engines/{classInfo.Name}", sb.ToString());
    }

    private static void AppendClassDefinitionLine(
        StringBuilder sb,
        ClassDataModel classInfo,
        int? archiveStructureKind,
        ImmutableDictionary<string, ClassDataModel> classInfos,
        out ImmutableList<string> additionalCodePieces)
    {
        if (!string.IsNullOrWhiteSpace(classInfo.Description))
        {
            sb.AppendLine("/// <summary>");
            sb.Append("/// ");
            sb.AppendLine(classInfo.Description);
            sb.AppendLine("/// </summary>");
        }

        if (classInfo.Id.HasValue)
        {
            sb.Append("/// <remarks>ID: 0x");
            sb.Append(classInfo.Id.Value.ToString("X8"));
            sb.AppendLine("</remarks>");

            var atts = classInfo.TypeSymbol?.GetAttributes() ?? [];

            var hasClassAttribute = atts.Any(x => x.AttributeClass?.Name == "ClassAttribute");

            if (!hasClassAttribute)
            {
                sb.Append($"[Class(0x");
                sb.Append(classInfo.Id.Value.ToString("X8"));
                sb.AppendLine(")]");
            }
        }

        sb.Append("public ");

        if (classInfo.IsAbstract)
        {
            sb.Append("abstract ");
        }

        sb.Append("partial class ");
        sb.Append(classInfo.Name);

        var inheritanceList = new List<string>();

        var existingSymbolDoesNotInherit = classInfo.TypeSymbol is null or { BaseType.Name: nameof(Object) };
        if (existingSymbolDoesNotInherit && classInfo.Name != "CMwNod")
        {
            inheritanceList.Add(classInfo.Inherits is null ? "CMwNod" : classInfo.Inherits);
        }

        var existingSymbolHasIClass = classInfo.TypeSymbol?.Interfaces.Any(x => x.Name == "IClass") ?? false;
        if (!existingSymbolHasIClass)
        {
            inheritanceList.Add("IClass");
        }

        if (archiveStructureKind.HasValue)
        {
            if (archiveStructureKind.Value == 1) // StructureKind == SeparateReadAndWrite
            {
                if (classInfo.TypeSymbol is null || classInfo.TypeSymbol.AllInterfaces.Any(x => x.Name == "IReadable") == false)
                {
                    inheritanceList.Add("IReadable");
                }

                if (classInfo.TypeSymbol is null || classInfo.TypeSymbol.AllInterfaces.Any(x => x.Name == "IWritable") == false)
                {
                    inheritanceList.Add("IWritable");
                }
            }
            else
            {
                if (classInfo.TypeSymbol is null || classInfo.TypeSymbol.AllInterfaces.Any(x => x.Name == "IReadableWritable") == false)
                {
                    inheritanceList.Add("IReadableWritable");
                }
            }
        }

        var inherits = classInfo.Inherits;
        while (inherits is not null && inherits != "CGameCtnMediaBlock")
        {
            if (classInfos.TryGetValue(inherits, out var inheritedClass))
            {
                inherits = inheritedClass.Inherits;
            }
        }
        var inheritsMediaBlock = inherits == "CGameCtnMediaBlock";

        var additionalCodePiecesBuilder = ImmutableList.CreateBuilder<string>();
        var timeSingleStartProp = default(ChunkProperty);
        var timeSingleEndProp = default(ChunkProperty);
        var keysProp = default(ChunkProperty);

        foreach (var chunk in classInfo.Chunks)
        {
            if (chunk.Value.ChunkLDefinition is null)
            {
                continue;
            }

            if (inheritsMediaBlock)
            {
                foreach (var member in RecurseMembers(chunk.Value.ChunkLDefinition))
                {
                    if (member is ChunkProperty { Name: "Start", Type.PrimaryType: "timefloat" } propStart)
                    {
                        timeSingleStartProp = propStart;
                    }

                    if (member is ChunkProperty { Name: "End", Type.PrimaryType: "timefloat" } propEnd)
                    {
                        timeSingleEndProp = propEnd;
                    }

                    if (member is ChunkProperty { Name: "Keys", Type.PrimaryType: "list", Type.GenericType: "Key" } propKeys)
                    {
                        keysProp = propKeys;
                    }
                }
            }

            static IEnumerable<IChunkMember> RecurseMembers(IChunkMemberBlock block)
            {
                foreach (var member in block.Members)
                {
                    yield return member;

                    if (member is IChunkMemberBlock b)
                    {
                        foreach (var m in RecurseMembers(b))
                        {
                            yield return m;
                        }
                    }
                }
            }
        }

        if (timeSingleStartProp is not null && timeSingleEndProp is not null)
        {
            if (classInfo.TypeSymbol is null || classInfo.TypeSymbol.AllInterfaces.Any(x => x.Name == "IHasTwoKeys") == false)
            {
                inheritanceList.Add("CGameCtnMediaBlock.IHasTwoKeys");
            }

            additionalCodePiecesBuilder.Add(timeSingleStartProp.IsNullable
                ? "TimeSingle IHasTwoKeys.Start { get => Start.GetValueOrDefault(); set => Start = value; }"
                : "TimeSingle IHasTwoKeys.Start { get => Start; set => Start = value; }");

            additionalCodePiecesBuilder.Add(timeSingleEndProp.IsNullable
                ? "TimeSingle IHasTwoKeys.End { get => End.GetValueOrDefault(); set => End = value; }"
                : "TimeSingle IHasTwoKeys.End { get => End; set => End = value; }");
        }

        if (keysProp is not null)
        {
            if (classInfo.TypeSymbol is null || classInfo.TypeSymbol.AllInterfaces.Any(x => x.Name == "IHasKeys") == false)
            {
                inheritanceList.Add("CGameCtnMediaBlock.IHasKeys");
            }

            additionalCodePiecesBuilder.Add("IEnumerable<IKey> IHasKeys.Keys => Keys ?? [];");
        }

        additionalCodePieces = additionalCodePiecesBuilder.ToImmutable();

        var first = true;

        foreach (var item in inheritanceList)
        {
            if (first)
            {
                sb.Append(" : ");
                first = false;
            }
            else
            {
                sb.Append(", ");
            }

            sb.Append(item);
        }

        sb.AppendLine();
    }

    private static void AppendStaticIdMemberLine(
        StringBuilder sb,
        uint id,
        ImmutableArray<ISymbol> existingMembers, 
        SourceProductionContext context)
    {
        var staticIdSymbol = existingMembers.FirstOrDefault(x => x.IsStatic && x.Name == "Id");

        if (staticIdSymbol is not null)
        {
            context.ReportDiagnostic(Diagnostic.Create(
                new DiagnosticDescriptor(
                "GBXNETGEN003",
                "Id should not be implemented manually.",
                "Member called 'Id' should not be implemented manually.",
                "GBX.NET.Generators",
                DiagnosticSeverity.Error,
                isEnabledByDefault: true),
                staticIdSymbol.Locations.FirstOrDefault()));

            return;
        }

        sb.Append("    public static new uint Id => 0x");
        sb.Append(id.ToString("X8"));
        sb.AppendLine(";");
    }

    private static void AppendDefaultCtorLine(StringBuilder sb, string className)
    {
        sb.AppendLine("    /// <summary>");
        sb.Append("    /// Creates a new instance of <see cref=\"");
        sb.Append(className);
        sb.AppendLine("\"/> with no chunks inside (no data will be serialized).");
        sb.AppendLine("    /// </summary>");
        sb.Append("    public ");
        sb.Append(className);
        sb.AppendLine("() { }");
    }

    private static void AppendArchiveMethodsLine(
        StringBuilder sb,
        ClassDataModel classInfo,
        ArchiveDataModel archiveInfo,
        ImmutableArray<ISymbol> existingMembers,
        ImmutableDictionary<string, ClassDataModel> classInfos,
        ImmutableDictionary<string, ArchiveDataModel> archiveInfos,
        SourceProductionContext context,
        int? archiveStructureKind,
        int indent,
        bool? overrideMethods)
    {
        if (archiveInfo.ChunkLDefinition is null)
        {
            return;
        }

        var kind = archiveStructureKind.GetValueOrDefault();

        var contextual = archiveInfo.ChunkLDefinition.Properties.ContainsKey("contextual") == true;

        var doReadMethod = false;
        var doWriteMethod = false;
        var doReadWriteMethod = false;

        if (kind == 1) // StructureKind == SeparateReadAndWrite
        {
            doReadMethod = true;
            doWriteMethod = true;

            foreach (var symbol in existingMembers)
            {
                if (symbol is not IMethodSymbol methodSymbol)
                {
                    continue;
                }

                if (!doReadMethod && !doWriteMethod)
                {
                    break;
                }

                if (methodSymbol.Name == "Read")
                {
                    if (!contextual && methodSymbol.Parameters.Length == 2 && methodSymbol.Parameters[0].Type.Name == "GbxReader" && methodSymbol.Parameters[1].Type.Name == nameof(Int32))
                    {
                        doReadMethod = false;
                    }

                    if (contextual && methodSymbol.Parameters.Length == 3 && methodSymbol.Parameters[0].Type.Name == "GbxReader" && methodSymbol.Parameters[1].Type.Name == classInfo.Name && methodSymbol.Parameters[2].Type.Name == nameof(Int32))
                    {
                        doReadMethod = false;
                    }
                }

                if (methodSymbol.Name == "Write")
                {
                    if (!contextual && methodSymbol.Parameters.Length == 2 && methodSymbol.Parameters[0].Type.Name == "GbxWriter" && methodSymbol.Parameters[1].Type.Name == nameof(Int32))
                    {
                        doWriteMethod = false;
                    }

                    if (contextual && methodSymbol.Parameters.Length == 3 && methodSymbol.Parameters[0].Type.Name == "GbxWriter" && methodSymbol.Parameters[1].Type.Name == classInfo.Name && methodSymbol.Parameters[2].Type.Name == nameof(Int32))
                    {
                        doWriteMethod = false;
                    }
                }
            }
        }
        else
        {
            doReadWriteMethod = true;

            foreach (var methodSymbol in existingMembers.OfType<IMethodSymbol>())
            {
                if (methodSymbol.Name == "ReadWrite" && methodSymbol.Parameters.Length == 2 && methodSymbol.Parameters[0].Type.Name == "GbxReaderWriter" && methodSymbol.Parameters[1].Type.Name == nameof(Int32))
                {
                    doReadWriteMethod = false;
                    break;
                }
            }
        }

        var existingFields = existingMembers.OfType<IFieldSymbol>()
            .ToImmutableDictionary(x => x.Name);
        var existingProps = existingMembers.OfType<IPropertySymbol>()
            .ToImmutableDictionary(x => x.Name);

        if (doReadMethod)
        {
            sb.AppendLine();
            sb.Append(indent, "    public ");

            if (overrideMethods.HasValue)
            {
                sb.Append(overrideMethods.Value ? "override " : "virtual ");
            }

            sb.Append("void Read(GbxReader r, ");

            if (contextual)
            {
                sb.Append(classInfo.Name);
                sb.Append(" n, ");
            }

            sb.Append("int v = 0)");
            sb.AppendLine();
            sb.AppendLine(indent, "    {");

            var memberWriter = new MemberSerializationWriter(
                sb, SerializationType.Read, archiveInfo, existingFields, existingProps, classInfo, classInfos, archiveInfos, autoProperty: true, context);
            memberWriter.Append(indent + 2, archiveInfo.ChunkLDefinition.Members);

            sb.AppendLine(indent, "    }");
        }

        if (doWriteMethod)
        {
            sb.AppendLine();
            sb.Append(indent, "    public ");

            if (overrideMethods.HasValue)
            {
                sb.Append(overrideMethods.Value ? "override " : "virtual ");
            }

            sb.Append("void Write(GbxWriter w, ");

            if (contextual)
            {
                sb.Append(classInfo.Name);
                sb.Append(" n, ");
            }

            sb.Append("int v = 0)");
            sb.AppendLine();
            sb.AppendLine(indent, "    {");

            var memberWriter = new MemberSerializationWriter(
                sb, SerializationType.Write, archiveInfo, existingFields, existingProps, classInfo, classInfos, archiveInfos, autoProperty: true, context);
            memberWriter.Append(indent + 2, archiveInfo.ChunkLDefinition.Members);

            sb.AppendLine(indent, "    }");
        }

        if (doReadWriteMethod)
        {
            sb.AppendLine();
            sb.Append(indent, "    public ");

            if (overrideMethods.HasValue)
            {
                sb.Append(overrideMethods.Value ? "override " : "virtual ");
            }

            sb.Append("void ReadWrite(GbxReaderWriter rw, int v = 0)");
            sb.AppendLine();
            sb.AppendLine(indent, "    {");

            var memberWriter = new MemberSerializationWriter(
                sb, SerializationType.ReadWrite, archiveInfo, existingFields, existingProps, classInfo, classInfos, archiveInfos, autoProperty: false, context);
            memberWriter.Append(indent + 2, archiveInfo.ChunkLDefinition.Members);

            sb.AppendLine(indent, "    }");
        }
    }

    private static void AppendEnumsLine(StringBuilder sb, ClassDataModel classInfo, SourceProductionContext context)
    {
        foreach (var enumInfo in classInfo.Enums)
        {
            if (enumInfo.Value.TypeSymbol is not null)
            {
                if (enumInfo.Value.TypeSymbol.TypeKind == TypeKind.Enum)
                {
                    context.ReportDiagnostic(Diagnostic.Create(
                        new DiagnosticDescriptor(
                        "GBXNETGEN008",
                        "Enum cannot be implemented manually.",
                        "Enum {0} cannot be implemented manually, it is defined in ChunkL.",
                        "GBX.NET.Generators",
                        DiagnosticSeverity.Error,
                        isEnabledByDefault: true),
                        enumInfo.Value.TypeSymbol.Locations.FirstOrDefault(),
                        enumInfo.Key));
                }
                else
                {
                    context.ReportDiagnostic(Diagnostic.Create(
                        new DiagnosticDescriptor(
                        "GBXNETGEN009",
                        "Name collision with nested type.",
                        "Name collision with nested type: {0}",
                        "GBX.NET.Generators",
                        DiagnosticSeverity.Error,
                        isEnabledByDefault: true),
                        enumInfo.Value.TypeSymbol.Locations.FirstOrDefault(),
                        enumInfo.Key));
                }

                continue;
            }

            sb.AppendLine();

            if (!string.IsNullOrEmpty(enumInfo.Value.ChunkLDefinition.Description))
            {
                sb.AppendLine("    /// <summary>");
                sb.Append("    /// ");
                sb.AppendLine(enumInfo.Value.ChunkLDefinition.Description);
                sb.AppendLine("    /// </summary>");
            }

            sb.Append("    public enum ");
            sb.Append(enumInfo.Key);
            sb.AppendLine();
            sb.AppendLine("    {");

            foreach (var member in enumInfo.Value.ChunkLDefinition.Values)
            {
                if (!string.IsNullOrEmpty(member.Description))
                {
                    sb.AppendLine("        /// <summary>");
                    sb.Append("        /// ");
                    sb.AppendLine(member.Description);
                    sb.AppendLine("        /// </summary>");
                }

                sb.Append("        ");
                sb.Append(member.Name);

                if (!string.IsNullOrWhiteSpace(member.ExplicitValue))
                {
                    sb.Append(" = ");
                    sb.Append(member.ExplicitValue);
                }

                sb.AppendLine(",");
            }

            sb.AppendLine("    }");
        }
    }

    private static void AppendArchivesLine(
        StringBuilder sb,
        ClassDataModel classInfo,
        ImmutableArray<ISymbol> existingMembers,
        ImmutableDictionary<string, ClassDataModel> classInfos,
        ImmutableDictionary<string, ArchiveDataModel> archiveInfos,
        SourceProductionContext context)
    {
        foreach (var archiveInfo in classInfo.Archives)
        {
            AppendArchiveLine(sb, archiveInfo.Key, archiveInfo.Value, classInfo, classInfos, archiveInfos, context);
        }
    }

    private static void AppendArchiveLine(
        StringBuilder sb,
        string archiveName,
        ArchiveDataModel archiveInfo,
        ClassDataModel classInfo,
        ImmutableDictionary<string, ClassDataModel> classInfos,
        ImmutableDictionary<string, ArchiveDataModel> archiveInfos,
        SourceProductionContext context)
    {
        sb.AppendLine();
        sb.Append("    public ");

        var sealedArchive = !archiveInfos.Any(x => x.Value
            .ChunkLDefinition?
            .Properties
            .TryGetValue("inherits", out var otherInherits) == true
                && otherInherits == archiveName);

        if (sealedArchive)
        {
            sb.Append("sealed ");
        }

        sb.Append("partial class ");
        sb.Append(archiveName);
        sb.Append(" : ");

        bool? overrideMethods = sealedArchive;

        if (archiveInfo.ChunkLDefinition?.Properties.TryGetValue("inherits", out var inherits) == true)
        {
            sb.Append(inherits);
            sb.Append(", ");

            if (inherits.StartsWith("I")) // kinda weird
            {
                overrideMethods = null;
            }
        }
        else if (sealedArchive)
        {
            overrideMethods = null;
        }

        var contextual = archiveInfo.ChunkLDefinition?.Properties.ContainsKey("contextual") == true;

        var archiveGenOptionsAtt = archiveInfo.TypeSymbol?
            .GetAttributes()
            .FirstOrDefault(x => x.AttributeClass?.Name == "ArchiveGenerationOptionsAttribute");

        var archiveStructureKind = (archiveGenOptionsAtt?.NamedArguments
            .FirstOrDefault(x => x.Key == "StructureKind").Value.Value as int?).GetValueOrDefault();

        var privateSet = (archiveGenOptionsAtt?.NamedArguments
            .FirstOrDefault(x => x.Key == "PrivateSet").Value.Value as bool?).GetValueOrDefault();

        var autoProperty = false;

        if (archiveStructureKind == 1) // StructureKind == SeparateReadAndWrite
        {
            sb.Append("IReadable");

            if (contextual)
            {
                sb.Append("<");
                sb.Append(classInfo.Name);
                sb.Append(">");
            }

            sb.Append(", IWritable");

            if (contextual)
            {
                sb.Append("<");
                sb.Append(classInfo.Name);
                sb.Append(">");
            }

            autoProperty = true;
        }
        else
        {
            sb.Append("IReadableWritable");
        }

        sb.AppendLine();
        sb.AppendLine("    {");

        var existingArchiveMembers = archiveInfo.TypeSymbol?.GetMembers() ?? [];

        var alreadyExistingProperties = existingArchiveMembers.OfType<IPropertySymbol>()
            .ToDictionary(x => x.Name) ?? [];

        var propWriter = new ChunkLPropertiesWriter(sb, classInfo: null, archiveInfo, alreadyExistingProperties, indent: 1, autoProperty, privateSet, context);
        propWriter.Append();

        AppendArchiveMethodsLine(sb, classInfo, archiveInfo, existingArchiveMembers, classInfos, archiveInfos, context, archiveStructureKind, indent: 1, overrideMethods);

        sb.AppendLine("    }");
    }

    private static void AppendChunksLine(
        StringBuilder sb,
        ClassDataModel classInfo,
        ImmutableArray<ISymbol> existingMembers,
        ImmutableDictionary<string, ClassDataModel> classInfos,
        ImmutableDictionary<string, ArchiveDataModel> archiveInfos,
        SourceProductionContext context)
    {
        foreach (var chunk in classInfo.HeaderChunks.OrderBy(x => x.Key))
        {
            AppendChunkLine(sb, chunk.Value, isHeaderChunk: true, existingMembers, classInfo, classInfos, archiveInfos, context);
        }

        sb.AppendLine();

        foreach (var chunk in classInfo.Chunks.OrderBy(x => x.Key))
        {
            AppendChunkLine(sb, chunk.Value, isHeaderChunk: false, existingMembers, classInfo, classInfos, archiveInfos, context);
        }
    }

    private static void AppendChunkLine(
        StringBuilder sb,
        ChunkDataModel chunk,
        bool isHeaderChunk,
        ImmutableArray<ISymbol> existingMembers,
        ClassDataModel classInfo,
        ImmutableDictionary<string, ClassDataModel> classInfos,
        ImmutableDictionary<string, ArchiveDataModel> archiveInfos,
        SourceProductionContext context)
    {
        if (isHeaderChunk && chunk.IsSkippable)
        {
            context.ReportDiagnostic(Diagnostic.Create(
                new DiagnosticDescriptor(
                "GBXNETGEN004",
                "Header chunk cannot be skippable",
                "Header chunk cannot be skippable",
                "GBX.NET.Generators",
                DiagnosticSeverity.Error,
                isEnabledByDefault: true),
                chunk.TypeSymbol?.Locations.FirstOrDefault()));
        }

        var expectedChunkName = (isHeaderChunk ? "HeaderChunk" : "Chunk") + chunk.Id.ToString("X8");
        
        if (chunk.TypeSymbol?.TypeKind is TypeKind.Enum)
        {
            context.ReportDiagnostic(Diagnostic.Create(
                new DiagnosticDescriptor(
                "GBXNETGEN005",
                "Enum cannot be named like a chunk type",
                "Enum cannot be named like a chunk type",
                "GBX.NET.Generators",
                DiagnosticSeverity.Error,
                isEnabledByDefault: true),
                chunk.TypeSymbol?.Locations.FirstOrDefault()));
        }

        sb.AppendLine();
        sb.AppendLine("    /// <summary>");
        sb.Append("    /// ");

        var structName = default(string);
        _ = chunk.ChunkLDefinition?.Properties.TryGetValue("struct", out structName);

        if (!string.IsNullOrWhiteSpace(structName))
        {
            sb.Append('[');
            sb.Append(structName);
            sb.Append("] ");
        }

        sb.Append(classInfo.Name);
        sb.Append(" 0x");
        sb.Append((chunk.Id & 0xFFF).ToString("X3"));

        if (isHeaderChunk)
        {
            sb.Append(" header");
        }
        else if (chunk.IsSkippable)
        {
            sb.Append(" skippable");
        }

        sb.Append(" chunk");

        if (!string.IsNullOrWhiteSpace(chunk.Description))
        {
            sb.Append(" (");
            sb.Append(chunk.Description);
            sb.Append(')');
        }

        sb.AppendLine();

        sb.AppendLine("    /// </summary>");

        var hasChunkAttribute = chunk.TypeSymbol?.GetAttributes()
            .Any(x => x.AttributeClass?.Name == "ChunkAttribute") ?? false;

        if (!hasChunkAttribute)
        {
            sb.Append("    [Chunk(0x");
            sb.Append(chunk.Id.ToString("X8"));

            if (!string.IsNullOrWhiteSpace(chunk.Description))
            {
                sb.Append(", \"");
                sb.Append(chunk.Description);
                sb.Append('\"');
            }

            sb.AppendLine(")]");
        }

        if (chunk.ChunkLDefinition?.Versions.Count > 0)
        {
            sb.Append("    [ChunkGameVersion(");

            var first = true;

            foreach (var pair in chunk.ChunkLDefinition.Versions)
            {
                if (first)
                {
                    first = false;
                }
                else
                {
                    sb.Append(" | ");
                }

                sb.Append("GameVersion.");
                sb.Append(pair.Key);
            }

            if (chunk.ChunkLDefinition.Versions.Any(x => x.Value.HasValue))
            {
                foreach (var pair in chunk.ChunkLDefinition.Versions)
                {
                    sb.Append(", ");
                    sb.Append(pair.Value ?? -1);
                }
            }

            sb.AppendLine(")]");
        }

        sb.Append("    public partial ");

        var isStructChunk = chunk.TypeSymbol?.IsValueType ?? false;
        sb.Append(isStructChunk ? "struct " : "class ");

        if (isHeaderChunk)
        {
            sb.Append("Header");
        }

        sb.Append("Chunk");
        sb.Append(chunk.Id.ToString("X8"));

        var hasImplementedIChunk = chunk.TypeSymbol?.AllInterfaces.Any(x => x.Name == "IChunk") == true;

        if (chunk.ChunkLDefinition is not null && hasImplementedIChunk)
        {
            context.ReportDiagnostic(Diagnostic.Create(
                new DiagnosticDescriptor(
                "GBXNETGEN006",
                "Do not implement any type of IChunk.",
                "Do not implement any type of IChunk. It should be automatically generated. Specify the type in .chunkl file instead.",
                "GBX.NET.Generators",
                DiagnosticSeverity.Error,
                isEnabledByDefault: true),
                chunk.TypeSymbol?.Locations.FirstOrDefault()));
        }

        var isVersionableAutomated = chunk.ChunkLDefinition?.IsVersionable == true;

        if (!hasImplementedIChunk || isVersionableAutomated)
        {
            sb.Append(" : ");
        }

        if (!hasImplementedIChunk)
        {
            if (chunk.ChunkLDefinition?.Properties.TryGetValue("base", out var baseChunkIdStr) == true)
            {
                if (classInfo.TypeSymbol?.IsValueType == true)
                {
                    context.ReportDiagnostic(Diagnostic.Create(
                        new DiagnosticDescriptor(
                        "GBXNETGEN007",
                        "Struct chunk cannot have base chunk",
                        "Struct chunk cannot have base chunk",
                        "GBX.NET.Generators",
                        DiagnosticSeverity.Error,
                        isEnabledByDefault: true),
                        chunk.TypeSymbol?.Locations.FirstOrDefault()));
                }

                sb.Append("Chunk");
                if (baseChunkIdStr.StartsWith("0x")) baseChunkIdStr = baseChunkIdStr.Substring(2);
                var baseChunkId = uint.Parse(baseChunkIdStr, NumberStyles.HexNumber);
                if (baseChunkId <= 0xFFF)
                {
                    baseChunkId = classInfo.Id.GetValueOrDefault() | baseChunkId;
                }
                sb.Append(baseChunkId.ToString("X8"));
            }
            else
            {
                if (chunk.TypeSymbol?.IsValueType == true)
                {
                    sb.Append("I");
                }

                if (isHeaderChunk)
                {
                    sb.Append("Header");
                }
                else if (chunk.IsSkippable)
                {
                    sb.Append("Skippable");
                }

                sb.Append("Chunk<");
                sb.Append(classInfo.Name);
                sb.Append(">");
            }
        }

        var isSelfContained = chunk.ChunkLDefinition?.Properties.ContainsKey("self-contained") == true;

        if (isSelfContained)
        {
            sb.Append(", ISelfContainedChunk<");
            sb.Append(classInfo.Name);
            sb.Append(">");
        }

        if (isVersionableAutomated)
        {
            sb.Append(", IVersionable");
        }

        sb.AppendLine();
        sb.AppendLine("    {");

        var existingChunkMembers = chunk.TypeSymbol?.GetMembers() ?? [];

        AppendChunkIdMemberLine(sb, existingChunkMembers, chunk.Id, chunk.TypeSymbol?.IsValueType ?? false, context);

        if (isStructChunk)
        {
            if (isHeaderChunk)
            {
                sb.AppendLine();
                sb.AppendLine("        /// <inheritdoc />");
                sb.AppendLine("        public bool IsHeavy { get; set; }");
            }

            sb.AppendLine();
            sb.AppendLine("        /// <inheritdoc />");
            sb.AppendLine("        public bool Ignore => false;");
            sb.AppendLine();
            sb.AppendLine("        /// <inheritdoc />");
            sb.AppendLine("        public GameVersion GameVersion => GameVersion.Unspecified;");
        }
        else
        {
            if (isSelfContained)
            {
                sb.AppendLine();
                sb.AppendLine("        /// <inheritdoc />");
                sb.Append("        public ");
                sb.Append(classInfo.Name);
                sb.AppendLine(" Node { get; set; } = new();");
                sb.AppendLine();
                sb.AppendLine("        /// <inheritdoc />");
                sb.Append("        IClass ISelfContainedChunk.Node { get => Node; set => Node = (");
                sb.Append(classInfo.Name);
                sb.AppendLine("?)value; }");
            }

            if (chunk.ChunkLDefinition?.Properties.ContainsKey("ignore") == true)
            {
                sb.AppendLine();
                sb.AppendLine("        /// <inheritdoc />");
                sb.AppendLine("        public override bool Ignore => true;");
            }

            if (chunk.ChunkLDefinition?.Versions.Count > 0)
            {
                sb.AppendLine();
                sb.AppendLine("        /// <inheritdoc />");
                sb.Append("        public override GameVersion GameVersion => ");

                var first = true;

                foreach (var pair in chunk.ChunkLDefinition.Versions)
                {
                    if (first)
                    {
                        first = false;
                    }
                    else
                    {
                        sb.Append(" | ");
                    }

                    sb.Append("GameVersion.");
                    sb.Append(pair.Key);
                }

                sb.AppendLine(";");
            }
        }

        if (isVersionableAutomated)
        {
            AppendVersionPropertyLine(sb, chunk);
        }

        sb.AppendLine();
        var fieldsWriter = new ChunkLFieldsWriter(sb, chunk, existingChunkMembers, context);
        fieldsWriter.Append();

        if (chunk.ChunkLDefinition?.Members.Count > 0 && !chunk.ChunkLDefinition.Properties.ContainsKey("demonstration"))
        {
            var generationOptionsAtt = chunk.TypeSymbol?.GetAttributes()
                .FirstOrDefault(x => x.AttributeClass?.Name == "ChunkGenerationOptionsAttribute");

            var structureKind = generationOptionsAtt?.NamedArguments
                .FirstOrDefault(x => x.Key == "StructureKind").Value.Value as int?;

            var existingFields = existingMembers.OfType<IFieldSymbol>()
                .ToImmutableDictionary(x => x.Name);
            var existingProps = existingMembers.OfType<IPropertySymbol>()
                .ToImmutableDictionary(x => x.Name);

            if (isStructChunk || structureKind == 1) // StructureKind == SeparateReadAndWrite
            {
                sb.AppendLine();
                sb.Append("        public ");

                if (!isStructChunk)
                {
                    sb.Append("override ");
                }

                sb.Append("void Read(");
                sb.Append(classInfo.Name);
                sb.AppendLine(" n, GbxReader r)");
                sb.AppendLine("        {");

                var readMemberWriter = new MemberSerializationWriter(
                    sb, SerializationType.Read, archive: null, existingFields, existingProps, classInfo, classInfos, archiveInfos, autoProperty: true, context);
                readMemberWriter.Append(indent: 3, chunk.ChunkLDefinition.Members);

                sb.AppendLine("        }");
                sb.AppendLine();
                sb.Append("        public ");

                if (!isStructChunk)
                {
                    sb.Append("override ");
                }

                sb.Append("void Write(");
                sb.Append(classInfo.Name);
                sb.AppendLine(" n, GbxWriter w)");
                sb.AppendLine("        {");

                var writeMemberWriter = new MemberSerializationWriter(
                    sb, SerializationType.Write, archive: null, existingFields, existingProps, classInfo, classInfos, archiveInfos, autoProperty: true, context);
                writeMemberWriter.Append(indent: 3, chunk.ChunkLDefinition.Members);

                sb.AppendLine("        }");

                if (isStructChunk)
                {
                    sb.AppendLine();
                    sb.AppendLine("        public IChunk DeepClone() => throw new NotImplementedException();");
                }
            }
            else if (structureKind.GetValueOrDefault() == 0)
            {
                var doReadWriteMethod = !existingChunkMembers.OfType<IMethodSymbol>()
                    .Any(x => x.Name == "ReadWrite" && x.Parameters.Length == 2 && x.Parameters[0].Type.Name == classInfo.Name && x.Parameters[1].Type.Name == "GbxReaderWriter");

                if (doReadWriteMethod)
                {
                    sb.AppendLine();
                    sb.Append("        public override void ReadWrite(");
                    sb.Append(classInfo.Name);
                    sb.AppendLine(" n, GbxReaderWriter rw)");
                    sb.AppendLine("        {");

                    var memberWriter = new MemberSerializationWriter(
                        sb, SerializationType.ReadWrite, archive: null, existingFields, existingProps, classInfo, classInfos, archiveInfos, autoProperty: false, context);
                    memberWriter.Append(indent: 3, chunk.ChunkLDefinition.Members);

                    sb.AppendLine("        }");
                }
            }
        }

        sb.AppendLine("    }");
    }

    private static void AppendVersionPropertyLine(StringBuilder sb, ChunkDataModel chunk)
    {
        sb.AppendLine();
        sb.Append("        public int Version { get; set; }");

        var versionProp = chunk.ChunkLDefinition?.Members
            .OfType<ChunkProperty>()
            .FirstOrDefault(p => p.Type.IsSimpleType() && p.Type.PrimaryType is "version" or "versionb");

        if (versionProp is not null && !string.IsNullOrEmpty(versionProp.DefaultValue))
        {
            sb.Append(" = ");
            sb.Append(versionProp.DefaultValue);
            sb.Append(';');
        }

        sb.AppendLine();
    }

    private static void AppendChunkIdMemberLine(StringBuilder sb, ImmutableArray<ISymbol> existingMembers, uint id, bool isStruct, SourceProductionContext context)
    {
        var existingIdMember = existingMembers.FirstOrDefault(x => x.Name == "Id");

        if (existingIdMember is not null)
        {
            context.ReportDiagnostic(Diagnostic.Create(
                new DiagnosticDescriptor(
                "GBXNETGEN003",
                "Id should not be implemented manually.",
                "Member called 'Id' should not be implemented manually.",
                "GBX.NET.Generators",
                DiagnosticSeverity.Error,
                isEnabledByDefault: true),
                existingIdMember.Locations.FirstOrDefault()));

            return;
        }

        sb.AppendLine("        /// <inheritdoc />");
        sb.Append("        public ");
        sb.Append(isStruct ? "readonly" : "override");
        sb.Append(" uint Id => 0x");
        sb.Append(id.ToString("X8"));
        sb.AppendLine(";");
    }

    private static void AppendChunkMethodsLine(StringBuilder sb, ClassDataModel classInfo, ImmutableArray<ISymbol> existingMembers)
    {
        var hasCreateHeaderChunkMethod = existingMembers
            .OfType<IMethodSymbol>()
            .Any(x => !x.IsStatic
                && x.Name == "CreateHeaderChunk"
                && x.ReturnType.Name == "IHeaderChunk"
                && (x.IsOverride || x.IsVirtual)
                && x.Parameters.Length == 1
                && x.Parameters[0].Type.Name == nameof(UInt32));

        if (!hasCreateHeaderChunkMethod && classInfo.HeaderChunks.Count > 0)
        {
            sb.AppendLine();
            sb.AppendLine("    public override IHeaderChunk? CreateHeaderChunk(uint chunkId)");
            sb.AppendLine("    {");
            sb.AppendLine("        IHeaderChunk? chunk;");
            sb.AppendLine("        switch (chunkId)");
            sb.AppendLine("        {");

            foreach (var chunk in classInfo.HeaderChunks)
            {
                sb.Append("            case 0x");
                sb.Append(chunk.Value.Id.ToString("X8"));
                sb.Append(": chunk = new HeaderChunk");
                sb.Append(chunk.Value.Id.ToString("X8"));
                sb.AppendLine("(); break;");
            }

            sb.AppendLine("            default: return base.CreateHeaderChunk(chunkId);");
            sb.AppendLine("        }");
            sb.AppendLine();
            sb.AppendLine("        if (chunk is not null)");
            sb.AppendLine("        {");
            sb.AppendLine("            Chunks.Add(chunk);");
            sb.AppendLine("        }");
            sb.AppendLine();
            sb.AppendLine("        return chunk;");
            sb.AppendLine("    }");
        }

        var hasCreateChunkMethod = existingMembers
            .OfType<IMethodSymbol>()
            .Any(x => !x.IsStatic
                && x.Name == "CreateChunk"
                && x.ReturnType.Name == "IChunk"
                && (x.IsOverride || x.IsVirtual)
                && x.Parameters.Length == 1
                && x.Parameters[0].Type.Name == nameof(UInt32));

        if (!hasCreateChunkMethod && classInfo.Chunks.Count > 0)
        {
            sb.AppendLine();
            sb.AppendLine("    public override IChunk? CreateChunk(uint chunkId)");
            sb.AppendLine("    {");
            sb.AppendLine("        IChunk? chunk;");
            sb.AppendLine("        switch (chunkId)");
            sb.AppendLine("        {");

            foreach (var chunk in classInfo.Chunks)
            {
                sb.Append("            case 0x");
                sb.Append(chunk.Value.Id.ToString("X8"));
                sb.Append(": chunk = new Chunk");
                sb.Append(chunk.Value.Id.ToString("X8"));
                sb.AppendLine("(); break;");
            }

            sb.AppendLine("            default: return base.CreateChunk(chunkId);");
            sb.AppendLine("        }");
            sb.AppendLine();
            sb.AppendLine("        if (chunk is not null)");
            sb.AppendLine("        {");
            sb.AppendLine("            Chunks.Add(chunk);");
            sb.AppendLine("        }");
            sb.AppendLine();
            sb.AppendLine("        return chunk;");
            sb.AppendLine("    }");
        }
    }
}
