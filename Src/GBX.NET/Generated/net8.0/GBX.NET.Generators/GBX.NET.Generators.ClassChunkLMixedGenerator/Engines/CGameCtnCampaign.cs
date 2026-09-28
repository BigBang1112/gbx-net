namespace GBX.NET.Engines.Game;

/// <remarks>ID: 0x03090000</remarks>
[Class(0x03090000)]
public partial class CGameCtnCampaign : CMwNod, IClass
{
    [Hexadecimal] public static new uint Id => 0x03090000;




    private List<CGameCtnChallengeGroup>? challengeGroups;
    [AppliedWithChunk<Chunk03090000>]
    public List<CGameCtnChallengeGroup>? ChallengeGroups { get => challengeGroups; set => challengeGroups = value; }

    private string? iconId;
    [AppliedWithChunk<Chunk03090002>]
    [AppliedWithChunk<Chunk0309000C>]
    public string? IconId { get => iconId; set => iconId = value; }

    private string? name;
    [AppliedWithChunk<Chunk03090003>]
    [AppliedWithChunk<Chunk03090004>]
    [AppliedWithChunk<Chunk0309000F>]
    public string? Name { get => name; set => name = value; }

    private EType type;
    [AppliedWithChunk<Chunk03090003>]
    [AppliedWithChunk<Chunk03090004>]
    [AppliedWithChunk<Chunk0309000F>]
    public EType Type { get => type; set => type = value; }

    private bool isInternal;
    [AppliedWithChunk<Chunk03090003>]
    [AppliedWithChunk<Chunk03090004>]
    public bool IsInternal { get => isInternal; set => isInternal = value; }

    private string? campaignId;
    [AppliedWithChunk<Chunk03090006>]
    public string? CampaignId { get => campaignId; set => campaignId = value; }

    private int index;
    [AppliedWithChunk<Chunk0309000A>]
    public int Index { get => index; set => index = value; }

    private string? unlockedByCampaign;
    [AppliedWithChunk<Chunk0309000B>]
    public string? UnlockedByCampaign { get => unlockedByCampaign; set => unlockedByCampaign = value; }

    private EUnlockType unlockType;
    [AppliedWithChunk<Chunk0309000D>]
    public EUnlockType UnlockType { get => unlockType; set => unlockType = value; }

    private ERequiredPlayersCount requiredPlayersCount;
    [AppliedWithChunk<Chunk0309000E>]
    public ERequiredPlayersCount RequiredPlayersCount { get => requiredPlayersCount; set => requiredPlayersCount = value; }

    private string? modeScriptName;
    [AppliedWithChunk<Chunk03090010>]
    public string? ModeScriptName { get => modeScriptName; set => modeScriptName = value; }

    private string? scoreContext;
    [AppliedWithChunk<Chunk03090012>]
    public string? ScoreContext { get => scoreContext; set => scoreContext = value; }

    private bool officialRecordEnabled;
    [AppliedWithChunk<Chunk03090012>]
    public bool OfficialRecordEnabled { get => officialRecordEnabled; set => officialRecordEnabled = value; }


    /// <summary>
    /// CGameCtnCampaign 0x000 chunk (map groups)
    /// </summary>
    [Chunk(0x03090000, "map groups")]
    public partial class Chunk03090000 : Chunk<CGameCtnCampaign>
    {
        /// <inheritdoc />
        public override uint Id => 0x03090000;


        public override void ReadWrite(CGameCtnCampaign n, GbxReaderWriter rw)
        {
            rw.ListNodeRef_deprec<CGameCtnChallengeGroup>(ref n.challengeGroups!);
        }
    }

    /// <summary>
    /// CGameCtnCampaign 0x001 chunk
    /// </summary>
    [Chunk(0x03090001)]
    public partial class Chunk03090001 : Chunk<CGameCtnCampaign>
    {
        /// <inheritdoc />
        public override uint Id => 0x03090001;

        public int U01;

        public override void ReadWrite(CGameCtnCampaign n, GbxReaderWriter rw)
        {
            n.CollectionId = rw.Id(n.CollectionId);
            rw.Int32(ref U01);
            if (U01>0)
            {
                throw new ("");
            }
        }
    }

    /// <summary>
    /// CGameCtnCampaign 0x002 chunk
    /// </summary>
    [Chunk(0x03090002)]
    public partial class Chunk03090002 : Chunk<CGameCtnCampaign>
    {
        /// <inheritdoc />
        public override uint Id => 0x03090002;

        public int U01;

        public override void ReadWrite(CGameCtnCampaign n, GbxReaderWriter rw)
        {
            rw.Id(ref n.iconId);
            rw.Int32(ref U01);
            rw.Int32(ref U01);
            rw.Int32(ref U01);
            rw.Int32(ref U01);
            rw.Int32(ref U01);
            rw.Int32(ref U01);
        }
    }

