namespace GBX.NET.Serialization.Chunking;

/// <summary>
/// Supports reading/writing data at the same time for the <typeparamref name="T"/> class.
/// </summary>
public interface IReadableWritableChunk<T> : IReadableChunk<T>, IWritableChunk<T> where T : IClass
{
    /// <summary>
    /// Reads and/or writes data via the reader/writer and uses <typeparamref name="T"/> class for it.
    /// </summary>
    /// <param name="n">A Gbx class.</param>
    /// <param name="rw">A reader/writer.</param>
    void ReadWrite(T n, IGbxReaderWriter rw);
}
