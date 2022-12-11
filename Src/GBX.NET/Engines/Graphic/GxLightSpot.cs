namespace GBX.NET.Engines.Graphic;

/// <remarks>ID: 0x0400B000</remarks>
[Node(0x0400B000)]
public class GxLightSpot : GxLightBall
{
    private float angleInner;
    private float angleOuter;

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk<Chunk0400B001>]
    public float AngleInner { get => angleInner; set => angleInner = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk<Chunk0400B001>]
    public float AngleOuter { get => angleOuter; set => angleOuter = value; }

    internal GxLightSpot()
    {

    }
    
    #region 0x001 chunk

    /// <summary>
    /// GxLightSpot 0x001 chunk
    /// </summary>
    [Chunk(0x0400B001)]
    public class Chunk0400B001 : Chunk<GxLightSpot>
    {
        public float U01;
        public float U02;

        public override void ReadWrite(GxLightSpot n, GameBoxReaderWriter rw)
        {
            rw.Single(ref n.angleInner);
            rw.Single(ref n.angleOuter);
            rw.Single(ref U01);
            rw.Single(ref U02);
        }
    }

    #endregion

    #region 0x002 chunk

    /// <summary>
    /// GxLightSpot 0x002 chunk
    /// </summary>
    [Chunk(0x0400B002)]
    public class Chunk0400B002 : Chunk<GxLightSpot>
    {
        public uint U01;
        public float U02;
        public float U03;
        public float U04;
        public float U05;

        public override void ReadWrite(GxLightSpot n, GameBoxReaderWriter rw)
        {
            rw.UInt32(ref U01);
            rw.Single(ref n.angleInner);
            rw.Single(ref n.angleOuter);
            rw.Single(ref U02); // same as U01 in 0x001
            rw.Single(ref U03);
            rw.Single(ref U04);
            rw.Single(ref U05); // may relate to U02 in 0x001
        }
    }

    #endregion
}