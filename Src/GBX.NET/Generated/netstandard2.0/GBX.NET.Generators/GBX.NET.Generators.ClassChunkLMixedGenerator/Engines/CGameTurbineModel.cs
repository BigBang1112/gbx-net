namespace GBX.NET.Engines.GameData;

/// <remarks>ID: 0x2E010000</remarks>
[Class(0x2E010000)]
public partial class CGameTurbineModel : CMwNod, IClass
{
    [Hexadecimal] public static new uint Id => 0x2E010000;




    private Iso4 loc;
    [AppliedWithChunk<Chunk2E010000>]
    public Iso4 Loc { get => loc; set => loc = value; }

    private float silencerBubble_RadiusMute;
    [AppliedWithChunk<Chunk2E010000>]
    public float SilencerBubble_RadiusMute { get => silencerBubble_RadiusMute; set => silencerBubble_RadiusMute = value; }

    private float silencerBubble_RadiusFade;
    [AppliedWithChunk<Chunk2E010000>]
    public float SilencerBubble_RadiusFade { get => silencerBubble_RadiusFade; set => silencerBubble_RadiusFade = value; }

    private bool silencerBubble_OnlyPlayerSounds;
    [AppliedWithChunk<Chunk2E010000>]
    public bool SilencerBubble_OnlyPlayerSounds { get => silencerBubble_OnlyPlayerSounds; set => silencerBubble_OnlyPlayerSounds = value; }

    /// <summary>
    /// Creates a new instance of <see cref="CGameTurbineModel"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public CGameTurbineModel() { }


    /// <summary>
    /// CGameTurbineModel 0x000 chunk
    /// </summary>
    [Chunk(0x2E010000)]
    public partial class Chunk2E010000 : Chunk<CGameTurbineModel>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x2E010000;

        public int Version { get; set; }


        public override void ReadWrite(CGameTurbineModel n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.Iso4(ref n.loc);
            rw.Single(ref n.silencerBubble_RadiusMute);
            rw.Single(ref n.silencerBubble_RadiusFade);
            rw.Boolean(ref n.silencerBubble_OnlyPlayerSounds);
        }
    }




    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x2E010000 => new Chunk2E010000(),
        _ => base.NewChunk(chunkId),
    };
}
