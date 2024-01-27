namespace GBX.NET;

/// <summary>
/// A data chunk located in the header part. Associated with <typeparamref name="T"/>, supports separate reading and writing.
/// </summary>
public interface IHeaderChunk<T> : IReadableChunk<T>, IWritableChunk<T>, IHeaderChunk where T : IClass;

/// <summary>
/// A data chunk located in the header part. Associated with <typeparamref name="T"/>, supports separate and joined reading and writing.
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract class HeaderChunk<T> : Chunk<T>, IHeaderChunk<T> where T : IClass
{
    /// <inheritdoc />
    public bool IsHeavy { get; set; }
}