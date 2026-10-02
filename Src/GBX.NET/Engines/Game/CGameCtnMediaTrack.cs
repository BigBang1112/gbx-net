namespace GBX.NET.Engines.Game;

public partial class CGameCtnMediaTrack
{
    private List<CGameCtnMediaBlock>? blocks;
    public partial List<CGameCtnMediaBlock> Blocks
    {
        get => blocks ??= [];
        set => blocks = value;
    }
}
