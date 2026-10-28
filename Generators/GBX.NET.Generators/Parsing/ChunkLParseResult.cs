namespace GBX.NET.Generators.Parsing;

internal sealed class ChunkLParseResult(ParsedChunkLFile? file, ImmutableArray<Diagnostic> diagnostics)
{
    // Failed/recovered ASTs are not passed to later generation stages.
    public ParsedChunkLFile? File { get; } = file;
    public ImmutableArray<Diagnostic> Diagnostics { get; } = diagnostics;
}
