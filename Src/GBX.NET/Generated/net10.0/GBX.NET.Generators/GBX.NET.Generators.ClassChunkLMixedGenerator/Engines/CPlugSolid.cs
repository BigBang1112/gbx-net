namespace GBX.NET.Engines.Plug;

/// <remarks>ID: 0x09005000</remarks>
[Class(0x09005000)]
public partial class CPlugSolid : CPlug, IClass
{
    [Hexadecimal] public static new uint Id => 0x09005000;




    private int typeAndIndex;
    [AppliedWithChunk<Chunk09005000>]
    public int TypeAndIndex { get => typeAndIndex; set => typeAndIndex = value; }

    private CPlug? tree;
    [AppliedWithChunk<Chunk0900500D>]
    [AppliedWithChunk<Chunk09005011>]
    public CPlug? Tree { get => treeFile?.GetNode(ref tree) ?? tree; set => tree = value; }
    private Components.GbxRefTableFile? treeFile;
    public Components.GbxRefTableFile? TreeFile { get => treeFile; set => treeFile = value; }
    public CPlug? GetTree(GbxReadSettings settings = default, bool exceptions = false) => treeFile?.GetNode(ref tree, settings, exceptions) ?? tree;

    private PreLightGen? solidPreLightGen;
    [AppliedWithChunk<Chunk09005017>]
    public PreLightGen? SolidPreLightGen { get => solidPreLightGen; set => solidPreLightGen = value; }

    private DateTime? fileWriteTime;
    [AppliedWithChunk<Chunk09005017>]
    public DateTime? FileWriteTime { get => fileWriteTime; set => fileWriteTime = value; }


    /// <summary>
    /// CPlugSolid 0x000 chunk
    /// </summary>
    [Chunk(0x09005000)]
    [ChunkGameVersion(GameVersion.TM10 | GameVersion.TMSX | GameVersion.TMNESWC | GameVersion.TMF | GameVersion.MP3 | GameVersion.TMT | GameVersion.MP4)]
    public partial class Chunk09005000 : Chunk<CPlugSolid>
    {
        /// <inheritdoc />
        public override uint Id => 0x09005000;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.TM10 | GameVersion.TMSX | GameVersion.TMNESWC | GameVersion.TMF | GameVersion.MP3 | GameVersion.TMT | GameVersion.MP4;


        public override void ReadWrite(CPlugSolid n, GbxReaderWriter rw)
        {
            rw.Int32(ref n.typeAndIndex);
        }
    }

    /// <summary>
    /// CPlugSolid 0x006 chunk
    /// </summary>
    [Chunk(0x09005006)]
    public partial class Chunk09005006 : Chunk<CPlugSolid>
    {
        /// <inheritdoc />
        public override uint Id => 0x09005006;

        public float U01;
        public float U02;
        public float U03;
        public float U04;
        public float U05;
        public float U06;
        public Mat3? U07;

        public override void ReadWrite(CPlugSolid n, GbxReaderWriter rw)
        {
            rw.Single(ref U01);
            rw.Single(ref U02);
            rw.Single(ref U03);
            rw.Single(ref U04);
            rw.Single(ref U05);
            rw.Single(ref U06);
            rw.Mat3(ref U07);
        }
    }

    /// <summary>
    /// CPlugSolid 0x007 chunk
    /// </summary>
    [Chunk(0x09005007)]
    [ChunkGameVersion(GameVersion.TM10 | GameVersion.TMSX)]
    public partial class Chunk09005007 : Chunk<CPlugSolid>
    {
        /// <inheritdoc />
        public override uint Id => 0x09005007;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.TM10 | GameVersion.TMSX;

        public bool U01;

        public override void ReadWrite(CPlugSolid n, GbxReaderWriter rw)
        {
            rw.Boolean(ref U01);
        }
    }

    /// <summary>
    /// CPlugSolid 0x00B chunk
    /// </summary>
    [Chunk(0x0900500B)]
    public partial class Chunk0900500B : Chunk<CPlugSolid>
    {
        /// <inheritdoc />
        public override uint Id => 0x0900500B;

        public bool U01;
        public bool U02;
        public bool U03;
        public bool U04;
        public bool U05;
        public bool U06;
        public int U07;

        public override void ReadWrite(CPlugSolid n, GbxReaderWriter rw)
        {
            rw.Boolean(ref U01);
            rw.Boolean(ref U02);
            rw.Boolean(ref U03);
            rw.Boolean(ref U04);
            rw.Boolean(ref U05);
            rw.Boolean(ref U06);
            rw.Int32(ref U07);
        }
    }

