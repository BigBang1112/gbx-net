using System.Numerics;
using System.Text;

namespace GBX.NET;

/// <summary>
/// Reads data types from GameBox serialization.
/// </summary>
public partial class GameBoxReader : BinaryReader
{
    internal ILogger? Logger { get; }

    public GameBoxReaderSettings Settings { get; }

    /// <summary>
    /// Constructs a binary reader specialized for GBX.
    /// </summary>
    /// <param name="input">The input stream.</param>
    /// <param name="stateGuid">ID used to point to a state that stores node references and lookback strings. If null, <see cref="Node"/>, Id, or <see cref="Ident"/> cannot be read and <see cref="PropertyNullException"/> can be thrown.</param>
    /// <param name="asyncAction">Specialized executions during asynchronous reading.</param>
    /// <param name="logger">Logger.</param>
    public GameBoxReader(Stream input, Guid? stateGuid = null, GameBoxAsyncReadAction? asyncAction = null, ILogger? logger = null) : base(input, Encoding.UTF8, true)
    {
        Settings = new GameBoxReaderSettings(stateGuid, asyncAction);

        Logger = logger;
    }

    public GameBoxReader(Stream input, GameBoxReaderSettings settings, ILogger? logger = null) : base(input, Encoding.UTF8, true)
    {
        Settings = settings;

        Logger = logger;
    }

    /// <summary>
    /// Reads the next <see cref="int"/> from the current stream, casts it as <see cref="bool"/> and advances the current position of the stream by 4 bytes.
    /// </summary>
    /// <returns>A boolean.</returns>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    public override bool ReadBoolean() => Convert.ToBoolean(ReadInt32());

    /// <summary>
    /// If <paramref name="asByte"/> is true, reads the next <see cref="byte"/> from the current stream and casts it as <see cref="bool"/>. Otherwise <see cref="ReadBoolean()"/> is called.
    /// </summary>
    /// <param name="asByte">Read the boolean as <see cref="byte"/> or <see cref="int"/>.</param>
    /// <returns>A boolean.</returns>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    public bool ReadBoolean(bool asByte) => asByte ? base.ReadBoolean() : ReadBoolean();

    /// <summary>
    /// Reads a <see cref="string"/> from the current stream with one of the prefix reading methods.
    /// </summary>
    /// <param name="readPrefix">The method to read the prefix.</param>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="ArgumentException"><paramref name="readPrefix"/> is <see cref="StringLengthPrefix.None"/>.</exception>
    /// <exception cref="StringLengthOutOfRangeException">String length is negative.</exception>
    /// <returns>The string being read.</returns>
    public string ReadString(StringLengthPrefix readPrefix)
    {
        var length = readPrefix switch
        {
            StringLengthPrefix.Byte => ReadByte(),
            StringLengthPrefix.Int32 => ReadInt32(),
            _ => throw new ArgumentException("Can't read string without knowing its length."),
        };

        if (length < 0)
        {
            throw new StringLengthOutOfRangeException(length);
        }

        return ReadString(length);
    }

    /// <summary>
    /// Reads a <see cref="string"/> from the current stream. The string is prefixed with the length, encoded as <see cref="int"/>.
    /// </summary>
    /// <returns>The string being read.</returns>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="StringLengthOutOfRangeException">String length is negative.</exception>
    public override string ReadString() => ReadString(readPrefix: StringLengthPrefix.Int32);

    /// <summary>
    /// Reads a <see cref="string"/> from the current stream using the <paramref name="length"/> parameter.
    /// </summary>
    /// <param name="length">Length of the bytes to read.</param>
    /// <returns>The string being read.</returns>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="length"/> is negative.</exception>
    public string ReadString(int length) => Encoding.UTF8.GetString(ReadBytes(length));

    /// <summary>
    /// First reads an <see cref="int"/> representing the length, then reads the sequence of bytes.
    /// </summary>
    /// <exception cref="ArgumentException">The number of decoded characters to read is greater than count. This can happen if a Unicode decoder returns fallback characters or a surrogate pair.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Length is negative.</exception>
    /// <returns>A byte array.</returns>
    public byte[] ReadBytes() => ReadBytes(count: ReadInt32());

