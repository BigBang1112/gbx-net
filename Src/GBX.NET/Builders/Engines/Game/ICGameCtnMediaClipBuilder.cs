
namespace GBX.NET.Builders.Engines.Game;

public interface ICGameCtnMediaClipBuilder : IBuilder, ICGameCtnMediaClipBuilderFor
{
    string? Name { get; set; }
    IList<CGameCtnMediaTrack>? Tracks { get; set; }

    CGameCtnMediaClipBuilder WithName(string name);
    CGameCtnMediaClipBuilder WithTracks(IList<CGameCtnMediaTrack> tracks);
    CGameCtnMediaClipBuilder WithTracks(params CGameCtnMediaTrack[] tracks);
}