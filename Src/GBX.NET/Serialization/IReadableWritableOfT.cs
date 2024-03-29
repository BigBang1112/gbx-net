namespace GBX.NET.Serialization;

/// <summary>
/// Supports reading/writing data at the same time.
/// </summary>
public interface IReadableWritable<T> where T : IClass
{
    /// <summary>
    /// Reads and/or writes data via the reader/writer.
    /// </summary>
    /// <param name="rw">A reader/writer.</param>
    /// <param name="n">Node to use for context.</param>
    /// <param name="v">Version to help with backwards compatibility.</param>
    void ReadWrite(GbxReaderWriter rw, T n, int v = 0);
}
