namespace GBX.NET.Serialization.Chunking;

public sealed class SkippableChunk(uint id) : ISkippableChunk
{
    public uint Id => id;

    /// <inheritdoc />
    public byte[]? Data { get; set; }

    public IChunk DeepClone()
    {
        return new SkippableChunk(Id)
        {
            Data = Data?.ToArray()
        };
    }
}