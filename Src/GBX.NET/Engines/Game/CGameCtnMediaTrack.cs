namespace GBX.NET.Engines.Game;

public partial class CGameCtnMediaTrack
{
    private IList<CGameCtnMediaBlock>? blocks;
    [AppliedWithChunk<Chunk03078001>]
    public IList<CGameCtnMediaBlock> Blocks
    {
        get => blocks ??= new List<CGameCtnMediaBlock>();
        set => blocks = value;
    }
}