    /// <summary>
    /// CPlugSolid 0x00C chunk
    /// </summary>
    [Chunk(0x0900500C)]
    [ChunkGameVersion(GameVersion.TM10 | GameVersion.TMSX | GameVersion.TMNESWC)]
    public partial class Chunk0900500C : Chunk<CPlugSolid>
    {
        /// <inheritdoc />
        public override uint Id => 0x0900500C;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.TM10 | GameVersion.TMSX | GameVersion.TMNESWC;

        public bool U01;
        public bool U02;
        public bool U03;
        public bool U04;
        public bool U05;
        public bool U06;
        public bool U07;
        public bool U08;
        public bool U09;
        public bool U10;
        public float U11;
        public int U12;
        public float U13;
        public float U14;
        public float U15;
        public float U16;
        public int U17;
        public int U18;

        public override void ReadWrite(CPlugSolid n, GbxReaderWriter rw)
        {
            rw.Boolean(ref U01);
            rw.Boolean(ref U02);
            rw.Boolean(ref U03);
            rw.Boolean(ref U04);
            rw.Boolean(ref U05);
            rw.Boolean(ref U06);
            rw.Boolean(ref U07);
            rw.Boolean(ref U08);
            rw.Boolean(ref U09);
            rw.Boolean(ref U10);
            rw.Single(ref U11);
            rw.Int32(ref U12);
            rw.Single(ref U13);
            rw.Single(ref U14);
            rw.Single(ref U15);
            rw.Single(ref U16);
            rw.Int32(ref U17);
            rw.Int32(ref U18);
        }
    }

    /// <summary>
    /// CPlugSolid 0x00D chunk
    /// </summary>
    [Chunk(0x0900500D)]
    [ChunkGameVersion(GameVersion.TM10 | GameVersion.TMSX | GameVersion.TMNESWC)]
    public partial class Chunk0900500D : Chunk<CPlugSolid>
    {
        /// <inheritdoc />
        public override uint Id => 0x0900500D;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.TM10 | GameVersion.TMSX | GameVersion.TMNESWC;

        public bool U01;
        public bool U02;

        public override void ReadWrite(CPlugSolid n, GbxReaderWriter rw)
        {
            rw.Boolean(ref U01);
            rw.Boolean(ref U02);
            rw.NodeRef<CPlug>(ref n.tree, ref n.treeFile);
        }
    }

    /// <summary>
    /// CPlugSolid 0x00E chunk
    /// </summary>
    [Chunk(0x0900500E)]
    [ChunkGameVersion(GameVersion.TM10 | GameVersion.TMSX | GameVersion.TMNESWC | GameVersion.TMF)]
    public partial class Chunk0900500E : Chunk<CPlugSolid>
    {
        /// <inheritdoc />
        public override uint Id => 0x0900500E;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.TM10 | GameVersion.TMSX | GameVersion.TMNESWC | GameVersion.TMF;

        public float U01;
        public float U02;
        public float U03;
        public float U04;
        public Iso4 U05;

        public override void ReadWrite(CPlugSolid n, GbxReaderWriter rw)
        {
            rw.Single(ref U01);
            rw.Single(ref U02);
            rw.Single(ref U03);
            rw.Single(ref U04);
            rw.Iso4(ref U05);
        }
    }

    /// <summary>
    /// CPlugSolid 0x00F chunk
    /// </summary>
    [Chunk(0x0900500F)]
    [ChunkGameVersion(GameVersion.TM10 | GameVersion.TMSX | GameVersion.TMNESWC)]
    public partial class Chunk0900500F : Chunk<CPlugSolid>
    {
        /// <inheritdoc />
        public override uint Id => 0x0900500F;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.TM10 | GameVersion.TMSX | GameVersion.TMNESWC;

        public float U01;
        public float U02;

        public override void ReadWrite(CPlugSolid n, GbxReaderWriter rw)
        {
            rw.Single(ref U01);
            rw.Single(ref U02);
        }
    }

    /// <summary>
    /// CPlugSolid 0x010 chunk
    /// </summary>
    [Chunk(0x09005010)]
    [ChunkGameVersion(GameVersion.TMSX | GameVersion.TMNESWC | GameVersion.TMF | GameVersion.MP3 | GameVersion.TMT | GameVersion.MP4)]
    public partial class Chunk09005010 : Chunk<CPlugSolid>
    {
        /// <inheritdoc />
        public override uint Id => 0x09005010;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.TMSX | GameVersion.TMNESWC | GameVersion.TMF | GameVersion.MP3 | GameVersion.TMT | GameVersion.MP4;

        /// <summary>
        /// CSceneVehicleEnvironment
        /// </summary>
        public CMwNod? U01;