    /// <summary>
    /// First reads the <see cref="int"/> of the Id version, if not read yet, considering the information from state. Then reads an <see cref="int"/> (index) that holds the flags of the representing <see cref="string"/>. If the first 30 bits are 0, a fresh <see cref="string"/> is also read.
    /// </summary>
    /// <returns>A string.</returns>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="PropertyNullException"><see cref="GameBoxReaderSettings.StateGuid"/> is null.</exception>
    /// <exception cref="NotSupportedException">GBX has the first Id presented without a version. Solution exists, but the stream does not support seeking.</exception>
    /// <exception cref="StringLengthOutOfRangeException">String length is negative.</exception>
    /// <exception cref="CorruptedIdException">The Id index is not matching any known values.</exception>
    public Id ReadId()
    {
        if (Settings.StateGuid is null)
        {
            throw new PropertyNullException(nameof(Settings.StateGuid));
        }

        var stateGuid = Settings.StateGuid.Value;

        var idState = Settings.IdSubStateGuid is null
            ? StateManager.Shared.GetIdState(stateGuid)
            : StateManager.Shared.GetIdSubState(stateGuid, Settings.IdSubStateGuid.Value);

        if (idState.Version is null)
        {
            idState.Version = ReadInt32();

            // Edge-case scenario where Id doesn't have a version for whatever reason (can be multiple)
            if ((idState.Version & 0xC0000000) > 10)
            {
                idState.Version = 3;

                if (!BaseStream.CanSeek)
                {
                    throw new NotSupportedException("Gbx has the first Id presented without a version. Solution exists, but the stream does not support seeking.");
                }

                BaseStream.Seek(-4, SeekOrigin.Current);
            }
        }

        var index = ReadUInt32();

        if (index == uint.MaxValue)
        {
            return "";
        }

        if ((index >> 16 & 0x1FF) == 0x1FF) // ????
        {
            var bytes = ReadBytes(5);
            var length = bytes[2];
            return ReadString(length);
        }

        if ((index & 0x3FFF) == 0 && (index >> 30 == 1 || index >> 30 == 2))
        {
            var str = ReadString();
            idState.Strings.Add(str);
            return str;
        }

        if ((index & 0x3FFF) == 0x3FFF)
        {
            return (index >> 30) switch
            {
                2 => "Unassigned",
                3 => "",
                _ => throw new CorruptedIdException(index >> 30),
            };
        }

        if (index >> 30 == 0)
        {
            return new Id((int)index);
        }

        if (idState.Strings.Count > (index & 0x3FFF) - 1)
        {
            return idState.Strings[(int)(index & 0x3FFF) - 1];
        }

        return "";
    }

    /// <summary>
    /// Reads an Id using <see cref="ReadId"/> 3 times.
    /// </summary>
    /// <returns>An <see cref="Ident"/>.</returns>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="PropertyNullException"><see cref="GameBoxReaderSettings.StateGuid"/> is null.</exception>
    /// <exception cref="NotSupportedException">GBX has the first Id presented without a version. Solution exists, but the stream does not support seeking.</exception>
    /// <exception cref="StringLengthOutOfRangeException">String length is negative.</exception>
    /// <exception cref="CorruptedIdException">The Id index is not matching any known values.</exception>
    public Ident ReadIdent()
    {
        var id = ReadId();
        var collection = ReadId();
        var author = ReadId();

        return new Ident(id, collection, author);
    }

    /// <summary>
    /// Reads the file reference data type - file path, checksum and locator URL.
    /// </summary>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="StringLengthOutOfRangeException">String length is negative.</exception>
    public FileRef ReadFileRef()
    {
        var version = ReadByte();

        byte[]? checksum = null;
        string locatorUrl = "";

        if (version >= 3)
        {
            checksum = ReadBytes(32);
        }

        var filePath = ReadString();

        if ((filePath.Length > 0 && version >= 1) || version >= 3)
        {
            locatorUrl = ReadString();
        }

        return new FileRef(version, checksum, filePath, locatorUrl);
    }

    /// <summary>
    /// Reads 2 <see cref="float"/>s.
    /// </summary>
    /// <returns>A 2D vector.</returns>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    public Vec2 ReadVec2() => new(X: ReadSingle(), Y: ReadSingle());

    /// <summary>
    /// Reads 3 <see cref="float"/>s.
    /// </summary>
    /// <returns>A 3D vector.</returns>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    public Vec3 ReadVec3() => new(X: ReadSingle(), Y: ReadSingle(), Z: ReadSingle());

