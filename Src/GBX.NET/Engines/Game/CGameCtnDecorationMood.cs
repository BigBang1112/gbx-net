namespace GBX.NET.Engines.Game;

public partial class CGameCtnDecorationMood
{
    private bool? isNight;

    [AppliedWithChunk<Chunk0303A001>]
    [AppliedWithChunk<Chunk0303A012>]
    public bool IsNight
    {
        get => isNight is null ? RemappedStartDayTime is > 0.25f and < 0.75f : isNight.Value;
        set => isNight = value;
    }
}
