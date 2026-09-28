namespace GBX.NET.Engines.Game;

/// <remarks>ID: 0x030E5000</remarks>
[Class(0x030E5000)]
public partial class CGameCtnMediaBlockGhost : CGameCtnMediaBlock, IClass, CGameCtnMediaBlock.IHasTwoKeys, CGameCtnMediaBlock.IHasKeys
{
    [Hexadecimal] public static new uint Id => 0x030E5000;

    TimeSingle IHasTwoKeys.Start { get => Start; set => Start = value; }
    TimeSingle IHasTwoKeys.End { get => End; set => End = value; }
    IEnumerable<IKey> IHasKeys.Keys => Keys ?? [];



    private TimeSingle start;
    [AppliedWithChunk<Chunk030E5001>]
    [AppliedWithChunk<Chunk030E5002>]
    public TimeSingle Start { get => start; set => start = value; }

    private TimeSingle end;
    [AppliedWithChunk<Chunk030E5001>]
    [AppliedWithChunk<Chunk030E5002>]
    public TimeSingle End { get => end; set => end = value; }

    private CGameCtnGhost? ghostModel;
    [AppliedWithChunk<Chunk030E5001>]
    [AppliedWithChunk<Chunk030E5002>]
    public CGameCtnGhost? GhostModel { get => ghostModel; set => ghostModel = value; }

    private float startOffset;
    [AppliedWithChunk<Chunk030E5001>]
    [AppliedWithChunk<Chunk030E5002>]
    public float StartOffset { get => startOffset; set => startOffset = value; }

    private List<Key>? keys;
    [AppliedWithChunk<Chunk030E5002>]
    public List<Key>? Keys { get => keys; set => keys = value; }

    private bool noDamage;
    [AppliedWithChunk<Chunk030E5002>]
    public bool NoDamage { get => noDamage; set => noDamage = value; }

    private bool forceLight;
    [AppliedWithChunk<Chunk030E5002>]
    public bool ForceLight { get => forceLight; set => forceLight = value; }

    private bool forceHue;
    [AppliedWithChunk<Chunk030E5002>]
    public bool ForceHue { get => forceHue; set => forceHue = value; }

    /// <summary>
    /// Creates a new instance of <see cref="CGameCtnMediaBlockGhost"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public CGameCtnMediaBlockGhost() { }


    /// <summary>
    /// CGameCtnMediaBlockGhost 0x001 chunk
    /// </summary>
    [Chunk(0x030E5001)]
    public partial class Chunk030E5001 : Chunk<CGameCtnMediaBlockGhost>
    {
        /// <inheritdoc />
        public override uint Id => 0x030E5001;


        public override void ReadWrite(CGameCtnMediaBlockGhost n, GbxReaderWriter rw)
        {
            rw.TimeSingle(ref n.start);
            rw.TimeSingle(ref n.end);
            rw.NodeRef<CGameCtnGhost>(ref n.ghostModel);
            rw.Single(ref n.startOffset);
        }
    }

    /// <summary>
    /// CGameCtnMediaBlockGhost 0x002 chunk
    /// </summary>
    [Chunk(0x030E5002)]
    public partial class Chunk030E5002 : Chunk<CGameCtnMediaBlockGhost>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x030E5002;

        public int Version { get; set; }


        public override void ReadWrite(CGameCtnMediaBlockGhost n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            if (Version >= 3)
            {
                rw.ListReadableWritable<Key>(ref n.keys!);
            }
            if (Version <= 2)
            {
                rw.TimeSingle(ref n.start);
                rw.TimeSingle(ref n.end);
            }
            rw.NodeRef<CGameCtnGhost>(ref n.ghostModel);
            rw.Single(ref n.startOffset);
            rw.Boolean(ref n.noDamage);
            rw.Boolean(ref n.forceLight);
            rw.Boolean(ref n.forceHue);
        }
    }


    public sealed partial class Key : IKey, IReadableWritable
    {

        private TimeSingle time;
        public TimeSingle Time { get => time; set => time = value; }

        private float lightIntensity;
        public float LightIntensity { get => lightIntensity; set => lightIntensity = value; }

        public void ReadWrite(GbxReaderWriter rw, int v = 0)
        {
            rw.TimeSingle(ref time);
            rw.Single(ref lightIntensity);
        }
    }



    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x030E5001 => new Chunk030E5001(),
        0x030E5002 => new Chunk030E5002(),
        _ => base.NewChunk(chunkId),
    };
}
