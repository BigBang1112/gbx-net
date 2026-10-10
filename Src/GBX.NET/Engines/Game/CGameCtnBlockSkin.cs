namespace GBX.NET.Engines.Game;

public partial class CGameCtnBlockSkin
{
    private string? skinName;

    [Obsolete("This legacy skin name is ignored by games from TMSX onward. Use PackDesc instead.")]
    public partial string? SkinName
    {
        get => skinName;
        set => skinName = value;
    }
}
