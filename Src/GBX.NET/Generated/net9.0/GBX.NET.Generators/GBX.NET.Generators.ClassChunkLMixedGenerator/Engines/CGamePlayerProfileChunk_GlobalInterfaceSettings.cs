namespace GBX.NET.Engines.Game;

/// <remarks>ID: 0x03179000</remarks>
[Class(0x03179000)]
public partial class CGamePlayerProfileChunk_GlobalInterfaceSettings : CGamePlayerProfileChunk, IClass
{
    [Hexadecimal] public static new uint Id => 0x03179000;




    private SPluginSettings[]? pluginSettings;
    [AppliedWithChunk<Chunk03179001>]
    public SPluginSettings[]? PluginSettings { get => pluginSettings; set => pluginSettings = value; }

    private bool openWebLinksInSteamOverlay;
    [AppliedWithChunk<Chunk03179004>]
    public bool OpenWebLinksInSteamOverlay { get => openWebLinksInSteamOverlay; set => openWebLinksInSteamOverlay = value; }

    private bool synchonizeSteamWorkshopFiles;
    [AppliedWithChunk<Chunk03179004>]
    public bool SynchonizeSteamWorkshopFiles { get => synchonizeSteamWorkshopFiles; set => synchonizeSteamWorkshopFiles = value; }

    private bool preferSteamScreenshots;
    [AppliedWithChunk<Chunk03179004>]
    public bool PreferSteamScreenshots { get => preferSteamScreenshots; set => preferSteamScreenshots = value; }

    /// <summary>
    /// Creates a new instance of <see cref="CGamePlayerProfileChunk_GlobalInterfaceSettings"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public CGamePlayerProfileChunk_GlobalInterfaceSettings() { }


    /// <summary>
    /// CGamePlayerProfileChunk_GlobalInterfaceSettings 0x001 skippable chunk
    /// </summary>
    [Chunk(0x03179001)]
    public partial class Chunk03179001 : SkippableChunk<CGamePlayerProfileChunk_GlobalInterfaceSettings>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x03179001;

        public int Version { get; set; }


        public override void ReadWrite(CGamePlayerProfileChunk_GlobalInterfaceSettings n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.ArrayReadableWritable<SPluginSettings>(ref n.pluginSettings!);
        }
    }

    /// <summary>
    /// CGamePlayerProfileChunk_GlobalInterfaceSettings 0x002 skippable chunk
    /// </summary>
    [Chunk(0x03179002)]
    public partial class Chunk03179002 : SkippableChunk<CGamePlayerProfileChunk_GlobalInterfaceSettings>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x03179002;

        public int Version { get; set; }

        public byte U01;

        public override void ReadWrite(CGamePlayerProfileChunk_GlobalInterfaceSettings n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.Byte(ref U01);
        }
    }

    /// <summary>
    /// CGamePlayerProfileChunk_GlobalInterfaceSettings 0x003 skippable chunk
    /// </summary>
    [Chunk(0x03179003)]
    public partial class Chunk03179003 : SkippableChunk<CGamePlayerProfileChunk_GlobalInterfaceSettings>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x03179003;

        public int Version { get; set; }

        public byte U01;

        public override void ReadWrite(CGamePlayerProfileChunk_GlobalInterfaceSettings n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.Byte(ref U01);
        }
    }

    /// <summary>
    /// CGamePlayerProfileChunk_GlobalInterfaceSettings 0x004 skippable chunk
    /// </summary>
    [Chunk(0x03179004)]
    public partial class Chunk03179004 : SkippableChunk<CGamePlayerProfileChunk_GlobalInterfaceSettings>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x03179004;

        public int Version { get; set; }


        public override void ReadWrite(CGamePlayerProfileChunk_GlobalInterfaceSettings n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.Boolean(ref n.openWebLinksInSteamOverlay);
            if (Version >= 1)
            {
                rw.Boolean(ref n.synchonizeSteamWorkshopFiles);
                if (Version >= 2)
                {
                    rw.Boolean(ref n.preferSteamScreenshots);
                }
            }
        }
    }

    /// <summary>
    /// CGamePlayerProfileChunk_GlobalInterfaceSettings 0x005 skippable chunk
    /// </summary>
    [Chunk(0x03179005)]
    public partial class Chunk03179005 : SkippableChunk<CGamePlayerProfileChunk_GlobalInterfaceSettings>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x03179005;

        public int Version { get; set; }

        public int U01;

        public override void ReadWrite(CGamePlayerProfileChunk_GlobalInterfaceSettings n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.Int32(ref U01);
        }
    }


    public sealed partial class SPluginSettings : IReadableWritable
    {

        private string? u01;
        public string? U01 { get => u01; set => u01 = value; }

        private float u02;
        public float U02 { get => u02; set => u02 = value; }

        private float u03;
        public float U03 { get => u03; set => u03 = value; }

        private float u04;
        public float U04 { get => u04; set => u04 = value; }

        public void ReadWrite(GbxReaderWriter rw, int v = 0)
        {
            rw.String(ref u01);
            rw.Single(ref u02);
            rw.Single(ref u03);
            rw.Single(ref u04);
        }
    }



    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x03179001 => new Chunk03179001(),
        0x03179002 => new Chunk03179002(),
        0x03179003 => new Chunk03179003(),
        0x03179004 => new Chunk03179004(),
        0x03179005 => new Chunk03179005(),
        _ => base.NewChunk(chunkId),
    };
}
