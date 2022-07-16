namespace GBX.NET.Engines.Game;

public partial class CGamePlayerScore
{
    public class LeagueScore
    {
        public int Version { get; }
        public Node? U01 { get; }
        public Node? U02 { get; }
        public CGameHighScore?[]? HighScores { get; }
        public int U03 { get; }
        public int U04 { get; }
        public string? U05 { get; }
        public string? U06 { get; }

        public LeagueScore(int version, Node? u01, Node? u02, CGameHighScore?[]? highScores,
            int u03, int u04, string? u05, string? u06)
        {
            Version = version;
            U01 = u01;
            U02 = u02;
            HighScores = highScores;
            U03 = u03;
            U04 = u04;
            U05 = u05;
            U06 = u06;
        }

        public static LeagueScore Parse(GameBoxReader r)
        {
            var version = r.ReadInt32();
            var u01 = default(Node);
            var u02 = default(Node);
            var highScores = Array.Empty<CGameHighScore?>();
            var u03 = default(int);
            var u04 = default(int);
            var u05 = default(string);
            var u06 = default(string);

            if (version == 4)
            {
                u01 = r.ReadNodeRef();
                u02 = r.ReadNodeRef();
                highScores = r.ReadArray(r => r.ReadNodeRef<CGameHighScore>());
                
                return new(version, u01, u02, highScores, u03, u04, u05, u06);
            }

            if (version < 5)
            {
                u03 = r.ReadInt32();
                u04 = r.ReadInt32();
                u05 = r.ReadString();
                
                highScores = r.ReadArray(r => r.ReadNodeRef<CGameHighScore>());

                if (version >= 3)
                {
                    u06 = r.ReadString();
                }
            }
            
            if (version >= 5)
            {
                u01 = r.ReadNodeRef();
                u02 = r.ReadNodeRef();
            }
            
            return new(version, u01, u02, highScores, u03, u04, u05, u06);
        }
    }
}
