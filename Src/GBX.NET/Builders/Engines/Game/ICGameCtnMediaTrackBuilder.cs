namespace GBX.NET.Builders.Engines.Game;

public interface ICGameCtnMediaTrackBuilder : IBuilder, ICGameCtnMediaTrackBuilderFor
{
    IList<CGameCtnMediaBlock>? Blocks { get; set; }
    string? Name { get; set; }

    CGameCtnMediaTrackBuilder WithBlocks(IList<CGameCtnMediaBlock> blocks);
    CGameCtnMediaTrackBuilder WithBlocks(params CGameCtnMediaBlock[] blocks);
    CGameCtnMediaTrackBuilder WithName(string name);
    CGameCtnMediaTrackBuilder KeepsPlaying();
    CGameCtnMediaTrackBuilder ReadOnly();
}
