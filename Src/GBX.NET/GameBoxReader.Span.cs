using System.Runtime.InteropServices;

namespace GBX.NET;

public partial class GameBoxReader
{
    /// <summary>
    /// Reads a <typeparamref name="T"/> span with the <paramref name="length"/> (<paramref name="lengthInBytes"/> determines the format) by using <see cref="MemoryMarshal.Cast{TFrom, TTo}(Span{TFrom})"/>, resulting in more optimized read of array for value types.
    /// </summary>
    /// <typeparam name="T">A struct type.</typeparam>
    /// <param name="length">Length of the array.</param>
    /// <param name="lengthInBytes">If to take length as the size of the byte array and not the <see cref="Vec3"/> array.</param>
    /// <returns>A span of <typeparamref name="T"/>.</returns>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Length is negative.</exception>
    [Obsolete("Prefer using ReadStructArray, turning the Span into an array will double the memory allocation.")]
    public Span<T> ReadSpan<T>(int length, bool lengthInBytes = false) where T : struct
    {
        var l = length * (lengthInBytes ? 1 : Marshal.SizeOf<T>());

#if NETSTANDARD2_1_OR_GREATER || NET6_0_OR_GREATER
        var bytes = new byte[l];
        Read(bytes);
#else
        var bytes = ReadBytes(l);
#endif

        return MemoryMarshal.Cast<byte, T>(bytes);
    }

    /// <summary>
    /// First reads an <see cref="int"/> representing the length, then reads a <typeparamref name="T"/> span with the length (<paramref name="lengthInBytes"/> determines the format) by using <see cref="MemoryMarshal.Cast{TFrom, TTo}(Span{TFrom})"/>, resulting in more optimized read of array for value types.
    /// </summary>
    /// <typeparam name="T">A struct type.</typeparam>
    /// <param name="lengthInBytes">If to take length as the size of the byte array and not the <see cref="Vec3"/> array.</param>
    /// <returns>A span of <typeparamref name="T"/>.</returns>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Length is negative.</exception>
    [Obsolete("Prefer using ReadStructArray, turning the Span into an array will double the memory allocation.")]
    public Span<T> ReadSpan<T>(bool lengthInBytes = false) where T : struct
    {
        return ReadSpan<T>(length: ReadInt32(), lengthInBytes);
    }
}
