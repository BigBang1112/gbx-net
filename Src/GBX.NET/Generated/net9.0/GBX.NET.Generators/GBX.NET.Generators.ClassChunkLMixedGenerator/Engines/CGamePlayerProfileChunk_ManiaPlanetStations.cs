namespace GBX.NET.Engines.Game;

/// <remarks>ID: 0x03180000</remarks>
[Class(0x03180000)]
public partial class CGamePlayerProfileChunk_ManiaPlanetStations : CGamePlayerProfileChunk, IClass
{
    [Hexadecimal] public static new uint Id => 0x03180000;




    private OldGameStationDesc[]? oldStations;
    [AppliedWithChunk<Chunk03180000>]
    public OldGameStationDesc[]? OldStations { get => oldStations; set => oldStations = value; }

    private DisplayInfo[]? displayInfos;
    [AppliedWithChunk<Chunk03180001>]
    public DisplayInfo[]? DisplayInfos { get => displayInfos; set => displayInfos = value; }

    private bool isFirstLaunch;
    [AppliedWithChunk<Chunk03180001>]
    public bool IsFirstLaunch { get => isFirstLaunch; set => isFirstLaunch = value; }

    private string? latestTitleIdLoaded;
    [AppliedWithChunk<Chunk03180003>]
    public string? LatestTitleIdLoaded { get => latestTitleIdLoaded; set => latestTitleIdLoaded = value; }


    /// <summary>
    /// CGamePlayerProfileChunk_ManiaPlanetStations 0x000 skippable chunk (OldStations)
    /// </summary>
    [Chunk(0x03180000, "OldStations")]
    public partial class Chunk03180000 : SkippableChunk<CGamePlayerProfileChunk_ManiaPlanetStations>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x03180000;

        public int Version { get; set; }


        public override void ReadWrite(CGamePlayerProfileChunk_ManiaPlanetStations n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.ArrayReadableWritable<OldGameStationDesc>(ref n.oldStations!, version: Version);
        }
    }

    /// <summary>
    /// CGamePlayerProfileChunk_ManiaPlanetStations 0x001 skippable chunk
    /// </summary>
    [Chunk(0x03180001)]
    public partial class Chunk03180001 : SkippableChunk<CGamePlayerProfileChunk_ManiaPlanetStations>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x03180001;

        public int Version { get; set; }

        public int U01;

        public override void ReadWrite(CGamePlayerProfileChunk_ManiaPlanetStations n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.ArrayReadableWritable<DisplayInfo>(ref n.displayInfos!);
            if (Version >= 2)
            {
                rw.Boolean(ref n.isFirstLaunch);
                if (Version >= 3)
                {
                    rw.Int32(ref U01);
                }
            }
        }
    }

    /// <summary>
    /// CGamePlayerProfileChunk_ManiaPlanetStations 0x002 skippable chunk (Stations)
    /// </summary>
    [Chunk(0x03180002, "Stations")]
    public partial class Chunk03180002 : SkippableChunk<CGamePlayerProfileChunk_ManiaPlanetStations>
    {
        /// <inheritdoc />
        public override uint Id => 0x03180002;

    }

    /// <summary>
    /// CGamePlayerProfileChunk_ManiaPlanetStations 0x003 skippable chunk (LatestTitleIdLoaded)
    /// </summary>
    [Chunk(0x03180003, "LatestTitleIdLoaded")]
    public partial class Chunk03180003 : SkippableChunk<CGamePlayerProfileChunk_ManiaPlanetStations>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x03180003;

        public int Version { get; set; }


        public override void ReadWrite(CGamePlayerProfileChunk_ManiaPlanetStations n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.Id(ref n.latestTitleIdLoaded);
        }
    }


    public sealed partial class GameStationDesc : IReadableWritable
    {

        private int u01;
        public int U01 { get => u01; set => u01 = value; }

        private int u02;
        public int U02 { get => u02; set => u02 = value; }

        private string? titleId;
        public string? TitleId { get => titleId; set => titleId = value; }

        private byte[]? checksum;
        public byte[]? Checksum { get => checksum; set => checksum = value; }

        private int u03;
        public int U03 { get => u03; set => u03 = value; }

        private int u04;
        public int U04 { get => u04; set => u04 = value; }

        private int u05;
        public int U05 { get => u05; set => u05 = value; }

        private int u06;
        public int U06 { get => u06; set => u06 = value; }

        public void ReadWrite(GbxReaderWriter rw, int v = 0)
        {
            rw.Int32(ref u01);
            rw.Int32(ref u02);
            rw.Id(ref titleId);
            rw.Data(ref checksum!, 32);
            rw.Int32(ref u03);
            rw.Int32(ref u04);
            rw.Int32(ref u05);
            rw.Int32(ref u06);
        }
    }

    public sealed partial class DisplayInfo : IReadableWritable
    {

        private int u01;
        public int U01 { get => u01; set => u01 = value; }

        private int u02;
        public int U02 { get => u02; set => u02 = value; }

        private int u03;
        public int U03 { get => u03; set => u03 = value; }

        private int u04;
        public int U04 { get => u04; set => u04 = value; }

        public void ReadWrite(GbxReaderWriter rw, int v = 0)
        {
            rw.Int32(ref u01);
            rw.Int32(ref u02);
            rw.Int32(ref u03);
            rw.Int32(ref u04);
        }
    }

    public sealed partial class OldGameStationDesc : IReadableWritable
    {

        private int u01;
        public int U01 { get => u01; set => u01 = value; }

        private int u02;
        public int U02 { get => u02; set => u02 = value; }

        private string? u03;
        public string? U03 { get => u03; set => u03 = value; }

        private int u04;
        public int U04 { get => u04; set => u04 = value; }

        private int u05;
        public int U05 { get => u05; set => u05 = value; }

        private int u06;
        public int U06 { get => u06; set => u06 = value; }

        private int u07;
        public int U07 { get => u07; set => u07 = value; }

        public void ReadWrite(GbxReaderWriter rw, int v = 0)
        {
            rw.Int32(ref u01);
            rw.Int32(ref u02);
            rw.Id(ref u03);
            if (v >= 3)
            {
                rw.Int32(ref u04);
            }
            if (v <= 2)
            {
                rw.Int32(ref u05);
                rw.Int32(ref u06);
                rw.Int32(ref u07);
            }
        }
    }



    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x03180000 => new Chunk03180000(),
        0x03180001 => new Chunk03180001(),
        0x03180002 => new Chunk03180002(),
        0x03180003 => new Chunk03180003(),
        _ => base.NewChunk(chunkId),
    };
}
