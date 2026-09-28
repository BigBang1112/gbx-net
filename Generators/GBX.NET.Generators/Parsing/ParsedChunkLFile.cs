using ChunkL.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace GBX.NET.Generators.Parsing;

/// <summary>A successfully parsed layout with its engine and original source.</summary>
internal sealed class ParsedChunkLFile(string path, string engine, SourceText source, ChunkLFile syntax)
{
    public string Path { get; } = path;
    public string Engine { get; } = engine;
    public string Namespace => "GBX.NET.Engines." + Engine;
    public string TypeKey => Namespace + "." + Syntax.Header.ClassName;
    public SourceText Source { get; } = source;
    public ChunkLFile Syntax { get; } = syntax;
}
