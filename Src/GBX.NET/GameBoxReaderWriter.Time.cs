namespace GBX.NET;

public partial class GameBoxReaderWriter
{
    /// <summary>
    /// Reads or writes an <see cref="int"/> as a <see cref="TmEssentials.TimeInt32"/> of milliseconds.
    /// </summary>
    /// <remarks>A regular <see cref="int"/> is read/written but converted to/from <see cref="TmEssentials.TimeInt32"/> using <see cref="TimeInt32.FromMilliseconds(float)"/>.</remarks>
    /// <param name="variable">Variable to write. Ignored in read mode.</param>
    /// <returns>Value read in read mode. In write mode, <paramref name="variable"/> is returned.</returns>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    public TimeInt32 TimeInt32(TimeInt32 variable = default)
    {
        if (Reader is not null) return Reader.ReadTimeInt32();
        if (Writer is not null) Writer.WriteTimeInt32(variable);
        return variable;
    }

    /// <summary>
    /// Reads or writes a nullable <see cref="int"/> as a nullable <see cref="TmEssentials.TimeInt32"/> of milliseconds.
    /// </summary>
    /// <remarks>A regular <see cref="int"/> is read/written but converted to/from <see cref="TmEssentials.TimeInt32"/> using <see cref="TimeInt32.FromMilliseconds(float)"/>.</remarks>
    /// <param name="variable">Variable to write. If null, <paramref name="defaultValue"/> is written. Ignored in read mode.</param>
    /// <param name="defaultValue">Value written when <paramref name="variable"/> is null. Ignored in read mode.</param>
    /// <returns>Value read in read mode. In write mode, <paramref name="variable"/> is returned (including null).</returns>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    public TimeInt32? TimeInt32(TimeInt32? variable, TimeInt32 defaultValue = default)
    {
        if (Reader is not null) return Reader.ReadTimeInt32();
        if (Writer is not null) Writer.WriteTimeInt32(variable ?? defaultValue);
        return variable;
    }

    /// <summary>
    /// Reads or writes an <see cref="int"/> as a <see cref="TmEssentials.TimeInt32"/> of milliseconds through reference.
    /// </summary>
    /// <remarks>A regular <see cref="int"/> is read/written but converted to/from <see cref="TmEssentials.TimeInt32"/> using <see cref="TimeInt32.FromMilliseconds(float)"/>.</remarks>
    /// <param name="variable">Variable to read or write. Read mode sets <paramref name="variable"/>, write mode uses <paramref name="variable"/> to write the value (keeping <paramref name="variable"/> unchanged).</param>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    public void TimeInt32(ref TimeInt32 variable) => variable = TimeInt32(variable);

    /// <summary>
    /// Reads or writes a nullable <see cref="int"/> as a nullable <see cref="TmEssentials.TimeInt32"/> of milliseconds through reference.
    /// </summary>
    /// <remarks>A regular <see cref="int"/> is read/written but converted to/from <see cref="TmEssentials.TimeInt32"/> using <see cref="TimeInt32.FromMilliseconds(float)"/>.</remarks>
    /// <param name="variable">Variable to read or write. Read mode sets <paramref name="variable"/>, write mode uses <paramref name="variable"/> to write the value (keeping <paramref name="variable"/> unchanged). If <paramref name="variable"/> is null, <paramref name="defaultValue"/> is written instead.</param>
    /// <param name="defaultValue">Value written when <paramref name="variable"/> is null. Ignored in read mode.</param>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    public void TimeInt32(ref TimeInt32? variable, TimeInt32 defaultValue = default) => variable = TimeInt32(variable, defaultValue);

