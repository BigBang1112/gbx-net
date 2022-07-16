namespace GBX.NET.Engines.Function;

/// <remarks>ID: 0x0501E000</remarks>
[Node(0x0501E000)]
[NodeExtension("FuncTree")]
public class CFuncTreeRotate : CFuncTree
{
    #region Fields

    private float angleMin;
    private float angleMax;

    #endregion

    #region Properties

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk(typeof(Chunk0501E001))]
    public float AngleMin { get => angleMin; set => angleMin = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk(typeof(Chunk0501E001))]
    public float AngleMax { get => angleMax; set => angleMax = value; }

    #endregion

    #region Constuctors

    protected CFuncTreeRotate()
    {

    }

    #endregion

    #region Chunks

    #region 0x001 chunk

    /// <summary>
    /// CFuncTreeRotate 0x001 chunk
    /// </summary>
    [Chunk(0x0501E001)]
    public class Chunk0501E001 : Chunk<CFuncTreeRotate>
    {
        public float U01;
        public float U02;
        public float U03;

        public override void ReadWrite(CFuncTreeRotate n, GameBoxReaderWriter rw)
        {
            rw.Single(ref U01);
            rw.Single(ref U02);
            rw.Single(ref U03);
            rw.Single(ref n.angleMin);
            rw.Single(ref n.angleMax);
        }
    }

    #endregion

    #endregion
}
