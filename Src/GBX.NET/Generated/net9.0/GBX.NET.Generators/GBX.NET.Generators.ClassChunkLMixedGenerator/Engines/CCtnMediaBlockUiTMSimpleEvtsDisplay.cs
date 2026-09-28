namespace GBX.NET.Engines.TrackMania;

/// <remarks>ID: 0x24092000</remarks>
[Class(0x24092000)]
public partial class CCtnMediaBlockUiTMSimpleEvtsDisplay : CGameCtnMediaBlockUi, IClass
{
    [Hexadecimal] public static new uint Id => 0x24092000;




    private bool stuntFigures;
    [AppliedWithChunk<Chunk24092001>]
    public bool StuntFigures { get => stuntFigures; set => stuntFigures = value; }

    private bool checkpoints;
    [AppliedWithChunk<Chunk24092001>]
    public bool Checkpoints { get => checkpoints; set => checkpoints = value; }

    private bool endOfRace;
    [AppliedWithChunk<Chunk24092001>]
    public bool EndOfRace { get => endOfRace; set => endOfRace = value; }

    private bool endOfLaps;
    [AppliedWithChunk<Chunk24092001>]
    public bool EndOfLaps { get => endOfLaps; set => endOfLaps = value; }

    private bool ghostsName;
    [AppliedWithChunk<Chunk24092001>]
    public bool GhostsName { get => ghostsName; set => ghostsName = value; }


    /// <summary>
    /// CCtnMediaBlockUiTMSimpleEvtsDisplay 0x000 chunk
    /// </summary>
    [Chunk(0x24092000)]
    public partial class Chunk24092000 : Chunk<CCtnMediaBlockUiTMSimpleEvtsDisplay>
    {
        /// <inheritdoc />
        public override uint Id => 0x24092000;

    }

    /// <summary>
    /// CCtnMediaBlockUiTMSimpleEvtsDisplay 0x001 chunk
    /// </summary>
    [Chunk(0x24092001)]
    public partial class Chunk24092001 : Chunk<CCtnMediaBlockUiTMSimpleEvtsDisplay>
    {
        /// <inheritdoc />
        public override uint Id => 0x24092001;


        public override void ReadWrite(CCtnMediaBlockUiTMSimpleEvtsDisplay n, GbxReaderWriter rw)
        {
            rw.Boolean(ref n.stuntFigures);
            rw.Boolean(ref n.checkpoints);
            rw.Boolean(ref n.endOfRace);
            rw.Boolean(ref n.endOfLaps);
            rw.Boolean(ref n.ghostsName);
        }
    }

    /// <summary>
    /// CCtnMediaBlockUiTMSimpleEvtsDisplay 0x002 chunk
    /// </summary>
    [Chunk(0x24092002)]
    public partial class Chunk24092002 : Chunk<CCtnMediaBlockUiTMSimpleEvtsDisplay>
    {
        /// <inheritdoc />
        public override uint Id => 0x24092002;

    }



    public enum EDisplayMode
    {
        OnlyTarget,
        Always,
        Never,
    }


    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x24092000 => new Chunk24092000(),
        0x24092001 => new Chunk24092001(),
        0x24092002 => new Chunk24092002(),
        _ => base.NewChunk(chunkId),
    };
}
