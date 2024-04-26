using GBX.NET.Extensions;
using Ionic.Zlib;

namespace GBX.NET.ZLib;

public sealed class ZLib : IZLib
{
    public void Compress(Stream input, Stream output)
    {
        using var zlib = new ZlibStream(output, CompressionMode.Compress, leaveOpen: true);
        input.CopyTo(zlib);
    }

    public void Decompress(Stream input, Stream output)
    {
        using var zlib = new ZlibStream(input, CompressionMode.Decompress, leaveOpen: true);
        zlib.CopyTo(output);
    }
}
