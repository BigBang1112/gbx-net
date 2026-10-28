using static GBX.NET.BitHelper;

namespace GBX.NET.Engines.Plug;

public partial class CPlugBitmap
{
    public uint LegacyFlags32
    {
        get => (uint)Flags;
        set => Flags = value;
    }

    public EUsage Usage
    {
        get => (EUsage)GetBitRange(Flags, 0, 8);
        set => Flags = SetBitRange(Flags, 0, 8, (int)value);
    }

    public int PixelUpdate
    {
        get => GetBitRange(Flags, 8, 8);
        set => Flags = SetBitRange(Flags, 8, 8, value);
    }
}
