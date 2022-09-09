namespace GBX.NET.Engines.Game;

/// <remarks>ID: 0x0308D000</remarks>
[Node(0x0308D000)]
[NodeExtension("Scores")]
public partial class CGamePlayerScore : CMwNod // Did CGameScriptDebugger stole ID of this class? xD
{
    [NodeMember]
    [AppliedWithChunk(typeof(Chunk0308D004))]
    public string? PlayerName { get; private set; }

    [NodeMember]
    [AppliedWithChunk(typeof(Chunk0308D004))]
    public string? PlayerDisplayName { get; private set; }

    [NodeMember]
    [AppliedWithChunk(typeof(Chunk0308D004))]
    public int ScoresVersion { get; private set; }

    [NodeMember]
    [AppliedWithChunk(typeof(Chunk0308D004))]
    public Score[]? Scores { get; private set; }

    [NodeMember]
    [AppliedWithChunk(typeof(Chunk0308D006))]
    public int SurvivalScoresVersion { get; private set; }

    [NodeMember]
    [AppliedWithChunk(typeof(Chunk0308D006))]
    public SurvivalScore[]? SurvivalScores { get; private set; }
    
    [NodeMember]
    [AppliedWithChunk(typeof(Chunk0308D008))]
    public PlayerCampaignSkillScore[]? PlayerCampaignSkillScores { get; private set; }

    [NodeMember]
    [AppliedWithChunk(typeof(Chunk0308D00F))]
    public CGamePlayerOfficialScores? PlayerOfficialScores { get; private set; }
    
    [NodeMember]
    [AppliedWithChunk(typeof(Chunk0308D00F))]
    public TrainingMedalsScore[]? TrainingMedalsScores { get; private set; }
    
    [NodeMember]
    [AppliedWithChunk(typeof(Chunk0308D00F))]
    public CGameCampaignPlayerScores?[]? CampaignPlayerScores { get; private set; }

    [NodeMember]
    [AppliedWithChunk(typeof(Chunk0308D010))]
    public CampaignRecordsState[]? CampaignRecordsStates { get; private set; }

    [NodeMember]
    [AppliedWithChunk(typeof(Chunk0308D012))]
    public LadderMatchResult[]? LadderMatchResults { get; private set; }

    protected CGamePlayerScore()
    {

    }

    #region 0x004 chunk

    /// <summary>
    /// CGamePlayerScore 0x004 chunk
    /// </summary>
    [Chunk(0x0308D004)]
    public class Chunk0308D004 : Chunk<CGamePlayerScore>
    {
        public string? U01; // GUID

        public override void Read(CGamePlayerScore n, GameBoxReader r)
        {
            n.PlayerName = r.ReadString();
            n.PlayerDisplayName = r.ReadString();
            U01 = r.ReadId();
            n.ScoresVersion = r.ReadInt32();
            n.Scores = r.ReadArray(r => Score.Parse(r, n.ScoresVersion));
        }
    }

    #endregion

    #region 0x006 chunk

    /// <summary>
    /// CGamePlayerScore 0x006 chunk
    /// </summary>
    [Chunk(0x0308D006)]
    public class Chunk0308D006 : Chunk<CGamePlayerScore>
    {
        public override void Read(CGamePlayerScore n, GameBoxReader r)
        {
            n.SurvivalScoresVersion = r.ReadInt32();
            n.SurvivalScores = r.ReadArray(r => SurvivalScore.Parse(r, n.SurvivalScoresVersion));
        }
    }

    #endregion

    #region 0x008 chunk

    /// <summary>
    /// CGamePlayerScore 0x008 chunk
    /// </summary>
    [Chunk(0x0308D008)]
    public class Chunk0308D008 : Chunk<CGamePlayerScore>
    {
        public ulong U01;

        public override void Read(CGamePlayerScore n, GameBoxReader r)
        {
            n.PlayerCampaignSkillScores = r.ReadArray(r => PlayerCampaignSkillScore.Parse(r));
            U01 = r.ReadUInt64(); // SSystemTime
        }
    }

    #endregion

    #region 0x00F chunk

    /// <summary>
    /// CGamePlayerScore 0x00F chunk
    /// </summary>
    [Chunk(0x0308D00F)]
    public class Chunk0308D00F : Chunk<CGamePlayerScore>
    {
        public override void Read(CGamePlayerScore n, GameBoxReader r)
        {
            n.PlayerOfficialScores = r.ReadNodeRef<CGamePlayerOfficialScores>();
            n.TrainingMedalsScores = r.ReadArray<TrainingMedalsScore>(r => new(
                campaignId: r.ReadId(),
                u01: r.ReadInt32(),
                score: r.ReadNodeRef<CGamePlayerOfficialScores>()));

            r.ReadInt32(); // list version 10
            n.CampaignPlayerScores = r.ReadArray(r => r.ReadNodeRef<CGameCampaignPlayerScores>());
        }
    }

    #endregion

    #region 0x010 chunk

    /// <summary>
    /// CGamePlayerScore 0x010 chunk
    /// </summary>
    [Chunk(0x0308D010)]
    public class Chunk0308D010 : Chunk<CGamePlayerScore>
    {
        public byte U01;

        public override void Read(CGamePlayerScore n, GameBoxReader r)
        {
            U01 = r.ReadByte();
            n.CampaignRecordsStates = r.ReadArray<CampaignRecordsState>(r => new(
                campaignId: r.ReadId(),
                u01: r.ReadUInt64())); // SSystemTime
        }
    }

    #endregion
    
    #region 0x011 chunk

    /// <summary>
    /// CGamePlayerScore 0x011 chunk
    /// </summary>
    [Chunk(0x0308D011)]
    public class Chunk0308D011 : Chunk<CGamePlayerScore>
    {
        public Int2[]? U01;

        public override void ReadWrite(CGamePlayerScore n, GameBoxReaderWriter rw)
        {
            rw.Array<Int2>(ref U01, 17);
        }
    }

    #endregion

    #region 0x012 chunk

    /// <summary>
    /// CGamePlayerScore 0x012 chunk
    /// </summary>
    [Chunk(0x0308D012)]
    public class Chunk0308D012 : Chunk<CGamePlayerScore>
    {
        public override void Read(CGamePlayerScore n, GameBoxReader r)
        {
            n.LadderMatchResults = r.ReadArray<LadderMatchResult>(); 
        }
    }

    #endregion
}
