using GBX.NET.Managers;

namespace GBX.NET.Serialization.Chunking;

public abstract class SkippableChunk<T> : Chunk<T>, ISkippableChunk where T : IClass
{
    /// <inheritdoc />
    public byte[]? Data { get; set; }

    public override IChunk DeepClone()
    {
        // Should stay abstract here, just temp avoid compile errors
        var clone = (ISkippableChunk)MemberwiseClone();
        clone.Data = Data?.ToArray();
        return clone;
    }

    public override string ToString() => $"{ClassManager.GetName(Id & 0xFFFFF000)} skippable chunk 0x{Id:X8}{(this is IVersionable v ? $" [v{v.Version}]" : "")}";
}