namespace GBX.NET.Engines.Game;

public partial class CGameCtnMediaTrack
{
    private List<CGameCtnMediaBlock>? blocks;
    [AppliedWithChunk<Chunk03078001>]
    public List<CGameCtnMediaBlock> Blocks
    {
        get => blocks ??= [];
        set => blocks = value;
    }
}