    /// <summary>
    /// CGameCtnCampaign 0x003 chunk
    /// </summary>
    [Chunk(0x03090003)]
    public partial class Chunk03090003 : Chunk<CGameCtnCampaign>
    {
        /// <inheritdoc />
        public override uint Id => 0x03090003;

        public bool U01;

        public override void ReadWrite(CGameCtnCampaign n, GbxReaderWriter rw)
        {
            rw.String(ref n.name);
            rw.EnumInt32<EType>(ref n.type);
            rw.Boolean(ref n.isInternal);
            rw.Boolean(ref U01);
        }
    }

    /// <summary>
    /// CGameCtnCampaign 0x004 chunk
    /// </summary>
    [Chunk(0x03090004)]
    public partial class Chunk03090004 : Chunk<CGameCtnCampaign>
    {
        /// <inheritdoc />
        public override uint Id => 0x03090004;

        public bool U01;
        public bool U02;

        public override void ReadWrite(CGameCtnCampaign n, GbxReaderWriter rw)
        {
            rw.String(ref n.name);
            rw.EnumInt32<EType>(ref n.type);
            rw.Boolean(ref n.isInternal);
            rw.Boolean(ref U01);
            rw.Boolean(ref U02);
        }
    }

    /// <summary>
    /// CGameCtnCampaign 0x005 chunk
    /// </summary>
    [Chunk(0x03090005)]
    public partial class Chunk03090005 : Chunk<CGameCtnCampaign>
    {
        /// <inheritdoc />
        public override uint Id => 0x03090005;

        public bool U01;

        public override void ReadWrite(CGameCtnCampaign n, GbxReaderWriter rw)
        {
            rw.Boolean(ref U01);
        }
    }

    /// <summary>
    /// CGameCtnCampaign 0x006 chunk (campaign ID)
    /// </summary>
    [Chunk(0x03090006, "campaign ID")]
    public partial class Chunk03090006 : Chunk<CGameCtnCampaign>
    {
        /// <inheritdoc />
        public override uint Id => 0x03090006;


        public override void ReadWrite(CGameCtnCampaign n, GbxReaderWriter rw)
        {
            rw.Id(ref n.campaignId);
        }
    }

    /// <summary>
    /// CGameCtnCampaign 0x007 chunk
    /// </summary>
    [Chunk(0x03090007)]
    public partial class Chunk03090007 : Chunk<CGameCtnCampaign>
    {
        /// <inheritdoc />
        public override uint Id => 0x03090007;

        public int U01;

        public override void ReadWrite(CGameCtnCampaign n, GbxReaderWriter rw)
        {
            rw.Int32(ref U01);
            rw.Int32(ref U01);
            rw.Int32(ref U01);
        }
    }

    /// <summary>
    /// CGameCtnCampaign 0x008 chunk
    /// </summary>
    [Chunk(0x03090008)]
    public partial class Chunk03090008 : Chunk<CGameCtnCampaign>
    {
        /// <inheritdoc />
        public override uint Id => 0x03090008;

        public int U01;

        public override void ReadWrite(CGameCtnCampaign n, GbxReaderWriter rw)
        {
            rw.Int32(ref U01);
        }
    }

    /// <summary>
    /// CGameCtnCampaign 0x009 skippable chunk (collection ID)
    /// </summary>
    [Chunk(0x03090009, "collection ID")]
    public partial class Chunk03090009 : SkippableChunk<CGameCtnCampaign>
    {
        /// <inheritdoc />
        public override uint Id => 0x03090009;

        public byte U01;

        public override void ReadWrite(CGameCtnCampaign n, GbxReaderWriter rw)
        {
            rw.Byte(ref U01);
            n.CollectionId = rw.Id(n.CollectionId);
        }
    }

    /// <summary>
    /// CGameCtnCampaign 0x00A skippable chunk (index)
    /// </summary>
    [Chunk(0x0309000A, "index")]
    public partial class Chunk0309000A : SkippableChunk<CGameCtnCampaign>
    {
        /// <inheritdoc />
        public override uint Id => 0x0309000A;


        public override void ReadWrite(CGameCtnCampaign n, GbxReaderWriter rw)
        {
            rw.Int32(ref n.index);
        }
    }

    /// <summary>
    /// CGameCtnCampaign 0x00B skippable chunk (unlocked by campaign)
    /// </summary>
    [Chunk(0x0309000B, "unlocked by campaign")]
    public partial class Chunk0309000B : SkippableChunk<CGameCtnCampaign>
    {
        /// <inheritdoc />
        public override uint Id => 0x0309000B;

        public int U01;

        public override void ReadWrite(CGameCtnCampaign n, GbxReaderWriter rw)
        {
            rw.String(ref n.unlockedByCampaign);
            rw.Int32(ref U01);
        }
    }

