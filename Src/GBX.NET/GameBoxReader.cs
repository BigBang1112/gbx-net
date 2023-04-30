using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;

namespace GBX.NET;

/// <summary>
/// Reads data types from GameBox serialization.
/// </summary>
public class GameBoxReader : BinaryReader
{
/* Later
 * 
 * #if NETSTANDARD2_1_OR_GREATER || NET6_0_OR_GREATER
    private string? previousString;
#endif*/

    public GameBox? Gbx { get; }

    /// <summary>
    /// A delegate collection that gets executed throughout the asynchronous reading.
    /// </summary>
    public GameBoxAsyncReadAction? AsyncAction { get; }

    internal ILogger? Logger { get; }

    public GbxState State { get; }

    private int? IdVersion { get => State.IdVersion; set => State.IdVersion = value; }
    private IList<string> IdStrings => State.IdStrings;
    private SortedDictionary<int, Node?> AuxNodes => State.AuxNodes;

    /// <summary>
    /// Constructs a binary reader specialized for Gbx deserializing.
    /// </summary>
    /// <param name="input">The input stream.</param>
    /// <param name="logger">Logger.</param>
    public GameBoxReader(Stream input, ILogger? logger = null) : this(input, default, default, logger, new())
    {
        
    }

    /// <summary>
    /// Constructs a binary reader specialized for Gbx deserializing.
    /// </summary>
    /// <param name="input">The input stream.</param>
    /// <param name="gbx">Gbx for reference table and sending its object to all the nodes.</param>
    /// <param name="asyncAction">Specialized executions during asynchronous reading.</param>
    /// <param name="logger">Logger.</param>
    /// <param name="state">State of <see cref="Id"/> and aux node write. Currently cannot be null.</param>
    internal GameBoxReader(Stream input, GameBox? gbx, GameBoxAsyncReadAction? asyncAction, ILogger? logger, GbxState state)
        : base(input, Encoding.UTF8, true)
    {        
        Gbx = gbx;
        AsyncAction = asyncAction;
        Logger = logger;
        State = state;
    }

    internal GameBoxReader(Stream input, GameBoxReader reference, bool encapsulated = false)
        : this(input, reference.Gbx, reference.AsyncAction, reference.Logger, encapsulated ? new(encapsulated) : reference.State)
    {

    }

    internal GameBoxReader(GameBoxReader reference, bool encapsulated = false) : this(reference.BaseStream, reference, encapsulated)
    {

    }

    /// <summary>
    /// Reads the next <see cref="int"/> from the current stream, casts it as <see cref="bool"/> and advances the current position of the stream by 4 bytes.
    /// </summary>
    /// <returns>A boolean.</returns>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="BooleanOutOfRangeException">Boolean is neither 0 or 1.</exception>
    public override bool ReadBoolean()
    {
        var booleanAsInt = ReadUInt32();

        if (GameBox.StrictBooleans && booleanAsInt > 1)
        {
            throw new BooleanOutOfRangeException(booleanAsInt);
        }

        return Convert.ToBoolean(booleanAsInt);
    }

    /// <summary>
    /// If <paramref name="asByte"/> is true, reads the next <see cref="byte"/> from the current stream and casts it as <see cref="bool"/>. Otherwise <see cref="ReadBoolean()"/> is called.
    /// </summary>
    /// <param name="asByte">Read the boolean as <see cref="byte"/> or <see cref="int"/>.</param>
    /// <returns>A boolean.</returns>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    public bool ReadBoolean(bool asByte)
    {
        if (!asByte)
        {
            return ReadBoolean();
        }

        var booleanAsByte = ReadByte();

        if (GameBox.StrictBooleans && booleanAsByte > 1)
        {
            throw new BooleanOutOfRangeException(booleanAsByte);
        }

        return booleanAsByte != 0;
    }

    /// <summary>
    /// Reads a <see cref="string"/> from the current stream with one of the prefix reading methods.
    /// </summary>
    /// <param name="readPrefix">The method to read the prefix.</param>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="ArgumentException"><paramref name="readPrefix"/> is <see cref="StringLengthPrefix.None"/>.</exception>
    /// <exception cref="StringLengthOutOfRangeException">String length is too big.</exception>
    /// <returns>The string being read.</returns>
    public string ReadString(StringLengthPrefix readPrefix)
    {
        var length = readPrefix switch
        {
            StringLengthPrefix.Byte => ReadByte(),
            StringLengthPrefix.Int32 => ReadInt32(),
            _ => throw new ArgumentException("Can't read string without knowing its length."),
        };

        return ReadString(length);
    }

    /// <summary>
    /// Reads a <see cref="string"/> from the current stream. The string is prefixed with the length, encoded as <see cref="int"/>.
    /// </summary>
    /// <returns>The string being read.</returns>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="StringLengthOutOfRangeException">String length is too big.</exception>
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
    /// <exception cref="LengthLimitException">String length is too big.</exception>
    public string ReadString(int length)
    {
        if (length == 0)
        {
            return "";
        }

        if (length < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(length), "Length cannot be negative.");
        }

        if (length > 0x10000000) // ~268MB
        {
            throw new LengthLimitException(length);
        }

#if NETSTANDARD2_1_OR_GREATER || NET6_0_OR_GREATER
        if (length > 1_500_000)
        {
            return Encoding.UTF8.GetString(ReadBytes(length));
        }

