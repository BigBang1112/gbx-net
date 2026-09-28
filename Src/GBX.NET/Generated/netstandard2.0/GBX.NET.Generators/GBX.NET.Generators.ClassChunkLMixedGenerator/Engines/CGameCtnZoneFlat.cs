namespace GBX.NET.Engines.Game;

/// <remarks>ID: 0x0305D000</remarks>
[Class(0x0305D000)]
public partial class CGameCtnZoneFlat : CGameCtnZone, IClass
{
    [Hexadecimal] public static new uint Id => 0x0305D000;




    private CGameCtnBlockInfoFlat? blockInfoFlat;
    [AppliedWithChunk<Chunk0305D001>]
    public CGameCtnBlockInfoFlat? BlockInfoFlat { get => blockInfoFlatFile?.GetNode(ref blockInfoFlat) ?? blockInfoFlat; set => blockInfoFlat = value; }
    private Components.GbxRefTableFile? blockInfoFlatFile;
    public Components.GbxRefTableFile? BlockInfoFlatFile { get => blockInfoFlatFile; set => blockInfoFlatFile = value; }
    public CGameCtnBlockInfoFlat? GetBlockInfoFlat(GbxReadSettings settings = default, bool exceptions = false) => blockInfoFlatFile?.GetNode(ref blockInfoFlat, settings, exceptions) ?? blockInfoFlat;

    private CGameCtnBlockInfoClip? blockInfoClip;
    [AppliedWithChunk<Chunk0305D001>]
    public CGameCtnBlockInfoClip? BlockInfoClip { get => blockInfoClipFile?.GetNode(ref blockInfoClip) ?? blockInfoClip; set => blockInfoClip = value; }
    private Components.GbxRefTableFile? blockInfoClipFile;
    public Components.GbxRefTableFile? BlockInfoClipFile { get => blockInfoClipFile; set => blockInfoClipFile = value; }
    public CGameCtnBlockInfoClip? GetBlockInfoClip(GbxReadSettings settings = default, bool exceptions = false) => blockInfoClipFile?.GetNode(ref blockInfoClip, settings, exceptions) ?? blockInfoClip;

    private CGameCtnBlockInfoRoad? blockInfoRoad;
    [AppliedWithChunk<Chunk0305D001>]
    public CGameCtnBlockInfoRoad? BlockInfoRoad { get => blockInfoRoadFile?.GetNode(ref blockInfoRoad) ?? blockInfoRoad; set => blockInfoRoad = value; }
    private Components.GbxRefTableFile? blockInfoRoadFile;
    public Components.GbxRefTableFile? BlockInfoRoadFile { get => blockInfoRoadFile; set => blockInfoRoadFile = value; }
    public CGameCtnBlockInfoRoad? GetBlockInfoRoad(GbxReadSettings settings = default, bool exceptions = false) => blockInfoRoadFile?.GetNode(ref blockInfoRoad, settings, exceptions) ?? blockInfoRoad;

    private CGameCtnBlockInfoPylon? blockInfoPylon;
    [AppliedWithChunk<Chunk0305D001>]
    public CGameCtnBlockInfoPylon? BlockInfoPylon { get => blockInfoPylonFile?.GetNode(ref blockInfoPylon) ?? blockInfoPylon; set => blockInfoPylon = value; }
    private Components.GbxRefTableFile? blockInfoPylonFile;
    public Components.GbxRefTableFile? BlockInfoPylonFile { get => blockInfoPylonFile; set => blockInfoPylonFile = value; }
    public CGameCtnBlockInfoPylon? GetBlockInfoPylon(GbxReadSettings settings = default, bool exceptions = false) => blockInfoPylonFile?.GetNode(ref blockInfoPylon, settings, exceptions) ?? blockInfoPylon;

    private bool groundOnly;
    [AppliedWithChunk<Chunk0305D002>]
    public bool GroundOnly { get => groundOnly; set => groundOnly = value; }

    private bool autoSimplifyGenealogy;
    [AppliedWithChunk<Chunk0305D003>]
    public bool AutoSimplifyGenealogy { get => autoSimplifyGenealogy; set => autoSimplifyGenealogy = value; }

    /// <summary>
    /// Creates a new instance of <see cref="CGameCtnZoneFlat"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public CGameCtnZoneFlat() { }


    /// <summary>
    /// CGameCtnZoneFlat 0x001 chunk
    /// </summary>
    [Chunk(0x0305D001)]
    public partial class Chunk0305D001 : Chunk<CGameCtnZoneFlat>
    {
        /// <inheritdoc />
        public override uint Id => 0x0305D001;


        public override void ReadWrite(CGameCtnZoneFlat n, GbxReaderWriter rw)
        {
            rw.NodeRef<CGameCtnBlockInfoFlat>(ref n.blockInfoFlat, ref n.blockInfoFlatFile);
            rw.NodeRef<CGameCtnBlockInfoClip>(ref n.blockInfoClip, ref n.blockInfoClipFile);
            rw.NodeRef<CGameCtnBlockInfoRoad>(ref n.blockInfoRoad, ref n.blockInfoRoadFile);
            rw.NodeRef<CGameCtnBlockInfoPylon>(ref n.blockInfoPylon, ref n.blockInfoPylonFile);
        }
    }

    /// <summary>
    /// CGameCtnZoneFlat 0x002 skippable chunk
    /// </summary>
    [Chunk(0x0305D002)]
    public partial class Chunk0305D002 : SkippableChunk<CGameCtnZoneFlat>
    {
        /// <inheritdoc />
        public override uint Id => 0x0305D002;

        public int U01;
        public bool U02;

        public override void ReadWrite(CGameCtnZoneFlat n, GbxReaderWriter rw)
        {
            rw.Int32(ref U01);
            rw.Boolean(ref n.groundOnly);
            rw.Boolean(ref U02);
        }
    }

    /// <summary>
    /// CGameCtnZoneFlat 0x003 skippable chunk
    /// </summary>
    [Chunk(0x0305D003)]
    public partial class Chunk0305D003 : SkippableChunk<CGameCtnZoneFlat>
    {
        /// <inheritdoc />
        public override uint Id => 0x0305D003;


        public override void ReadWrite(CGameCtnZoneFlat n, GbxReaderWriter rw)
        {
            rw.Boolean(ref n.autoSimplifyGenealogy);
        }
    }




    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x0305D001 => new Chunk0305D001(),
        0x0305D002 => new Chunk0305D002(),
        0x0305D003 => new Chunk0305D003(),
        _ => base.NewChunk(chunkId),
    };
}
