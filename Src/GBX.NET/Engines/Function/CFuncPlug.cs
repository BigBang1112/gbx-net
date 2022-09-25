namespace GBX.NET.Engines.Function;

/// <remarks>ID: 0x0500B000</remarks>
[Node(0x0500B000)]
public abstract class CFuncPlug : CFunc
{
    #region Fields

    private float period;
    private float phase;
    private bool autoCreateMotion;
    private bool randomizePhase;
    private string? inputValId;

    #endregion

    #region Properties

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk(typeof(Chunk0500B003))]
    [AppliedWithChunk(typeof(Chunk0500B004))]
    [AppliedWithChunk(typeof(Chunk0500B005))]
    public float Period { get => period; set => period = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk(typeof(Chunk0500B003))]
    [AppliedWithChunk(typeof(Chunk0500B004))]
    [AppliedWithChunk(typeof(Chunk0500B005))]
    public float Phase { get => phase; set => phase = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk(typeof(Chunk0500B003))]
    [AppliedWithChunk(typeof(Chunk0500B004))]
    [AppliedWithChunk(typeof(Chunk0500B005))]
    public bool AutoCreateMotion { get => autoCreateMotion; set => autoCreateMotion = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk(typeof(Chunk0500B004))]
    [AppliedWithChunk(typeof(Chunk0500B005))]
    public bool RandomizePhase { get => randomizePhase; set => randomizePhase = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk(typeof(Chunk0500B005))]
    public string? InputValId { get => inputValId; set => inputValId = value; }

    #endregion

    #region Constructors

    internal CFuncPlug()
    {

    }

    #endregion

    #region Chunks

    #region 0x003 chunk

    /// <summary>
    /// CFuncPlug 0x003 chunk
    /// </summary>
    [Chunk(0x0500B003)]
    public class Chunk0500B003 : Chunk<CFuncPlug>
    {
        public override void ReadWrite(CFuncPlug n, GameBoxReaderWriter rw)
        {
            rw.Single(ref n.period);
            rw.Single(ref n.phase);
            rw.Boolean(ref n.autoCreateMotion);
        }
    }

    #endregion

    #region 0x004 chunk

    /// <summary>
    /// CFuncPlug 0x004 chunk
    /// </summary>
    [Chunk(0x0500B004)]
    public class Chunk0500B004 : Chunk<CFuncPlug>
    {
        public override void ReadWrite(CFuncPlug n, GameBoxReaderWriter rw)
        {
            rw.Single(ref n.period);
            rw.Single(ref n.phase);
            rw.Boolean(ref n.autoCreateMotion);
            rw.Boolean(ref n.randomizePhase);
        }
    }

    #endregion

    #region 0x005 chunk

    /// <summary>
    /// CFuncPlug 0x005 chunk
    /// </summary>
    [Chunk(0x0500B005)]
    public class Chunk0500B005 : Chunk<CFuncPlug>
    {
        public override void ReadWrite(CFuncPlug n, GameBoxReaderWriter rw)
        {
            rw.Single(ref n.period);
            rw.Single(ref n.phase);
            rw.Boolean(ref n.autoCreateMotion);
            rw.Boolean(ref n.randomizePhase);
            rw.Id(ref n.inputValId);
        }
    }

    #endregion

    #endregion
}