    /// <summary>
    /// CGameCtnCampaign 0x00C skippable chunk (icon ID)
    /// </summary>
    [Chunk(0x0309000C, "icon ID")]
    public partial class Chunk0309000C : SkippableChunk<CGameCtnCampaign>
    {
        /// <inheritdoc />
        public override uint Id => 0x0309000C;


        public override void ReadWrite(CGameCtnCampaign n, GbxReaderWriter rw)
        {
            rw.Id(ref n.iconId);
        }
    }

    /// <summary>
    /// CGameCtnCampaign 0x00D chunk (unlock type)
    /// </summary>
    [Chunk(0x0309000D, "unlock type")]
    public partial class Chunk0309000D : Chunk<CGameCtnCampaign>
    {
        /// <inheritdoc />
        public override uint Id => 0x0309000D;


        public override void ReadWrite(CGameCtnCampaign n, GbxReaderWriter rw)
        {
            rw.EnumInt32<EUnlockType>(ref n.unlockType);
        }
    }

    /// <summary>
    /// CGameCtnCampaign 0x00E chunk (required players count)
    /// </summary>
    [Chunk(0x0309000E, "required players count")]
    public partial class Chunk0309000E : Chunk<CGameCtnCampaign>
    {
        /// <inheritdoc />
        public override uint Id => 0x0309000E;


        public override void ReadWrite(CGameCtnCampaign n, GbxReaderWriter rw)
        {
            rw.EnumInt32<ERequiredPlayersCount>(ref n.requiredPlayersCount);
        }
    }

    /// <summary>
    /// CGameCtnCampaign 0x00F skippable chunk (name)
    /// </summary>
    [Chunk(0x0309000F, "name")]
    public partial class Chunk0309000F : SkippableChunk<CGameCtnCampaign>
    {
        /// <inheritdoc />
        public override uint Id => 0x0309000F;

        public int U01;

        public override void ReadWrite(CGameCtnCampaign n, GbxReaderWriter rw)
        {
            rw.String(ref n.name);
            rw.Int32(ref U01);
            rw.EnumInt32<EType>(ref n.type);
        }
    }

    /// <summary>
    /// CGameCtnCampaign 0x010 chunk (mode script name)
    /// </summary>
    [Chunk(0x03090010, "mode script name")]
    public partial class Chunk03090010 : Chunk<CGameCtnCampaign>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x03090010;

        public int Version { get; set; }


        public override void ReadWrite(CGameCtnCampaign n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.String(ref n.modeScriptName);
        }
    }

    /// <summary>
    /// CGameCtnCampaign 0x012 skippable chunk (official mode)
    /// </summary>
    [Chunk(0x03090012, "official mode")]
    public partial class Chunk03090012 : SkippableChunk<CGameCtnCampaign>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x03090012;

        public int Version { get; set; }

        public int? U01;
        public string? U02;
        public string? U03;
        public string? U04;
        public bool? U05;
        public bool? U06;

        public override void ReadWrite(CGameCtnCampaign n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            if (Version <= 1)
            {
                rw.String(ref n.scoreContext);
            }
            if (Version >= 2)
            {
                rw.Int32(ref U01);
                rw.String(ref U02);
                rw.String(ref U03);
                rw.String(ref U04);
            }
            rw.Boolean(ref n.officialRecordEnabled);
            if (Version == 0)
            {
                rw.Boolean(ref U05);
                rw.Boolean(ref U06);
            }
        }
    }



    public enum EUnlockType
    {
        ByRow,
        ByColumn,
        Custom,
    }

    public enum ERequiredPlayersCount
    {
        SoloOnly,
        MultiOnly,
        DuoOnly,
        TrioOnly,
        QuatuorOnly,
        All,
    }

    public enum EType
    {
        None,
        Race,
        Puzzle,
        Survival,
        Platform,
        Stunts,
        Training,
    }


    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x03090000 => new Chunk03090000(),
        0x03090001 => new Chunk03090001(),
        0x03090002 => new Chunk03090002(),
        0x03090003 => new Chunk03090003(),
        0x03090004 => new Chunk03090004(),
        0x03090005 => new Chunk03090005(),
        0x03090006 => new Chunk03090006(),
        0x03090007 => new Chunk03090007(),
        0x03090008 => new Chunk03090008(),
        0x03090009 => new Chunk03090009(),
        0x0309000A => new Chunk0309000A(),
        0x0309000B => new Chunk0309000B(),
        0x0309000C => new Chunk0309000C(),
        0x0309000D => new Chunk0309000D(),
        0x0309000E => new Chunk0309000E(),
        0x0309000F => new Chunk0309000F(),
        0x03090010 => new Chunk03090010(),
        0x03090012 => new Chunk03090012(),
        _ => base.NewChunk(chunkId),
    };
}
