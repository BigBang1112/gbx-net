using GBX.NET.Engines.Game;
using System;
using System.Collections.Generic;
using System.Text;

namespace GBX.NET.Builders.Engines.Game;

public partial class CGameCtnMediaTrackBuilder : ICGameCtnMediaTrackBuilder
{
    public string? Name { get; set; }
    public IList<CGameCtnMediaBlock>? Blocks { get; set; }

    public CGameCtnMediaTrackBuilder WithName(string name)
    {
        Name = name;
        return this;
    }

    public CGameCtnMediaTrackBuilder WithBlocks(IList<CGameCtnMediaBlock> blocks)
    {
        Blocks = blocks;
        return this;
    }

    public CGameCtnMediaTrackBuilder WithBlocks(params CGameCtnMediaBlock[] blocks)
    {
        Blocks = blocks;
        return this;
    }

    public TMSX ForTMSX() => new(this, NewNode());
    public TMUF ForTMUF() => new(this, NewNode());
    public TM2 ForTM2() => new(this, NewNode());

    private static CGameCtnMediaTrack NewNode()
    {
        var node = NodeCacheManager.GetNodeInstance<CGameCtnMediaTrack>(0x03078000);
        node.CreateChunk<CGameCtnMediaTrack.Chunk03078001>();
        return node;
    }
}
