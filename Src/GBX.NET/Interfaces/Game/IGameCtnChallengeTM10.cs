namespace GBX.NET.Interfaces.Game;

public interface IGameCtnChallengeTM10 : IGameCtnChallenge
{
    IEnumerable<IGameCtnBlockTM10> GetBlocks();
    IGameCtnBlockTM10? GetBlock(Int3 pos);
    IEnumerable<IGameCtnBlockTM10> GetBlocks(Int3 pos);
    IGameCtnBlockTM10 PlaceBlock(Ident blockModel, Int3 coord, Direction direction, bool isGround = false, byte variant = 0, byte subVariant = 0);
    int RemoveBlocks(Predicate<IGameCtnBlockTM10> match);
    int RemoveBlock(Predicate<IGameCtnBlockTM10> match);
}
