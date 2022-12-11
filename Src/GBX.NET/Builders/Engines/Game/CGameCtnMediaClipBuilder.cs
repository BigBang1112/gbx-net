namespace GBX.NET.Builders.Engines.Game;

public partial class CGameCtnMediaClipBuilder : Builder
{
    public string? Name { get; set; }
    public IList<CGameCtnMediaTrack>? Tracks { get; set; }

    public CGameCtnMediaClipBuilder WithName(string name)
    {
        Name = name;
        return this;
    }

    public CGameCtnMediaClipBuilder WithTracks(IList<CGameCtnMediaTrack> tracks)
    {
        Tracks = tracks;
        return this;
    }

    public CGameCtnMediaClipBuilder WithTracks(params CGameCtnMediaTrack[] tracks)
    {
        Tracks = tracks;
        return this;
    }

    public TMSX ForTMSX() => new(this, NewNode());
    public TMU ForTMU() => new(this, NewNode());
    public TMUF ForTMUF() => new(this, NewNode());
    public TM2 ForTM2() => new(this, NewNode());
    public TM2020 ForTM2020() => new(this, NewNode());

    internal CGameCtnMediaClip NewNode() => new()
    {
        Name = Name ?? "Unnamed clip",
        Tracks = Tracks ?? new List<CGameCtnMediaTrack>()
    };
}