using ChunkL;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using System.Collections.Immutable;
using System.IO;
using System.Threading;
using ChunkLSeverity = ChunkL.Diagnostics.DiagnosticSeverity;

namespace GBX.NET.Generators.Parsing;

internal static class ChunkLParsing
{
    private static readonly DiagnosticDescriptor UnreadableFile = new(
        "GBXNETGEN100", "Cannot read ChunkL file", "Cannot read ChunkL file '{0}'",
        "GBX.NET.Generators", DiagnosticSeverity.Error, isEnabledByDefault: true);

    private static readonly DiagnosticDescriptor ParseError = new(
        "GBXNETGEN101", "ChunkL parse error", "{0}",
        "GBX.NET.Generators", DiagnosticSeverity.Error, isEnabledByDefault: true);

    private static readonly DiagnosticDescriptor ParseWarning = new(
        "GBXNETGEN102", "ChunkL parse warning", "{0}",
        "GBX.NET.Generators", DiagnosticSeverity.Warning, isEnabledByDefault: true);

    private static readonly DiagnosticDescriptor ParseInfo = new(
        "GBXNETGEN103", "ChunkL parse information", "{0}",
        "GBX.NET.Generators", DiagnosticSeverity.Info, isEnabledByDefault: true);

    public static ChunkLParseResult Parse(AdditionalText additionalText, CancellationToken token)
    {
        token.ThrowIfCancellationRequested();
        var start = Location.Create(additionalText.Path, default, default);

        try
        {
            var source = additionalText.GetText(token);
            if (source is null)
            {
                return new ChunkLParseResult(null, ImmutableArray.Create(
                    Diagnostic.Create(UnreadableFile, start, additionalText.Path)));
            }

            // ParseSource consumes the snapshot supplied by Roslyn, never a disk read.
            // Keep the package's AST intact, including source order and wire types.
            var result = ChunkLParser.ParseSource(source.ToString());
            token.ThrowIfCancellationRequested();
            var diagnostics = result.Diagnostics.Select(diagnostic => Diagnostic.Create(
                diagnostic.Severity switch
                {
                    ChunkLSeverity.Warning => ParseWarning,
                    ChunkLSeverity.Info => ParseInfo,
                    _ => ParseError
                },
                GetLocation(additionalText.Path, source, diagnostic.Position),
                string.IsNullOrEmpty(diagnostic.Code) ? diagnostic.Message :
                    $"{diagnostic.Code}: {diagnostic.Message}")).ToImmutableArray();

            var engine = Path.GetFileName(Path.GetDirectoryName(additionalText.Path)) ?? "";
            var file = result.Success && result.File is not null
                ? new ParsedChunkLFile(additionalText.Path, engine, source, result.File)
                : null;

            return new ChunkLParseResult(file, diagnostics);
        }
        catch (Exception exception) when (exception is not OperationCanceledException)
        {
            // Isolate a failed input so other layouts can still be parsed. Cancellation
            // propagates to Roslyn rather than becoming a misleading parse diagnostic.
            return new ChunkLParseResult(null, ImmutableArray.Create(
                Diagnostic.Create(ParseError, start, $"Failed to parse ChunkL file: {exception.Message}")));
        }
    }

    private static Location GetLocation(string path, SourceText source, SourcePosition position)
    {
        // ChunkL positions are 1-based. Clamp recovery/EOF positions to the snapshot;
        // underline one character when available, otherwise use an empty EOF span.
        var line = source.Lines[Math.Max(0, Math.Min(position.Line - 1, source.Lines.Count - 1))];
        var column = Math.Max(0, Math.Min(position.Column - 1, line.Span.Length));
        var span = new TextSpan(line.Start + column, column < line.Span.Length ? 1 : 0);
        return Location.Create(path, span, source.Lines.GetLinePositionSpan(span));
    }
}