    /// <summary>
    /// Reads or writes an <see cref="int"/> as a <see cref="TmEssentials.TimeInt32"/> of milliseconds. If the read value is -1, null is returned. If the written <paramref name="variable"/> is null, -1 value is written.
    /// </summary>
    /// <remarks>A regular <see cref="int"/> is read/written but converted to/from <see cref="TmEssentials.TimeInt32"/> using <see cref="TimeInt32.FromMilliseconds(float)"/> (except for -1).</remarks>
    /// <param name="variable">Variable to write. Ignored in read mode.</param>
    /// <returns>Value read in read mode. In write mode, <paramref name="variable"/> is returned. If the read value is -1, null is returned.</returns>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    public TimeInt32? TimeInt32Nullable(TimeInt32? variable = default)
    {
        if (Reader is not null) return Reader.ReadTimeInt32Nullable();
        if (Writer is not null) Writer.WriteTimeInt32Nullable(variable);
        return variable;
    }

    /// <summary>
    /// Reads or writes an <see cref="int"/> as a <see cref="TmEssentials.TimeInt32"/> of milliseconds through reference. If the read value is -1, <paramref name="variable"/> is set to null. If the written <paramref name="variable"/> is null, -1 value is written.
    /// </summary>
    /// <remarks>A regular <see cref="int"/> is read/written but converted to/from <see cref="TmEssentials.TimeInt32"/> using <see cref="TimeInt32.FromMilliseconds(float)"/> (except for -1).</remarks>
    /// <param name="variable">Variable to read or write. Read mode sets <paramref name="variable"/>, write mode uses <paramref name="variable"/> to write the value (keeping <paramref name="variable"/> unchanged).</param>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    public void TimeInt32Nullable(ref TimeInt32? variable) => variable = TimeInt32Nullable(variable);

    /// <summary>
    /// Reads or writes a <see cref="float"/> as a <see cref="TmEssentials.TimeSingle"/> of seconds.
    /// </summary>
    /// <remarks>A regular <see cref="float"/> is read/written but converted to/from <see cref="TmEssentials.TimeSingle"/> using <see cref="TimeSingle.FromSeconds(float)"/>.</remarks>
    /// <param name="variable">Variable to write. Ignored in read mode.</param>
    /// <returns>Value read in read mode. In write mode, <paramref name="variable"/> is returned.</returns>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    public TimeSingle TimeSingle(TimeSingle variable = default)
    {
        if (Reader is not null) return Reader.ReadTimeSingle();
        if (Writer is not null) Writer.WriteTimeSingle(variable);
        return variable;
    }

    /// <summary>
    /// Reads or writes a nullable <see cref="float"/> as a nullable <see cref="TmEssentials.TimeSingle"/> of seconds.
    /// </summary>
    /// <remarks>A regular <see cref="float"/> is read/written but converted to/from <see cref="TmEssentials.TimeSingle"/> using <see cref="TimeSingle.FromSeconds(float)"/>.</remarks>
    /// <param name="variable">Variable to write. If null, <paramref name="defaultValue"/> is written. Ignored in read mode.</param>
    /// <param name="defaultValue">Value written when <paramref name="variable"/> is null. Ignored in read mode.</param>
    /// <returns>Value read in read mode. In write mode, <paramref name="variable"/> is returned (including null).</returns>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    public TimeSingle? TimeSingle(TimeSingle? variable, TimeSingle defaultValue = default)
    {
        if (Reader is not null) return Reader.ReadTimeSingle();
        if (Writer is not null) Writer.WriteTimeSingle(variable ?? defaultValue);
        return variable;
    }

    /// <summary>
    /// Reads or writes a <see cref="float"/> as a <see cref="TmEssentials.TimeSingle"/> of seconds through reference.
    /// </summary>
    /// <remarks>A regular <see cref="float"/> is read/written but converted to/from <see cref="TmEssentials.TimeSingle"/> using <see cref="TimeSingle.FromSeconds(float)"/>.</remarks>
    /// <param name="variable">Variable to read or write. Read mode sets <paramref name="variable"/>, write mode uses <paramref name="variable"/> to write the value (keeping <paramref name="variable"/> unchanged).</param>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    public void TimeSingle(ref TimeSingle variable) => variable = TimeSingle(variable);

