using System.Numerics;

namespace GBX.NET;

/// <summary>
/// Provides single-method reading and writing by wrapping <see cref="GameBoxReader"/> and <see cref="GameBoxWriter"/> depending on the mode.
/// </summary>
public partial class GameBoxReaderWriter
{
    /// <summary>
    /// Reader component of the reader-writer. This will be null if <see cref="Mode"/> is <see cref="GameBoxReaderWriterMode.Write"/>.
    /// </summary>
    public GameBoxReader? Reader { get; }

    /// <summary>
    /// Writer component of the reader-writer. This will be null if <see cref="Mode"/> is <see cref="GameBoxReaderWriterMode.Read"/>.
    /// </summary>
    public GameBoxWriter? Writer { get; }

    public Stream BaseStream => Reader?.BaseStream ?? Writer?.BaseStream ?? throw new ThisShouldNotHappenException();

    /// <summary>
    /// Mode of the reader-writer.
    /// </summary>
    public GameBoxReaderWriterMode Mode
    {
        get
        {
            if (Reader is not null)
                return GameBoxReaderWriterMode.Read;
            if (Writer is not null)
                return GameBoxReaderWriterMode.Write;
            throw new ThisShouldNotHappenException();
        }
    }

    /// <summary>
    /// Constructs a reader-writer in reader mode.
    /// </summary>
    /// <param name="reader">Reader to use.</param>
    public GameBoxReaderWriter(GameBoxReader reader) => Reader = reader;

    /// <summary>
    /// Constructs a reader-writer in writer mode.
    /// </summary>
    /// <param name="writer">Writer to use.</param>
    public GameBoxReaderWriter(GameBoxWriter writer) => Writer = writer;

    /// <summary>
    /// Reads or writes a <see cref="bool"/>.
    /// </summary>
    /// <param name="variable">Variable to write. Ignored in read mode.</param>
    /// <param name="asByte">If the <see cref="bool"/> is going to be read/written as 1 byte instead of 4.</param>
    /// <returns>Value read in read mode. In write mode, <paramref name="variable"/> is returned.</returns>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    public bool Boolean(bool variable = default, bool asByte = false)
    {
        if (Reader is not null) return Reader.ReadBoolean(asByte);
        if (Writer is not null) Writer.Write(variable, asByte);
        return variable;
    }

    /// <summary>
    /// Reads or writes a nullable <see cref="bool"/>.
    /// </summary>
    /// <param name="variable">Variable to write. If null, <paramref name="defaultValue"/> is written. Ignored in read mode.</param>
    /// <param name="defaultValue">Value written when <paramref name="variable"/> is null. Ignored in read mode.</param>
    /// <param name="asByte">If the <see cref="bool"/> is going to be read/written as 1 byte instead of 4.</param>
    /// <returns>Value read in read mode. In write mode, <paramref name="variable"/> is returned (including null).</returns>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    public bool? Boolean(bool? variable, bool defaultValue = default, bool asByte = false)
    {
        if (Reader is not null) return Reader.ReadBoolean(asByte);
        if (Writer is not null) Writer.Write(variable.GetValueOrDefault(defaultValue), asByte);
        return variable;
    }

    /// <summary>
    /// Reads or writes a <see cref="bool"/> through reference.
    /// </summary>
    /// <param name="variable">Variable to read or write. Read mode sets <paramref name="variable"/>, write mode uses <paramref name="variable"/> to write the value (keeping <paramref name="variable"/> unchanged).</param>
    /// <param name="asByte">If the <see cref="bool"/> is going to be read/written as 1 byte instead of 4.</param>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    public void Boolean(ref bool variable, bool asByte = false)
    {
        variable = Boolean(variable, asByte);
    }

    /// <summary>
    /// Reads or writes a nullable <see cref="bool"/> through reference.
    /// </summary>
    /// <param name="variable">Variable to read or write. Read mode sets <paramref name="variable"/>, write mode uses <paramref name="variable"/> to write the value (keeping <paramref name="variable"/> unchanged). If <paramref name="variable"/> is null, <paramref name="defaultValue"/> is written instead.</param>
    /// <param name="defaultValue">Value written when <paramref name="variable"/> is null. Ignored in read mode.</param>
    /// <param name="asByte">If the <see cref="bool"/> is going to be read/written as 1 byte instead of 4.</param>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    public void Boolean(ref bool? variable, bool defaultValue = default, bool asByte = false)
    {
        variable = Boolean(variable, defaultValue, asByte);
    }

    /// <summary>
    /// Reads or writes a <see cref="byte"/>.
    /// </summary>
    /// <param name="variable">Variable to write. Ignored in read mode.</param>
    /// <returns>Value read in read mode. In write mode, <paramref name="variable"/> is returned.</returns>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    public byte Byte(byte variable = default)
    {
        if (Reader is not null) return Reader.ReadByte();
        if (Writer is not null) Writer.Write(variable);
        return variable;
    }

    /// <summary>
    /// Reads or writes a nullable <see cref="byte"/>.
    /// </summary>
    /// <param name="variable">Variable to write. If null, <paramref name="defaultValue"/> is written. Ignored in read mode.</param>
    /// <param name="defaultValue">Value written when <paramref name="variable"/> is null. Ignored in read mode.</param>
    /// <returns>Value read in read mode. In write mode, <paramref name="variable"/> is returned (including null).</returns>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    public byte? Byte(byte? variable, byte defaultValue = default)
    {
        if (Reader is not null) return Reader.ReadByte();
        if (Writer is not null) Writer.Write(variable.GetValueOrDefault(defaultValue));
        return variable;
    }

    /// <summary>
    /// Reads or writes a <see cref="byte"/> through reference.
    /// </summary>
    /// <param name="variable">Variable to read or write. Read mode sets <paramref name="variable"/>, write mode uses <paramref name="variable"/> to write the value (keeping <paramref name="variable"/> unchanged).</param>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    public void Byte(ref byte variable)
    {
        variable = Byte(variable);
    }

    /// <summary>
    /// Reads or writes a nullable <see cref="byte"/> through reference.
    /// </summary>
    /// <param name="variable">Variable to read or write. Read mode sets <paramref name="variable"/>, write mode uses <paramref name="variable"/> to write the value (keeping <paramref name="variable"/> unchanged). If <paramref name="variable"/> is null, <paramref name="defaultValue"/> is written instead.</param>
    /// <param name="defaultValue">Value written when <paramref name="variable"/> is null. Ignored in read mode.</param>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    public void Byte(ref byte? variable, byte defaultValue = default)
    {
        variable = Byte(variable, defaultValue);
    }

    /// <summary>
    /// Reads or writes a <see cref="byte"/> from casted <see cref="int"/>.
    /// </summary>
    /// <param name="variable">Variable to write. Ignored in read mode.</param>
    /// <returns>Value read in read mode. In write mode, <paramref name="variable"/> is returned.</returns>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    public int Byte(int variable)
    {
        return Byte((byte)variable);
    }

    /// <summary>
    /// Reads or writes a nullable <see cref="byte"/> from casted nullable <see cref="int"/>.
    /// </summary>
    /// <param name="variable">Variable to write. If null, <paramref name="defaultValue"/> is written. Ignored in read mode.</param>
    /// <param name="defaultValue">Value written when <paramref name="variable"/> is null. Ignored in read mode.</param>
    /// <returns>Value read in read mode. In write mode, <paramref name="variable"/> is returned (including null).</returns>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    public int? Byte(int? variable, int defaultValue = default)
    {
        return Byte(variable.HasValue ? (byte)variable.Value : null, (byte)defaultValue);
    }

    /// <summary>
    /// Reads or writes a <see cref="byte"/> from casted <see cref="int"/> through reference.
    /// </summary>
    /// <param name="variable">Variable to read or write. Read mode sets <paramref name="variable"/>, write mode uses <paramref name="variable"/> to write the value (keeping <paramref name="variable"/> unchanged).</param>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    public void Byte(ref int variable)
    {
        variable = Byte(variable);
    }

    /// <summary>
    /// Reads or writes a nullable <see cref="byte"/> from casted nullable <see cref="int"/> through reference.
    /// </summary>
    /// <param name="variable">Variable to read or write. Read mode sets <paramref name="variable"/>, write mode uses <paramref name="variable"/> to write the value (keeping <paramref name="variable"/> unchanged). If <paramref name="variable"/> is null, <paramref name="defaultValue"/> is written instead.</param>
    /// <param name="defaultValue">Value written when <paramref name="variable"/> is null. Ignored in read mode.</param>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    public void Byte(ref int? variable, int defaultValue = default)
    {
        variable = Byte(variable, defaultValue);
    }

    /// <summary>
    /// Reads or writes a <see cref="short"/>.
    /// </summary>
    /// <param name="variable">Variable to write. Ignored in read mode.</param>
    /// <returns>Value read in read mode. In write mode, <paramref name="variable"/> is returned.</returns>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    public short Int16(short variable = default)
    {
        if (Reader is not null) return Reader.ReadInt16();
        if (Writer is not null) Writer.Write(variable);
        return variable;
    }

    /// <summary>
    /// Reads or writes a nullable <see cref="short"/>.
    /// </summary>
    /// <param name="variable">Variable to write. If null, <paramref name="defaultValue"/> is written. Ignored in read mode.</param>
    /// <param name="defaultValue">Value written when <paramref name="variable"/> is null. Ignored in read mode.</param>
    /// <returns>Value read in read mode. In write mode, <paramref name="variable"/> is returned (including null).</returns>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    public short? Int16(short? variable, short defaultValue = default)
    {
        if (Reader is not null) return Reader.ReadInt16();
        if (Writer is not null) Writer.Write(variable.GetValueOrDefault(defaultValue));
        return variable;
    }

    /// <summary>
    /// Reads or writes a <see cref="short"/> through reference.
    /// </summary>
    /// <param name="variable">Variable to read or write. Read mode sets <paramref name="variable"/>, write mode uses <paramref name="variable"/> to write the value (keeping <paramref name="variable"/> unchanged).</param>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    public void Int16(ref short variable) => variable = Int16(variable);

    /// <summary>
    /// Reads or writes a nullable <see cref="short"/> through reference.
    /// </summary>
    /// <param name="variable">Variable to read or write. Read mode sets <paramref name="variable"/>, write mode uses <paramref name="variable"/> to write the value (keeping <paramref name="variable"/> unchanged). If <paramref name="variable"/> is null, <paramref name="defaultValue"/> is written instead.</param>
    /// <param name="defaultValue">Value written when <paramref name="variable"/> is null. Ignored in read mode.</param>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    public void Int16(ref short? variable, short defaultValue = default) => variable = Int16(variable, defaultValue);

    /// <summary>
    /// Reads or writes an <see cref="int"/>.
    /// </summary>
    /// <param name="variable">Variable to write. Ignored in read mode.</param>
    /// <returns>Value read in read mode. In write mode, <paramref name="variable"/> is returned.</returns>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    public int Int32(int variable = default)
    {
        if (Reader is not null) return Reader.ReadInt32();
        if (Writer is not null) Writer.Write(variable);
        return variable;
    }

    /// <summary>
    /// Reads or writes a nullable <see cref="int"/>.
    /// </summary>
    /// <param name="variable">Variable to write. If null, <paramref name="defaultValue"/> is written. Ignored in read mode.</param>
    /// <param name="defaultValue">Value written when <paramref name="variable"/> is null. Ignored in read mode.</param>
    /// <returns>Value read in read mode. In write mode, <paramref name="variable"/> is returned (including null).</returns>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    public int? Int32(int? variable, int defaultValue = default)
    {
        if (Reader is not null) return Reader.ReadInt32();
        if (Writer is not null) Writer.Write(variable.GetValueOrDefault(defaultValue));
        return variable;
    }

