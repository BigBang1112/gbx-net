namespace GBX.NET.Engines.Game;

/// <remarks>ID: 0x03196000</remarks>
[Class(0x03196000)]
public partial class CGameCtnMediaBlockObject : CGameCtnMediaBlock, IClass, CGameCtnMediaBlock.IHasTwoKeys
{
    [Hexadecimal] public static new uint Id => 0x03196000;

    TimeSingle IHasTwoKeys.Start { get => Start; set => Start = value; }
    TimeSingle IHasTwoKeys.End { get => End; set => End = value; }



    private TimeSingle start;
    [AppliedWithChunk<Chunk03196000>]
    public TimeSingle Start { get => start; set => start = value; }

    private TimeSingle end;
    [AppliedWithChunk<Chunk03196000>]
    public TimeSingle End { get => end; set => end = value; }

    private AnchoredObjectInfo[]? anchoredObjectInfos;
    [AppliedWithChunk<Chunk03196000>]
    public AnchoredObjectInfo[]? AnchoredObjectInfos { get => anchoredObjectInfos; set => anchoredObjectInfos = value; }

    private CGameReplayObjectVisData? visData;
    [AppliedWithChunk<Chunk03196000>]
    public CGameReplayObjectVisData? VisData { get => visData; set => visData = value; }

    /// <summary>
    /// Creates a new instance of <see cref="CGameCtnMediaBlockObject"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public CGameCtnMediaBlockObject() { }


    /// <summary>
    /// CGameCtnMediaBlockObject 0x000 chunk
    /// </summary>
    [Chunk(0x03196000)]
    public partial class Chunk03196000 : Chunk<CGameCtnMediaBlockObject>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x03196000;

        public int Version { get; set; } = 1;


        public override void ReadWrite(CGameCtnMediaBlockObject n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.TimeSingle(ref n.start);
            rw.TimeSingle(ref n.end);
            if (Version == 0)
            {
                rw.ArrayReadableWritable<AnchoredObjectInfo>(ref n.anchoredObjectInfos!);
            }
            if (Version >= 1)
            {
                rw.NodeRef<CGameReplayObjectVisData>(ref n.visData);
            }
        }
    }


    public sealed partial class AnchoredObjectInfo : IReadableWritable
    {

        private bool u01;
        public bool U01 { get => u01; set => u01 = value; }

        private int u02;
        public int U02 { get => u02; set => u02 = value; }

        private float u03;
        public float U03 { get => u03; set => u03 = value; }

        private float u04;
        public float U04 { get => u04; set => u04 = value; }

        private float u05;
        public float U05 { get => u05; set => u05 = value; }

        private int u06;
        public int U06 { get => u06; set => u06 = value; }

        public void ReadWrite(GbxReaderWriter rw, int v = 0)
        {
            rw.Boolean(ref u01);
            rw.Int32(ref u02);
            rw.Single(ref u03);
            rw.Single(ref u04);
            rw.Single(ref u05);
            rw.Int32(ref u06);
        }
    }



    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x03196000 => new Chunk03196000(),
        _ => base.NewChunk(chunkId),
    };
}
