namespace GBX.NET.Interfaces.Game;

public interface IGameCtnChallengeTMF : IGameCtnChallenge
{
    IEnumerable<IGameCtnBlockTMF> GetBlocks();
    void ComputeCrc32();
    void RemovePassword();
    IGameCtnBlockTMF? GetBlock(Int3 pos);
    IEnumerable<IGameCtnBlockTMF> GetBlocks(Int3 pos);
    IGameCtnBlockTMF PlaceBlock(string blockModel, Int3 coord, Direction direction, bool isGround = false, byte variant = 0, byte subVariant = 0);
    int RemoveBlocks(Predicate<IGameCtnBlockTMF> match);
    int RemoveBlock(Predicate<IGameCtnBlockTMF> match);
}
