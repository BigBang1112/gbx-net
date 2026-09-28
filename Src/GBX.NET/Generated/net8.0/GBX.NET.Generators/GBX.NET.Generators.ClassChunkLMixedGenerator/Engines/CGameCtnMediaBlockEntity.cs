namespace GBX.NET.Engines.Game;

/// <remarks>ID: 0x0329F000</remarks>
[Class(0x0329F000)]
public partial class CGameCtnMediaBlockEntity : CGameCtnMediaBlock, IClass, CGameCtnMediaBlock.IHasTwoKeys, CGameCtnMediaBlock.IHasKeys
{
    [Hexadecimal] public static new uint Id => 0x0329F000;

    TimeSingle IHasTwoKeys.Start { get => Start.GetValueOrDefault(); set => Start = value; }
    TimeSingle IHasTwoKeys.End { get => End.GetValueOrDefault(); set => End = value; }
    IEnumerable<IKey> IHasKeys.Keys => Keys ?? [];



    private CPlugEntRecordData? recordData;
    [AppliedWithChunk<Chunk0329F000>]
    public CPlugEntRecordData? RecordData { get => recordData; set => recordData = value; }

    private TimeSingle? start;
    [AppliedWithChunk<Chunk0329F000>]
    public TimeSingle? Start { get => start; set => start = value; }

    private TimeSingle? end;
    [AppliedWithChunk<Chunk0329F000>]
    public TimeSingle? End { get => end; set => end = value; }

    private TimeSingle startOffset;
    [AppliedWithChunk<Chunk0329F000>]
    public TimeSingle StartOffset { get => startOffset; set => startOffset = value; }

    private int[]? noticeRecords;
    /// <summary>
    /// SPlugEntRecord array
    /// </summary>
    [AppliedWithChunk<Chunk0329F000>]
    public int[]? NoticeRecords { get => noticeRecords; set => noticeRecords = value; }

    private bool noDamage;
    [AppliedWithChunk<Chunk0329F000>]
    public bool NoDamage { get => noDamage; set => noDamage = value; }

    private bool forceLight;
    [AppliedWithChunk<Chunk0329F000>]
    public bool ForceLight { get => forceLight; set => forceLight = value; }

    private bool forceHue;
    [AppliedWithChunk<Chunk0329F000>]
    public bool ForceHue { get => forceHue; set => forceHue = value; }

    private Vec3 lightTrailColor;
    [AppliedWithChunk<Chunk0329F000>]
    public Vec3 LightTrailColor { get => lightTrailColor; set => lightTrailColor = value; }

    private Ident playerModel = Ident.Empty;
    [AppliedWithChunk<Chunk0329F000>]
    public Ident PlayerModel { get => playerModel; set => playerModel = value; }

    private List<PackDesc>? skinNames;
    /// <summary>
    /// name assumed from getter
    /// </summary>
    [AppliedWithChunk<Chunk0329F000>]
    public List<PackDesc>? SkinNames { get => skinNames; set => skinNames = value; }

    private bool hasBadges;
    [AppliedWithChunk<Chunk0329F000>]
    public bool HasBadges { get => hasBadges; set => hasBadges = value; }

    private SBadge? badge;
    [AppliedWithChunk<Chunk0329F000>]
    public SBadge? Badge { get => badge; set => badge = value; }

    private string? skinOptions;
    [AppliedWithChunk<Chunk0329F000>]
    [AppliedWithChunk<Chunk0329F002>]
    public string? SkinOptions { get => skinOptions; set => skinOptions = value; }

    private List<Key>? keys;
    [AppliedWithChunk<Chunk0329F000>]
    public List<Key>? Keys { get => keys; set => keys = value; }

    private string? ghostName;
    [AppliedWithChunk<Chunk0329F000>]
    public string? GhostName { get => ghostName; set => ghostName = value; }

    /// <summary>
    /// Creates a new instance of <see cref="CGameCtnMediaBlockEntity"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public CGameCtnMediaBlockEntity() { }


    /// <summary>
    /// CGameCtnMediaBlockEntity 0x000 chunk
    /// </summary>
    [Chunk(0x0329F000)]
    public partial class Chunk0329F000 : Chunk<CGameCtnMediaBlockEntity>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x0329F000;

        public int Version { get; set; }

        public bool U01;
        public int U02;
        /// <summary>
        /// some rgb, new light trail color?
        /// </summary>
        public Vec3? U03;
        public float U04;
        public int U05;
        public int U06;
        public int U07;

