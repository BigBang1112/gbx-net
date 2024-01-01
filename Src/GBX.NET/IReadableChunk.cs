namespace GBX.NET;

/// <summary>
/// Supports reading data for a Gbx class.
/// </summary>
public interface IReadableChunk : IChunk
{
    /// <summary>
    /// Reads data via the reader and applies it to the Gbx class.
    /// </summary>
    /// <param name="n">A Gbx class.</param>
    /// <param name="r">A reader.</param>
    void Read(IClass n, IGbxReader r);
}