    /// <summary>
    /// Reads or writes a nullable <see cref="float"/> as a nullable <see cref="TmEssentials.TimeSingle"/> of seconds through reference.
    /// </summary>
    /// <remarks>A regular <see cref="float"/> is read/written but converted to/from <see cref="TmEssentials.TimeSingle"/> using <see cref="TimeSingle.FromSeconds(float)"/>.</remarks>
    /// <param name="variable">Variable to read or write. Read mode sets <paramref name="variable"/>, write mode uses <paramref name="variable"/> to write the value (keeping <paramref name="variable"/> unchanged). If <paramref name="variable"/> is null, <paramref name="defaultValue"/> is written instead.</param>
    /// <param name="defaultValue">Value written when <paramref name="variable"/> is null. Ignored in read mode.</param>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    public void TimeSingle(ref TimeSingle? variable, TimeSingle defaultValue = default) => variable = TimeSingle(variable, defaultValue);

    /// <summary>
    /// Reads or writes an <see cref="float"/> as a <see cref="TmEssentials.TimeSingle"/> of seconds. If the read value is -1, null is returned. If the written <paramref name="variable"/> is null, -1 value is written.
    /// </summary>
    /// <remarks>A regular <see cref="float"/> is read/written but converted to/from <see cref="TmEssentials.TimeSingle"/> using <see cref="TimeSingle.FromSeconds(float)"/> (except for -1).</remarks>
    /// <param name="variable">Variable to write. Ignored in read mode.</param>
    /// <returns>Value read in read mode. In write mode, <paramref name="variable"/> is returned. If the read value is -1, null is returned.</returns>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    public TimeSingle? TimeSingleNullable(TimeSingle? variable = default)
    {
        if (Reader is not null) return Reader.ReadTimeSingleNullable();
        if (Writer is not null) Writer.WriteTimeSingleNullable(variable);
        return variable;
    }

    /// <summary>
    /// Reads or writes an <see cref="float"/> as a <see cref="TmEssentials.TimeSingle"/> of seconds through reference. If the read value is -1, <paramref name="variable"/> is set to null. If the written <paramref name="variable"/> is null, -1 value is written.
    /// </summary>
    /// <remarks>A regular <see cref="float"/> is read/written but converted to/from <see cref="TmEssentials.TimeSingle"/> using <see cref="TimeSingle.FromSeconds(float)"/> (except for -1).</remarks>
    /// <param name="variable">Variable to read or write. Read mode sets <paramref name="variable"/>, write mode uses <paramref name="variable"/> to write the value (keeping <paramref name="variable"/> unchanged).</param>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    public void TimeSingleNullable(ref TimeSingle? variable) => variable = TimeSingleNullable(variable);

    /// <summary>
    /// Reads or writes an <see cref="int"/> as an <see cref="TmEssentials.TimeInt32"/> of seconds.
    /// </summary>
    /// <remarks>A regular <see cref="int"/> is read/written but converted to/from <see cref="TmEssentials.TimeInt32"/> using <see cref="TimeInt32.FromSeconds(float)"/>.</remarks>
    /// <param name="variable">Variable to write. Ignored in read mode.</param>
    /// <returns>Value read in read mode. In write mode, <paramref name="variable"/> is returned.</returns>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    public TimeInt32 Int32_s(TimeInt32 variable = default)
    {
        if (Reader is not null) return Reader.ReadInt32_s();
        if (Writer is not null) Writer.WriteInt32_s(variable);
        return variable;
    }

    /// <summary>
    /// Reads or writes a nullable <see cref="int"/> as a nullable <see cref="TmEssentials.TimeInt32"/> of seconds.
    /// </summary>
    /// <remarks>A regular <see cref="int"/> is read/written but converted to/from <see cref="TmEssentials.TimeInt32"/> using <see cref="TimeInt32.FromSeconds(float)"/>.</remarks>
    /// <param name="variable">Variable to write. If null, <paramref name="defaultValue"/> is written. Ignored in read mode.</param>
    /// <param name="defaultValue">Value written when <paramref name="variable"/> is null. Ignored in read mode.</param>
    /// <returns>Value read in read mode. In write mode, <paramref name="variable"/> is returned (including null).</returns>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    public TimeInt32? Int32_s(TimeInt32? variable, TimeInt32 defaultValue = default)
    {
        if (Reader is not null) return Reader.ReadInt32_s();
        if (Writer is not null) Writer.WriteInt32_s(variable ?? defaultValue);
        return variable;
    }

