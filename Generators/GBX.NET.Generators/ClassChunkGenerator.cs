﻿using ChunkL;
using ChunkL.Structure;
using Microsoft.CodeAnalysis;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace GBX.NET.Generators;

[Generator(LanguageNames.CSharp)]
public class ClassChunkGenerator : IIncrementalGenerator
{
    private const bool Debug = false;

    private const string TypeMatch = @"^(\w+)(<(\w+)>)?(\[([0-9]*)\])?(_deprec)?$";

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        if (Debug && !Debugger.IsAttached)
        {
            Debugger.Launch();
        }

        var existingClasses = context.CompilationProvider.Select((compilation, token) =>
        {
            var enginesNamespace = compilation.GlobalNamespace.GetNamespaceMembers()
                .FirstOrDefault(x => x.Name == "GBX")
                .GetNamespaceMembers()
                .FirstOrDefault(x => x.Name == "NET")
                .GetNamespaceMembers()
                .FirstOrDefault(x => x.Name == "Engines");

            var dict = enginesNamespace.GetNamespaceMembers()
                .SelectMany(x => x.GetTypeMembers())
                .ToDictionary(x => x.Name);

            return dict;
        });

        var chunklFiles = context.AdditionalTextsProvider
            .Where(static file => file.Path.EndsWith(".chunkl", StringComparison.OrdinalIgnoreCase))
            .Select((chunklFile, token) =>
            {
                if (chunklFile.GetText()?.ToString() is not string chunklText)
                {
                    throw new Exception("Could not get text from file.");
                }

                using var reader = new StringReader(chunklText);

                return new ChunkLData(
                    DataModel: ChunkLSerializer.Deserialize(reader),
                    Engine: Path.GetFileName(Path.GetDirectoryName(chunklFile.Path)),
                    IsSealed: false);
            });

