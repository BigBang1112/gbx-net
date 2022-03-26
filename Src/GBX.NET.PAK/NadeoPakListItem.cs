namespace GBX.NET.PAK;

public record NadeoPakListItem(byte[] Key, byte Flags)
{
    public override string ToString()
    {
        return BitConverter.ToString(Key);
    }
}
