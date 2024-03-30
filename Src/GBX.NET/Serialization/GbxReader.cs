using GBX.NET.Components;
using GBX.NET.Managers;
using Microsoft.Extensions.Logging;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
using System.Xml;

namespace GBX.NET.Serialization;

/// <summary>
/// A binary/text reader specialized for Gbx.
/// </summary>
public partial interface IGbxReader : IDisposable
{
    Stream BaseStream { get; }
    SerializationMode Mode { get; }
    GbxFormat Format { get; }

    bool ReadGbxMagic();

    byte ReadByte();
    sbyte ReadSByte();
    short ReadInt16();
    ushort ReadUInt16();
    int ReadInt32();
    uint ReadUInt32();
    long ReadInt64();
    ulong ReadUInt64();
    float ReadSingle();
    void Close();

    int ReadHexInt32();
    uint ReadHexUInt32();
    BigInteger ReadBigInt(int byteLength);
    Int128 ReadInt128();
    UInt128 ReadUInt128();
    UInt256 ReadUInt256();
    Int2 ReadInt2();
    Int3 ReadInt3();
    Int4 ReadInt4();
    Byte3 ReadByte3();
    Vec2 ReadVec2();
    Vec3 ReadVec3();
    Vec4 ReadVec4();
    BoxAligned ReadBoxAligned();
    BoxInt3 ReadBoxInt3();
    Color ReadColor();
    Iso4 ReadIso4();
    Mat3 ReadMat3();
    Mat4 ReadMat4();
    Quat ReadQuat();
    Rect ReadRect();
    bool ReadBoolean();
    bool ReadBoolean(bool asByte);
    byte[] ReadData();
    Task<byte[]> ReadDataAsync(CancellationToken cancellationToken = default);
    byte[] ReadData(int length);
    Task<byte[]> ReadDataAsync(int length, CancellationToken cancellationToken = default);
    byte[] ReadBytes(int count);
    Task<byte[]> ReadBytesAsync(int count, CancellationToken cancellationToken = default);
    string ReadString();
    string ReadString(int length);
    string ReadString(StringLengthPrefix readPrefix);
    string ReadIdAsString();
    Id ReadId();
    Ident ReadIdent();
    PackDesc ReadPackDesc();
    T? ReadNodeRef<T>() where T : IClass;
    T? ReadNodeRef<T>(out GbxRefTableFile? file) where T : IClass;
    T? ReadNode<T>() where T : IClass, new();
    TimeInt32 ReadTimeInt32();
    TimeInt32? ReadTimeInt32Nullable();
    TimeSingle ReadTimeSingle();
    TimeSingle? ReadTimeSingleNullable();
    TimeSpan? ReadTimeOfDay();
    DateTime ReadFileTime();
    int ReadSmallLen();
    string ReadSmallString();
    void ReadMarker(string value);
    int ReadOptimizedInt(int determineFrom);
    T ReadReadable<T>(int version = 0) where T : IReadable, new();
    TReadable ReadReadable<TReadable, TNode>(TNode node, int version = 0)
        where TNode : IClass
        where TReadable : IReadable<TNode>, new();
    void ReadDeprecVersion();

    int[] ReadArrayOptimizedInt(int length, int? determineFrom = null);
    int[] ReadArrayOptimizedInt(int? determineFrom = null);
    Int2[] ReadArrayOptimizedInt2(int length, int? determineFrom = null);
    Int2[] ReadArrayOptimizedInt2(int? determineFrom = null);

    T[] ReadArray<T>(int length, bool lengthInBytes = false) where T : struct;
    T[] ReadArray<T>(bool lengthInBytes = false) where T : struct;
    T[] ReadArray_deprec<T>(int length, bool lengthInBytes = false) where T : struct;
    T[] ReadArray_deprec<T>(bool lengthInBytes = false) where T : struct;
    IList<T> ReadList<T>(int length, bool lengthInBytes = false) where T : struct;
    IList<T> ReadList<T>(bool lengthInBytes = false) where T : struct;
    IList<T> ReadList_deprec<T>(bool lengthInBytes = false) where T : struct;
    T?[] ReadArrayNodeRef<T>(int length) where T : IClass;
    T?[] ReadArrayNodeRef<T>() where T : IClass;
    T?[] ReadArrayNodeRef_deprec<T>() where T : IClass;
    IList<T?> ReadListNodeRef<T>(int length) where T : IClass;
    IList<T?> ReadListNodeRef<T>() where T : IClass;
    IList<T?> ReadListNodeRef_deprec<T>() where T : IClass;
    T[] ReadArrayReadable<T>(int length, int version = 0) where T : IReadable, new();
    T[] ReadArrayReadable<T>(bool byteLengthPrefix = false, int version = 0) where T : IReadable, new();
    T[] ReadArrayReadable_deprec<T>(bool byteLengthPrefix = false, int version = 0) where T : IReadable, new();
    IList<T> ReadListReadable<T>(int length, int version = 0) where T : IReadable, new();
    IList<T> ReadListReadable<T>(bool byteLengthPrefix = false, int version = 0) where T : IReadable, new();
    IList<T> ReadListReadable_deprec<T>(bool byteLengthPrefix = false, int version = 0) where T : IReadable, new();

