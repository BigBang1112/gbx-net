namespace GBX.NET;

/// <summary>
/// Supports reading data for the <typeparamref name="T"/> class.
/// </summary>
/// <typeparam name="T">A Gbx class to use.</typeparam>
public interface IReadableChunk<T> : IChunk<T> where T : IClass
{
    /// <summary>
    /// Reads data via the reader and applies it to the <typeparamref name="T"/> class.
    /// </summary>
    /// <param name="n">A Gbx class.</param>
    /// <param name="r">A reader.</param>
    void Read(T n, IGbxReader r);
}