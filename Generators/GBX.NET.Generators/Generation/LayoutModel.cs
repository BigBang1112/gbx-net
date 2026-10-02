using ChunkL.Syntax;
using GBX.NET.Generators.Analysis;
using GBX.NET.Generators.Parsing;
using System.Collections.Immutable;
using System.Globalization;

namespace GBX.NET.Generators.Generation;

internal sealed class LayoutModel
{
    public ParsedChunkLFile File { get; }
    public ExistingType? Existing { get; }
    public string Name => File.Syntax.Header.ClassName;
    public string QualifiedName => "global::" + File.TypeKey;
    public uint Id { get; }
    public string? BaseName { get; }
    public bool IsAbstract { get; }
    public ArchiveDeclaration? SelfArchive => File.Syntax.Archives.FirstOrDefault(static x => string.IsNullOrEmpty(x.Name));
    public List<ChunkModel> Chunks { get; } = [];
    public ScopeModel Scope { get; }
    public Dictionary<string, ScopeModel> Archives { get; } = new(StringComparer.Ordinal);

    public LayoutModel(ParsedChunkLFile file, ImmutableDictionary<string, ExistingType> existing)
    {
        File = file;

        existing.TryGetValue(file.TypeKey, out var implemented);
        Existing = implemented;

        Id = Hex(file.Syntax.Header.ClassId);
        BaseName = Name == "CMwNod" ? null : file.Syntax.ClassAttributes
            .FirstOrDefault(static x => x.Name == "inherits")?.Value ??
            implemented?.BaseTypes.Select(static x => x.ToString()).FirstOrDefault(static x => !x.StartsWith("I", StringComparison.Ordinal)) ?? "CMwNod";
        
        IsAbstract = implemented?.IsAbstract == true || file.Syntax.ClassAttributes.Any(static x => x.Name == "abstract");
        Scope = new ScopeModel(implemented, SelfArchive?.Attributes, SelfArchive?.Body ?? new List<IBodyStatement>());
        
        foreach (var chunk in file.Syntax.Chunks)
        {
            var id = Hex(chunk.Offset.HexValue);
            if (id <= 0xFFF) id |= Id;

            var name = (Has(chunk.Attributes, "header") ? "HeaderChunk" : "Chunk") + id.ToString("X8");

            existing.TryGetValue(file.TypeKey + "+" + name, out var chunkType);

            var model = new ChunkModel(id, name, chunk, new ScopeModel(chunkType, chunk.Attributes, chunk.Body));
            
            Chunks.Add(model);

            foreach (var field in model.Scope.Fields.Where(x => !OmitsDemonstrationMembers(chunk.Attributes) && !x.IsUnknown && !x.IsVersion))
            {
                foreach (var occurrence in field.Occurrences)
                {
                    Scope.Add(occurrence);
                }
            }
        }
        foreach (var archive in file.Syntax.Archives.Where(static x => !string.IsNullOrEmpty(x.Name)))
        {
            existing.TryGetValue(file.TypeKey + "+" + archive.Name, out var archiveType);
            Archives.Add(archive.Name!, new ScopeModel(archiveType, archive.Attributes, archive.Body));
        }
    }

    public static uint Hex(string value)
    {
        return uint.Parse(value.Replace("0x", ""), NumberStyles.HexNumber, CultureInfo.InvariantCulture);
    }

    public static bool Has(AttributeList? attributes, string name)
    {
        return attributes?.Entries.Any(x => x.Name == name) == true;
    }

    public static bool OmitsDemonstrationMembers(AttributeList? attributes)
    {
        return Has(attributes, "demonstration") && Attribute(attributes, "demonstration") != "partial";
    }

    public static string? Attribute(AttributeList? attributes, string name)
    {
        return attributes?.Entries.FirstOrDefault(x => x.Name == name)?.Value;
    }

    public static string? WriteExpression(AttributeList? attributes)
    {
        var value = Attribute(attributes, "write");
        if (value is null) return null;

        var expression = Microsoft.CodeAnalysis.CSharp.SyntaxFactory.ParseExpression(value);
        return expression is Microsoft.CodeAnalysis.CSharp.Syntax.LiteralExpressionSyntax literal && literal.Token.Value is string text
            ? text : value;
    }
}
