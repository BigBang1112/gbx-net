namespace GBX.NET.Engines.Game;

public partial class CGameCtnMediaClip
{
    private string? name;
    public partial string? Name { get => name; set => name = value; }

    private List<CGameCtnMediaTrack>? tracks;
    public partial List<CGameCtnMediaTrack> Tracks
    {
        get => tracks ??= [];
        set => tracks = value;
    }

    public TMUnlimiter? TMUnlimiterData { get; set; }

    [AppliedWithChunk<Chunk0307900E>]
    public bool TriggersBeforeRaceStart
    {
        get => (Flags & 1) != 0;
        set => Flags = value ? Flags | 1 : Flags & ~1;
    }

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
        .Select(blockGhost => blockGhost.GhostModel)
        .OfType<CGameCtnGhost>();
}
