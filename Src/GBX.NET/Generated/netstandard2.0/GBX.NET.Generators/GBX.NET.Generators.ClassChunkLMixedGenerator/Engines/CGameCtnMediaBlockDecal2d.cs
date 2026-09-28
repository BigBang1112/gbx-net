namespace GBX.NET.Engines.Game;

/// <remarks>ID: 0x031AA000</remarks>
[Class(0x031AA000)]
public partial class CGameCtnMediaBlockDecal2d : CGameCtnMediaBlock, IClass, CGameCtnMediaBlock.IHasTwoKeys
{
    [Hexadecimal] public static new uint Id => 0x031AA000;

    TimeSingle IHasTwoKeys.Start { get => Start; set => Start = value; }
    TimeSingle IHasTwoKeys.End { get => End; set => End = value; }



    private TimeSingle start;
    [AppliedWithChunk<Chunk031AA000>]
    public TimeSingle Start { get => start; set => start = value; }

    private TimeSingle end;
    [AppliedWithChunk<Chunk031AA000>]
    public TimeSingle End { get => end; set => end = value; }

    private PackDesc[]? images;
    [AppliedWithChunk<Chunk031AA000>]
    public PackDesc[]? Images { get => images; set => images = value; }

    private List<Decal>? decals;
    [AppliedWithChunk<Chunk031AA000>]
    public List<Decal>? Decals { get => decals; set => decals = value; }

    /// <summary>
    /// Creates a new instance of <see cref="CGameCtnMediaBlockDecal2d"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public CGameCtnMediaBlockDecal2d() { }


    /// <summary>
    /// CGameCtnMediaBlockDecal2d 0x000 chunk
    /// </summary>
    [Chunk(0x031AA000)]
    public partial class Chunk031AA000 : Chunk<CGameCtnMediaBlockDecal2d>
    {
        /// <inheritdoc />
        public override uint Id => 0x031AA000;


        public override void ReadWrite(CGameCtnMediaBlockDecal2d n, GbxReaderWriter rw)
        {
            rw.TimeSingle(ref n.start);
            rw.TimeSingle(ref n.end);
            rw.ArrayPackDesc(ref n.images!);
            rw.ListReadableWritable<Decal>(ref n.decals!);
        }
    }


    public sealed partial class Decal : IReadableWritable
    {

        private Iso4 u01;
        public Iso4 U01 { get => u01; set => u01 = value; }

        private Vec3 scale;
        public Vec3 Scale { get => scale; set => scale = value; }

        private float u02;
        public float U02 { get => u02; set => u02 = value; }

        private bool u03;
        public bool U03 { get => u03; set => u03 = value; }

        private int u04;
        public int U04 { get => u04; set => u04 = value; }

        public void ReadWrite(GbxReaderWriter rw, int v = 0)
        {
            rw.Iso4(ref u01);
            rw.Vec3(ref scale);
            rw.Single(ref u02);
            rw.Boolean(ref u03);
            rw.Int32(ref u04);
        }
    }



    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x031AA000 => new Chunk031AA000(),
        _ => base.NewChunk(chunkId),
    };
}
