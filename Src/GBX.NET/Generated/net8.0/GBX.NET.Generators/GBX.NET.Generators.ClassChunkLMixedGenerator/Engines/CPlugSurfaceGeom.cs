namespace GBX.NET.Engines.Plug;

/// <remarks>ID: 0x0900F000</remarks>
[Class(0x0900F000)]
public partial class CPlugSurfaceGeom : CPlug, IClass
{
    [Hexadecimal] public static new uint Id => 0x0900F000;





    /// <summary>
    /// CPlugSurfaceGeom 0x000 chunk
    /// </summary>
    [Chunk(0x0900C000)]
    [ChunkGameVersion(GameVersion.TM10 | GameVersion.TMSX | GameVersion.TMNESWC)]
    public partial class Chunk0900C000 : Chunk<CPlugSurfaceGeom>
    {
        /// <inheritdoc />
        public override uint Id => 0x0900C000;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.TM10 | GameVersion.TMSX | GameVersion.TMNESWC;

        public string? U01;

        public override void ReadWrite(CPlugSurfaceGeom n, GbxReaderWriter rw)
        {
            rw.Id(ref U01);
        }
    }

    /// <summary>
    /// CPlugSurfaceGeom 0x001 chunk
    /// </summary>
    [Chunk(0x0900C001)]
    [ChunkGameVersion(GameVersion.TM10 | GameVersion.TMSX)]
    public partial class Chunk0900C001 : Chunk<CPlugSurfaceGeom>
    {
        /// <inheritdoc />
        public override uint Id => 0x0900C001;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.TM10 | GameVersion.TMSX;

        public bool U01;

        public override void ReadWrite(CPlugSurfaceGeom n, GbxReaderWriter rw)
        {
            rw.Boolean(ref U01);
        }
    }

    /// <summary>
    /// CPlugSurfaceGeom 0x002 chunk
    /// </summary>
    [Chunk(0x0900D002)]
    [ChunkGameVersion(GameVersion.TM10 | GameVersion.TMSX | GameVersion.TMNESWC)]
    public partial class Chunk0900D002 : Chunk<CPlugSurfaceGeom>
    {
        /// <inheritdoc />
        public override uint Id => 0x0900D002;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.TM10 | GameVersion.TMSX | GameVersion.TMNESWC;

    }

    /// <summary>
    /// CPlugSurfaceGeom 0x002 chunk
    /// </summary>
    [Chunk(0x0900F002)]
    public partial class Chunk0900F002 : Chunk<CPlugSurfaceGeom>
    {
        /// <inheritdoc />
        public override uint Id => 0x0900F002;

    }

    /// <summary>
    /// CPlugSurfaceGeom 0x004 chunk
    /// </summary>
    [Chunk(0x0900F004)]
    [ChunkGameVersion(GameVersion.TMF)]
    public partial class Chunk0900F004 : Chunk<CPlugSurfaceGeom>
    {
        /// <inheritdoc />
        public override uint Id => 0x0900F004;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.TMF;

    }




    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x0900C000 => new Chunk0900C000(),
        0x0900C001 => new Chunk0900C001(),
        0x0900D002 => new Chunk0900D002(),
        0x0900F002 => new Chunk0900F002(),
        0x0900F004 => new Chunk0900F004(),
        _ => base.NewChunk(chunkId),
    };
}