    string[] ReadArrayId(int length);
    string[] ReadArrayId();
    string[] ReadArrayId_deprec();
    IList<string> ReadListId(int length);
    IList<string> ReadListId();
    IList<string> ReadListId_deprec();

    uint PeekUInt32();
    void SkipData(int length);
    byte[] ReadToEnd();
    Task<byte[]> ReadToEndAsync(CancellationToken cancellationToken = default);
    void ResetIdState();
}

/// <summary>
/// A binary/text reader specialized for Gbx.
/// </summary>
public sealed partial class GbxReader : BinaryReader, IGbxReader
{
    internal const int MaxDataSize = 0x10000000; // ~268MB

    private static readonly Encoding encoding = Encoding.UTF8;

#if NET6_0_OR_GREATER
    private string? prevString;
#endif

    private bool enablePreviousStringCache;
    public bool EnablePreviousStringCache { get => enablePreviousStringCache; set => enablePreviousStringCache = value; }

    private IReadOnlyDictionary<int, GbxRefTableFile>? refTable;
    private readonly XmlReader? xmlReader;
    private readonly ILogger? logger;

    private int? idVersion;
    private Dictionary<uint, string>? idDict;
    private Dictionary<int, object>? nodeDict;
    private Encapsulation? encapsulation;
    private byte? packDescVersion;
    private int? deprecVersion;
    private GbxReaderWriter? rw;

    internal int? IdVersion
    {
        get => encapsulation is null ? idVersion : encapsulation.IdVersion;
        set
        {
            if (encapsulation is null)
            {
                idVersion = value;
            }
            else
            {
                encapsulation.IdVersion = value;
            }
        }
    }

    internal Dictionary<uint, string> IdDict => encapsulation is null
        ? idDict ??= []
        : encapsulation.IdReadDict;

    internal Dictionary<int, object> NodeDict => nodeDict ??= ExpectedNodeCount.HasValue
        ? new(ExpectedNodeCount.Value) : [];

    internal Encapsulation? Encapsulation { get => encapsulation; set => encapsulation = value; }

    internal byte? PackDescVersion { get => packDescVersion; set => packDescVersion = value; }
    internal int? DeprecVersion { get => deprecVersion; set => deprecVersion = value; }

    internal int? ExpectedNodeCount { get; set; }

    public SerializationMode Mode { get; }
    public GbxFormat Format { get; private set; } = GbxFormat.Binary;

    private GbxReaderLimiter? limiter;

    public GbxReader(Stream input, ILogger? logger = null) : base(input, encoding)
    {
        this.logger = logger;
    }

    public GbxReader(Stream input, bool leaveOpen, ILogger? logger = null) : base(input, encoding, leaveOpen)
    {
        this.logger = logger;
    }

    public GbxReader(XmlReader input, ILogger? logger = null) : base(Stream.Null, encoding)
    {
        xmlReader = input;
        this.logger = logger;

        Mode = SerializationMode.Xml;
    }

    internal void LoadFrom(IGbxReader reader)
    {
        Format = reader.Format;

        if (reader is GbxReader r)
        {
            refTable = r.refTable;
            idVersion = r.idVersion;
            idDict = r.idDict;
            nodeDict = r.nodeDict;
            encapsulation = r.encapsulation;
            packDescVersion = r.packDescVersion;
            deprecVersion = r.deprecVersion;

            ExpectedNodeCount = r.ExpectedNodeCount;
        }
    }

    internal void LoadRefTable(IReadOnlyDictionary<int, GbxRefTableFile> refTable)
    {
        this.refTable = refTable;
    }

    public bool ReadGbxMagic()
    {
        return base.ReadByte() == 'G' && base.ReadByte() == 'B' && base.ReadByte() == 'X';
    }

    public override byte ReadByte()
    {
        limiter?.ThrowIfLimitExceeded(sizeof(byte));

        return Mode switch
        {
            SerializationMode.Gbx => base.ReadByte(),
            _ => throw new SerializationModeNotSupportedException(Mode),
        };
    }

    public override sbyte ReadSByte()
    {
        limiter?.ThrowIfLimitExceeded(sizeof(sbyte));

        return Mode switch
        {
            SerializationMode.Gbx => base.ReadSByte(),
            _ => throw new SerializationModeNotSupportedException(Mode),
        };
    }