    /// <summary>
    /// Reads or writes an <see cref="int"/> through reference.
    /// </summary>
    /// <param name="variable">Variable to read or write. Read mode sets <paramref name="variable"/>, write mode uses <paramref name="variable"/> to write the value (keeping <paramref name="variable"/> unchanged).</param>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    public void Int32(ref int variable) => variable = Int32(variable);

    /// <summary>
    /// Reads or writes a nullable <see cref="int"/> through reference.
    /// </summary>
    /// <param name="variable">Variable to read or write. Read mode sets <paramref name="variable"/>, write mode uses <paramref name="variable"/> to write the value (keeping <paramref name="variable"/> unchanged). If <paramref name="variable"/> is null, <paramref name="defaultValue"/> is written instead.</param>
    /// <param name="defaultValue">Value written when <paramref name="variable"/> is null. Ignored in read mode.</param>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    public void Int32(ref int? variable, int defaultValue = default) => variable = Int32(variable, defaultValue);

    /// <summary>
    /// Reads or writes an <see cref="int"/> as a <see cref="TimeSpan"/> of seconds.
    /// </summary>
    /// <remarks>A regular <see cref="int"/> is read/written but converted to/from <see cref="TimeSpan"/> using <see cref="TimeSpan.FromSeconds(double)"/>.</remarks>
    /// <param name="variable">Variable to write. Ignored in read mode.</param>
    /// <returns>Value read in read mode. In write mode, <paramref name="variable"/> is returned.</returns>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    public TimeSpan Int32_s(TimeSpan variable = default)
    {
        if (Reader is not null) return Reader.ReadInt32_s();
        if (Writer is not null) Writer.WriteInt32_s(variable);
        return variable;
    }

    /// <summary>
    /// Reads or writes a nullable <see cref="int"/> as a nullable <see cref="TimeSpan"/> of seconds.
    /// </summary>
    /// <remarks>A regular <see cref="int"/> is read/written but converted to/from <see cref="TimeSpan"/> using <see cref="TimeSpan.FromSeconds(double)"/>.</remarks>
    /// <param name="variable">Variable to write. If null, <paramref name="defaultValue"/> is written. Ignored in read mode.</param>
    /// <param name="defaultValue">Value written when <paramref name="variable"/> is null. Ignored in read mode.</param>
    /// <returns>Value read in read mode. In write mode, <paramref name="variable"/> is returned (including null).</returns>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    public TimeSpan? Int32_s(TimeSpan? variable, TimeSpan defaultValue = default)
    {
        if (Reader is not null) return Reader.ReadInt32_s();
        if (Writer is not null) Writer.WriteInt32_s(variable.GetValueOrDefault(defaultValue));
        return variable;
    }

    /// <summary>
    /// Reads or writes an <see cref="int"/> as a <see cref="TimeSpan"/> of seconds through reference.
    /// </summary>
    /// <remarks>A regular <see cref="int"/> is read/written but converted to/from <see cref="TimeSpan"/> using <see cref="TimeSpan.FromSeconds(double)"/>.</remarks>
    /// <param name="variable">Variable to read or write. Read mode sets <paramref name="variable"/>, write mode uses <paramref name="variable"/> to write the value (keeping <paramref name="variable"/> unchanged).</param>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    public void Int32_s(ref TimeSpan variable) => variable = Int32_s(variable);

    /// <summary>
    /// Reads or writes a nullable <see cref="int"/> as a nullable <see cref="TimeSpan"/> of seconds through reference.
    /// </summary>
    /// <remarks>A regular <see cref="int"/> is read/written but converted to/from <see cref="TimeSpan"/> using <see cref="TimeSpan.FromSeconds(double)"/>.</remarks>
    /// <param name="variable">Variable to read or write. Read mode sets <paramref name="variable"/>, write mode uses <paramref name="variable"/> to write the value (keeping <paramref name="variable"/> unchanged). If <paramref name="variable"/> is null, <paramref name="defaultValue"/> is written instead.</param>
    /// <param name="defaultValue">Value written when <paramref name="variable"/> is null. Ignored in read mode.</param>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    public void Int32_s(ref TimeSpan? variable, TimeSpan defaultValue = default) => variable = Int32_s(variable, defaultValue);

    /// <summary>
    /// Reads or writes an <see cref="int"/> as a <see cref="TimeSpan"/> of milliseconds.
    /// </summary>
    /// <remarks>A regular <see cref="int"/> is read/written but converted to/from <see cref="TimeSpan"/> using <see cref="TimeSpan.FromMilliseconds(double)"/>.</remarks>
    /// <param name="variable">Variable to write. Ignored in read mode.</param>
    /// <returns>Value read in read mode. In write mode, <paramref name="variable"/> is returned.</returns>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    public TimeSpan Int32_ms(TimeSpan variable = default)
    {
        if (Reader is not null) return Reader.ReadInt32_ms();
        if (Writer is not null) Writer.WriteInt32_ms(variable);
        return variable;
    }

    /// <summary>
    /// Reads or writes a nullable <see cref="int"/> as a nullable <see cref="TimeSpan"/> of milliseconds.
    /// </summary>
    /// <remarks>A regular <see cref="int"/> is read/written but converted to/from <see cref="TimeSpan"/> using <see cref="TimeSpan.FromMilliseconds(double)"/>.</remarks>
    /// <param name="variable">Variable to write. If null, <paramref name="defaultValue"/> is written. Ignored in read mode.</param>
    /// <param name="defaultValue">Value written when <paramref name="variable"/> is null. Ignored in read mode.</param>
    /// <returns>Value read in read mode. In write mode, <paramref name="variable"/> is returned (including null).</returns>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    public TimeSpan? Int32_ms(TimeSpan? variable, TimeSpan defaultValue = default)
    {
        if (Reader is not null) return Reader.ReadInt32_ms();
        if (Writer is not null) Writer.WriteInt32_ms(variable.GetValueOrDefault(defaultValue));
        return variable;
    }

    /// <summary>
    /// Reads or writes an <see cref="int"/> as a <see cref="TimeSpan"/> of milliseconds through reference.
    /// </summary>
    /// <remarks>A regular <see cref="int"/> is read/written but converted to/from <see cref="TimeSpan"/> using <see cref="TimeSpan.FromMilliseconds(double)"/>.</remarks>
    /// <param name="variable">Variable to read or write. Read mode sets <paramref name="variable"/>, write mode uses <paramref name="variable"/> to write the value (keeping <paramref name="variable"/> unchanged).</param>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    public void Int32_ms(ref TimeSpan variable) => variable = Int32_ms(variable);

    /// <summary>
    /// Reads or writes a nullable <see cref="int"/> as a nullable <see cref="TimeSpan"/> of milliseconds through reference.
    /// </summary>
    /// <remarks>A regular <see cref="int"/> is read/written but converted to/from <see cref="TimeSpan"/> using <see cref="TimeSpan.FromMilliseconds(double)"/>.</remarks>
    /// <param name="variable">Variable to read or write. Read mode sets <paramref name="variable"/>, write mode uses <paramref name="variable"/> to write the value (keeping <paramref name="variable"/> unchanged). If <paramref name="variable"/> is null, <paramref name="defaultValue"/> is written instead.</param>
    /// <param name="defaultValue">Value written when <paramref name="variable"/> is null. Ignored in read mode.</param>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    public void Int32_ms(ref TimeSpan? variable, TimeSpan defaultValue = default) => variable = Int32_ms(variable, defaultValue);

    /// <summary>
    /// Reads or writes an <see cref="int"/> as a <see cref="TimeSpan"/> of seconds. If the read value is -1, null is returned. If the written <paramref name="variable"/> is null, -1 value is written.
    /// </summary>
    /// <remarks>A regular <see cref="int"/> is read/written but converted to/from <see cref="TimeSpan"/> using <see cref="TimeSpan.FromSeconds(double)"/> (except for -1).</remarks>
    /// <param name="variable">Variable to write. Ignored in read mode.</param>
    /// <returns>Value read in read mode. In write mode, <paramref name="variable"/> is returned. If the read value is -1, null is returned.</returns>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    public TimeSpan? Int32_sn(TimeSpan? variable = default)
    {
        if (Reader is not null) return Reader.ReadInt32_sn();
        if (Writer is not null) Writer.WriteInt32_sn(variable);
        return variable;
    }

    /// <summary>
    /// Reads or writes an <see cref="int"/> as a <see cref="TimeSpan"/> of seconds through reference. If the read value is -1, <paramref name="variable"/> is set to null. If the written <paramref name="variable"/> is null, -1 value is written.
    /// </summary>
    /// <remarks>A regular <see cref="int"/> is read/written but converted to/from <see cref="TimeSpan"/> using <see cref="TimeSpan.FromSeconds(double)"/> (except for -1).</remarks>
    /// <param name="variable">Variable to read or write. Read mode sets <paramref name="variable"/>, write mode uses <paramref name="variable"/> to write the value (keeping <paramref name="variable"/> unchanged).</param>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    public void Int32_sn(ref TimeSpan? variable) => variable = Int32_sn(variable);

    /// <summary>
    /// Reads or writes an <see cref="int"/> as a <see cref="TimeSpan"/> of milliseconds. If the read value is -1, null is returned. If the written <paramref name="variable"/> is null, -1 value is written.
    /// </summary>
    /// <remarks>A regular <see cref="int"/> is read/written but converted to/from <see cref="TimeSpan"/> using <see cref="TimeSpan.FromMilliseconds(double)"/> (except for -1).</remarks>
    /// <param name="variable">Variable to write. Ignored in read mode.</param>
    /// <returns>Value read in read mode. In write mode, <paramref name="variable"/> is returned. If the read value is -1, null is returned.</returns>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    public TimeSpan? Int32_msn(TimeSpan? variable = default)
    {
        if (Reader is not null) return Reader.ReadInt32_msn();
        if (Writer is not null) Writer.WriteInt32_msn(variable);
        return variable;
    }

    /// <summary>
    /// Reads or writes an <see cref="int"/> as a <see cref="TimeSpan"/> of milliseconds through reference. If the read value is -1, <paramref name="variable"/> is set to null. If the written <paramref name="variable"/> is null, -1 value is written.
    /// </summary>
    /// <remarks>A regular <see cref="int"/> is read/written but converted to/from <see cref="TimeSpan"/> using <see cref="TimeSpan.FromMilliseconds(double)"/> (except for -1).</remarks>
    /// <param name="variable">Variable to read or write. Read mode sets <paramref name="variable"/>, write mode uses <paramref name="variable"/> to write the value (keeping <paramref name="variable"/> unchanged).</param>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    public void Int32_msn(ref TimeSpan? variable) => variable = Int32_msn(variable);

    /// <summary>
    /// Reads or writes a <see cref="long"/>.
    /// </summary>
    /// <param name="variable">Variable to write. Ignored in read mode.</param>
    /// <returns>Value read in read mode. In write mode, <paramref name="variable"/> is returned.</returns>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    public long Int64(long variable = default)
    {
        if (Reader is not null) return Reader.ReadInt64();
        if (Writer is not null) Writer.Write(variable);
        return variable;
    }

