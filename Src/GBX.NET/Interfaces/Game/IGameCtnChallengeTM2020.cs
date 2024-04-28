namespace GBX.NET.Interfaces.Game;

public interface IGameCtnChallengeTM2020 : IGameCtnChallenge
{
    IEnumerable<IGameCtnBlockTM2020> GetBlocks();
}
