namespace GBX.NET.Tool;

/// <summary>
/// Immutable output - can produce a Gbx object without mutating the source.
/// </summary>
/// <typeparam name="T">Type that is expected to be produced. To produce multiple objects, use <see cref="IEnumerable{T}"/> or tuples.</typeparam>
public interface IProductive<T>
{
    /// <summary>
    /// Produces a new object without mutating the source. The implementation should be certain that the source object is not changed.
    /// </summary>
    /// <returns>A newly generated object.</returns>
    T Produce();
}
