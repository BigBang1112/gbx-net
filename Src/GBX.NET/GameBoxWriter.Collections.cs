using System.Runtime.InteropServices;

namespace GBX.NET;

public partial class GameBoxWriter
{
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    public void WriteArray<T>(T[]? array) where T : struct
    {
        if (array is null)
        {
            Write(0);
            return;
        }

        Write(array.Length);
        WriteArray_NoPrefix(array);
    }

    /// <exception cref="ArgumentNullException"><paramref name="array"/> is null.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    public void WriteArray_NoPrefix<T>(T[] array) where T : struct
    {
        if (array is null)
        {
            throw new ArgumentNullException(nameof(array));
        }

#if NET6_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
        Write(MemoryMarshal.Cast<T, byte>(array));
#else
        Write(MemoryMarshal.Cast<T, byte>(array).ToArray());
#endif
    }

    /// <summary>
    /// First writes an <see cref="int"/> representing the length, then does a for loop with this length, each yield having an option to write something from <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">Type of the array.</typeparam>
    /// <param name="array">An array.</param>
    /// <param name="forLoop">Each element.</param>
    /// <exception cref="ArgumentNullException"><paramref name="forLoop"/> is null.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    public void WriteArray<T>(T[]? array, Action<T> forLoop)
    {
        if (forLoop is null)
        {
            throw new ArgumentNullException(nameof(forLoop));
        }

        if (array is null)
        {
            Write(0);
            return;
        }

        Write(array.Length);

        for (var i = 0; i < array.Length; i++)
        {
            forLoop.Invoke(array[i]);
        }
    }

    /// <exception cref="ArgumentNullException"><paramref name="forLoop"/> is null.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    public void WriteArray<T>(T[]? array, Action<T, GameBoxWriter> forLoop)
    {
        if (forLoop is null)
        {
            throw new ArgumentNullException(nameof(forLoop));
        }

        if (array is null)
        {
            Write(0);
            return;
        }

        Write(array.Length);

        for (var i = 0; i < array.Length; i++)
        {
            forLoop.Invoke(array[i], this);
        }
    }

    /// <exception cref="ArgumentNullException"><paramref name="forLoop"/> is null.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    public void WriteList<T>(IList<T>? list, Action<T> forLoop)
    {
        if (forLoop is null)
        {
            throw new ArgumentNullException(nameof(forLoop));
        }

        if (list is null)
        {
            Write(0);
            return;
        }

        Write(list.Count);

        for (var i = 0; i < list.Count; i++)
        {
            forLoop.Invoke(list[i]);
        }
    }

    /// <exception cref="ArgumentNullException"><paramref name="forLoop"/> is null.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    public void WriteList<T>(IList<T>? list, Action<T, GameBoxWriter> forLoop)
    {
        if (forLoop is null)
        {
            throw new ArgumentNullException(nameof(forLoop));
        }

        if (list is null)
        {
            Write(0);
            return;
        }

        Write(list.Count);

        for (var i = 0; i < list.Count; i++)
        {
            forLoop.Invoke(list[i], this);
        }
    }

    /// <exception cref="ArgumentNullException"><paramref name="forLoop"/> is null.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="OverflowException">The number of elements in source is larger than <see cref="int.MaxValue"/>.</exception>
    public void WriteEnumerable<T>(IEnumerable<T>? enumerable, Action<T> forLoop)
    {
        if (forLoop is null)
            throw new ArgumentNullException(nameof(forLoop));

        if (enumerable is null)
        {
            Write(0);
            return;
        }

        var count = enumerable.Count();

        Write(count);

        foreach (var element in enumerable)
        {
            forLoop.Invoke(element);
        }
    }

    /// <exception cref="ArgumentNullException">Key or value in dictionary is null.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    public void WriteDictionary<TKey, TValue>(IDictionary<TKey, TValue>? dictionary)
    {
        if (dictionary is null)
        {
            Write(0);
            return;
        }

        Write(dictionary.Count);

        foreach (var pair in dictionary)
        {
            if (pair.Key is null)
            {
                throw new ArgumentNullException(nameof(pair.Key));
            }

            if (pair.Value is null)
            {
                throw new ArgumentNullException(nameof(pair.Value));
            }

            WriteAny(pair.Key);
            WriteAny(pair.Value);
        }
    }
}