    /// <summary>
    /// Reads or writes a nullable <see cref="long"/>.
    /// </summary>
    /// <param name="variable">Variable to write. If null, <paramref name="defaultValue"/> is written. Ignored in read mode.</param>
    /// <param name="defaultValue">Value written when <paramref name="variable"/> is null. Ignored in read mode.</param>
    /// <returns>Value read in read mode. In write mode, <paramref name="variable"/> is returned (including null).</returns>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    public long? Int64(long? variable, long defaultValue = default)
    {
        if (Reader is not null) return Reader.ReadInt64();
        if (Writer is not null) Writer.Write(variable.GetValueOrDefault(defaultValue));
        return variable;
    }

    /// <summary>
    /// Reads or writes an <see cref="long"/> through reference.
    /// </summary>
    /// <param name="variable">Variable to read or write. Read mode sets <paramref name="variable"/>, write mode uses <paramref name="variable"/> to write the value (keeping <paramref name="variable"/> unchanged).</param>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    public void Int64(ref long variable) => variable = Int64(variable);

    /// <summary>
    /// Reads or writes a nullable <see cref="int"/> through reference.
    /// </summary>
    /// <param name="variable">Variable to read or write. Read mode sets <paramref name="variable"/>, write mode uses <paramref name="variable"/> to write the value (keeping <paramref name="variable"/> unchanged). If <paramref name="variable"/> is null, <paramref name="defaultValue"/> is written instead.</param>
    /// <param name="defaultValue">Value written when <paramref name="variable"/> is null. Ignored in read mode.</param>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    public void Int64(ref long? variable, long defaultValue = default) => variable = Int64(variable, defaultValue);

    /// <summary>
    /// Reads or writes a <see cref="ushort"/>.
    /// </summary>
    /// <param name="variable">Variable to write. Ignored in read mode.</param>
    /// <returns>Value read in read mode. In write mode, <paramref name="variable"/> is returned.</returns>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    public ushort UInt16(ushort variable = default)
    {
        if (Reader is not null) return Reader.ReadUInt16();
        if (Writer is not null) Writer.Write(variable);
        return variable;
    }

    /// <summary>
    /// Reads or writes a nullable <see cref="ushort"/>.
    /// </summary>
    /// <param name="variable">Variable to write. If null, <paramref name="defaultValue"/> is written. Ignored in read mode.</param>
    /// <param name="defaultValue">Value written when <paramref name="variable"/> is null. Ignored in read mode.</param>
    /// <returns>Value read in read mode. In write mode, <paramref name="variable"/> is returned (including null).</returns>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    public ushort? UInt16(ushort? variable, ushort defaultValue = default)
    {
        if (Reader is not null) return Reader.ReadUInt16();
        if (Writer is not null) Writer.Write(variable.GetValueOrDefault(defaultValue));
        return variable;
    }

    /// <summary>
    /// Reads or writes an <see cref="ushort"/> through reference.
    /// </summary>
    /// <param name="variable">Variable to read or write. Read mode sets <paramref name="variable"/>, write mode uses <paramref name="variable"/> to write the value (keeping <paramref name="variable"/> unchanged).</param>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    public void UInt16(ref ushort variable) => variable = UInt16(variable);

    /// <summary>
    /// Reads or writes a nullable <see cref="ushort"/> through reference.
    /// </summary>
    /// <param name="variable">Variable to read or write. Read mode sets <paramref name="variable"/>, write mode uses <paramref name="variable"/> to write the value (keeping <paramref name="variable"/> unchanged). If <paramref name="variable"/> is null, <paramref name="defaultValue"/> is written instead.</param>
    /// <param name="defaultValue">Value written when <paramref name="variable"/> is null. Ignored in read mode.</param>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    public void UInt16(ref ushort? variable, ushort defaultValue = default) => variable = UInt16(variable, defaultValue);

    /// <summary>
    /// Reads or writes a <see cref="uint"/>.
    /// </summary>
    /// <param name="variable">Variable to write. Ignored in read mode.</param>
    /// <returns>Value read in read mode. In write mode, <paramref name="variable"/> is returned.</returns>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    public uint UInt32(uint variable = default)
    {
        if (Reader is not null) return Reader.ReadUInt32();
        if (Writer is not null) Writer.Write(variable);
        return variable;
    }

    /// <summary>
    /// Reads or writes a nullable <see cref="uint"/>.
    /// </summary>
    /// <param name="variable">Variable to write. If null, <paramref name="defaultValue"/> is written. Ignored in read mode.</param>
    /// <param name="defaultValue">Value written when <paramref name="variable"/> is null. Ignored in read mode.</param>
    /// <returns>Value read in read mode. In write mode, <paramref name="variable"/> is returned (including null).</returns>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    public uint? UInt32(uint? variable, uint defaultValue = default)
    {
        if (Reader is not null) return Reader.ReadUInt32();
        if (Writer is not null) Writer.Write(variable.GetValueOrDefault(defaultValue));
        return variable;
    }

    /// <summary>
    /// Reads or writes an <see cref="uint"/> through reference.
    /// </summary>
    /// <param name="variable">Variable to read or write. Read mode sets <paramref name="variable"/>, write mode uses <paramref name="variable"/> to write the value (keeping <paramref name="variable"/> unchanged).</param>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    public void UInt32(ref uint variable) => variable = UInt32(variable);

    /// <summary>
    /// Reads or writes a nullable <see cref="uint"/> through reference.
    /// </summary>
    /// <param name="variable">Variable to read or write. Read mode sets <paramref name="variable"/>, write mode uses <paramref name="variable"/> to write the value (keeping <paramref name="variable"/> unchanged). If <paramref name="variable"/> is null, <paramref name="defaultValue"/> is written instead.</param>
    /// <param name="defaultValue">Value written when <paramref name="variable"/> is null. Ignored in read mode.</param>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    public void UInt32(ref uint? variable, uint defaultValue = default) => variable = UInt32(variable, defaultValue);

    /// <summary>
    /// Reads or writes a <see cref="ulong"/>.
    /// </summary>
    /// <param name="variable">Variable to write. Ignored in read mode.</param>
    /// <returns>Value read in read mode. In write mode, <paramref name="variable"/> is returned.</returns>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    public ulong UInt64(ulong variable = default)
    {
        if (Reader is not null) return Reader.ReadUInt64();
        if (Writer is not null) Writer.Write(variable);
        return variable;
    }

    /// <summary>
    /// Reads or writes a nullable <see cref="ulong"/>.
    /// </summary>
    /// <param name="variable">Variable to write. If null, <paramref name="defaultValue"/> is written. Ignored in read mode.</param>
    /// <param name="defaultValue">Value written when <paramref name="variable"/> is null. Ignored in read mode.</param>
    /// <returns>Value read in read mode. In write mode, <paramref name="variable"/> is returned (including null).</returns>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    public ulong? UInt64(ulong? variable, ulong defaultValue = default)
    {
        if (Reader is not null) return Reader.ReadUInt64();
        if (Writer is not null) Writer.Write(variable.GetValueOrDefault(defaultValue));
        return variable;
    }

    /// <summary>
    /// Reads or writes an <see cref="ulong"/> through reference.
    /// </summary>
    /// <param name="variable">Variable to read or write. Read mode sets <paramref name="variable"/>, write mode uses <paramref name="variable"/> to write the value (keeping <paramref name="variable"/> unchanged).</param>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    public void UInt64(ref ulong variable) => variable = UInt64(variable);

    /// <summary>
    /// Reads or writes a nullable <see cref="ulong"/> through reference.
    /// </summary>
    /// <param name="variable">Variable to read or write. Read mode sets <paramref name="variable"/>, write mode uses <paramref name="variable"/> to write the value (keeping <paramref name="variable"/> unchanged). If <paramref name="variable"/> is null, <paramref name="defaultValue"/> is written instead.</param>
    /// <param name="defaultValue">Value written when <paramref name="variable"/> is null. Ignored in read mode.</param>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    public void UInt64(ref ulong? variable, ulong defaultValue = default) => variable = UInt64(variable, defaultValue);

    /// <summary>
    /// Reads or writes a <see cref="float"/>.
    /// </summary>
    /// <param name="variable">Variable to write. Ignored in read mode.</param>
    /// <returns>Value read in read mode. In write mode, <paramref name="variable"/> is returned.</returns>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    public float Single(float variable = default)
    {
        if (Reader is not null) return Reader.ReadSingle();
        if (Writer is not null) Writer.Write(variable);
        return variable;
    }

    /// <summary>
    /// Reads or writes a nullable <see cref="float"/>.
    /// </summary>
    /// <param name="variable">Variable to write. If null, <paramref name="defaultValue"/> is written. Ignored in read mode.</param>
    /// <param name="defaultValue">Value written when <paramref name="variable"/> is null. Ignored in read mode.</param>
    /// <returns>Value read in read mode. In write mode, <paramref name="variable"/> is returned (including null).</returns>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    public float? Single(float? variable, float defaultValue = default)
    {
        if (Reader is not null) return Reader.ReadSingle();
        if (Writer is not null) Writer.Write(variable.GetValueOrDefault(defaultValue));
        return variable;
    }

    /// <summary>
    /// Reads or writes a <see cref="float"/> through reference.
    /// </summary>
    /// <param name="variable">Variable to read or write. Read mode sets <paramref name="variable"/>, write mode uses <paramref name="variable"/> to write the value (keeping <paramref name="variable"/> unchanged).</param>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    public void Single(ref float variable) => variable = Single(variable);

    /// <summary>
    /// Reads or writes a nullable <see cref="float"/> through reference.
    /// </summary>
    /// <param name="variable">Variable to read or write. Read mode sets <paramref name="variable"/>, write mode uses <paramref name="variable"/> to write the value (keeping <paramref name="variable"/> unchanged). If <paramref name="variable"/> is null, <paramref name="defaultValue"/> is written instead.</param>
    /// <param name="defaultValue">Value written when <paramref name="variable"/> is null. Ignored in read mode.</param>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    public void Single(ref float? variable, float defaultValue = default) => variable = Single(variable, defaultValue);

    /// <summary>
    /// Reads or writes a <see cref="float"/> as a <see cref="TimeSpan"/> of seconds.
    /// </summary>
    /// <remarks>A regular <see cref="float"/> is read/written but converted to/from <see cref="TimeSpan"/> using <see cref="TimeSpan.FromSeconds(double)"/>.</remarks>
    /// <param name="variable">Variable to write. Ignored in read mode.</param>
    /// <returns>Value read in read mode. In write mode, <paramref name="variable"/> is returned.</returns>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    public TimeSpan Single_s(TimeSpan variable = default)
    {
        if (Reader is not null) return Reader.ReadSingle_s();
        if (Writer is not null) Writer.WriteSingle_s(variable);
        return variable;
    }

    /// <summary>
    /// Reads or writes a nullable <see cref="float"/> as a nullable <see cref="TimeSpan"/> of seconds.
    /// </summary>
    /// <remarks>A regular <see cref="float"/> is read/written but converted to/from <see cref="TimeSpan"/> using <see cref="TimeSpan.FromSeconds(double)"/>.</remarks>
    /// <param name="variable">Variable to write. If null, <paramref name="defaultValue"/> is written. Ignored in read mode.</param>
    /// <param name="defaultValue">Value written when <paramref name="variable"/> is null. Ignored in read mode.</param>
    /// <returns>Value read in read mode. In write mode, <paramref name="variable"/> is returned (including null).</returns>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    public TimeSpan? Single_s(TimeSpan? variable, TimeSpan defaultValue = default)
    {
        if (Reader is not null) return Reader.ReadSingle_s();
        if (Writer is not null) Writer.WriteSingle_s(variable.GetValueOrDefault(defaultValue));
        return variable;
    }

