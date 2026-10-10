namespace GBX.NET.Engines.Game;

public partial class CGameCtnBlockUnitInfo
{
    public partial class Chunk0303600C
    {
        private int ValidateClipCounts(CGameCtnBlockUnitInfo n)
        {
            if (Version == 0 && (n.ClipsNorth?.Length > 3
                || n.ClipsEast?.Length > 3
                || n.ClipsSouth?.Length > 3
                || n.ClipsWest?.Length > 3
                || n.ClipsTop?.Length > 3
                || n.ClipsBottom?.Length > 3))
            {
                throw new InvalidOperationException("Version 0 supports at most 3 clips per direction.");
            }

            return Version;
        }
    }
}
