namespace GBX.NET.Engines.Game;

/// <summary>
/// Note: The class ID has been replaced with CGameScriptDebugger.
/// </summary>
/// <remarks>ID: 0x0308D000</remarks>
[Class(0x0308D000)]
public partial class CGamePlayerScore : CMwNod, IClass
{
    [Hexadecimal] public static new uint Id => 0x0308D000;




    private string? playerName;
    [AppliedWithChunk<Chunk0308D003>]
    [AppliedWithChunk<Chunk0308D004>]
    public string? PlayerName { get => playerName; set => playerName = value; }

    private string? nickName;
    [AppliedWithChunk<Chunk0308D003>]
    [AppliedWithChunk<Chunk0308D004>]
    public string? NickName { get => nickName; set => nickName = value; }

    private int scoresVersion;
    [AppliedWithChunk<Chunk0308D003>]
    [AppliedWithChunk<Chunk0308D004>]
    public int ScoresVersion { get => scoresVersion; set => scoresVersion = value; }

    private Score[]? scores;
    [AppliedWithChunk<Chunk0308D003>]
    [AppliedWithChunk<Chunk0308D004>]
    public Score[]? Scores { get => scores; set => scores = value; }

    private int survivalScoresVersion;
    [AppliedWithChunk<Chunk0308D006>]
    public int SurvivalScoresVersion { get => survivalScoresVersion; set => survivalScoresVersion = value; }

    private SurvivalScore[]? survivalScores;
    [AppliedWithChunk<Chunk0308D006>]
    public SurvivalScore[]? SurvivalScores { get => survivalScores; set => survivalScores = value; }

    private CGamePlayerOfficialScores? playerOfficialScores;
    [AppliedWithChunk<Chunk0308D00F>]
    public CGamePlayerOfficialScores? PlayerOfficialScores { get => playerOfficialScores; set => playerOfficialScores = value; }

    private TrainingMedalsScore[]? trainingMedalsScores;
    [AppliedWithChunk<Chunk0308D00F>]
    public TrainingMedalsScore[]? TrainingMedalsScores { get => trainingMedalsScores; set => trainingMedalsScores = value; }

    private CGameCampaignPlayerScores[]? campaignPlayerScores;
    [AppliedWithChunk<Chunk0308D00F>]
    public CGameCampaignPlayerScores[]? CampaignPlayerScores { get => campaignPlayerScores; set => campaignPlayerScores = value; }

    private CampaignRecordsState[]? campaignRecordsStates;
    [AppliedWithChunk<Chunk0308D010>]
    public CampaignRecordsState[]? CampaignRecordsStates { get => campaignRecordsStates; set => campaignRecordsStates = value; }

    private LadderMatchResult[]? ladderMatchResults;
    [AppliedWithChunk<Chunk0308D012>]
    public LadderMatchResult[]? LadderMatchResults { get => ladderMatchResults; set => ladderMatchResults = value; }

    /// <summary>
    /// Creates a new instance of <see cref="CGamePlayerScore"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public CGamePlayerScore() { }


    /// <summary>
    /// CGamePlayerScore 0x003 chunk
    /// </summary>
    [Chunk(0x0308D003)]
    public partial class Chunk0308D003 : Chunk<CGamePlayerScore>
    {
        /// <inheritdoc />
        public override uint Id => 0x0308D003;

        public string? U01;

        public override void ReadWrite(CGamePlayerScore n, GbxReaderWriter rw)
        {
            rw.String(ref n.playerName);
            rw.String(ref n.nickName);
            rw.Id(ref U01);
            rw.Int32(ref n.scoresVersion);
            rw.ArrayReadableWritable<Score>(ref n.scores!, version: n.ScoresVersion);
        }
    }

    /// <summary>
    /// CGamePlayerScore 0x004 chunk
    /// </summary>
    [Chunk(0x0308D004)]
    public partial class Chunk0308D004 : Chunk<CGamePlayerScore>
    {
        /// <inheritdoc />
        public override uint Id => 0x0308D004;

        public string? U01;

        public override void ReadWrite(CGamePlayerScore n, GbxReaderWriter rw)
        {
            rw.String(ref n.playerName);
            rw.String(ref n.nickName);
            rw.Id(ref U01);
            rw.Int32(ref n.scoresVersion);
            rw.ArrayReadableWritable<Score>(ref n.scores!, version: n.ScoresVersion);
        }
    }

    /// <summary>
    /// CGamePlayerScore 0x006 chunk
    /// </summary>
    [Chunk(0x0308D006)]
    public partial class Chunk0308D006 : Chunk<CGamePlayerScore>
    {
        /// <inheritdoc />
        public override uint Id => 0x0308D006;