    /// <summary>
    /// Reads or writes a <see cref="float"/> as a <see cref="TimeSpan"/> of seconds through reference.
    /// </summary>
    /// <remarks>A regular <see cref="float"/> is read/written but converted to/from <see cref="TimeSpan"/> using <see cref="TimeSpan.FromSeconds(double)"/>.</remarks>
    /// <param name="variable">Variable to read or write. Read mode sets <paramref name="variable"/>, write mode uses <paramref name="variable"/> to write the value (keeping <paramref name="variable"/> unchanged).</param>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    public void Single_s(ref TimeSpan variable) => variable = Single_s(variable);

    /// <summary>
    /// Reads or writes a nullable <see cref="float"/> as a nullable <see cref="TimeSpan"/> of seconds through reference.
    /// </summary>
    /// <remarks>A regular <see cref="float"/> is read/written but converted to/from <see cref="TimeSpan"/> using <see cref="TimeSpan.FromSeconds(double)"/>.</remarks>
    /// <param name="variable">Variable to read or write. Read mode sets <paramref name="variable"/>, write mode uses <paramref name="variable"/> to write the value (keeping <paramref name="variable"/> unchanged). If <paramref name="variable"/> is null, <paramref name="defaultValue"/> is written instead.</param>
    /// <param name="defaultValue">Value written when <paramref name="variable"/> is null. Ignored in read mode.</param>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    public void Single_s(ref TimeSpan? variable, TimeSpan defaultValue = default) => variable = Single_s(variable, defaultValue);

    /// <summary>
    /// Reads or writes an <see cref="float"/> as a <see cref="TimeSpan"/> of milliseconds.
    /// </summary>
    /// <remarks>A regular <see cref="float"/> is read/written but converted to/from <see cref="TimeSpan"/> using <see cref="TimeSpan.FromMilliseconds(double)"/>.</remarks>
    /// <param name="variable">Variable to write. Ignored in read mode.</param>
    /// <returns>Value read in read mode. In write mode, <paramref name="variable"/> is returned.</returns>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    public TimeSpan Single_ms(TimeSpan variable = default)
    {
        if (Reader is not null) return Reader.ReadSingle_ms();
        if (Writer is not null) Writer.WriteSingle_ms(variable);
        return variable;
    }

    /// <summary>
    /// Reads or writes a nullable <see cref="float"/> as a nullable <see cref="TimeSpan"/> of milliseconds.
    /// </summary>
    /// <remarks>A regular <see cref="float"/> is read/written but converted to/from <see cref="TimeSpan"/> using <see cref="TimeSpan.FromMilliseconds(double)"/>.</remarks>
    /// <param name="variable">Variable to write. If null, <paramref name="defaultValue"/> is written. Ignored in read mode.</param>
    /// <param name="defaultValue">Value written when <paramref name="variable"/> is null. Ignored in read mode.</param>
    /// <returns>Value read in read mode. In write mode, <paramref name="variable"/> is returned (including null).</returns>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    public TimeSpan? Single_ms(TimeSpan? variable, TimeSpan defaultValue = default)
    {
        if (Reader is not null) return Reader.ReadSingle_ms();
        if (Writer is not null) Writer.WriteSingle_ms(variable.GetValueOrDefault(defaultValue));
        return variable;
    }

    /// <summary>
    /// Reads or writes an <see cref="float"/> as a <see cref="TimeSpan"/> of milliseconds through reference.
    /// </summary>
    /// <remarks>A regular <see cref="float"/> is read/written but converted to/from <see cref="TimeSpan"/> using <see cref="TimeSpan.FromMilliseconds(double)"/>.</remarks>
    /// <param name="variable">Variable to read or write. Read mode sets <paramref name="variable"/>, write mode uses <paramref name="variable"/> to write the value (keeping <paramref name="variable"/> unchanged).</param>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    public void Single_ms(ref TimeSpan variable) => variable = Single_ms(variable);

    /// <summary>
    /// Reads or writes a nullable <see cref="float"/> as a nullable <see cref="TimeSpan"/> of milliseconds through reference.
    /// </summary>
    /// <remarks>A regular <see cref="float"/> is read/written but converted to/from <see cref="TimeSpan"/> using <see cref="TimeSpan.FromMilliseconds(double)"/>.</remarks>
    /// <param name="variable">Variable to read or write. Read mode sets <paramref name="variable"/>, write mode uses <paramref name="variable"/> to write the value (keeping <paramref name="variable"/> unchanged). If <paramref name="variable"/> is null, <paramref name="defaultValue"/> is written instead.</param>
    /// <param name="defaultValue">Value written when <paramref name="variable"/> is null. Ignored in read mode.</param>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    public void Single_ms(ref TimeSpan? variable, TimeSpan defaultValue = default) => variable = Single_ms(variable, defaultValue);

    /// <summary>
    /// Reads or writes an <see cref="float"/> as a <see cref="TimeSpan"/> of seconds. If the read value is -1, null is returned. If the written <paramref name="variable"/> is null, -1 value is written.
    /// </summary>
    /// <remarks>A regular <see cref="float"/> is read/written but converted to/from <see cref="TimeSpan"/> using <see cref="TimeSpan.FromSeconds(double)"/> (except for -1).</remarks>
    /// <param name="variable">Variable to write. Ignored in read mode.</param>
    /// <returns>Value read in read mode. In write mode, <paramref name="variable"/> is returned. If the read value is -1, null is returned.</returns>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    public TimeSpan? Single_sn(TimeSpan? variable = default)
    {
        if (Reader is not null) return Reader.ReadSingle_sn();
        if (Writer is not null) Writer.WriteSingle_sn(variable);
        return variable;
    }

    /// <summary>
    /// Reads or writes an <see cref="float"/> as a <see cref="TimeSpan"/> of seconds through reference. If the read value is -1, <paramref name="variable"/> is set to null. If the written <paramref name="variable"/> is null, -1 value is written.
    /// </summary>
    /// <remarks>A regular <see cref="float"/> is read/written but converted to/from <see cref="TimeSpan"/> using <see cref="TimeSpan.FromSeconds(double)"/> (except for -1).</remarks>
    /// <param name="variable">Variable to read or write. Read mode sets <paramref name="variable"/>, write mode uses <paramref name="variable"/> to write the value (keeping <paramref name="variable"/> unchanged).</param>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    public void Single_sn(ref TimeSpan? variable) => variable = Single_sn(variable);

    /// <summary>
    /// Reads or writes an <see cref="float"/> as a <see cref="TimeSpan"/> of milliseconds. If the read value is -1, null is returned. If the written <paramref name="variable"/> is null, -1 value is written.
    /// </summary>
    /// <remarks>A regular <see cref="float"/> is read/written but converted to/from <see cref="TimeSpan"/> using <see cref="TimeSpan.FromMilliseconds(double)"/> (except for -1).</remarks>
    /// <param name="variable">Variable to write. Ignored in read mode.</param>
    /// <returns>Value read in read mode. In write mode, <paramref name="variable"/> is returned. If the read value is -1, null is returned.</returns>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    public TimeSpan? Single_msn(TimeSpan? variable = default)
    {
        if (Reader is not null) return Reader.ReadSingle_msn();
        if (Writer is not null) Writer.WriteSingle_msn(variable);
        return variable;
    }

    /// <summary>
    /// Reads or writes an <see cref="float"/> as a <see cref="TimeSpan"/> of milliseconds through reference. If the read value is -1, <paramref name="variable"/> is set to null. If the written <paramref name="variable"/> is null, -1 value is written.
    /// </summary>
    /// <remarks>A regular <see cref="float"/> is read/written but converted to/from <see cref="TimeSpan"/> using <see cref="TimeSpan.FromMilliseconds(double)"/> (except for -1).</remarks>
    /// <param name="variable">Variable to read or write. Read mode sets <paramref name="variable"/>, write mode uses <paramref name="variable"/> to write the value (keeping <paramref name="variable"/> unchanged).</param>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    public void Single_msn(ref TimeSpan? variable) => variable = Single_msn(variable);

    /// <summary>
    /// Reads or writes a <paramref name="byteLength"/> amount of bytes as a <see cref="BigInteger"/>.
    /// </summary>
    /// <param name="variable">Variable to write. Ignored in read mode.</param>
    /// <param name="byteLength">Amount of bytes to use for reading/writing.</param>
    /// <returns>Value read in read mode. In write mode, <paramref name="variable"/> is returned.</returns>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    public BigInteger BigInt(BigInteger variable, int byteLength)
    {
        if (Reader is not null) return Reader.ReadBigInt(byteLength);
        if (Writer is not null) Writer.WriteBigInt(variable);
        return variable;
    }

    /// <summary>
    /// Reads or writes a <paramref name="byteLength"/> amount of bytes as a nullable <see cref="BigInteger"/>.
    /// </summary>
    /// <param name="variable">Variable to write. If null, <paramref name="defaultValue"/> is written. Ignored in read mode.</param>
    /// <param name="byteLength">Amount of bytes to use for reading/writing.</param>
    /// <param name="defaultValue">Value written when <paramref name="variable"/> is null. Ignored in read mode.</param>
    /// <returns>Value read in read mode. In write mode, <paramref name="variable"/> is returned (including null).</returns>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    public BigInteger? BigInt(BigInteger? variable, int byteLength, BigInteger defaultValue = default)
    {
        if (Reader is not null) return Reader.ReadBigInt(byteLength);
        if (Writer is not null) Writer.WriteBigInt(variable.GetValueOrDefault(defaultValue));
        return variable;
    }

    /// <summary>
    /// Reads or writes a <paramref name="byteLength"/> amount of bytes as a <see cref="BigInteger"/> through reference.
    /// </summary>
    /// <param name="variable">Variable to read or write. Read mode sets <paramref name="variable"/>, write mode uses <paramref name="variable"/> to write the value (keeping <paramref name="variable"/> unchanged).</param>
    /// <param name="byteLength">Amount of bytes to use for reading/writing.</param>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    public void BigInt(ref BigInteger variable, int byteLength) => variable = BigInt(variable, byteLength);

    /// <summary>
    /// Reads or writes a <paramref name="byteLength"/> amount of bytes as a nullable <see cref="BigInteger"/> through reference.
    /// </summary>
    /// <param name="variable">Variable to read or write. Read mode sets <paramref name="variable"/>, write mode uses <paramref name="variable"/> to write the value (keeping <paramref name="variable"/> unchanged). If <paramref name="variable"/> is null, <paramref name="defaultValue"/> is written instead.</param>
    /// <param name="byteLength">Amount of bytes to use for reading/writing.</param>
    /// <param name="defaultValue">Value written when <paramref name="variable"/> is null. Ignored in read mode.</param>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    public void BigInt(ref BigInteger? variable, int byteLength, BigInteger defaultValue = default) => variable = BigInt(variable, byteLength, defaultValue);

    /// <summary>
    /// Reads or writes a number with a size of 16 bytes as a <see cref="BigInteger"/>.
    /// </summary>
    /// <param name="variable">Variable to write. Ignored in read mode.</param>
    /// <returns>Value read in read mode. In write mode, <paramref name="variable"/> is returned.</returns>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    public BigInteger Int128(BigInteger variable = default) => BigInt(variable, byteLength: 16);

