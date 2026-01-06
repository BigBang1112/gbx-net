using static GBX.NET.BitHelper;

namespace GBX.NET.Engines.Plug;

public partial class CPlugBitmap
{
    public EUsage Usage
    {
        get => (EUsage)GetBitRange(Flags, 8, 8);
        set => Flags = SetBitRange(Flags, 8, 8, (int)value);
    }

    public int PixelUpdate
    {
        get => GetBitRange(Flags, 0, 8);
        set => Flags = SetBitRange(Flags, 0, 8, value);
    }

    public partial class Chunk09011030 : IVersionable
    {
        public int Version { get; set; }

        public Int3 U01;
        public CMwNod? U02;
        public CMwNod? U03;

        public override void ReadWrite(CPlugBitmap n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            n.image = (CPlugFileImg?)rw.NodeRef((CMwNod?)n.image, ref n.imageFile);
            if (Version >= 1)
            {
                rw.Int3(ref U01);
            }
            if (Version == 2)
            {
                rw.NodeRef<CMwNod>(ref U02);
            }
            rw.Single(ref n.mipMapLowerAlpha);
            rw.Single(ref n.bumpScaleFactor);
            rw.Single(ref n.mipMapLodBiasDefault);
            rw.Int32(ref n.borderRGB);
            if (n.Image != null)
            {
                rw.NodeRef<CMwNod>(ref U03);
            }
        }
    }
}