        public override void ReadWrite(CGamePlayerScore n, GbxReaderWriter rw)
        {
            rw.Int32(ref n.survivalScoresVersion);
            rw.ArrayReadableWritable<SurvivalScore>(ref n.survivalScores!, version: n.SurvivalScoresVersion);
        }
    }

    /// <summary>
    /// CGamePlayerScore 0x00F chunk
    /// </summary>
    [Chunk(0x0308D00F)]
    public partial class Chunk0308D00F : Chunk<CGamePlayerScore>
    {
        /// <inheritdoc />
        public override uint Id => 0x0308D00F;


        public override void ReadWrite(CGamePlayerScore n, GbxReaderWriter rw)
        {
            rw.NodeRef<CGamePlayerOfficialScores>(ref n.playerOfficialScores);
            rw.ArrayReadableWritable<TrainingMedalsScore>(ref n.trainingMedalsScores!);
            rw.ArrayNodeRef_deprec<CGameCampaignPlayerScores>(ref n.campaignPlayerScores!);
        }
    }

    /// <summary>
    /// CGamePlayerScore 0x010 chunk
    /// </summary>
    [Chunk(0x0308D010)]
    public partial class Chunk0308D010 : Chunk<CGamePlayerScore>
    {
        /// <inheritdoc />
        public override uint Id => 0x0308D010;

        public byte U01;

        public override void ReadWrite(CGamePlayerScore n, GbxReaderWriter rw)
        {
            rw.Byte(ref U01);
            rw.ArrayReadableWritable<CampaignRecordsState>(ref n.campaignRecordsStates!);
        }
    }

    /// <summary>
    /// CGamePlayerScore 0x011 chunk
    /// </summary>
    [Chunk(0x0308D011)]
    public partial class Chunk0308D011 : Chunk<CGamePlayerScore>
    {
        /// <inheritdoc />
        public override uint Id => 0x0308D011;

        public int U01;
        public int U02;
        public int U03;
        public int U04;
        public int U05;
        public int U06;
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
        public int U17;
        public int U18;
        public int U19;
        public int U20;
        public int U21;
        public int U22;
        public int U23;
        public int U24;
        public int U25;
        public int U26;
        public int U27;
        public int U28;
        public int U29;
        public int U30;
        public int U31;
        public int U32;
        public int U33;
        public int U34;

        public override void ReadWrite(CGamePlayerScore n, GbxReaderWriter rw)
        {
            rw.Int32(ref U01);
            rw.Int32(ref U02);
            rw.Int32(ref U03);
            rw.Int32(ref U04);
            rw.Int32(ref U05);
            rw.Int32(ref U06);
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
            rw.Int32(ref U17);
            rw.Int32(ref U18);
            rw.Int32(ref U19);
            rw.Int32(ref U20);
            rw.Int32(ref U21);
            rw.Int32(ref U22);
            rw.Int32(ref U23);
            rw.Int32(ref U24);
            rw.Int32(ref U25);
            rw.Int32(ref U26);
            rw.Int32(ref U27);
            rw.Int32(ref U28);
            rw.Int32(ref U29);
            rw.Int32(ref U30);
            rw.Int32(ref U31);
            rw.Int32(ref U32);
            rw.Int32(ref U33);
            rw.Int32(ref U34);
        }
    }

    /// <summary>
    /// CGamePlayerScore 0x012 chunk
    /// </summary>
    [Chunk(0x0308D012)]
    public partial class Chunk0308D012 : Chunk<CGamePlayerScore>
    {
        /// <inheritdoc />
        public override uint Id => 0x0308D012;


        public override void ReadWrite(CGamePlayerScore n, GbxReaderWriter rw)
        {
            rw.ArrayReadableWritable<LadderMatchResult>(ref n.ladderMatchResults!);
        }
    }


    public sealed partial class LadderMatchResult : IReadableWritable
    {

        private DateTime? u01;
        public DateTime? U01 { get => u01; set => u01 = value; }

        private float u02;
        public float U02 { get => u02; set => u02 = value; }

        private byte u03;
        public byte U03 { get => u03; set => u03 = value; }

        private byte u04;
        public byte U04 { get => u04; set => u04 = value; }

        private byte u05;
        public byte U05 { get => u05; set => u05 = value; }

        public void ReadWrite(GbxReaderWriter rw, int v = 0)
        {
            rw.SystemTime(ref u01);
            rw.Single(ref u02);
            rw.Byte(ref u03);
            rw.Byte(ref u04);
            rw.Byte(ref u05);
        }
    }

    public sealed partial class Score : IReadableWritable
    {

        private Ident? mapInfo;
        public Ident? MapInfo { get => mapInfo; set => mapInfo = value; }

        private int u01;
        public int U01 { get => u01; set => u01 = value; }

        private TimeInt32 personalBest;
        public TimeInt32 PersonalBest { get => personalBest; set => personalBest = value; }