    /// <summary>
    /// Reads or writes a number with a size of 16 bytes as a nullable <see cref="BigInteger"/>.
    /// </summary>
    /// <param name="variable">Variable to write. If null, <paramref name="defaultValue"/> is written. Ignored in read mode.</param>
    /// <param name="defaultValue">Value written when <paramref name="variable"/> is null. Ignored in read mode.</param>
    /// <returns>Value read in read mode. In write mode, <paramref name="variable"/> is returned (including null).</returns>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    public BigInteger? Int128(BigInteger? variable, BigInteger defaultValue = default) => BigInt(variable, byteLength: 16, defaultValue);

    /// <summary>
    /// Reads or writes a number with a size of 16 bytes as a <see cref="BigInteger"/> through reference.
    /// </summary>
    /// <param name="variable">Variable to read or write. Read mode sets <paramref name="variable"/>, write mode uses <paramref name="variable"/> to write the value (keeping <paramref name="variable"/> unchanged).</param>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    public void Int128(ref BigInteger variable) => BigInt(ref variable, byteLength: 16);

    /// <summary>
    /// Reads or writes a number with a size of 16 bytes as a nullable <see cref="BigInteger"/> through reference.
    /// </summary>
    /// <param name="variable">Variable to read or write. Read mode sets <paramref name="variable"/>, write mode uses <paramref name="variable"/> to write the value (keeping <paramref name="variable"/> unchanged). If <paramref name="variable"/> is null, <paramref name="defaultValue"/> is written instead.</param>
    /// <param name="defaultValue">Value written when <paramref name="variable"/> is null. Ignored in read mode.</param>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    public void Int128(ref BigInteger? variable, BigInteger defaultValue = default) => BigInt(ref variable, byteLength: 16, defaultValue);

    /// <summary>
    /// Reads or writes a <see cref="NET.Int2"/>.
    /// </summary>
    /// <param name="variable">Variable to write. Ignored in read mode.</param>
    /// <returns>Value read in read mode. In write mode, <paramref name="variable"/> is returned.</returns>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    public Int2 Int2(Int2 variable = default)
    {
        if (Reader is not null) return Reader.ReadInt2();
        if (Writer is not null) Writer.Write(variable);
        return variable;
    }

    /// <summary>
    /// Reads or writes a nullable <see cref="NET.Int2"/>.
    /// </summary>
    /// <param name="variable">Variable to write. If null, <paramref name="defaultValue"/> is written. Ignored in read mode.</param>
    /// <param name="defaultValue">Value written when <paramref name="variable"/> is null. Ignored in read mode.</param>
    /// <returns>Value read in read mode. In write mode, <paramref name="variable"/> is returned (including null).</returns>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    public Int2? Int2(Int2? variable = default, Int2 defaultValue = default)
    {
        if (Reader is not null) return Reader.ReadInt2();
        if (Writer is not null) Writer.Write(variable.GetValueOrDefault(defaultValue));
        return variable;
    }

    /// <summary>
    /// Reads or writes an <see cref="NET.Int2"/> through reference.
    /// </summary>
    /// <param name="variable">Variable to read or write. Read mode sets <paramref name="variable"/>, write mode uses <paramref name="variable"/> to write the value (keeping <paramref name="variable"/> unchanged).</param>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    public void Int2(ref Int2 variable) => variable = Int2(variable);

    /// <summary>
    /// Reads or writes a nullable <see cref="NET.Int2"/> through reference.
    /// </summary>
    /// <param name="variable">Variable to read or write. Read mode sets <paramref name="variable"/>, write mode uses <paramref name="variable"/> to write the value (keeping <paramref name="variable"/> unchanged). If <paramref name="variable"/> is null, <paramref name="defaultValue"/> is written instead.</param>
    /// <param name="defaultValue">Value written when <paramref name="variable"/> is null. Ignored in read mode.</param>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    public void Int2(ref Int2? variable, Int2 defaultValue = default) => variable = Int2(variable, defaultValue);

    /// <summary>
    /// Reads or writes a <see cref="NET.Int3"/>.
    /// </summary>
    /// <param name="variable">Variable to write. Ignored in read mode.</param>
    /// <returns>Value read in read mode. In write mode, <paramref name="variable"/> is returned.</returns>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    public Int3 Int3(Int3 variable = default)
    {
        if (Reader is not null) return Reader.ReadInt3();
        if (Writer is not null) Writer.Write(variable);
        return variable;
    }

    /// <summary>
    /// Reads or writes a nullable <see cref="NET.Int3"/>.
    /// </summary>
    /// <param name="variable">Variable to write. If null, <paramref name="defaultValue"/> is written. Ignored in read mode.</param>
    /// <param name="defaultValue">Value written when <paramref name="variable"/> is null. Ignored in read mode.</param>
    /// <returns>Value read in read mode. In write mode, <paramref name="variable"/> is returned (including null).</returns>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    public Int3? Int3(Int3? variable, Int3 defaultValue = default)
    {
        if (Reader is not null) return Reader.ReadInt3();
        if (Writer is not null) Writer.Write(variable.GetValueOrDefault(defaultValue));
        return variable;
    }

    /// <summary>
    /// Reads or writes an <see cref="NET.Int3"/> through reference.
    /// </summary>
    /// <param name="variable">Variable to read or write. Read mode sets <paramref name="variable"/>, write mode uses <paramref name="variable"/> to write the value (keeping <paramref name="variable"/> unchanged).</param>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    public void Int3(ref Int3 variable) => variable = Int3(variable);

    /// <summary>
    /// Reads or writes a nullable <see cref="NET.Int3"/> through reference.
    /// </summary>
    /// <param name="variable">Variable to read or write. Read mode sets <paramref name="variable"/>, write mode uses <paramref name="variable"/> to write the value (keeping <paramref name="variable"/> unchanged). If <paramref name="variable"/> is null, <paramref name="defaultValue"/> is written instead.</param>
    /// <param name="defaultValue">Value written when <paramref name="variable"/> is null. Ignored in read mode.</param>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    public void Int3(ref Int3? variable, Int3 defaultValue = default) => variable = Int3(variable, defaultValue);

    /// <summary>
    /// Reads or writes a <see cref="NET.Byte3"/>.
    /// </summary>
    /// <param name="variable">Variable to write. Ignored in read mode.</param>
    /// <returns>Value read in read mode. In write mode, <paramref name="variable"/> is returned.</returns>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    public Byte3 Byte3(Byte3 variable = default)
    {
        if (Reader is not null) return Reader.ReadByte3();
        if (Writer is not null) Writer.Write(variable);
        return variable;
    }

    /// <summary>
    /// Reads or writes a nullable <see cref="NET.Byte3"/>.
    /// </summary>
    /// <param name="variable">Variable to write. If null, <paramref name="defaultValue"/> is written. Ignored in read mode.</param>
    /// <param name="defaultValue">Value written when <paramref name="variable"/> is null. Ignored in read mode.</param>
    /// <returns>Value read in read mode. In write mode, <paramref name="variable"/> is returned (including null).</returns>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    public Byte3? Byte3(Byte3? variable, Byte3 defaultValue = default)
    {
        if (Reader is not null) return Reader.ReadByte3();
        if (Writer is not null) Writer.Write(variable.GetValueOrDefault(defaultValue));
        return variable;
    }

    /// <summary>
    /// Reads or writes an <see cref="NET.Byte3"/> through reference.
    /// </summary>
    /// <param name="variable">Variable to read or write. Read mode sets <paramref name="variable"/>, write mode uses <paramref name="variable"/> to write the value (keeping <paramref name="variable"/> unchanged).</param>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    public void Byte3(ref Byte3 variable) => variable = Byte3(variable);

    /// <summary>
    /// Reads or writes a nullable <see cref="NET.Byte3"/> through reference.
    /// </summary>
    /// <param name="variable">Variable to read or write. Read mode sets <paramref name="variable"/>, write mode uses <paramref name="variable"/> to write the value (keeping <paramref name="variable"/> unchanged). If <paramref name="variable"/> is null, <paramref name="defaultValue"/> is written instead.</param>
    /// <param name="defaultValue">Value written when <paramref name="variable"/> is null. Ignored in read mode.</param>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    public void Byte3(ref Byte3? variable, Byte3 defaultValue = default) => variable = Byte3(variable, defaultValue);

    /// <summary>
    /// Reads or writes a <see cref="NET.Vec2"/>.
    /// </summary>
    /// <param name="variable">Variable to write. Ignored in read mode.</param>
    /// <returns>Value read in read mode. In write mode, <paramref name="variable"/> is returned.</returns>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    public Vec2 Vec2(Vec2 variable = default)
    {
        if (Reader is not null) return Reader.ReadVec2();
        if (Writer is not null) Writer.Write(variable);
        return variable;
    }

    /// <summary>
    /// Reads or writes a nullable <see cref="NET.Vec2"/>.
    /// </summary>
    /// <param name="variable">Variable to write. If null, <paramref name="defaultValue"/> is written. Ignored in read mode.</param>
    /// <param name="defaultValue">Value written when <paramref name="variable"/> is null. Ignored in read mode.</param>
    /// <returns>Value read in read mode. In write mode, <paramref name="variable"/> is returned (including null).</returns>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    public Vec2? Vec2(Vec2? variable, Vec2 defaultValue = default)
    {
        if (Reader is not null) return Reader.ReadVec2();
        if (Writer is not null) Writer.Write(variable.GetValueOrDefault(defaultValue));
        return variable;
    }

    /// <summary>
    /// Reads or writes an <see cref="NET.Vec2"/> through reference.
    /// </summary>
    /// <param name="variable">Variable to read or write. Read mode sets <paramref name="variable"/>, write mode uses <paramref name="variable"/> to write the value (keeping <paramref name="variable"/> unchanged).</param>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    public void Vec2(ref Vec2 variable) => variable = Vec2(variable);

    /// <summary>
    /// Reads or writes a nullable <see cref="NET.Vec2"/> through reference.
    /// </summary>
    /// <param name="variable">Variable to read or write. Read mode sets <paramref name="variable"/>, write mode uses <paramref name="variable"/> to write the value (keeping <paramref name="variable"/> unchanged). If <paramref name="variable"/> is null, <paramref name="defaultValue"/> is written instead.</param>
    /// <param name="defaultValue">Value written when <paramref name="variable"/> is null. Ignored in read mode.</param>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    public void Vec2(ref Vec2? variable, Vec2 defaultValue = default) => variable = Vec2(variable, defaultValue);

    /// <summary>
    /// Reads or writes a <see cref="NET.Vec3"/>.
    /// </summary>
    /// <param name="variable">Variable to write. Ignored in read mode.</param>
    /// <returns>Value read in read mode. In write mode, <paramref name="variable"/> is returned.</returns>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    public Vec3 Vec3(Vec3 variable = default)
    {
        if (Reader is not null) return Reader.ReadVec3();
        if (Writer is not null) Writer.Write(variable);
        return variable;
    }

    /// <summary>
    /// Reads or writes a nullable <see cref="NET.Vec3"/>.
    /// </summary>
    /// <param name="variable">Variable to write. If null, <paramref name="defaultValue"/> is written. Ignored in read mode.</param>
    /// <param name="defaultValue">Value written when <paramref name="variable"/> is null. Ignored in read mode.</param>
    /// <returns>Value read in read mode. In write mode, <paramref name="variable"/> is returned (including null).</returns>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    public Vec3? Vec3(Vec3? variable, Vec3 defaultValue = default)
    {
        if (Reader is not null) return Reader.ReadVec3();
        if (Writer is not null) Writer.Write(variable.GetValueOrDefault(defaultValue));
        return variable;
    }

