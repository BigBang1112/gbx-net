using System.IO.Compression;

namespace GBX.NET.Interfaces.Game;

public interface IGameCtnChallengeMP4 : IGameCtnChallenge
{
    IEnumerable<IGameCtnBlockMP4> GetBlocks();
    IEnumerable<IGameCtnBlockMP4> GetBakedBlocks();
    ZipArchive OpenReadEmbeddedZipData();
    void UpdateEmbeddedZipData(Action<ZipArchive> update);
    Task UpdateEmbeddedZipDataAsync(Func<ZipArchive, Task> update, CancellationToken cancellationToken = default);
    Task UpdateEmbeddedZipDataAsync(Func<ZipArchive, CancellationToken, Task> update, CancellationToken cancellationToken = default);
    void ComputeCrc32();
    void RemovePassword();
    IGameCtnBlockMP4? GetBlock(Int3 pos);
    IEnumerable<IGameCtnBlockMP4> GetBlocks(Int3 pos);
    IEnumerable<IGameCtnBlockMP4> GetGhostBlocks();
    IGameCtnBlockMP4 PlaceBlock(string blockModel, Int3 coord, Direction direction, bool isGround = false, byte variant = 0, byte subVariant = 0);
    int RemoveBlocks(Predicate<IGameCtnBlockMP4> match);
    int RemoveBlock(Predicate<IGameCtnBlockMP4> match);
    void RemoveAllAnchoredObjects();
    void RemoveAllOffZone();
    void RemoveAll();
}
