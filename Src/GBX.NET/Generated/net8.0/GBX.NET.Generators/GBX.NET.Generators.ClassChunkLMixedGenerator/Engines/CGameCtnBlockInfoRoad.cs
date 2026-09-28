namespace GBX.NET.Engines.Game;

/// <remarks>ID: 0x03052000</remarks>
[Class(0x03052000)]
public partial class CGameCtnBlockInfoRoad : CGameCtnBlockInfo, IClass
{
    [Hexadecimal] public static new uint Id => 0x03052000;




    private CGameCtnBlockInfoSlope? slope;
    [AppliedWithChunk<Chunk03052000>]
    public CGameCtnBlockInfoSlope? Slope { get => slopeFile?.GetNode(ref slope) ?? slope; set => slope = value; }
    private Components.GbxRefTableFile? slopeFile;
    public Components.GbxRefTableFile? SlopeFile { get => slopeFile; set => slopeFile = value; }
    public CGameCtnBlockInfoSlope? GetSlope(GbxReadSettings settings = default, bool exceptions = false) => slopeFile?.GetNode(ref slope, settings, exceptions) ?? slope;

    /// <summary>
    /// Creates a new instance of <see cref="CGameCtnBlockInfoRoad"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public CGameCtnBlockInfoRoad() { }


    /// <summary>
    /// CGameCtnBlockInfoRoad 0x000 chunk
    /// </summary>
    [Chunk(0x03052000)]
    public partial class Chunk03052000 : Chunk<CGameCtnBlockInfoRoad>
    {
        /// <inheritdoc />
        public override uint Id => 0x03052000;


        public override void ReadWrite(CGameCtnBlockInfoRoad n, GbxReaderWriter rw)
        {
            rw.NodeRef<CGameCtnBlockInfoSlope>(ref n.slope, ref n.slopeFile);
        }
    }




    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x03052000 => new Chunk03052000(),
        _ => base.NewChunk(chunkId),
    };
}
