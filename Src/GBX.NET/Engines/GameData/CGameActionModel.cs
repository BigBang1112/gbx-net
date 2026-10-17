namespace GBX.NET.Engines.GameData;

public partial class CGameActionModel
{
    [Obsolete("Use CrossHairRef instead.")]
    public string? Crosshair
    {
        get => CrossHairRef;
        set => CrossHairRef = value;
    }
}
