using GBX.NET.Managers;

namespace GBX.NET.Exceptions;

/// <summary>
/// The exception that is thrown when a chunk cannot be created, read, or written.
/// </summary>
public class ChunkNotSupportedException(uint chunkId)
    : NotSupportedException($"Chunk ID 0x{chunkId:X8} ({ClassManager.GetName(chunkId & 0xFFFFF000) ?? "unknown class name"}) is not supported.")
{
    /// <summary>
    /// The chunk ID that is not supported.
    /// </summary>
    public uint ChunkId { get; } = chunkId;
}
