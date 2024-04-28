namespace GBX.NET.Interfaces.Game;

public interface IGameCtnBlockMP4 : IGameCtnBlock
{
    bool HasFlags { get; }
    byte? Variant { get; set; }
    byte? SubVariant { get; set; }
    bool IsClip { get; set; }
    CGameWaypointSpecialProperty? WaypointSpecialProperty { get; set; }
    bool IsGhost { get; set; }
}
