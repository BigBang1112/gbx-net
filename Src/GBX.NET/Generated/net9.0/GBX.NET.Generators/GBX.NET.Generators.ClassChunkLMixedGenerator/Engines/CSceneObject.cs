namespace GBX.NET.Engines.Scene;

/// <remarks>ID: 0x0A005000</remarks>
[Class(0x0A005000)]
public abstract partial class CSceneObject : CMwNod, IClass
{
    [Hexadecimal] public static new uint Id => 0x0A005000;




    private string? name;
    [AppliedWithChunk<Chunk0A005001>]
    public string? Name { get => name; set => name = value; }

    private CMotion? motion;
    [AppliedWithChunk<Chunk0A005003>]
    public CMotion? Motion { get => motion; set => motion = value; }

    /// <summary>
    /// Creates a new instance of <see cref="CSceneObject"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public CSceneObject() { }


    /// <summary>
    /// CSceneObject 0x001 chunk
    /// </summary>
    [Chunk(0x0A005001)]
    [ChunkGameVersion(GameVersion.TM10 | GameVersion.TMSX | GameVersion.TMF | GameVersion.MP3)]
    public partial class Chunk0A005001 : Chunk<CSceneObject>
    {
        /// <inheritdoc />
        public override uint Id => 0x0A005001;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.TM10 | GameVersion.TMSX | GameVersion.TMF | GameVersion.MP3;


        public override void ReadWrite(CSceneObject n, GbxReaderWriter rw)
        {
            rw.Id(ref n.name);
        }
    }

    /// <summary>
    /// CSceneObject 0x002 chunk
    /// </summary>
    [Chunk(0x0A005002)]
    public partial class Chunk0A005002 : Chunk<CSceneObject>
    {
        /// <inheritdoc />
        public override uint Id => 0x0A005002;

        /// <summary>
        /// same as 0x004 U01
        /// </summary>
        public bool U01;

        public override void ReadWrite(CSceneObject n, GbxReaderWriter rw)
        {
            rw.Boolean(ref U01); // same as 0x004 U01
        }
    }

    /// <summary>
    /// CSceneObject 0x003 chunk
    /// </summary>
    [Chunk(0x0A005003)]
    [ChunkGameVersion(GameVersion.TM10 | GameVersion.TMSX | GameVersion.TMF | GameVersion.MP3)]
    public partial class Chunk0A005003 : Chunk<CSceneObject>
    {
        /// <inheritdoc />
        public override uint Id => 0x0A005003;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.TM10 | GameVersion.TMSX | GameVersion.TMF | GameVersion.MP3;


        public override void ReadWrite(CSceneObject n, GbxReaderWriter rw)
        {
            rw.NodeRef<CMotion>(ref n.motion);
        }
    }

    /// <summary>
    /// CSceneObject 0x004 chunk
    /// </summary>
    [Chunk(0x0A005004)]
    [ChunkGameVersion(GameVersion.TM10 | GameVersion.TMSX)]
    public partial class Chunk0A005004 : Chunk<CSceneObject>
    {
        /// <inheritdoc />
        public override uint Id => 0x0A005004;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.TM10 | GameVersion.TMSX;

        /// <summary>
        /// same as 0x002 U01
        /// </summary>
        public int U01;

        public override void ReadWrite(CSceneObject n, GbxReaderWriter rw)
        {
            rw.Int32(ref U01); // same as 0x002 U01
        }
    }




    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x0A005001 => new Chunk0A005001(),
        0x0A005002 => new Chunk0A005002(),
        0x0A005003 => new Chunk0A005003(),
        0x0A005004 => new Chunk0A005004(),
        _ => base.NewChunk(chunkId),
    };
}
