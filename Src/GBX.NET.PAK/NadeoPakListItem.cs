namespace GBX.NET.PAK;

public class NadeoPakListItem
{
    public string Name { get; set; }
    public byte Flags { get; set; }
    public byte[] Key { get; set; }

    public NadeoPakListItem(string name, byte flags, byte[] key)
    {
        Name = name;
        Flags = flags;
        Key = key;
    }

    public override string ToString()
    {
        return $"{Name} ({BitConverter.ToString(Key)})";
    }
}