    /// <summary>
    /// Reads or writes an <see cref="NET.Vec3"/> through reference.
    /// </summary>
    /// <param name="variable">Variable to read or write. Read mode sets <paramref name="variable"/>, write mode uses <paramref name="variable"/> to write the value (keeping <paramref name="variable"/> unchanged).</param>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    public void Vec3(ref Vec3 variable) => variable = Vec3(variable);

    /// <summary>
    /// Reads or writes a nullable <see cref="NET.Vec3"/> through reference.
    /// </summary>
    /// <param name="variable">Variable to read or write. Read mode sets <paramref name="variable"/>, write mode uses <paramref name="variable"/> to write the value (keeping <paramref name="variable"/> unchanged). If <paramref name="variable"/> is null, <paramref name="defaultValue"/> is written instead.</param>
    /// <param name="defaultValue">Value written when <paramref name="variable"/> is null. Ignored in read mode.</param>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    public void Vec3(ref Vec3? variable, Vec3 defaultValue = default) => variable = Vec3(variable, defaultValue);

    /// <summary>
    /// Reads or writes a <see cref="NET.Vec4"/>.
    /// </summary>
    /// <param name="variable">Variable to write. Ignored in read mode.</param>
    /// <returns>Value read in read mode. In write mode, <paramref name="variable"/> is returned.</returns>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    public Vec4 Vec4(Vec4 variable = default)
    {
        if (Reader is not null) return Reader.ReadVec4();
        if (Writer is not null) Writer.Write(variable);
        return variable;
    }

    /// <summary>
    /// Reads or writes a nullable <see cref="NET.Vec4"/>.
    /// </summary>
    /// <param name="variable">Variable to write. If null, <paramref name="defaultValue"/> is written. Ignored in read mode.</param>
    /// <param name="defaultValue">Value written when <paramref name="variable"/> is null. Ignored in read mode.</param>
    /// <returns>Value read in read mode. In write mode, <paramref name="variable"/> is returned (including null).</returns>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    public Vec4? Vec4(Vec4? variable, Vec4 defaultValue = default)
    {
        if (Reader is not null) return Reader.ReadVec4();
        if (Writer is not null) Writer.Write(variable.GetValueOrDefault(defaultValue));
        return variable;
    }

    /// <summary>
    /// Reads or writes an <see cref="NET.Vec4"/> through reference.
    /// </summary>
    /// <param name="variable">Variable to read or write. Read mode sets <paramref name="variable"/>, write mode uses <paramref name="variable"/> to write the value (keeping <paramref name="variable"/> unchanged).</param>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    public void Vec4(ref Vec4 variable) => variable = Vec4(variable);

    /// <summary>
    /// Reads or writes a nullable <see cref="NET.Vec4"/> through reference.
    /// </summary>
    /// <param name="variable">Variable to read or write. Read mode sets <paramref name="variable"/>, write mode uses <paramref name="variable"/> to write the value (keeping <paramref name="variable"/> unchanged). If <paramref name="variable"/> is null, <paramref name="defaultValue"/> is written instead.</param>
    /// <param name="defaultValue">Value written when <paramref name="variable"/> is null. Ignored in read mode.</param>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    public void Vec4(ref Vec4? variable, Vec4 defaultValue = default) => variable = Vec4(variable, defaultValue);

    /// <summary>
    /// Reads or writes a <see cref="NET.Rect"/>.
    /// </summary>
    /// <param name="variable">Variable to write. Ignored in read mode.</param>
    /// <returns>Value read in read mode. In write mode, <paramref name="variable"/> is returned.</returns>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    public Rect Rect(Rect variable = default)
    {
        if (Reader is not null) return Reader.ReadRect();
        if (Writer is not null) Writer.Write(variable);
        return variable;
    }

    /// <summary>
    /// Reads or writes a nullable <see cref="NET.Rect"/>.
    /// </summary>
    /// <param name="variable">Variable to write. If null, <paramref name="defaultValue"/> is written. Ignored in read mode.</param>
    /// <param name="defaultValue">Value written when <paramref name="variable"/> is null. Ignored in read mode.</param>
    /// <returns>Value read in read mode. In write mode, <paramref name="variable"/> is returned (including null).</returns>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    public Rect? Rect(Rect? variable, Rect defaultValue = default)
    {
        if (Reader is not null) return Reader.ReadRect();
        if (Writer is not null) Writer.Write(variable.GetValueOrDefault(defaultValue));
        return variable;
    }

    /// <summary>
    /// Reads or writes an <see cref="NET.Rect"/> through reference.
    /// </summary>
    /// <param name="variable">Variable to read or write. Read mode sets <paramref name="variable"/>, write mode uses <paramref name="variable"/> to write the value (keeping <paramref name="variable"/> unchanged).</param>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    public void Rect(ref Rect variable) => variable = Rect(variable);

    /// <summary>
    /// Reads or writes a nullable <see cref="NET.Rect"/> through reference.
    /// </summary>
    /// <param name="variable">Variable to read or write. Read mode sets <paramref name="variable"/>, write mode uses <paramref name="variable"/> to write the value (keeping <paramref name="variable"/> unchanged). If <paramref name="variable"/> is null, <paramref name="defaultValue"/> is written instead.</param>
    /// <param name="defaultValue">Value written when <paramref name="variable"/> is null. Ignored in read mode.</param>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    public void Rect(ref Rect? variable, Rect defaultValue = default) => variable = Rect(variable, defaultValue);
    
    /// <summary>
    /// Reads or writes a <see cref="NET.Box"/>.
    /// </summary>
    /// <param name="variable">Variable to write. Ignored in read mode.</param>
    /// <returns>Value read in read mode. In write mode, <paramref name="variable"/> is returned.</returns>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    public Box Box(Box variable = default)
    {
        if (Reader is not null) return Reader.ReadBox();
        if (Writer is not null) Writer.Write(variable);
        return variable;
    }

    /// <summary>
    /// Reads or writes a nullable <see cref="NET.Box"/>.
    /// </summary>
    /// <param name="variable">Variable to write. If null, <paramref name="defaultValue"/> is written. Ignored in read mode.</param>
    /// <param name="defaultValue">Value written when <paramref name="variable"/> is null. Ignored in read mode.</param>
    /// <returns>Value read in read mode. In write mode, <paramref name="variable"/> is returned (including null).</returns>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    public Box? Box(Box? variable, Box defaultValue = default)
    {
        if (Reader is not null) return Reader.ReadBox();
        if (Writer is not null) Writer.Write(variable.GetValueOrDefault(defaultValue));
        return variable;
    }

    /// <summary>
    /// Reads or writes an <see cref="NET.Box"/> through reference.
    /// </summary>
    /// <param name="variable">Variable to read or write. Read mode sets <paramref name="variable"/>, write mode uses <paramref name="variable"/> to write the value (keeping <paramref name="variable"/> unchanged).</param>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    public void Box(ref Box variable) => variable = Box(variable);

    /// <summary>
    /// Reads or writes a nullable <see cref="NET.Box"/> through reference.
    /// </summary>
    /// <param name="variable">Variable to read or write. Read mode sets <paramref name="variable"/>, write mode uses <paramref name="variable"/> to write the value (keeping <paramref name="variable"/> unchanged). If <paramref name="variable"/> is null, <paramref name="defaultValue"/> is written instead.</param>
    /// <param name="defaultValue">Value written when <paramref name="variable"/> is null. Ignored in read mode.</param>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    public void Box(ref Box? variable, Box defaultValue = default) => variable = Box(variable, defaultValue);

    /// <summary>
    /// Reads or writes a <see cref="NET.Quat"/>.
    /// </summary>
    /// <param name="variable">Variable to write. Ignored in read mode.</param>
    /// <returns>Value read in read mode. In write mode, <paramref name="variable"/> is returned.</returns>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    public Quat Quat(Quat variable = default)
    {
        if (Reader is not null) return Reader.ReadQuat();
        if (Writer is not null) Writer.Write(variable);
        return variable;
    }

    /// <summary>
    /// Reads or writes a nullable <see cref="NET.Quat"/>.
    /// </summary>
    /// <param name="variable">Variable to write. If null, <paramref name="defaultValue"/> is written. Ignored in read mode.</param>
    /// <param name="defaultValue">Value written when <paramref name="variable"/> is null. Ignored in read mode.</param>
    /// <returns>Value read in read mode. In write mode, <paramref name="variable"/> is returned (including null).</returns>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    public Quat? Quat(Quat? variable, Quat defaultValue = default)
    {
        if (Reader is not null) return Reader.ReadQuat();
        if (Writer is not null) Writer.Write(variable.GetValueOrDefault(defaultValue));
        return variable;
    }

    /// <summary>
    /// Reads or writes an <see cref="NET.Quat"/> through reference.
    /// </summary>
    /// <param name="variable">Variable to read or write. Read mode sets <paramref name="variable"/>, write mode uses <paramref name="variable"/> to write the value (keeping <paramref name="variable"/> unchanged).</param>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    public void Quat(ref Quat variable) => variable = Quat(variable);

    /// <summary>
    /// Reads or writes a nullable <see cref="NET.Quat"/> through reference.
    /// </summary>
    /// <param name="variable">Variable to read or write. Read mode sets <paramref name="variable"/>, write mode uses <paramref name="variable"/> to write the value (keeping <paramref name="variable"/> unchanged). If <paramref name="variable"/> is null, <paramref name="defaultValue"/> is written instead.</param>
    /// <param name="defaultValue">Value written when <paramref name="variable"/> is null. Ignored in read mode.</param>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    public void Quat(ref Quat? variable, Quat defaultValue = default) => variable = Quat(variable, defaultValue);

    /// <summary>
    /// Reads or writes a <see cref="NET.FileRef"/>.
    /// </summary>
    /// <param name="variable">Variable to write. Ignored in read mode.</param>
    /// <returns>Value read in read mode. In write mode, <paramref name="variable"/> is returned.</returns>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="StringLengthOutOfRangeException">String length is negative.</exception>
    public FileRef? FileRef(FileRef? variable = default)
    {
        if (Reader is not null) return Reader.ReadFileRef();
        if (Writer is not null) Writer.Write(variable ?? new FileRef());
        return variable;
    }

