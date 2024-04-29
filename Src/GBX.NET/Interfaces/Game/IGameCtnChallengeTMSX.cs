namespace GBX.NET.Interfaces.Game;

public interface IGameCtnChallengeTMSX : IGameCtnChallenge
{
    IEnumerable<IGameCtnBlockTMSX> GetBlocks();
    IGameCtnBlockTMSX? GetBlock(Int3 pos);
    IEnumerable<IGameCtnBlockTMSX> GetBlocks(Int3 pos);
    IGameCtnBlockTMSX PlaceBlock(string blockModel, Int3 coord, Direction direction, bool isGround = false, byte variant = 0, byte subVariant = 0);
    int RemoveBlocks(Predicate<IGameCtnBlockTMSX> match);
    int RemoveBlock(Predicate<IGameCtnBlockTMSX> match);
}
