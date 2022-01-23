namespace GBX.NET.Exceptions;

public class ChunkParseException : Exception
{
    public uint ChunkId { get; }
    public uint? PreviousChunkId { get; }
    public string ClassName { get; }
    public string? PreviousClassName { get; }

    public override string Message => $"Wrong chunk format or unskippable chunk: 0x{ChunkId:X8} ({ClassName})\nPrevious chunk: 0x{PreviousChunkId ?? 0:X8} ({PreviousClassName})";

    public ChunkParseException(uint chunkId, uint? previousChunkId)
    {
        ChunkId = chunkId;
        PreviousChunkId = previousChunkId;

        ClassName = NodeCacheManager.Names
            .Where(x => x.Key == Chunk.Remap(chunkId & 0xFFFFF000))
            .Select(x => x.Value)
            .FirstOrDefault() ?? "unknown class";

        PreviousClassName = previousChunkId.HasValue
            ? (NodeCacheManager.Names
                .Where(x => x.Key == Chunk.Remap(previousChunkId.Value & 0xFFFFF000))
                .Select(x => x.Value)
                .FirstOrDefault() ?? "unknown class")
            : "not a class";
    }
}
