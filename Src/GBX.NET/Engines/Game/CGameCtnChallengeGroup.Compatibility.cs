namespace GBX.NET.Engines.Game;

public partial class CGameCtnChallengeGroup
{
    public partial class MapInfo
    {
        [Obsolete("Use IsTestReportSent instead.")]
        public bool U01 { get => IsTestReportSent; set => IsTestReportSent = value; }
    }
}
