namespace GBX.NET.Builders.Engines.Game;

public partial class CGameCtnMediaTrackBuilder : Builder
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

    internal CGameCtnMediaTrack NewNode()
    {
        var node = new CGameCtnMediaTrack
        {
            Name = Name ?? "Unnamed track",
            Blocks = Blocks ?? new List<CGameCtnMediaBlock>(),
            IsKeepPlaying = IsKeepPlaying,
            IsReadOnly = IsReadOnly
        };
        
        node.CreateChunk<CGameCtnMediaTrack.Chunk03078001>();
        
        return node;
    }
}