    /// <summary>
    /// Reads or writes an <see cref="NET.FileRef"/> through reference.
    /// </summary>
    /// <param name="variable">Variable to read or write. Read mode sets <paramref name="variable"/>, write mode uses <paramref name="variable"/> to write the value (keeping <paramref name="variable"/> unchanged).</param>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="StringLengthOutOfRangeException">String length is negative.</exception>
    public void FileRef(ref FileRef? variable) => variable = FileRef(variable);

    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="NotSupportedException">GBX has the first Id presented without a version. Solution exists, but the stream does not support seeking.</exception>
    /// <exception cref="StringLengthOutOfRangeException">String length is negative.</exception>
    /// <exception cref="CorruptedIdException">The Id index is not matching any known values.</exception>
    /// <exception cref="PropertyNullException"><see cref="GameBoxReaderSettings.StateGuid"/> or <see cref="GameBoxWriterSettings.StateGuid"/> is null.</exception>
    public string? Id(string? variable = default)
    {
        if (Reader is not null) return Reader.ReadId();
        if (Writer is not null) Writer.WriteId(variable ?? "");
        return variable;
    }

    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="NotSupportedException">GBX has the first Id presented without a version. Solution exists, but the stream does not support seeking.</exception>
    /// <exception cref="StringLengthOutOfRangeException">String length is negative.</exception>
    /// <exception cref="CorruptedIdException">The Id index is not matching any known values.</exception>
    public void Id(ref string? variable) => variable = Id(variable);

    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="PropertyNullException"><see cref="GameBoxReaderSettings.StateGuid"/> or <see cref="GameBoxWriterSettings.StateGuid"/> is null.</exception>
    /// <exception cref="NotSupportedException">GBX has the first Id presented without a version. Solution exists, but the stream does not support seeking.</exception>
    /// <exception cref="StringLengthOutOfRangeException">String length is negative.</exception>
    /// <exception cref="CorruptedIdException">The Id index is not matching any known values.</exception>
    public Ident? Ident(Ident? variable = default)
    {
        if (Reader is not null) return Reader.ReadIdent();
        if (Writer is not null) Writer.Write(variable);
        return variable;
    }

    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="PropertyNullException"><see cref="GameBoxReaderSettings.StateGuid"/> or <see cref="GameBoxWriterSettings.StateGuid"/> is null.</exception>
    /// <exception cref="NotSupportedException">GBX has the first Id presented without a version. Solution exists, but the stream does not support seeking.</exception>
    /// <exception cref="StringLengthOutOfRangeException">String length is negative.</exception>
    /// <exception cref="CorruptedIdException">The Id index is not matching any known values.</exception>
    public void Ident(ref Ident? variable) => variable = Ident(variable);

    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    public void EnumByte<T>(ref T variable) where T : struct, Enum
    {
        var v = Mode == GameBoxReaderWriterMode.Write ? CastTo<byte>.From(variable) : default;

        Byte(ref v);

        if (Mode == GameBoxReaderWriterMode.Read)
            variable = CastTo<T>.From(v);
    }

    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    public void EnumInt32<T>(ref T variable) where T : struct, Enum
    {
        var v = Mode == GameBoxReaderWriterMode.Write ? CastTo<int>.From(variable) : default;

        Int32(ref v);

        if (Mode == GameBoxReaderWriterMode.Read)
            variable = CastTo<T>.From(v);
    }

    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    public void EnumInt32<T>(ref T? variable, T defaultValue = default) where T : struct, Enum
    {
        var v = Mode == GameBoxReaderWriterMode.Write ? CastTo<int?>.From(variable) : default;

        if (Mode == GameBoxReaderWriterMode.Write)
        {
            if (defaultValue.Equals(default(T)))
            {
                Int32(ref v);
                return;
            }

            Int32(ref v, CastTo<int>.From(defaultValue));
        }

        if (Mode == GameBoxReaderWriterMode.Read)
        {
            Int32(ref v);
            variable = CastTo<T>.From(v);
        }
    }

    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="ArgumentException"><paramref name="readPrefix"/> is <see cref="StringLengthPrefix.None"/>.</exception>
    /// <exception cref="StringLengthOutOfRangeException">String length is negative.</exception>
    public string? String(string? variable = default, StringLengthPrefix readPrefix = default)
    {
        if (Reader is not null) return Reader.ReadString(readPrefix);
        if (Writer is not null) Writer.Write(variable, readPrefix);
        return variable;
    }

    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="ArgumentException"><paramref name="readPrefix"/> is <see cref="StringLengthPrefix.None"/>.</exception>
    /// <exception cref="StringLengthOutOfRangeException">String length is negative.</exception>
    public void String(ref string? variable, StringLengthPrefix readPrefix = default) => variable = String(variable, readPrefix);

    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="ArgumentException"><paramref name="readPrefix"/> is <see cref="StringLengthPrefix.None"/>.</exception>
    /// <exception cref="StringLengthOutOfRangeException">String length is negative.</exception>
    public void Uri(ref Uri? variable, StringLengthPrefix readPrefix = default)
    {
        if (Reader is not null) System.Uri.TryCreate(Reader.ReadString(readPrefix), UriKind.Absolute, out variable);
        if (Writer is not null) Writer.Write(variable?.ToString(), readPrefix);
    }

    /// <exception cref="ArgumentException">The number of decoded characters to read is greater than count. This can happen if a Unicode decoder returns fallback characters or a surrogate pair.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="count"/> is negative.</exception>
    public byte[]? Bytes(byte[]? variable = default, int? count = null)
    {
        if (count is null)
        {
            if (Reader is not null) return Reader.ReadBytes();
            if (Writer is not null)
            {
                if (variable is null)
                {
                    Writer.Write(0);
                    return variable;
                }

                Writer.Write(variable.Length);
                Writer.Write(variable);
            }

            return variable;
        }

        var c = count.Value;

        if (Reader is not null) return Reader.ReadBytes(c);
        if (Writer is not null)
        {
            if (c < 0)
                throw new ArgumentOutOfRangeException(nameof(count), "Count is negative");

            Writer.Write(variable ?? new byte[c], 0, c);

            return variable;
        }

        return variable;
    }

    /// <exception cref="ArgumentException">The number of decoded characters to read is greater than count. This can happen if a Unicode decoder returns fallback characters or a surrogate pair.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="count"/> is negative.</exception>
    public void Bytes(ref byte[]? variable, int count)
    {
        if (Reader is not null) variable = Reader.ReadBytes(count);
        if (Writer is not null && variable is not null) Writer.Write(variable, 0, count);
    }

    /// <exception cref="ArgumentException">The number of decoded characters to read is greater than count. This can happen if a Unicode decoder returns fallback characters or a surrogate pair.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    public void Bytes(ref byte[]? variable) => variable = Bytes(variable);

    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Array length is negative.</exception>
    public T[]? Array<T>(T[]? array = default, int? count = null) where T : struct
    {
        if (count is null)
        {
            if (Reader is not null) return Reader.ReadArray<T>();
            if (Writer is not null) Writer.WriteArray(array);
            return array;
        }

        if (Reader is not null) return Reader.ReadArray<T>(count.Value);
        if (Writer is not null && array is not null) Writer.WriteArray_NoPrefix(array);
        return array;
    }

    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Array length is negative.</exception>
    public void Array<T>(ref T[]? array, int count) where T : struct => array = Array(array, count);

    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Array length is negative.</exception>
    public void Array<T>(ref T[]? array) where T : struct => array = Array(array);

    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="forLoopRead"/> or <paramref name="forLoopWrite"/> is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Array length is negative.</exception>
    public T[]? Array<T>(T[]? array, Func<int, T> forLoopRead, Action<T> forLoopWrite)
    {
        if (Reader is not null) return Reader.ReadArray(forLoopRead);
        if (Writer is not null) Writer.Write(array, forLoopWrite);
        return array;
    }

    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="forLoopRead"/> or <paramref name="forLoopWrite"/> is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Array length is negative.</exception>
    public void Array<T>(ref T[]? array, Func<int, T> forLoopRead, Action<T> forLoopWrite)
    {
        array = Array(array, forLoopRead, forLoopWrite);
    }

    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="forLoopRead"/> or <paramref name="forLoopWrite"/> is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Array length is negative.</exception>
    public T[]? Array<T>(T[]? array, Func<int, GameBoxReader, T> forLoopRead, Action<T, GameBoxWriter> forLoopWrite)
    {
        if (Reader is not null) return Reader.ReadArray(forLoopRead);
        if (Writer is not null) Writer.Write(array, forLoopWrite);
        return array;
    }

    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="forLoopRead"/> or <paramref name="forLoopWrite"/> is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Array length is negative.</exception>
    public void Array<T>(ref T[]? array, Func<int, GameBoxReader, T> forLoopRead, Action<T, GameBoxWriter> forLoopWrite)
    {
        array = Array(array, forLoopRead, forLoopWrite);
    }

    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="forLoopRead"/> or <paramref name="forLoopWrite"/> is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Array length is negative.</exception>
    public T[]? Array<T>(T[]? array, Func<T> forLoopRead, Action<T> forLoopWrite)
    {
        if (Reader is not null) return Reader.ReadArray(forLoopRead);
        if (Writer is not null) Writer.Write(array, forLoopWrite);
        return array;
    }

    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="forLoopRead"/> or <paramref name="forLoopWrite"/> is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Array length is negative.</exception>
    public void Array<T>(ref T[]? array, Func<T> forLoopRead, Action<T> forLoopWrite)
    {
        array = Array(array, forLoopRead, forLoopWrite);
    }

    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="forLoopRead"/> or <paramref name="forLoopWrite"/> is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Array length is negative.</exception>
    public T[]? Array<T>(T[]? array, Func<GameBoxReader, T> forLoopRead, Action<T, GameBoxWriter> forLoopWrite)
    {
        if (Reader is not null) return Reader.ReadArray(forLoopRead);
        if (Writer is not null) Writer.Write(array, forLoopWrite);
        return array;
    }

    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="forLoopRead"/> or <paramref name="forLoopWrite"/> is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Array length is negative.</exception>
    public void Array<T>(ref T[]? array, Func<GameBoxReader, T> forLoopRead, Action<T, GameBoxWriter> forLoopWrite)
    {
        array = Array(array, forLoopRead, forLoopWrite);
    }

    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="forLoopReadWrite"/> is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException">List count is negative.</exception>
    public T[]? Array<T>(T[]? array, Action<GameBoxReaderWriter, T> forLoopReadWrite) where T : new()
    {
        if (forLoopReadWrite is null)
        {
            throw new ArgumentNullException(nameof(forLoopReadWrite));
        }

        if (Reader is not null)
        {
            var length = Reader.ReadInt32();

            array = new T[length];

            for (int i = 0; i < length; i++)
            {
                var t = new T();
                forLoopReadWrite(this, t);
                array[i] = t;
            }

            return array;
        }

        if (Writer is not null)
        {
            if (array is null)
            {
                Writer.Write(0);
                return array;
            }

            Writer.Write(array.Length);

            for (int i = 0; i < array.Length; i++)
            {
                forLoopReadWrite(this, array[i]);
            }
        }

        return array;
    }

