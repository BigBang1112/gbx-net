namespace GBX.NET.Engines.Function;

/// <remarks>ID: 0x0501A000</remarks>
[Node(0x0501A000)]
public class CFuncKeysReal : CFuncKeys
{
    public enum ERealInterp
    {
        None,
        Linear,
        Hermite,
        SmoothStep
    }

    private float[] ys = Array.Empty<float>();
    private ERealInterp realInterp = ERealInterp.Linear;
    private bool forceTangentMinX;
    private bool forceTangentMaxX;
    private float forcedTangentMinX;
    private float forcedTangentMaxX;

    [NodeMember]
    [AppliedWithChunk<Chunk0501A001>]
    [AppliedWithChunk<Chunk0501A002>]
    public float[] Ys { get => ys; set => ys = value; }

    [NodeMember]
    [AppliedWithChunk<Chunk0501A001>]
    [AppliedWithChunk<Chunk0501A002>]
    public ERealInterp RealInterp { get => realInterp; set => realInterp = value; }

    [NodeMember]
    [AppliedWithChunk<Chunk0501A002>]
    public bool ForceTangentMinX { get => forceTangentMinX; set => forceTangentMinX = value; }

    [NodeMember]
    [AppliedWithChunk<Chunk0501A002>]
    public bool ForceTangentMaxX { get => forceTangentMaxX; set => forceTangentMaxX = value; }

    [NodeMember]
    [AppliedWithChunk<Chunk0501A002>]
    public float ForcedTangentMinX { get => forcedTangentMinX; set => forcedTangentMinX = value; }

    [NodeMember]
    [AppliedWithChunk<Chunk0501A002>]
    public float ForcedTangentMaxX { get => forcedTangentMaxX; set => forcedTangentMaxX = value; }

    internal CFuncKeysReal()
    {

    }

    #region 0x001 chunk

    /// <summary>
    /// CFuncKeysReal 0x001 chunk
    /// </summary>
    [Chunk(0x0501A001)]
    public class Chunk0501A001 : Chunk<CFuncKeysReal>
    {
        public override void ReadWrite(CFuncKeysReal n, GameBoxReaderWriter rw)
        {
            rw.Array<float>(ref n.ys!);
            rw.EnumInt32<ERealInterp>(ref n.realInterp);
        }
    }

    #endregion

    #region 0x002 chunk

    /// <summary>
    /// CFuncKeysReal 0x002 chunk
    /// </summary>
    [Chunk(0x0501A002)]
    public class Chunk0501A002 : Chunk0501A001
    {
        public override void ReadWrite(CFuncKeysReal n, GameBoxReaderWriter rw)
        {
            base.ReadWrite(n, rw);

            rw.Boolean(ref n.forceTangentMinX);
            rw.Boolean(ref n.forceTangentMaxX);
            rw.Single(ref n.forcedTangentMinX);
            rw.Single(ref n.forcedTangentMaxX);
        }
    }

    #endregion
}