        private int u02;
        public int U02 { get => u02; set => u02 = value; }

        private bool u03;
        public bool U03 { get => u03; set => u03 = value; }

        private int u04;
        public int U04 { get => u04; set => u04 = value; }

        private int u05;
        public int U05 { get => u05; set => u05 = value; }

        private int u06;
        public int U06 { get => u06; set => u06 = value; }

        private int u07;
        public int U07 { get => u07; set => u07 = value; }

        private int u08;
        public int U08 { get => u08; set => u08 = value; }

        private int u09;
        public int U09 { get => u09; set => u09 = value; }

        private int u10;
        public int U10 { get => u10; set => u10 = value; }

        private int u11;
        public int U11 { get => u11; set => u11 = value; }

        private short u12;
        public short U12 { get => u12; set => u12 = value; }

        private short u13;
        public short U13 { get => u13; set => u13 = value; }

        private int u14;
        public int U14 { get => u14; set => u14 = value; }

        private int u15;
        public int U15 { get => u15; set => u15 = value; }

        private int u16;
        public int U16 { get => u16; set => u16 = value; }

        private DateTime? u17;
        public DateTime? U17 { get => u17; set => u17 = value; }

        private DateTime? u18;
        public DateTime? U18 { get => u18; set => u18 = value; }

        private DeprecatedChallengeLeagueScore[]? deprecatedChallengeLeagueScores;
        public DeprecatedChallengeLeagueScore[]? DeprecatedChallengeLeagueScores { get => deprecatedChallengeLeagueScores; set => deprecatedChallengeLeagueScores = value; }

        private int u19;
        public int U19 { get => u19; set => u19 = value; }

        private int u20;
        public int U20 { get => u20; set => u20 = value; }

        private int u21;
        public int U21 { get => u21; set => u21 = value; }

        private byte u22;
        public byte U22 { get => u22; set => u22 = value; }

        private string? mapName;
        public string? MapName { get => mapName; set => mapName = value; }

        private int u23;
        public int U23 { get => u23; set => u23 = value; }

        private int u24;
        public int U24 { get => u24; set => u24 = value; }

        private int u25;
        public int U25 { get => u25; set => u25 = value; }

        private int u26;
        public int U26 { get => u26; set => u26 = value; }

        private int u27;
        public int U27 { get => u27; set => u27 = value; }

        private int u28;
        public int U28 { get => u28; set => u28 = value; }

        private int u29;
        public int U29 { get => u29; set => u29 = value; }

        private int u30;
        public int U30 { get => u30; set => u30 = value; }

        private int u31;
        public int U31 { get => u31; set => u31 = value; }

        private short u32;
        public short U32 { get => u32; set => u32 = value; }

        private short u33;
        public short U33 { get => u33; set => u33 = value; }

        public void ReadWrite(GbxReaderWriter rw, int v = 0)
        {
            rw.Ident(ref mapInfo);
            if (v == 0)
            {
                rw.Int32(ref u01);
            }
            rw.TimeInt32(ref personalBest);
            if (v <= 4)
            {
                if (v >= 1)
                {
                    rw.Int32(ref u02);
                    rw.Boolean(ref u03);
                }
                if (v >= 2)
                {
                    rw.Int32(ref u04);
                    rw.Int32(ref u05);
                    rw.Int32(ref u06);
                }
                if (v >= 3)
                {
                    rw.Int32(ref u07);
                }
                if (v == 4)
                {
                    rw.Int32(ref u08);
                }
            }
            if (v >= 5)
            {
                rw.Int32(ref u09);
                rw.Int32(ref u10);
                rw.Int32(ref u11);
                rw.Int16(ref u12);
                rw.Int16(ref u13);
            }
            if (v >= 6)
            {
                rw.Int32(ref u14);
            }
            if (v >= 7)
            {
                rw.Int32(ref u15);
            }
            if (v >= 8)
            {
                rw.Int32(ref u16);
            }
            if (v >= 9)
            {
                rw.SystemTime(ref u17);
                rw.SystemTime(ref u18);
                if (v <= 15)
                {
                    rw.ArrayReadableWritable<DeprecatedChallengeLeagueScore>(ref deprecatedChallengeLeagueScores!);
                }
            }
            if (v >= 10)
            {
                rw.Int32(ref u19);
            }
            if (v >= 11)
            {
                rw.Int32(ref u20);
            }
            if (v >= 12)
            {
                rw.Int32(ref u21);
            }
            if (v >= 13)
            {
                rw.Byte(ref u22);
                rw.String(ref mapName);
            }
            if (v >= 14)
            {
                rw.Int32(ref u23);
            }
            if (v >= 15)
            {
                rw.Int32(ref u24);
            }
            if (v >= 17)
            {
                rw.Int32(ref u25);
                rw.Int32(ref u26);
                rw.Int32(ref u27);
                rw.Int32(ref u28);
            }
            if (v >= 18)
            {
                rw.Int32(ref u29);
                rw.Int32(ref u30);
                rw.Int32(ref u31);
                rw.Int16(ref u32);
                rw.Int16(ref u33);
            }
        }
    }

