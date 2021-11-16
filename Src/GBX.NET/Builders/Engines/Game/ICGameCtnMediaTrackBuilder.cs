using GBX.NET.Engines.Game;
using System.Collections.Generic;

namespace GBX.NET.Builders.Engines.Game;

public interface ICGameCtnMediaTrackBuilder : IBuilder
{
    IList<CGameCtnMediaBlock>? Blocks { get; set; }
    string? Name { get; set; }

    CGameCtnMediaTrackBuilder WithBlocks(IList<CGameCtnMediaBlock> blocks);
    CGameCtnMediaTrackBuilder WithBlocks(params CGameCtnMediaBlock[] blocks);
    CGameCtnMediaTrackBuilder WithName(string name);
}
