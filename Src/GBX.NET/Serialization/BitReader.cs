namespace GBX.NET.Serialization;

internal sealed class BitReader
{
    private readonly byte[] data;

    public int Position { get; private set; }
    public int Length { get; }

    public BitReader(byte[] data)
    {
        this.data = data;

        Length = data.Length * 8;
    }

    private bool ReadBit(byte[] data)
    {
        var result = (data[Position / 8] & (1 << (Position % 8))) != 0;
        Position++;
        return result;
    }

    public bool ReadBit()
    {
        return ReadBit(data);
    }

    public ulong ReadNumber(int bits)
    {
        ulong result = 0;

        for (var i = 0; i < bits; i++)
        {
            result |= (ulong)(ReadBit(data) ? 1 : 0) << i;
        }

        return result;
    }

    public byte Read2Bit()
    {
        return (byte)ReadNumber(bits: 2);
    }

    public byte ReadByte()
    {
        return (byte)ReadNumber(bits: 8);
    }

    public sbyte ReadSByte()
    {
        return (sbyte)ReadNumber(bits: 8);
    }

    public short ReadInt16()
    {
        return (short)ReadNumber(bits: 16);
    }

    public ushort ReadUInt16()
    {
        return (ushort)ReadNumber(bits: 16);
    }

    public int ReadInt32()
    {
        return (int)ReadNumber(bits: 32);
    }

    // ReadToEnd through bits

    public byte[] ReadToEnd()
    {
        var remaining = Length - Position;
        var result = new byte[(remaining + 7) / 8];
        for (var i = 0; i < remaining; i++)
            result[i / 8] |= (byte)((ReadBit() ? 1 : 0) << (i % 8));
        return result;
    }
}
