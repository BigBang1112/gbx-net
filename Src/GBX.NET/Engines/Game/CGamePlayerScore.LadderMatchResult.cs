namespace GBX.NET.Engines.Game;

public partial class CGamePlayerScore
{
    public struct LadderMatchResult
    {
        public ulong U01 { get; } // SSystemTime
        public float U02 { get; }
        public Byte3 U03 { get; }
        
        public LadderMatchResult(ulong u01, float u02, Byte3 u03)
        {
            U01 = u01;
            U02 = u02;
            U03 = u03;
        }
    }
}
