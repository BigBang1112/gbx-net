using System.Buffers.Binary;

namespace GBX.NET;

public static class UInt128Extensions
{
    public static void WriteLittleEndian(this UInt128 value, Span<byte> destination)
    {
        if (destination.Length < 16)
        {
            throw new ArgumentException("Destination span is too short.", nameof(destination));
        }

#if NET8_0_OR_GREATER
        BinaryPrimitives.WriteUInt128LittleEndian(destination, value);
#else
        BinaryPrimitives.WriteUInt64LittleEndian(destination, value.Low);
        BinaryPrimitives.WriteUInt64LittleEndian(destination.Slice(8), value.High);
#endif
    }

    public static byte[] GetBytes(this UInt128 value)
    {
        var bytes = new byte[16];
        value.WriteLittleEndian(bytes);
        return bytes;
    }
}