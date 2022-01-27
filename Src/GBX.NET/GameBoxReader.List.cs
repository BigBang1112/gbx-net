namespace GBX.NET;

public partial class GameBoxReader
{
    /// <summary>
    /// Does a for loop with <paramref name="length"/> parameter, each element requiring to return an instance of <typeparamref name="T"/>. A list is allocated and elements are added via enumeration.
    /// </summary>
    /// <typeparam name="T">Type of the list.</typeparam>
    /// <param name="length">Length of the list.</param>
    /// <param name="forLoop">Each element.</param>
    /// <returns>List of <typeparamref name="T"/>.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="forLoop"/> is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="length"/> is negative.</exception>
    public static IList<T> ReadList<T>(int length, Func<T> forLoop)
    {
        return ReadEnumerable(length, forLoop).ToList(length);
    }

    /// <summary>
    /// First reads an <see cref="int"/> representing the length, then does a for loop with this length, each element requiring to return an instance of <typeparamref name="T"/>. A list is allocated and elements are added via enumeration.
    /// </summary>
    /// <typeparam name="T">Type of the list.</typeparam>
    /// <param name="forLoop">Each element.</param>
    /// <returns>List of <typeparamref name="T"/>.</returns>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="forLoop"/> is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Length is negative.</exception>
    public IList<T> ReadList<T>(Func<T> forLoop)
    {
        return ReadList(length: ReadInt32(), forLoop);
    }

    /// <summary>
    /// Does a for loop with <paramref name="length"/> parameter, each element requiring to return an instance of <typeparamref name="T"/>. A list is allocated and elements are added via enumeration.
    /// </summary>
    /// <typeparam name="T">Type of the list.</typeparam>
    /// <param name="length">Length of the list.</param>
    /// <param name="forLoop">Each element with this reader (to avoid closures).</param>
    /// <returns>List of <typeparamref name="T"/>.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="forLoop"/> is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="length"/> is negative.</exception>
    public IList<T> ReadList<T>(int length, Func<GameBoxReader, T> forLoop)
    {
        return ReadEnumerable(length, forLoop).ToList(length);
    }

    /// <summary>
    /// First reads an <see cref="int"/> representing the length, then does a for loop with this length, each element requiring to return an instance of <typeparamref name="T"/>. A list is allocated and elements are added via enumeration.
    /// </summary>
    /// <typeparam name="T">Type of the list.</typeparam>
    /// <param name="forLoop">Each element with this reader (to avoid closures).</param>
    /// <returns>List of <typeparamref name="T"/>.</returns>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="forLoop"/> is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Length is negative.</exception>
    public IList<T> ReadList<T>(Func<GameBoxReader, T> forLoop)
    {
        return ReadList(length: ReadInt32(), forLoop);
    }

    /// <summary>
    /// Does a for loop with <paramref name="length"/> parameter, each element requiring to return an instance of <typeparamref name="T"/>. A list is allocated and elements are added via enumeration.
    /// </summary>
    /// <typeparam name="T">Type of the list.</typeparam>
    /// <param name="length">Length of the list.</param>
    /// <param name="forLoop">Each element with an index parameter.</param>
    /// <returns>List of <typeparamref name="T"/>.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="forLoop"/> is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="length"/> is negative.</exception>
    public static IList<T> ReadList<T>(int length, Func<int, T> forLoop)
    {
        return ReadEnumerable(length, forLoop).ToList(length);
    }

    /// <summary>
    /// First reads an <see cref="int"/> representing the length, then does a for loop with this length, each element requiring to return an instance of <typeparamref name="T"/>. A list is allocated and elements are added via enumeration.
    /// </summary>
    /// <typeparam name="T">Type of the list.</typeparam>
    /// <param name="forLoop">Each element with an index parameter.</param>
    /// <returns>List of <typeparamref name="T"/>.</returns>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="forLoop"/> is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Length is negative.</exception>
    public IList<T> ReadList<T>(Func<int, T> forLoop)
    {
        return ReadList(length: ReadInt32(), forLoop);
    }

    /// <summary>
    /// Does a for loop with <paramref name="length"/> parameter, each element requiring to return an instance of <typeparamref name="T"/>. A list is allocated and elements are added via enumeration.
    /// </summary>
    /// <typeparam name="T">Type of the list.</typeparam>
    /// <param name="length">Length of the list.</param>
    /// <param name="forLoop">Each element with an index parameter and this reader (to avoid closures).</param>
    /// <returns>List of <typeparamref name="T"/>.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="forLoop"/> is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Length is negative.</exception>
    public IList<T> ReadList<T>(int length, Func<int, GameBoxReader, T> forLoop)
    {
        return ReadEnumerable(length, forLoop).ToList(length);
    }

    /// <summary>
    /// First reads an <see cref="int"/> representing the length, then does a for loop with this length, each element requiring to return an instance of <typeparamref name="T"/>. A list is allocated and elements are added via enumeration.
    /// </summary>
    /// <typeparam name="T">Type of the list.</typeparam>
    /// <param name="forLoop">Each element with an index parameter and this reader (to avoid closures).</param>
    /// <returns>List of <typeparamref name="T"/>.</returns>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="forLoop"/> is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Length is negative.</exception>
    public IList<T> ReadList<T>(Func<int, GameBoxReader, T> forLoop)
    {
        return ReadList(length: ReadInt32(), forLoop);
    }
}