        public override void ReadWrite(CPlugSolid n, GbxReaderWriter rw)
        {
            rw.NodeRef<CMwNod>(ref U01); // CSceneVehicleEnvironment
        }
    }

    /// <summary>
    /// CPlugSolid 0x011 chunk
    /// </summary>
    [Chunk(0x09005011)]
    [ChunkGameVersion(GameVersion.TMF | GameVersion.MP3 | GameVersion.TMT | GameVersion.MP4)]
    public partial class Chunk09005011 : Chunk<CPlugSolid>
    {
        /// <inheritdoc />
        public override uint Id => 0x09005011;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.TMF | GameVersion.MP3 | GameVersion.TMT | GameVersion.MP4;

        /// <summary>
        /// part of SetUseModel, possibly the 'Use' parameter
        /// </summary>
        public bool U01;
        /// <summary>
        /// m_Model != null
        /// </summary>
        public bool U02;
        public bool? U03;

        public override void ReadWrite(CPlugSolid n, GbxReaderWriter rw)
        {
            rw.Boolean(ref U01); // part of SetUseModel, possibly the 'Use' parameter
            rw.Boolean(ref U02); // m_Model != null
            if (U02)
            {
                rw.Boolean(ref U03);
            }
            rw.NodeRef<CPlug>(ref n.tree, ref n.treeFile);
        }
    }

    /// <summary>
    /// CPlugSolid 0x012 chunk
    /// </summary>
    [Chunk(0x09005012)]
    [ChunkGameVersion(GameVersion.TMF)]
    public partial class Chunk09005012 : Chunk<CPlugSolid>
    {
        /// <inheritdoc />
        public override uint Id => 0x09005012;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.TMF;

        public byte U01;

        public override void ReadWrite(CPlugSolid n, GbxReaderWriter rw)
        {
            rw.Byte(ref U01);
        }
    }

    /// <summary>
    /// CPlugSolid 0x017 chunk (SolidPreLightGen)
    /// </summary>
    [Chunk(0x09005017, "SolidPreLightGen")]
    [ChunkGameVersion(GameVersion.MP3 | GameVersion.TMT | GameVersion.MP4, 3, 3, 3)]
    public partial class Chunk09005017 : Chunk<CPlugSolid>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x09005017;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.MP3 | GameVersion.TMT | GameVersion.MP4;

        public int Version { get; set; }

        public bool U01;
        public byte U02;
        public float U03;
        public bool U04;
        public Rect U05;
        public Rect U06;
        /// <summary>
        /// sprite count def
        /// </summary>
        public Int2 U07;
        public BoxAligned[]? U08;