    /// <summary>
    /// Reads or writes an <see cref="int"/> as a <see cref="TmEssentials.TimeInt32"/> of seconds through reference.
    /// </summary>
    /// <remarks>A regular <see cref="int"/> is read/written but converted to/from <see cref="TmEssentials.TimeInt32"/> using <see cref="TimeInt32.FromSeconds(float)"/>.</remarks>
    /// <param name="variable">Variable to read or write. Read mode sets <paramref name="variable"/>, write mode uses <paramref name="variable"/> to write the value (keeping <paramref name="variable"/> unchanged).</param>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    public void Int32_s(ref TimeInt32 variable) => variable = Int32_s(variable);

    /// <summary>
    /// Reads or writes a nullable <see cref="int"/> as a nullable <see cref="TmEssentials.TimeInt32"/> of seconds through reference.
    /// </summary>
    /// <remarks>A regular <see cref="int"/> is read/written but converted to/from <see cref="TmEssentials.TimeInt32"/> using <see cref="TimeInt32.FromSeconds(float)"/>.</remarks>
    /// <param name="variable">Variable to read or write. Read mode sets <paramref name="variable"/>, write mode uses <paramref name="variable"/> to write the value (keeping <paramref name="variable"/> unchanged). If <paramref name="variable"/> is null, <paramref name="defaultValue"/> is written instead.</param>
    /// <param name="defaultValue">Value written when <paramref name="variable"/> is null. Ignored in read mode.</param>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    public void Int32_s(ref TimeInt32? variable, TimeInt32 defaultValue = default) => variable = Int32_s(variable, defaultValue);

    /// <summary>
    /// Reads or writes an <see cref="int"/> as a <see cref="TmEssentials.TimeInt32"/> of milliseconds.
    /// </summary>
    /// <remarks>A regular <see cref="int"/> is read/written but converted to/from <see cref="TmEssentials.TimeInt32"/> using <see cref="TimeInt32.FromMilliseconds(float)"/>.</remarks>
    /// <param name="variable">Variable to write. Ignored in read mode.</param>
    /// <returns>Value read in read mode. In write mode, <paramref name="variable"/> is returned.</returns>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    [Obsolete("Prefer using TimeInt32()")]
    public TimeInt32 Int32_ms(TimeInt32 variable = default)
    {
        if (Reader is not null) return Reader.ReadInt32_ms();
        if (Writer is not null) Writer.WriteInt32_ms(variable);
        return variable;
    }

    /// <summary>
    /// Reads or writes a nullable <see cref="int"/> as a nullable <see cref="TmEssentials.TimeInt32"/> of milliseconds.
    /// </summary>
    /// <remarks>A regular <see cref="int"/> is read/written but converted to/from <see cref="TmEssentials.TimeInt32"/> using <see cref="TimeInt32.FromMilliseconds(float)"/>.</remarks>
    /// <param name="variable">Variable to write. If null, <paramref name="defaultValue"/> is written. Ignored in read mode.</param>
    /// <param name="defaultValue">Value written when <paramref name="variable"/> is null. Ignored in read mode.</param>
    /// <returns>Value read in read mode. In write mode, <paramref name="variable"/> is returned (including null).</returns>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    [Obsolete("Prefer using TimeInt32()")]
    public TimeInt32? Int32_ms(TimeInt32? variable, TimeInt32 defaultValue = default)
    {
        if (Reader is not null) return Reader.ReadInt32_ms();
        if (Writer is not null) Writer.WriteInt32_ms(variable ?? defaultValue);
        return variable;
    }

