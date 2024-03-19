using GBX.NET.Managers;

namespace GBX.NET.Serialization.Chunking;

public abstract class SkippableChunk<T> : Chunk<T>, ISkippableChunk where T : IClass
{
    /// <inheritdoc />
    public byte[]? Data { get; set; }

    // Should stay abstract here, just temp avoid compile errors
#if NETSTANDARD2_0
    public override Chunk DeepClone()
#else
    public override SkippableChunk<T> DeepClone()
#endif
    {
        var clone = (SkippableChunk<T>)MemberwiseClone();
        clone.Data = Data?.ToArray();
        return clone;
    }
    //

    public override string ToString() => $"{ClassManager.GetName(Id & 0xFFFFF000)} skippable chunk 0x{Id:X8}{(this is IVersionable v ? $" [v{v.Version}]" : "")}";
}