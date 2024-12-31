namespace GBX.NET.Engines.Game;

public partial class CGameCtnMediaClip
{
    private string? name;
    [AppliedWithChunk<Chunk03079002>]
    [AppliedWithChunk<Chunk03079003>]
    [AppliedWithChunk<Chunk03079005>]
    [AppliedWithChunk<Chunk0307900D>]
    public string? Name { get => name; set => name = value; }

    private List<CGameCtnMediaTrack>? tracks;
    [AppliedWithChunk<Chunk03079002>]
    [AppliedWithChunk<Chunk03079003>]
    [AppliedWithChunk<Chunk03079005>]
    [AppliedWithChunk<Chunk0307900D>]
    public List<CGameCtnMediaTrack> Tracks
    {
        get => tracks ??= [];
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