    /// <summary>
    /// Reads or writes an <see cref="int"/> as a <see cref="TmEssentials.TimeInt32"/> of milliseconds through reference.
    /// </summary>
    /// <remarks>A regular <see cref="int"/> is read/written but converted to/from <see cref="TmEssentials.TimeInt32"/> using <see cref="TimeInt32.FromMilliseconds(float)"/>.</remarks>
    /// <param name="variable">Variable to read or write. Read mode sets <paramref name="variable"/>, write mode uses <paramref name="variable"/> to write the value (keeping <paramref name="variable"/> unchanged).</param>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    [Obsolete("Prefer using TimeInt32()")]
    public void Int32_ms(ref TimeInt32 variable) => variable = Int32_ms(variable);

    /// <summary>
    /// Reads or writes a nullable <see cref="int"/> as a nullable <see cref="TmEssentials.TimeInt32"/> of milliseconds through reference.
    /// </summary>
    /// <remarks>A regular <see cref="int"/> is read/written but converted to/from <see cref="TmEssentials.TimeInt32"/> using <see cref="TimeInt32.FromMilliseconds(float)"/>.</remarks>
    /// <param name="variable">Variable to read or write. Read mode sets <paramref name="variable"/>, write mode uses <paramref name="variable"/> to write the value (keeping <paramref name="variable"/> unchanged). If <paramref name="variable"/> is null, <paramref name="defaultValue"/> is written instead.</param>
    /// <param name="defaultValue">Value written when <paramref name="variable"/> is null. Ignored in read mode.</param>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    [Obsolete("Prefer using TimeInt32()")]
    public void Int32_ms(ref TimeInt32? variable, TimeInt32 defaultValue = default) => variable = Int32_ms(variable, defaultValue);

    /// <summary>
    /// Reads or writes an <see cref="int"/> as a <see cref="TmEssentials.TimeInt32"/> of seconds. If the read value is -1, null is returned. If the written <paramref name="variable"/> is null, -1 value is written.
    /// </summary>
    /// <remarks>A regular <see cref="int"/> is read/written but converted to/from <see cref="TmEssentials.TimeInt32"/> using <see cref="TimeInt32.FromSeconds(float)"/> (except for -1).</remarks>
    /// <param name="variable">Variable to write. Ignored in read mode.</param>
    /// <returns>Value read in read mode. In write mode, <paramref name="variable"/> is returned. If the read value is -1, null is returned.</returns>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    public TimeInt32? Int32_sn(TimeInt32? variable = default)
    {
        if (Reader is not null) return Reader.ReadInt32_sn();
        if (Writer is not null) Writer.WriteInt32_sn(variable);
        return variable;
    }

    /// <summary>
    /// Reads or writes an <see cref="int"/> as a <see cref="TmEssentials.TimeInt32"/> of seconds through reference. If the read value is -1, <paramref name="variable"/> is set to null. If the written <paramref name="variable"/> is null, -1 value is written.
    /// </summary>
    /// <remarks>A regular <see cref="int"/> is read/written but converted to/from <see cref="TmEssentials.TimeInt32"/> using <see cref="TimeInt32.FromSeconds(float)"/> (except for -1).</remarks>
    /// <param name="variable">Variable to read or write. Read mode sets <paramref name="variable"/>, write mode uses <paramref name="variable"/> to write the value (keeping <paramref name="variable"/> unchanged).</param>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    public void Int32_sn(ref TimeInt32? variable) => variable = Int32_sn(variable);

    /// <summary>
    /// Reads or writes an <see cref="int"/> as a <see cref="TmEssentials.TimeInt32"/> of milliseconds. If the read value is -1, null is returned. If the written <paramref name="variable"/> is null, -1 value is written.
    /// </summary>
    /// <remarks>A regular <see cref="int"/> is read/written but converted to/from <see cref="TmEssentials.TimeInt32"/> using <see cref="TimeInt32.FromMilliseconds(float)"/> (except for -1).</remarks>
    /// <param name="variable">Variable to write. Ignored in read mode.</param>
    /// <returns>Value read in read mode. In write mode, <paramref name="variable"/> is returned. If the read value is -1, null is returned.</returns>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    [Obsolete("Prefer using TimeInt32Nullable()")]
    public TimeInt32? Int32_msn(TimeInt32? variable = default)
    {
        if (Reader is not null) return Reader.ReadInt32_msn();
        if (Writer is not null) Writer.WriteInt32_msn(variable);
        return variable;
    }

