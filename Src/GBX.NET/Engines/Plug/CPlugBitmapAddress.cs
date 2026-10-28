namespace GBX.NET.Engines.Plug;

public partial class CPlugBitmapAddress
{
    public bool AreUVProjected
    {
        get => (AddressFlags & 0x100) != 0;
        set => AddressFlags = value ? AddressFlags | 0x100 : AddressFlags & 0xFFFFFEFF;
    }

    public CPlugBitmap.EGxUVGenerate GenerateUV
    {
        get => (CPlugBitmap.EGxUVGenerate)(AddressFlags & 0xFF);
        set => AddressFlags = (AddressFlags & 0xFFFFFF00) | ((uint)value & 0xFF);
    }

    public int TexCoordIndex
    {
        get => (int)(AddressFlags << 12) >> 27;
        set => AddressFlags = (AddressFlags & 0xFFF07FFF) | (((uint)value & 0x1F) << 15);
    }

    [Obsolete("Use TransfoIso3 instead.")]
    public BoxAligned? UVTransfoIso3
    {
        get => TransfoIso3 is Iso3 v ? new BoxAligned(v.XX, v.XY, v.YX, v.YY, v.TX, v.TY) : null;
        set => TransfoIso3 = value is BoxAligned v ? new Iso3(v.X, v.Y, v.Z, v.X2, v.Y2, v.Z2) : null;
    }
}
