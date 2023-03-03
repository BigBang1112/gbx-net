using GBX.NET.Builders.Engines.Game;

namespace GBX.NET.Engines.Game;

/// <summary>
/// MediaTracker block - Entity.
/// </summary>
/// <remarks>ID: 0x0329F000</remarks>
[Node(0x0329F000)]
public partial class CGameCtnMediaBlockEntity : CGameCtnMediaBlock, CGameCtnMediaBlock.IHasTwoKeys, CGameCtnMediaBlock.IHasKeys
{
    private CPlugEntRecordData recordData;
    private TimeSingle? start;
    private TimeSingle? end;
    private IList<Key>? keys;
    private TimeSingle startOffset;
    private int[] noticeRecords = Array.Empty<int>();
    private bool noDamage;
    private bool forceLight;
    private bool forceHue;
    private Vec3 lightTrailColor = (1, 0, 0);
    private Ident? playerModel;
    private bool hasBadges;
    private IList<FileRef> skinNames = Array.Empty<FileRef>();
    private int? badgeVersion;
    private Badge? badge;
    private string? skinOptions;
    private string? ghostName;

    IEnumerable<CGameCtnMediaBlock.Key> IHasKeys.Keys
    {
        get => keys?.Cast<CGameCtnMediaBlock.Key>() ?? Enumerable.Empty<CGameCtnMediaBlock.Key>();
        set => keys = value.Cast<Key>().ToList();
    }

    TimeSingle IHasTwoKeys.Start
    {
        get => start.GetValueOrDefault();
        set => start = value;
    }

    TimeSingle IHasTwoKeys.End
    {
        get => end.GetValueOrDefault(start.GetValueOrDefault() + TimeSingle.FromSeconds(3));
        set => end = value;
    }

    [NodeMember(ExactName = "EntRecordData")]
    [AppliedWithChunk<Chunk0329F000>]
    public CPlugEntRecordData RecordData { get => recordData; set => recordData = value; }

    [NodeMember]
    [AppliedWithChunk<Chunk0329F000>(sinceVersion: 0, upToVersion: 3)]
    public TimeSingle? Start { get => start; set => start = value; }

    [NodeMember]
    [AppliedWithChunk<Chunk0329F000>(sinceVersion: 0, upToVersion: 3)]
    public TimeSingle? End { get => end; set => end = value; }

    [NodeMember]
    [AppliedWithChunk<Chunk0329F000>(sinceVersion: 4)]
    public IList<Key>? Keys { get => keys; set => keys = value; }

    [NodeMember]
    [AppliedWithChunk<Chunk0329F000>]
    public TimeSingle StartOffset { get => startOffset; set => startOffset = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk<Chunk0329F000>]
    public int[] NoticeRecords { get => noticeRecords; set => noticeRecords = value; }

    [NodeMember]
    [AppliedWithChunk<Chunk0329F000>(sinceVersion: 2)]
    public bool NoDamage { get => noDamage; set => noDamage = value; }

    [NodeMember]
    [AppliedWithChunk<Chunk0329F000>(sinceVersion: 2)]
    public bool ForceLight { get => forceLight; set => forceLight = value; }

    [NodeMember]
    [AppliedWithChunk<Chunk0329F000>(sinceVersion: 2)]
    public bool ForceHue { get => forceHue; set => forceHue = value; }
    
    [NodeMember]
    [AppliedWithChunk<Chunk0329F000>(sinceVersion: 2)]
    public Vec3 LightTrailColor { get => lightTrailColor; set => lightTrailColor = value; }

    [NodeMember]
    [AppliedWithChunk<Chunk0329F000>(sinceVersion: 3)]
    public Ident? PlayerModel { get => playerModel; set => playerModel = value; }

    [NodeMember]
    [AppliedWithChunk<Chunk0329F000>(sinceVersion: 3)]
    public IList<FileRef> SkinNames { get => skinNames; set => skinNames = value; }