    /// <summary>
    /// Reads or writes an <see cref="int"/> as a <see cref="TmEssentials.TimeInt32"/> of milliseconds through reference. If the read value is -1, <paramref name="variable"/> is set to null. If the written <paramref name="variable"/> is null, -1 value is written.
    /// </summary>
    /// <remarks>A regular <see cref="int"/> is read/written but converted to/from <see cref="TmEssentials.TimeInt32"/> using <see cref="TimeInt32.FromMilliseconds(float)"/> (except for -1).</remarks>
    /// <param name="variable">Variable to read or write. Read mode sets <paramref name="variable"/>, write mode uses <paramref name="variable"/> to write the value (keeping <paramref name="variable"/> unchanged).</param>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    [Obsolete("Prefer using TimeInt32Nullable()")]
    public void Int32_msn(ref TimeInt32? variable) => variable = Int32_msn(variable);

    /// <summary>
    /// Reads or writes a <see cref="float"/> as a <see cref="TmEssentials.TimeSingle"/> of seconds.
    /// </summary>
    /// <remarks>A regular <see cref="float"/> is read/written but converted to/from <see cref="TmEssentials.TimeSingle"/> using <see cref="TimeSingle.FromSeconds(float)"/>.</remarks>
    /// <param name="variable">Variable to write. Ignored in read mode.</param>
    /// <returns>Value read in read mode. In write mode, <paramref name="variable"/> is returned.</returns>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    [Obsolete("Prefer using TimeSingle()")]
    public TimeSingle Single_s(TimeSingle variable = default)
    {
        if (Reader is not null) return Reader.ReadSingle_s();
        if (Writer is not null) Writer.WriteSingle_s(variable);
        return variable;
    }

    /// <summary>
    /// Reads or writes a nullable <see cref="float"/> as a nullable <see cref="TmEssentials.TimeSingle"/> of seconds.
    /// </summary>
    /// <remarks>A regular <see cref="float"/> is read/written but converted to/from <see cref="TmEssentials.TimeSingle"/> using <see cref="TimeSingle.FromSeconds(float)"/>.</remarks>
    /// <param name="variable">Variable to write. If null, <paramref name="defaultValue"/> is written. Ignored in read mode.</param>
    /// <param name="defaultValue">Value written when <paramref name="variable"/> is null. Ignored in read mode.</param>
    /// <returns>Value read in read mode. In write mode, <paramref name="variable"/> is returned (including null).</returns>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    [Obsolete("Prefer using TimeSingle()")]
    public TimeSingle? Single_s(TimeSingle? variable, TimeSingle defaultValue = default)
    {
        if (Reader is not null) return Reader.ReadSingle_s();
        if (Writer is not null) Writer.WriteSingle_s(variable ?? defaultValue);
        return variable;
    }

    /// <summary>
    /// Reads or writes a <see cref="float"/> as a <see cref="TmEssentials.TimeSingle"/> of seconds through reference.
    /// </summary>
    /// <remarks>A regular <see cref="float"/> is read/written but converted to/from <see cref="TmEssentials.TimeSingle"/> using <see cref="TimeSingle.FromSeconds(float)"/>.</remarks>
    /// <param name="variable">Variable to read or write. Read mode sets <paramref name="variable"/>, write mode uses <paramref name="variable"/> to write the value (keeping <paramref name="variable"/> unchanged).</param>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    [Obsolete("Prefer using TimeSingle()")]
    public void Single_s(ref TimeSingle variable) => variable = Single_s(variable);

