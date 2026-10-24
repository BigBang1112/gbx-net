namespace GBX.NET.Serialization;

public partial interface IGbxReader
{
    sbyte? ReadSByteNullable();
    short? ReadInt16Nullable();
    int? ReadInt32Nullable();
    long? ReadInt64Nullable();
    Int128? ReadInt128Nullable();
}

public partial class GbxReader
{
    public sbyte? ReadSByteNullable()
    {
        var value = ReadSByte();
        return value == -1 ? null : value;
    }

    public short? ReadInt16Nullable()
    {
        var value = ReadInt16();
        return value == -1 ? null : value;
    }

    public int? ReadInt32Nullable()
    {
        var value = ReadInt32();
        return value == -1 ? null : value;
    }

    public long? ReadInt64Nullable()
    {
        var value = ReadInt64();
        return value == -1 ? null : value;
    }

    public Int128? ReadInt128Nullable()
    {
        var value = ReadInt128();
        return value == new Int128(ulong.MaxValue, ulong.MaxValue) ? null : value;
    }
}
