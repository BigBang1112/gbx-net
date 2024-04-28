namespace GBX.NET.Interfaces.Game;

public interface IGameCtnChallengeTM10 : IGameCtnChallenge
{
    IEnumerable<IGameCtnBlockTM10> GetBlocks();
}
