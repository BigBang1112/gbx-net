namespace GBX.NET.Engines.Game;

/// <remarks>ID: 0x0312E000</remarks>
[Class(0x0312E000)]
public partial class CGamePlayerProfileChunk_InterfaceSettings : CGamePlayerProfileChunk, IClass
{
    [Hexadecimal] public static new uint Id => 0x0312E000;




    private string? menuLeagueFilter;
    [AppliedWithChunk<Chunk0312E000>]
    public string? MenuLeagueFilter { get => menuLeagueFilter; set => menuLeagueFilter = value; }

    private SCampaignSettings[]? campaignSettings;
    [AppliedWithChunk<Chunk0312E004>]
    public SCampaignSettings[]? CampaignSettings { get => campaignSettings; set => campaignSettings = value; }

    /// <summary>
    /// Creates a new instance of <see cref="CGamePlayerProfileChunk_InterfaceSettings"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public CGamePlayerProfileChunk_InterfaceSettings() { }


    /// <summary>
    /// CGamePlayerProfileChunk_InterfaceSettings 0x000 skippable chunk
    /// </summary>
    [Chunk(0x0312E000)]
    public partial class Chunk0312E000 : SkippableChunk<CGamePlayerProfileChunk_InterfaceSettings>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x0312E000;

        public int Version { get; set; }

        public byte U01;
        public byte U02;

        public override void ReadWrite(CGamePlayerProfileChunk_InterfaceSettings n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.String(ref n.menuLeagueFilter);
            rw.Byte(ref U01);
            rw.Byte(ref U02);
        }
    }

    /// <summary>
    /// CGamePlayerProfileChunk_InterfaceSettings 0x002 skippable chunk
    /// </summary>
    [Chunk(0x0312E002)]
    public partial class Chunk0312E002 : SkippableChunk<CGamePlayerProfileChunk_InterfaceSettings>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x0312E002;

        public int Version { get; set; }

        public int U01;
        public Pair[]? U02;

        public override void ReadWrite(CGamePlayerProfileChunk_InterfaceSettings n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.Int32(ref U01);
            rw.ArrayReadableWritable<Pair>(ref U02!);
        }
    }

    /// <summary>
    /// CGamePlayerProfileChunk_InterfaceSettings 0x003 skippable chunk
    /// </summary>
    [Chunk(0x0312E003)]
    public partial class Chunk0312E003 : SkippableChunk<CGamePlayerProfileChunk_InterfaceSettings>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x0312E003;

        public int Version { get; set; }

        public Vec3 U01;
        public Vec3 U02;
        public Vec3 U03;
        public Vec3 U04;
        public Vec3 U05;

        public override void ReadWrite(CGamePlayerProfileChunk_InterfaceSettings n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.Vec3(ref U01);
            rw.Vec3(ref U02);
            rw.Vec3(ref U03);
            rw.Vec3(ref U04);
            rw.Vec3(ref U05);
        }
    }

    /// <summary>
    /// CGamePlayerProfileChunk_InterfaceSettings 0x004 skippable chunk
    /// </summary>
    [Chunk(0x0312E004)]
    public partial class Chunk0312E004 : SkippableChunk<CGamePlayerProfileChunk_InterfaceSettings>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x0312E004;

        public int Version { get; set; }


        public override void ReadWrite(CGamePlayerProfileChunk_InterfaceSettings n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.ArrayReadableWritable<SCampaignSettings>(ref n.campaignSettings!);
        }
    }

    /// <summary>
    /// CGamePlayerProfileChunk_InterfaceSettings 0x005 skippable chunk
    /// </summary>
    [Chunk(0x0312E005)]
    public partial class Chunk0312E005 : SkippableChunk<CGamePlayerProfileChunk_InterfaceSettings>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x0312E005;

        public int Version { get; set; }

        public string? U01;
        public string? U02;
        public string? U03;
        public string? U04;
        public string? U05;
        public string? U06;

        public override void ReadWrite(CGamePlayerProfileChunk_InterfaceSettings n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.String(ref U01);
            rw.String(ref U02);
            rw.String(ref U03);
            rw.String(ref U04);
            rw.String(ref U05);
            rw.String(ref U06);
        }
    }

    /// <summary>
    /// CGamePlayerProfileChunk_InterfaceSettings 0x006 skippable chunk
    /// </summary>
    [Chunk(0x0312E006)]
    public partial class Chunk0312E006 : SkippableChunk<CGamePlayerProfileChunk_InterfaceSettings>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x0312E006;

        public int Version { get; set; }

        public int U01;
        public int U02;
        public Unknown[]? U03;
        public bool U04;

        public override void ReadWrite(CGamePlayerProfileChunk_InterfaceSettings n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.Int32(ref U01);
            rw.Int32(ref U02);
            rw.ArrayReadableWritable<Unknown>(ref U03!, version: U02);
            rw.Boolean(ref U04);
        }
    }


    public sealed partial class SCampaignSettings : IReadableWritable
    {

        private byte u01;
        public byte U01 { get => u01; set => u01 = value; }

        private string? u02;
        public string? U02 { get => u02; set => u02 = value; }

        private int u03;
        public int U03 { get => u03; set => u03 = value; }

        private SSoloChallengesCurrentPage[]? soloChallengesCurrentPages;
        public SSoloChallengesCurrentPage[]? SoloChallengesCurrentPages { get => soloChallengesCurrentPages; set => soloChallengesCurrentPages = value; }

        public void ReadWrite(GbxReaderWriter rw, int v = 0)
        {
            rw.Byte(ref u01);
            rw.Id(ref u02);
            rw.Int32(ref u03);
            rw.ArrayReadableWritable<SSoloChallengesCurrentPage>(ref soloChallengesCurrentPages!);
        }
    }

    public sealed partial class SSoloChallengesCurrentPage : IReadableWritable
    {

        private int u01;
        public int U01 { get => u01; set => u01 = value; }

        private int u02;
        public int U02 { get => u02; set => u02 = value; }

        public void ReadWrite(GbxReaderWriter rw, int v = 0)
        {
            rw.Int32(ref u01);
            rw.Int32(ref u02);
        }
    }

    public sealed partial class Pair : IReadableWritable
    {

        private string? u01;
        public string? U01 { get => u01; set => u01 = value; }

        private int u02;
        public int U02 { get => u02; set => u02 = value; }

        public void ReadWrite(GbxReaderWriter rw, int v = 0)
        {
            rw.Id(ref u01);
            rw.Int32(ref u02);
        }
    }

    public sealed partial class Unknown : IReadableWritable
    {

        private Ident? u01;
        public Ident? U01 { get => u01; set => u01 = value; }

        private byte[]? u02;
        public byte[]? U02 { get => u02; set => u02 = value; }

        private int u03;
        public int U03 { get => u03; set => u03 = value; }

        private string? u04;
        public string? U04 { get => u04; set => u04 = value; }

        public void ReadWrite(GbxReaderWriter rw, int v = 0)
        {
            rw.Ident(ref u01);
            if (v == 0)
            {
                rw.Data(ref u02);
            }
            rw.Int32(ref u03);
            rw.String(ref u04);
        }
    }



    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x0312E000 => new Chunk0312E000(),
        0x0312E002 => new Chunk0312E002(),
        0x0312E003 => new Chunk0312E003(),
        0x0312E004 => new Chunk0312E004(),
        0x0312E005 => new Chunk0312E005(),
        0x0312E006 => new Chunk0312E006(),
        _ => base.NewChunk(chunkId),
    };
}
