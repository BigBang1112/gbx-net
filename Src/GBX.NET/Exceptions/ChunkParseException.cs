namespace GBX.NET.Exceptions;

public class ChunkParseException : Exception
{
    public uint ChunkId { get; }
    public uint? PreviousChunkId { get; }
    public string ClassName { get; }
    public string? PreviousClassName { get; }

    public ChunkParseException(uint chunkId, uint? previousChunkId) : base(GetMessage(chunkId, previousChunkId))
    {
        ChunkId = chunkId;
        PreviousChunkId = previousChunkId;
        ClassName = GetClassNameFromChunkId(chunkId);
        PreviousClassName = GetClassNameFromPreviousChunkId(previousChunkId);
    }

    private static string GetMessage(uint chunkId, uint? previousChunkId)
    {
        return $"Wrong chunk format or unskippable chunk: 0x{chunkId:X8} ({GetClassNameFromChunkId(chunkId)})\nPrevious chunk: 0x{previousChunkId ?? 0:X8} ({GetClassNameFromPreviousChunkId(previousChunkId)})";
    }

    private static string GetClassNameFromChunkId(uint chunkId)
    {
        NodeCacheManager.Names.TryGetValue(chunkId & 0xFFFFF000, out string? className);
        return className ?? "unknown class";
    }

    private static string GetClassNameFromPreviousChunkId(uint? previousChunkId)
    {
        if (previousChunkId is null)
        {
            return "not a class";
        }

        NodeCacheManager.Names.TryGetValue(previousChunkId.Value & 0xFFFFF000, out string? previousClassName);
        return previousClassName ?? "unknown class";
    }
}