        public override void ReadWrite(CPlugSolid n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            if (Version >= 3)
            {
                rw.Boolean(ref U01);
                if (U01)
                {
                    rw.ReadableWritable<PreLightGen>(ref n.solidPreLightGen);
                }
            }
            if (Version <= 2)
            {
                rw.Byte(ref U02);
                rw.Single(ref U03);
                rw.Boolean(ref U04);
                rw.Rect(ref U05);
                rw.Rect(ref U06);
                rw.Int2(ref U07); // sprite count def
                if (Version >= 1)
                {
                    rw.Array<BoxAligned>(ref U08!);
                }
            }
            if (Version >= 2)
            {
                rw.FileTime(ref n.fileWriteTime);
            }
        }
    }

    /// <summary>
    /// CPlugSolid 0x019 chunk
    /// </summary>
    [Chunk(0x09005019)]
    [ChunkGameVersion(GameVersion.MP3 | GameVersion.TMT | GameVersion.MP4, 0, 0, 3)]
    public partial class Chunk09005019 : Chunk<CPlugSolid>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x09005019;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.MP3 | GameVersion.TMT | GameVersion.MP4;

        public int Version { get; set; }

        public CPlugSound[]? U01;
        public CPlugParticleEmitterModel[]? U02;
        public LocatedInstance[]? U03;
        public LocatedInstance[]? U04;
        public int U05;
        public string[]? U06;
        public Iso4[]? U07;
        public string? U08;
        public int U09;
        public CPlugPath? U10;

        public override void ReadWrite(CPlugSolid n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.ArrayNodeRef_deprec<CPlugSound>(ref U01!);
            rw.ArrayNodeRef_deprec<CPlugParticleEmitterModel>(ref U02!);
            rw.ArrayReadableWritable<LocatedInstance>(ref U03!);
            rw.ArrayReadableWritable<LocatedInstance>(ref U04!);
            if (Version >= 1)
            {
                rw.Int32(ref U05);
                if (Version >= 2)
                {
                    rw.ArrayId(ref U06!);
                    rw.Array<Iso4>(ref U07!);
                    if (Version >= 3)
                    {
                        rw.String(ref U08);
                        if (Version >= 4)
                        {
                            rw.Int32(ref U09);
                            if (Version >= 5)
                            {
                                rw.NodeRef<CPlugPath>(ref U10);
                            }
                        }
                    }
                }
            }
        }
    }

    /// <summary>
    /// CPlugSolid 0x01A skippable chunk (lod normal map)
    /// </summary>
    [Chunk(0x0900501A, "lod normal map")]
    public partial class Chunk0900501A : SkippableChunk<CPlugSolid>
    {
        /// <inheritdoc />
        public override uint Id => 0x0900501A;

        /// <inheritdoc />
        public override bool Ignore => true;

    }


    public sealed partial class PreLightGen : IReadableWritable
    {

        private int version;
        public int Version { get => version; set => version = value; }

        private int u01;
        public int U01 { get => u01; set => u01 = value; }

        private float u02;
        public float U02 { get => u02; set => u02 = value; }

        private bool u03;
        public bool U03 { get => u03; set => u03 = value; }

        private float u04;
        public float U04 { get => u04; set => u04 = value; }

        private float u05;
        public float U05 { get => u05; set => u05 = value; }

        private float u06;
        public float U06 { get => u06; set => u06 = value; }

        private float u07;
        public float U07 { get => u07; set => u07 = value; }

        private float u08;
        public float U08 { get => u08; set => u08 = value; }

        private float u09;
        public float U09 { get => u09; set => u09 = value; }

        private float u10;
        public float U10 { get => u10; set => u10 = value; }

        private float u11;
        public float U11 { get => u11; set => u11 = value; }

        private int u12;
        public int U12 { get => u12; set => u12 = value; }

        private int u13;
        public int U13 { get => u13; set => u13 = value; }

        private BoxAligned[]? u14;
        public BoxAligned[]? U14 { get => u14; set => u14 = value; }

        private UvGroup[]? u15;
        public UvGroup[]? U15 { get => u15; set => u15 = value; }

        public void ReadWrite(GbxReaderWriter rw, int v = 0)
        {
            rw.Int32(ref this.version);
            rw.Int32(ref u01);
            rw.Single(ref u02);
            rw.Boolean(ref u03);
            rw.Single(ref u04);
            rw.Single(ref u05);
            rw.Single(ref u06);
            rw.Single(ref u07);
            rw.Single(ref u08);
            rw.Single(ref u09);
            rw.Single(ref u10);
            rw.Single(ref u11);
            rw.Int32(ref u12);
            rw.Int32(ref u13);
            rw.Array<BoxAligned>(ref u14!);
            if (Version>=1)
            {
                rw.ArrayReadableWritable<UvGroup>(ref u15!);
            }
        }
    }

    public sealed partial class LocatedInstance : IReadableWritable
    {

        private int u01;
        public int U01 { get => u01; set => u01 = value; }

        private Iso4 u02;
        public Iso4 U02 { get => u02; set => u02 = value; }

        public void ReadWrite(GbxReaderWriter rw, int v = 0)
        {
            rw.Int32(ref u01);
            rw.Iso4(ref u02);
        }
    }

    public sealed partial class UvGroup : IReadableWritable
    {

        private float u01;
        public float U01 { get => u01; set => u01 = value; }

        private float u02;
        public float U02 { get => u02; set => u02 = value; }

        private float u03;
        public float U03 { get => u03; set => u03 = value; }

        private float u04;
        public float U04 { get => u04; set => u04 = value; }

        private float u05;
        public float U05 { get => u05; set => u05 = value; }

        public void ReadWrite(GbxReaderWriter rw, int v = 0)
        {
            rw.Single(ref u01);
            rw.Single(ref u02);
            rw.Single(ref u03);
            rw.Single(ref u04);
            rw.Single(ref u05);
        }
    }



    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x09005000 => new Chunk09005000(),
        0x09005006 => new Chunk09005006(),
        0x09005007 => new Chunk09005007(),
        0x0900500B => new Chunk0900500B(),
        0x0900500C => new Chunk0900500C(),
        0x0900500D => new Chunk0900500D(),
        0x0900500E => new Chunk0900500E(),
        0x0900500F => new Chunk0900500F(),
        0x09005010 => new Chunk09005010(),
        0x09005011 => new Chunk09005011(),
        0x09005012 => new Chunk09005012(),
        0x09005017 => new Chunk09005017(),
        0x09005019 => new Chunk09005019(),
        0x0900501A => new Chunk0900501A(),
        _ => base.NewChunk(chunkId),
    };
}
