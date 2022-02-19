namespace GBX.NET;

public partial class GameBoxReader
{
    /// <summary>
    /// Reads an <see cref="int"/>, which is then presented as time in milliseconds.
    /// </summary>
    /// <returns>A <see cref="TimeInt32"/>.</returns>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    public TimeInt32 ReadTimeInt32() => new(TotalMilliseconds: ReadInt32());

    /// <summary>
    /// Reads an <see cref="int"/>, which is then presented as time in milliseconds. If value is -1, a null is returned instead.
    /// </summary>
    /// <returns>A <see cref="TimeInt32"/>. Null if the read value is -1.</returns>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    public TimeInt32? ReadTimeInt32Nullable()
    {
        var totalMilliseconds = ReadInt32();
        if (totalMilliseconds < 0) return null;
        return new TimeInt32(totalMilliseconds);
    }

    /// <summary>
    /// Reads a <see cref="float"/>, which is then presented as time in seconds.
    /// </summary>
    /// <returns>A <see cref="TimeSingle"/>.</returns>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    public TimeSingle ReadTimeSingle() => new(TotalSeconds: ReadSingle());

    /// <summary>
    /// Reads a <see cref="float"/>, which is then presented as time in seconds. If value is -1, a null is returned instead.
    /// </summary>
    /// <returns>A <see cref="TimeSingle"/>. Null if the read value is -1.</returns>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    public TimeSingle? ReadTimeSingleNullable()
    {
        var totalSeconds = ReadSingle();
        if (totalSeconds < 0) return null;
        return new TimeSingle(totalSeconds);
    }

    /// <summary>
    /// Reads an <see cref="int"/>, which is then presented as time in seconds.
    /// </summary>
    /// <returns>A <see cref="TimeInt32"/>.</returns>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    public TimeInt32 ReadInt32_s() => TimeInt32.FromSeconds(ReadInt32());

    /// <summary>
    /// Reads an <see cref="int"/>, which is then presented as time in milliseconds.
    /// </summary>
    /// <returns>A <see cref="TimeInt32"/>.</returns>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    [Obsolete("Prefer using ReadTimeInt32()")]
    public TimeInt32 ReadInt32_ms() => TimeInt32.FromMilliseconds(ReadInt32());

    /// <summary>
    /// Reads an <see cref="int"/>, which is then presented as time in seconds. If value is -1, a null is returned instead.
    /// </summary>
    /// <returns>A <see cref="TimeInt32"/>. Null if the read value is -1.</returns>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    public TimeInt32? ReadInt32_sn()
    {
        var time = ReadInt32();
        if (time < 0) return null;
        return TimeInt32.FromSeconds(time);
    }

    /// <summary>
    /// Reads an <see cref="int"/>, which is then presented as time in milliseconds. If value is -1, a null is returned instead.
    /// </summary>
    /// <returns>A <see cref="TimeInt32"/>. Null if the read value is -1.</returns>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    [Obsolete("Prefer using ReadTimeInt32Nullable()")]
    public TimeInt32? ReadInt32_msn()
    {
        var time = ReadInt32();
        if (time < 0) return null;
        return TimeInt32.FromMilliseconds(time);
    }

    /// <summary>
    /// Reads a <see cref="float"/>, which is then presented as time in seconds.
    /// </summary>
    /// <returns>A <see cref="TimeSingle"/>.</returns>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    [Obsolete("Prefer using ReadTimeSingle()")]
    public TimeSingle ReadSingle_s() => TimeSingle.FromSeconds(ReadSingle());

    /// <summary>
    /// Reads a <see cref="float"/>, which is then presented as time in milliseconds.
    /// </summary>
    /// <returns>A <see cref="TimeSingle"/>.</returns>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    public TimeSingle ReadSingle_ms() => TimeSingle.FromMilliseconds(ReadSingle());

    /// <summary>
    /// Reads a <see cref="float"/>, which is then presented as time in seconds. If value is -1, a null is returned instead.
    /// </summary>
    /// <returns>A <see cref="TimeSingle"/>. Null if the read value is -1.</returns>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    [Obsolete("Prefer using ReadTimeSingleNullable()")]
    public TimeSingle? ReadSingle_sn()
    {
        var time = ReadSingle();
        if (time < 0) return null;
        return TimeSingle.FromSeconds(time);
    }

    /// <summary>
    /// Reads a <see cref="float"/>, which is then presented as time in milliseconds. If value is -1, a null is returned instead.
    /// </summary>
    /// <returns>A <see cref="TimeSingle"/>. Null if the read value is -1.</returns>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    public TimeSingle? ReadSingle_msn()
    {
        var time = ReadSingle();
        if (time < 0) return null;
        return TimeSingle.FromMilliseconds(time);
    }
}