    public override short ReadInt16()
    {
        limiter?.ThrowIfLimitExceeded(sizeof(short));

        return Mode switch
        {
            SerializationMode.Gbx => base.ReadInt16(),
            _ => throw new SerializationModeNotSupportedException(Mode),
        };
    }

    public override int ReadInt32()
    {
        limiter?.ThrowIfLimitExceeded(sizeof(int));

        return Mode switch
        {
            SerializationMode.Gbx => base.ReadInt32(),
            _ => throw new SerializationModeNotSupportedException(Mode),
        };
    }

    public int ReadHexInt32()
    {
        limiter?.ThrowIfLimitExceeded(sizeof(int));

        return Mode switch
        {
            SerializationMode.Gbx => base.ReadInt32(),
            _ => throw new SerializationModeNotSupportedException(Mode),
        };
    }

    public override uint ReadUInt32()
    {
        limiter?.ThrowIfLimitExceeded(sizeof(uint));

        return Mode switch
        {
            SerializationMode.Gbx => base.ReadUInt32(),
            _ => throw new SerializationModeNotSupportedException(Mode),
        };
    }

    public uint ReadHexUInt32()
    {
        limiter?.ThrowIfLimitExceeded(sizeof(uint));

        return Mode switch
        {
            SerializationMode.Gbx => base.ReadUInt32(),
            _ => throw new SerializationModeNotSupportedException(Mode),
        };
    }

    public override long ReadInt64()
    {
        limiter?.ThrowIfLimitExceeded(sizeof(long));

        return Mode switch
        {
            SerializationMode.Gbx => base.ReadInt64(),
            _ => throw new SerializationModeNotSupportedException(Mode),
        };
    }

    public override ulong ReadUInt64()
    {
        limiter?.ThrowIfLimitExceeded(sizeof(ulong));

        return Mode switch
        {
            SerializationMode.Gbx => base.ReadUInt64(),
            _ => throw new SerializationModeNotSupportedException(Mode),
        };
    }

    public override float ReadSingle()
    {
        limiter?.ThrowIfLimitExceeded(sizeof(float));

        return Mode switch
        {
            SerializationMode.Gbx => base.ReadSingle(),
            _ => throw new SerializationModeNotSupportedException(Mode),
        };
    }

    public BigInteger ReadBigInt(int byteLength)
    {
        return new BigInteger(ReadBytes(byteLength));
    }

    public Int128 ReadInt128()
    {
        var a = ReadUInt64();
        var b = ReadUInt64();
        return new Int128(b, a);
    }

    public UInt128 ReadUInt128()
    {
        var a = ReadUInt64();
        var b = ReadUInt64();
        return new UInt128(b, a);
    }

    public UInt256 ReadUInt256()
    {
        return new UInt256(ReadUInt64(), ReadUInt64(), ReadUInt64(), ReadUInt64());
    }

    public Int2 ReadInt2()
    {
        return new(ReadInt32(), ReadInt32());
    }

    public Int3 ReadInt3()
    {
        return new(ReadInt32(), ReadInt32(), ReadInt32());
    }

    public Int4 ReadInt4()
    {
        return new(ReadInt32(), ReadInt32(), ReadInt32(), ReadInt32());
    }

    public Byte3 ReadByte3()
    {
        return new(ReadByte(), ReadByte(), ReadByte());
    }

    public Vec2 ReadVec2()
    {
        return new(ReadSingle(), ReadSingle());
    }

    public Vec3 ReadVec3()
    {
        return new(ReadSingle(), ReadSingle(), ReadSingle());
    }

    public Vec4 ReadVec4()
    {
        return new(ReadSingle(), ReadSingle(), ReadSingle(), ReadSingle());
    }

    public BoxAligned ReadBoxAligned()
    {
        return new BoxAligned(X: ReadSingle(),
                              Y: ReadSingle(),
                              Z: ReadSingle(),
                              X2: ReadSingle(),
                              Y2: ReadSingle(),
                              Z2: ReadSingle());
    }

    public BoxInt3 ReadBoxInt3()
    {
        return new BoxInt3(X: ReadInt32(),
                           Y: ReadInt32(),
                           Z: ReadInt32(),
                           X2: ReadInt32(),
                           Y2: ReadInt32(),
                           Z2: ReadInt32());
    }

