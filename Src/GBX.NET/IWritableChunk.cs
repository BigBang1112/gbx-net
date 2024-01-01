namespace GBX.NET;

/// <summary>
/// Supports writing data for the Gbx class.
/// </summary>
public interface IWritableChunk : IChunk
{
    /// <summary>
    /// Writes data via the writer and uses Gbx class for it.
    /// </summary>
    /// <param name="n">A Gbx class.</param>
    /// <param name="w">A writer.</param>
    void Write(IClass n, IGbxWriter w);
}
