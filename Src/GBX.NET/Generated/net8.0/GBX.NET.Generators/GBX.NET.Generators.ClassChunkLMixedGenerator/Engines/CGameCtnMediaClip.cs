namespace GBX.NET.Engines.Game;

/// <remarks>ID: 0x03079000</remarks>
[Class(0x03079000)]
public partial class CGameCtnMediaClip : CMwNod, IClass
{
    [Hexadecimal] public static new uint Id => 0x03079000;




    private CSceneLayout? scene;
    [AppliedWithChunk<Chunk03079004>]
    public CSceneLayout? Scene { get => scene; set => scene = value; }

    private int localPlayerClipEntIndex = -1 ;
    /// <summary>
    /// also called LocalPlayerGhostId
    /// </summary>
    [AppliedWithChunk<Chunk03079007>]
    [AppliedWithChunk<Chunk0307900D>]
    public int LocalPlayerClipEntIndex { get => localPlayerClipEntIndex; set => localPlayerClipEntIndex = value; }

    private bool stopWhenLeave;
    [AppliedWithChunk<Chunk0307900A>]
    [AppliedWithChunk<Chunk0307900D>]
    public bool StopWhenLeave { get => stopWhenLeave; set => stopWhenLeave = value; }

    private bool stopWhenRespawn;
    [AppliedWithChunk<Chunk0307900D>]
    public bool StopWhenRespawn { get => stopWhenRespawn; set => stopWhenRespawn = value; }


    /// <summary>
    /// CGameCtnMediaClip 0x002 chunk (tracks)
    /// </summary>
    [Chunk(0x03079002, "tracks")]
    public partial class Chunk03079002 : Chunk<CGameCtnMediaClip>
    {
        /// <inheritdoc />
        public override uint Id => 0x03079002;

        public bool U01;

        public override void ReadWrite(CGameCtnMediaClip n, GbxReaderWriter rw)
        {
            rw.ListNodeRef_deprec<CGameCtnMediaTrack>(ref n.tracks!);
            rw.String(ref n.name);
            rw.Boolean(ref U01);
        }
    }

    /// <summary>
    /// CGameCtnMediaClip 0x003 chunk (tracks with sorting)
    /// </summary>
    [Chunk(0x03079003, "tracks with sorting")]
    [ChunkGameVersion(GameVersion.TMSX | GameVersion.TMNESWC | GameVersion.TMU)]
    public partial class Chunk03079003 : Chunk<CGameCtnMediaClip>
    {
        /// <inheritdoc />
        public override uint Id => 0x03079003;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.TMSX | GameVersion.TMNESWC | GameVersion.TMU;


        public override void ReadWrite(CGameCtnMediaClip n, GbxReaderWriter rw)
        {
            rw.ListNodeRef_deprec<CGameCtnMediaTrack>(ref n.tracks!);
            rw.String(ref n.name);
        }
    }

    /// <summary>
    /// CGameCtnMediaClip 0x004 chunk (Scene)
    /// </summary>
    [Chunk(0x03079004, "Scene")]
    [ChunkGameVersion(GameVersion.TMU | GameVersion.TMF)]
    public partial class Chunk03079004 : Chunk<CGameCtnMediaClip>
    {
        /// <inheritdoc />
        public override uint Id => 0x03079004;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.TMU | GameVersion.TMF;


        public override void ReadWrite(CGameCtnMediaClip n, GbxReaderWriter rw)
        {
            rw.NodeRef<CSceneLayout>(ref n.scene);
        }
    }

    /// <summary>
    /// CGameCtnMediaClip 0x005 chunk (tracks without sorting)
    /// </summary>
    [Chunk(0x03079005, "tracks without sorting")]
    [ChunkGameVersion(GameVersion.TMF | GameVersion.MP3)]
    public partial class Chunk03079005 : Chunk03079003
    {
        /// <inheritdoc />
        public override uint Id => 0x03079005;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.TMF | GameVersion.MP3;

    }

    /// <summary>
    /// CGameCtnMediaClip 0x007 chunk (LocalPlayerClipEntIndex)
    /// </summary>
    [Chunk(0x03079007, "LocalPlayerClipEntIndex")]
    [ChunkGameVersion(GameVersion.TMF | GameVersion.MP3)]
    public partial class Chunk03079007 : Chunk<CGameCtnMediaClip>
    {
        /// <inheritdoc />
        public override uint Id => 0x03079007;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.TMF | GameVersion.MP3;


        public override void ReadWrite(CGameCtnMediaClip n, GbxReaderWriter rw)
        {
            rw.Int32(ref n.localPlayerClipEntIndex); // also called LocalPlayerGhostId
        }
    }

    /// <summary>
    /// CGameCtnMediaClip 0x008 chunk
    /// </summary>
    [Chunk(0x03079008)]
    [ChunkGameVersion(GameVersion.MP3)]
    public partial class Chunk03079008 : Chunk<CGameCtnMediaClip>
    {
        /// <inheritdoc />
        public override uint Id => 0x03079008;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.MP3;

