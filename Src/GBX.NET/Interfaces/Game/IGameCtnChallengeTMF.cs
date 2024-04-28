namespace GBX.NET.Interfaces.Game;

public interface IGameCtnChallengeTMF : IGameCtnChallenge
{
    IEnumerable<IGameCtnBlockTMF> GetBlocks();
}
