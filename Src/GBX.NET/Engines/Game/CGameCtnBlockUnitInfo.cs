namespace GBX.NET.Engines.Game;

public partial class CGameCtnBlockUnitInfo
{
    public partial class Chunk0303600C
    {
        private int ValidateClipCounts(CGameCtnBlockUnitInfo n)
        {
            if (Version == 0 && (n.ClipsNorth?.Count > 3
                || n.ClipsEast?.Count > 3
                || n.ClipsSouth?.Count > 3
                || n.ClipsWest?.Count > 3
                || n.ClipsTop?.Count > 3
                || n.ClipsBottom?.Count > 3))
            {
                throw new InvalidOperationException("Version 0 supports at most 3 clips per direction.");
            }

            return Version;
        }
    }
}
