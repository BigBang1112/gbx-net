namespace GBX.NET.Engines.Plug;

/// <remarks>ID: 0x0905E000</remarks>
[Class(0x0905E000)]
public partial class CPlugSoundSurface : CPlugSound, IClass
{
    [Hexadecimal] public static new uint Id => 0x0905E000;




    /// <summary>
    /// Creates a new instance of <see cref="CPlugSoundSurface"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public CPlugSoundSurface() { }


    /// <summary>
    /// CPlugSoundSurface 0x000 chunk
    /// </summary>
    [Chunk(0x0905E000)]
    public partial class Chunk0905E000 : Chunk<CPlugSoundSurface>
    {
        /// <inheritdoc />
        public override uint Id => 0x0905E000;

        /// <inheritdoc />
        public override bool Ignore => true;

    }

    /// <summary>
    /// CPlugSoundSurface 0x002 chunk
    /// </summary>
    [Chunk(0x0905E002)]
    public partial class Chunk0905E002 : Chunk<CPlugSoundSurface>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x0905E002;

        public int Version { get; set; }

        public float U01;
        public float U02;
        public float U03;
        public float U04;
        public bool U05;

        public override void ReadWrite(CPlugSoundSurface n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.Single(ref U01);
            rw.Single(ref U02);
            rw.Single(ref U03);
            rw.Single(ref U04);
            if (Version >= 2)
            {
                rw.Boolean(ref U05);
            }
        }
    }

    /// <summary>
    /// CPlugSoundSurface 0x003 chunk
    /// </summary>
    [Chunk(0x0905E003)]
    public partial class Chunk0905E003 : Chunk<CPlugSoundSurface>
    {
        /// <inheritdoc />
        public override uint Id => 0x0905E003;

        /// <inheritdoc />
        public override bool Ignore => true;

    }




    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x0905E000 => new Chunk0905E000(),
        0x0905E002 => new Chunk0905E002(),
        0x0905E003 => new Chunk0905E003(),
        _ => base.NewChunk(chunkId),
    };
}
