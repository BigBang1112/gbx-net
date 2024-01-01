using GBX.NET.Exceptions;
using System.Text;

namespace GBX.NET.Benchmarks.OldClasses;

public class GameBoxReader : BinaryReader
{
    private static readonly Encoding encoding = Encoding.UTF8;

    public GameBoxReader(Stream input) : base(input, encoding)
    {
    }

    public GameBoxReader(Stream input, bool leaveOpen) : base(input, encoding, leaveOpen)
    {
    }

    public override string ReadString()
    {
        return ReadString(ReadInt32());
    }

    public string ReadString(int length)
    {
        if (length == 0)
        {
            return "";
        }

        if (length < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(length), "Length cannot be negative.");
        }

        if (length > 0x10000000) // ~268MB
        {
            throw new LengthLimitException(length);
        }

#if NETSTANDARD2_1_OR_GREATER || NET6_0_OR_GREATER
        if (length > 1_500_000)
        {
            return Encoding.UTF8.GetString(ReadBytes(length));
        }

        Span<byte> bytes = stackalloc byte[length];

        if (Read(bytes) != length)
        {
            throw new EndOfStreamException();
        }

        return Encoding.UTF8.GetString(bytes);
#else
        return Encoding.UTF8.GetString(ReadBytes(length));
#endif
    }
}