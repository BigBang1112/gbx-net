namespace GBX.NET.Interfaces.Game;

public interface IGameCtnChallengeMP4 : IGameCtnChallenge
{
    IEnumerable<IGameCtnBlockMP4> GetBlocks(bool includeUnassigned1 = true);
}
