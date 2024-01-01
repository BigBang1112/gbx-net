namespace GBX.NET;

/// <summary>
/// Supports writing data for the <typeparamref name="T"/> class.
/// </summary>
/// <typeparam name="T">A Gbx class to use.</typeparam>
public interface IWritableChunk<T> : IChunk<T> where T : IClass
{
    /// <summary>
    /// Writes data via the writer and uses <typeparamref name="T"/> class for it.
    /// </summary>
    /// <param name="n">A Gbx class.</param>
    /// <param name="w">A writer.</param>
    void Write(T n, IGbxWriter w);
}