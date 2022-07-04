using System.Runtime.InteropServices;

namespace GBX.NET;

public partial class GameBoxReader
{
    /// <summary>
    /// Reads bytes into a stack-allocated area (decided by <paramref name="length"/>, where <paramref name="lengthInBytes"/> determines the format), then casts the data into <typeparamref name="T"/> structs by using <see cref="MemoryMarshal.Cast{TFrom, TTo}(Span{TFrom})"/>, resulting in more optimized read of array for value types.
    /// </summary>
    /// <typeparam name="T">A struct type.</typeparam>
    /// <param name="length">Length of the array.</param>
    /// <param name="lengthInBytes">If to take length as the size of the byte array and not the <typeparamref name="T"/> array.</param>
    /// <returns>An array of <typeparamref name="T"/>.</returns>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Length is negative.</exception>
    public T[] ReadArray<T>(int length, bool lengthInBytes = false) where T : struct
    {
        if (length < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(length), "Length is negative.");
        }

        if (length == 0)
        {
            return Array.Empty<T>();
        }

        var l = length * (lengthInBytes ? 1 : Marshal.SizeOf<T>());

        if (l > 1_000_000 || l < 0)
        {
            throw new Exception($"Length is too big to handle on the stack ({(l < 0 ? length : l)}).");
        }

#if NETSTANDARD2_1_OR_GREATER || NET6_0_OR_GREATER
        Span<byte> bytes = stackalloc byte[l];
        Read(bytes);
#else
        var bytes = ReadBytes(l);
#endif

