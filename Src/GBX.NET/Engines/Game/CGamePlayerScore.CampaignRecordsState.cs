namespace GBX.NET.Engines.Game;

public partial class CGamePlayerScore
{
    public class CampaignRecordsState
    {
        public string CampaignId { get; }
        public ulong U01 { get; } // SSystemTime
        
        public CampaignRecordsState(string campaignId, ulong u01)
        {
            CampaignId = campaignId;
            U01 = u01;
        }

        public override string ToString()
        {
            return $"{CampaignId}: {U01}";
        }
    }
}