    /// <summary>
    /// Reads or writes a nullable <see cref="float"/> as a nullable <see cref="TmEssentials.TimeSingle"/> of seconds through reference.
    /// </summary>
    /// <remarks>A regular <see cref="float"/> is read/written but converted to/from <see cref="TmEssentials.TimeSingle"/> using <see cref="TimeSingle.FromSeconds(float)"/>.</remarks>
    /// <param name="variable">Variable to read or write. Read mode sets <paramref name="variable"/>, write mode uses <paramref name="variable"/> to write the value (keeping <paramref name="variable"/> unchanged). If <paramref name="variable"/> is null, <paramref name="defaultValue"/> is written instead.</param>
    /// <param name="defaultValue">Value written when <paramref name="variable"/> is null. Ignored in read mode.</param>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    [Obsolete("Prefer using TimeSingle()")]
    public void Single_s(ref TimeSingle? variable, TimeSingle defaultValue = default) => variable = Single_s(variable, defaultValue);

    /// <summary>
    /// Reads or writes an <see cref="float"/> as a <see cref="TmEssentials.TimeSingle"/> of milliseconds.
    /// </summary>
    /// <remarks>A regular <see cref="float"/> is read/written but converted to/from <see cref="TmEssentials.TimeSingle"/> using <see cref="TimeSingle.FromMilliseconds(float)"/>.</remarks>
    /// <param name="variable">Variable to write. Ignored in read mode.</param>
    /// <returns>Value read in read mode. In write mode, <paramref name="variable"/> is returned.</returns>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    public TimeSingle Single_ms(TimeSingle variable = default)
    {
        if (Reader is not null) return Reader.ReadSingle_ms();
        if (Writer is not null) Writer.WriteSingle_ms(variable);
        return variable;
    }

    /// <summary>
    /// Reads or writes a nullable <see cref="float"/> as a nullable <see cref="TmEssentials.TimeSingle"/> of milliseconds.
    /// </summary>
    /// <remarks>A regular <see cref="float"/> is read/written but converted to/from <see cref="TmEssentials.TimeSingle"/> using <see cref="TimeSingle.FromMilliseconds(float)"/>.</remarks>
    /// <param name="variable">Variable to write. If null, <paramref name="defaultValue"/> is written. Ignored in read mode.</param>
    /// <param name="defaultValue">Value written when <paramref name="variable"/> is null. Ignored in read mode.</param>
    /// <returns>Value read in read mode. In write mode, <paramref name="variable"/> is returned (including null).</returns>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    public TimeSingle? Single_ms(TimeSingle? variable, TimeSingle defaultValue = default)
    {
        if (Reader is not null) return Reader.ReadSingle_ms();
        if (Writer is not null) Writer.WriteSingle_ms(variable ?? defaultValue);
        return variable;
    }

    /// <summary>
    /// Reads or writes an <see cref="float"/> as a <see cref="TmEssentials.TimeSingle"/> of milliseconds through reference.
    /// </summary>
    /// <remarks>A regular <see cref="float"/> is read/written but converted to/from <see cref="TmEssentials.TimeSingle"/> using <see cref="TimeSingle.FromMilliseconds(float)"/>.</remarks>
    /// <param name="variable">Variable to read or write. Read mode sets <paramref name="variable"/>, write mode uses <paramref name="variable"/> to write the value (keeping <paramref name="variable"/> unchanged).</param>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    public void Single_ms(ref TimeSingle variable) => variable = Single_ms(variable);

    /// <summary>
    /// Reads or writes a nullable <see cref="float"/> as a nullable <see cref="TmEssentials.TimeSingle"/> of milliseconds through reference.
    /// </summary>
    /// <remarks>A regular <see cref="float"/> is read/written but converted to/from <see cref="TmEssentials.TimeSingle"/> using <see cref="TimeSingle.FromMilliseconds(float)"/>.</remarks>
    /// <param name="variable">Variable to read or write. Read mode sets <paramref name="variable"/>, write mode uses <paramref name="variable"/> to write the value (keeping <paramref name="variable"/> unchanged). If <paramref name="variable"/> is null, <paramref name="defaultValue"/> is written instead.</param>
    /// <param name="defaultValue">Value written when <paramref name="variable"/> is null. Ignored in read mode.</param>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    public void Single_ms(ref TimeSingle? variable, TimeSingle defaultValue = default) => variable = Single_ms(variable, defaultValue);