        public override void ReadWrite(CGameCtnMediaBlockEntity n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.NodeRef<CPlugEntRecordData>(ref n.recordData);
            if (Version <= 3)
            {
                rw.TimeSingleNullable(ref n.start);
                rw.TimeSingleNullable(ref n.end);
            }
            rw.TimeSingle(ref n.startOffset);
            rw.Array<int>(ref n.noticeRecords!); // SPlugEntRecord array
            if (Version >= 2)
            {
                rw.Boolean(ref n.noDamage);
                rw.Boolean(ref U01);
                rw.Boolean(ref n.forceLight);
                rw.Boolean(ref n.forceHue);
                if (Version <= 5)
                {
                    rw.Vec3(ref n.lightTrailColor);
                }
                if (Version >= 3)
                {
                    if (Version >= 11)
                    {
                        rw.Int32(ref U02);
                    }
                    rw.Ident(ref n.playerModel);
                    rw.Vec3(ref U03); // some rgb, new light trail color?
                    rw.ListPackDesc(ref n.skinNames!); // name assumed from getter
                    rw.Boolean(ref n.hasBadges);
                    if (n.HasBadges)
                    {
                        rw.ReadableWritable<SBadge>(ref n.badge);
                    }
                    if (Version >= 4)
                    {
                        if (Version >= 11)
                        {
                            rw.String(ref n.skinOptions);
                        }
                        rw.ListReadableWritable<Key>(ref n.keys!, version: Version);
                        if (Version == 5)
                        {
                            rw.Single(ref U04);
                        }
                        if (Version >= 7)
                        {
                            rw.String(ref n.ghostName);
                            if (Version >= 8)
                            {
                                rw.Int32(ref U05);
                                if (Version >= 11)
                                {
                                    rw.Int32(ref U06);
                                    rw.Int32(ref U07);
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    /// <summary>
    /// CGameCtnMediaBlockEntity 0x002 chunk (SkinOptions)
    /// </summary>
    [Chunk(0x0329F002, "SkinOptions")]
    public partial class Chunk0329F002 : Chunk<CGameCtnMediaBlockEntity>
    {
        /// <inheritdoc />
        public override uint Id => 0x0329F002;


        public override void ReadWrite(CGameCtnMediaBlockEntity n, GbxReaderWriter rw)
        {
            rw.String(ref n.skinOptions);
        }
    }


    public sealed partial class SSticker : IReadableWritable
    {

        private string? u01;
        public string? U01 { get => u01; set => u01 = value; }

        private string? u02;
        public string? U02 { get => u02; set => u02 = value; }

        public void ReadWrite(GbxReaderWriter rw, int v = 0)
        {
            rw.String(ref u01);
            rw.String(ref u02);
        }
    }

    public sealed partial class Key : IKey, IReadableWritable
    {

        private TimeSingle time;
        public TimeSingle Time { get => time; set => time = value; }

        private ELights lights;
        public ELights Lights { get => lights; set => lights = value; }

        private float? u01;
        public float? U01 { get => u01; set => u01 = value; }

        private int? u02;
        public int? U02 { get => u02; set => u02 = value; }

        private int? u03;
        public int? U03 { get => u03; set => u03 = value; }

        private float trailIntensity;
        public float TrailIntensity { get => trailIntensity; set => trailIntensity = value; }

        private float selfIllumIntensity;
        public float SelfIllumIntensity { get => selfIllumIntensity; set => selfIllumIntensity = value; }

        public void ReadWrite(GbxReaderWriter rw, int v = 0)
        {
            rw.TimeSingle(ref time);
            rw.EnumInt32<ELights>(ref lights);
            if (v >= 6)
            {
                rw.Single(ref u01);
                rw.Int32(ref u02);
                rw.Int32(ref u03);
                rw.Single(ref trailIntensity);
                if (v >= 9)
                {
                    rw.Single(ref selfIllumIntensity);
                }
            }
        }
    }

    public sealed partial class SBadge : IReadableWritable
    {

        private int version;
        public int Version { get => version; set => version = value; }

        private Vec3 color;
        public Vec3 Color { get => color; set => color = value; }

        private int u01;
        public int U01 { get => u01; set => u01 = value; }

        private string? u02;
        public string? U02 { get => u02; set => u02 = value; }

        private List<SSticker>? stickers;
        public List<SSticker>? Stickers { get => stickers; set => stickers = value; }

        private List<string>? layers;
        public List<string>? Layers { get => layers; set => layers = value; }

        public void ReadWrite(GbxReaderWriter rw, int v = 0)
        {
            rw.Int32(ref this.version);
            rw.Vec3(ref color);
            if (v == 0)
            {
                rw.Int32(ref u01);
                rw.String(ref u02);
            }
            rw.ListReadableWritable<SSticker>(ref stickers!);
            rw.ListString(ref layers!);
        }
    }


    public enum ELights
    {
        Auto,
        On,
        Off,
    }


    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x0329F000 => new Chunk0329F000(),
        0x0329F002 => new Chunk0329F002(),
        _ => base.NewChunk(chunkId),
    };
}
