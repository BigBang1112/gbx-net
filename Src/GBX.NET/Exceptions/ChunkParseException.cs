namespace GBX.NET.Exceptions;

public class ChunkParseException : Exception
{
    public ChunkParseException(uint chunkId, uint? previousChunkId)
        : base(GetMessage(chunkId, previousChunkId))
    {

    }

    private static string GetMessage(uint chunkId, uint? previousChunkId)
    {
        var className = NodeCacheManager.Names
            .Where(x => x.Key == Chunk.Remap(chunkId & 0xFFFFF000))
            .Select(x => x.Value)
            .FirstOrDefault() ?? "unknown class";

        var previousClassName = previousChunkId.HasValue
            ? (NodeCacheManager.Names
                .Where(x => x.Key == Chunk.Remap(previousChunkId.Value & 0xFFFFF000))
                .Select(x => x.Value)
                .FirstOrDefault() ?? "unknown class")
            : "not a class";

        return $"Wrong chunk format or unskippable chunk: 0x{chunkId:X8} ({className})\nPrevious chunk: 0x{previousChunkId ?? 0:X8} ({previousClassName})";
    }
}
