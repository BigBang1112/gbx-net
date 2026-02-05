namespace GBX.NET.PAK;

public sealed record PakListItem(string Key, byte Flags)
{
    public byte[] GetBytes()
    {
        return Convert.FromHexString(Key);
    }
}
