namespace GBX.NET.Interfaces.Game;

public interface IGameCtnBlock : IClassVersion<CGameCtnBlock>
{
    string Name { get; set; }
    Int3 Coord { get; set; }
    Direction Direction { get; set; }
    int Flags { get; set; }
    bool IsGround { get; set; }
}
