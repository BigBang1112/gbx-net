namespace GBX.NET.Engines.Plug;

/// <remarks>ID: 0x09182000</remarks>
[Node(0x09182000)]
[NodeExtension("FuncCloudsParam")]
public class CPlugCloudsParam : CMwNod
{
    private Vec2[]? points;

    [NodeMember]
    [AppliedWithChunk(typeof(Chunk09182001))]
    public Vec2[]? Points { get => points; set => points = value; }

    internal CPlugCloudsParam()
    {

    }

    #region 0x001 chunk

    /// <summary>
    /// CPlugCloudsParam 0x001 chunk
    /// </summary>
    [Chunk(0x09182001)]
    public class Chunk09182001 : Chunk<CPlugCloudsParam>
    {
        public override void ReadWrite(CPlugCloudsParam n, GameBoxReaderWriter rw)
        {
            rw.Array<Vec2>(ref n.points);
        }
    }

    #endregion

    #region 0x002 chunk

    /// <summary>
    /// CPlugCloudsParam 0x002 chunk
    /// </summary>
    [Chunk(0x09182002)]
    public class Chunk09182002 : Chunk<CPlugCloudsParam>
    {
        public int U01;
        public int U02;
        public float U03;
        public float U04;
        public float U05;
        public float U06;
        public float U07;
        public int U08;
        public int U09;

        public override void ReadWrite(CPlugCloudsParam n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref U01);
            rw.Int32(ref U02);
            rw.Single(ref U03);
            rw.Single(ref U04);
            rw.Single(ref U05);
            rw.Single(ref U06);
            rw.Single(ref U07);
            rw.Int32(ref U08);
            rw.Int32(ref U09);
        }
    }

    #endregion
}