    public Color ReadColor()
    {
        return new(ReadSingle(), ReadSingle(), ReadSingle(), ReadSingle());
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

    public Quat ReadQuat()
    {
        return new(X: ReadSingle(), Y: ReadSingle(), Z: ReadSingle(), W: ReadSingle());
    }

    public Rect ReadRect()
    {
        return new Rect(X: ReadSingle(),
                        Y: ReadSingle(),
                        X2: ReadSingle(),
                        Y2: ReadSingle());
    }

    public GbxFormat ReadFormatByte()
    {
        Format = (GbxFormat)ReadByte();
        return Format;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    /// <exception cref="BooleanOutOfRangeException"></exception>
    /// <exception cref="SerializationModeNotSupportedException"></exception>
    public override bool ReadBoolean()
    {
        limiter?.ThrowIfLimitExceeded(4);

        switch (Mode)
        {
            case SerializationMode.Gbx:
                var booleanAsInt = base.ReadUInt32();

                if (Gbx.StrictBooleans && booleanAsInt > 1)
                {
                    throw new BooleanOutOfRangeException(booleanAsInt);
                }

                return booleanAsInt != 0;
            default:
                throw new SerializationModeNotSupportedException(Mode);
        }
    }

    public bool ReadBoolean(bool asByte)
    {
        if (!asByte)
        {
            return ReadBoolean();
        }

        var booleanAsByte = ReadByte();

        if (Gbx.StrictBooleans && booleanAsByte > 1)
        {
            throw new BooleanOutOfRangeException(booleanAsByte);
        }

        return booleanAsByte != 0;
    }

    public override string ReadString()
    {
        switch (Mode)
        {
            case SerializationMode.Gbx:
                limiter?.ThrowIfLimitExceeded(sizeof(int));
                return ReadString(base.ReadInt32());
            default:
                throw new SerializationModeNotSupportedException(Mode);
        }
    }

    public string ReadString(StringLengthPrefix readPrefix)
    {
        switch (Mode)
        {
            case SerializationMode.Gbx:
                // Length of the string in bytes, not chars
                int length;
                switch (readPrefix)
                {
                    case StringLengthPrefix.Byte:
                        limiter?.ThrowIfLimitExceeded(sizeof(byte));
                        length = base.ReadByte();
                        break;
                    case StringLengthPrefix.Int32:
                        limiter?.ThrowIfLimitExceeded(sizeof(int));
                        length = base.ReadInt32();
                        break;
                    default:
                        throw new ArgumentException("Can't read string without knowing its length.", nameof(readPrefix));
                }

                return ReadString(length);
            default:
                throw new SerializationModeNotSupportedException(Mode);
        }
    }

    public string ReadString(int length)
    {
        switch (Mode)
        {
            case SerializationMode.Gbx:
                if (length == 0)
                {
                    return string.Empty;
                }

                if (length < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(length), "Length cannot be negative.");
                }

                if (length > MaxDataSize) // ~268MB
                {
                    throw new LengthLimitException(length);
                }

#if NET6_0_OR_GREATER
                if (length > 2048)
                {
#endif
                    return encoding.GetString(ReadBytes(length));
#if NET6_0_OR_GREATER
                }

                limiter?.ThrowIfLimitExceeded(length);

                Span<byte> bytes = stackalloc byte[length];

                if (Read(bytes) != length)
                {
                    throw new EndOfStreamException();
                }

                if (!enablePreviousStringCache)
                {
                    return encoding.GetString(bytes);
                }

                Span<char> chars = stackalloc char[1024];

                var charLength = encoding.GetChars(bytes, chars);
                var charSlice = chars.Slice(0, charLength);

                if (prevString is not null && MemoryExtensions.Equals(charSlice, prevString, StringComparison.Ordinal))
                {
                    return prevString;
                }

                return prevString = charSlice.ToString();
#endif
            default:
                throw new SerializationModeNotSupportedException(Mode);
        }
        
    }

    [Zomp.SyncMethodGenerator.CreateSyncVersion]
    public async Task<byte[]> ReadDataAsync(CancellationToken cancellationToken = default)
    {
        return await ReadBytesAsync(ReadInt32(), cancellationToken);
    }

    [Zomp.SyncMethodGenerator.CreateSyncVersion]
    public async Task<byte[]> ReadDataAsync(int length, CancellationToken cancellationToken = default)
    {
        return await ReadBytesAsync(length, cancellationToken);
    }

    public override byte[] ReadBytes(int count)
    {
        if (count > MaxDataSize)
        {
            throw new LengthLimitException(count);
        }

        limiter?.ThrowIfLimitExceeded(count);

        return Mode switch
        {
            SerializationMode.Gbx => base.ReadBytes(count),
            _ => throw new SerializationModeNotSupportedException(Mode),
        };
    }

    public async Task<byte[]> ReadBytesAsync(int count, CancellationToken cancellationToken = default)
    {
        if (count > MaxDataSize)
        {
            throw new LengthLimitException(count);
        }

        limiter?.ThrowIfLimitExceeded(count);

        var buffer = new byte[count];

        _ = Mode switch
        {
            SerializationMode.Gbx => await BaseStream.ReadAsync(buffer, 0, count, cancellationToken),
            _ => throw new SerializationModeNotSupportedException(Mode),
        };

        return buffer;
    }

    public string ReadIdAsString()
    {
        var index = ReadIdIndex();

        if (index == uint.MaxValue)
        {
            return string.Empty;
        }

        if ((index & 0xC0000000) is not 0x40000000 and not 0x80000000)
        {
            return index.ToString();
        }

        return ReadIdAsString(index);
    }

    public Id ReadId()
    {
        var index = ReadIdIndex();

        if (index == uint.MaxValue)
        {
            return new();
        }

        if ((index & 0xC0000000) is not 0x40000000 and not 0x80000000)
        {
            return new((int)index);
        }

        return new(ReadIdAsString(index));
    }

    private uint ReadIdIndex()
    {
        return Mode switch
        {
            SerializationMode.Gbx => Format switch
            {
                GbxFormat.Binary => GbxBinary(),
                _ => throw new Exception(),
            },
            _ => throw new SerializationModeNotSupportedException(Mode),
        };

        uint GbxBinary()
        {
            IdVersion ??= ReadInt32();

            if (IdVersion.Value < 3)
            {
                throw new NotSupportedException($"Unsupported Id version ({IdVersion}).");
            }

            return ReadUInt32();
        }
    }

    private string ReadIdAsString(uint index)
    {
        if ((index & 0x3FFFFFFF) != 0)
        {
            return IdDict?[index] ?? throw new Exception("Invalid Id index.");
        }

        var str = ReadString();

        if ((index & 0xC0000000) == 0x40000000)
        {
            // SetLocalName
            IdDict.Add((uint)(index + IdDict.Count + 1), str);
        }
        else
        {
            // AddName
            IdDict.Add((uint)(index + IdDict.Count + 1), str);
        }

        return str;
    }

    public Ident ReadIdent()
    {
        var id = ReadIdAsString();
        var collection = ReadId();
        var author = ReadIdAsString();

        return new Ident(id, collection, author);
    }

    public PackDesc ReadPackDesc()
    {
        var version = ReadByte();
        packDescVersion ??= version;

        if (version != packDescVersion)
        {
            // PackDesc version mismatch. but it's fine
        }

        var checksum = default(UInt256?);
        var locatorUrl = "";

        if (version >= 3)
        {
            checksum = ReadUInt256();
        }

        var filePath = ReadString();

        if ((filePath.Length > 0 && version >= 1) || version >= 3)
        {
            locatorUrl = ReadString();
        }

        return new PackDesc(filePath, checksum, locatorUrl);
    }

    public T? ReadNodeRef<T>(out GbxRefTableFile? file) where T : IClass
    {
        var index = default(int?);

        if (encapsulation is null)
        {
            index = ReadInt32();

            if (index == -1)
            {
                file = null;
                return default;
            }

            if (NodeDict.TryGetValue(index.Value, out var existingNode))
            {
                file = null;
                return (T)existingNode;
            }

            if (refTable?.TryGetValue(index.Value, out file) == true)
            {
                // this can return null as the file is guaranteed to exist
                // this file is then used next to the actual member, and when requested, loaded into the member
                // then the file uses this instance in its Node property, and the referenced file is nullified
                // this will avoid any lost references
                return default;
            }
        }

        file = null;

        var originalClassId = ReadUInt32();

        if (encapsulation is not null && originalClassId == uint.MaxValue)
        {
            return default;
        }

        var classId = ClassManager.Wrap(originalClassId);

#if NET8_0_OR_GREATER
        var node = T.New(classId) ?? throw new Exception($"Unknown class ID: 0x{classId:X8} ({ClassManager.GetName(classId) ?? "unknown class name"})");
#else
        var node = ClassManager.New(classId) ?? throw new Exception($"Unknown class ID: 0x{classId:X8} ({ClassManager.GetName(classId) ?? "unknown class name"})");
#endif

        if (node is not T nod)
        {
            throw new InvalidCastException($"Class ID 0x{classId:X8} ({ClassManager.GetName(classId) ?? "unknown class name"}) cannot be casted to {typeof(T).Name}.");
        }

        if (index.HasValue)
        {
            // TODO: Report on replacements
            NodeDict[index.Value] = nod;
        }

        return ReadNode(nod);
    }

    public T? ReadNodeRef<T>() where T : IClass
    {
        var node = ReadNodeRef<T>(out var file);

        if (file is not null)
        {
            logger?.LogWarning("Reference table file discard: {File}", file);
        }

        return node;
    }

    public T? ReadNode<T>() where T : IClass, new()
    {
        var node = new T();
        return ReadNode(node);
    }

    private T ReadNode<T>(T node) where T : IClass
    {
        rw ??= new GbxReaderWriter(this, leaveOpen: true);

#if NET8_0_OR_GREATER
        T.Read(node, rw);
#else
        node.ReadWrite(rw);
#endif

        return node;
    }

    public TimeInt32 ReadTimeInt32()
    {
        return new(ReadInt32());
    }

    public TimeInt32? ReadTimeInt32Nullable()
    {
        var val = ReadInt32();
        if (val == -1) return null;
        return new(val);
    }

    public TimeSingle ReadTimeSingle()
    {
        return new(ReadSingle());
    }

    public TimeSingle? ReadTimeSingleNullable()
    {
        var val = ReadSingle();
        if (val == -1) return null;
        return new(val);
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

    public DateTime ReadFileTime()
    {
        return DateTime.FromFileTime(ReadInt64());
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

    public void ReadMarker(string value)
    {
#if NET8_0_OR_GREATER
        Span<byte> expected = stackalloc byte[value.Length * 4];
        encoding.TryGetBytes(value, expected, out var bytesWritten);
        limiter?.ThrowIfLimitExceeded(bytesWritten);
        Span<byte> actual = stackalloc byte[bytesWritten];
        Read(actual);

        for (var i = 0; i < bytesWritten; i++)
        {
            if (expected[i] != actual[i])
            {
                throw new Exception($"Invalid marker: {value} != {encoding.GetString(actual)}");
            }
        }
#else

#if NET6_0_OR_GREATER
        var count = encoding.GetByteCount(value);
        limiter?.ThrowIfLimitExceeded(count);
        Span<byte> actual = stackalloc byte[count];
        Read(actual);
#else
        var count = encoding.GetByteCount(value);
        var actual = ReadBytes(count);
#endif

        if (!encoding.GetString(actual).Equals(value, StringComparison.Ordinal))
        {
            throw new Exception($"Invalid marker: {value} != {encoding.GetString(actual)}");
        }

#endif
    }

    public int ReadOptimizedInt(int determineFrom) => (uint)determineFrom switch
    {
        >= ushort.MaxValue => ReadInt32(),
        >= byte.MaxValue => ReadUInt16(),
        _ => ReadByte()
    };

    public int[] ReadArrayOptimizedInt(int length, int? determineFrom = null)
    {
        if (length == 0)
        {
            return [];
        }

        ValidateCollectionLength(length);

        return (uint)determineFrom.GetValueOrDefault(length) switch
        {
            >= ushort.MaxValue => ReadArray<int>(length),
            >= byte.MaxValue => Array.ConvertAll(ReadArray<ushort>(length), x => (int)x),
            _ => Array.ConvertAll(ReadBytes(length), x => (int)x)
        };
    }

    public int[] ReadArrayOptimizedInt(int? determineFrom = null) => ReadArrayOptimizedInt(ReadInt32(), determineFrom);

    public Int2[] ReadArrayOptimizedInt2(int length, int? determineFrom = null)
    {
        if (length == 0)
        {
            return [];
        }

        ValidateCollectionLength(length);

        return (uint)determineFrom.GetValueOrDefault(length) switch
        {
            >= ushort.MaxValue => ReadArray<Int2>(length),
            >= byte.MaxValue => Array.ConvertAll(ReadArray<int>(length), x => new Int2(x & 0xFFFF, x >> 16)),
            _ => Array.ConvertAll(ReadArray<ushort>(length), x => new Int2(x & 0xFF, x >> 8))
        };
    }

    public Int2[] ReadArrayOptimizedInt2(int? determineFrom = null) => ReadArrayOptimizedInt2(ReadInt32(), determineFrom);

    public T ReadReadable<T>(int version = 0) where T : IReadable, new()
    {
        var readable = new T();
        readable.Read(this, version);
        return readable;
    }

    public TReadable ReadReadable<TReadable, TNode>(TNode node, int version = 0)
        where TNode : IClass
        where TReadable : IReadable<TNode>, new()
    {
        var readable = new TReadable();
        readable.Read(this, node, version);
        return readable;
    }

    public T[] ReadArrayReadable<T>(int length, int version = 0) where T : IReadable, new()
    {
        if (length == 0)
        {
            return [];
        }

        ValidateCollectionLength(length);

        var array = new T[length];

        for (int i = 0; i < length; i++)
        {
            array[i] = ReadReadable<T>(version);
        }

        return array;
    }

    public T[] ReadArrayReadable<T>(bool byteLengthPrefix = false, int version = 0) where T : IReadable, new()
        => ReadArrayReadable<T>(byteLengthPrefix ? ReadByte() : ReadInt32(), version);

    public T[] ReadArrayReadable_deprec<T>(bool byteLengthPrefix = false, int version = 0) where T : IReadable, new()
    {
        ReadDeprecVersion();
        return ReadArrayReadable<T>(byteLengthPrefix, version);
    }

    public IList<T> ReadListReadable<T>(int length, int version = 0) where T : IReadable, new()
    {
        if (length == 0)
        {
            return new List<T>();
        }

        ValidateCollectionLength(length);

        var list = new List<T>(length);

        for (int i = 0; i < length; i++)
        {
            list.Add(ReadReadable<T>(version));
        }

        return list;
    }

    public IList<T> ReadListReadable<T>(bool byteLengthPrefix = false, int version = 0) where T : IReadable, new()
        => ReadListReadable<T>(byteLengthPrefix ? ReadByte() : ReadInt32(), version);

    public IList<T> ReadListReadable_deprec<T>(bool byteLengthPrefix = false, int version = 0) where T : IReadable, new()
    {
        ReadDeprecVersion();
        return ReadListReadable<T>(byteLengthPrefix, version);
    }

    private void ValidateCollectionLength(int length)
    {
        if (length < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(length), "Length is not valid.");
        }

        if (length < 0 || length > 0x10000000) // ~268MB
        {
            throw new Exception($"Length is too big to handle ({length}).");
        }

        limiter?.ThrowIfLimitExceeded(length);
    }

    private int ValidateCollectionLength<T>(int length, bool lengthInBytes) where T : struct
    {
        if (length < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(length), "Length is not valid (-1).");
        }

        var l = lengthInBytes ? length : Marshal.SizeOf<T>() * length;

        if (l is < 0 or > 0x10000000) // ~268MB
        {
            throw new Exception($"Length is too big to handle ({(l < 0 ? length : l)}).");
        }

        limiter?.ThrowIfLimitExceeded(l);

        return l;
    }

    public void ReadDeprecVersion()
    {
        var version = ReadInt32();
        deprecVersion ??= version;

        if (version != deprecVersion)
        {
            // Version mismatch. but it's fine
        }
    }

    public T[] ReadArray<T>(int length, bool lengthInBytes = false) where T : struct
    {
        if (length == 0)
        {
            return [];
        }

        var l = ValidateCollectionLength<T>(length, lengthInBytes);

        if (l > 1_000_000)
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

    public T[] ReadArray<T>(bool lengthInBytes = false) where T : struct => ReadArray<T>(ReadInt32(), lengthInBytes);

    public T[] ReadArray_deprec<T>(int length, bool lengthInBytes = false) where T : struct
    {
        ReadDeprecVersion();
        return ReadArray<T>(length, lengthInBytes);
    }

    public T[] ReadArray_deprec<T>(bool lengthInBytes = false) where T : struct => ReadArray_deprec<T>(ReadInt32(), lengthInBytes);

    public IList<T> ReadList<T>(int length, bool lengthInBytes = false) where T : struct
    {
        if (length == 0)
        {
            return new List<T>();
        }

        var l = ValidateCollectionLength<T>(length, lengthInBytes);

        var list = new List<T>(l);

        if (l > 1_000_000)
        {
            var largerSpan = MemoryMarshal.Cast<byte, T>(ReadBytes(l));

            for (var i = 0; i < largerSpan.Length; i++)
            {
                list.Add(largerSpan[i]);
            }

            return list;
        }

#if NETSTANDARD2_1_OR_GREATER || NET6_0_OR_GREATER
        Span<byte> bytes = stackalloc byte[l];
        Read(bytes);
#else
        var bytes = ReadBytes(l);
#endif

        var span = MemoryMarshal.Cast<byte, T>(bytes);

        for (var i = 0; i < span.Length; i++)
        {
            list.Add(span[i]);
        }

        return list;
    }

    public IList<T> ReadList<T>(bool lengthInBytes = false) where T : struct => ReadList<T>(ReadInt32(), lengthInBytes);

    public IList<T> ReadList_deprec<T>(bool lengthInBytes = false) where T : struct
    {
        ReadDeprecVersion();
        return ReadList<T>(lengthInBytes);
    }

    public T?[] ReadArrayNodeRef<T>(int length) where T : IClass
    {
        if (length == 0)
        {
            return [];
        }

        ValidateCollectionLength(length);

        var array = new T?[length];

        for (var i = 0; i < length; i++)
        {
            array[i] = ReadNodeRef<T>();
        }

        return array;
    }

    public T?[] ReadArrayNodeRef<T>() where T : IClass => ReadArrayNodeRef<T>(ReadInt32());

    public T?[] ReadArrayNodeRef_deprec<T>() where T : IClass
    {
        ReadDeprecVersion();
        return ReadArrayNodeRef<T>();
    }

    public IList<T?> ReadListNodeRef<T>(int length) where T : IClass
    {
        if (length == 0)
        {
            return new List<T?>();
        }

        ValidateCollectionLength(length);

        var list = new List<T?>(length);

        for (var i = 0; i < length; i++)
        {
            list.Add(ReadNodeRef<T>());
        }

        return list;
    }

    public IList<T?> ReadListNodeRef<T>() where T : IClass => ReadListNodeRef<T>(ReadInt32());

    public IList<T?> ReadListNodeRef_deprec<T>() where T : IClass
    {
        ReadDeprecVersion();
        return ReadListNodeRef<T>();
    }

    public string[] ReadArrayId(int length)
    {
        if (length == 0)
        {
            return [];
        }

        ValidateCollectionLength(length);

        var array = new string[length];

        for (var i = 0; i < length; i++)
        {
            array[i] = ReadIdAsString();
        }

        return array;
    }

    public string[] ReadArrayId() => ReadArrayId(ReadInt32());

    public string[] ReadArrayId_deprec()
    {
        ReadDeprecVersion();
        return ReadArrayId();
    }

    public IList<string> ReadListId(int length)
    {
        if (length == 0)
        {
            return new List<string>();
        }

        ValidateCollectionLength(length);

        var list = new List<string>(length);

        for (var i = 0; i < length; i++)
        {
            list.Add(ReadIdAsString());
        }

        return list;
    }

    public IList<string> ReadListId() => ReadListId(ReadInt32());

    public IList<string> ReadListId_deprec()
    {
        ReadDeprecVersion();
        return ReadListId();
    }

    /// <summary>
    /// If can seek, position moves past the <paramref name="length"/>. If seeking is NOT supported, data is read with no allocation using <see cref="BinaryReader.Read(Span{byte})"/>. If .NET Standard 2.0, unavoidable byte array allocation happens with <see cref="BinaryReader.ReadBytes(int)"/>.
    /// </summary>
    /// <param name="length">Length in bytes to skip.</param>
    /// <exception cref="EndOfStreamException"></exception>
    public void SkipData(int length)
    {
        if (length == 0)
        {
            return;
        }

        if (length < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(length), "Length is not valid.");
        }

        limiter?.ThrowIfLimitExceeded(length);

        if (BaseStream.CanSeek)
        {
            _ = BaseStream.Seek(length, SeekOrigin.Current);

            return;
        }

#if NET6_0_OR_GREATER
        if ((length > 1_000_000 && ReadBytes(length).Length != length) || Read(stackalloc byte[length]) != length)
#else
        if (ReadBytes(length).Length != length)
#endif
        {
            // Should be silent?
        }
    }

    public uint PeekUInt32()
    {
        var result = ReadUInt32();
        BaseStream.Seek(-sizeof(uint), SeekOrigin.Current);
        return result;
    }

    public byte[] ReadToEnd()
    {
        if (BaseStream.CanSeek)
        {
            return ReadBytes((int)(BaseStream.Length - BaseStream.Position));
        }

        using var ms = new MemoryStream();
        BaseStream.CopyTo(ms);
        return ms.ToArray();
    }

    public async Task<byte[]> ReadToEndAsync(CancellationToken cancellationToken = default)
    {
        if (BaseStream.CanSeek)
        {
            var buffer = new byte[BaseStream.Length - BaseStream.Position];
#if NET6_0_OR_GREATER
            _ = await BaseStream.ReadAsync(buffer, cancellationToken);
#else
            _ = await BaseStream.ReadAsync(buffer, 0, buffer.Length, cancellationToken);
#endif
            return buffer;
        }

        using var ms = new MemoryStream();
        await BaseStream.CopyToAsync(ms, bufferSize: 81920, cancellationToken);
        return ms.ToArray();
    }

    public void ResetIdState()
    {
        IdVersion = null;
        IdDict.Clear();
    }

    private string ReadToCRLF()
    {
        var sb = new StringBuilder();

        while (true)
        {
            var b = base.ReadByte();

            if (b == 0x0D)
            {
                if (base.ReadByte() != 0x0A)
                {
                    throw new Exception("Invalid string format.");
                }

                break;
            }

            sb.Append((char)b);
        }

        return sb.ToString();
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            idDict = null;
        }

        base.Dispose(disposing);
    }

    internal void Limit(int limit)
    {
        limiter ??= new();
        limiter.Limit(limit);
    }

    internal void Unlimit(bool skipToLimitWhenUnreached = false)
    {
        if (limiter is null)
        {
            return;
        }

        if (skipToLimitWhenUnreached)
        {
            SkipData(limiter.Remaining);
        }
        else if (limiter.Remaining > 0)
        {
            throw new Exception($"Data limit not reached ({limiter.Remaining} bytes remaining).");
        }

        limiter.Unlimit();
    }
}