    /// <summary>
    /// Reads or writes an <see cref="float"/> as a <see cref="TmEssentials.TimeSingle"/> of seconds. If the read value is -1, null is returned. If the written <paramref name="variable"/> is null, -1 value is written.
    /// </summary>
    /// <remarks>A regular <see cref="float"/> is read/written but converted to/from <see cref="TmEssentials.TimeSingle"/> using <see cref="TimeSingle.FromSeconds(float)"/> (except for -1).</remarks>
    /// <param name="variable">Variable to write. Ignored in read mode.</param>
    /// <returns>Value read in read mode. In write mode, <paramref name="variable"/> is returned. If the read value is -1, null is returned.</returns>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    [Obsolete("Prefer using TimeSingleNullable()")]
    public TimeSingle? Single_sn(TimeSingle? variable = default)
    {
        if (Reader is not null) return Reader.ReadSingle_sn();
        if (Writer is not null) Writer.WriteSingle_sn(variable);
        return variable;
    }

    /// <summary>
    /// Reads or writes an <see cref="float"/> as a <see cref="TmEssentials.TimeSingle"/> of seconds through reference. If the read value is -1, <paramref name="variable"/> is set to null. If the written <paramref name="variable"/> is null, -1 value is written.
    /// </summary>
    /// <remarks>A regular <see cref="float"/> is read/written but converted to/from <see cref="TmEssentials.TimeSingle"/> using <see cref="TimeSingle.FromSeconds(float)"/> (except for -1).</remarks>
    /// <param name="variable">Variable to read or write. Read mode sets <paramref name="variable"/>, write mode uses <paramref name="variable"/> to write the value (keeping <paramref name="variable"/> unchanged).</param>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    [Obsolete("Prefer using TimeSingleNullable()")]
    public void Single_sn(ref TimeSingle? variable) => variable = Single_sn(variable);

    /// <summary>
    /// Reads or writes an <see cref="float"/> as a <see cref="TmEssentials.TimeSingle"/> of milliseconds. If the read value is -1, null is returned. If the written <paramref name="variable"/> is null, -1 value is written.
    /// </summary>
    /// <remarks>A regular <see cref="float"/> is read/written but converted to/from <see cref="TmEssentials.TimeSingle"/> using <see cref="TimeSingle.FromMilliseconds(float)"/> (except for -1).</remarks>
    /// <param name="variable">Variable to write. Ignored in read mode.</param>
    /// <returns>Value read in read mode. In write mode, <paramref name="variable"/> is returned. If the read value is -1, null is returned.</returns>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    public TimeSingle? Single_msn(TimeSingle? variable = default)
    {
        if (Reader is not null) return Reader.ReadSingle_msn();
        if (Writer is not null) Writer.WriteSingle_msn(variable);
        return variable;
    }

    /// <summary>
    /// Reads or writes an <see cref="float"/> as a <see cref="TmEssentials.TimeSingle"/> of milliseconds through reference. If the read value is -1, <paramref name="variable"/> is set to null. If the written <paramref name="variable"/> is null, -1 value is written.
    /// </summary>
    /// <remarks>A regular <see cref="float"/> is read/written but converted to/from <see cref="TmEssentials.TimeSingle"/> using <see cref="TimeSingle.FromMilliseconds(float)"/> (except for -1).</remarks>
    /// <param name="variable">Variable to read or write. Read mode sets <paramref name="variable"/>, write mode uses <paramref name="variable"/> to write the value (keeping <paramref name="variable"/> unchanged).</param>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    public void Single_msn(ref TimeSingle? variable) => variable = Single_msn(variable);
}
