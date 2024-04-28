namespace GBX.NET.Interfaces.Game;

public interface IGameCtnChallengeTMSX : IGameCtnChallenge
{
    IEnumerable<IGameCtnBlockTMSX> GetBlocks();
}
