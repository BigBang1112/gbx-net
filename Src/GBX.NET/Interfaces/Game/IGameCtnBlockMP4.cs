namespace GBX.NET.Interfaces.Game;

public partial interface IGameCtnBlockMP4 : IGameCtnBlock
{
    new byte Variant { get; set; }
    byte SubVariant { get; set; }
    bool IsClip { get; set; }
    CGameWaypointSpecialProperty? WaypointSpecialProperty { get; set; }
    bool IsGhost { get; set; }
}
