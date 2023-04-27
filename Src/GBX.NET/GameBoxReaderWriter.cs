using System.Numerics;

namespace GBX.NET;

/// <summary>
/// Provides single-method reading and writing by wrapping <see cref="GameBoxReader"/> and <see cref="GameBoxWriter"/> depending on the mode.
/// </summary>
public partial class GameBoxReaderWriter
{
    /// <summary>
    /// Reader component of the reader-writer.
    /// </summary>
    public GameBoxReader? Reader { get; }

    /// <summary>
    /// Writer component of the reader-writer.
    /// </summary>
    public GameBoxWriter? Writer { get; }

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
    /// Constructs a reader-writer in reader and writer mode at the same time.
    /// </summary>
    /// <param name="reader">Reader to use.</param>
    /// <param name="writer">Writer to use.</param>
    public GameBoxReaderWriter(GameBoxReader reader, GameBoxWriter writer)
    {
        Reader = reader;
        Writer = writer;
    }

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
        if (Reader is not null) variable = Reader.ReadBoolean(asByte);
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
        if (Reader is not null) variable = Reader.ReadBoolean(asByte);
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
        if (Reader is not null) variable = Reader.ReadByte();
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
        if (Reader is not null) variable = Reader.ReadByte();
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
        if (Reader is not null) variable = Reader.ReadInt16();
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
        if (Reader is not null) variable = Reader.ReadInt16();
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
        if (Reader is not null) variable = Reader.ReadInt32();
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
        if (Reader is not null) variable = Reader.ReadInt32();
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

    public int? Int32Nullable(int? variable)
    {
        if (Reader is not null)
        {
            variable = Reader.ReadInt32();
            if (variable == -1) variable = null;
        }
        
        Writer?.Write(variable ?? -1);

        return variable;
    }
    
