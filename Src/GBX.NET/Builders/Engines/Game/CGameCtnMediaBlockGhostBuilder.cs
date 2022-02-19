namespace GBX.NET.Builders.Engines.Game;

public partial class CGameCtnMediaBlockGhostBuilder : Builder
{
    public float StartOffset { get; set; }
    public CGameCtnGhost GhostModel { get; set; }

    public CGameCtnMediaBlockGhostBuilder(CGameCtnGhost ghostModel)
    {
        GhostModel = ghostModel;
    }

    public CGameCtnMediaBlockGhostBuilder WithStartOffset(float startOffset)
    {
        StartOffset = startOffset;
        return this;
    }

    public TMUF ForTMUF() => new(this, NewNode());

    internal CGameCtnMediaBlockGhost NewNode()
    {
        var node = NodeCacheManager.GetNodeInstance<CGameCtnMediaBlockGhost>(0x030E5000);

        node.GhostModel = GhostModel;
        node.StartOffset = StartOffset;

        return node;
    }
}