        Span<byte> bytes = stackalloc byte[length];

        if (Read(bytes) != length)
        {
            throw new EndOfStreamException();
        }

        /* Chars are different length than bytes
         * 
         * Span<char> chars = stackalloc char[length];

        _ = Encoding.UTF8.GetChars(bytes, chars);
        
        if (previousString is not null && MemoryExtensions.Equals(chars, previousString, StringComparison.Ordinal))
        {
            return previousString;
        }
        
        return previousString = chars.ToString(); */

        return Encoding.UTF8.GetString(bytes);
#else
        return Encoding.UTF8.GetString(ReadBytes(length));
#endif
    }

    /// <summary>
    /// First reads an <see cref="int"/> representing the length, then reads the sequence of bytes.
    /// </summary>
    /// <exception cref="ArgumentException">The number of decoded characters to read is greater than count. This can happen if a Unicode decoder returns fallback characters or a surrogate pair.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Length is negative.</exception>
    /// <returns>A byte array.</returns>
    public byte[] ReadBytes()
    {
        var length = ReadInt32();

        if (length > 0x10000000) // ~268MB
        {
            throw new LengthLimitException(length);
        }

        return ReadBytes(length);
    }

    /// <summary>
    /// First reads the <see cref="int"/> of the Id version, if not read yet, considering the information from state. Then reads an <see cref="int"/> (index) that holds the flags of the representing <see cref="string"/>. If the first 30 bits are 0, a fresh <see cref="string"/> is also read.
    /// </summary>
    /// <returns>A string.</returns>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="NotSupportedException">Gbx has the first Id presented without a version. Solution exists, but the stream does not support seeking.</exception>
    /// <exception cref="LengthLimitException">String length is negative.</exception>
    /// <exception cref="CorruptedIdException">The Id index is not matching any known values.</exception>
    public Id ReadId(bool cannotBeCollection = false)
    {
        if (IdVersion is null)
        {
            IdVersion = ReadInt32();

            if (IdVersion < 3)
            {
                throw new VersionNotSupportedException(IdVersion.Value);
            }

            // Edge-case scenario where Id doesn't have a version for whatever reason (can be multiple)
            if ((IdVersion & 0xC0000000) > 10)
            {
                Logger?.LogWarning("The detected Id version is {version}! Make sure this is correct.", IdVersion);

                IdVersion = 3;

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

        /*if ((index >> 16 & 0x1FF) == 0x1FF) // ????
        {
            var bytes = ReadBytes(5);
            var length = bytes[2];
            return ReadString(length);
        }*/

        if ((index & 0x3FFF) == 0 && (index >> 30 == 1 || index >> 30 == 2))
        {
            var str = ReadString();
            IdStrings.Add(str);
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

        if (index >> 30 == 0 && !cannotBeCollection)
        {
            return new Id((int)index);
        }

        if (cannotBeCollection)
        {
            return new Id(index.ToString());
        }

        if (IdStrings.Count > (index & 0x3FFF) - 1)
        {
            return IdStrings[(int)(index & 0x3FFF) - 1];
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
    /// <exception cref="NotSupportedException">GBX has the first Id presented without a version. Solution exists, but the stream does not support seeking.</exception>
    /// <exception cref="LengthLimitException">String length is negative.</exception>
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
    /// <exception cref="LengthLimitException">String length is negative.</exception>
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

    public Iso4 ReadIso4()
    {
        return new Iso4(XX: ReadSingle(),
                        XY: ReadSingle(),
                        XZ: ReadSingle(),
                        YX: ReadSingle(),
                        YY: ReadSingle(),
                        YZ: ReadSingle(),
                        ZX: ReadSingle(),
                        ZY: ReadSingle(),
                        ZZ: ReadSingle(),
                        TX: ReadSingle(),
                        TY: ReadSingle(),
                        TZ: ReadSingle());
    }

    public Mat3 ReadMat3()
    {
        return new Mat3(XX: ReadSingle(),
                        XY: ReadSingle(),
                        XZ: ReadSingle(),
                        YX: ReadSingle(),
                        YY: ReadSingle(),
                        YZ: ReadSingle(),
                        ZX: ReadSingle(),
                        ZY: ReadSingle(),
                        ZZ: ReadSingle());
    }

    public Mat4 ReadMat4()
    {
        return new Mat4(XX: ReadSingle(),
                        XY: ReadSingle(),
                        XZ: ReadSingle(),
                        XW: ReadSingle(),
                        YX: ReadSingle(),
                        YY: ReadSingle(),
                        YZ: ReadSingle(),
                        YW: ReadSingle(),
                        ZX: ReadSingle(),
                        ZY: ReadSingle(),
                        ZZ: ReadSingle(),
                        ZW: ReadSingle(),
                        WX: ReadSingle(),
                        WY: ReadSingle(),
                        WZ: ReadSingle(),
                        WW: ReadSingle());
    }

    public TimeSpan? ReadTimeOfDay()
    {
        var dayTime = ReadUInt32();

        if (dayTime == uint.MaxValue)
        {
            return null;
        }

        if (dayTime > ushort.MaxValue)
        {
            throw new InvalidDataException("Day time is over 65535");
        }

        var maxTime = TimeSpan.FromDays(1) - TimeSpan.FromSeconds(1);
        var maxSecs = maxTime.TotalSeconds;

        return TimeSpan.FromSeconds(Convert.ToInt32(dayTime / (float)ushort.MaxValue * maxSecs));
    }

    /// <summary>
    /// Reads a common transform representation.
    /// </summary>
    /// <returns>3D position, quaternion rotation, speed, vector velocity.</returns>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    [Obsolete]
    public (Vec3 position, Quat rotation, float speed, Vec3 velocity) ReadTransform()
    {
        var pos = ReadVec3();

#if NETSTANDARD2_1_OR_GREATER || NET5_0_OR_GREATER

        var angle = ReadUInt16() * MathF.PI / ushort.MaxValue;
        var axisHeading = ReadInt16() * MathF.PI / short.MaxValue;
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

    public int ReadOptimizedInt(int determineFrom) => (uint)determineFrom switch
    {
        >= ushort.MaxValue => ReadInt32(),
        >= byte.MaxValue => ReadUInt16(),
        _ => ReadByte()
    };

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
        Type byteType when byteType == typeof(byte) => (T)Convert.ChangeType(ReadByte(), typeof(T)),
        Type shortType when shortType == typeof(short) => (T)Convert.ChangeType(ReadInt16(), typeof(T)),
        Type intType when intType == typeof(int) => (T)Convert.ChangeType(ReadInt32(), typeof(T)),
        Type longType when longType == typeof(long) => (T)Convert.ChangeType(ReadInt64(), typeof(T)),
        Type floatType when floatType == typeof(float) => (T)Convert.ChangeType(ReadSingle(), typeof(T)),
        Type boolType when boolType == typeof(bool) => (T)Convert.ChangeType(ReadBoolean(), typeof(T)),
        Type stringType when stringType == typeof(string) => (T)Convert.ChangeType(ReadString(), typeof(T)),
        Type sbyteType when sbyteType == typeof(sbyte) => (T)Convert.ChangeType(ReadSByte(), typeof(T)),
        Type ushortType when ushortType == typeof(ushort) => (T)Convert.ChangeType(ReadUInt16(), typeof(T)),
        Type uintType when uintType == typeof(uint) => (T)Convert.ChangeType(ReadUInt32(), typeof(T)),
        Type ulongType when ulongType == typeof(ulong) => (T)Convert.ChangeType(ReadUInt64(), typeof(T)),
        Type byte3Type when byte3Type == typeof(Byte3) => (T)Convert.ChangeType(ReadByte3(), typeof(T)),
        Type vec2Type when vec2Type == typeof(Vec2) => (T)Convert.ChangeType(ReadVec2(), typeof(T)),
        Type vec3Type when vec3Type == typeof(Vec3) => (T)Convert.ChangeType(ReadVec3(), typeof(T)),
        Type vec4Type when vec4Type == typeof(Vec4) => (T)Convert.ChangeType(ReadVec4(), typeof(T)),
        Type int2Type when int2Type == typeof(Int2) => (T)Convert.ChangeType(ReadInt2(), typeof(T)),
        Type int3Type when int3Type == typeof(Int3) => (T)Convert.ChangeType(ReadInt3(), typeof(T)),
        Type identType when identType == typeof(Ident) => (T)Convert.ChangeType(ReadIdent(), typeof(T)),

        _ => throw new NotSupportedException($"{typeof(T)} is not supported for Read<T>."),
    };

    /// <summary>
    /// Reads an <see cref="int"/> containing the node reference index, then the node using the <see cref="Node"/>'s Parse method. The index is also checked if it isn't a part of a reference table, which currently returns null.
    /// </summary>
    /// <param name="nodeRefFile">File reference to the node (if from reference table).</param>
    /// <returns>A node, or null if the index is -1 or the node is from reference table.</returns>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    public Node? ReadNodeRef(out GameBoxRefTable.File? nodeRefFile)
    {
        if (State.Encapsulated)
        {
            nodeRefFile = null;
            return ReadNode();
        }

        var index = ReadInt32() - 1; // GBX seems to start the index at 1

        // If aux node index is below 0 or the node index is part of the reference table
        if (index < 0)
        {
            nodeRefFile = null;
            return null;
        }

        if (TryGetRefTableNode(index, out nodeRefFile))
        {
            AuxNodes[index] = null;
            return null;
        }

        var node = default(Node?);

        if (NodeShouldBeParsed(index))
        {
            node = Node.Parse(this, classId: null, progress: null);
        }

        TryGetNodeIfNullOrAssignExistingNode(index, ref node);

        if (node is null)
        {
            // Sometimes it indexes the node reference that is further in the expected indexes
            // So it grabs the last one instead, needs to be further tested
            return AuxNodes.Values.Last();
        }

        return node;
    }

    /// <summary>
    /// Reads an <see cref="int"/> containing the node reference index, then the node using the <see cref="Node"/>'s Parse method. The index is also checked if it isn't a part of a reference table, which currently returns null.
    /// </summary>
    /// <returns>A node, or null if the index is -1 or the node is from reference table.</returns>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    public Node? ReadNodeRef()
    {
        var node = ReadNodeRef(out GameBoxRefTable.File? nodeRefFile);

        if (Logger is not null && nodeRefFile is not null)
        {
            Logger.LogDiscardedExternalNode(nodeRefFile);
        }

        return node;
    }

    /// <summary>
    /// Reads an <see cref="int"/> containing the node reference index, then the node using the <see cref="Node"/>'s Parse method. The index is also checked if it isn't a part of a reference table, which currently returns null.
    /// </summary>
    /// <typeparam name="T">Type of node.</typeparam>
    /// <param name="nodeRefFile">File reference to the node (if from reference table).</param>
    /// <returns>A node, or null if the index is -1 or the node is from reference table.</returns>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    public T? ReadNodeRef<T>(out GameBoxRefTable.File? nodeRefFile) where T : Node
    {
        return ReadNodeRef(out nodeRefFile) as T;
    }

    /// <summary>
    /// Reads an <see cref="int"/> containing the node reference index, then the node using the <see cref="Node"/>'s Parse method. The index is also checked if it isn't a part of a reference table, which currently returns null.
    /// </summary>
    /// <typeparam name="T">Type of node.</typeparam>
    /// <returns>A node, or null if the index is -1 or the node is from reference table.</returns>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    public T? ReadNodeRef<T>() where T : Node
    {
        var node = ReadNodeRef(out GameBoxRefTable.File? nodeRefFile);

        var nodeT = node as T;

        if (Logger is not null && node != nodeT)
        {
            Logger.LogWarning("Discarded node! Check your chunk code!");
        }

        if (nodeRefFile is not null)
        {
            Logger?.LogDiscardedExternalNode(nodeRefFile);
        }

        return nodeT;
    }

    /// <summary>
    /// Reads an <see cref="int"/> containing the node reference index, then the node using the <see cref="Node"/>'s ParseAsync method. The index is also checked if it isn't a part of a reference table, which currently returns null.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A node, or null if the index is -1 or the node is from reference table.</returns>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    public async Task<Node?> ReadNodeRefAsync(CancellationToken cancellationToken = default)
    {
        var index = ReadInt32() - 1; // GBX seems to start the index at 1

        if (index < 0)
        {
            return null;
        }

        // If the node index is part of the reference table
        if (TryGetRefTableNode(index, out GameBoxRefTable.File? nodeRefFile))
        {
            Logger?.LogDiscardedExternalNode(nodeRefFile!);

            AuxNodes[index] = null;
            return null;
        }

        var node = default(Node?);

        if (NodeShouldBeParsed(index))
        {
            node = (await Node.ParseAsync(this, classId: null, cancellationToken)) ?? throw new ThisShouldNotHappenException();
        }

        TryGetNodeIfNullOrAssignExistingNode(index, ref node);

        if (node is null)
        {
            // Sometimes it indexes the node reference that is further in the expected indexes
            // So it grabs the last one instead, needs to be further tested
            return AuxNodes.Values.Last();
        }

        return node;
    }

    /// <summary>
    /// Reads an <see cref="int"/> containing the node reference index, then the node using the <see cref="Node"/>'s ParseAsync method. The index is also checked if it isn't a part of a reference table, which currently returns null.
    /// </summary>
    /// <typeparam name="T">Type of node.</typeparam>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A node, or null if the index is -1 or the node is from reference table.</returns>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    public async Task<T?> ReadNodeRefAsync<T>(CancellationToken cancellationToken = default) where T : Node
    {
        var node = await ReadNodeRefAsync(cancellationToken);

        var nodeT = node as T;

        if (Logger is not null && node != nodeT)
        {
            Logger.LogWarning("Discarded node! Check your chunk code!");
        }

        return nodeT;
    }

    private bool NodeShouldBeParsed(int index)
    {
        var containsNodeIndex = AuxNodes.ContainsKey(index);
        _ = AuxNodes.TryGetValue(index, out Node? node);

        // If index is 0 or bigger and the node wasn't read yet, or is null
        return index >= 0 && (!containsNodeIndex || node is null);
    }

    private void TryGetNodeIfNullOrAssignExistingNode(int index, ref Node? node)
    {
        if (node is null)
        {
            AuxNodes.TryGetValue(index, out node); // Tries to get the available node from index
        }
        else
        {
            AuxNodes[index] = node;
        }
    }

#if NET6_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    private bool TryGetRefTableNode(int index, [NotNullWhen(true)] out GameBoxRefTable.File? nodeRefFile)
#else
    private bool TryGetRefTableNode(int index, out GameBoxRefTable.File? nodeRefFile)
#endif
    {
        var refTable = Gbx?.RefTable;

        // First checks if reference table is used
        if (refTable is null || refTable.Files.Count <= 0 && refTable.Folders.Count <= 0)
        {
            // Reference table isn't used so it's a nested object
            nodeRefFile = null;
            return false;
        }

        var allFiles = refTable.Files; // Returns available external references

        if (allFiles.Count == 0) // If there's none
        {
            nodeRefFile = null;
            return false;
        }

        // Tries to get the one with this node index
        nodeRefFile = allFiles.FirstOrDefault(x => x.NodeIndex == index);

        return nodeRefFile is not null;
    }
    
    public Node? ReadNode(uint? expectedClassId = null)
    {
        return Node.Parse(this, expectedClassId, progress: null);
    }

    public T? ReadNode<T>(uint? expectedClassId = null) where T : Node
    {
        var node = ReadNode(expectedClassId);

        var nodeT = node as T;

        if (Logger is not null && node != nodeT)
        {
            Logger.LogWarning("Discarded node! Check your chunk code!");
        }

        return nodeT;
    }

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
    /// <exception cref="ArgumentOutOfRangeException">Length is not valid.</exception>
    public T[] ReadArray<T>(int length, bool lengthInBytes = false) where T : struct
    {
        if (length == 0)
        {
            return Array.Empty<T>();
        }

        if (length < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(length), "Length is not valid.");
        }

        var l = length * (lengthInBytes ? 1 : TypeSize<T>.Size);

        if (l < 0 || l > 0x10000000) // ~268MB
        {
            throw new Exception($"Length is too big to handle ({(l < 0 ? length : l)}).");
        }

        if (l > 1_500_000)
        {
            return MemoryMarshal.Cast<byte, T>(ReadBytes(l)).ToArray();
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

        if (length < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(length), "Length is negative.");
        }

        if (length > 0x10000000) // ~268MB
        {
            throw new LengthLimitException(length);
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

        if (length == 0)
        {
            return Array.Empty<T>();
        }

        if (length < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(length), "Length is negative.");
        }

        if (length > 0x10000000) // ~268MB
        {
            throw new LengthLimitException(length);
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

        if (length == 0)
        {
            return Array.Empty<T>();
        }

        if (length < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(length));
        }

        if (length > 0x10000000) // ~268MB
        {
            throw new LengthLimitException(length);
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

        if (length < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(length));
        }

        if (length > 0x10000000) // ~268MB
        {
            throw new LengthLimitException(length);
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

        if (length < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(length));
        }

        if (length > 0x10000000) // ~268MB
        {
            throw new LengthLimitException(length);
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

    /// <summary>
    /// Does a for loop with <paramref name="length"/> parameter, each element requiring to return an instance of <typeparamref name="T"/>. Instead of array allocation, elements are yielded one by one.
    /// </summary>
    /// <typeparam name="T">Type of the enumerable.</typeparam>
    /// <param name="length">Length of the enumerable.</param>
    /// <param name="forLoop">Each element.</param>
    /// <returns>Enumerable of <typeparamref name="T"/>.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="forLoop"/> is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="length"/> is negative.</exception>
    public static IEnumerable<T> ReadEnumerable<T>(int length, Func<T> forLoop)
    {
        if (forLoop is null)
        {
            throw new ArgumentNullException(nameof(forLoop));
        }

        // In theory it doesn't have to be there, but it ensures that the parse can crash as soon as something weird happens
        if (length < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(length));
        }

        for (var i = 0; i < length; i++)
        {
            yield return forLoop.Invoke();
        }
    }

    /// <summary>
    /// First reads an <see cref="int"/> representing the length, then does a for loop with this length, each element requiring to return an instance of <typeparamref name="T"/>. Instead of array allocation, elements are yielded one by one.
    /// </summary>
    /// <typeparam name="T">Type of the enumerable.</typeparam>
    /// <param name="forLoop">Each element.</param>
    /// <returns>Enumerable of <typeparamref name="T"/>.</returns>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="forLoop"/> is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Length is negative.</exception>
    public IEnumerable<T> ReadEnumerable<T>(Func<T> forLoop)
    {
        if (forLoop is null)
        {
            throw new ArgumentNullException(nameof(forLoop));
        }

        foreach (var x in ReadEnumerable(length: ReadInt32(), forLoop))
        {
            yield return x;
        }
    }

    /// <summary>
    /// Does a for loop with <paramref name="length"/> parameter, each element requiring to return an instance of <typeparamref name="T"/>. Instead of array allocation, elements are yielded one by one.
    /// </summary>
    /// <typeparam name="T">Type of the enumerable.</typeparam>
    /// <param name="length">Length of the enumerable.</param>
    /// <param name="forLoop">Each element with this reader (to avoid closures).</param>
    /// <returns>Enumerable of <typeparamref name="T"/>.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="forLoop"/> is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="length"/> is negative.</exception>
    public IEnumerable<T> ReadEnumerable<T>(int length, Func<GameBoxReader, T> forLoop)
    {
        if (forLoop is null)
        {
            throw new ArgumentNullException(nameof(forLoop));
        }

        // In theory it doesn't have to be there, but it ensures that the parse can crash as soon as something weird happens
        if (length < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(length));
        }

        if (length > 0x10000000) // ~268MB
        {
            throw new LengthLimitException(length);
        }

        for (var i = 0; i < length; i++)
        {
            yield return forLoop.Invoke(this);
        }
    }

    /// <summary>
    /// First reads an <see cref="int"/> representing the length, then does a for loop with this length, each element requiring to return an instance of <typeparamref name="T"/>. Instead of array allocation, elements are yielded one by one.
    /// </summary>
    /// <typeparam name="T">Type of the enumerable.</typeparam>
    /// <param name="forLoop">Each element with this reader (to avoid closures).</param>
    /// <returns>Enumerable of <typeparamref name="T"/>.</returns>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="forLoop"/> is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Length is negative.</exception>
    public IEnumerable<T> ReadEnumerable<T>(Func<GameBoxReader, T> forLoop)
    {
        if (forLoop is null)
        {
            throw new ArgumentNullException(nameof(forLoop));
        }

        foreach (var x in ReadEnumerable(length: ReadInt32(), forLoop))
        {
            yield return x;
        }
    }

    /// <summary>
    /// Does a for loop with <paramref name="length"/> parameter, each element requiring to return an instance of <typeparamref name="T"/>. Instead of array allocation, elements are yielded one by one.
    /// </summary>
    /// <typeparam name="T">Type of the enumerable.</typeparam>
    /// <param name="length">Length of the enumerable.</param>
    /// <param name="forLoop">Each element with an index parameter.</param>
    /// <returns>Enumerable of <typeparamref name="T"/>.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="forLoop"/> is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="length"/> is negative.</exception>
    public static IEnumerable<T> ReadEnumerable<T>(int length, Func<int, T> forLoop)
    {
        if (forLoop is null)
        {
            throw new ArgumentNullException(nameof(forLoop));
        }

        // In theory it doesn't have to be there, but it ensures that the parse can crash as soon as something weird happens
        if (length < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(length));
        }

        if (length > 0x10000000) // ~268MB
        {
            throw new LengthLimitException(length);
        }

        for (var i = 0; i < length; i++)
        {
            yield return forLoop.Invoke(i);
        }
    }

    /// <summary>
    /// First reads an <see cref="int"/> representing the length, then does a for loop with this length, each element requiring to return an instance of <typeparamref name="T"/>. Instead of array allocation, elements are yielded one by one.
    /// </summary>
    /// <typeparam name="T">Type of the enumerable.</typeparam>
    /// <param name="forLoop">Each element with an index parameter.</param>
    /// <returns>Enumerable of <typeparamref name="T"/>.</returns>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="forLoop"/> is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Length is negative.</exception>
    public IEnumerable<T> ReadEnumerable<T>(Func<int, T> forLoop)
    {
        if (forLoop is null)
        {
            throw new ArgumentNullException(nameof(forLoop));
        }

        foreach (var x in ReadEnumerable(length: ReadInt32(), forLoop))
        {
            yield return x;
        }
    }

    /// <summary>
    /// Does a for loop with <paramref name="length"/> parameter, each element requiring to return an instance of <typeparamref name="T"/>. Instead of array allocation, elements are yielded one by one.
    /// </summary>
    /// <typeparam name="T">Type of the enumerable.</typeparam>
    /// <param name="length">Length of the enumerable.</param>
    /// <param name="forLoop">Each element with an index parameter and this reader (to avoid closures).</param>
    /// <returns>Enumerable of <typeparamref name="T"/>.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="forLoop"/> is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Length is negative.</exception>
    public IEnumerable<T> ReadEnumerable<T>(int length, Func<int, GameBoxReader, T> forLoop)
    {
        if (forLoop is null)
        {
            throw new ArgumentNullException(nameof(forLoop));
        }

        // In theory it doesn't have to be there, but it ensures that the parse can crash as soon as something weird happens
        if (length < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(length));
        }

        if (length > 0x10000000) // ~268MB
        {
            throw new LengthLimitException(length);
        }

        for (var i = 0; i < length; i++)
        {
            yield return forLoop.Invoke(i, this);
        }
    }

    /// <summary>
    /// First reads an <see cref="int"/> representing the length, then does a for loop with this length, each element requiring to return an instance of <typeparamref name="T"/>. Instead of array allocation, elements are yielded one by one.
    /// </summary>
    /// <typeparam name="T">Type of the enumerable.</typeparam>
    /// <param name="forLoop">Each element with an index parameter and this reader (to avoid closures).</param>
    /// <returns>Enumerable of <typeparamref name="T"/>.</returns>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="forLoop"/> is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Length is negative.</exception>
    public IEnumerable<T> ReadEnumerable<T>(Func<int, GameBoxReader, T> forLoop)
    {
        if (forLoop is null)
        {
            throw new ArgumentNullException(nameof(forLoop));
        }

        foreach (var x in ReadEnumerable(length: ReadInt32(), forLoop))
        {
            yield return x;
        }
    }

#if NET6_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER

    /// <summary>
    /// Does a for loop with <paramref name="length"/> parameter, each element requiring to return an instance of <typeparamref name="T"/>. Instead of array allocation, elements are yielded one by one.
    /// </summary>
    /// <typeparam name="T">Type of the enumerable.</typeparam>
    /// <param name="length">Length of the enumerable.</param>
    /// <param name="forLoop">Each element with this reader (to avoid closures).</param>
    /// <returns>Enumerable of <typeparamref name="T"/>.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="forLoop"/> is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="length"/> is negative.</exception>
    public async IAsyncEnumerable<T> ReadEnumerableAsync<T>(int length, Func<GameBoxReader, Task<T>> forLoop)
    {
        if (forLoop is null)
        {
            throw new ArgumentNullException(nameof(forLoop));
        }

        // In theory it doesn't have to be there, but it ensures that the parse can crash as soon as something weird happens
        if (length < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(length));
        }

        if (length > 0x10000000) // ~268MB
        {
            throw new LengthLimitException(length);
        }

        for (var i = 0; i < length; i++)
        {
            yield return await forLoop.Invoke(this);
        }
    }

    /// <summary>
    /// First reads an <see cref="int"/> representing the length, then does a for loop with this length, each element requiring to return an instance of <typeparamref name="T"/>. Instead of array allocation, elements are yielded one by one.
    /// </summary>
    /// <typeparam name="T">Type of the enumerable.</typeparam>
    /// <param name="forLoop">Each element with this reader (to avoid closures).</param>
    /// <returns>Enumerable of <typeparamref name="T"/>.</returns>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="forLoop"/> is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Length is negative.</exception>
    public async IAsyncEnumerable<T> ReadEnumerableAsync<T>(Func<GameBoxReader, Task<T>> forLoop)
    {
        if (forLoop is null)
        {
            throw new ArgumentNullException(nameof(forLoop));
        }

        await foreach (var x in ReadEnumerableAsync(length: ReadInt32(), forLoop))
        {
            yield return x;
        }
    }

#endif

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

    /// <summary>
    /// Does a for loop with <paramref name="length"/> parameter, each element requiring to return an instance of <typeparamref name="T"/>. A list is allocated and elements are added via enumeration.
    /// </summary>
    /// <typeparam name="T">Type of the list.</typeparam>
    /// <param name="length">Length of the list.</param>
    /// <param name="forLoop">Each element with this reader (to avoid closures).</param>
    /// <returns>List of <typeparamref name="T"/>.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="forLoop"/> is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="length"/> is negative.</exception>
    public async Task<IList<T>> ReadListAsync<T>(int length, Func<GameBoxReader, Task<T>> forLoop)
    {
#if NET6_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
        return await ReadEnumerableAsync(length, forLoop).ToListAsync(length);
#else
        if (forLoop is null)
        {
            throw new ArgumentNullException(nameof(forLoop));
        }

        if (length > 0x10000000) // ~268MB
        {
            throw new LengthLimitException(length);
        }

        var list = new List<T>(length);

        for (var i = 0; i < length; i++)
        {
            list.Add(await forLoop(this));
        }

        return list;
#endif
    }

    public async Task<IList<T>?> ReadListAsync<T>(Func<GameBoxReader, Task<T>> forLoop)
    {
        return await ReadListAsync(length: ReadInt32(), forLoop);
    }

    /// <summary>
    /// Reads values in a dictionary kind (first key, then value). For node dictionaries, use the <see cref="ReadDictionaryNode{TKey, TValue}"/> method for better performance.
    /// </summary>
    /// <typeparam name="TKey">One of the supported types of <see cref="Read{T}"/>. Mustn't be null.</typeparam>
    /// <typeparam name="TValue">One of the supported types of <see cref="Read{T}"/>.</typeparam>
    /// <param name="overrideKey">If the pair in the dictionary should be overriden by the new value when a duplicate key is found. It is recommended to keep it false to easily spot errors.</param>
    /// <returns>A dictionary.</returns>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="ArgumentException">An element with the same key already exists in the dictionary.</exception>
    public IDictionary<TKey, TValue> ReadDictionary<TKey, TValue>(bool overrideKey = false) where TKey : notnull
    {
        var length = ReadInt32();

        if (length > 0x10000000) // ~268MB
        {
            throw new LengthLimitException(length);
        }

        var dictionary = new Dictionary<TKey, TValue>(length);

        for (var i = 0; i < length; i++)
        {
            var key = Read<TKey>();
            var value = Read<TValue>();

            if (overrideKey)
            {
                dictionary[key] = value;
            }
            else
            {
                dictionary.Add(key, value);
            }
        }

        return dictionary;
    }

    /// <summary>
    /// Reads nodes in a dictionary kind (first key, then value).
    /// </summary>
    /// <typeparam name="TKey">Type of the key. Mustn't be nullable.</typeparam>
    /// <typeparam name="TValue">A node that is presented as node reference.</typeparam>
    /// <param name="overrideKey">If the pair in the dictionary should be overriden by the new value when a duplicate key is found. It is recommended to keep it false to easily spot errors.</param>
    /// <param name="keyReader">An optional way to read the key. Default is <see cref="Read{T}"/></param>
    /// <returns>A dictionary.</returns>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="ArgumentException">An element with the same key already exists in the dictionary.</exception>
    public IDictionary<TKey, TValue?> ReadDictionaryNode<TKey, TValue>(
        bool overrideKey = false,
        Func<GameBoxReader, TKey>? keyReader = null)
        where TKey : notnull where TValue : Node
    {
        var length = ReadInt32();

        if (length > 0x10000000) // ~268MB
        {
            throw new LengthLimitException(length);
        }

        var dictionary = new Dictionary<TKey, TValue?>(length);

        for (var i = 0; i < length; i++)
        {
            var key = keyReader is null ? Read<TKey>() : keyReader(this);
            var value = ReadNodeRef<TValue>();

            AddOrOverrideKey(dictionary, key, value, overrideKey);
        }

        return dictionary;
    }

    public async Task<IDictionary<TKey, TValue?>?> ReadDictionaryNodeAsync<TKey, TValue>(
        bool overrideKey = false,
        Func<GameBoxReader, TKey>? keyReader = null,
        CancellationToken cancellationToken = default)
        where TKey : notnull where TValue : Node
    {
        var length = ReadInt32();

        if (length > 0x10000000) // ~268MB
        {
            throw new LengthLimitException(length);
        }

        var dictionary = new Dictionary<TKey, TValue?>(length);

        for (var i = 0; i < length; i++)
        {
            var key = keyReader is null ? Read<TKey>() : keyReader(this);
            var value = await ReadNodeRefAsync<TValue>(cancellationToken);

            AddOrOverrideKey(dictionary, key, value, overrideKey);
        }

        return dictionary;
    }

    private static void AddOrOverrideKey<TKey, TValue>(Dictionary<TKey, TValue?> dictionary,
                                                       TKey key,
                                                       TValue? value,
                                                       bool overrideKey)
                                                       where TKey : notnull
                                                       where TValue : Node
    {
        if (overrideKey)
        {
            dictionary[key] = value;
        }
        else
        {
            dictionary.Add(key, value);
        }
    }

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
    [Obsolete("Prefer using ReadArray, turning the Span into an array will double the memory allocation.")]
    public Span<T> ReadSpan<T>(int length, bool lengthInBytes = false) where T : struct
    {
        var l = length * (lengthInBytes ? 1 : Marshal.SizeOf<T>());

        if (length > 0x10000000) // ~268MB
        {
            throw new LengthLimitException(length);
        }

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
    [Obsolete("Prefer using ReadArray, turning the Span into an array will double the memory allocation.")]
    public Span<T> ReadSpan<T>(bool lengthInBytes = false) where T : struct
    {
        return ReadSpan<T>(length: ReadInt32(), lengthInBytes);
    }

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
        if (totalMilliseconds == -1) return null;
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

    public ExternalNode<T>[] ReadExternalNodeArray<T>(int length) where T : Node
    {
        return ReadArray(length, r =>
        {
            var node = r.ReadNodeRef(out GameBoxRefTable.File? file);
            return new ExternalNode<T>(node as T, file);
        });
    }

    public ExternalNode<T>[] ReadExternalNodeArray<T>() where T : Node
    {
        return ReadExternalNodeArray<T>(length: ReadInt32());
    }

    public DateTime ReadFileTime()
    {
        return DateTime.FromFileTimeUtc(ReadInt64());
    }

    public int ReadSmallLen()
    {
        var firstByte = ReadByte();
        var secondUInt16 = default(ushort);

        if (firstByte > 127)
        {
            secondUInt16 = ReadUInt16();
        }
        
        return firstByte & 127 | secondUInt16 << 7;
    }

    public string ReadSmallString()
    {
        return ReadString(ReadSmallLen());
    }

    /// <summary>
    /// Reads a 2-byte <see cref="Vec3"/>.
    /// </summary>
    /// <returns></returns>
    public Vec3 ReadVec3Unit2()
    {
#if NETSTANDARD2_1_OR_GREATER || NET5_0_OR_GREATER
        var axisHeading = ReadByte() * MathF.PI / sbyte.MaxValue;
        var axisPitch = ReadByte() * (MathF.PI / 2) / sbyte.MaxValue;

        return new Vec3(MathF.Cos(axisHeading) * MathF.Cos(axisPitch), MathF.Sin(axisHeading) * MathF.Cos(axisPitch), MathF.Sin(axisPitch));
#else
        var axisHeading = ReadByte() * Math.PI / sbyte.MaxValue;
        var axisPitch = ReadByte() * (Math.PI / 2) / sbyte.MaxValue;

        return new Vec3((float)(Math.Cos(axisHeading) * Math.Cos(axisPitch)), (float)(Math.Sin(axisHeading) * Math.Cos(axisPitch)), (float)Math.Sin(axisPitch));
#endif
    }

    /// <summary>
    /// Reads a 4-byte <see cref="Vec3"/>.
    /// </summary>
    /// <returns></returns>
    public Vec3 ReadVec3Unit4()
    {
#if NETSTANDARD2_1_OR_GREATER || NET5_0_OR_GREATER
        var axisHeading = ReadInt16() * MathF.PI / short.MaxValue;
        var axisPitch = ReadInt16() * (MathF.PI / 2) / short.MaxValue;

        return new Vec3(MathF.Cos(axisHeading) * MathF.Cos(axisPitch), MathF.Sin(axisHeading) * MathF.Cos(axisPitch), MathF.Sin(axisPitch));
#else
        var axisHeading = ReadInt16() * Math.PI / short.MaxValue;
        var axisPitch = ReadInt16() * (Math.PI / 2) / short.MaxValue;

        return new Vec3((float)(Math.Cos(axisHeading) * Math.Cos(axisPitch)), (float)(Math.Sin(axisHeading) * Math.Cos(axisPitch)), (float)Math.Sin(axisPitch));
#endif
    }

    /// <summary>
    /// Reads a 6-byte <see cref="Quat"/>.
    /// </summary>
    /// <returns></returns>
    public Quat ReadQuat6()
    {
#if NETSTANDARD2_1_OR_GREATER || NET5_0_OR_GREATER
        var angle = ReadUInt16() * MathF.PI / ushort.MaxValue;
        var axis = ReadVec3Unit4();
        
        return new Quat(axis * MathF.Sin(angle), MathF.Cos(angle));
#else
        var angle = ReadUInt16() * Math.PI / ushort.MaxValue;
        var axis = ReadVec3Unit4();

        return new Quat(axis * (float)Math.Sin(angle), (float)Math.Cos(angle));
#endif
    }

    public Vec3 ReadVec3_4()
    {
        var mag16 = ReadInt16();

        var mag = mag16 == short.MinValue
            ? 0
#if NETSTANDARD2_1_OR_GREATER || NET5_0_OR_GREATER
            : MathF.Exp(mag16 / 1000f);
#else
            : (float)Math.Exp(mag16 / 1000.0);
#endif

        return ReadVec3Unit2() * mag;
    }

    public Vec3 ReadVec3_10b()
    {
        var val = ReadInt32();

        return new Vec3(
            (val & 0x3FF) / (float)0x1FF,
            ((val >> 10) & 0x3FF) / (float)0x1FF,
            ((val >> 20) & 0x3FF) / (float)0x1FF);
    }
}