    public void Int32Nullable(ref int? variable) => variable = Int32(variable);

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
        if (Reader is not null) variable = Reader.ReadInt64();
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
        if (Reader is not null) variable = Reader.ReadInt64();
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
        if (Reader is not null) variable = Reader.ReadUInt16();
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
        if (Reader is not null) variable = Reader.ReadUInt16();
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
        if (Reader is not null) variable = Reader.ReadUInt32();
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
        if (Reader is not null) variable = Reader.ReadUInt32();
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
        if (Reader is not null) variable = Reader.ReadUInt64();
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
        if (Reader is not null) variable = Reader.ReadUInt64();
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
        if (Reader is not null) variable = Reader.ReadSingle();
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
        if (Reader is not null) variable = Reader.ReadSingle();
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
        if (Reader is not null) variable = Reader.ReadBigInt(byteLength);
        if (Writer is not null) Writer.WriteBigInt(variable, byteLength);
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
        if (Reader is not null) variable = Reader.ReadBigInt(byteLength);
        if (Writer is not null) Writer.WriteBigInt(variable.GetValueOrDefault(defaultValue), byteLength);
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
        if (Reader is not null) variable = Reader.ReadInt2();
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
        if (Reader is not null) variable = Reader.ReadInt2();
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
        if (Reader is not null) variable = Reader.ReadInt3();
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
        if (Reader is not null) variable = Reader.ReadInt3();
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
        if (Reader is not null) variable = Reader.ReadByte3();
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
        if (Reader is not null) variable = Reader.ReadByte3();
        if (Writer is not null) Writer.Write(variable.GetValueOrDefault(defaultValue));
        return variable;
    }

    /// <summary>
    /// Reads or writes a <see cref="NET.Byte3"/> through reference.
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

    public Int3 Byte3(Int3 variable = default)
    {
        if (Reader is not null) variable = (Int3)Reader.ReadByte3();
        if (Writer is not null) Writer.Write((Byte3)variable);
        return variable;
    }

    public Int3? Byte3(Int3? variable, Int3 defaultValue = default)
    {
        if (Reader is not null) variable = (Int3)Reader.ReadByte3();
        if (Writer is not null) Writer.Write((Byte3)variable.GetValueOrDefault(defaultValue));
        return variable;
    }

    public void Byte3(ref Int3 variable) => variable = Byte3(variable);

    public void Byte3(ref Int3? variable, Int3 defaultValue = default) => variable = Byte3(variable, defaultValue);

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
        if (Reader is not null) variable = Reader.ReadVec2();
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
        if (Reader is not null) variable = Reader.ReadVec2();
        if (Writer is not null) Writer.Write(variable.GetValueOrDefault(defaultValue));
        return variable;
    }

    /// <summary>
    /// Reads or writes a <see cref="NET.Vec2"/> through reference.
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
        if (Reader is not null) variable = Reader.ReadVec3();
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
        if (Reader is not null) variable = Reader.ReadVec3();
        if (Writer is not null) Writer.Write(variable.GetValueOrDefault(defaultValue));
        return variable;
    }

    /// <summary>
    /// Reads or writes a <see cref="NET.Vec3"/> through reference.
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
        if (Reader is not null) variable = Reader.ReadVec4();
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
        if (Reader is not null) variable = Reader.ReadVec4();
        if (Writer is not null) Writer.Write(variable.GetValueOrDefault(defaultValue));
        return variable;
    }

    /// <summary>
    /// Reads or writes a <see cref="NET.Vec4"/> through reference.
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
        if (Reader is not null) variable = Reader.ReadRect();
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
        if (Reader is not null) variable = Reader.ReadRect();
        if (Writer is not null) Writer.Write(variable.GetValueOrDefault(defaultValue));
        return variable;
    }

    /// <summary>
    /// Reads or writes a <see cref="NET.Rect"/> through reference.
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
        if (Reader is not null) variable = Reader.ReadBox();
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
        if (Reader is not null) variable = Reader.ReadBox();
        if (Writer is not null) Writer.Write(variable.GetValueOrDefault(defaultValue));
        return variable;
    }

    /// <summary>
    /// Reads or writes a <see cref="NET.Box"/> through reference.
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
        if (Reader is not null) variable = Reader.ReadQuat();
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
        if (Reader is not null) variable = Reader.ReadQuat();
        if (Writer is not null) Writer.Write(variable.GetValueOrDefault(defaultValue));
        return variable;
    }

    /// <summary>
    /// Reads or writes a <see cref="NET.Quat"/> through reference.
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
    /// Reads or writes an <see cref="NET.Iso4"/>.
    /// </summary>
    /// <param name="variable">Variable to write. Ignored in read mode.</param>
    /// <returns>Value read in read mode. In write mode, <paramref name="variable"/> is returned.</returns>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    public Iso4 Iso4(Iso4 variable = default)
    {
        if (Reader is not null) variable = Reader.ReadIso4();
        if (Writer is not null) Writer.Write(variable);
        return variable;
    }

    /// <summary>
    /// Reads or writes a nullable <see cref="NET.Iso4"/>.
    /// </summary>
    /// <param name="variable">Variable to write. If null, <paramref name="defaultValue"/> is written. Ignored in read mode.</param>
    /// <param name="defaultValue">Value written when <paramref name="variable"/> is null. Ignored in read mode.</param>
    /// <returns>Value read in read mode. In write mode, <paramref name="variable"/> is returned (including null).</returns>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    public Iso4? Iso4(Iso4? variable, Iso4 defaultValue = default)
    {
        if (Reader is not null) variable = Reader.ReadIso4();
        if (Writer is not null) Writer.Write(variable.GetValueOrDefault(defaultValue));
        return variable;
    }

    /// <summary>
    /// Reads or writes an <see cref="NET.Iso4"/> through reference.
    /// </summary>
    /// <param name="variable">Variable to read or write. Read mode sets <paramref name="variable"/>, write mode uses <paramref name="variable"/> to write the value (keeping <paramref name="variable"/> unchanged).</param>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    public void Iso4(ref Iso4 variable) => variable = Iso4(variable);

    /// <summary>
    /// Reads or writes a nullable <see cref="NET.Iso4"/> through reference.
    /// </summary>
    /// <param name="variable">Variable to read or write. Read mode sets <paramref name="variable"/>, write mode uses <paramref name="variable"/> to write the value (keeping <paramref name="variable"/> unchanged). If <paramref name="variable"/> is null, <paramref name="defaultValue"/> is written instead.</param>
    /// <param name="defaultValue">Value written when <paramref name="variable"/> is null. Ignored in read mode.</param>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    public void Iso4(ref Iso4? variable, Iso4 defaultValue = default) => variable = Iso4(variable, defaultValue);

    /// <summary>
    /// Reads or writes an <see cref="NET.Mat3"/>.
    /// </summary>
    /// <param name="variable">Variable to write. Ignored in read mode.</param>
    /// <returns>Value read in read mode. In write mode, <paramref name="variable"/> is returned.</returns>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    public Mat3 Mat3(Mat3 variable = default)
    {
        if (Reader is not null) variable = Reader.ReadMat3();
        if (Writer is not null) Writer.Write(variable);
        return variable;
    }

    /// <summary>
    /// Reads or writes a nullable <see cref="NET.Mat3"/>.
    /// </summary>
    /// <param name="variable">Variable to write. If null, <paramref name="defaultValue"/> is written. Ignored in read mode.</param>
    /// <param name="defaultValue">Value written when <paramref name="variable"/> is null. Ignored in read mode.</param>
    /// <returns>Value read in read mode. In write mode, <paramref name="variable"/> is returned (including null).</returns>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    public Mat3? Mat3(Mat3? variable, Mat3 defaultValue = default)
    {
        if (Reader is not null) variable = Reader.ReadMat3();
        if (Writer is not null) Writer.Write(variable.GetValueOrDefault(defaultValue));
        return variable;
    }

    /// <summary>
    /// Reads or writes an <see cref="NET.Mat3"/> through reference.
    /// </summary>
    /// <param name="variable">Variable to read or write. Read mode sets <paramref name="variable"/>, write mode uses <paramref name="variable"/> to write the value (keeping <paramref name="variable"/> unchanged).</param>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    public void Mat3(ref Mat3 variable) => variable = Mat3(variable);

    /// <summary>
    /// Reads or writes a nullable <see cref="NET.Mat3"/> through reference.
    /// </summary>
    /// <param name="variable">Variable to read or write. Read mode sets <paramref name="variable"/>, write mode uses <paramref name="variable"/> to write the value (keeping <paramref name="variable"/> unchanged). If <paramref name="variable"/> is null, <paramref name="defaultValue"/> is written instead.</param>
    /// <param name="defaultValue">Value written when <paramref name="variable"/> is null. Ignored in read mode.</param>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    public void Mat3(ref Mat3? variable, Mat3 defaultValue = default) => variable = Mat3(variable, defaultValue);

    /// <summary>
    /// Reads or writes an <see cref="NET.Mat4"/>.
    /// </summary>
    /// <param name="variable">Variable to write. Ignored in read mode.</param>
    /// <returns>Value read in read mode. In write mode, <paramref name="variable"/> is returned.</returns>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    public Mat4 Mat4(Mat4 variable = default)
    {
        if (Reader is not null) variable = Reader.ReadMat4();
        if (Writer is not null) Writer.Write(variable);
        return variable;
    }

    /// <summary>
    /// Reads or writes a nullable <see cref="NET.Mat4"/>.
    /// </summary>
    /// <param name="variable">Variable to write. If null, <paramref name="defaultValue"/> is written. Ignored in read mode.</param>
    /// <param name="defaultValue">Value written when <paramref name="variable"/> is null. Ignored in read mode.</param>
    /// <returns>Value read in read mode. In write mode, <paramref name="variable"/> is returned (including null).</returns>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    public Mat4? Mat4(Mat4? variable, Mat4 defaultValue = default)
    {
        if (Reader is not null) variable = Reader.ReadMat4();
        if (Writer is not null) Writer.Write(variable.GetValueOrDefault(defaultValue));
        return variable;
    }

    /// <summary>
    /// Reads or writes an <see cref="NET.Mat4"/> through reference.
    /// </summary>
    /// <param name="variable">Variable to read or write. Read mode sets <paramref name="variable"/>, write mode uses <paramref name="variable"/> to write the value (keeping <paramref name="variable"/> unchanged).</param>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    public void Mat4(ref Mat4 variable) => variable = Mat4(variable);

    /// <summary>
    /// Reads or writes a nullable <see cref="NET.Mat4"/> through reference.
    /// </summary>
    /// <param name="variable">Variable to read or write. Read mode sets <paramref name="variable"/>, write mode uses <paramref name="variable"/> to write the value (keeping <paramref name="variable"/> unchanged). If <paramref name="variable"/> is null, <paramref name="defaultValue"/> is written instead.</param>
    /// <param name="defaultValue">Value written when <paramref name="variable"/> is null. Ignored in read mode.</param>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    public void Mat4(ref Mat4? variable, Mat4 defaultValue = default) => variable = Mat4(variable, defaultValue);

    /// <summary>
    /// Reads or writes a nullable time of day using <see cref="TimeSpan"/>.
    /// </summary>
    /// <param name="variable">Variable to write.Ignored in read mode.</param>
    /// <returns>Value read in read mode. In write mode, <paramref name="variable"/> is returned (including null).</returns>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    public TimeSpan? TimeOfDay(TimeSpan? variable = default)
    {
        if (Reader is not null) variable = Reader.ReadTimeOfDay();
        if (Writer is not null) Writer.WriteTimeOfDay(variable);
        return variable;
    }

    /// <summary>
    /// Reads or writes a nullable time of day using <see cref="TimeSpan"/> through reference.
    /// </summary>
    /// <param name="variable">Variable to read or write. Read mode sets <paramref name="variable"/>, write mode uses <paramref name="variable"/> to write the value (keeping <paramref name="variable"/> unchanged).</param>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    public void TimeOfDay(ref TimeSpan? variable)
    {
        variable = TimeOfDay(variable);
    }

    /// <summary>
    /// Reads or writes an <see cref="DateTime"/>.
    /// </summary>
    /// <param name="variable">Variable to write. Ignored in read mode.</param>
    /// <returns>Value read in read mode. In write mode, <paramref name="variable"/> is returned.</returns>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    public DateTime FileTime(DateTime variable = default)
    {
        if (Reader is not null) variable = Reader.ReadFileTime();
        if (Writer is not null) Writer.Write(variable);
        return variable;
    }

    /// <summary>
    /// Reads or writes a nullable <see cref="DateTime"/>.
    /// </summary>
    /// <param name="variable">Variable to write. If null, <paramref name="defaultValue"/> is written. Ignored in read mode.</param>
    /// <param name="defaultValue">Value written when <paramref name="variable"/> is null. Ignored in read mode.</param>
    /// <returns>Value read in read mode. In write mode, <paramref name="variable"/> is returned (including null).</returns>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    public DateTime? FileTime(DateTime? variable, DateTime defaultValue = default)
    {
        if (Reader is not null) variable = Reader.ReadFileTime();
        if (Writer is not null) Writer.Write(variable.GetValueOrDefault(defaultValue));
        return variable;
    }

    /// <summary>
    /// Reads or writes an <see cref="DateTime"/> through reference.
    /// </summary>
    /// <param name="variable">Variable to read or write. Read mode sets <paramref name="variable"/>, write mode uses <paramref name="variable"/> to write the value (keeping <paramref name="variable"/> unchanged).</param>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    public void FileTime(ref DateTime variable) => variable = FileTime(variable);

    /// <summary>
    /// Reads or writes a nullable <see cref="DateTime"/> through reference.
    /// </summary>
    /// <param name="variable">Variable to read or write. Read mode sets <paramref name="variable"/>, write mode uses <paramref name="variable"/> to write the value (keeping <paramref name="variable"/> unchanged). If <paramref name="variable"/> is null, <paramref name="defaultValue"/> is written instead.</param>
    /// <param name="defaultValue">Value written when <paramref name="variable"/> is null. Ignored in read mode.</param>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    public void FileTime(ref DateTime? variable, DateTime defaultValue = default) => variable = FileTime(variable, defaultValue);

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
        if (Reader is not null) variable = Reader.ReadFileRef();
        if (Writer is not null) Writer.Write(variable ?? NET.FileRef.Default);
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
    public string? Id(string? variable = default, bool tryParseToInt32 = false)
    {
        if (Reader is not null) variable = Reader.ReadId(cannotBeCollection: tryParseToInt32);
        if (Writer is not null) Writer.WriteId(variable ?? "", tryParseToInt32);
        return variable;
    }

    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="NotSupportedException">GBX has the first Id presented without a version. Solution exists, but the stream does not support seeking.</exception>
    /// <exception cref="StringLengthOutOfRangeException">String length is negative.</exception>
    /// <exception cref="CorruptedIdException">The Id index is not matching any known values.</exception>
    public void Id(ref string? variable, bool tryParseToInt32 = false) => variable = Id(variable, tryParseToInt32);

    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="NotSupportedException">GBX has the first Id presented without a version. Solution exists, but the stream does not support seeking.</exception>
    /// <exception cref="StringLengthOutOfRangeException">String length is negative.</exception>
    /// <exception cref="CorruptedIdException">The Id index is not matching any known values.</exception>
    public Id? Collection(Id? variable = default)
    {
        if (Reader is not null) variable = Reader.ReadId();
        if (Writer is not null) Writer.WriteId(variable ?? -1);
        return variable;
    }

    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="NotSupportedException">GBX has the first Id presented without a version. Solution exists, but the stream does not support seeking.</exception>
    /// <exception cref="StringLengthOutOfRangeException">String length is negative.</exception>
    /// <exception cref="CorruptedIdException">The Id index is not matching any known values.</exception>
    public void Collection(ref Id? variable) => variable = Collection(variable);

    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="NotSupportedException">GBX has the first Id presented without a version. Solution exists, but the stream does not support seeking.</exception>
    /// <exception cref="StringLengthOutOfRangeException">String length is negative.</exception>
    /// <exception cref="CorruptedIdException">The Id index is not matching any known values.</exception>
    public Ident? Ident(Ident? variable = default)
    {
        if (Reader is not null) variable = Reader.ReadIdent();
        if (Writer is not null) Writer.Write(variable);
        return variable;
    }

    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="NotSupportedException">GBX has the first Id presented without a version. Solution exists, but the stream does not support seeking.</exception>
    /// <exception cref="StringLengthOutOfRangeException">String length is negative.</exception>
    /// <exception cref="CorruptedIdException">The Id index is not matching any known values.</exception>
    public void Ident(ref Ident? variable) => variable = Ident(variable);

    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    public void EnumByte<T>(ref T variable) where T : struct, Enum
    {
        var v = Writer is not null ? CastTo<byte>.From(variable) : default;

        Byte(ref v);

        if (Reader is not null)
        {
            variable = CastTo<T>.From(v);
        }
    }

    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    public void EnumByte<T>(ref T? variable, T defaultValue = default) where T : struct, Enum
    {
        var v = Writer is not null ? CastTo<byte?>.From(variable) : default;

        if (Writer is not null)
        {
            if (defaultValue.Equals(default(T)))
            {
                Byte(ref v);
                return;
            }

            Byte(ref v, CastTo<byte>.From(defaultValue));
        }

        if (Reader is not null)
        {
            Byte(ref v);
            variable = CastTo<T>.From(v);
        }
    }

    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    public void EnumInt32<T>(ref T variable) where T : struct, Enum
    {
        var v = Writer is not null ? CastTo<int>.From(variable) : default;

        Int32(ref v);

        if (Reader is not null)
        {
            variable = CastTo<T>.From(v);
        }
    }

    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    public void EnumInt32<T>(ref T? variable, T defaultValue = default) where T : struct, Enum
    {
        var v = Writer is not null ? CastTo<int?>.From(variable) : default;

        if (Writer is not null)
        {
            if (defaultValue.Equals(default(T)))
            {
                Int32(ref v);
                return;
            }

            Int32(ref v, CastTo<int>.From(defaultValue));
        }

        if (Reader is not null)
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
        if (Reader is not null) variable = Reader.ReadString(readPrefix);
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
            if (Reader is not null) variable = Reader.ReadBytes();
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

        if (Reader is not null) variable = Reader.ReadBytes(c);
        if (Writer is not null)
        {
            if (c < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(count), "Count is negative");
            }
            
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
        if (Writer is not null && variable is not null) Writer.Write(variable, 0, count); // May bring trouble
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
            if (Reader is not null) array = Reader.ReadArray<T>();
            if (Writer is not null) Writer.WriteArray(array);
            return array;
        }

        if (Reader is not null) array = Reader.ReadArray<T>(count.Value);
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
        if (Reader is not null) array = Reader.ReadArray(forLoopRead);
        if (Writer is not null) Writer.WriteArray(array, forLoopWrite);
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
        if (Reader is not null) array = Reader.ReadArray(forLoopRead);
        if (Writer is not null) Writer.WriteArray(array, forLoopWrite);
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
        if (Reader is not null) array = Reader.ReadArray(forLoopRead);
        if (Writer is not null) Writer.WriteArray(array, forLoopWrite);
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
        if (Reader is not null) array = Reader.ReadArray(forLoopRead);
        if (Writer is not null) Writer.WriteArray(array, forLoopWrite);
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
    /// <exception cref="ArgumentOutOfRangeException">Array count is negative.</exception>
    public T[]? Array<T>(T[]? array, Action<GameBoxReaderWriter, T> forLoopReadWrite, bool shortLength = false) where T : new()
    {
        if (forLoopReadWrite is null)
        {
            throw new ArgumentNullException(nameof(forLoopReadWrite));
        }

        int length;

        if (shortLength)
        {
            length = Reader?.ReadUInt16() ?? array?.Length ?? 0;
            Writer?.Write((ushort)length);
        }
        else
        {
            length = Reader?.ReadInt32() ?? array?.Length ?? 0;
            Writer?.Write(length);
        }

        return Array(array, forLoopReadWrite, length);
    }

    public T[]? Array<T>(T[]? array, Action<GameBoxReaderWriter, T> forLoopReadWrite, int length) where T : new()
    {
        if (forLoopReadWrite is null)
        {
            throw new ArgumentNullException(nameof(forLoopReadWrite));
        }

        if (Reader is not null)
        {
            array = new T[length];
        }

        if (array is null)
        {
            return null;
        }

        for (int i = 0; i < length; i++)
        {
            var t = array[i];

            if (Reader is not null)
            {
                t = new T();
            }

            if (Writer is not null)
            {
                t = array[i];
            }

            forLoopReadWrite(this, t);

            if (Reader is not null)
            {
                array[i] = t;
            }
        }

        return array;
    }

    public void Array<T>(ref T[]? array, Action<GameBoxReaderWriter, T> forLoopReadWrite, bool shortLength = false) where T : new()
    {
        array = Array(array, forLoopReadWrite, shortLength);
    }

    public T[]? ArrayArchive<T>(T[]? array, bool shortLength = false) where T : IReadableWritable, new()
    {
        return Array(array, (rw, x) => x.ReadWrite(rw), shortLength);
    }

    public void ArrayArchive<T>(ref T[]? array, bool shortLength = false) where T : IReadableWritable, new()
    {
        array = ArrayArchive(array, shortLength);
    }

    public T[]? ArrayArchive<T>(T[]? array, int version, bool shortLength = false) where T : IReadableWritable, new()
    {
        return Array(array, (rw, x) => x.ReadWrite(rw, version), shortLength);
    }

    public void ArrayArchive<T>(ref T[]? array, int version, bool shortLength = false) where T : IReadableWritable, new()
    {
        array = ArrayArchive(array, version, shortLength);
    }

    public T[]? ArrayArchive<T>(T[]? array, int version, int length) where T : IReadableWritable, new()
    {
        return Array(array, (rw, x) => x.ReadWrite(rw, version), length);
    }

    public void ArrayArchive<T>(ref T[]? array, int version, int length) where T : IReadableWritable, new()
    {
        array = ArrayArchive(array, version, length);
    }

    public T[]? ArrayArchiveWithGbx<T>(T[]? array, bool shortLength = false) where T : IReadableWritableWithGbx, new()
    {
        return Array(array, (rw, x) => x.ReadWrite(rw, Reader?.Gbx), shortLength);
    }

    public void ArrayArchiveWithGbx<T>(ref T[]? array, bool shortLength = false) where T : IReadableWritableWithGbx, new()
    {
        array = ArrayArchiveWithGbx(array, shortLength);
    }

    public T[]? ArrayArchiveWithGbx<T>(T[]? array, int version, bool shortLength = false) where T : IReadableWritableWithGbx, new()
    {
        return Array(array, (rw, x) => x.ReadWrite(rw, Reader?.Gbx, version), shortLength);
    }

    public void ArrayArchiveWithGbx<T>(ref T[]? array, int version, bool shortLength = false) where T : IReadableWritableWithGbx, new()
    {
        array = ArrayArchiveWithGbx(array, version, shortLength);
    }

    public T[]? ArrayArchiveWithNode<T, TNode>(T[]? array, TNode node, bool shortLength = false) where T : IReadableWritableWithNode<TNode>, new() where TNode : Node
    {
        return Array(array, (rw, x) => x.ReadWrite(rw, node), shortLength);
    }

    public void ArrayArchiveWithNode<T, TNode>(ref T[]? array, TNode node, bool shortLength = false) where T : IReadableWritableWithNode<TNode>, new() where TNode : Node
    {
        array = ArrayArchiveWithNode(array, node, shortLength);
    }

    public T[]? ArrayArchiveWithNode<T, TNode>(T[]? array, TNode node, int version, bool shortLength = false) where T : IReadableWritableWithNode<TNode>, new() where TNode : Node
    {
        return Array(array, (rw, x) => x.ReadWrite(rw, node, version), shortLength);
    }

    public void ArrayArchiveWithNode<T, TNode>(ref T[]? array, TNode node, int version, bool shortLength = false) where T : IReadableWritableWithNode<TNode>, new() where TNode : Node
    {
        array = ArrayArchiveWithNode(array, node, version, shortLength);
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
    public string[]? ArrayId(string[]? array = default)
    {
        return Array<string>(array, r => r.ReadId(), (x, w) => w.WriteId(x));
    }

    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Array length is negative.</exception>
    public void ArrayId(ref string[]? array)
    {
        array = Array<string>(array, r => r.ReadId(), (x, w) => w.WriteId(x));
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
    /// <exception cref="ArgumentOutOfRangeException">Array length is negative.</exception>
    /// <exception cref="PropertyNullException">Body of <see cref="Reader"/> or <see cref="Writer"/> is null.</exception>
    public ExternalNode<T>[]? ArrayNode<T>(ExternalNode<T>[]? array = default) where T : Node
    {
        if (Reader is not null) array = Reader.ReadExternalNodeArray<T>();
        if (Writer is not null) Writer.WriteExternalNodeArray(array);
        return array;
    }

    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Array length is negative.</exception>
    /// <exception cref="PropertyNullException">Body of <see cref="Reader"/> or <see cref="Writer"/> is null.</exception>
    public void ArrayNode<T>(ref ExternalNode<T>[]? array) where T : Node
    {
        array = ArrayNode(array);
    }

    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="forLoopRead"/> or <paramref name="forLoopWrite"/> is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException">List count is negative.</exception>
    public IList<T>? List<T>(IList<T>? list, Func<int, T> forLoopRead, Action<T> forLoopWrite)
    {
        if (Reader is not null) list = Reader.ReadList(forLoopRead);
        if (Writer is not null) Writer.WriteList(list, forLoopWrite);
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
        if (Reader is not null) list = Reader.ReadList(forLoopRead);
        if (Writer is not null) Writer.WriteList(list, forLoopWrite);
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
        if (Reader is not null) list = Reader.ReadList(forLoopRead);
        if (Writer is not null) Writer.WriteList(list, forLoopWrite);
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
        if (Reader is not null) list = Reader.ReadList(forLoopRead);
        if (Writer is not null) Writer.WriteList(list, forLoopWrite);
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

        var count = Reader?.ReadInt32() ?? list?.Count ?? 0;
        Writer?.Write(count);

        if (Reader is not null)
        {
            list = new List<T>(count);
        }

        if (list is null)
        {
            return null;
        }

        for (int i = 0; i < count; i++)
        {
            var t = default(T);

            if (Reader is not null)
            {
                t = new T();
            }

            if (Writer is not null)
            {
                t ??= list[i];
            }

            if (t is null)
            {
                throw new ThisShouldNotHappenException();
            }

            forLoopReadWrite(this, t);

            if (Reader is not null)
            {
                list.Add(t);
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
        if (Reader is not null) list = await Reader.ReadListAsync(forLoopRead);
        if (Writer is not null) Writer.WriteList(list, forLoopWrite);
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
        return List(list, r => r.ReadFileRef(), (x, w) => w.Write(x));
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
        return List(list, r => r.ReadNodeRef<T>(), (x, w) => w.Write(x));
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

    public IList<T>? ListArchive<T>(IList<T>? list) where T : IReadableWritable, new()
    {
        return List(list, (rw, x) => x.ReadWrite(rw));
    }

    public void ListArchive<T>(ref IList<T>? list) where T : IReadableWritable, new()
    {
        list = ListArchive(list);
    }

    public IList<T>? ListArchive<T>(IList<T>? list, int version) where T : IReadableWritable, new()
    {
        return List(list, (rw, x) => x.ReadWrite(rw, version));
    }

    public void ListArchive<T>(ref IList<T>? list, int version) where T : IReadableWritable, new()
    {
        list = ListArchive(list, version);
    }

    /// <param name="dictionary">Dictionary to write. Ignored in read mode.</param>
    /// <param name="overrideKey">Only affecting read mode: if the pair in the dictionary should be overriden by the new value when a duplicate key is read. It is recommended to keep it false to easily spot errors.</param>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="ArgumentException">An element with the same key already exists in the dictionary.</exception>
    public IDictionary<TKey, TValue>? Dictionary<TKey, TValue>(IDictionary<TKey, TValue>? dictionary = default, bool overrideKey = false) where TKey : notnull
    {
        if (Reader is not null) dictionary = Reader.ReadDictionary<TKey, TValue>(overrideKey);
        if (Writer is not null) Writer.WriteDictionary(dictionary);
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

    /// <param name="dictionary">Dictionary of <see cref="Node"/> values to write. Ignored in read mode.</param>
    /// <param name="overrideKey">Only affecting read mode: if the pair in the dictionary should be overriden by the new value when a duplicate key is read. It is recommended to keep it false to easily spot errors.</param>
    /// <param name="keyReaderWriter">A way to read and write the key. Null will use the <see cref="GameBoxReader.Read{T}"/> and <see cref="GameBoxWriter.WriteAny(object)"/>, which are slower.</param>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="ArgumentException">An element with the same key already exists in the dictionary.</exception>
    /// <exception cref="PropertyNullException">Body of <see cref="Reader"/> or <see cref="Writer"/> is null.</exception>
    public IDictionary<TKey, TValue?>? DictionaryNode<TKey, TValue>(IDictionary<TKey, TValue?>? dictionary = default,
        bool overrideKey = false,
        (Func<GameBoxReader, TKey>, Action<TKey, GameBoxWriter>)? keyReaderWriter = null)
        where TKey : notnull where TValue : Node
    {
        if (Reader is not null) dictionary = Reader.ReadDictionaryNode<TKey, TValue>(overrideKey, keyReaderWriter?.Item1);
        if (Writer is not null) Writer.WriteDictionaryNode(dictionary, keyReaderWriter?.Item2);
        return dictionary;
    }

    /// <param name="dictionary">Dictionary of <see cref="Node"/> values to read or write. Read mode sets <paramref name="dictionary"/>, write mode uses <paramref name="dictionary"/> to write the value (keeping <paramref name="dictionary"/> unchanged).</param>
    /// <param name="overrideKey">Only affecting read mode: if the pair in the dictionary should be overriden by the new value when a duplicate key is read. It is recommended to keep it false to easily spot errors.</param>
    /// <param name="keyReaderWriter">A way to read and write the key. Null will use the <see cref="GameBoxReader.Read{T}"/> and <see cref="GameBoxWriter.WriteAny(object)"/>, which are slower.</param>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="ArgumentException">An element with the same key already exists in the dictionary.</exception>
    /// <exception cref="PropertyNullException">Body of <see cref="Reader"/> or <see cref="Writer"/> is null.</exception>
    public void DictionaryNode<TKey, TValue>(ref IDictionary<TKey, TValue?>? dictionary,
        bool overrideKey = false,
        (Func<GameBoxReader, TKey>, Action<TKey, GameBoxWriter>)? keyReaderWriter = null)
        where TKey : notnull where TValue : Node
    {
        dictionary = DictionaryNode(dictionary, overrideKey, keyReaderWriter);
    }

    /// <param name="dictionary">Dictionary of <see cref="Node"/> values to read or write. Read mode sets <paramref name="dictionary"/>, write mode uses <paramref name="dictionary"/> to write the value (keeping <paramref name="dictionary"/> unchanged).</param>
    /// <param name="overrideKey">Only affecting read mode: if the pair in the dictionary should be overriden by the new value when a duplicate key is read. It is recommended to keep it false to easily spot errors.</param>
    /// <param name="keyReaderWriter">A way to read and write the key. Null will use the <see cref="GameBoxReader.Read{T}"/> and <see cref="GameBoxWriter.WriteAny(object)"/>, which are slower.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    public async Task<IDictionary<TKey, TValue?>?> DictionaryNodeAsync<TKey, TValue>(
        IDictionary<TKey, TValue?>? dictionary,
        bool overrideKey = false,
        (Func<GameBoxReader, TKey>, Action<TKey, GameBoxWriter>)? keyReaderWriter = null,
        CancellationToken cancellationToken = default)
        where TKey : notnull where TValue : Node
    {
        if (Reader is not null) dictionary = await Reader.ReadDictionaryNodeAsync<TKey, TValue>(overrideKey, keyReaderWriter?.Item1, cancellationToken);
        if (Writer is not null) Writer.WriteDictionaryNode(dictionary, keyReaderWriter?.Item2);
        return dictionary;
    }
    
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    public Node? NodeRef(Node? variable = default)
    {
        if (Reader is not null) variable = Reader.ReadNodeRef();
        if (Writer is not null) Writer.Write(variable);
        return variable;
    }

    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    public void NodeRef(ref Node? variable) => variable = NodeRef(variable);

    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    public Node? NodeRef(Node? variable, ref GameBoxRefTable.File? nodeRefFile)
    {
        if (Reader is not null) variable = Reader.ReadNodeRef(out nodeRefFile);
        if (Writer is not null) Writer.Write(variable, nodeRefFile);
        return variable;
    }

    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    public void NodeRef(ref Node? variable, ref GameBoxRefTable.File? nodeRefFile)
    {
        variable = NodeRef(variable, ref nodeRefFile);
    }

    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    public T? NodeRef<T>(T? variable = default) where T : Node
    {
        if (Reader is not null) variable = Reader.ReadNodeRef<T>();
        if (Writer is not null) Writer.Write(variable);
        return variable;
    }

    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    public void NodeRef<T>(ref T? variable, bool disallowOverride = false) where T : Node
    {
        if (disallowOverride)
        {
            var node = NodeRef(variable);
            variable ??= node;
        }
        else
        {
            variable = NodeRef(variable);
        }
    }

    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    public T? NodeRef<T>(T? variable, ref GameBoxRefTable.File? nodeRefFile) where T : Node
    {
        if (Reader is not null) variable = Reader.ReadNodeRef<T>(out nodeRefFile);
        if (Writer is not null) Writer.Write(variable, nodeRefFile);
        return variable;
    }

    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    public void NodeRef<T>(ref T? variable, ref GameBoxRefTable.File? nodeRefFile) where T : Node
    {
        variable = NodeRef(variable, ref nodeRefFile);
    }

    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    public async Task<Node?> NodeRefAsync(Node? variable = default, CancellationToken cancellationToken = default)
    {
        if (Reader is not null) variable = await Reader.ReadNodeRefAsync(cancellationToken);
        if (Writer is not null) Writer.Write(variable);
        return variable;
    }

    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    public async Task<T?> NodeRefAsync<T>(T? variable = default, CancellationToken cancellationToken = default) where T : Node
    {
        if (Reader is not null) variable = await Reader.ReadNodeRefAsync<T>(cancellationToken);
        if (Writer is not null) Writer.Write(variable);
        return variable;
    }

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
        if (Reader is not null) variable = Reader.ReadTimeInt32();
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
        if (Reader is not null) variable = Reader.ReadTimeInt32();
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
        if (Reader is not null) variable = Reader.ReadTimeInt32Nullable();
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
        if (Reader is not null) variable = Reader.ReadTimeSingle();
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
        if (Reader is not null) variable = Reader.ReadTimeSingle();
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
        if (Reader is not null) variable = Reader.ReadTimeSingleNullable();
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
        if (Reader is not null) variable = Reader.ReadInt32_s();
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
        if (Reader is not null) variable = Reader.ReadInt32_s();
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
        if (Reader is not null) variable = Reader.ReadInt32_ms();
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
        if (Reader is not null) variable = Reader.ReadInt32_ms();
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
        if (Reader is not null) variable = Reader.ReadInt32_sn();
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
        if (Reader is not null) variable = Reader.ReadInt32_msn();
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
        if (Reader is not null) variable = Reader.ReadSingle_s();
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
        if (Reader is not null) variable = Reader.ReadSingle_s();
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
        if (Reader is not null) variable = Reader.ReadSingle_ms();
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
        if (Reader is not null) variable = Reader.ReadSingle_ms();
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
        if (Reader is not null) variable = Reader.ReadSingle_sn();
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
        if (Reader is not null) variable = Reader.ReadSingle_msn();
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
    
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    public int OptimizedInt(int variable, int determineFrom)
    {
        if (Reader is not null) variable = Reader.ReadOptimizedInt(determineFrom);
        if (Writer is not null) Writer.WriteOptimizedInt(variable, determineFrom);
        return variable;
    }
    
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    public int? OptimizedInt(int? variable, int determineFrom, int defaultValue = default)
    {
        if (Reader is not null) variable = Reader.ReadOptimizedInt(determineFrom);
        if (Writer is not null) Writer.WriteOptimizedInt(variable.GetValueOrDefault(defaultValue), determineFrom);
        return variable;
    }
    
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    public void OptimizedInt(ref int variable, int determineFrom) => variable = OptimizedInt(variable, determineFrom);
    
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    public void OptimizedInt(ref int? variable, int determineFrom) => variable = OptimizedInt(variable, determineFrom);

    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Array length is negative.</exception>
    public int[]? OptimizedIntArray(int[]? array = default, int? count = null, int? determineFrom = null)
    {
        if (count is null)
        {
            if (Reader is not null) array = Reader.ReadOptimizedIntArray(determineFrom);
            if (Writer is not null) Writer.WriteOptimizedIntArray(array, determineFrom);
            return array;
        }

        if (Reader is not null) array = Reader.ReadOptimizedIntArray(count.Value, determineFrom);
        if (Writer is not null && array is not null) Writer.WriteOptimizedIntArray_NoPrefix(array, determineFrom);
        return array;
    }

    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Array length is negative.</exception>
    public void OptimizedIntArray(ref int[]? array, int count, int? determineFrom = null) => array = OptimizedIntArray(array, count, determineFrom);

    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Array length is negative.</exception>
    public void OptimizedIntArray(ref int[]? array, int? determineFrom = null) => array = OptimizedIntArray(array, determineFrom: determineFrom);

    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="NotSupportedException">The stream does not support seeking.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="stream"/> is null.</exception>
    public void UntilFacade(MemoryStream stream)
    {
        if (stream is null)
        {
            throw new ArgumentNullException(nameof(stream));
        }

        if (Reader is not null)
        {
            using var w = new GameBoxWriter(stream);
            w.Write(Reader.ReadUntilFacade().ToArray());

            return;
        }

        if (Writer is not null)
        {
            var buffer = new byte[stream.Length - stream.Position];
            stream.Read(buffer, 0, buffer.Length);
            Writer.Write(buffer);

            return;
        }

        throw new ThisShouldNotHappenException();
    }

    public T? Archive<T>(T? obj, int version = 0) where T : IReadableWritable, new()
    {
        if (obj is null)
        {
            if (Reader is not null)
            {
                obj = new();
            }
        }

        obj?.ReadWrite(this, version);

        return obj;
    }

    /// <summary>
    /// Reads or writes a comprehensive binary component, usually in a way it's defined in the game code.
    /// </summary>
    /// <remarks>Binary component has to be defined with the <see cref="IReadableWritable"/> interface.</remarks>
    public void Archive<T>(ref T? obj, int version = 0) where T : IReadableWritable, new()
    {
        obj = Archive(obj, version);
    }

    public void VersionInt32(IVersionable versionable)
    {
        versionable.Version = Int32(versionable.Version);
    }

    public void VersionByte(IVersionable versionable)
    {
        versionable.Version = Byte(versionable.Version);
    }

    public void ArrayVec3_10b(ref Vec3[]? array, int count)
    {
        if (Reader is not null) array = Reader.ReadArray(count, r => Reader.ReadVec3_10b());
        if (Writer is not null) Writer.WriteArray(array, (x, w) => w.WriteVec3_10b(x));
    }
}