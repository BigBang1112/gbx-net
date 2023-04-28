namespace GBX.NET.Engines.Game;

[Node(0x0303B000)]
public class CGameCtnDecorationSize : CMwNod
{
    private Vec2 editionZoneMin;
    private Vec2 editionZoneMax;
    private int baseHeightBase;
    private Int3 size;
    private CSceneLayout? scene;
    private GameBoxRefTable.File? sceneFile;
    private bool offsetBlockY;
    private int baseHeightOffset;

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk<Chunk0303B000>]
    public Vec2 EditionZoneMin { get => editionZoneMin; set => editionZoneMin = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk<Chunk0303B000>]
    public Vec2 EditionZoneMax { get => editionZoneMax; set => editionZoneMax = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk<Chunk0303B001>]
    [AppliedWithChunk<Chunk0303B002>]
    public int BaseHeightBase { get => baseHeightBase; set => baseHeightBase = value; }

    [NodeMember]
    [AppliedWithChunk<Chunk0303B001>]
    [AppliedWithChunk<Chunk0303B002>]
    public Int3 Size { get => size; set => size = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk<Chunk0303B001>]
    [AppliedWithChunk<Chunk0303B002>]
    public CSceneLayout? Scene
    {
        get => scene = GetNodeFromRefTable(scene, sceneFile) as CSceneLayout;
        set => scene = value;
    }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk<Chunk0303B002>]
    public bool OffsetBlockY { get => offsetBlockY; set => offsetBlockY = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk<Chunk0303B003>]
    public int BaseHeightOffset
    {
        get
        {
            DiscoverChunk<Chunk0303B003>();
            return baseHeightOffset;
        }
        set
        {
            DiscoverChunk<Chunk0303B003>();
            baseHeightOffset = value;
        }
    }

    internal CGameCtnDecorationSize()
    {

    }

    #region 0x000 chunk

    /// <summary>
    /// CGameCtnDecorationSize 0x000 chunk
    /// </summary>
    [Chunk(0x0303B000)]
    public class Chunk0303B000 : Chunk<CGameCtnDecorationSize>
    {
        public float U01;

        public override void ReadWrite(CGameCtnDecorationSize n, GameBoxReaderWriter rw)
        {
            rw.Vec2(ref n.editionZoneMin);
            rw.Vec2(ref n.editionZoneMax);
            rw.Single(ref U01);
        }
    }

    #endregion

    #region 0x001 chunk

    /// <summary>
    /// CGameCtnDecorationSize 0x001 chunk
    /// </summary>
    [Chunk(0x0303B001)]
    public class Chunk0303B001 : Chunk<CGameCtnDecorationSize>
    {
        public override void ReadWrite(CGameCtnDecorationSize n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref n.baseHeightBase);
            rw.Int3(ref n.size);
            rw.NodeRef<CSceneLayout>(ref n.scene, ref n.sceneFile);
        }
    }

    #endregion

    #region 0x002 chunk

    /// <summary>
    /// CGameCtnDecorationSize 0x002 chunk
    /// </summary>
    [Chunk(0x0303B002)]
    public class Chunk0303B002 : Chunk<CGameCtnDecorationSize>
    {
        public override void ReadWrite(CGameCtnDecorationSize n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref n.baseHeightBase);
            rw.Int3(ref n.size);
            rw.Boolean(ref n.offsetBlockY);
            rw.NodeRef<CSceneLayout>(ref n.scene, ref n.sceneFile);
        }
    }

    #endregion

    #region 0x003 skippable chunk

    /// <summary>
    /// CGameCtnDecorationSize 0x003 skippable chunk
    /// </summary>
    [Chunk(0x0303B003)]
    public class Chunk0303B003 : SkippableChunk<CGameCtnDecorationSize>, IVersionable
    {
        public int Version { get; set; }

        public override void ReadWrite(CGameCtnDecorationSize n, GameBoxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.Int32(ref n.baseHeightOffset);
        }
    }

    #endregion
}