    public void Array<T>(ref T[]? array, Action<GameBoxReaderWriter, T> forLoopReadWrite) where T : new()
    {
        array = Array(array, forLoopReadWrite);
    }

    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Array length is negative.</exception>
    public Vec2[]? ArrayVec2(Vec2[]? array = default)
    {
        return Array(array, r => r.ReadVec2(), (x, w) => w.Write(x));
    }

    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Array length is negative.</exception>
    public void ArrayVec2(ref Vec2[]? array)
    {
        array = Array(array, r => r.ReadVec2(), (x, w) => w.Write(x));
    }

    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Array length is negative.</exception>
    public Vec3[]? ArrayVec3(Vec3[]? array = default)
    {
        return Array(array, r => r.ReadVec3(), (x, w) => w.Write(x));
    }

    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Array length is negative.</exception>
    public void ArrayVec3(ref Vec3[]? array)
    {
        array = Array(array, r => r.ReadVec3(), (x, w) => w.Write(x));
    }

    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Array length is negative.</exception>
    public string[]? ArrayString(string[]? array = default)
    {
        return Array(array, r => r.ReadString(), (x, w) => w.Write(x));
    }

    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Array length is negative.</exception>
    public void ArrayString(ref string[]? array)
    {
        array = Array(array, r => r.ReadString(), (x, w) => w.Write(x));
    }

    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Array length is negative.</exception>
    /// <exception cref="PropertyNullException">Body of <see cref="Reader"/> or <see cref="Writer"/> is null.</exception>
    public T?[]? ArrayNode<T>(T?[]? array = default) where T : Node
    {
        return Array(array, r => r.ReadNodeRef<T>(), (x, w) => w.Write(x));
    }

    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Array length is negative.</exception>
    /// <exception cref="PropertyNullException">Body of <see cref="Reader"/> or <see cref="Writer"/> is null.</exception>
    public void ArrayNode<T>(ref T?[]? array) where T : Node
    {
        array = Array(array, r => r.ReadNodeRef<T>(), (x, w) => w.Write(x));
    }

    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="forLoopRead"/> or <paramref name="forLoopWrite"/> is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException">List count is negative.</exception>
    public IList<T>? List<T>(IList<T>? list, Func<int, T> forLoopRead, Action<T> forLoopWrite)
    {
        if (Reader is not null) return Reader.ReadList(forLoopRead);
        if (Writer is not null) Writer.Write(list, forLoopWrite);
        return list;
    }

    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="forLoopRead"/> or <paramref name="forLoopWrite"/> is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException">List count is negative.</exception>
    public void List<T>(ref IList<T>? list, Func<int, T> forLoopRead, Action<T> forLoopWrite)
    {
        list = List(list, forLoopRead, forLoopWrite);
    }

    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="forLoopRead"/> or <paramref name="forLoopWrite"/> is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException">List count is negative.</exception>
    public IList<T>? List<T>(IList<T>? list, Func<int, GameBoxReader, T> forLoopRead, Action<T, GameBoxWriter> forLoopWrite)
    {
        if (Reader is not null) return Reader.ReadList(forLoopRead);
        if (Writer is not null) Writer.Write(list, forLoopWrite);
        return list;
    }

    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="forLoopRead"/> or <paramref name="forLoopWrite"/> is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException">List count is negative.</exception>
    public void List<T>(ref IList<T>? list, Func<int, GameBoxReader, T> forLoopRead, Action<T, GameBoxWriter> forLoopWrite)
    {
        list = List(list, forLoopRead, forLoopWrite);
    }

    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="forLoopRead"/> or <paramref name="forLoopWrite"/> is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException">List count is negative.</exception>
    public IList<T>? List<T>(IList<T>? list, Func<T> forLoopRead, Action<T> forLoopWrite)
    {
        if (Reader is not null) return Reader.ReadList(forLoopRead);
        if (Writer is not null) Writer.Write(list, forLoopWrite);
        return list;
    }

    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="forLoopRead"/> or <paramref name="forLoopWrite"/> is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException">List count is negative.</exception>
    public void List<T>(ref IList<T>? list, Func<T> forLoopRead, Action<T> forLoopWrite)
    {
        list = List(list, forLoopRead, forLoopWrite);
    }

    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="forLoopRead"/> or <paramref name="forLoopWrite"/> is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException">List count is negative.</exception>
    public IList<T>? List<T>(IList<T>? list, Func<GameBoxReader, T> forLoopRead, Action<T, GameBoxWriter> forLoopWrite)
    {
        if (Reader is not null) return Reader.ReadList(forLoopRead);
        if (Writer is not null) Writer.Write(list, forLoopWrite);
        return list;
    }

    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="forLoopRead"/> or <paramref name="forLoopWrite"/> is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException">List count is negative.</exception>
    public void List<T>(ref IList<T>? list, Func<GameBoxReader, T> forLoopRead, Action<T, GameBoxWriter> forLoopWrite)
    {
        list = List(list, forLoopRead, forLoopWrite);
    }

    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="forLoopReadWrite"/> is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException">List count is negative.</exception>
    public IList<T>? List<T>(IList<T>? list, Action<GameBoxReaderWriter, T> forLoopReadWrite) where T : new()
    {
        if (forLoopReadWrite is null)
        {
            throw new ArgumentNullException(nameof(forLoopReadWrite));
        }

        if (Reader is not null)
        {
            var length = Reader.ReadInt32();

            list = new List<T>(length);

            for (int i = 0; i < length; i++)
            {
                var t = new T();
                forLoopReadWrite(this, t);
                list.Add(t);
            }

            return list;
        }

        if (Writer is not null)
        {
            if (list is null)
            {
                Writer.Write(0);
                return list;
            }

            Writer.Write(list.Count);

            foreach (var t in list)
            {
                forLoopReadWrite(this, t);
            }
        }

        return list;
    }

    public void List<T>(ref IList<T>? list, Action<GameBoxReaderWriter, T> forLoopReadWrite) where T : new()
    {
        list = List(list, forLoopReadWrite);
    }

    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="forLoopRead"/> or <paramref name="forLoopWrite"/> is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException">List count is negative.</exception>
    public async Task<IList<T>?> ListAsync<T>(IList<T>? list, Func<GameBoxReader, Task<T>> forLoopRead, Action<T, GameBoxWriter> forLoopWrite)
    {
        if (Reader is not null) return await Reader.ReadListAsync(forLoopRead);
        if (Writer is not null) Writer.Write(list, forLoopWrite);
        return list;
    }

    public void ListKey<T>(ref IList<T>? list, int version) where T : CGameCtnMediaBlock.Key, new()
    {
        List(ref list, (rw, x) => x.ReadWrite(rw, version));
    }

    public void ListKey<T>(ref IList<T>? list) where T : CGameCtnMediaBlock.Key, new()
    {
        // Almost duplicate method content to avoid closures
        List(ref list, (rw, x) => x.ReadWrite(rw));
    }

    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="ArgumentOutOfRangeException">List count is negative.</exception>
    public IList<FileRef>? ListFileRef(IList<FileRef>? list = default)
    {
        return List(list,
            r => r.ReadFileRef(),
            (x, w) => w.Write(x));
    }

    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="ArgumentOutOfRangeException">List count is negative.</exception>
    public void ListFileRef(ref IList<FileRef>? list)
    {
        list = ListFileRef(list);
    }

    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="ArgumentOutOfRangeException">List count is negative.</exception>
    /// <exception cref="PropertyNullException">Body of <see cref="Reader"/> or <see cref="Writer"/> is null.</exception>
    public IList<T?>? ListNode<T>(IList<T?>? list = default) where T : Node
    {
        return List(list,
            r => r.ReadNodeRef<T>(),
            (x, w) => w.Write(x));
    }

    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="ArgumentOutOfRangeException">List count is negative.</exception>
    /// <exception cref="PropertyNullException">Body of <see cref="Reader"/> or <see cref="Writer"/> is null.</exception>
    public void ListNode<T>(ref IList<T?>? list) where T : Node
    {
        list = ListNode(list);
    }

    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="ArgumentOutOfRangeException">List count is negative.</exception>
    /// <exception cref="PropertyNullException">Body of <see cref="Reader"/> or <see cref="Writer"/> is null.</exception>
    public async Task<IList<T?>?> ListNodeAsync<T>(IList<T?>? list = default, CancellationToken cancellationToken = default) where T : Node
    {
        return await ListAsync(list,
            async r => await r.ReadNodeRefAsync<T>(cancellationToken),
            (x, w) => w.Write(x));
    }

    /// <param name="dictionary">Dictionary to write. Ignored in read mode.</param>
    /// <param name="overrideKey">Only affecting read mode: if the pair in the dictionary should be overriden by the new value when a duplicate key is read. It is recommended to keep it false to easily spot errors.</param>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="ArgumentException">An element with the same key already exists in the dictionary.</exception>
    public IDictionary<TKey, TValue>? Dictionary<TKey, TValue>(IDictionary<TKey, TValue>? dictionary = default, bool overrideKey = false) where TKey : notnull
    {
        if (Reader is not null) return Reader.ReadDictionary<TKey, TValue>(overrideKey);
        if (Writer is not null) Writer.Write(dictionary);
        return dictionary;
    }

    /// <param name="dictionary">Dictionary to read or write. Read mode sets <paramref name="dictionary"/>, write mode uses <paramref name="dictionary"/> to write the value (keeping <paramref name="dictionary"/> unchanged).</param>
    /// <param name="overrideKey">Only affecting read mode: if the pair in the dictionary should be overriden by the new value when a duplicate key is read. It is recommended to keep it false to easily spot errors.</param>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="ArgumentException">An element with the same key already exists in the dictionary.</exception>
    public void Dictionary<TKey, TValue>(ref IDictionary<TKey, TValue>? dictionary, bool overrideKey = false) where TKey : notnull
    {
        dictionary = Dictionary(dictionary, overrideKey);
    }

    /// <param name="dictionary">Dictionary to write. Ignored in read mode.</param>
    /// <param name="overrideKey">Only affecting read mode: if the pair in the dictionary should be overriden by the new value when a duplicate key is read. It is recommended to keep it false to easily spot errors.</param>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="ArgumentException">An element with the same key already exists in the dictionary.</exception>
    /// <exception cref="PropertyNullException">Body of <see cref="Reader"/> or <see cref="Writer"/> is null.</exception>
    public IDictionary<TKey, TValue?>? DictionaryNode<TKey, TValue>(IDictionary<TKey, TValue?>? dictionary = default, bool overrideKey = false) where TKey : notnull where TValue : Node
    {
        if (Reader is not null) return Reader.ReadDictionaryNode<TKey, TValue>(overrideKey);
        if (Writer is not null) Writer.WriteDictionaryNode(dictionary);
        return dictionary;
    }

    /// <param name="dictionary">Dictionary to read or write. Read mode sets <paramref name="dictionary"/>, write mode uses <paramref name="dictionary"/> to write the value (keeping <paramref name="dictionary"/> unchanged).</param>
    /// <param name="overrideKey">Only affecting read mode: if the pair in the dictionary should be overriden by the new value when a duplicate key is read. It is recommended to keep it false to easily spot errors.</param>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="ArgumentException">An element with the same key already exists in the dictionary.</exception>
    /// <exception cref="PropertyNullException">Body of <see cref="Reader"/> or <see cref="Writer"/> is null.</exception>
    public void DictionaryNode<TKey, TValue>(ref IDictionary<TKey, TValue?>? dictionary, bool overrideKey = false) where TKey : notnull where TValue : Node
    {
        dictionary = DictionaryNode(dictionary, overrideKey);
    }

    public async Task<IDictionary<TKey, TValue?>?> DictionaryNodeAsync<TKey, TValue>(
        IDictionary<TKey, TValue?>? dictionary,
        bool overrideKey,
        CancellationToken cancellationToken = default)
        where TKey : notnull where TValue : Node
    {
        if (Reader is not null) return await Reader.ReadDictionaryNodeAsync<TKey, TValue>(overrideKey, cancellationToken);
        if (Writer is not null) Writer.WriteDictionaryNode(dictionary);
        return dictionary;
    }

    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="NotSupportedException">The stream does not support seeking.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="stream"/> is null.</exception>
    /// <exception cref="PropertyNullException"><see cref="GameBoxReaderSettings.StateGuid"/> or <see cref="GameBoxWriterSettings.StateGuid"/> is null.</exception>
    public void UntilFacade(MemoryStream stream)
    {
        if (stream is null)
            throw new ArgumentNullException(nameof(stream));

        if (Reader is not null)
        {
            if (Reader.Settings.StateGuid is null)
            {
                throw new PropertyNullException(nameof(Reader.Settings.StateGuid));
            }

            using var w = new GameBoxWriter(stream);
            w.Write(Reader.ReadUntilFacade().ToArray());

            return;
        }

        if (Writer is not null)
        {
            var buffer = new byte[stream.Length - stream.Position];
            stream.Read(buffer, 0, buffer.Length);
            Writer.WriteBytes(buffer);

            return;
        }

        throw new ThisShouldNotHappenException();
    }
}

/// <summary>
/// Reader-writer mode.
/// </summary>
public enum GameBoxReaderWriterMode
{
    /// <summary>
    /// Read mode.
    /// </summary>
    Read,
    /// <summary>
    /// Write mode.
    /// </summary>
    Write
}
