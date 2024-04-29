using System.IO.Compression;

namespace GBX.NET.Interfaces.Game;

public interface IGameCtnChallengeTM2020 : IGameCtnChallenge
{
    IEnumerable<IGameCtnBlockTM2020> GetBlocks();
    IEnumerable<IGameCtnBlockTM2020> GetBakedBlocks();
    ZipArchive OpenReadEmbeddedZipData();
    void UpdateEmbeddedZipData(Action<ZipArchive> update);
    Task UpdateEmbeddedZipDataAsync(Func<ZipArchive, Task> update, CancellationToken cancellationToken = default);
    Task UpdateEmbeddedZipDataAsync(Func<ZipArchive, CancellationToken, Task> update, CancellationToken cancellationToken = default);
    void ComputeCrc32();
    void RemovePassword();
    IGameCtnBlockTM2020? GetBlock(Int3 pos);
    IEnumerable<IGameCtnBlockTM2020> GetBlocks(Int3 pos);
    IEnumerable<IGameCtnBlockTM2020> GetGhostBlocks();
    IGameCtnBlockTM2020 PlaceBlock(string blockModel, Int3 coord, Direction direction, bool isGround = false, byte variant = 0, byte subVariant = 0);
    int RemoveBlocks(Predicate<IGameCtnBlockTM2020> match);
    int RemoveBlock(Predicate<IGameCtnBlockTM2020> match);
    void RemoveAllAnchoredObjects();
    void RemoveAllOffZone();
    void RemoveAll();
}
