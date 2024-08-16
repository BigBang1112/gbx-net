namespace GBX.NET.PAK;

public sealed record PakListItem(byte[] Key, byte Flags)
{
    public override string ToString()
    {
        return BitConverter.ToString(Key);
    }
}
