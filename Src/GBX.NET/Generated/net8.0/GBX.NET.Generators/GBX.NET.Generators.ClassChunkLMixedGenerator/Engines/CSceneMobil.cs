namespace GBX.NET.Engines.Scene;

/// <remarks>ID: 0x0A011000</remarks>
[Class(0x0A011000)]
public partial class CSceneMobil : CSceneObject, IClass
{
    [Hexadecimal] public static new uint Id => 0x0A011000;




    private CSceneObjectLink[]? objectLink;
    [AppliedWithChunk<Chunk0A011003>]
    public CSceneObjectLink[]? ObjectLink { get => objectLink; set => objectLink = value; }

    private CHmsItem? item;
    [AppliedWithChunk<Chunk0A011005>]
    public CHmsItem? Item { get => item; set => item = value; }

    private CSceneMessageHandler? messageHandler;
    [AppliedWithChunk<Chunk0A011006>]
    public CSceneMessageHandler? MessageHandler { get => messageHandler; set => messageHandler = value; }

    /// <summary>
    /// Creates a new instance of <see cref="CSceneMobil"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public CSceneMobil() { }


    /// <summary>
    /// CSceneMobil 0x003 chunk
    /// </summary>
    [Chunk(0x0A011003)]
    [ChunkGameVersion(GameVersion.TM10 | GameVersion.TMSX | GameVersion.TMF | GameVersion.MP3)]
    public partial class Chunk0A011003 : Chunk<CSceneMobil>
    {
        /// <inheritdoc />
        public override uint Id => 0x0A011003;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.TM10 | GameVersion.TMSX | GameVersion.TMF | GameVersion.MP3;


        public override void ReadWrite(CSceneMobil n, GbxReaderWriter rw)
        {
            rw.ArrayNodeRef<CSceneObjectLink>(ref n.objectLink!);
        }
    }

    /// <summary>
    /// CSceneMobil 0x004 chunk
    /// </summary>
    [Chunk(0x0A011004)]
    [ChunkGameVersion(GameVersion.TM10 | GameVersion.TMSX)]
    public partial class Chunk0A011004 : Chunk<CSceneMobil>
    {
        /// <inheritdoc />
        public override uint Id => 0x0A011004;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.TM10 | GameVersion.TMSX;

        public CMwNod? U01;

        public override void ReadWrite(CSceneMobil n, GbxReaderWriter rw)
        {
            rw.NodeRef<CMwNod>(ref U01);
        }
    }

    /// <summary>
    /// CSceneMobil 0x005 chunk
    /// </summary>
    [Chunk(0x0A011005)]
    [ChunkGameVersion(GameVersion.TM10 | GameVersion.TMSX | GameVersion.TMF | GameVersion.MP3)]
    public partial class Chunk0A011005 : Chunk<CSceneMobil>
    {
        /// <inheritdoc />
        public override uint Id => 0x0A011005;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.TM10 | GameVersion.TMSX | GameVersion.TMF | GameVersion.MP3;


        public override void ReadWrite(CSceneMobil n, GbxReaderWriter rw)
        {
            rw.Node<CHmsItem>(ref n.item);
        }
    }

    /// <summary>
    /// CSceneMobil 0x006 chunk
    /// </summary>
    [Chunk(0x0A011006)]
    [ChunkGameVersion(GameVersion.TM10 | GameVersion.TMSX | GameVersion.TMF)]
    public partial class Chunk0A011006 : Chunk<CSceneMobil>
    {
        /// <inheritdoc />
        public override uint Id => 0x0A011006;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.TM10 | GameVersion.TMSX | GameVersion.TMF;


        public override void ReadWrite(CSceneMobil n, GbxReaderWriter rw)
        {
            rw.NodeRef<CSceneMessageHandler>(ref n.messageHandler);
        }
    }




    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x0A011003 => new Chunk0A011003(),
        0x0A011004 => new Chunk0A011004(),
        0x0A011005 => new Chunk0A011005(),
        0x0A011006 => new Chunk0A011006(),
        _ => base.NewChunk(chunkId),
    };
}
