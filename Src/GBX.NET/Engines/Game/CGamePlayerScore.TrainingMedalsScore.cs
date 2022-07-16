namespace GBX.NET.Engines.Game;

public partial class CGamePlayerScore
{
    public class TrainingMedalsScore
    {
        public string CampaignId { get; }
        public int U01 { get; }
        public CGamePlayerOfficialScores? Score { get; }

        public TrainingMedalsScore(string campaignId, int u01, CGamePlayerOfficialScores? score)
        {
            CampaignId = campaignId;
            U01 = u01;
            Score = score;
        }

        public override string ToString()
        {
            return $"{CampaignId}: {Score}";
        }
    }
}
