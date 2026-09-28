namespace GBX.NET.Engines.Game;

/// <remarks>ID: 0x03061000</remarks>
[Class(0x03061000)]
public partial class CGameCampaignsScoresManager : CMwNod, IClass
{
    [Hexadecimal] public static new uint Id => 0x03061000;




    private CGameChallengeScores[]? challengeScores;
    [AppliedWithChunk<Chunk03061000>]
    public CGameChallengeScores[]? ChallengeScores { get => challengeScores; set => challengeScores = value; }

    private CGameCampaignScores[]? campaignScores;
    [AppliedWithChunk<Chunk03061000>]
    public CGameCampaignScores[]? CampaignScores { get => campaignScores; set => campaignScores = value; }

    private CGameGeneralScores? generalScores;
    [AppliedWithChunk<Chunk03061000>]
    public CGameGeneralScores? GeneralScores { get => generalScores; set => generalScores = value; }

    private CGameSkillScoreComputer? skillScoreComputer;
    [AppliedWithChunk<Chunk03061000>]
    public CGameSkillScoreComputer? SkillScoreComputer { get => skillScoreComputer; set => skillScoreComputer = value; }

    /// <summary>
    /// Creates a new instance of <see cref="CGameCampaignsScoresManager"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public CGameCampaignsScoresManager() { }


    /// <summary>
    /// CGameCampaignsScoresManager 0x000 chunk
    /// </summary>
    [Chunk(0x03061000)]
    public partial class Chunk03061000 : Chunk<CGameCampaignsScoresManager>
    {
        /// <inheritdoc />
        public override uint Id => 0x03061000;


        public override void ReadWrite(CGameCampaignsScoresManager n, GbxReaderWriter rw)
        {
            rw.ArrayNodeRef_deprec<CGameChallengeScores>(ref n.challengeScores!);
            rw.ArrayNodeRef_deprec<CGameCampaignScores>(ref n.campaignScores!);
            rw.NodeRef<CGameGeneralScores>(ref n.generalScores);
            rw.NodeRef<CGameSkillScoreComputer>(ref n.skillScoreComputer);
        }
    }

    /// <summary>
    /// CGameCampaignsScoresManager 0x001 chunk
    /// </summary>
    [Chunk(0x03061001)]
    public partial class Chunk03061001 : Chunk<CGameCampaignsScoresManager>
    {
        /// <inheritdoc />
        public override uint Id => 0x03061001;

        public string? U01;

        public override void ReadWrite(CGameCampaignsScoresManager n, GbxReaderWriter rw)
        {
            rw.String(ref U01);
        }
    }




    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x03061000 => new Chunk03061000(),
        0x03061001 => new Chunk03061001(),
        _ => base.NewChunk(chunkId),
    };
}
