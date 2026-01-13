using GBX.NET.Extensions;

namespace GBX.NET.LZO;

public sealed class Lzo : ILzo
{
    public byte[] Compress(byte[] data)
    {
        return SharpLzo.Lzo.Compress(SharpLzo.CompressionMode.Lzo1x_999, data);
    }

    public void Decompress(in Span<byte> input, byte[] output)
    {
        var result = SharpLzo.Lzo.TryDecompress(input, input.Length, output, out var _);
        
        if (result != SharpLzo.LzoResult.OK)
        {
            throw new SharpLzo.LzoException(result);
        }
    }
}
