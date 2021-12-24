namespace GBX.NET.Engines.Plug;

/// <summary>
/// Bitmap address (0x09047000)
/// </summary>
[Node(0x09047000)]
public class CPlugBitmapAddress : CPlugBitmapSampler
{
    protected CPlugBitmapAddress()
    {

    }

    [Chunk(0x09047007)]
    public class Chunk09047007 : Chunk<CPlugBitmapAddress>
    {
        public int U01;
        public int U02;
        public byte U03;

        public override void ReadWrite(CPlugBitmapAddress n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref U01);
            rw.Int32(ref U02);
            rw.Byte(ref U03);
        }
    }

    [Chunk(0x09047008)]
    public class Chunk09047008 : Chunk<CPlugBitmapAddress>
    {
        public int U01;
        public float U02;

        public override void ReadWrite(CPlugBitmapAddress n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref U01); // ArchiveCountAndElems SMapFxDescOld/SBitmapElemToPack count*0x14
            rw.Single(ref U02);
        }
    }
}
