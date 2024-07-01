namespace GBX.NET.Tool;

/// <summary>
/// Mutative input - mutates the source object/s.
/// </summary>
/// <typeparam name="T">Type/s expected to be mutated. It should only include types provided through the constructor. It is used as a hint to tell which objects have changed. To hint multiple objects, use tuples.</typeparam>
public interface IMutative<T>
{
    /// <summary>
    /// Mutates the source object/s. It should return an object from the constructor that has been changed. If multiple objects have been changed, use tuples.
    /// </summary>
    /// <returns>Objects that have been changed.</returns>
    T? Mutate();
}