    /// <summary>
    /// Reads 4 <see cref="float"/>s.
    /// </summary>
    /// <returns>A 4D vector.</returns>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    public Vec4 ReadVec4() => new(X: ReadSingle(), Y: ReadSingle(), Z: ReadSingle(), W: ReadSingle());

    /// <summary>
    /// Reads 4 <see cref="float"/>s.
    /// </summary>
    /// <returns>A 4D quaternion.</returns>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    public Quat ReadQuat() => new(X: ReadSingle(), Y: ReadSingle(), Z: ReadSingle(), W: ReadSingle());

    /// <summary>
    /// Reads 3 <see cref="int"/>s.
    /// </summary>
    /// <returns>A 3-int.</returns>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    public Int3 ReadInt3() => new(X: ReadInt32(), Y: ReadInt32(), Z: ReadInt32());

    /// <summary>
    /// Reads 2 <see cref="int"/>s.
    /// </summary>
    /// <returns>A 2-int.</returns>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    public Int2 ReadInt2() => new(X: ReadInt32(), Y: ReadInt32());

    /// <summary>
    /// Reads 3 <see cref="byte"/>s.
    /// </summary>
    /// <returns>A 3-byte.</returns>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    public Byte3 ReadByte3() => new(X: ReadByte(), Y: ReadByte(), Z: ReadByte());

    /// <summary>
    /// Reads a <paramref name="byteLength"/> amount of bytes and converts them into <see cref="BigInteger"/>.
    /// </summary>
    /// <returns>A big integer.</returns>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    public BigInteger ReadBigInt(int byteLength) => new(ReadBytes(byteLength));

    /// <summary>
    /// Reads a 2D rectangle.
    /// </summary>
    /// <returns>A rectangle.</returns>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    public Rect ReadRect()
    {
        return new Rect(X: ReadSingle(),
                        Y: ReadSingle(),
                        X2: ReadSingle(),
                        Y2: ReadSingle());
    }

    /// <summary>
    /// Reads a 3D box.
    /// </summary>
    /// <returns>A box.</returns>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    public Box ReadBox()
    {
        return new Box(X: ReadSingle(),
                       Y: ReadSingle(),
                       Z: ReadSingle(),
                       X2: ReadSingle(),
                       Y2: ReadSingle(),
                       Z2: ReadSingle());
    }

    public TimeSpan? ReadTimeOfDay()
    {
        var dayTime = ReadInt32();

        if (dayTime == -1)
        {
            return null;
        }

        return TimeSpan.FromSeconds(dayTime / (double)ushort.MaxValue * new TimeSpan(23, 59, 59).TotalSeconds);
    }

    /// <summary>
    /// Reads a common transform representation.
    /// </summary>
    /// <returns>3D position, quaternion rotation, speed, vector velocity.</returns>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    public (Vec3 position, Quat rotation, float speed, Vec3 velocity) ReadTransform()
    {
        var pos = ReadVec3();
        
#if NETSTANDARD2_1_OR_GREATER || NET5_0_OR_GREATER

        var angle = ReadUInt16() / (float)ushort.MaxValue * MathF.PI;
        var axisHeading = ReadInt16() / (float)short.MaxValue * MathF.PI;
        var axisPitch = ReadInt16() / (float)short.MaxValue * MathF.PI / 2;
        var speed = (float)Math.Exp(ReadInt16() / 1000.0);
        var velocityHeading = ReadSByte() / (float)sbyte.MaxValue * MathF.PI;
        var velocityPitch = ReadSByte() / (float)sbyte.MaxValue * MathF.PI / 2;

        var axis = new Vec3((float)(MathF.Sin(angle) * MathF.Cos(axisPitch) * MathF.Cos(axisHeading)),
            (float)(MathF.Sin(angle) * MathF.Cos(axisPitch) * MathF.Sin(axisHeading)),
            (float)(MathF.Sin(angle) * MathF.Sin(axisPitch)));

        var quaternion = new Quat(axis, MathF.Cos(angle));

        var velocity = new Vec3((float)(speed * MathF.Cos(velocityPitch) * MathF.Cos(velocityHeading)),
            (float)(speed * MathF.Cos(velocityPitch) * MathF.Sin(velocityHeading)),
            (float)(speed * MathF.Sin(velocityPitch)));

#else

        var angle = ReadUInt16() / (double)ushort.MaxValue * Math.PI;
        var axisHeading = ReadInt16() / (double)short.MaxValue * Math.PI;
        var axisPitch = ReadInt16() / (double)short.MaxValue * Math.PI / 2;
        var speed = (float)Math.Exp(ReadInt16() / 1000.0);
        var velocityHeading = ReadSByte() / (double)sbyte.MaxValue * Math.PI;
        var velocityPitch = ReadSByte() / (double)sbyte.MaxValue * Math.PI / 2;

        var axis = new Vec3((float)(Math.Sin(angle) * Math.Cos(axisPitch) * Math.Cos(axisHeading)),
            (float)(Math.Sin(angle) * Math.Cos(axisPitch) * Math.Sin(axisHeading)),
            (float)(Math.Sin(angle) * Math.Sin(axisPitch)));

        var quaternion = new Quat(axis, (float)Math.Cos(angle));

        var velocity = new Vec3((float)(speed * Math.Cos(velocityPitch) * Math.Cos(velocityHeading)),
            (float)(speed * Math.Cos(velocityPitch) * Math.Sin(velocityHeading)),
            (float)(speed * Math.Sin(velocityPitch)));

#endif

        return (pos, quaternion, speed, velocity);
    }

