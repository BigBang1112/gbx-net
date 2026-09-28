namespace GBX.NET.Engines.TrackMania;

/// <remarks>ID: 0x240D5000</remarks>
[Class(0x240D5000)]
public partial class CGamePlayerProfileChunk_TrackManiaSettings : CGamePlayerProfileChunk, IClass
{
    [Hexadecimal] public static new uint Id => 0x240D5000;




    /// <summary>
    /// Creates a new instance of <see cref="CGamePlayerProfileChunk_TrackManiaSettings"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public CGamePlayerProfileChunk_TrackManiaSettings() { }


    /// <summary>
    /// CGamePlayerProfileChunk_TrackManiaSettings 0x000 skippable chunk
    /// </summary>
    [Chunk(0x240D5000)]
    public partial class Chunk240D5000 : SkippableChunk<CGamePlayerProfileChunk_TrackManiaSettings>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x240D5000;

        public int Version { get; set; }

        public byte U01;

        public override void ReadWrite(CGamePlayerProfileChunk_TrackManiaSettings n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.Byte(ref U01);
        }
    }

    /// <summary>
    /// CGamePlayerProfileChunk_TrackManiaSettings 0x001 skippable chunk
    /// </summary>
    [Chunk(0x240D5001)]
    public partial class Chunk240D5001 : SkippableChunk<CGamePlayerProfileChunk_TrackManiaSettings>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x240D5001;

        public int Version { get; set; }

        public byte[]? U01;

        public override void ReadWrite(CGamePlayerProfileChunk_TrackManiaSettings n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.Array<byte>(ref U01!);
        }
    }

    /// <summary>
    /// CGamePlayerProfileChunk_TrackManiaSettings 0x002 skippable chunk
    /// </summary>
    [Chunk(0x240D5002)]
    public partial class Chunk240D5002 : SkippableChunk<CGamePlayerProfileChunk_TrackManiaSettings>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x240D5002;

        public int Version { get; set; }

        public byte[]? U01;

        public override void ReadWrite(CGamePlayerProfileChunk_TrackManiaSettings n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.Array<byte>(ref U01!);
        }
    }

    /// <summary>
    /// CGamePlayerProfileChunk_TrackManiaSettings 0x003 skippable chunk
    /// </summary>
    [Chunk(0x240D5003)]
    public partial class Chunk240D5003 : SkippableChunk<CGamePlayerProfileChunk_TrackManiaSettings>
    {
        /// <inheritdoc />
        public override uint Id => 0x240D5003;

        /// <inheritdoc />
        public override bool Ignore => true;

    }

    /// <summary>
    /// CGamePlayerProfileChunk_TrackManiaSettings 0x004 skippable chunk
    /// </summary>
    [Chunk(0x240D5004)]
    public partial class Chunk240D5004 : SkippableChunk<CGamePlayerProfileChunk_TrackManiaSettings>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x240D5004;

        public int Version { get; set; }

        public int U01;
        public int U02;

        public override void ReadWrite(CGamePlayerProfileChunk_TrackManiaSettings n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.Int32(ref U01);
            rw.Int32(ref U02);
        }
    }




    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x240D5000 => new Chunk240D5000(),
        0x240D5001 => new Chunk240D5001(),
        0x240D5002 => new Chunk240D5002(),
        0x240D5003 => new Chunk240D5003(),
        0x240D5004 => new Chunk240D5004(),
        _ => base.NewChunk(chunkId),
    };
}