        return MemoryMarshal.Cast<byte, T>(bytes).ToArray();
    }

    /// <summary>
    /// First reads an <see cref="int"/> representing the length, then reads bytes into a stack-allocated area (decided by the length, where <paramref name="lengthInBytes"/> determines the format), then casts the data into <typeparamref name="T"/> structs by using <see cref="MemoryMarshal.Cast{TFrom, TTo}(Span{TFrom})"/>, resulting in more optimized read of array for value types.
    /// </summary>
    /// <typeparam name="T">A struct type.</typeparam>
    /// <param name="lengthInBytes">If to take length as the size of the byte array and not the <see cref="Vec3"/> array.</param>
    /// <returns>An array of <typeparamref name="T"/>.</returns>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Length is negative.</exception>
    public T[] ReadArray<T>(bool lengthInBytes = false) where T : struct
    {
        return ReadArray<T>(length: ReadInt32(), lengthInBytes);
    }

    public int[] ReadOptimizedIntArray(int length, int? determineFrom = null)
    {
        if (length == 0)
        {
            return Array.Empty<int>();
        }

        return (uint)determineFrom.GetValueOrDefault(length) switch
        {
            >= ushort.MaxValue => ReadArray<int>(length),
            >= byte.MaxValue => Array.ConvertAll(ReadArray<ushort>(length), x => (int)x),
            _ => Array.ConvertAll(ReadBytes(length), x => (int)x),
        };
    }
    
    public int[] ReadOptimizedIntArray(int? determineFrom = null)
    {
        return ReadOptimizedIntArray(length: ReadInt32(), determineFrom);
    }

    /// <summary>
    /// Does a for loop with <paramref name="length"/> parameter, each element requiring to return an instance of <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">Type of the array.</typeparam>
    /// <param name="length">Length of the array.</param>
    /// <param name="forLoop">Each element with an index parameter.</param>
    /// <returns>An array of <typeparamref name="T"/>.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="forLoop"/> is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="length"/> is negative.</exception>
    public static T[] ReadArray<T>(int length, Func<int, T> forLoop)
    {
        if (forLoop is null)
        {
            throw new ArgumentNullException(nameof(forLoop));
        }

        if (length < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(length));
        }

        if (length == 0)
        {
            return Array.Empty<T>();
        }

        var result = new T[length];

        for (var i = 0; i < length; i++)
        {
            result[i] = forLoop.Invoke(i);
        }

        return result;
    }

    /// <summary>
    /// First reads an <see cref="int"/> representing the length, then does a for loop with this length, each element requiring to return an instance of <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">Type of the array.</typeparam>
    /// <param name="forLoop">Each element with an index parameter.</param>
    /// <returns>An array of <typeparamref name="T"/>.</returns>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="forLoop"/> is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Length is negative.</exception>
    public T[] ReadArray<T>(Func<int, T> forLoop)
    {
        if (forLoop is null)
        {
            throw new ArgumentNullException(nameof(forLoop));
        }

        return ReadArray(length: ReadInt32(), forLoop);
    }

    /// <summary>
    /// Does a for loop with <paramref name="length"/> parameter, each element requiring to return an instance of <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">Type of the array.</typeparam>
    /// <param name="length">Length of the array.</param>
    /// <param name="forLoop">Each element.</param>
    /// <returns>An array of <typeparamref name="T"/>.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="forLoop"/> is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="length"/> is negative.</exception>
    public static T[] ReadArray<T>(int length, Func<T> forLoop)
    {
        if (forLoop is null)
        {
            throw new ArgumentNullException(nameof(forLoop));
        }

        if (length < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(length));
        }

        if (length == 0)
        {
            return Array.Empty<T>();
        }

        var result = new T[length];

        for (var i = 0; i < length; i++)
        {
            result[i] = forLoop.Invoke();
        }

        return result;
    }

    /// <summary>
    /// First reads an <see cref="int"/> representing the length, then does a for loop with this length, each element requiring to return an instance of <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">Type of the array.</typeparam>
    /// <param name="forLoop">Each element.</param>
    /// <returns>An array of <typeparamref name="T"/>.</returns>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="forLoop"/> is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Length is negative.</exception>
    public T[] ReadArray<T>(Func<T> forLoop)
    {
        if (forLoop is null)
        {
            throw new ArgumentNullException(nameof(forLoop));
        }

        return ReadArray(length: ReadInt32(), forLoop);
    }

    /// <summary>
    /// Does a for loop with <paramref name="length"/> parameter, each element requiring to return an instance of <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">Type of the array.</typeparam>
    /// <param name="length">Length of the array.</param>
    /// <param name="forLoop">Each element with an index parameter and this reader (to avoid closures).</param>
    /// <returns>An array of <typeparamref name="T"/>.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="forLoop"/> is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="length"/> is negative.</exception>
    public T[] ReadArray<T>(int length, Func<int, GameBoxReader, T> forLoop)
    {
        if (forLoop is null)
        {
            throw new ArgumentNullException(nameof(forLoop));
        }

        if (length < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(length));
        }

        if (length == 0)
        {
            return Array.Empty<T>();
        }

        var result = new T[length];

        for (var i = 0; i < length; i++)
        {
            result[i] = forLoop.Invoke(i, this);
        }

        return result;
    }

    /// <summary>
    /// First reads an <see cref="int"/> representing the length, then does a for loop with this length, each element requiring to return an instance of <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">Type of the array.</typeparam>
    /// <param name="forLoop">Each element with an index parameter and this reader (to avoid closures).</param>
    /// <returns>An array of <typeparamref name="T"/>.</returns>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="forLoop"/> is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Length is negative.</exception>
    public T[] ReadArray<T>(Func<int, GameBoxReader, T> forLoop)
    {
        if (forLoop is null)
        {
            throw new ArgumentNullException(nameof(forLoop));
        }

        return ReadArray(length: ReadInt32(), forLoop);
    }

    /// <summary>
    /// Does a for loop with <paramref name="length"/> parameter, each element requiring to return an instance of <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">Type of the array.</typeparam>
    /// <param name="length">Length of the array.</param>
    /// <param name="forLoop">Each element with this reader (to avoid closures).</param>
    /// <returns>An array of <typeparamref name="T"/>.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="forLoop"/> is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="length"/> is negative.</exception>
    public T[] ReadArray<T>(int length, Func<GameBoxReader, T> forLoop)
    {
        if (forLoop is null)
        {
            throw new ArgumentNullException(nameof(forLoop));
        }

        if (length == 0)
        {
            return Array.Empty<T>();
        }

        var result = new T[length];

        for (var i = 0; i < length; i++)
        {
            result[i] = forLoop.Invoke(this);
        }

        return result;
    }

    /// <summary>
    /// First reads an <see cref="int"/> representing the length, then does a for loop with this length, each element requiring to return an instance of <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">Type of the array.</typeparam>
    /// <param name="forLoop">Each element with this reader (to avoid closures).</param>
    /// <returns>An array of <typeparamref name="T"/>.</returns>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="forLoop"/> is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Length is negative.</exception>
    public T[] ReadArray<T>(Func<GameBoxReader, T> forLoop)
    {
        if (forLoop is null)
        {
            throw new ArgumentNullException(nameof(forLoop));
        }

        return ReadArray(length: ReadInt32(), forLoop);
    }

    /// <summary>
    /// Does a for loop with <paramref name="length"/> parameter, each element requiring to return an instance of <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">Type of the array.</typeparam>
    /// <param name="length">Length of the array.</param>
    /// <param name="forLoop">Each element with this reader (to avoid closures).</param>
    /// <returns>An array of <typeparamref name="T"/>.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="forLoop"/> is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="length"/> is negative.</exception>
    public async Task<T[]> ReadArrayAsync<T>(int length, Func<GameBoxReader, Task<T>> forLoop)
    {
        if (forLoop is null)
        {
            throw new ArgumentNullException(nameof(forLoop));
        }

        if (length == 0)
        {
            return Array.Empty<T>();
        }

        var result = new T[length];

        for (var i = 0; i < length; i++)
        {
            result[i] = await forLoop.Invoke(this);
        }

        return result;
    }

    /// <summary>
    /// First reads an <see cref="int"/> representing the length, then does a for loop with this length, each element requiring to return an instance of <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">Type of the array.</typeparam>
    /// <param name="forLoop">Each element with this reader (to avoid closures).</param>
    /// <returns>An array of <typeparamref name="T"/>.</returns>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="forLoop"/> is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Length is negative.</exception>
    public async Task<T[]> ReadArrayAsync<T>(Func<GameBoxReader, Task<T>> forLoop)
    {
        if (forLoop is null)
        {
            throw new ArgumentNullException(nameof(forLoop));
        }

        return await ReadArrayAsync(length: ReadInt32(), forLoop);
    }

    /// <summary>
    /// Continues reading the stream until facade (<c>0xFACADE01</c>) is reached and the result is converted into an array of <typeparamref name="T"/>.
    /// </summary>
    /// <returns>An array of <typeparamref name="T"/>.</returns>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="NotSupportedException">The stream does not support seeking.</exception>
    public T[] ReadArrayUntilFacade<T>()
    {
        return CreateArrayForUntilFacade<T>(bytes: ReadUntilFacade().ToArray());
    }

    /// <summary>
    /// Continues reading the stream until facade (<c>0xFACADE01</c>) is reached and the result is converted into an array of <typeparamref name="T1"/> and <typeparamref name="T2"/>.
    /// </summary>
    /// <returns>An tuple of arrays.</returns>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="NotSupportedException">The stream does not support seeking.</exception>
    public (T1[], T2[]) ReadArrayUntilFacade<T1, T2>()
    {
        var bytes = ReadUntilFacade().ToArray();

        return (
            CreateArrayForUntilFacade<T1>(bytes),
            CreateArrayForUntilFacade<T2>(bytes)
        );
    }

    /// <summary>
    /// Continues reading the stream until facade (<c>0xFACADE01</c>) is reached and the result is converted into an array of <typeparamref name="T1"/>, <typeparamref name="T2"/> and <typeparamref name="T3"/>.
    /// </summary>
    /// <returns>An tuple of arrays.</returns>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="NotSupportedException">The stream does not support seeking.</exception>
    public (T1[], T2[], T3[]) ReadArrayUntilFacade<T1, T2, T3>()
    {
        var bytes = ReadUntilFacade().ToArray();

        return (
            CreateArrayForUntilFacade<T1>(bytes),
            CreateArrayForUntilFacade<T2>(bytes),
            CreateArrayForUntilFacade<T3>(bytes)
        );
    }

    private static T[] CreateArrayForUntilFacade<T>(byte[] bytes)
    {
        var array = new T[(int)Math.Ceiling(bytes.Length / (double)Marshal.SizeOf(default(T)))];
        Buffer.BlockCopy(bytes, 0, array, 0, bytes.Length);
        return array;
    }

    /// <summary>
    /// Returns the upcoming array but does not consume it.
    /// </summary>
    /// <returns>An array of <typeparamref name="T"/>.</returns>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="NotSupportedException">The stream does not support seeking.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="length"/> is negative.</exception>
    public T[] PeekArray<T>(int length) where T : struct
    {
        var array = ReadArray<T>(length);
        BaseStream.Position -= length * Marshal.SizeOf(default(T));
        return array;
    }

    /// <summary>
    /// Returns the upcoming array but does not consume it.
    /// </summary>
    /// <param name="length">Length of the array.</param>
    /// <param name="forLoop">Each element with an index parameter.</param>
    /// <returns>An array of <typeparamref name="T"/>.</returns>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="NotSupportedException">The stream does not support seeking.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="length"/> is negative.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="forLoop"/> is null.</exception>
    public T[] PeekArray<T>(int length, Func<int, T> forLoop)
    {
        if (forLoop is null)
        {
            throw new ArgumentNullException(nameof(forLoop));
        }

        var beforePos = BaseStream.Position;

        var array = ReadArray(length, forLoop);

        BaseStream.Position = beforePos;

        return array;
    }
}