    public sealed partial class DeprecatedChallengeLeagueScore : IReadableWritable
    {

        private int version;
        public int Version { get => version; set => version = value; }

        private CMwNod? u01;
        public CMwNod? U01 { get => u01; set => u01 = value; }

        private CMwNod? u02;
        public CMwNod? U02 { get => u02; set => u02 = value; }

        private CMwNod? u03;
        public CMwNod? U03 { get => u03; set => u03 = value; }

        private CMwNod? u04;
        public CMwNod? U04 { get => u04; set => u04 = value; }

        private CGameHighScore[]? u05;
        public CGameHighScore[]? U05 { get => u05; set => u05 = value; }

        private int u06;
        public int U06 { get => u06; set => u06 = value; }

        private int u07;
        public int U07 { get => u07; set => u07 = value; }

        private string? u08;
        public string? U08 { get => u08; set => u08 = value; }

        private CMwNod[]? u09;
        public CMwNod[]? U09 { get => u09; set => u09 = value; }

        private CGameHighScore[]? u10;
        public CGameHighScore[]? U10 { get => u10; set => u10 = value; }

        private string? u11;
        public string? U11 { get => u11; set => u11 = value; }

        public void ReadWrite(GbxReaderWriter rw, int v = 0)
        {
            rw.Int32(ref this.version);
            if (Version>=5)
            {
                rw.NodeRef<CMwNod>(ref u01);
                rw.NodeRef<CMwNod>(ref u02);
            }
            if (Version==4)
            {
                rw.NodeRef<CMwNod>(ref u03);
                rw.NodeRef<CMwNod>(ref u04);
                rw.ArrayNodeRef<CGameHighScore>(ref u05!);
            }
            if (Version<4)
            {
                rw.Int32(ref u06);
                rw.Int32(ref u07);
                rw.String(ref u08);
                if (Version<2)
                {
                    rw.ArrayNodeRef<CMwNod>(ref u09!);
                }
                if (Version>=2)
                {
                    rw.ArrayNodeRef<CGameHighScore>(ref u10!);
                }
                if (Version>=3)
                {
                    rw.String(ref u11);
                }
            }
        }
    }

    public sealed partial class TrainingMedalsScore : IReadableWritable
    {

        private string? campaignId;
        public string? CampaignId { get => campaignId; set => campaignId = value; }

        private int u01;
        public int U01 { get => u01; set => u01 = value; }

        private CGamePlayerOfficialScores? score;
        public CGamePlayerOfficialScores? Score { get => score; set => score = value; }

        public void ReadWrite(GbxReaderWriter rw, int v = 0)
        {
            rw.Id(ref campaignId);
            rw.Int32(ref u01);
            rw.NodeRef<CGamePlayerOfficialScores>(ref score);
        }
    }

    public sealed partial class CampaignRecordsState : IReadableWritable
    {

        private string? campaignId;
        public string? CampaignId { get => campaignId; set => campaignId = value; }

        private DateTime? u01;
        public DateTime? U01 { get => u01; set => u01 = value; }

        public void ReadWrite(GbxReaderWriter rw, int v = 0)
        {
            rw.Id(ref campaignId);
            rw.SystemTime(ref u01);
        }
    }

    public sealed partial class SurvivalScore : IReadableWritable
    {

        private string? mapUid;
        public string? MapUid { get => mapUid; set => mapUid = value; }

        private int u01;
        public int U01 { get => u01; set => u01 = value; }

        private int u02;
        public int U02 { get => u02; set => u02 = value; }

        private int u03;
        public int U03 { get => u03; set => u03 = value; }

        private int u04;
        public int U04 { get => u04; set => u04 = value; }

        private int u05 = -1;
        public int U05 { get => u05; set => u05 = value; }

        public void ReadWrite(GbxReaderWriter rw, int v = 0)
        {
            rw.Id(ref mapUid);
            rw.Int32(ref u01);
            rw.Int32(ref u02);
            rw.Int32(ref u03);
            rw.Int32(ref u04);
            if (v >= 2)
            {
                rw.Int32(ref u05);
            }
        }
    }



    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x0308D003 => new Chunk0308D003(),
        0x0308D004 => new Chunk0308D004(),
        0x0308D006 => new Chunk0308D006(),
        0x0308D00F => new Chunk0308D00F(),
        0x0308D010 => new Chunk0308D010(),
        0x0308D011 => new Chunk0308D011(),
        0x0308D012 => new Chunk0308D012(),
        _ => base.NewChunk(chunkId),
    };
}
