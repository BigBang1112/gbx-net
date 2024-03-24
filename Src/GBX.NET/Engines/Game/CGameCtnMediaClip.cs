namespace GBX.NET.Engines.Game;

public partial class CGameCtnMediaClip
{
    public override string ToString()
    {
        return $"{nameof(CGameCtnMediaClip)}: {(string.IsNullOrEmpty(Name) ? "(unnamed)" : Name)}";
    }

    /// <summary>
    /// This does not include Shootmania and TM2020 ghosts.
    /// </summary>
    /// <returns></returns>
    public IEnumerable<CGameCtnGhost> GetGhosts() => Tracks
        .SelectMany(track => track.Blocks)
        .OfType<CGameCtnMediaBlockGhost>()
        .Select(blockGhost => blockGhost.GhostModel);
}
