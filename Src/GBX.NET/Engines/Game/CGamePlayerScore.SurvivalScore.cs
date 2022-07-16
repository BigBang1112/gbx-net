namespace GBX.NET.Engines.Game;

public partial class CGamePlayerScore
{
    public class SurvivalScore
    {
        public string MapUid { get; }
        public int U01 { get; }
        public int U02 { get; }
        public int U03 { get; }
        public int U04 { get; }
        public int U05 { get; }
        
        public SurvivalScore(string mapUid, int u01, int u02, int u03, int u04, int u05)
        {
            MapUid = mapUid;
            U01 = u01;
            U02 = u02;
            U03 = u03;
            U04 = u04;
            U05 = u05;
        }

        public static SurvivalScore Parse(GameBoxReader r, int version)
        {
            var mapUid = r.ReadId();
            var u01 = r.ReadInt32(); // + 8
            var u02 = r.ReadInt32(); // + 0xc
            var u03 = r.ReadInt32(); // + 0x10
            var u04 = r.ReadInt32(); // + 0x14
            var u05 = -1;

            if (version >= 2)
            {
                u05 = r.ReadInt32(); // + 4
            }

            return new SurvivalScore(mapUid, u01, u02, u03, u04, u05);
        }
    }
}
