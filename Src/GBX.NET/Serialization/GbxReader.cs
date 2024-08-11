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
    int ReadDataInt32();
    uint ReadDataUInt32();
    long ReadDataInt64();
    ulong ReadDataUInt64();
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
    Vec3 ReadVec3_10b();
    Vec3 ReadVec3Unit2();
    Vec3 ReadVec3_4();
    Vec3 ReadVec3Unit4();
    Vec4 ReadVec4();
    BoxAligned ReadBoxAligned();
    BoxInt3 ReadBoxInt3();
    Color ReadColor();
    Iso4 ReadIso4();
    Mat3 ReadMat3();
    Mat4 ReadMat4();
    Quat ReadQuat();
    Quat ReadQuat6();
    Rect ReadRect();
    TransQuat ReadTransQuat();
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
    [IgnoreForCodeGeneration] IClass? ReadNodeRef();
    T? ReadNodeRef<T>(out GbxRefTableFile? file) where T : IClass;
    [IgnoreForCodeGeneration] IClass? ReadNodeRef(out GbxRefTableFile? file);
    T? ReadNode<T>() where T : IClass, new();
    T? ReadMetaRef<T>() where T : IClass;
    TimeInt32 ReadTimeInt32();
    TimeInt32? ReadTimeInt32Nullable();
    TimeSingle ReadTimeSingle();
    TimeSingle? ReadTimeSingleNullable();
    TimeSpan? ReadTimeOfDay();
    DateTime ReadFileTime();
    DateTime ReadSystemTime();
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
    Vec3[] ReadArrayVec3_10b();
    Vec3[] ReadArrayVec3_10b(int length);

    T[] ReadArray<T>(int length, bool lengthInBytes = false) where T : struct;
    T[] ReadArray<T>(bool lengthInBytes = false) where T : struct;
    T[] ReadArray_deprec<T>(int length, bool lengthInBytes = false) where T : struct;
    T[] ReadArray_deprec<T>(bool lengthInBytes = false) where T : struct;
    List<T> ReadList<T>(int length, bool lengthInBytes = false) where T : struct;
    List<T> ReadList<T>(bool lengthInBytes = false) where T : struct;
    List<T> ReadList_deprec<T>(bool lengthInBytes = false) where T : struct;
    T?[] ReadArrayNodeRef<T>(int length) where T : IClass;
    T?[] ReadArrayNodeRef<T>() where T : IClass;
    T?[] ReadArrayNodeRef_deprec<T>() where T : IClass;
    List<T?> ReadListNodeRef<T>(int length) where T : IClass;
    List<T?> ReadListNodeRef<T>() where T : IClass;
    List<T?> ReadListNodeRef_deprec<T>() where T : IClass;
    External<T>[] ReadArrayExternalNodeRef<T>(int length) where T : CMwNod;
    External<T>[] ReadArrayExternalNodeRef<T>() where T : CMwNod;
    External<T>[] ReadArrayExternalNodeRef_deprec<T>() where T : CMwNod;
    List<External<T>> ReadListExternalNodeRef<T>(int length) where T : CMwNod;
    List<External<T>> ReadListExternalNodeRef<T>() where T : CMwNod;
    List<External<T>> ReadListExternalNodeRef_deprec<T>() where T : CMwNod;
    T[] ReadArrayReadable<T>(int length, int version = 0) where T : IReadable, new();
    T[] ReadArrayReadable<T>(bool byteLengthPrefix = false, int version = 0) where T : IReadable, new();
    T[] ReadArrayReadable_deprec<T>(bool byteLengthPrefix = false, int version = 0) where T : IReadable, new();
    List<T> ReadListReadable<T>(int length, int version = 0) where T : IReadable, new();
    List<T> ReadListReadable<T>(bool byteLengthPrefix = false, int version = 0) where T : IReadable, new();
    List<T> ReadListReadable_deprec<T>(bool byteLengthPrefix = false, int version = 0) where T : IReadable, new();

    string[] ReadArrayId(int length);
    string[] ReadArrayId();
    string[] ReadArrayId_deprec();
    List<string> ReadListId(int length);
    List<string> ReadListId();
    List<string> ReadListId_deprec();

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

    private IReadOnlyDictionary<int, GbxRefTableNode>? refTable;
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

    internal GbxReadSettings Settings { get; }

    internal ILogger? Logger => logger;

    internal ClassIdRemapMode ClassIdRemapMode { get; set; }

    private GbxReaderLimiter? limiter;

    public GbxReader(Stream input, GbxReadSettings settings = default) : base(input, encoding, !settings.CloseStream)
    {
        Settings = settings;
        logger = settings.Logger;
    }

    public GbxReader(XmlReader input, GbxReadSettings settings = default) : base(Stream.Null, encoding)
    {
        xmlReader = input;

        Settings = settings;
        logger = settings.Logger;

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

    internal void LoadRefTable(IReadOnlyDictionary<int, GbxRefTableNode> refTable)
    {
        this.refTable = refTable;
    }

    public bool ReadGbxMagic()
    {
#if NETSTANDARD2_0
        return base.ReadByte() == 'G' && base.ReadByte() == 'B' && base.ReadByte() == 'X';
#else
        Span<byte> buffer = stackalloc byte[3];

        if (BaseStream.Read(buffer) != 3)
        {
            return false;
        }

        return buffer[0] == 'G' && buffer[1] == 'B' && buffer[2] == 'X';
#endif
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
            SerializationMode.Gbx => Format switch
            {
                GbxFormat.Binary => base.ReadInt16(),
                GbxFormat.Text => GbxText(),
                _ => throw new FormatNotSupportedException(Format)
            },
            _ => throw new SerializationModeNotSupportedException(Mode),
        };

        short GbxText()
        {
            var value = ReadToCRLF();
            return value == "4294967295" ? (short)-1 : short.Parse(value);
        }
    }

    public override int ReadInt32()
    {
        limiter?.ThrowIfLimitExceeded(sizeof(int));

        return Mode switch
        {
            SerializationMode.Gbx => Format switch
            {
                GbxFormat.Binary => base.ReadInt32(),
                GbxFormat.Text => GbxText(),
                _ => throw new FormatNotSupportedException(Format)
            },
            _ => throw new SerializationModeNotSupportedException(Mode),
        };

        int GbxText()
        {
            var value = ReadToCRLF();
            return value == "4294967295" ? -1 : int.Parse(value);
        }
    }

    public int ReadHexInt32()
    {
        limiter?.ThrowIfLimitExceeded(sizeof(int));

        return Mode switch
        {
            SerializationMode.Gbx => Format switch
            {
                GbxFormat.Binary => base.ReadInt32(),
                GbxFormat.Text => int.Parse(ReadToCRLF(), System.Globalization.NumberStyles.HexNumber),
                _ => throw new FormatNotSupportedException(Format)
            },
            _ => throw new SerializationModeNotSupportedException(Mode),
        };
    }

    public override uint ReadUInt32()
    {
        limiter?.ThrowIfLimitExceeded(sizeof(uint));

        return Mode switch
        {
            SerializationMode.Gbx => Format switch
            {
                GbxFormat.Binary => base.ReadUInt32(),
                GbxFormat.Text => uint.Parse(ReadToCRLF()),
                _ => throw new FormatNotSupportedException(Format)
            },
            _ => throw new SerializationModeNotSupportedException(Mode),
        };
    }

    public uint ReadHexUInt32()
    {
        limiter?.ThrowIfLimitExceeded(sizeof(uint));

        return Mode switch
        {
            SerializationMode.Gbx => Format switch
            {
                GbxFormat.Binary => base.ReadUInt32(),
                GbxFormat.Text => uint.Parse(ReadToCRLF(), System.Globalization.NumberStyles.HexNumber),
                _ => throw new FormatNotSupportedException(Format)
            },
            _ => throw new SerializationModeNotSupportedException(Mode),
        };
    }

    public int ReadDataInt32()
    {
        limiter?.ThrowIfLimitExceeded(sizeof(int));

        return Mode switch
        {
            SerializationMode.Gbx => Format switch
            {
                GbxFormat.Binary => ReadInt32(),
                GbxFormat.Text => base.ReadInt32(),
                _ => throw new FormatNotSupportedException(Format)
            },
            _ => throw new SerializationModeNotSupportedException(Mode),
        };
    }

    public uint ReadDataUInt32()
    {
        limiter?.ThrowIfLimitExceeded(sizeof(uint));

        return Mode switch
        {
            SerializationMode.Gbx => Format switch
            {
                GbxFormat.Binary => ReadUInt32(),
                GbxFormat.Text => base.ReadUInt32(),
                _ => throw new FormatNotSupportedException(Format)
            },
            _ => throw new SerializationModeNotSupportedException(Mode),
        };
    }

    public long ReadDataInt64()
    {
        limiter?.ThrowIfLimitExceeded(sizeof(long));

        return Mode switch
        {
            SerializationMode.Gbx => Format switch
            {
                GbxFormat.Binary => ReadInt64(),
                GbxFormat.Text => base.ReadInt64(),
                _ => throw new FormatNotSupportedException(Format)
            },
            _ => throw new SerializationModeNotSupportedException(Mode),
        };
    }

    public ulong ReadDataUInt64()
    {
        limiter?.ThrowIfLimitExceeded(sizeof(ulong));

        return Mode switch
        {
            SerializationMode.Gbx => Format switch
            {
                GbxFormat.Binary => ReadUInt64(),
                GbxFormat.Text => base.ReadUInt64(),
                _ => throw new FormatNotSupportedException(Format)
            },
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
            SerializationMode.Gbx => Format switch
            {
                GbxFormat.Binary => base.ReadSingle(),
                GbxFormat.Text => float.Parse(ReadToCRLF(), System.Globalization.CultureInfo.InvariantCulture),
                _ => throw new FormatNotSupportedException(Format)
            },
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

    public Vec3 ReadVec3_10b()
    {
        var val = ReadInt32();

        return new Vec3(
            (val & 0x3FF) / (float)0x1FF,
            ((val >> 10) & 0x3FF) / (float)0x1FF,
            ((val >> 20) & 0x3FF) / (float)0x1FF);
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

    public Rect ReadRect()
    {
        return new Rect(X: ReadSingle(),
                        Y: ReadSingle(),
                        X2: ReadSingle(),
                        Y2: ReadSingle());
    }

    public TransQuat ReadTransQuat()
    {
        return new TransQuat(TX: ReadSingle(),
                             TY: ReadSingle(),
                             TZ: ReadSingle(),
                             X: ReadSingle(),
                             Y: ReadSingle(),
                             Z: ReadSingle(),
                             W: ReadSingle());
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

        return Mode switch
        {
            SerializationMode.Gbx => Format switch
            {
                GbxFormat.Binary => GbxBinary(),
                GbxFormat.Text => bool.Parse(ReadToCRLF()),
                _ => throw new FormatNotSupportedException(Format)
            },
            _ => throw new SerializationModeNotSupportedException(Mode),
        };
        
        bool GbxBinary()
        {
            var booleanAsInt = base.ReadUInt32();

            if (Gbx.StrictBooleans && booleanAsInt > 1)
            {
                throw new BooleanOutOfRangeException(booleanAsInt);
            }

            return booleanAsInt != 0;
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
        limiter?.ThrowIfLimitExceeded(sizeof(int));

        return Mode switch
        {
            SerializationMode.Gbx => Format switch
            {
                GbxFormat.Binary => ReadString(base.ReadInt32()),
                GbxFormat.Text => ReadToCRLF(),
                _ => throw new FormatNotSupportedException(Format)
            },
            _ => throw new SerializationModeNotSupportedException(Mode),
        };
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
        switch (Mode)
        {
            case SerializationMode.Gbx:
                IdVersion ??= ReadInt32();

                if (IdVersion.Value < 3)
                {
                    throw new NotSupportedException($"Unsupported Id version ({IdVersion}).");
                }

                switch (Format)
                {
                    case GbxFormat.Binary:
                        return ReadHexUInt32();
                    case GbxFormat.Text:
                        var value = ReadToCRLF();
                        return value == "4294967295"
                            ? uint.MaxValue
                            : uint.Parse(value, System.Globalization.NumberStyles.HexNumber);
                    default:
                        throw new FormatNotSupportedException(Format);
                }
            default:
                throw new SerializationModeNotSupportedException(Mode);
        }
    }

    private string ReadIdAsString(uint index)
    {
        if ((index & 0x3FFFFFFF) != 0)
        {
            if (Gbx.StrictIdIndices)
            {
                return IdDict[index];
            }

            return IdDict.TryGetValue(index, out var s) ? s : string.Empty;
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
                logger?.LogDebug("NodeRef #{Index}: {ExistingNode} (existing)", index.Value, existingNode);

                file = null;
                return (T)existingNode;
            }

            if (refTable?.TryGetValue(index.Value, out var externalNode) == true)
            {
                logger?.LogDebug("NodeRef #{Index}: {ExternalNode} (external)", index.Value, externalNode);

                file = externalNode as GbxRefTableFile;
                return default;
            }
        }

        file = null;

        var rawClassId = ReadHexUInt32();

        if (encapsulation is not null && rawClassId == uint.MaxValue)
        {
            return default;
        }

        var classId = ClassManager.Wrap(rawClassId);

        if (logger is not null)
        {
            if (classId == rawClassId)
            {
                logger.LogDebug("NodeRef #{Index}: 0x{ClassId:X8} ({ClassName})", index, classId, ClassManager.GetName(classId));
            }
            else
            {
                logger.LogDebug("NodeRef #{Index}: 0x{ClassId:X8} ({ClassName}, raw: 0x{RawClassId:X8})", index, classId, ClassManager.GetName(classId), rawClassId);
            }
        }

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
            if (logger is not null && NodeDict.TryGetValue(index.Value, out var existingNode))
            {
                logger.LogWarning("NodeRef #{Index}: {ExistingNode} (existing was overwriten!)", index.Value, existingNode);
            }

            NodeDict[index.Value] = nod;
        }

        return ReadNode(nod);
    }

    [IgnoreForCodeGeneration]
    public IClass? ReadNodeRef(out GbxRefTableFile? file)
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
                logger?.LogDebug("NodeRef #{Index}: {ExistingNode} (existing)", index.Value, existingNode);

                file = null;
                return (IClass)existingNode;
            }

            if (refTable?.TryGetValue(index.Value, out var externalNode) == true)
            {
                logger?.LogDebug("NodeRef #{Index}: {ExternalNode} (external)", index.Value, externalNode);

                file = externalNode as GbxRefTableFile;
                return default;
            }
        }

        file = null;

        var rawClassId = ReadUInt32();

        if (encapsulation is not null && rawClassId == uint.MaxValue)
        {
            return default;
        }

        var classId = ClassManager.Wrap(rawClassId);

        if (logger is not null)
        {
            if (classId == rawClassId)
            {
                logger.LogDebug("NodeRef #{Index}: 0x{ClassId:X8} ({ClassName})", index, classId, ClassManager.GetName(classId));
            }
            else
            {
                logger.LogDebug("NodeRef #{Index}: 0x{ClassId:X8} ({ClassName}, raw: 0x{RawClassId:X8})", index, classId, ClassManager.GetName(classId), rawClassId);
            }
        }

        var node = ClassManager.New(classId) ?? throw new Exception($"Unknown class ID: 0x{classId:X8} ({ClassManager.GetName(classId) ?? "unknown class name"})");

        if (index.HasValue)
        {
            if (logger is not null && NodeDict.TryGetValue(index.Value, out var existingNode))
            {
                logger.LogWarning("NodeRef #{Index}: {ExistingNode} (existing was overwriten!)", index.Value, existingNode);
            }

            NodeDict[index.Value] = node;
        }

        return ReadNode(node);
    }

    public T? ReadNodeRef<T>() where T : IClass
    {
        var node = ReadNodeRef<T>(out var file);

        if (file is not null)
        {
            logger?.LogWarning("Reference table file discard: {File} {StackTrace}", file, Environment.StackTrace);
        }

        return node;
    }

    [IgnoreForCodeGeneration]
    public IClass? ReadNodeRef()
    {
        var node = ReadNodeRef(out var file);

        if (file is not null)
        {
            logger?.LogWarning("Reference table file discard: {File} {StackTrace}", file, Environment.StackTrace);
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
        rw ??= new GbxReaderWriter(this);

        using var _ = logger?.BeginScope("{ClassName} (aux)", ClassManager.GetName(node.GetType()));

#if NET8_0_OR_GREATER
        T.Read(node, rw);
#else
        node.ReadWrite(rw);
#endif

        return node;
    }

    [IgnoreForCodeGeneration]
    private IClass ReadNode(IClass node)
    {
        rw ??= new GbxReaderWriter(this);

        using var _ = logger?.BeginScope("{ClassName} (aux)", ClassManager.GetName(node.GetType()));

        node.ReadWrite(rw);

        return node;
    }

    public T? ReadMetaRef<T>() where T : IClass
    {
        var rawClassId = ReadUInt32();

        if (rawClassId == uint.MaxValue)
        {
            return default;
        }

        var classId = ClassManager.Wrap(rawClassId);

#if NET8_0_OR_GREATER
        var node = T.New(classId) ?? throw new Exception($"Unknown class ID: 0x{classId:X8} ({ClassManager.GetName(classId) ?? "unknown class name"})");
#else
        var node = ClassManager.New(classId) ?? throw new Exception($"Unknown class ID: 0x{classId:X8} ({ClassManager.GetName(classId) ?? "unknown class name"})");
#endif

        if (node is not T metaNod)
        {
            throw new InvalidCastException($"Class ID 0x{classId:X8} ({ClassManager.GetName(classId) ?? "unknown class name"}) cannot be casted to {typeof(T).Name}.");
        }

        return ReadNode(metaNod);
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

    public DateTime ReadSystemTime()
    {
        return new DateTime(ReadInt64());
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

    public Vec3[] ReadArrayVec3_10b(int length)
    {
        if (length == 0)
        {
            return [];
        }
        
        ValidateCollectionLength(length);
        
        var array = new Vec3[length];
        
        for (int i = 0; i < length; i++)
        {
            array[i] = ReadVec3_10b();
        }
        
        return array;
    }

    public Vec3[] ReadArrayVec3_10b()
    {
        return ReadArrayVec3_10b(ReadInt32());
    }

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

    public List<T> ReadListReadable<T>(int length, int version = 0) where T : IReadable, new()
    {
        if (length == 0)
        {
            return [];
        }

        ValidateCollectionLength(length);

        var list = new List<T>(length);

        for (int i = 0; i < length; i++)
        {
            list.Add(ReadReadable<T>(version));
        }

        return list;
    }

    public List<T> ReadListReadable<T>(bool byteLengthPrefix = false, int version = 0) where T : IReadable, new()
        => ReadListReadable<T>(byteLengthPrefix ? ReadByte() : ReadInt32(), version);

    public List<T> ReadListReadable_deprec<T>(bool byteLengthPrefix = false, int version = 0) where T : IReadable, new()
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

    public List<T> ReadList<T>(int length, bool lengthInBytes = false) where T : struct
    {
        if (length == 0)
        {
            return [];
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

    public List<T> ReadList<T>(bool lengthInBytes = false) where T : struct => ReadList<T>(ReadInt32(), lengthInBytes);

    public List<T> ReadList_deprec<T>(bool lengthInBytes = false) where T : struct
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

    public List<T?> ReadListNodeRef<T>(int length) where T : IClass
    {
        if (length == 0)
        {
            return [];
        }

        ValidateCollectionLength(length);

        var list = new List<T?>(length);

        for (var i = 0; i < length; i++)
        {
            list.Add(ReadNodeRef<T>());
        }

        return list;
    }

    public List<T?> ReadListNodeRef<T>() where T : IClass => ReadListNodeRef<T>(ReadInt32());

    public List<T?> ReadListNodeRef_deprec<T>() where T : IClass
    {
        ReadDeprecVersion();
        return ReadListNodeRef<T>();
    }

    public External<T>[] ReadArrayExternalNodeRef<T>(int length) where T : CMwNod
    {
        if (length == 0)
        {
            return [];
        }

        ValidateCollectionLength(length);

        var array = new External<T>[length];

        for (var i = 0; i < length; i++)
        {
            var node = ReadNodeRef<T>(out var file);
            array[i] = new(node, file);
        }

        return array;
    }

    public External<T>[] ReadArrayExternalNodeRef<T>() where T : CMwNod => ReadArrayExternalNodeRef<T>(ReadInt32());

    public External<T>[] ReadArrayExternalNodeRef_deprec<T>() where T : CMwNod
    {
        ReadDeprecVersion();
        return ReadArrayExternalNodeRef<T>();
    }

    public List<External<T>> ReadListExternalNodeRef<T>(int length) where T : CMwNod
    {
        if (length == 0)
        {
            return [];
        }

        ValidateCollectionLength(length);

        var list = new List<External<T>>(length);

        for (var i = 0; i < length; i++)
        {
            var node = ReadNodeRef<T>(out var file);
            list.Add(new(node, file));
        }

        return list;
    }

    public List<External<T>> ReadListExternalNodeRef<T>() where T : CMwNod => ReadListExternalNodeRef<T>(ReadInt32());

    public List<External<T>> ReadListExternalNodeRef_deprec<T>() where T : CMwNod
    {
        ReadDeprecVersion();
        return ReadListExternalNodeRef<T>();
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

    public List<string> ReadListId(int length)
    {
        if (length == 0)
        {
            return [];
        }

        ValidateCollectionLength(length);

        var list = new List<string>(length);

        for (var i = 0; i < length; i++)
        {
            list.Add(ReadIdAsString());
        }

        return list;
    }

    public List<string> ReadListId() => ReadListId(ReadInt32());

    public List<string> ReadListId_deprec()
    {
        ReadDeprecVersion();
        return ReadListId();
    }

    internal (Vec3 position, Quat rotation, float speed, Vec3 velocity) ReadTransform()
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

        for (var i = 0; i < 255; i++)
        {
            var b = base.ReadByte();

            if (b == 0x0D)
            {
                if (base.ReadByte() != 0x0A)
                {
                    throw new Exception("Invalid string format.");
                }

                return sb.ToString();
            }

            sb.Append((char)b);
        }

        throw new Exception("String is too long.");
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

    internal BinaryScope ForceBinary() => new(this);

    internal readonly struct BinaryScope : IDisposable
    {
        private readonly GbxReader reader;
        private readonly GbxFormat format;

        public BinaryScope(GbxReader reader)
        {
            this.reader = reader;
            format = reader.Format;
            reader.Format = GbxFormat.Binary;
        }

        public void Dispose()
        {
            reader.Format = format;
        }
    }
}