namespace GBX.NET.Engines.Game;

/// <summary>
/// Campaign info (0x03090000)
/// </summary>
/// <remarks>Information about a campaign.</remarks>
[Node(0x03090000)]
public class CGameCtnCampaign : CMwNod
{
    #region Fields

    private CGameCtnChallengeGroup[] mapGroups;
    private string? campaignID;
    private int index;
    private string? name;
    private int type;
    private int unlockType;

    #endregion

    #region Properties

    public CGameCtnChallengeGroup[] MapGroups
    {
        get => mapGroups;
        set => mapGroups = value;
    }

    public string? CampaignID
    {
        get => campaignID;
        set => campaignID = value;
    }

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

    public int Type
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

    public int UnlockType
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

    #endregion

    #region Constructors

    protected CGameCtnCampaign()
    {
        mapGroups = null!;
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

        public int Version
        {
            get => version;
            set => version = value;
        }

        public override void ReadWrite(CGameCtnCampaign n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref version);
            rw.ArrayNode(ref n.mapGroups!);
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
            rw.Id(ref n.campaignID);
        }
    }

    #endregion

    #region 0x009 skippable chunk

    /// <summary>
    /// CGameCtnCampaign 0x009 skippable chunk
    /// </summary>
    [Chunk(0x03090009)]
    public class Chunk03090009 : SkippableChunk<CGameCtnCampaign>
    {
        public byte U01;
        public int U02;

        public override void ReadWrite(CGameCtnCampaign n, GameBoxReaderWriter rw)
        {
            rw.Byte(ref U01);
            rw.Int32(ref U02);
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

    #region 0x00D chunk

    /// <summary>
    /// CGameCtnCampaign 0x00D chunk
    /// </summary>
    [Chunk(0x0309000D)]
    public class Chunk0309000D : Chunk<CGameCtnCampaign>
    {
        public int U01;

        public override void ReadWrite(CGameCtnCampaign n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref U01);
        }
    }

    #endregion

    #region 0x00E chunk

    /// <summary>
    /// CGameCtnCampaign 0x00E chunk
    /// </summary>
    [Chunk(0x0309000E)]
    public class Chunk0309000E : Chunk<CGameCtnCampaign>
    {
        public int U01;

        public override void ReadWrite(CGameCtnCampaign n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref U01);
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
        public override void ReadWrite(CGameCtnCampaign n, GameBoxReaderWriter rw)
        {
            rw.String(ref n.name);
            rw.Int32(ref n.type);
            rw.Int32(ref n.unlockType);
        }
    }

    #endregion

    #region 0x010 chunk

    /// <summary>
    /// CGameCtnCampaign 0x010 chunk
    /// </summary>
    [Chunk(0x03090010)]
    public class Chunk03090010 : Chunk<CGameCtnCampaign>
    {
        public int U01;
        public int U02;

        public override void ReadWrite(CGameCtnCampaign n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref U01);
            rw.Int32(ref U02);
        }
    }

    #endregion

    #endregion
}
