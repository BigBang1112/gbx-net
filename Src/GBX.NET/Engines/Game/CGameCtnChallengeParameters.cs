namespace GBX.NET.Engines.Game;

/// <summary>
/// Map parameters.
/// </summary>
/// <remarks>ID: 0x0305B000</remarks>
[Node(0x0305B000)]
public class CGameCtnChallengeParameters : CMwNod
{
    #region Fields

    private string tip;
    private TimeInt32? bronzeTime;
    private TimeInt32? silverTime;
    private TimeInt32? goldTime;
    private TimeInt32? authorTime;
    private int? authorScore;
    private TimeInt32 timeLimit = TimeInt32.FromMinutes(1);
    private CGameCtnGhost? raceValidateGhost;
    private string? mapType;
    private string? mapStyle;
    private bool isValidatedForScriptModes;

    #endregion

    #region Properties

    [NodeMember]
    public string Tip
    {
        get => tip;
        set => tip = value;
    }

    /// <summary>
    /// Time of the bronze medal.
    /// </summary>
    [NodeMember]
    public TimeInt32? BronzeTime
    {
        get
        {
            DiscoverChunk<Chunk0305B00A>();
            return bronzeTime;
        }
        set
        {
            DiscoverChunk<Chunk0305B00A>();
            bronzeTime = value;
        }
    }

    /// <summary>
    /// Time of the silver medal.
    /// </summary>
    [NodeMember]
    public TimeInt32? SilverTime
    {
        get
        {
            DiscoverChunk<Chunk0305B00A>();
            return silverTime;
        }
        set
        {
            DiscoverChunk<Chunk0305B00A>();
            silverTime = value;
        }
    }

    /// <summary>
    /// Time of the gold medal.
    /// </summary>
    [NodeMember]
    public TimeInt32? GoldTime
    {
        get
        {
            DiscoverChunk<Chunk0305B00A>();
            return goldTime;
        }
        set
        {
            DiscoverChunk<Chunk0305B00A>();
            goldTime = value;
        }
    }

    /// <summary>
    /// Time of the author medal.
    /// </summary>
    [NodeMember]
    public TimeInt32? AuthorTime
    {
        get
        {
            DiscoverChunk<Chunk0305B00A>();
            return authorTime;
        }
        set
        {
            DiscoverChunk<Chunk0305B00A>();
            authorTime = value;
        }
    }

    /// <summary>
    /// Usually author time or stunts score.
    /// </summary>
    [NodeMember]
    public int? AuthorScore
    {
        get => authorScore;
        set => authorScore = value;
    }

    /// <summary>
    /// Stunts time limit.
    /// </summary>
    [NodeMember]
    public TimeInt32 TimeLimit
    {
        get
        {
            DiscoverChunk<Chunk0305B00A>();
            return timeLimit;
        }
        set
        {
            DiscoverChunk<Chunk0305B00A>();
            timeLimit = value;
        }
    }

    [NodeMember]
    public CGameCtnGhost? RaceValidateGhost
    {
        get => raceValidateGhost;
        set => raceValidateGhost = value;
    }

    [NodeMember]
    public string? MapType
    {
        get
        {
            DiscoverChunk<Chunk0305B00E>();
            return mapType;
        }
        set
        {
            DiscoverChunk<Chunk0305B00E>();
            mapType = value;
        }
    }

    [NodeMember]
    public string? MapStyle
    {
        get
        {
            DiscoverChunk<Chunk0305B00E>();
            return mapStyle;
        }
        set
        {
            DiscoverChunk<Chunk0305B00E>();
            mapStyle = value;
        }
    }

    [NodeMember]
    public bool IsValidatedForScriptModes
    {
        get
        {
            DiscoverChunk<Chunk0305B00E>();
            return isValidatedForScriptModes;
        }
        set
        {
            DiscoverChunk<Chunk0305B00E>();
            isValidatedForScriptModes = value;
        }
    }

    #endregion

    #region Constructors

    protected CGameCtnChallengeParameters()
    {
        tip = null!;
    }

