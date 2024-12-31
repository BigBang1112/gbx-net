using GBX.NET.Serialization;

namespace GBX.NET.PAK;

public static class GbxReaderExtensions
{
    private static readonly byte[] nadeoPakMagic = [(byte)'N', (byte)'a', (byte)'d', (byte)'e', (byte)'o', (byte)'P', (byte)'a', (byte)'k'];

    public static bool ReadPakMagic(this GbxReader reader)
    {
        var magicLength = nadeoPakMagic.Length;
#if NETSTANDARD2_0
        var magic = reader.ReadBytes(magicLength);
#else
        Span<byte> magic = stackalloc byte[magicLength];
        if (reader.Read(magic) != magicLength)
        {
            throw new EndOfStreamException("Failed to read NadeoPak magic bytes.");
        }
#endif

        for (var i = 0; i < magicLength; i++)
        {
            if (magic[i] != nadeoPakMagic[i])
            {
                return false;
            }
        }

        return true;
    }
}
