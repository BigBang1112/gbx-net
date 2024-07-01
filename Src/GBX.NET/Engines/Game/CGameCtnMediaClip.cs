namespace GBX.NET.Engines.Game;

public partial class CGameCtnMediaClip
{
    private IList<CGameCtnMediaTrack>? tracks;
    [AppliedWithChunk<Chunk03079002>]
    [AppliedWithChunk<Chunk03079003>]
    [AppliedWithChunk<Chunk0307900D>]
    public IList<CGameCtnMediaTrack> Tracks
    {
        get => tracks ??= new List<CGameCtnMediaTrack>();
        set => tracks = value;
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
