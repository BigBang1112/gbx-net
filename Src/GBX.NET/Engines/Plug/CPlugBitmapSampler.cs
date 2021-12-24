namespace GBX.NET.Engines.Plug;

/// <summary>
/// Bitmap sampler (0x0907E000)
/// </summary>
[Node(0x0907E000)]
public class CPlugBitmapSampler : CPlug
{
    protected CPlugBitmapSampler()
    {

    }

    [Chunk(0x0907E005)]
    public class Chunk0907E005 : Chunk<CPlugBitmapSampler>
    {
        public string? U01;
        public CMwNod? U02;
        public int? U03;
        public float? U04;

        public override void ReadWrite(CPlugBitmapSampler n, GameBoxReaderWriter rw)
        {
            rw.Id(ref U01);
            rw.NodeRef(ref U02);
            rw.Int32(ref U03);
            rw.Single(ref U04);
        }
    }

    [Chunk(0x0907E006)]
    public class Chunk0907E006 : Chunk<CPlugBitmapSampler>
    {
        public string? U01;
        public CMwNod? U02;
        public int? U03;
        public float? U04;

        public override void ReadWrite(CPlugBitmapSampler n, GameBoxReaderWriter rw)
        {
            rw.Id(ref U01);
            rw.NodeRef(ref U02);
            rw.Int32(ref U03);
            rw.Single(ref U04);
        }
    }
}