        public float U01 = 0.2f;

        public override void ReadWrite(CGameCtnMediaClip n, GbxReaderWriter rw)
        {
            rw.Single(ref U01);
        }
    }

    /// <summary>
    /// CGameCtnMediaClip 0x009 chunk
    /// </summary>
    [Chunk(0x03079009)]
    [ChunkGameVersion(GameVersion.MP3)]
    public partial class Chunk03079009 : Chunk<CGameCtnMediaClip>
    {
        /// <inheritdoc />
        public override uint Id => 0x03079009;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.MP3;

        /// <summary>
        /// same as 0x00D U02
        /// </summary>
        public string? U01;

        public override void ReadWrite(CGameCtnMediaClip n, GbxReaderWriter rw)
        {
            rw.String(ref U01); // same as 0x00D U02
        }
    }

    /// <summary>
    /// CGameCtnMediaClip 0x00A chunk (StopWhenLeave)
    /// </summary>
    [Chunk(0x0307900A, "StopWhenLeave")]
    [ChunkGameVersion(GameVersion.MP3)]
    public partial class Chunk0307900A : Chunk<CGameCtnMediaClip>
    {
        /// <inheritdoc />
        public override uint Id => 0x0307900A;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.MP3;


        public override void ReadWrite(CGameCtnMediaClip n, GbxReaderWriter rw)
        {
            rw.Boolean(ref n.stopWhenLeave);
        }
    }

    /// <summary>
    /// CGameCtnMediaClip 0x00B chunk
    /// </summary>
    [Chunk(0x0307900B)]
    [ChunkGameVersion(GameVersion.MP3)]
    public partial class Chunk0307900B : Chunk<CGameCtnMediaClip>
    {
        /// <inheritdoc />
        public override uint Id => 0x0307900B;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.MP3;

        /// <summary>
        /// probably NOT StopWhenRespawn
        /// </summary>
        public bool U01;

        public override void ReadWrite(CGameCtnMediaClip n, GbxReaderWriter rw)
        {
            rw.Boolean(ref U01); // probably NOT StopWhenRespawn
        }
    }

    /// <summary>
    /// CGameCtnMediaClip 0x00C chunk
    /// </summary>
    [Chunk(0x0307900C)]
    public partial class Chunk0307900C : Chunk<CGameCtnMediaClip>
    {
        /// <inheritdoc />
        public override uint Id => 0x0307900C;

        public int U01;

        public override void ReadWrite(CGameCtnMediaClip n, GbxReaderWriter rw)
        {
            rw.Int32(ref U01);
        }
    }

    /// <summary>
    /// CGameCtnMediaClip 0x00D chunk (MP tracks)
    /// </summary>
    [Chunk(0x0307900D, "MP tracks")]
    [ChunkGameVersion(GameVersion.MP4 | GameVersion.TM2020, 0, 1)]
    public partial class Chunk0307900D : Chunk<CGameCtnMediaClip>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x0307900D;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.MP4 | GameVersion.TM2020;

        public int Version { get; set; }

        public bool U01;
        /// <summary>
        /// Same as 0x009 U01
        /// </summary>
        public string? U02;
        public float U03;

        public override void ReadWrite(CGameCtnMediaClip n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.ListNodeRef_deprec<CGameCtnMediaTrack>(ref n.tracks!);
            rw.String(ref n.name);
            rw.Boolean(ref n.stopWhenLeave);
            rw.Boolean(ref U01);
            rw.Boolean(ref n.stopWhenRespawn);
            rw.String(ref U02); // Same as 0x009 U01
            rw.Single(ref U03);
            rw.Int32(ref n.localPlayerClipEntIndex); // = -1
        }
    }

    /// <summary>
    /// CGameCtnMediaClip 0x00E skippable chunk
    /// </summary>
    [Chunk(0x0307900E)]
    [ChunkGameVersion(GameVersion.TM2020)]
    public partial class Chunk0307900E : SkippableChunk<CGameCtnMediaClip>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x0307900E;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.TM2020;

        public int Version { get; set; }

        public int U01;

        public override void ReadWrite(CGameCtnMediaClip n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.Int32(ref U01);
        }
    }




    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x03079002 => new Chunk03079002(),
        0x03079003 => new Chunk03079003(),
        0x03079004 => new Chunk03079004(),
        0x03079005 => new Chunk03079005(),
        0x03079007 => new Chunk03079007(),
        0x03079008 => new Chunk03079008(),
        0x03079009 => new Chunk03079009(),
        0x0307900A => new Chunk0307900A(),
        0x0307900B => new Chunk0307900B(),
        0x0307900C => new Chunk0307900C(),
        0x0307900D => new Chunk0307900D(),
        0x0307900E => new Chunk0307900E(),
        _ => base.NewChunk(chunkId),
    };
}