    #endregion

    #region Chunks

    #region 0x000 chunk

    /// <summary>
    /// CGameCtnChallengeParameters 0x000 chunk
    /// </summary>
    [Chunk(0x0305B000)]
    public class Chunk0305B000 : Chunk<CGameCtnChallengeParameters>
    {
        public int U01;
        public int U02;
        public int U03;
        public int U04;
        public int U05;
        public int U06;
        public int U07;
        public int U08;

        public override void ReadWrite(CGameCtnChallengeParameters n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref U01);
            rw.Int32(ref U02);
            rw.Int32(ref U03);
            rw.Int32(ref U04);

            rw.Int32(ref U05);
            rw.Int32(ref U06);
            rw.Int32(ref U07);

            rw.Int32(ref U08);
        }
    }

    #endregion

    #region 0x001 chunk (tips)

    /// <summary>
    /// CGameCtnChallengeParameters 0x001 chunk (tips)
    /// </summary>
    [Chunk(0x0305B001, "tips")]
    public class Chunk0305B001 : Chunk<CGameCtnChallengeParameters>
    {
        public override void ReadWrite(CGameCtnChallengeParameters n, GameBoxReaderWriter rw)
        {
            for (var i = 0; i < 4; i++)
                rw.String(ref n.tip!);
        }
    }

    #endregion

    #region 0x002 chunk

    /// <summary>
    /// CGameCtnChallengeParameters 0x002 chunk
    /// </summary>
    [Chunk(0x0305B002)]
    public class Chunk0305B002 : Chunk<CGameCtnChallengeParameters>
    {
        public int U01;
        public int U02;
        public int U03;
        public float U04;
        public float U05;
        public float U06;
        public int U07;
        public int U08;
        public int U09;
        public int U10;
        public int U11;
        public int U12;
        public int U13;
        public int U14;
        public int U15;
        public int U16;

        public override void ReadWrite(CGameCtnChallengeParameters n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref U01);
            rw.Int32(ref U02);
            rw.Int32(ref U03);

            rw.Single(ref U04);
            rw.Single(ref U05);
            rw.Single(ref U06);

            rw.Int32(ref U07);
            rw.Int32(ref U08);
            rw.Int32(ref U09);
            rw.Int32(ref U10);
            rw.Int32(ref U11);
            rw.Int32(ref U12);
            rw.Int32(ref U13);
            rw.Int32(ref U14);
            rw.Int32(ref U15);
            rw.Int32(ref U16);
        }
    }

    #endregion

    #region 0x003 chunk

    /// <summary>
    /// CGameCtnChallengeParameters 0x003 chunk
    /// </summary>
    [Chunk(0x0305B003)]
    public class Chunk0305B003 : Chunk<CGameCtnChallengeParameters>
    {
        private int U01;
        private float U02;
        private int U03;
        private int U04;
        private int U05;
        private int U06;

        public override void ReadWrite(CGameCtnChallengeParameters n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref U01);
            rw.Single(ref U02);

            rw.Int32(ref U03);
            rw.Int32(ref U04);
            rw.Int32(ref U05);

            rw.Int32(ref U06);
        }
    }

    #endregion

    #region 0x004 chunk (medals)

    /// <summary>
    /// CGameCtnChallengeParameters 0x004 chunk (medals)
    /// </summary>
    [Chunk(0x0305B004, "medals")]
    public class Chunk0305B004 : Chunk<CGameCtnChallengeParameters>
    {
        public uint U01;

        public override void ReadWrite(CGameCtnChallengeParameters n, GameBoxReaderWriter rw)
        {
            rw.TimeInt32Nullable(ref n.bronzeTime);
            rw.TimeInt32Nullable(ref n.silverTime);
            rw.TimeInt32Nullable(ref n.goldTime);
            rw.TimeInt32Nullable(ref n.authorTime);

            rw.UInt32(ref U01);
        }
    }

    #endregion

    #region 0x005 chunk

    /// <summary>
    /// CGameCtnChallengeParameters 0x005 chunk
    /// </summary>
    [Chunk(0x0305B005)]
    public class Chunk0305B005 : Chunk<CGameCtnChallengeParameters>
    {
        public int U01;
        public int U02;
        public int U03;

        public override void ReadWrite(CGameCtnChallengeParameters n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref U01);
            rw.Int32(ref U02);
            rw.Int32(ref U03);
        }
    }

    #endregion

    #region 0x006 chunk (items)

    /// <summary>
    /// CGameCtnChallengeParameters 0x006 chunk (items). This chunk causes "Couldn't load map" in ManiaPlanet.
    /// </summary>
    [Chunk(0x0305B006, "items")]
    public class Chunk0305B006 : Chunk<CGameCtnChallengeParameters>
    {
        public uint[]? U01;

        public override void ReadWrite(CGameCtnChallengeParameters n, GameBoxReaderWriter rw)
        {
            rw.Array(ref U01,
                r => r.ReadUInt32(),
                (x, w) => w.Write(x));
        }
    }

    #endregion

    #region 0x007 chunk

    /// <summary>
    /// CGameCtnChallengeParameters 0x007 chunk
    /// </summary>
    [Chunk(0x0305B007)]
    public class Chunk0305B007 : Chunk<CGameCtnChallengeParameters>
    {
        public uint U01;

        public override void ReadWrite(CGameCtnChallengeParameters n, GameBoxReaderWriter rw)
        {
            rw.UInt32(ref U01);
        }
    }

    #endregion

    #region 0x008 chunk (stunts)

    /// <summary>
    /// CGameCtnChallengeParameters 0x008 chunk (stunts)
    /// </summary>
    [Chunk(0x0305B008, "stunts")]
    public class Chunk0305B008 : Chunk<CGameCtnChallengeParameters>
    {
        public override void ReadWrite(CGameCtnChallengeParameters n, GameBoxReaderWriter rw)
        {
            rw.TimeInt32(ref n.timeLimit);
            rw.Int32(ref n.authorScore);
        }
    }

    #endregion

    #region 0x00A chunk (medals)

    /// <summary>
    /// CGameCtnChallengeParameters 0x00A chunk (medals)
    /// </summary>
    [Chunk(0x0305B00A, "medals")]
    public class Chunk0305B00A : SkippableChunk<CGameCtnChallengeParameters>
    {
        public override void ReadWrite(CGameCtnChallengeParameters n, GameBoxReaderWriter rw)
        {
            rw.String(ref n.tip!);

            rw.TimeInt32Nullable(ref n.bronzeTime);
            rw.TimeInt32Nullable(ref n.silverTime);
            rw.TimeInt32Nullable(ref n.goldTime);
            rw.TimeInt32Nullable(ref n.authorTime);
            rw.TimeInt32(ref n.timeLimit);
            rw.Int32(ref n.authorScore);
        }
    }

    #endregion

    #region 0x00D chunk (race validate ghost)

    /// <summary>
    /// CGameCtnChallengeParameters 0x00D chunk (race validate ghost)
    /// </summary>
    [Chunk(0x0305B00D, "race validate ghost")]
    public class Chunk0305B00D : Chunk<CGameCtnChallengeParameters>
    {
        public override void ReadWrite(CGameCtnChallengeParameters n, GameBoxReaderWriter rw)
        {
            rw.NodeRef<CGameCtnGhost>(ref n.raceValidateGhost);
        }
    }

    #endregion

    #region 0x00E skippable chunk (map type)

    /// <summary>
    /// CGameCtnChallengeParameters 0x00E skippable chunk (map type)
    /// </summary>
    [Chunk(0x0305B00E, "map type")]
    public class Chunk0305B00E : SkippableChunk<CGameCtnChallengeParameters>
    {
        public override void ReadWrite(CGameCtnChallengeParameters n, GameBoxReaderWriter rw)
        {
            rw.String(ref n.mapType);
            rw.String(ref n.mapStyle);
            rw.Boolean(ref n.isValidatedForScriptModes);
        }
    }

    #endregion

    #endregion
}
