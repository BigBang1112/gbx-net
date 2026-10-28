using GBX.NET.Components;

namespace GBX.NET.Engines.Game;

public partial class CGameCtnBlockInfoClip
{
    [Obsolete("Use SymmetricalClipId instead.")]
    public string? ASymmetricalClipId
    {
        get => SymmetricalClipId;
        set => SymmetricalClipId = value;
    }
}
