namespace GBX.NET.Engines.Motion;

/// <remarks>ID: 0x08034000</remarks>
[Class(0x08034000)]
public partial class CMotionPlayer : CMotion, IClass
{
    [Hexadecimal] public static new uint Id => 0x08034000;




    private CMotionCmdBase? @base;
    [AppliedWithChunk<Chunk08034004>]
    public CMotionCmdBase? Base { get => @base; set => @base = value; }

    private List<CMotionTrack>? tracks;
    [AppliedWithChunk<Chunk08034004>]
    public List<CMotionTrack>? Tracks { get => tracks; set => tracks = value; }

    /// <summary>
    /// Creates a new instance of <see cref="CMotionPlayer"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public CMotionPlayer() { }


    /// <summary>
    /// CMotionPlayer 0x004 chunk
    /// </summary>
    [Chunk(0x08034004)]
    public partial class Chunk08034004 : Chunk<CMotionPlayer>
    {
        /// <inheritdoc />
        public override uint Id => 0x08034004;

        /// <summary>
        /// SavePlayState?
        /// </summary>
        public int U01;
        /// <summary>
        /// IsPlaying?
        /// </summary>
        public bool U02;
        public string? U03;

        public override void ReadWrite(CMotionPlayer n, GbxReaderWriter rw)
        {
            rw.Node<CMotionCmdBase>(ref n.@base);
            rw.Int32(ref U01); // SavePlayState?
            rw.Boolean(ref U02); // IsPlaying?
            rw.Id(ref U03);
            rw.ListNodeRef<CMotionTrack>(ref n.tracks!);
        }
    }




    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x08034004 => new Chunk08034004(),
        _ => base.NewChunk(chunkId),
    };
}
