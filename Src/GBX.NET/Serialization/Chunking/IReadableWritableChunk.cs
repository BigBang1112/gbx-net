namespace GBX.NET.Serialization.Chunking;

/// <summary>
/// Supports reading/writing data at the same time for the Gbx class.
/// </summary>
public interface IReadableWritableChunk : IReadableChunk, IWritableChunk
{
    /// <summary>
    /// Reads and/or writes data via the reader/writer and uses Gbx class for it.
    /// </summary>
    /// <param name="n">A Gbx class.</param>
    /// <param name="rw">A reader/writer.</param>
    void ReadWrite(IClass n, GbxReaderWriter rw);
}