    [NodeMember]
    [AppliedWithChunk<Chunk0329F000>(sinceVersion: 3)]
    public bool HasBadges { get => hasBadges; set => hasBadges = value; }

    [NodeMember]
    [AppliedWithChunk<Chunk0329F000>(sinceVersion: 3)]
    public int? BadgeVersion { get => badgeVersion; set => badgeVersion = value; }
    
    [NodeMember]
    [AppliedWithChunk<Chunk0329F000>(sinceVersion: 3)]
    public Badge? Badge { get => badge; set => badge = value; }

    [NodeMember]
    [AppliedWithChunk<Chunk0329F002>]
    public string? SkinOptions { get => skinOptions; set => skinOptions = value; }

    [NodeMember]
    [AppliedWithChunk<Chunk0329F000>(sinceVersion: 7)]
    public string? GhostName { get => ghostName; set => ghostName = value; }

    internal CGameCtnMediaBlockEntity()
    {
        recordData = null!;
    }

    public static CGameCtnMediaBlockEntityBuilder Create(CPlugEntRecordData recordData) => new(recordData);

    #region Chunks

    #region 0x000 chunk

    /// <summary>
    /// CGameCtnMediaBlockEntity 0x000 chunk
    /// </summary>
    [Chunk(0x0329F000)]
    public class Chunk0329F000 : Chunk<CGameCtnMediaBlockEntity>, IVersionable
    {
        private int version;

        public bool? U04;
        public Vec3? U09;
        public float U10 = -1;
        public string? U11;
        public int? U12;

        public int Version { get => version; set => version = value; }

        public override void ReadWrite(CGameCtnMediaBlockEntity n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref version);
            rw.NodeRef<CPlugEntRecordData>(ref n.recordData!);

            if (version < 4)
            {
                rw.TimeSingle(ref n.start); // according to getters setters
                rw.TimeSingle(ref n.end); // according to getters setters
            }
            
            rw.TimeSingle(ref n.startOffset);
            rw.Array<int>(ref n.noticeRecords!); // SPlugEntRecord array

            if (version >= 2)
            {
                rw.Boolean(ref n.noDamage); // ComputeDamageStatesFromDamageZoneAmounts?
                rw.Boolean(ref U04);
                rw.Boolean(ref n.forceLight);
                rw.Boolean(ref n.forceHue);

                if (version < 6)
                {
                    rw.Vec3(ref n.lightTrailColor);
                }
                
                if (version >= 3)
                {
                    // SGamePlayerMobilAppearanceParams::Archive
                    rw.Ident(ref n.playerModel);
                    rw.Vec3(ref U09); // some rgb, new light trail color?
                    rw.ListFileRef(ref n.skinNames!); // Name assumed from getter
                    rw.Boolean(ref n.hasBadges);

                    if (n.hasBadges)
                    {
                        rw.Int32(ref n.badgeVersion);
                        rw.Archive<Badge>(ref n.badge, n.badgeVersion.GetValueOrDefault()); // NGameBadge::BadgeArchive
                    }
                    //

                    if (version >= 4)
                    {
                        rw.ListKey<Key>(ref n.keys, version);

                        if (version == 5)
                        {
                            rw.Single(ref U10);
                        }

                        if (version >= 7)
                        {
                            rw.String(ref n.ghostName);

                            if (version >= 8)
                            {
                                rw.Int32(ref U12);

                                if (version >= 10)
                                {
                                    throw new ChunkVersionNotSupportedException(version);
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    #endregion

    #region 0x002 chunk (SkinOptions)

    /// <summary>
    /// CGameCtnMediaBlockEntity 0x002 chunk (SkinOptions)
    /// </summary>
    [Chunk(0x0329F002, "SkinOptions")]
    public class Chunk0329F002 : Chunk<CGameCtnMediaBlockEntity>
    {
        public override void ReadWrite(CGameCtnMediaBlockEntity n, GameBoxReaderWriter rw)
        {
            rw.String(ref n.skinOptions);
        }
    }

    #endregion

    #endregion
}
