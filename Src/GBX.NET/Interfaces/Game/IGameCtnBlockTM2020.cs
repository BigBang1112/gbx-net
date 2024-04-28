namespace GBX.NET.Interfaces.Game;

public interface IGameCtnBlockTM2020 : IGameCtnBlockMP4
{
    bool IsFree { get; set; }
    Vec3? AbsolutePositionInMap { get; set; }
    Vec3? PitchYawRoll { get; set; }
    DifficultyColor Color { get; set; }
    LightmapQuality LightmapQuality { get; set; }
    MacroblockInstance? MacroblockReference { get; set; }
}
