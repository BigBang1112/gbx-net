namespace GBX.NET.Engines.Game;

public partial class CGamePlayerOfficialScores
{
    public class FilteredPlayerRank
    {
        public string Zone { get; }
        public bool U01 { get; }
        public CGameHighScore? Score { get; }
        
        public FilteredPlayerRank(string zone, bool u01, CGameHighScore? score)
        {
            Zone = zone;
            U01 = u01;
            Score = score;
        }

        public override string ToString()
        {
            return $"{Zone}: {Score}";
        }
    }
}
