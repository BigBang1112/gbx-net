using GBX.NET.Extensions;

namespace GBX.NET.ZLib;

public sealed class ZLib : IZLib
{
    private readonly ZLibDotNet.ZLib zlib = new();

    public byte[] Compress(ReadOnlySpan<byte> input)
    {
        var sourceLen = zlib.CompressBound((uint)input.Length);

        var output = new byte[sourceLen];

        ZLibDotNet.ZStream zStream = new()
        {
            Input = input,
            Output = output
        };

        _ = zlib.DeflateInit(ref zStream, ZLibDotNet.ZLib.Z_DEFAULT_COMPRESSION);
        _ = zlib.Deflate(ref zStream, ZLibDotNet.ZLib.Z_FULL_FLUSH);
        _ = zlib.DeflateEnd(ref zStream);

        return output;
    }

    public void Decompress(ReadOnlySpan<byte> input, Span<byte> output)
    {
        var zStream = new ZLibDotNet.ZStream
        {
            Input = input,
            Output = output
        };

        _ = zlib.InflateInit(ref zStream);
        _ = zlib.Inflate(ref zStream, ZLibDotNet.ZLib.Z_SYNC_FLUSH);
    }
}
