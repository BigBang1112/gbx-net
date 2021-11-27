namespace GBX.NET.Builders.Engines.Game;

public partial class CGameCtnMediaTrackBuilder : ICGameCtnMediaTrackBuilder
{
    public string? Name { get; set; }
    public IList<CGameCtnMediaBlock>? Blocks { get; set; }
    public bool IsKeepPlaying { get; set; }
    public bool IsReadOnly { get; set; }

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

    public CGameCtnMediaTrackBuilder KeepsPlaying()
    {
        IsKeepPlaying = true;
        return this;
    }

    public CGameCtnMediaTrackBuilder ReadOnly()
    {
        IsReadOnly = true;
        return this;
    }

    public TMSX ForTMSX() => new(this, NewNode());
    public TMU ForTMU() => new(this, NewNode());
    public TMUF ForTMUF() => new(this, NewNode());
    public TM2 ForTM2() => new(this, NewNode());
    public TM2020 ForTM2020() => new(this, NewNode());

    GameBuilder<ICGameCtnMediaTrackBuilder, CGameCtnMediaTrack>
        IBuilderForTMSX<ICGameCtnMediaTrackBuilder, CGameCtnMediaTrack>.ForTMSX() => ForTMSX();
    GameBuilder<ICGameCtnMediaTrackBuilder, CGameCtnMediaTrack>
        IBuilderForTMU<ICGameCtnMediaTrackBuilder, CGameCtnMediaTrack>.ForTMU() => ForTMU();
    GameBuilder<ICGameCtnMediaTrackBuilder, CGameCtnMediaTrack>
        IBuilderForTMUF<ICGameCtnMediaTrackBuilder, CGameCtnMediaTrack>.ForTMUF() => ForTMUF();
    GameBuilder<ICGameCtnMediaTrackBuilder, CGameCtnMediaTrack>
        IBuilderForTM2<ICGameCtnMediaTrackBuilder, CGameCtnMediaTrack>.ForTM2() => ForTM2();
    GameBuilder<ICGameCtnMediaTrackBuilder, CGameCtnMediaTrack>
        IBuilderForTM2020<ICGameCtnMediaTrackBuilder, CGameCtnMediaTrack>.ForTM2020() => ForTM2020();

    internal CGameCtnMediaTrack NewNode()
    {
        var node = NodeCacheManager.GetNodeInstance<CGameCtnMediaTrack>(0x03078000);
        node.Name = Name ?? "Unnamed track";
        node.Blocks = Blocks ?? new List<CGameCtnMediaBlock>();
        node.IsKeepPlaying = IsKeepPlaying;
        node.IsReadOnly = IsReadOnly;
        node.CreateChunk<CGameCtnMediaTrack.Chunk03078001>();
        return node;
    }
}
