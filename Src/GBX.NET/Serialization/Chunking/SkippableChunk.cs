namespace GBX.NET.Serialization.Chunking;

public sealed class SkippableChunk(uint id) : ISkippableChunk
{
    public uint Id => id;

    /// <inheritdoc />
    public byte[]? Data { get; set; }

    public bool Ignore => false; // non-ignored skippable chunks get reported in logs, viable for unknown ones

    public IChunk DeepClone()
    {
        return new SkippableChunk(Id)
        {
            Data = Data?.ToArray()
        };
    }
}