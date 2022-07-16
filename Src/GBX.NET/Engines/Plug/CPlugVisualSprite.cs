namespace GBX.NET.Engines.Plug;

/// <remarks>ID: 0x09010000</remarks>
[Node(0x09010000)]
public class CPlugVisualSprite : CPlugVisual3D
{
    protected CPlugVisualSprite()
    {

    }

    /// <summary>
    /// CPlugVisualSprite 0x005 chunk
    /// </summary>
    [Chunk(0x09010005)]
    public class Chunk09010005 : Chunk<CPlugVisualSprite>
    {
        public int U01;
        public float U02;
        public float U03;
        public float U04;
        public float U05;
        public float U06;

        public override void ReadWrite(CPlugVisualSprite n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref U01);
            rw.Single(ref U02);
            rw.Single(ref U03);
            rw.Single(ref U04);
            rw.Single(ref U05);
            rw.Single(ref U06);
        }
    }

    /// <summary>
    /// CPlugVisualSprite 0x006 chunk
    /// </summary>
    [Chunk(0x09010006)]
    public class Chunk09010006 : Chunk<CPlugVisualSprite>
    {
        public short U01;
        public short U02;

        public override void ReadWrite(CPlugVisualSprite n, GameBoxReaderWriter rw)
        {
            rw.Int16(ref U01);
            rw.Int16(ref U02);
        }
    }
}
