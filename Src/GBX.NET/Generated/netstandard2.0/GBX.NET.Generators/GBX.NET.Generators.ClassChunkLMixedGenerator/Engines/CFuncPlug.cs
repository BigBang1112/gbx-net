namespace GBX.NET.Engines.Function;

/// <remarks>ID: 0x0500B000</remarks>
[Class(0x0500B000)]
public partial class CFuncPlug : CFunc, IClass
{
    [Hexadecimal] public static new uint Id => 0x0500B000;




    private float period;
    [AppliedWithChunk<Chunk0500B003>]
    [AppliedWithChunk<Chunk0500B004>]
    [AppliedWithChunk<Chunk0500B005>]
    public float Period { get => period; set => period = value; }

    private float phase;
    [AppliedWithChunk<Chunk0500B003>]
    [AppliedWithChunk<Chunk0500B004>]
    [AppliedWithChunk<Chunk0500B005>]
    public float Phase { get => phase; set => phase = value; }

    private bool autoCreateMotion;
    [AppliedWithChunk<Chunk0500B003>]
    [AppliedWithChunk<Chunk0500B004>]
    [AppliedWithChunk<Chunk0500B005>]
    public bool AutoCreateMotion { get => autoCreateMotion; set => autoCreateMotion = value; }

    private bool randomizePhase;
    [AppliedWithChunk<Chunk0500B004>]
    [AppliedWithChunk<Chunk0500B005>]
    public bool RandomizePhase { get => randomizePhase; set => randomizePhase = value; }

    private string? inputValId;
    [AppliedWithChunk<Chunk0500B005>]
    public string? InputValId { get => inputValId; set => inputValId = value; }

    /// <summary>
    /// Creates a new instance of <see cref="CFuncPlug"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public CFuncPlug() { }


    /// <summary>
    /// CFuncPlug 0x003 chunk
    /// </summary>
    [Chunk(0x0500B003)]
    [ChunkGameVersion(GameVersion.TM10)]
    public partial class Chunk0500B003 : Chunk<CFuncPlug>
    {
        /// <inheritdoc />
        public override uint Id => 0x0500B003;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.TM10;


        public override void ReadWrite(CFuncPlug n, GbxReaderWriter rw)
        {
            rw.Single(ref n.period);
            rw.Single(ref n.phase);
            rw.Boolean(ref n.autoCreateMotion);
        }
    }

    /// <summary>
    /// CFuncPlug 0x004 chunk
    /// </summary>
    [Chunk(0x0500B004)]
    public partial class Chunk0500B004 : Chunk<CFuncPlug>
    {
        /// <inheritdoc />
        public override uint Id => 0x0500B004;


        public override void ReadWrite(CFuncPlug n, GbxReaderWriter rw)
        {
            rw.Single(ref n.period);
            rw.Single(ref n.phase);
            rw.Boolean(ref n.autoCreateMotion);
            rw.Boolean(ref n.randomizePhase);
        }
    }

    /// <summary>
    /// CFuncPlug 0x005 chunk
    /// </summary>
    [Chunk(0x0500B005)]
    public partial class Chunk0500B005 : Chunk<CFuncPlug>
    {
        /// <inheritdoc />
        public override uint Id => 0x0500B005;


        public override void ReadWrite(CFuncPlug n, GbxReaderWriter rw)
        {
            rw.Single(ref n.period);
            rw.Single(ref n.phase);
            rw.Boolean(ref n.autoCreateMotion);
            rw.Boolean(ref n.randomizePhase);
            rw.Id(ref n.inputValId);
        }
    }




    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x0500B003 => new Chunk0500B003(),
        0x0500B004 => new Chunk0500B004(),
        0x0500B005 => new Chunk0500B005(),
        _ => base.NewChunk(chunkId),
    };
}
