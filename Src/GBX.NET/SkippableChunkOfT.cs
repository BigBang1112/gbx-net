namespace GBX.NET;

public abstract class SkippableChunk<T> : Chunk<T>, ISkippableChunk where T : IClass
{
    /// <inheritdoc />
    public byte[]? Data { get; set; }
}