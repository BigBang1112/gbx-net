namespace GBX.NET.Serialization;

public partial interface IGbxWriter
{
    void WriteSByteNullable(sbyte? value);
    void WriteInt16Nullable(short? value);
    void WriteInt32Nullable(int? value);
    void WriteInt64Nullable(long? value);
    void WriteInt128Nullable(Int128? value);
}

public partial class GbxWriter
{
    public void WriteSByteNullable(sbyte? value) => Write(value ?? -1);

    public void WriteInt16Nullable(short? value) => Write(value ?? -1);

    public void WriteInt32Nullable(int? value) => Write(value ?? -1);

    public void WriteInt64Nullable(long? value) => Write(value ?? -1);

    public void WriteInt128Nullable(Int128? value)
        => WriteInt128(value ?? new Int128(ulong.MaxValue, ulong.MaxValue));
}