        context.RegisterSourceOutput(chunklFiles.Combine(existingClasses), GenerateSource);
    }

    private record ChunkLData(ChunkLDataModel DataModel, string Engine, bool IsSealed);

    private void GenerateSource(SourceProductionContext context, (ChunkLData Data, Dictionary<string, INamedTypeSymbol> ExistingClasses) source)
    {
        var sb = new StringBuilder();
        var generator = new SourceGeneratorPrimaryContext(source.Data, source.ExistingClasses, sb);
        generator.WriteSource();
        context.AddSource(source.Data.DataModel.Header.Name, sb.ToString());
    }

    private class SourceGeneratorPrimaryContext
    {
        private readonly ChunkLData data;
        private readonly Dictionary<string, INamedTypeSymbol> existingClasses;
        private readonly StringBuilder sb;

        private readonly ChunkLDataModel chunkl;
        private readonly bool hasExistingSymbol;
        private readonly INamedTypeSymbol? existingSymbol;

        public SourceGeneratorPrimaryContext(
            ChunkLData data,
            Dictionary<string, INamedTypeSymbol> existingClasses,
            StringBuilder sb)
        {
            this.data = data;
            this.existingClasses = existingClasses;
            this.sb = sb;

            chunkl = data.DataModel;
            hasExistingSymbol = existingClasses.TryGetValue(data.DataModel.Header.Name, out var existingSymbol);
            
            this.existingSymbol = existingSymbol;
        }

        public void WriteSource()
        {
            sb.Append("namespace GBX.NET.Engines.");
            sb.Append(data.Engine);
            sb.AppendLine(";");
            sb.AppendLine();

            AppendClassDefinition();

            sb.AppendLine("{");

            var existingMembers = existingSymbol?.GetMembers() ?? ImmutableArray<ISymbol>.Empty;

            AppendStaticIdMember(existingMembers);

            sb.AppendLine();

            AppendProperties();

            AppendDefaultCtor();

            AppendChunks(existingMembers);

            AppendStaticMethods(existingMembers);

            sb.AppendLine("}");
        }

        private void AppendClassDefinition()
        {
            var isSealed = data.IsSealed;
            var hasInherits = data.DataModel.Header.Features.TryGetValue("inherits", out var inherits);

            if (!string.IsNullOrWhiteSpace(chunkl.Header.Description))
            {
                sb.AppendLine("/// <summary>");
                sb.Append("/// ");
                sb.AppendLine(chunkl.Header.Description);
                sb.AppendLine("/// </summary>");
            }

            sb.Append("/// <remarks>ID: 0x");
            sb.Append(chunkl.Header.Id.ToString("X8"));
            sb.AppendLine("</remarks>");

            var atts = existingSymbol?.GetAttributes() ?? ImmutableArray<AttributeData>.Empty;

            var hasClassAttribute = atts.Any(x => x.AttributeClass?.Name == "ClassAttribute");

            if (!hasClassAttribute)
            {
                sb.Append($"[Class(0x");
                sb.Append(chunkl.Header.Id.ToString("X8"));
                sb.AppendLine(")]");
            }

            sb.Append("public ");

            if (isSealed)
            {
                sb.Append("sealed ");
            }

            if (hasExistingSymbol)
            {
                sb.Append("partial ");
            }

            sb.Append("class ");
            sb.Append(chunkl.Header.Name);

            var inheritanceList = new List<string>();

            var existingSymbolDoesNotInherit = existingSymbol is null or { BaseType.Name: nameof(Object) };
            if (existingSymbolDoesNotInherit)
            {
                inheritanceList.Add(hasInherits ? inherits : "CMwNod");
            }

            var existingSymbolHasIClass = existingSymbol?.Interfaces.Any(x => x.Name == "IClass") ?? false;
            if (!existingSymbolHasIClass)
            {
                inheritanceList.Add("IClass");
            }

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

        private void AppendStaticIdMember(ImmutableArray<ISymbol> existingMembers)
        {
            if (existingMembers.Any(x => x.IsStatic && x.Name == "Id"))
            {
                return;
            }

            sb.Append("    public static new uint Id => 0x");
            sb.Append(chunkl.Header.Id.ToString("X8"));
            sb.AppendLine(";");
        }

        private void AppendProperties()
        {
            var alreadyExistingProperties = existingSymbol?.GetMembers()
                .OfType<IPropertySymbol>()
                .ToDictionary(x => x.Name) ?? [];

            var alreadyWrittenProps = new HashSet<string>();

            foreach (var chunk in chunkl.Body.ChunkDefinitions)
            {
                AppendPropertiesRecurseChunkMembers(alreadyExistingProperties, alreadyWrittenProps, chunk);
            }
        }
        
        private void AppendPropertiesRecurseChunkMembers(Dictionary<string, IPropertySymbol> alreadyExistingProperties, HashSet<string> alreadyWrittenProps, IChunkMemberBlock memberBlock)
        {
            foreach (var mem in memberBlock.Members)
            {
                switch (mem)
                {
                    case IChunkMemberBlock memberBl:
                        AppendPropertiesRecurseChunkMembers(alreadyExistingProperties, alreadyWrittenProps, memberBl);
                        break;
                    case ChunkProperty prop:
                        if (string.IsNullOrEmpty(prop.Name) || alreadyWrittenProps.Contains(prop.Name) || alreadyExistingProperties.ContainsKey(prop.Name)) continue;

                        alreadyWrittenProps.Add(prop.Name);

                        var fieldName = char.ToLowerInvariant(prop.Name[0]) + prop.Name.Substring(1);

                        var mappedType = SimpleMapping(prop.Type);
                        if (string.IsNullOrEmpty(mappedType)) mappedType = AdvancedMapping(prop.Type);

                        sb.AppendLine();

                        sb.Append("    private ");
                        sb.Append(mappedType);

                        if (prop.IsNullable)
                        {
                            sb.Append('?');
                        }

                        sb.Append(' ');
                        sb.Append(fieldName);
                        sb.AppendLine(";");

                        if (!string.IsNullOrWhiteSpace(prop.Description))
                        {
                            sb.AppendLine("    /// <summary>");
                            sb.Append("    /// ");
                            sb.AppendLine(prop.Description);
                            sb.AppendLine("    /// </summary>");
                        }

                        sb.Append("    public ");
                        sb.Append(mappedType);

                        if (prop.IsNullable)
                        {
                            sb.Append('?');
                        }

                        sb.Append(' ');
                        sb.Append(prop.Name);

                        sb.AppendLine();
                        sb.AppendLine("    {");
                        sb.Append("        get => ");
                        sb.Append(fieldName);
                        sb.AppendLine(";");
                        sb.Append("        set => ");
                        sb.Append(fieldName);
                        sb.AppendLine(" = value;");
                        sb.AppendLine("    }");
                        break;
                }
            }
        }

        internal static string SimpleMapping(string type) => type switch
        {
            "string" or "id" or "lookbackstring" => "string",
            "byte" => "byte",
            "bool" or "boolean" => "bool",
            "short" or "int16" => "short",
            "ushort" or "uint16" => "ushort",
            "int" or "int32" => "int",
            "uint" or "uint32" => "uint",
            "long" or "int64" => "long",
            "ulong" or "uint64" => "ulong",
            "float" => "float",
            "byte3" => "Byte3",
            "int2" => "Int2",
            "int3" => "Int3",
            "vec2" => "Vec2",
            "vec3" => "Vec3",
            "vec4" => "Vec4",
            "int128" => "System.Numerics.BigInteger",
            "ident" or "meta" => "Ident",
            "packdesc" or "fileref" => "PackDesc",
            "data" or "bytes" => "byte[]",
            "list" => "List",
            "timeint" => "TimeInt32",
            "timefloat" => "TimeSingle",
            "timeofday" => "TimeSpan",
            _ => string.Empty
        };

        internal static string AdvancedMapping(string type)
        {
            if (type.StartsWith("throw "))
            {
                return type;
            }

            var regex = Regex.Match(type, TypeMatch);

            if (!regex.Success)
            {
                throw new Exception("Could not parse type.");
            }

            var primaryType = regex.Groups[1].Value;
            var genericType = regex.Groups[3].Value;
            var isArray = regex.Groups[4].Success;
            var arrayLength = isArray ? regex.Groups[5].Value : string.Empty;

            var mappedType = SimpleMapping(primaryType);
            if (string.IsNullOrEmpty(mappedType)) mappedType = primaryType;

            var mappedGenericType = string.Empty;

            if (!string.IsNullOrEmpty(genericType))
            {
                mappedGenericType = SimpleMapping(genericType);
                if (string.IsNullOrEmpty(mappedGenericType)) mappedGenericType = genericType;
            }

            var finalType = mappedType;

            if (mappedType == "List")
            {
                finalType += $"<{mappedGenericType}>";
            }

            if (isArray && primaryType is not "data" or "bytes")
            {
                finalType += "[]";
            }

            return finalType;
        }

        private void AppendDefaultCtor()
        {
            sb.AppendLine();
            sb.AppendLine("    /// <summary>");
            sb.Append("    /// Creates a new instance of <see cref=\"");
            sb.Append(chunkl.Header.Name);
            sb.AppendLine("\"/> with no chunks inside (no data will be serialized).");
            sb.AppendLine("    /// </summary>");
            sb.Append("    public ");
            sb.Append(chunkl.Header.Name);
            sb.AppendLine("() { }");
        }

        private void AppendChunks(ImmutableArray<ISymbol> existingMembers)
        {
            var existingChunkSymbols = existingMembers
                .OfType<INamedTypeSymbol>()
                .Where(x => x.AllInterfaces.Any(x => x.Name == "IChunk"))
                .ToDictionary(x => x.Name);

            foreach (var chunk in chunkl.Body.ChunkDefinitions)
            {
                var isHeaderChunk = chunk.Properties.ContainsKey("header");
                var isSkippableChunk = chunk.Properties.ContainsKey("skippable");

                if (isHeaderChunk && isSkippableChunk)
                {
                    throw new Exception("Chunk cannot be a header chunk and a skippable chunk at the same time.");
                }

                var fullId = chunkl.Header.Id | chunk.Id;

                var hasExistingChunkSymbol = false;
                var existingChunkSymbol = default(INamedTypeSymbol);
                if (isHeaderChunk)
                {
                    hasExistingChunkSymbol = existingChunkSymbols.TryGetValue($"HeaderChunk{fullId:X8}", out existingChunkSymbol);
                }
                else
                {
                    hasExistingChunkSymbol = existingChunkSymbols.TryGetValue($"Chunk{fullId:X8}", out existingChunkSymbol);
                }

                if (existingChunkSymbol?.IsValueType ?? false)
                {
                    continue; // TODO: Better determine struct behaviour
                }

                sb.AppendLine();

                sb.AppendLine("    /// <summary>");
                sb.Append("    /// ");

                var hasStructName = chunk.Properties.TryGetValue("struct", out var structName);

                if (hasStructName)
                {
                    sb.Append('[');
                    sb.Append(structName);
                    sb.Append("] ");
                }

                sb.Append(chunkl.Header.Name);
                sb.Append(" 0x");
                sb.Append(chunk.Id.ToString("X3"));

                if (isHeaderChunk)
                {
                    sb.Append(" header");
                }
                else if (isSkippableChunk)
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

                var hasChunkAttribute = existingChunkSymbol?.GetAttributes()
                    .Any(x => x.AttributeClass?.Name == "ChunkAttribute") ?? false;

                if (!hasChunkAttribute)
                {
                    sb.Append("    [Chunk(0x");
                    sb.Append(fullId.ToString("X8"));

                    if (!string.IsNullOrWhiteSpace(chunk.Description))
                    {
                        sb.Append(", \"");
                        sb.Append(chunk.Description);
                        sb.Append('\"');
                    }

                    sb.AppendLine(")]");
                }

                sb.Append("    public ");

                if (hasExistingChunkSymbol)
                {
                    sb.Append("partial ");
                }

                sb.Append("class ");

                if (isHeaderChunk)
                {
                    sb.Append("Header");
                }

                sb.Append("Chunk");
                sb.Append(fullId.ToString("X8"));

                sb.Append(" : ");

                if (chunk.Properties.TryGetValue("base", out var baseChunkIdStr))
                {
                    sb.Append("Chunk");
                    if (baseChunkIdStr.StartsWith("0x")) baseChunkIdStr = baseChunkIdStr.Substring(2);
                    var baseChunkId = chunkl.Header.Id | uint.Parse(baseChunkIdStr, NumberStyles.HexNumber);
                    sb.Append(baseChunkId.ToString("X8"));
                }
                else
                {
                    if (isHeaderChunk)
                    {
                        sb.Append("Header");
                    }

                    sb.Append("Chunk<");
                    sb.Append(chunkl.Header.Name);
                    sb.Append(">");
                }

                if (isSkippableChunk)
                {
                    sb.Append(", ISkippableChunk");
                }

                if (chunk.IsVersionable)
                {
                    sb.Append(", IVersionable");
                }

                sb.AppendLine();

                sb.AppendLine("    {");

                var existingChunkMembers = existingChunkSymbol?.GetMembers() ?? ImmutableArray<ISymbol>.Empty;

                AppendChunkIdMember(existingChunkMembers, fullId);

                if (chunk.IsVersionable)
                {
                    sb.AppendLine();
                    sb.AppendLine("        public int Version { get; set; }");
                }

                if (chunk.Members.Count > 0)
                {
                    var chunkContentsWriter = new ChunkContentsWriter(chunkl, chunk, sb, existingChunkMembers);
                    chunkContentsWriter.Write();
                }

                sb.AppendLine("    }");
            }
        }

        private void AppendChunkIdMember(ImmutableArray<ISymbol> existingMembers, uint fullId)
        {
            if (existingMembers.Any(x => x.IsOverride && x.Name == "Id"))
            {
                return;
            }

            sb.Append("        public override uint Id => 0x");
            sb.Append(fullId.ToString("X8"));
            sb.AppendLine(";");
        }

        private void AppendStaticMethods(ImmutableArray<ISymbol> existingMembers)
        {
            var hasInherits = data.DataModel.Header.Features.TryGetValue("inherits", out var inherits);
            inherits ??= existingSymbol?.BaseType?.Name ?? nameof(Object);
            if (inherits == nameof(Object)) inherits = "CMwNod";

            var hasNewHeaderChunkMethod = existingMembers
                .OfType<IMethodSymbol>()
                .Any(x => x.IsStatic
                    && x.Name == "NewHeaderChunk"
                    && x.ReturnType.Name == "IHeaderChunk"
                    && x.Parameters.Length == 1
                    && x.Parameters[0].Type.Name == nameof(UInt32));

            if (!hasNewHeaderChunkMethod)
            {
                sb.AppendLine();
                sb.AppendLine("    public static new IHeaderChunk? NewHeaderChunk(uint chunkId) => chunkId switch");
                sb.AppendLine("    {");

                foreach (var chunk in chunkl.Body.ChunkDefinitions)
                {
                    if (!chunk.Properties.ContainsKey("header"))
                    {
                        continue;
                    }

                    var fullId = chunkl.Header.Id | chunk.Id;

                    sb.Append("        0x");
                    sb.Append(fullId.ToString("X8"));
                    sb.Append(" => new HeaderChunk");
                    sb.Append(fullId.ToString("X8"));
                    sb.AppendLine("(),");
                }

                sb.Append("        _ => ");
                sb.Append(inherits);
                sb.AppendLine(".NewHeaderChunk(chunkId),");
                sb.AppendLine("    };");
            }

            var hasNewChunkMethod = existingMembers
                .OfType<IMethodSymbol>()
                .Any(x => x.IsStatic
                    && x.Name == "NewChunk"
                    && x.ReturnType.Name == "IChunk"
                    && x.Parameters.Length == 1
                    && x.Parameters[0].Type.Name == nameof(UInt32));

            if (!hasNewChunkMethod)
            {
                sb.AppendLine();
                sb.AppendLine("    public static new IChunk? NewChunk(uint chunkId) => chunkId switch");
                sb.AppendLine("    {");

                foreach (var chunk in chunkl.Body.ChunkDefinitions)
                {
                    if (chunk.Properties.ContainsKey("header"))
                    {
                        continue;
                    }

                    var fullId = chunkl.Header.Id | chunk.Id;

                    sb.Append("        0x");
                    sb.Append(fullId.ToString("X8"));
                    sb.Append(" => new Chunk");
                    sb.Append(fullId.ToString("X8"));
                    sb.AppendLine("(),");
                }

                sb.Append("        _ => ");
                sb.Append(inherits);
                sb.AppendLine(".NewChunk(chunkId),");
                sb.AppendLine("    };");
            }
        }
    }

    private class ChunkContentsWriter
    {
        private readonly ChunkLDataModel chunkl;
        private readonly ChunkDefinition chunk;
        private readonly StringBuilder sb;
        private readonly ImmutableArray<ISymbol> existingChunkMembers;

        public ChunkContentsWriter(
            ChunkLDataModel chunkl,
            ChunkDefinition chunk,
            StringBuilder sb,
            ImmutableArray<ISymbol> existingChunkMembers)
        {
            this.chunkl = chunkl;
            this.chunk = chunk;
            this.sb = sb;
            this.existingChunkMembers = existingChunkMembers;
        }

        internal void Write()
        {
            var unknownCounter = 0;

            WriteProperties(chunk, ref unknownCounter);

            sb.AppendLine();

            // RW mode
            sb.Append("        internal override void ReadWrite(");
            sb.Append(chunkl.Header.Name);
            sb.AppendLine(" n, GbxReaderWriter rw)");
            sb.AppendLine("        {");

            unknownCounter = 0;

            foreach (var mem in chunk.Members)
            {
                switch (mem)
                {
                    case ChunkProperty prop:
                        sb.Append("            ");

                        var standardProperty = default(string);

                        switch (prop.Type.ToLowerInvariant())
                        {
                            case "version":
                                sb.Append("rw.VersionInt32(this);");
                                break;
                            case "versionb":
                                sb.Append("rw.VersionByte(this);");
                                break;
                            case "bool":
                            case "boolean":
                                standardProperty = nameof(Boolean);
                                break;
                            case "string":
                                standardProperty = nameof(String);
                                break;
                            case "byte":
                                standardProperty = nameof(Byte);
                                break;
                            case "short":
                            case "int16":
                                standardProperty = nameof(Int16);
                                break;
                            case "ushort":
                            case "uint16":
                                standardProperty = nameof(UInt16);
                                break;
                            case "int":
                            case "int32":
                                standardProperty = nameof(Int32);
                                break;
                            case "uint":
                            case "uint32":
                                standardProperty = nameof(UInt32);
                                break;
                            case "long":
                            case "int64":
                                standardProperty = nameof(Int32);
                                break;
                            case "ulong":
                            case "uint64":
                                standardProperty = nameof(UInt32);
                                break;
                            case "float":
                                standardProperty = nameof(Single);
                                break;
                            case "byte3":
                                standardProperty = "Byte3";
                                break;
                            case "int2":
                                standardProperty = "Int2";
                                break;
                            case "int3":
                                standardProperty = "Int3";
                                break;
                            case "vec2":
                                standardProperty = "Vec2";
                                break;
                            case "vec3":
                                standardProperty = "Vec3";
                                break;
                            case "vec4":
                                standardProperty = "Vec4";
                                break;
                            case "int128":
                                standardProperty = "Int128";
                                break;
                            case "id":
                            case "lookbackstring":
                                standardProperty = "Id";
                                break;
                            case "ident":
                            case "meta":
                                standardProperty = "Ident";
                                break;
                            case "fileref":
                            case "packdesc":
                                standardProperty = "PackDesc";
                                break;
                            case "timeint":
                                standardProperty = "TimeInt32";
                                break;
                            case "timefloat":
                                standardProperty = "TimeSingle";
                                break;
                            case "timeofday":
                                standardProperty = "TimeOfDay";
                                break;
                            case "base":
                                sb.Append("base.ReadWrite(n, rw);");
                                break;
                            case "data":
                            case "bytes":
                                standardProperty = "Data";
                                break;
                            default:
                                var regex = Regex.Match(prop.Type, TypeMatch);

                                if (!regex.Success)
                                {
                                    sb.Append("/* invalid type */");
                                    break;
                                }

                                var primaryType = regex.Groups[1].Value;
                                var genericType = regex.Groups[3].Value;
                                var isArray = regex.Groups[4].Success;
                                var arrayLength = isArray ? regex.Groups[5].Value : string.Empty;
                                var deprec = regex.Groups[6].Value;

                                var mappedGenericType = string.Empty;

                                if (!string.IsNullOrEmpty(genericType))
                                {
                                    mappedGenericType = SourceGeneratorPrimaryContext.SimpleMapping(genericType);
                                    if (string.IsNullOrEmpty(mappedGenericType)) mappedGenericType = genericType;
                                }

                                switch (primaryType)
                                {
                                    case "list":
                                        sb.Append("rw.List");
                                        sb.Append(deprec);
                                        sb.Append("<");
                                        sb.Append(mappedGenericType);
                                        sb.Append(">(ref ");
                                        AppendPropertyName(prop, ref unknownCounter);
                                        sb.Append(");");
                                        break;
                                    case "data":
                                        sb.Append("rw.Data(ref ");
                                        AppendPropertyName(prop, ref unknownCounter);

                                        if (isArray)
                                        {
                                            sb.Append(", ");
                                            sb.Append(arrayLength);
                                        }

                                        sb.Append(");");
                                        break;
                                    default:
                                        if (isArray)
                                        {
                                            sb.Append("rw.Array");
                                            sb.Append(deprec);
                                            sb.Append("<");
                                            var mappedType = SourceGeneratorPrimaryContext.SimpleMapping(primaryType);
                                            if (string.IsNullOrEmpty(mappedType)) mappedType = primaryType;
                                            sb.Append(mappedType);
                                            sb.Append(">(ref ");
                                            AppendPropertyName(prop, ref unknownCounter);
                                            sb.Append(");");
                                        }
                                        else
                                        {
                                            sb.Append("rw.NodeRef<");
                                            sb.Append(prop.Type);
                                            sb.Append(">(ref ");
                                            AppendPropertyName(prop, ref unknownCounter);
                                            sb.Append(");");
                                        }

                                        break;
                                }

                                break;
                        }

                        if (standardProperty is not null)
                        {
                            sb.Append("rw.");
                            sb.Append(standardProperty);
                            sb.Append("(ref ");
                            AppendPropertyName(prop, ref unknownCounter);
                            sb.Append(");");
                        }

                        if (!string.IsNullOrWhiteSpace(prop.Description))
                        {
                            sb.Append(" // ");
                            sb.Append(prop.Description);
                        }

                        sb.AppendLine();

                        break;
                }
            }

            sb.AppendLine("        }");
        }

        private void WriteProperties(IChunkMemberBlock block, ref int unknownCounter)
        {
            var firstLine = true;

            foreach (var mem in block.Members)
            {
                switch (mem)
                {
                    case ChunkProperty prop:
                        if (prop.Type is "return" or "version" or "versionb" or "base" or "throw") break;
                        if (prop.Type.StartsWith("throw ")) break;
                        if (!string.IsNullOrWhiteSpace(prop.Name)) break;

                        if (firstLine)
                        {
                            sb.AppendLine();
                            firstLine = false;
                        }

                        var mappedType = SourceGeneratorPrimaryContext.SimpleMapping(prop.Type);
                        if (string.IsNullOrEmpty(mappedType)) mappedType = SourceGeneratorPrimaryContext.AdvancedMapping(prop.Type);

                        unknownCounter++;

                        sb.Append("        public ");
                        sb.Append(mappedType);
                        sb.Append(' ');
                        sb.Append("U");
                        sb.Append(unknownCounter.ToString("00"));
                        sb.AppendLine(";");
                        break;
                    case IChunkMemberBlock b:
                        WriteProperties(b, ref unknownCounter);
                        break;
                }
            }
        }

        private void AppendPropertyName(ChunkProperty prop, ref int unknownCounter)
        {
            if (string.IsNullOrEmpty(prop.Name))
            {
                unknownCounter++;
                sb.Append("U");
                sb.Append(unknownCounter.ToString("00"));
            }
            else
            {
                sb.Append("n.");
                sb.Append(char.ToLowerInvariant(prop.Name[0]) + prop.Name.Substring(1));
            }
        }
    }
}