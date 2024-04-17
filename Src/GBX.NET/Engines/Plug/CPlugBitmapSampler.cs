namespace GBX.NET.Engines.Plug;

public partial class CPlugBitmapSampler
{
    public partial class Chunk0907E008
    {
        public uint U04;

        public override void ReadWrite(CPlugBitmapSampler n, GbxReaderWriter rw)
        {
            base.ReadWrite(n, rw);

            // CPlugBitmapSampler::IsBorderColorUsed
            if ((((byte)U02 & 0x18) != 0x18) && (((byte)U02 & 0x60) != 0x60) && ((U02 & 0x1800) != 0x1800))
            {
                return;
            }

            rw.UInt32(ref U04);
        }
    }
}
