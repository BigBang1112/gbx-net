using GBX.NET.Managers;

namespace GBX.NET.Serialization.Chunking;

public sealed class SkippableChunk(uint id) : ISkippableChunk
{
    public uint Id => id;

    /// <inheritdoc />
    public byte[]? Data { get; set; }

    public bool Ignore => false; // non-ignored skippable chunks get reported in logs, viable for unknown ones

    public GameVersion GameVersion => GameVersion.Unspecified;

    public SkippableChunk DeepClone() => new(Id)
    {
        Data = Data?.ToArray()
    };

    IChunk IChunk.DeepClone() => DeepClone();

    public override string ToString() => $"{ClassManager.GetName(Id & 0xFFFFF000)} unknown skippable chunk 0x{Id:X8}";
}