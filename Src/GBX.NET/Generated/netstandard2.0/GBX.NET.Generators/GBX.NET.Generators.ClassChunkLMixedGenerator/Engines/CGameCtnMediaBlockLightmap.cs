namespace GBX.NET.Engines.Game;

/// <remarks>ID: 0x030C0000</remarks>
[Class(0x030C0000)]
public partial class CGameCtnMediaBlockLightmap : CGameCtnMediaBlock, IClass, CGameCtnMediaBlock.IHasTwoKeys
{
    [Hexadecimal] public static new uint Id => 0x030C0000;

    TimeSingle IHasTwoKeys.Start { get => Start; set => Start = value; }
    TimeSingle IHasTwoKeys.End { get => End; set => End = value; }



    private TimeSingle start;
    [AppliedWithChunk<Chunk030C0000>]
    public TimeSingle Start { get => start; set => start = value; }

    private TimeSingle end;
    [AppliedWithChunk<Chunk030C0000>]
    public TimeSingle End { get => end; set => end = value; }

    /// <summary>
    /// Creates a new instance of <see cref="CGameCtnMediaBlockLightmap"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public CGameCtnMediaBlockLightmap() { }


    /// <summary>
    /// CGameCtnMediaBlockLightmap 0x000 chunk
    /// </summary>
    [Chunk(0x030C0000)]
    public partial class Chunk030C0000 : Chunk<CGameCtnMediaBlockLightmap>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x030C0000;

        public int Version { get; set; }

        public float U01;

        public override void ReadWrite(CGameCtnMediaBlockLightmap n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.TimeSingle(ref n.start);
            rw.TimeSingle(ref n.end);
            if (Version >= 1)
            {
                rw.Single(ref U01);
            }
        }
    }




    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x030C0000 => new Chunk030C0000(),
        _ => base.NewChunk(chunkId),
    };
}
