namespace GBX.NET.Engines.Game;

/// <summary>
/// Campaign map list and other information about a campaign.
/// </summary>
/// <remarks>ID: 0x03090000</remarks>
[Node(0x03090000)]
[NodeExtension("GameCtnCampaign")]
[NodeExtension("ConstructionCampaign")]
[NodeExtension("Campaign")]
public class CGameCtnCampaign : CMwNod
{
    #region Enums

    public enum EType
    {
        None,
        Race,
        Puzzle,
        Survival,
        Platform,
        Stunts,
        Training
    }

    public enum ERequiredPlayersCount
    {
        SoloOnly,
        MultiOnly,
        DuoOnly,
        TrioOnly,
        QuatuorOnly,
        All
    }

    public enum EUnlockType
    {
        ByRow,
        ByColumn,
        Custom
    }

    #endregion

    #region Fields

    private CGameCtnChallengeGroup[] mapGroups;
    private string? campaignId;
    private int index;
    private string? name;
    private EType type;
    private string? iconId;
    private EUnlockType unlockType;
    private string? collectionId;
    private ERequiredPlayersCount requiredPlayersCount;
    private string? modeScriptName;
    private string? unlockedByCampaign;
    private string? scoreContext;
    private bool officialRecordEnabled;

    #endregion

