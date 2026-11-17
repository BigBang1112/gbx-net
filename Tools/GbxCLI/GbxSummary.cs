using GBX.NET;
using GBX.NET.Components;

namespace GbxCLI;

/// <summary>
/// Writes the <c>inspect</c> summary of a Gbx: its header, body, external references and node.
/// </summary>
internal sealed class GbxSummary(CliConsole console)
{
    private readonly ConsoleValueWriter values = new(console);

    public async Task WriteAsync(Gbx gbx)
    {
        await SectionAsync("Header");
        await values.LineAsync("Header", gbx.Header);
        await values.LineAsync("Class ID", $"0x{gbx.Header.ClassId:X8}");
        await values.LineAsync("Version", gbx.Header.Basic.Version);
        await values.LineAsync("Format", gbx.Header.Basic.Format);
        await values.LineAsync("Serialization mode", gbx.Header.Basic.Mode);
        await values.LineAsync("Body compression", gbx.Header.Basic.CompressionOfBody);
        await values.LineAsync("Reference table compression", gbx.Header.Basic.CompressionOfRefTable);
        await values.LineAsync("Declared nodes", gbx.Header.NumNodes);

        await SectionAsync("Body");

        // Uncompressed bodies do not store their size in the header.
        if (gbx.Body.CompressedSize is int compressedSize)
        {
            await values.LineAsync("Compressed size", $"{compressedSize:N0} bytes");
            await values.LineAsync("Uncompressed size", $"{gbx.Body.UncompressedSize:N0} bytes");

            if (gbx.Body.UncompressedSize > 0)
            {
                await values.LineAsync("Compression ratio", $"{gbx.Body.CompressionRatio:P2}");
            }
        }

        await values.LineAsync("Parse status", gbx.Node is null ? "Unknown class (node unavailable)"
            : gbx.Body.Exception is null ? "Complete" : $"Partial: {gbx.Body.Exception.Message}");
        await values.LineAsync("ID version", gbx.IdVersion);
        await values.LineAsync("Pack descriptor version", gbx.PackDescVersion);
        await values.LineAsync("Deprecation version", gbx.DeprecVersion);
        await values.LineAsync("Class ID remapping", gbx.ClassIdRemapMode);

        await SectionAsync("External references");

        if (gbx.RefTable is null)
        {
            await values.LineAsync("References", "None");
        }
        else
        {
            await values.LineAsync("Ancestor level", gbx.RefTable.AncestorLevel);
            await values.WriteValueAsync("Files", gbx.RefTable.Files, 1);
            await values.WriteValueAsync("Resources", gbx.RefTable.Resources, 1);
        }

        if (gbx.Header is GbxHeaderUnknown unknown)
        {
            await SectionAsync("Header chunks");
            await values.WriteValueAsync("Chunks", unknown.UserData, 1);
        }

        await SectionAsync("Node");
        values.HasExternalReferences = ValueInspector.HasExternalReferences(gbx);
        await values.WriteValueAsync("Node", gbx.Node, 0);
        await values.WriteTruncationNoticeAsync();

        await console.WriteLineAsync(string.Empty);
        await console.WriteLineAsync($"{CliConsole.Label}Preview limits: {ConsoleValueWriter.CollectionLimit} items per collection ({ConsoleValueWriter.ChunkLimit} chunks), "
            + $"{ConsoleValueWriter.DepthLimit} nesting levels, {ValueFormatter.TextLimit} characters per value, {ConsoleValueWriter.LineLimit} detail lines. "
            + $"Use gbx <file> to browse everything, or gbx eval <file> <expression>.{CliConsole.Reset}");
    }

    private async Task SectionAsync(string title)
    {
        await console.WriteLineAsync(string.Empty);
        await console.WriteLineAsync($"{CliConsole.Heading}{CliConsole.Escape(title)}{CliConsole.Reset}");
    }
}