    /// <summary>
    /// Continues reading the stream until a specific <paramref name="value"/> is reached.
    /// </summary>
    /// <returns>Enumeration of bytes.</returns>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="NotSupportedException">The stream does not support seeking.</exception>
    public IEnumerable<byte> ReadUntilUInt32(uint value)
    {
        while (PeekUInt32() != value)
        {
            yield return ReadByte();
        }
    }

    /// <summary>
    /// Continues reading the stream until facade (<c>0xFACADE01</c>) is reached.
    /// </summary>
    /// <returns>Enumeration of bytes.</returns>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="NotSupportedException">The stream does not support seeking.</exception>
    public IEnumerable<byte> ReadUntilFacade() => ReadUntilUInt32(0xFACADE01);

    /// <summary>
    /// Continues reading the stream until the end of the is reached. The stream can use either seeking or non-seeking method, depends what works best for you.
    /// </summary>
    /// <param name="seek">If the method should use seeking features.</param>
    /// <returns>Array of bytes.</returns>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="NotSupportedException">The stream does not support seeking.</exception>
    public byte[] ReadToEnd(bool seek = false)
    {
        if (seek)
        {
            return ReadBytes((int)(BaseStream.Length - BaseStream.Position));
        }

        using var ms = new MemoryStream();
        BaseStream.CopyTo(ms);
        return ms.ToArray();
    }

    /// <summary>
    /// Continues reading the stream until facade (<c>0xFACADE01</c>) is reached and converts the result to string.
    /// </summary>
    /// <returns>A <see cref="string"/>.</returns>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="NotSupportedException">The stream does not support seeking.</exception>
    public string ReadStringUntilFacade()
    {
        return Encoding.UTF8.GetString(ReadUntilFacade().ToArray());
    }

    /// <summary>
    /// Continues reading the stream until a specific chunk ID with the base <paramref name="classId"/> is reached, using remapping and masking.
    /// </summary>
    /// <returns>Enumeration of bytes.</returns>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    public IEnumerable<byte> ReadUntilNextChunk(uint classId)
    {
        classId |= 0x00000FFF;

        while (true)
        {
            var peekedUint = PeekUInt32();
            if ((Chunk.Remap(peekedUint) | 0x00000FFF) == classId)
                yield break;
            yield return ReadByte();
        }
    }

    /// <summary>
    /// Returns the next available <see cref="uint"/> but does not consume it.
    /// </summary>
    /// <returns>An <see cref="uint"/>.</returns>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="NotSupportedException">The stream does not support seeking.</exception>
    public uint PeekUInt32()
    {
        var result = ReadUInt32();
        BaseStream.Position -= sizeof(uint);
        return result;
    }

    /// <summary>
    /// Compares the <paramref name="magic"/> with the string to be read, using its length.
    /// </summary>
    /// <returns>If the <see cref="string"/> inside the stream is equal to <paramref name="magic"/>.</returns>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="magic"/> is null.</exception>
    public bool HasMagic(string magic)
    {
        if (magic is null)
            throw new ArgumentNullException(nameof(magic));

        return ReadString(Encoding.UTF8.GetByteCount(magic)) == magic;
    }

