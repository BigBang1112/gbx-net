namespace GBX.NET;

public partial class GameBoxReader
{
    /// <summary>
    /// Reads a 4-byte signed integer from the current stream and advances the current position of the stream by four bytes.
    /// The integer is then presented as time in seconds.
    /// </summary>
    /// <returns>A TimeSpan converted from the integer.</returns>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    public TimeSpan ReadInt32_s() => TimeSpan.FromSeconds(ReadInt32());

    /// <summary>
    /// Reads a 4-byte signed integer from the current stream and advances the current position of the stream by four bytes.
    /// The integer is then presented as time in milliseconds.
    /// </summary>
    /// <returns>A TimeSpan converted from the integer.</returns>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    public TimeSpan ReadInt32_ms() => TimeSpan.FromMilliseconds(ReadInt32());

    /// <summary>
    /// Reads a 4-byte signed integer from the current stream and advances the current position of the stream by four bytes.
    /// The integer is then presented as time in seconds. If value is -1, a null is returned instead.
    /// </summary>
    /// <returns>A TimeSpan converted from the integer. Null if the read value is -1.</returns>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    public TimeSpan? ReadInt32_sn()
    {
        var time = ReadInt32();
        if (time < 0)
            return null;
        return TimeSpan.FromSeconds(time);
    }

    /// <summary>
    /// Reads a 4-byte signed integer from the current stream and advances the current position of the stream by four bytes.
    /// The integer is then presented as time in milliseconds. If value is -1, a null is returned instead.
    /// </summary>
    /// <returns>A TimeSpan converted from the integer. Null if the read value is -1.</returns>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    public TimeSpan? ReadInt32_msn()
    {
        var time = ReadInt32();
        if (time < 0)
            return null;
        return TimeSpan.FromMilliseconds(time);
    }

    /// <summary>
    /// Reads a 4-byte floating point value from the current stream and advances the current position of the stream by four bytes.
    /// The floating point value is then presented as time in seconds.
    /// </summary>
    /// <returns>A TimeSpan converted from the floating point value.</returns>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    public TimeSpan ReadSingle_s() => TimeSpan.FromSeconds(ReadSingle());

    /// <summary>
    /// Reads a 4-byte floating point value from the current stream and advances the current position of the stream by four bytes.
    /// The floating point value is then presented as time in milliseconds.
    /// </summary>
    /// <returns>A TimeSpan converted from the floating point value.</returns>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    public TimeSpan ReadSingle_ms() => TimeSpan.FromMilliseconds(ReadSingle());

    /// <summary>
    /// Reads a 4-byte floating point value from the current stream and advances the current position of the stream by four bytes.
    /// The floating point value is then presented as time in seconds. If value is -1, a null is returned instead.
    /// </summary>
    /// <returns>A TimeSpan converted from the floating point value. Null if the read value is -1.</returns>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    public TimeSpan? ReadSingle_sn()
    {
        var time = ReadSingle();
        if (time < 0)
            return null;
        return TimeSpan.FromSeconds(time);
    }

    /// <summary>
    /// Reads a 4-byte floating point value from the current stream and advances the current position of the stream by four bytes.
    /// The floating point value is then presented as time in milliseconds. If value is -1, a null is returned instead.
    /// </summary>
    /// <returns>A TimeSpan converted from the floating point value. Null if the read value is -1.</returns>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    public TimeSpan? ReadSingle_msn()
    {
        var time = ReadSingle();
        if (time < 0)
            return null;
        return TimeSpan.FromMilliseconds(time);
    }
}
