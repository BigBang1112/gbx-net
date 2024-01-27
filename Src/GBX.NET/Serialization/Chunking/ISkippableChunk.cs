namespace GBX.NET.Serialization.Chunking;

public interface ISkippableChunk : IChunk
{
    /// <summary>
    /// Data of the skippable chunk that has not been processed. If this value is not null, this data will be used when writing the chunk.
    /// </summary>
    byte[]? Data { get; set; }
}
