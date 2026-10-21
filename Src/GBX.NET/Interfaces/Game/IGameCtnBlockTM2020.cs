namespace GBX.NET.Interfaces.Game;

public partial interface IGameCtnBlockTM2020 : IGameCtnBlockMP4
{
    bool IsFree { get; set; }
    Vec3? AbsolutePositionInMap { get; set; }
    Vec3? YawPitchRoll { get; set; }
    DifficultyColor Color { get; set; }
    LightmapQuality LightmapQuality { get; set; }
    MacroblockInstance? MacroblockReference { get; set; }
}
