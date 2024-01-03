namespace GBX.NET;

/// <summary>
/// Supports reading/writing data at the same time.
/// </summary>
public interface IReadableWritable
{
    /// <summary>
    /// Reads and/or writes data via the reader/writer.
    /// </summary>
    /// <param name="rw">A reader/writer.</param>
    /// <param name="version">Version to help with backwards compatibility.</param>
    void ReadWrite(IGbxReaderWriter rw, int version = 0);
}