    public void StartIdSubState()
    {
        if (Settings.StateGuid is null)
        {
            throw new PropertyNullException(nameof(Settings.StateGuid));
        }

        Settings.IdSubStateGuid = StateManager.Shared.CreateIdSubState(Settings.StateGuid.Value);
    }

    public void EndIdSubState()
    {
        if (Settings.IdSubStateGuid is null)
        {
            return;
        }

        if (Settings.StateGuid is null)
        {
            throw new PropertyNullException(nameof(Settings.StateGuid));
        }

        StateManager.Shared.RemoveIdSubState(Settings.StateGuid.Value, Settings.IdSubStateGuid.Value);

        Settings.IdSubStateGuid = null;
    }

    /// <summary>
    /// A generic read method of parameterless types for the cost of performance loss. Prefer using the pre-defined data read methods.
    /// </summary>
    /// <typeparam name="T">Type of the variable to read. Supported types are <see cref="byte"/>, <see cref="short"/>, <see cref="int"/>,
    /// <see cref="long"/>, <see cref="float"/>, <see cref="bool"/>, <see cref="string"/>, <see cref="sbyte"/>, <see cref="ushort"/>,
    /// <see cref="uint"/>, <see cref="ulong"/>, <see cref="Byte3"/>, <see cref="Vec2"/>, <see cref="Vec3"/>,
    /// <see cref="Vec4"/>, <see cref="Int3"/>, and <see cref="Ident"/>.</typeparam>
    /// <returns>Object read from the stream.</returns>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    public T Read<T>() => typeof(T) switch
    {
        Type byteType   when byteType   == typeof(byte)     => (T)Convert.ChangeType(ReadByte(), typeof(T)),
        Type shortType  when shortType  == typeof(short)    => (T)Convert.ChangeType(ReadInt16(), typeof(T)),
        Type intType    when intType    == typeof(int)      => (T)Convert.ChangeType(ReadInt32(), typeof(T)),
        Type longType   when longType   == typeof(long)     => (T)Convert.ChangeType(ReadInt64(), typeof(T)),
        Type floatType  when floatType  == typeof(float)    => (T)Convert.ChangeType(ReadSingle(), typeof(T)),
        Type boolType   when boolType   == typeof(bool)     => (T)Convert.ChangeType(ReadBoolean(), typeof(T)),
        Type stringType when stringType == typeof(string)   => (T)Convert.ChangeType(ReadString(), typeof(T)),
        Type sbyteType  when sbyteType  == typeof(sbyte)    => (T)Convert.ChangeType(ReadSByte(), typeof(T)),
        Type ushortType when ushortType == typeof(ushort)   => (T)Convert.ChangeType(ReadUInt16(), typeof(T)),
        Type uintType   when uintType   == typeof(uint)     => (T)Convert.ChangeType(ReadUInt32(), typeof(T)),
        Type ulongType  when ulongType  == typeof(ulong)    => (T)Convert.ChangeType(ReadUInt64(), typeof(T)),
        Type byte3Type  when byte3Type  == typeof(Byte3)    => (T)Convert.ChangeType(ReadByte3(), typeof(T)),
        Type vec2Type   when vec2Type   == typeof(Vec2)     => (T)Convert.ChangeType(ReadVec2(), typeof(T)),
        Type vec3Type   when vec3Type   == typeof(Vec3)     => (T)Convert.ChangeType(ReadVec3(), typeof(T)),
        Type vec4Type   when vec4Type   == typeof(Vec4)     => (T)Convert.ChangeType(ReadVec4(), typeof(T)),
        Type int2Type   when int2Type   == typeof(Int2)     => (T)Convert.ChangeType(ReadInt2(), typeof(T)),
        Type int3Type   when int3Type   == typeof(Int3)     => (T)Convert.ChangeType(ReadInt3(), typeof(T)),
        Type identType  when identType  == typeof(Ident)    => (T)Convert.ChangeType(ReadIdent(), typeof(T)),

        _ => throw new NotSupportedException($"{typeof(T)} is not supported for Read<T>."),
    };

    protected override void Dispose(bool disposing)
    {
        EndIdSubState();

        base.Dispose(disposing);
    }
}
