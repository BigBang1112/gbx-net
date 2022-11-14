namespace GBX.NET.Engines.TrackMania;

/// <remarks>ID: 0x24092000</remarks>
[Node(0x24092000)]
public class CCtnMediaBlockUiTMSimpleEvtsDisplay : CGameCtnMediaBlockUi
{
    #region Enums

    public enum EDisplayMode
    {
        OnlyTarget,
        Always,
        Never
    }

    #endregion

    #region Fields

    private EDisplayMode displayMode;
    private bool stuntFigures;
    private bool checkpoints;
    private bool endOfRace;
    private bool endOfLaps;
    private bool ghostsName;

    #endregion

    #region Properties

    [NodeMember]
    [AppliedWithChunk<Chunk24092000>]
    [AppliedWithChunk<Chunk24092002>]
    public EDisplayMode DisplayMode { get => displayMode; set => displayMode = value; }

    [NodeMember]
    [AppliedWithChunk<Chunk24092001>]
    public bool StuntFigures { get => stuntFigures; set => stuntFigures = value; }

    [NodeMember]
    [AppliedWithChunk<Chunk24092001>]
    public bool Checkpoints { get => checkpoints; set => checkpoints = value; }

    [NodeMember]
    [AppliedWithChunk<Chunk24092001>]
    public bool EndOfRace { get => endOfRace; set => endOfRace = value; }

    [NodeMember]
    [AppliedWithChunk<Chunk24092001>]
    public bool EndOfLaps { get => endOfLaps; set => endOfLaps = value; }

    [NodeMember]
    [AppliedWithChunk<Chunk24092001>]
    public bool GhostsName { get => ghostsName; set => ghostsName = value; }

    #endregion

    #region Constructors

    internal CCtnMediaBlockUiTMSimpleEvtsDisplay()
    {

    }

    #endregion

    #region Chunks

    #region 0x000 chunk

    /// <summary>
    /// CCtnMediaBlockUiTMSimpleEvtsDisplay 0x000 chunk
    /// </summary>
    [Chunk(0x24092000)]
    public class Chunk24092000 : Chunk<CCtnMediaBlockUiTMSimpleEvtsDisplay>
    {
        public override void Read(CCtnMediaBlockUiTMSimpleEvtsDisplay n, GameBoxReader r)
        {
            n.displayMode = (EDisplayMode)r.ReadInt32();
        }

        public override void Write(CCtnMediaBlockUiTMSimpleEvtsDisplay n, GameBoxWriter w)
        {
            w.Write(n.displayMode == EDisplayMode.Always || n.displayMode == EDisplayMode.Never);
        }
    }

    #endregion

    #region 0x001 chunk

    /// <summary>
    /// CCtnMediaBlockUiTMSimpleEvtsDisplay 0x001 chunk
    /// </summary>
    [Chunk(0x24092001)]
    public class Chunk24092001 : Chunk<CCtnMediaBlockUiTMSimpleEvtsDisplay>
    {
        public override void ReadWrite(CCtnMediaBlockUiTMSimpleEvtsDisplay n, GameBoxReaderWriter rw)
        {
            rw.Boolean(ref n.stuntFigures);
            rw.Boolean(ref n.checkpoints);
            rw.Boolean(ref n.endOfRace);
            rw.Boolean(ref n.endOfLaps);
            rw.Boolean(ref n.ghostsName);
        }
    }

    #endregion

    #region 0x002 chunk

    /// <summary>
    /// CCtnMediaBlockUiTMSimpleEvtsDisplay 0x002 chunk
    /// </summary>
    [Chunk(0x24092002)]
    public class Chunk24092002 : Chunk<CCtnMediaBlockUiTMSimpleEvtsDisplay>
    {
        public override void Read(CCtnMediaBlockUiTMSimpleEvtsDisplay n, GameBoxReader r)
        {
            var displayMode = r.ReadBoolean();
            if (displayMode) n.displayMode = EDisplayMode.Never;
        }

        public override void Write(CCtnMediaBlockUiTMSimpleEvtsDisplay n, GameBoxWriter w)
        {
            w.Write(n.displayMode == EDisplayMode.Never);
        }
    }

    #endregion

    #endregion
}