    #region Properties

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk(typeof(Chunk03090000))]
    public CGameCtnChallengeGroup[] MapGroups { get => mapGroups; set => mapGroups = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk(typeof(Chunk03090006))]
    public string? CampaignId { get => campaignId; set => campaignId = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk(typeof(Chunk03090009))]
    public string? CollectionId
    {
        get
        {
            DiscoverChunk<Chunk03090009>();
            return collectionId;
        }
        set
        {
            DiscoverChunk<Chunk03090009>();
            collectionId = value;
        }
    }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk(typeof(Chunk0309000A))]
    public int Index
    {
        get
        {
            DiscoverChunk<Chunk0309000A>();
            return index;
        }
        set
        {
            DiscoverChunk<Chunk0309000A>();
            index = value;
        }
    }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk(typeof(Chunk0309000B))]
    public string? UnlockedByCampaign
    {
        get
        {
            DiscoverChunk<Chunk0309000B>();
            return unlockedByCampaign;
        }
        set
        {
            DiscoverChunk<Chunk0309000B>();
            unlockedByCampaign = value;
        }
    }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk(typeof(Chunk0309000C))]
    public string? IconId
    {
        get
        {
            DiscoverChunk<Chunk0309000C>();
            return iconId;
        }
        set
        {
            DiscoverChunk<Chunk0309000C>();
            iconId = value;
        }
    }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk(typeof(Chunk0309000E))]
    public ERequiredPlayersCount RequiredPlayersCount { get => requiredPlayersCount; set => requiredPlayersCount = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk(typeof(Chunk03090004))]
    [AppliedWithChunk(typeof(Chunk0309000F))]
    public string? Name
    {
        get
        {
            DiscoverChunk<Chunk0309000F>();
            return name;
        }
        set
        {
            DiscoverChunk<Chunk0309000F>();
            name = value;
        }
    }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk(typeof(Chunk03090004))]
    [AppliedWithChunk(typeof(Chunk0309000F))]
    public EType Type
    {
        get
        {
            DiscoverChunk<Chunk0309000F>();
            return type;
        }
        set
        {
            DiscoverChunk<Chunk0309000F>();
            type = value;
        }
    }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk(typeof(Chunk0309000D))]
    [AppliedWithChunk(typeof(Chunk0309000F))]
    public EUnlockType UnlockType
    {
        get
        {
            DiscoverChunk<Chunk0309000F>();
            return unlockType;
        }
        set
        {
            DiscoverChunk<Chunk0309000F>();
            unlockType = value;
        }
    }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk(typeof(Chunk03090010))]
    public string? ModeScriptName { get => modeScriptName; set => modeScriptName = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk(typeof(Chunk03090012))]
    public string? ScoreContext
    {
        get
        {
            DiscoverChunk<Chunk03090012>();
            return scoreContext;
        }
        set
        {
            DiscoverChunk<Chunk03090012>();
            scoreContext = value;
        }
    }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk(typeof(Chunk03090012))]
    public bool OfficialRecordEnabled
    {
        get
        {
            DiscoverChunk<Chunk03090012>();
            return officialRecordEnabled;
        }
        set
        {
            DiscoverChunk<Chunk03090012>();
            officialRecordEnabled = value;
        }
    }

    #endregion

    #region Constructors

    protected CGameCtnCampaign()
    {
        mapGroups = Array.Empty<CGameCtnChallengeGroup>();
    }

    #endregion

    #region Chunks

    #region 0x000 chunk (map groups)

    /// <summary>
    /// CGameCtnCampaign 0x000 chunk (map groups)
    /// </summary>
    [Chunk(0x03090000, "map groups")]
    public class Chunk03090000 : Chunk<CGameCtnCampaign>, IVersionable
    {
        private int version;

        public int Version { get => version; set => version = value; }

        public override void ReadWrite(CGameCtnCampaign n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref version);
            rw.ArrayNode(ref n.mapGroups!);
        }
    }

    #endregion

    #region 0x001 chunk

    /// <summary>
    /// CGameCtnCampaign 0x001 chunk
    /// </summary>
    [Chunk(0x03090001)]
    public class Chunk03090001 : Chunk<CGameCtnCampaign>, IVersionable
    {
        private int version;

        public int U01;

        public int Version { get => version; set => version = value; }

        public override void ReadWrite(CGameCtnCampaign n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref version);
            rw.Int32(ref U01);

            if (U01 > 0)
            {
                throw new Exception("U01 > 0");
            }
        }
    }

    #endregion

    #region 0x002 chunk

    /// <summary>
    /// CGameCtnCampaign 0x002 chunk
    /// </summary>
    [Chunk(0x03090002)]
    public class Chunk03090002 : Chunk<CGameCtnCampaign>
    {
        public int U01;

        public override void ReadWrite(CGameCtnCampaign n, GameBoxReaderWriter rw)
        {
            for (var i = 0; i < 6; i++)
            {
                rw.Int32(ref U01);
            }
        }
    }

    #endregion

    #region 0x004 chunk

    /// <summary>
    /// CGameCtnCampaign 0x004 chunk
    /// </summary>
    [Chunk(0x03090004)]
    public class Chunk03090004 : Chunk<CGameCtnCampaign>
    {
        public bool U01;
        public bool U02;
        public bool U03;

        public override void ReadWrite(CGameCtnCampaign n, GameBoxReaderWriter rw)
        {
            rw.String(ref n.name);
            rw.EnumInt32<EType>(ref n.type);

            rw.Boolean(ref U01);
            rw.Boolean(ref U02);
            rw.Boolean(ref U03);
        }
    }

    #endregion

    #region 0x005 chunk

    /// <summary>
    /// CGameCtnCampaign 0x005 chunk
    /// </summary>
    [Chunk(0x03090005)]
    public class Chunk03090005 : Chunk<CGameCtnCampaign>
    {
        public bool U01;

        public override void ReadWrite(CGameCtnCampaign n, GameBoxReaderWriter rw)
        {
            rw.Boolean(ref U01);
        }
    }

    #endregion

    #region 0x006 chunk (campaign ID)

    /// <summary>
    /// CGameCtnCampaign 0x006 chunk (campaign ID)
    /// </summary>
    [Chunk(0x03090006, "campaign ID")]
    public class Chunk03090006 : Chunk<CGameCtnCampaign>
    {
        public override void ReadWrite(CGameCtnCampaign n, GameBoxReaderWriter rw)
        {
            rw.Id(ref n.campaignId);
        }
    }

    #endregion

    #region 0x007 chunk

    /// <summary>
    /// CGameCtnCampaign 0x007 chunk
    /// </summary>
    [Chunk(0x03090007)]
    public class Chunk03090007 : Chunk<CGameCtnCampaign>
    {
        public int U01;

        public override void ReadWrite(CGameCtnCampaign n, GameBoxReaderWriter rw)
        {
            for (var i = 0; i < 3; i++)
            {
                rw.Int32(ref U01);
            }
        }
    }

    #endregion

    #region 0x008 chunk

    /// <summary>
    /// CGameCtnCampaign 0x008 chunk
    /// </summary>
    [Chunk(0x03090008)]
    public class Chunk03090008 : Chunk<CGameCtnCampaign>
    {
        public int U01;

        public override void ReadWrite(CGameCtnCampaign n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref U01);
        }
    }

    #endregion

    #region 0x009 skippable chunk (collection ID)

    /// <summary>
    /// CGameCtnCampaign 0x009 skippable chunk (collection ID)
    /// </summary>
    [Chunk(0x03090009, "collection ID")]
    public class Chunk03090009 : SkippableChunk<CGameCtnCampaign>
    {
        public byte U01;

        public override void ReadWrite(CGameCtnCampaign n, GameBoxReaderWriter rw)
        {
            rw.Byte(ref U01);
            rw.Id(ref n.collectionId);
        }
    }

    #endregion

    #region 0x00A skippable chunk (index)

    /// <summary>
    /// CGameCtnCampaign 0x00A skippable chunk (index)
    /// </summary>
    [Chunk(0x0309000A, "index")]
    public class Chunk0309000A : SkippableChunk<CGameCtnCampaign>
    {
        public override void ReadWrite(CGameCtnCampaign n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref n.index);
        }
    }

    #endregion

    #region 0x00B skippable chunk (unlocked by campaign)

    /// <summary>
    /// CGameCtnCampaign 0x00B skippable chunk (unlocked by campaign)
    /// </summary>
    [Chunk(0x0309000B, "unlocked by campaign")]
    public class Chunk0309000B : SkippableChunk<CGameCtnCampaign>
    {
        public int U01;

        public override void ReadWrite(CGameCtnCampaign n, GameBoxReaderWriter rw)
        {
            rw.String(ref n.unlockedByCampaign);
            rw.Int32(ref U01);
        }
    }

    #endregion

    #region 0x00C skippable chunk (icon ID)

    /// <summary>
    /// CGameCtnCampaign 0x00C skippable chunk (icon ID)
    /// </summary>
    [Chunk(0x0309000C, "icon ID")]
    public class Chunk0309000C : SkippableChunk<CGameCtnCampaign>
    {
        public override void ReadWrite(CGameCtnCampaign n, GameBoxReaderWriter rw)
        {
            rw.Id(ref n.iconId);
        }
    }

    #endregion

    #region 0x00D chunk (unlock type)

    /// <summary>
    /// CGameCtnCampaign 0x00D chunk (unlock type)
    /// </summary>
    [Chunk(0x0309000D, "unlock type")]
    public class Chunk0309000D : Chunk<CGameCtnCampaign>
    {
        public override void ReadWrite(CGameCtnCampaign n, GameBoxReaderWriter rw)
        {
            rw.EnumInt32<EUnlockType>(ref n.unlockType);
        }
    }

    #endregion

    #region 0x00E chunk (required players count)

    /// <summary>
    /// CGameCtnCampaign 0x00E chunk (required players count)
    /// </summary>
    [Chunk(0x0309000E, "required players count")]
    public class Chunk0309000E : Chunk<CGameCtnCampaign>
    {
        public override void ReadWrite(CGameCtnCampaign n, GameBoxReaderWriter rw)
        {
            rw.EnumInt32<ERequiredPlayersCount>(ref n.requiredPlayersCount);
        }
    }

    #endregion

    #region 0x00F skippable chunk (name)

    /// <summary>
    /// CGameCtnCampaign 0x00F skippable chunk (name)
    /// </summary>
    [Chunk(0x0309000F, "name")]
    public class Chunk0309000F : SkippableChunk<CGameCtnCampaign>
    {
        public int U01;

        public override void ReadWrite(CGameCtnCampaign n, GameBoxReaderWriter rw)
        {
            rw.String(ref n.name);
            rw.Int32(ref U01); // Not seen in code
            rw.EnumInt32<EType>(ref n.type);
        }
    }

    #endregion

    #region 0x010 chunk (mode script name)

    /// <summary>
    /// CGameCtnCampaign 0x010 chunk (mode script name)
    /// </summary>
    [Chunk(0x03090010, "mode script name")]
    public class Chunk03090010 : Chunk<CGameCtnCampaign>, IVersionable
    {
        private int version = 0;

        public int Version { get => version; set => version = value; }

        public override void ReadWrite(CGameCtnCampaign n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref version);
            rw.String(ref n.modeScriptName);
        }
    }

    #endregion

    #region 0x012 skippable chunk (official mode)

    /// <summary>
    /// CGameCtnCampaign 0x012 skippable chunk (official mode)
    /// </summary>
    [Chunk(0x03090012, "official mode")]
    public class Chunk03090012 : SkippableChunk<CGameCtnCampaign>, IVersionable
    {
        private int version = 2;

        public bool? U01;
        public bool? U02;

        public int? U03;
        public string? U04;
        public string? U05;
        public string? U06;

        public int Version { get => version; set => version = value; }

        public override void ReadWrite(CGameCtnCampaign n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref version);
            
            if (version < 2)
            {
                rw.String(ref n.scoreContext);
            }
            else // some weird Archive
            {
                rw.Int32(ref U03);
                rw.String(ref U04);
                rw.String(ref U05);
                rw.String(ref U06);
            }

            rw.Boolean(ref n.officialRecordEnabled);
            
            if (version == 0)
            {
                rw.Boolean(ref U01);
                rw.Boolean(ref U02);
            }
        }
    }

    #endregion

    #endregion
}
