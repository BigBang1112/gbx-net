using GBX.NET.Components;
using GBX.NET.Managers;
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
    BigInteger ReadInt128();
    Int2 ReadInt2();
    Int3 ReadInt3();
    Int4 ReadInt4();
    Byte3 ReadByte3();
    Vec2 ReadVec2();
    Vec3 ReadVec3();
    Vec4 ReadVec4();
    Box ReadBox();
    Color ReadColor();
    Iso4 ReadIso4();
    Mat3 ReadMat3();
    Mat4 ReadMat4();
    Quat ReadQuat();
    Rect ReadRect();
    bool ReadBoolean();
    bool ReadBoolean(bool asByte);
    byte[] ReadData();
    byte[] ReadData(int length);
    byte[] ReadBytes(int count);
    string ReadString();
    string ReadString(int length);
    string ReadString(StringLengthPrefix readPrefix);
    string ReadIdAsString();
    Id ReadId();
    Ident ReadIdent();
    PackDesc ReadPackDesc();
    T? ReadNodeRef<T>() where T : IClass;
    T? ReadNode<T>() where T : IClass, new();
    TimeInt32 ReadTimeInt32();
    TimeInt32? ReadTimeInt32Nullable();
    TimeSingle ReadTimeSingle();
    TimeSingle? ReadTimeSingleNullable();
    TimeSpan? ReadTimeOfDay();
    void ReadMarker(string value);
    T ReadReadable<T>(int version = 0) where T : IReadable, new();
    void ReadDeprecVersion();

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
    T[] ReadArrayReadable<T>(int version = 0) where T : IReadable, new();
    T[] ReadArrayReadable_deprec<T>(int version = 0) where T : IReadable, new();
    IList<T> ReadListReadable<T>(int length, int version = 0) where T : IReadable, new();
    IList<T> ReadListReadable<T>(int version = 0) where T : IReadable, new();
    IList<T> ReadListReadable_deprec<T>(int version = 0) where T : IReadable, new();

    string[] ReadArrayId(int length);
    string[] ReadArrayId();
    string[] ReadArrayId_deprec();
    IList<string> ReadListId(int length);
    IList<string> ReadListId();
    IList<string> ReadListId_deprec();

    uint PeekUInt32();
    void SkipData(int length);
    byte[] ReadToEnd();
    void ResetIdState();
    void LoadStateFrom(IGbxReader reader);
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

    private GbxRefTable? refTable;
    private readonly XmlReader? xmlReader;

    private int? idVersion;
    private Dictionary<int, string>? idDict;
    private Dictionary<int, object>? nodeDict;
    private Encapsulation? encapsulation;
    private byte? packDescVersion;
    private int? deprecVersion;

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

    internal Dictionary<int, string> IdDict => encapsulation is null
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

    public GbxReader(Stream input, GbxRefTable? refTable = null) : base(input, encoding)
    {
        this.refTable = refTable;
    }

    public GbxReader(Stream input, bool leaveOpen, GbxRefTable? refTable = null) : base(input, encoding, leaveOpen)
    {
        this.refTable = refTable;
    }

    public GbxReader(XmlReader input) : base(Stream.Null, encoding)
    {
        xmlReader = input;
        Mode = SerializationMode.Xml;
    }

    public void LoadStateFrom(IGbxReader reader)
    {
        Format = reader.Format;

        if (reader is GbxReader r)
        {
            refTable = r.refTable;
            idVersion = r.idVersion;
            idDict = r.idDict;
            encapsulation = r.encapsulation;
            packDescVersion = r.packDescVersion;
            deprecVersion = r.deprecVersion;

            ExpectedNodeCount = r.ExpectedNodeCount;
        }
    }

    public bool ReadGbxMagic()
    {
        return base.ReadByte() == 'G' && base.ReadByte() == 'B' && base.ReadByte() == 'X';
    }

    public override byte ReadByte()
    {
        return Mode switch
        {
            SerializationMode.Gbx => base.ReadByte(),
            _ => throw new SerializationModeNotSupportedException(Mode),
        };
    }

    public override sbyte ReadSByte()
    {
        return Mode switch
        {
            SerializationMode.Gbx => base.ReadSByte(),
            _ => throw new SerializationModeNotSupportedException(Mode),
        };
    }

    public override short ReadInt16()
    {
        return Mode switch
        {
            SerializationMode.Gbx => base.ReadInt16(),
            _ => throw new SerializationModeNotSupportedException(Mode),
        };
    }

    public override int ReadInt32()
    {
        return Mode switch
        {
            SerializationMode.Gbx => base.ReadInt32(),
            _ => throw new SerializationModeNotSupportedException(Mode),
        };
    }

    public int ReadHexInt32()
    {
        return Mode switch
        {
            SerializationMode.Gbx => base.ReadInt32(),
            _ => throw new SerializationModeNotSupportedException(Mode),
        };
    }

    public override uint ReadUInt32()
    {
        return Mode switch
        {
            SerializationMode.Gbx => base.ReadUInt32(),
            _ => throw new SerializationModeNotSupportedException(Mode),
        };
    }

    public uint ReadHexUInt32()
    {
        return Mode switch
        {
            SerializationMode.Gbx => base.ReadUInt32(),
            _ => throw new SerializationModeNotSupportedException(Mode),
        };
    }

    public override long ReadInt64()
    {
        return Mode switch
        {
            SerializationMode.Gbx => base.ReadInt64(),
            _ => throw new SerializationModeNotSupportedException(Mode),
        };
    }

    public override ulong ReadUInt64()
    {
        return Mode switch
        {
            SerializationMode.Gbx => base.ReadUInt64(),
            _ => throw new SerializationModeNotSupportedException(Mode),
        };
    }

    public override float ReadSingle()
    {
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

    public BigInteger ReadInt128()
    {
        return ReadBigInt(byteLength: 16);
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

    public Box ReadBox()
    {
        return new Box(X: ReadSingle(),
                       Y: ReadSingle(),
                       Z: ReadSingle(),
                       X2: ReadSingle(),
                       Y2: ReadSingle(),
                       Z2: ReadSingle());
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

        switch (Mode)
        {
            case SerializationMode.Gbx:
                var booleanAsByte = base.ReadByte();

                if (Gbx.StrictBooleans && booleanAsByte > 1)
                {
                    throw new BooleanOutOfRangeException(booleanAsByte);
                }

                return booleanAsByte != 0;
            default:
                throw new SerializationModeNotSupportedException(Mode);
        }
    }

    public override string ReadString()
    {
        return Mode switch
        {
            SerializationMode.Gbx => ReadString(base.ReadInt32()),
            _ => throw new SerializationModeNotSupportedException(Mode),
        };
    }

    public string ReadString(StringLengthPrefix readPrefix)
    {
        switch (Mode)
        {
            case SerializationMode.Gbx:
                // Length of the string in bytes, not chars
                var length = readPrefix switch
                {
                    StringLengthPrefix.Byte => base.ReadByte(),
                    StringLengthPrefix.Int32 => base.ReadInt32(),
                    _ => throw new ArgumentException("Can't read string without knowing its length.", nameof(readPrefix)),
                };

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

    public byte[] ReadData()
    {
        return ReadBytes(ReadInt32());
    }

    public byte[] ReadData(int length)
    {
        return ReadBytes(length);
    }

    public override byte[] ReadBytes(int count)
    {
        if (count > MaxDataSize)
        {
            throw new LengthLimitException(count);
        }

        return Mode switch
        {
            SerializationMode.Gbx => base.ReadBytes(count),
            _ => throw new SerializationModeNotSupportedException(Mode),
        };
    }

    public string ReadIdAsString()
    {
        var index = ReadIdIndex();

        if (index == -1)
        {
            return string.Empty;
        }

        if ((index & 0xC0000000) is not 0x40000000 and not 0x80000000)
        {
            throw new NotSupportedException("This Id cannot be read as string.");
        }

        return ReadIdAsString(index);
    }

    public Id ReadId()
    {
        var index = ReadIdIndex();

        if (index == -1)
        {
            return new();
        }

        if ((index & 0xC0000000) is not 0x40000000 and not 0x80000000)
        {
            return new(index);
        }

        return new(ReadIdAsString(index));
    }

    private int ReadIdIndex()
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

        int GbxBinary()
        {
            IdVersion ??= ReadInt32();

            if (IdVersion.Value < 3)
            {
                throw new NotSupportedException($"Unsupported Id version ({IdVersion}).");
            }

            return ReadInt32();
        }
    }

    private string ReadIdAsString(int index)
    {
        if ((index & 0x3FFFFFFF) != 0)
        {
            return IdDict?[index] ?? throw new Exception("Invalid Id index.");
        }

        var str = ReadString();

        if ((index & 0xC0000000) == 0x40000000)
        {
            // SetLocalName
            IdDict.Add(index + IdDict.Count + 1, str);
        }
        else
        {
            // AddName
            IdDict.Add(index + IdDict.Count + 1, str);
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

        var checksum = default(byte[]);
        var locatorUrl = "";

        if (version >= 3)
        {
            checksum = ReadBytes(32);
        }

        var filePath = ReadString();

        if ((filePath.Length > 0 && version >= 1) || version >= 3)
        {
            locatorUrl = ReadString();
        }

        return new PackDesc(filePath, checksum, locatorUrl);
    }

    public T? ReadNodeRef<T>() where T : IClass
    {
        var index = default(int?);

        if (encapsulation is null)
        {
            index = ReadInt32();

            if (index == -1)
            {
                return default;
            }

            if (NodeDict.TryGetValue(index.Value, out var existingNode))
            {
                return (T)existingNode;
            }

            // check if in ref. table
        }

        var originalClassId = ReadUInt32();

        /* TODO: Verify this
         * if (originalClassId == uint.MaxValue)
        {
            return default;
        }*/

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

    public T? ReadNode<T>() where T : IClass, new()
    {
        var node = new T();
        return ReadNode(node);
    }

    private T ReadNode<T>(T node) where T : IClass
    {
        var rw = new GbxReaderWriter(this, leaveOpen: true);

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

    public void ReadMarker(string value)
    {
#if NET8_0_OR_GREATER
        Span<byte> expected = stackalloc byte[value.Length * 4];
        encoding.TryGetBytes(value, expected, out var bytesWritten);
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

    public T ReadReadable<T>(int version = 0) where T : IReadable, new()
    {
        var readable = new T();
        readable.Read(this, version);
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

    public T[] ReadArrayReadable<T>(int version = 0) where T : IReadable, new() => ReadArrayReadable<T>(ReadInt32(), version);

    public T[] ReadArrayReadable_deprec<T>(int version = 0) where T : IReadable, new()
    {
        ReadDeprecVersion();
        return ReadArrayReadable<T>(version);
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

    public IList<T> ReadListReadable<T>(int version = 0) where T : IReadable, new() => ReadListReadable<T>(ReadInt32(), version);

    public IList<T> ReadListReadable_deprec<T>(int version = 0) where T : IReadable, new()
    {
        ReadDeprecVersion();
        return ReadListReadable<T>(version);
    }

    private static void ValidateCollectionLength(int length)
    {
        if (length < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(length), "Length is not valid.");
        }

        if (length < 0 || length > 0x10000000) // ~268MB
        {
            throw new Exception($"Length is too big to handle ({length}).");
        }
    }

    private static int ValidateCollectionLength<T>(int length, bool lengthInBytes) where T : struct
    {
        if (length < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(length), "Length is not valid.");
        }

        var l = lengthInBytes ? length : Marshal.SizeOf<T>() * length;

        if (l < 0 || l > 0x10000000) // ~268MB
        {
            throw new Exception($"Length is too big to handle ({(l < 0 ? length : l)}).");
        }

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
        if (length < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(length), "Length is not valid.");
        }

        if (BaseStream.CanSeek)
        {
            if (BaseStream.Position + length > BaseStream.Length)
            {
                throw new EndOfStreamException();
            }

            _ = BaseStream.Seek(length, SeekOrigin.Current);

            return;
        }

#if NET6_0_OR_GREATER
        if (Read(stackalloc byte[length]) != length)
#else
        if (ReadBytes(length).Length != length)
#endif
        {
            throw new EndOfStreamException();
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

    public void ResetIdState()
    {
        IdVersion = null;
        IdDict.Clear();
    }

    private string ReadToWindowsNewLine()
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
}