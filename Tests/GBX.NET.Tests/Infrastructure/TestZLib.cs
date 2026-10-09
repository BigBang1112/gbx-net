using GBX.NET.Extensions;
using System.IO.Compression;

namespace GBX.NET.Tests.Infrastructure;

internal sealed class TestZLib : IZLib
{
    public void Compress(Stream input, Stream output)
    {
        using var stream = new ZLibStream(output, CompressionLevel.Optimal, leaveOpen: true);
        // .NET 8 needs a write to emit an empty zlib stream; CopyTo skips empty input.
        stream.Write(ReadOnlySpan<byte>.Empty);
        input.CopyTo(stream);
    }

    public void Decompress(Stream input, Stream output)
    {
        using var stream = Decompress(input);
        stream.CopyTo(output);
    }

    public Stream Decompress(Stream input) => new ZLibStream(input, CompressionMode.Decompress, leaveOpen: true);
}
