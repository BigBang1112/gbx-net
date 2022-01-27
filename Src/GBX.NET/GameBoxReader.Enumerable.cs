namespace GBX.NET;

public partial class GameBoxReader
{
    /// <summary>
    /// Does a for loop with <paramref name="length"/> parameter, each element requiring to return an instance of <typeparamref name="T"/>. Instead of array allocation, elements are yielded one by one.
    /// </summary>
    /// <typeparam name="T">Type of the enumerable.</typeparam>
    /// <param name="length">Length of the enumerable.</param>
    /// <param name="forLoop">Each element.</param>
    /// <returns>Enumerable of <typeparamref name="T"/>.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="forLoop"/> is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="length"/> is negative.</exception>
    public static IEnumerable<T> ReadEnumerable<T>(int length, Func<T> forLoop)
    {
        if (forLoop is null)
        {
            throw new ArgumentNullException(nameof(forLoop));
        }

        // In theory it doesn't have to be there, but it ensures that the parse can crash as soon as something weird happens
        if (length < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(length));
        }

        for (var i = 0; i < length; i++)
        {
            yield return forLoop.Invoke();
        }
    }

    /// <summary>
    /// First reads an <see cref="int"/> representing the length, then does a for loop with this length, each element requiring to return an instance of <typeparamref name="T"/>. Instead of array allocation, elements are yielded one by one.
    /// </summary>
    /// <typeparam name="T">Type of the enumerable.</typeparam>
    /// <param name="forLoop">Each element.</param>
    /// <returns>Enumerable of <typeparamref name="T"/>.</returns>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="forLoop"/> is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Length is negative.</exception>
    public IEnumerable<T> ReadEnumerable<T>(Func<T> forLoop)
    {
        if (forLoop is null)
        {
            throw new ArgumentNullException(nameof(forLoop));
        }

        foreach (var x in ReadEnumerable(length: ReadInt32(), forLoop))
        {
            yield return x;
        }
    }

    /// <summary>
    /// Does a for loop with <paramref name="length"/> parameter, each element requiring to return an instance of <typeparamref name="T"/>. Instead of array allocation, elements are yielded one by one.
    /// </summary>
    /// <typeparam name="T">Type of the enumerable.</typeparam>
    /// <param name="length">Length of the enumerable.</param>
    /// <param name="forLoop">Each element with this reader (to avoid closures).</param>
    /// <returns>Enumerable of <typeparamref name="T"/>.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="forLoop"/> is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="length"/> is negative.</exception>
    public IEnumerable<T> ReadEnumerable<T>(int length, Func<GameBoxReader, T> forLoop)
    {
        if (forLoop is null)
        {
            throw new ArgumentNullException(nameof(forLoop));
        }

        for (var i = 0; i < length; i++)
            yield return forLoop.Invoke(this);
    }

    /// <summary>
    /// First reads an <see cref="int"/> representing the length, then does a for loop with this length, each element requiring to return an instance of <typeparamref name="T"/>. Instead of array allocation, elements are yielded one by one.
    /// </summary>
    /// <typeparam name="T">Type of the enumerable.</typeparam>
    /// <param name="forLoop">Each element with this reader (to avoid closures).</param>
    /// <returns>Enumerable of <typeparamref name="T"/>.</returns>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="forLoop"/> is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Length is negative.</exception>
    public IEnumerable<T> ReadEnumerable<T>(Func<GameBoxReader, T> forLoop)
    {
        if (forLoop is null)
        {
            throw new ArgumentNullException(nameof(forLoop));
        }

        foreach (var x in ReadEnumerable(length: ReadInt32(), forLoop))
        {
            yield return x;
        }
    }

    /// <summary>
    /// Does a for loop with <paramref name="length"/> parameter, each element requiring to return an instance of <typeparamref name="T"/>. Instead of array allocation, elements are yielded one by one.
    /// </summary>
    /// <typeparam name="T">Type of the enumerable.</typeparam>
    /// <param name="length">Length of the enumerable.</param>
    /// <param name="forLoop">Each element with an index parameter.</param>
    /// <returns>Enumerable of <typeparamref name="T"/>.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="forLoop"/> is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="length"/> is negative.</exception>
    public static IEnumerable<T> ReadEnumerable<T>(int length, Func<int, T> forLoop)
    {
        if (forLoop is null)
        {
            throw new ArgumentNullException(nameof(forLoop));
        }

        for (var i = 0; i < length; i++)
        {
            yield return forLoop.Invoke(i);
        }
    }

    /// <summary>
    /// First reads an <see cref="int"/> representing the length, then does a for loop with this length, each element requiring to return an instance of <typeparamref name="T"/>. Instead of array allocation, elements are yielded one by one.
    /// </summary>
    /// <typeparam name="T">Type of the enumerable.</typeparam>
    /// <param name="forLoop">Each element with an index parameter.</param>
    /// <returns>Enumerable of <typeparamref name="T"/>.</returns>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="forLoop"/> is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Length is negative.</exception>
    public IEnumerable<T> ReadEnumerable<T>(Func<int, T> forLoop)
    {
        if (forLoop is null)
        {
            throw new ArgumentNullException(nameof(forLoop));
        }

        foreach (var x in ReadEnumerable(length: ReadInt32(), forLoop))
        {
            yield return x;
        }
    }

    /// <summary>
    /// Does a for loop with <paramref name="length"/> parameter, each element requiring to return an instance of <typeparamref name="T"/>. Instead of array allocation, elements are yielded one by one.
    /// </summary>
    /// <typeparam name="T">Type of the enumerable.</typeparam>
    /// <param name="length">Length of the enumerable.</param>
    /// <param name="forLoop">Each element with an index parameter and this reader (to avoid closures).</param>
    /// <returns>Enumerable of <typeparamref name="T"/>.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="forLoop"/> is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Length is negative.</exception>
    public IEnumerable<T> ReadEnumerable<T>(int length, Func<int, GameBoxReader, T> forLoop)
    {
        if (forLoop is null)
        {
            throw new ArgumentNullException(nameof(forLoop));
        }

        for (var i = 0; i < length; i++)
        {
            yield return forLoop.Invoke(i, this);
        }
    }

    /// <summary>
    /// First reads an <see cref="int"/> representing the length, then does a for loop with this length, each element requiring to return an instance of <typeparamref name="T"/>. Instead of array allocation, elements are yielded one by one.
    /// </summary>
    /// <typeparam name="T">Type of the enumerable.</typeparam>
    /// <param name="forLoop">Each element with an index parameter and this reader (to avoid closures).</param>
    /// <returns>Enumerable of <typeparamref name="T"/>.</returns>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="forLoop"/> is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Length is negative.</exception>
    public IEnumerable<T> ReadEnumerable<T>(Func<int, GameBoxReader, T> forLoop)
    {
        if (forLoop is null)
        {
            throw new ArgumentNullException(nameof(forLoop));
        }

        foreach (var x in ReadEnumerable(length: ReadInt32(), forLoop))
        {
            yield return x;
        }
    }
}
