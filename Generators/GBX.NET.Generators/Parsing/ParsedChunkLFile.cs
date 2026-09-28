using ChunkL.Syntax;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using System.Collections.Immutable;

namespace GBX.NET.Generators.Parsing;

/// <summary>A successfully parsed layout with its engine and original source.</summary>
internal sealed class ParsedChunkLFile
{
    public string Path { get; }
    public string Engine { get; }
    public string Namespace => "GBX.NET.Engines." + Engine;
    public string TypeKey => Namespace + "." + Syntax.Header.ClassName;
    public SourceText Source { get; }
    public ChunkLFile Syntax { get; }

    public ParsedChunkLFile(string path, string engine, SourceText source, ChunkLFile syntax)
    {
        Path = path;
        Engine = engine;
        Source = source;
        Syntax = syntax;
    }
}

internal sealed class ChunkLParseResult
{
    // Failed/recovered ASTs are not passed to later generation stages.
    public ParsedChunkLFile? File { get; }
    public ImmutableArray<Diagnostic> Diagnostics { get; }

    public ChunkLParseResult(ParsedChunkLFile? file, ImmutableArray<Diagnostic> diagnostics)
    {
        File = file;
        Diagnostics = diagnostics;
    }
}
