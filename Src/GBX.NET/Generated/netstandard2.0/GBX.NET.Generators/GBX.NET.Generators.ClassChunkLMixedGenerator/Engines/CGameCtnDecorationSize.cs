namespace GBX.NET.Engines.Game;

/// <remarks>ID: 0x0303B000</remarks>
[Class(0x0303B000)]
public partial class CGameCtnDecorationSize : CMwNod, IClass
{
    [Hexadecimal] public static new uint Id => 0x0303B000;




    private Vec2 editionZoneMin;
    [AppliedWithChunk<Chunk0303B000>]
    public Vec2 EditionZoneMin { get => editionZoneMin; set => editionZoneMin = value; }

    private Vec2 editionZoneMax;
    [AppliedWithChunk<Chunk0303B000>]
    public Vec2 EditionZoneMax { get => editionZoneMax; set => editionZoneMax = value; }

    private int baseHeightBase;
    [AppliedWithChunk<Chunk0303B001>]
    [AppliedWithChunk<Chunk0303B002>]
    public int BaseHeightBase { get => baseHeightBase; set => baseHeightBase = value; }

    private Int3 size;
    [AppliedWithChunk<Chunk0303B001>]
    [AppliedWithChunk<Chunk0303B002>]
    public Int3 Size { get => size; set => size = value; }

    private CSceneLayout? scene;
    [AppliedWithChunk<Chunk0303B001>]
    [AppliedWithChunk<Chunk0303B002>]
    public CSceneLayout? Scene { get => sceneFile?.GetNode(ref scene) ?? scene; set => scene = value; }
    private Components.GbxRefTableFile? sceneFile;
    public Components.GbxRefTableFile? SceneFile { get => sceneFile; set => sceneFile = value; }
    public CSceneLayout? GetScene(GbxReadSettings settings = default, bool exceptions = false) => sceneFile?.GetNode(ref scene, settings, exceptions) ?? scene;

    private bool offsetBlockY;
    [AppliedWithChunk<Chunk0303B002>]
    public bool OffsetBlockY { get => offsetBlockY; set => offsetBlockY = value; }

    private int baseHeightOffset;
    [AppliedWithChunk<Chunk0303B003>]
    public int BaseHeightOffset { get => baseHeightOffset; set => baseHeightOffset = value; }

    /// <summary>
    /// Creates a new instance of <see cref="CGameCtnDecorationSize"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public CGameCtnDecorationSize() { }


    /// <summary>
    /// CGameCtnDecorationSize 0x000 chunk
    /// </summary>
    [Chunk(0x0303B000)]
    public partial class Chunk0303B000 : Chunk<CGameCtnDecorationSize>
    {
        /// <inheritdoc />
        public override uint Id => 0x0303B000;

        public float U01;

        public override void ReadWrite(CGameCtnDecorationSize n, GbxReaderWriter rw)
        {
            rw.Vec2(ref n.editionZoneMin);
            rw.Vec2(ref n.editionZoneMax);
            rw.Single(ref U01);
        }
    }

    /// <summary>
    /// CGameCtnDecorationSize 0x001 chunk
    /// </summary>
    [Chunk(0x0303B001)]
    public partial class Chunk0303B001 : Chunk<CGameCtnDecorationSize>
    {
        /// <inheritdoc />
        public override uint Id => 0x0303B001;


        public override void ReadWrite(CGameCtnDecorationSize n, GbxReaderWriter rw)
        {
            rw.Int32(ref n.baseHeightBase);
            rw.Int3(ref n.size);
            rw.NodeRef<CSceneLayout>(ref n.scene, ref n.sceneFile);
        }
    }

    /// <summary>
    /// CGameCtnDecorationSize 0x002 chunk
    /// </summary>
    [Chunk(0x0303B002)]
    public partial class Chunk0303B002 : Chunk<CGameCtnDecorationSize>
    {
        /// <inheritdoc />
        public override uint Id => 0x0303B002;


        public override void ReadWrite(CGameCtnDecorationSize n, GbxReaderWriter rw)
        {
            rw.Int32(ref n.baseHeightBase);
            rw.Int3(ref n.size);
            rw.Boolean(ref n.offsetBlockY);
            rw.NodeRef<CSceneLayout>(ref n.scene, ref n.sceneFile);
        }
    }

    /// <summary>
    /// CGameCtnDecorationSize 0x003 skippable chunk
    /// </summary>
    [Chunk(0x0303B003)]
    public partial class Chunk0303B003 : SkippableChunk<CGameCtnDecorationSize>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x0303B003;

        public int Version { get; set; }


        public override void ReadWrite(CGameCtnDecorationSize n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.Int32(ref n.baseHeightOffset);
        }
    }




    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x0303B000 => new Chunk0303B000(),
        0x0303B001 => new Chunk0303B001(),
        0x0303B002 => new Chunk0303B002(),
        0x0303B003 => new Chunk0303B003(),
        _ => base.NewChunk(chunkId),
    };
}
