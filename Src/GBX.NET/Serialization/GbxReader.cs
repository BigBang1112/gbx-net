using GBX.NET.Components;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
using System.Xml;

namespace GBX.NET.Serialization;

/// <summary>
/// A binary/text reader specialized for Gbx.
/// </summary>
public interface IGbxReader : IDisposable
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
    Byte3 ReadByte3();
    Vec2 ReadVec2();
    Vec3 ReadVec3();
    Vec4 ReadVec4();
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
    T ReadNodeRef<T>() where T : IClass;
    TimeInt32 ReadTimeInt32();
    TimeSingle ReadTimeSingle();
    TimeSpan? ReadTimeOfDay();

    T[] ReadArray<T>(int length, bool lengthInBytes = false) where T : struct;
    T[] ReadArray<T>(bool lengthInBytes = false) where T : struct;
    T[] ReadArray_deprec<T>(int length, bool lengthInBytes = false) where T : struct;
    T[] ReadArray_deprec<T>(bool lengthInBytes = false) where T : struct;
    IList<T> ReadList<T>(int length, bool lengthInBytes = false) where T : struct;
    IList<T> ReadList<T>(bool lengthInBytes = false) where T : struct;
    IList<T> ReadList_deprec<T>(int length, bool lengthInBytes = false) where T : struct;
    IList<T> ReadList_deprec<T>(bool lengthInBytes = false) where T : struct;
    T[] ReadArrayNode<T>(int length) where T : IClass;
    T[] ReadArrayNode<T>() where T : IClass;
    T[] ReadArrayNode_deprec<T>(int length) where T : IClass;
    T[] ReadArrayNode_deprec<T>() where T : IClass;
    IList<T> ReadListNode<T>(int length) where T : IClass;
    IList<T> ReadListNode<T>() where T : IClass;
    IList<T> ReadListNode_deprec<T>(int length) where T : IClass;
    IList<T> ReadListNode_deprec<T>() where T : IClass;

    PackDesc[] ReadArrayPackDesc(int length);
    PackDesc[] ReadArrayPackDesc();
    IList<PackDesc> ReadListPackDesc(int length);
    IList<PackDesc> ReadListPackDesc();

    void SkipData(int length);
    byte[] ReadToEnd();
    void ResetIdState();
}

/// <summary>
/// A binary/text reader specialized for Gbx.
/// </summary>
internal sealed class GbxReader : BinaryReader, IGbxReader
{
    internal const int MaxDataSize = 0x10000000; // ~268MB

    private static readonly Encoding encoding = Encoding.UTF8;

#if NET6_0_OR_GREATER
    private string? prevString;
#endif

    private bool enablePreviousStringCache;
    public bool EnablePreviousStringCache { get => enablePreviousStringCache; set => enablePreviousStringCache = value; }

    private readonly GbxRefTable? refTable;
    private readonly XmlReader? xmlReader;

    private int? idVersion;
    private Dictionary<int, string>? idDict;
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

    internal Encapsulation? Encapsulation { get => encapsulation; set => encapsulation = value; }

    internal byte? PackDescVersion { get => packDescVersion; set => packDescVersion = value; }
    internal int? DeprecVersion { get => deprecVersion; set => deprecVersion = value; }

    public SerializationMode Mode { get; }
    public GbxFormat Format { get; private set; }

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

    public GbxFormat ReadFormatByte()
    {
        Format = (GbxFormat)ReadByte();
        return Format;
    }

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

        if ((index & 0xC0000000) is not 0x40000000 and not 0x80000000)
        {
            throw new NotSupportedException("This Id cannot be read as string.");
        }

        return ReadIdAsString(index);
    }

    public Id ReadId()
    {
        var index = ReadIdIndex();

        if ((index & 0xC0000000) is not 0x40000000 and not 0x80000000)
        {
            return new(index);
        }

        return new(ReadIdAsString(index));
    }

    private int ReadIdIndex()
    {
        switch (Mode)
        {
            case SerializationMode.Gbx:
                IdVersion ??= ReadInt32();

                if (IdVersion < 3)
                {
                    throw new NotSupportedException($"Unsupported Id version ({IdVersion}).");
                }

                return ReadInt32();
            default:
                throw new SerializationModeNotSupportedException(Mode);
        }
        
    }

    private string ReadIdAsString(int index)
    {
        if ((index & 0xFFFFFFF) != 0)
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

    public T ReadNodeRef<T>() where T : IClass
    {
        throw new NotImplementedException();
    }

    public TimeInt32 ReadTimeInt32()
    {
        return new(ReadInt32());
    }

    public TimeSingle ReadTimeSingle()
    {
        return new(ReadSingle());
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

    private void ReadDeprecVersion()
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

    public IList<T> ReadList_deprec<T>(int length, bool lengthInBytes = false) where T : struct
    {
        ReadDeprecVersion();
        return ReadList<T>(length, lengthInBytes);
    }

    public IList<T> ReadList_deprec<T>(bool lengthInBytes = false) where T : struct => ReadList_deprec<T>(ReadInt32(), lengthInBytes);

    public T[] ReadArrayNode<T>(int length) where T : IClass
    {
        if (length == 0)
        {
            return [];
        }

        ValidateCollectionLength(length);

        var array = new T[length];

        for (var i = 0; i < length; i++)
        {
            array[i] = ReadNodeRef<T>();
        }

        return array;
    }

    public T[] ReadArrayNode<T>() where T : IClass => ReadArrayNode<T>(ReadInt32());

    public T[] ReadArrayNode_deprec<T>(int length) where T : IClass
    {
        ReadDeprecVersion();
        return ReadArrayNode<T>(length);
    }

    public T[] ReadArrayNode_deprec<T>() where T : IClass => ReadArrayNode_deprec<T>(ReadInt32());

    public IList<T> ReadListNode<T>(int length) where T : IClass
    {
        if (length == 0)
        {
            return new List<T>();
        }

        ValidateCollectionLength(length);

        var list = new List<T>(length);

        for (var i = 0; i < length; i++)
        {
            list[i] = ReadNodeRef<T>();
        }

        return list;
    }

    public IList<T> ReadListNode<T>() where T : IClass => ReadListNode<T>(ReadInt32());

    public IList<T> ReadListNode_deprec<T>(int length) where T : IClass
    {
        ReadDeprecVersion();
        return ReadListNode<T>(length);
    }

    public IList<T> ReadListNode_deprec<T>() where T : IClass => ReadListNode_deprec<T>(ReadInt32());

    /// <summary>
    /// If can seek, position moves past the <paramref name="length"/>. If seeking is NOT supported, data is read with no allocation using <see cref="BinaryReader.Read(Span{byte})"/>. If .NET Standard 2.0, unavoidable byte array allocation happens with <see cref="BinaryReader.ReadBytes(int)"/>.
    /// </summary>
    /// <param name="length">Length in bytes to skip.</param>
    /// <exception cref="EndOfStreamException"></exception>
    public void SkipData(int length)
    {
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

    public PackDesc[] ReadArrayPackDesc(int length)
    {
        var array = new PackDesc[length];

        for (var i = 0; i < length; i++)
        {
            array[i] = ReadPackDesc();
        }

        return array;
    }

    public PackDesc[] ReadArrayPackDesc() => ReadArrayPackDesc(ReadInt32());

    public IList<PackDesc> ReadListPackDesc(int length)
    {
        var list = new List<PackDesc>(length);

        for (var i = 0; i < length; i++)
        {
            list.Add(ReadPackDesc());
        }

        return list;
    }

    public IList<PackDesc> ReadListPackDesc() => ReadListPackDesc(ReadInt32());
}