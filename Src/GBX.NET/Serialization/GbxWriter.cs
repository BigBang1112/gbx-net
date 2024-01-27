using System.Numerics;
using System.Text;
using System.Xml;

#if NET6_0_OR_GREATER
using System.Buffers;
#endif

namespace GBX.NET.Serialization;

/// <summary>
/// A binary/text writer specialized for Gbx.
/// </summary>
public partial interface IGbxWriter : IDisposable
{
    Stream BaseStream { get; }
    SerializationMode Mode { get; }
    GbxFormat Format { get; }

    void Write(byte value);
    void Write(sbyte value);
    void Write(short value);
    void Write(ushort value);
    void Write(int value);
    void Write(uint value);
    void Write(long value);
    void Write(ulong value);
    void Write(float value);
    void Close();

    void WriteFormat(GbxFormat format);
    void WriteHexInt32(int value);
    void WriteHexUInt32(uint value);
    void Write(bool value);
    void Write(bool value, bool asByte);
    void Write(string? value);
    void Write(string? value, StringLengthPrefix lengthPrefix);
    void WriteGbxMagic();
    void WriteBigInt(BigInteger value, int byteLength);
    void WriteInt128(BigInteger value);
    void Write(Int2 value);
    void Write(Int3 value);
    void Write(Int4 value);
    void Write(Byte3 value);
    void Write(Vec2 value);
    void Write(Vec3 value);
    void Write(Vec4 value);
    void Write(Box value);
    void Write(Color value);
    void Write(Iso4 value);
    void Write(Mat3 value);
    void Write(Mat4 value);
    void Write(Quat value);
    void Write(Rect value);
    void Write(Id value);
    void WriteIdAsString(string? value);
    void Write(Ident? value);
    void Write(PackDesc? value);
    void WriteNodeRef<T>(T? value) where T : IClass;
    void WriteNode<T>(T? value) where T : IClass;
    void Write(TimeInt32 value);
    void WriteTimeInt32Nullable(TimeInt32? value);
    void Write(TimeSingle value);
    void WriteTimeSingleNullable(TimeSingle? value);
    void WriteTimeOfDay(TimeSpan? value);
    void WriteMarker(string value);

    void Write(byte[]? value);
    void WriteData(byte[]? value);
    void WriteData(byte[]? value, int length);

    void WriteArray<T>(T[]? value, bool lengthInBytes = false) where T : struct;
    void WriteArray<T>(T[]? value, int length, bool lengthInBytes = false) where T : struct;
    void WriteArray_deprec<T>(T[]? value, bool lengthInBytes = false) where T : struct;
    void WriteArray_deprec<T>(T[]? value, int length, bool lengthInBytes = false) where T : struct;
    void WriteList<T>(IList<T>? value, bool lengthInBytes = false) where T : struct;
    void WriteList<T>(IList<T>? value, int length, bool lengthInBytes = false) where T : struct;
    void WriteList_deprec<T>(IList<T>? value, bool lengthInBytes = false) where T : struct;
    void WriteList_deprec<T>(IList<T>? value, int length, bool lengthInBytes = false) where T : struct;
    void WriteArrayNodeRef<T>(T?[]? value) where T : IClass;
    void WriteArrayNodeRef<T>(T?[]? value, int length) where T : IClass;
    void WriteArrayNodeRef_deprec<T>(T?[]? value) where T : IClass;
    void WriteListNodeRef<T>(IList<T?>? value) where T : IClass;
    void WriteListNodeRef<T>(IList<T?>? value, int length) where T : IClass;
    void WriteListNodeRef_deprec<T>(IList<T?>? value) where T : IClass;

    void WriteArrayId(string[]? value);
    void WriteArrayId(string[]? value, int length);
    void WriteArrayId_deprec(string[]? value);
    void WriteListId(IList<string>? value);
    void WriteListId(IList<string>? value, int length);
    void WriteListId_deprec(IList<string>? value);

    void ResetIdState();
}

/// <summary>
/// A binary/text writer specialized for Gbx.
/// </summary>
internal sealed partial class GbxWriter : BinaryWriter, IGbxWriter
{
    private static readonly Encoding encoding = Encoding.UTF8;

    private readonly XmlWriter? xmlWriter;

    private const int IdVersionToWrite = 3;

    private int? idVersion;
    private Dictionary<string, int>? idDict;
    private Encapsulation? encapsulation;

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

    internal Dictionary<string, int> IdDict => encapsulation is null
        ? idDict ??= []
        : encapsulation.IdWriteDict;

    internal Encapsulation? Encapsulation { get => encapsulation; set => encapsulation = value; }

    internal byte PackDescVersion { get; set; } = 3;
    internal int DeprecVersion { get; set; } = 10;

    public SerializationMode Mode { get; }
    public GbxFormat Format { get; private set; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="output"></param>
    public GbxWriter(Stream output) : base(output, encoding)
    {
    }

    public GbxWriter(Stream output, bool leaveOpen) : base(output, encoding, leaveOpen)
    {
    }

    public GbxWriter(XmlWriter output) : base(Stream.Null, encoding)
    {
        xmlWriter = output;
        Mode = SerializationMode.Xml;
    }

    public void WriteGbxMagic()
    {
        switch (Mode)
        {
            case SerializationMode.Gbx:
                Write((byte)'G');
                Write((byte)'B');
                Write((byte)'X');
                break;
            case SerializationMode.Xml:
                xmlWriter?.WriteElementString("Magic", "GBX");
                break;
            default:
                throw new SerializationModeNotSupportedException(Mode);
        }
    }

    public void WriteFormat(GbxFormat format)
    {
        Write((byte)format);
        Format = format;
    }

    /// <summary>
    /// Writes a boolean to this stream. An integer is written to the stream with the value 0 representing false or the value 1 representing true.
    /// </summary>
    /// <param name="value"></param>
    public override void Write(bool value)
    {
        Write(value, asByte: false);
    }

    public void Write(bool value, bool asByte)
    {
        if (asByte)
        {
            base.Write(value);
        }
        else
        {
            Write(Convert.ToInt32(value));
        }
    }

    public void Write(string? value, StringLengthPrefix lengthPrefix)
    {
        switch (lengthPrefix)
        {
            case StringLengthPrefix.Byte:

                if (value is null || value == string.Empty)
                {
                    Write((byte)0);
                    return;
                }

                if (value.Length > 255)
                {
                    throw new LengthLimitException(value.Length);
                }

#if NET6_0_OR_GREATER
                Span<byte> buffer = stackalloc byte[1024];

                var actualByteCount = encoding.GetBytes(value, buffer.Slice(1));

                if (actualByteCount > 255)
                {
                    throw new LengthLimitException(actualByteCount);
                }

                buffer[0] = (byte)actualByteCount;

                OutStream.Write(buffer.Slice(0, actualByteCount + 1));
#else
                Write(encoding.GetBytes(value));
#endif

                return;
            case StringLengthPrefix.Int32:

                if (string.IsNullOrEmpty(value))
                {
                    Write(0);
                    return;
                }

                var length = encoding.GetByteCount(value);

                if (length > 0x10000000) // ~268MB
                {
                    throw new LengthLimitException(length);
                }

                Write(length);

                if (length == 0)
                {
                    return;
                }

#if NET6_0_OR_GREATER
                if (length < 128)
                {
                    buffer = stackalloc byte[127];
                    actualByteCount = encoding.GetBytes(value, buffer);
                    OutStream.Write(buffer[..actualByteCount]);
                    return;
                }

                if (length < ushort.MaxValue)
                {
                    var rented = ArrayPool<byte>.Shared.Rent(value.Length * 3); // max expansion: each char -> 3 bytes
                    actualByteCount = encoding.GetBytes(value, rented);
                    OutStream.Write(rented, 0, actualByteCount);
                    ArrayPool<byte>.Shared.Return(rented);
                    return;
                }
#endif

                Write(encoding.GetBytes(value));

                return;
            case StringLengthPrefix.None:
                if (value is null)
                {
                    return;
                }
    
                // TODO: Optimize
                Write(encoding.GetBytes(value));
    
                return;
            default:
                throw new ArgumentException("Invalid length prefix.", nameof(lengthPrefix));
        }
    }

    public override void Write(string? value)
    {
        Write(value, StringLengthPrefix.Int32);
    }

    public void WriteHexInt32(int value)
    {
        switch (Mode)
        {
            case SerializationMode.Gbx:
                Write(value);
                break;
            default:
                throw new SerializationModeNotSupportedException(Mode);
        }
    }

    public void WriteHexUInt32(uint value)
    {
        switch (Mode)
        {
            case SerializationMode.Gbx:
                Write(value);
                break;
            default:
                throw new SerializationModeNotSupportedException(Mode);
        }
    }

    public void WriteBigInt(BigInteger value, int byteLength)
    {
        var bytes = value.ToByteArray();
        
        if (bytes.Length == byteLength)
        {
            // No padding necessary
            Write(bytes);
            return;
        }

        if (bytes.Length > byteLength)
        {
            throw new ArgumentException($"Value too large to fit in {byteLength} bytes");
        }

        // Pad with leading zeros (edit dec23: wtf why allocate array)
        var paddedBytes = new byte[byteLength];
        Array.Copy(bytes, 0, paddedBytes, byteLength - bytes.Length, bytes.Length);

        Write(paddedBytes);
    }

    public void WriteInt128(BigInteger value)
    {
        WriteBigInt(value, byteLength: 16);
    }

    public void Write(Int2 value)
    {
        Write(value.X);
        Write(value.Y);
    }

    public void Write(Int3 value)
    {
        Write(value.X);
        Write(value.Y);
        Write(value.Z);
    }

    public void Write(Int4 value)
    {
        Write(value.X);
        Write(value.Y);
        Write(value.Z);
        Write(value.W);
    }

    public void Write(Byte3 value)
    {
        Write(value.X);
        Write(value.Y);
        Write(value.Z);
    }

    public void Write(Vec2 value)
    {
        Write(value.X);
        Write(value.Y);
    }

    public void Write(Vec3 value)
    {
        Write(value.X);
        Write(value.Y);
        Write(value.Z);
    }

    public void Write(Vec4 value)
    {
        Write(value.X);
        Write(value.Y);
        Write(value.Z);
        Write(value.W);
    }

    public void Write(Box value)
    {
        Write(value.X);
        Write(value.Y);
        Write(value.Z);
        Write(value.X2);
        Write(value.Y2);
        Write(value.Z2);
    }

    public void Write(Color value)
    {
        Write(value.R);
        Write(value.G);
        Write(value.B);
        Write(value.A);
    }

    public void Write(Iso4 value)
    {
        Write(value.XX);
        Write(value.XY);
        Write(value.XZ);
        Write(value.YX);
        Write(value.YY);
        Write(value.YZ);
        Write(value.ZX);
        Write(value.ZY);
        Write(value.ZZ);
        Write(value.TX);
        Write(value.TY);
        Write(value.TZ);
    }

    public void Write(Mat3 value)
    {
        Write(value.XX);
        Write(value.XY);
        Write(value.XZ);
        Write(value.YX);
        Write(value.YY);
        Write(value.YZ);
        Write(value.ZX);
        Write(value.ZY);
        Write(value.ZZ);
    }

    public void Write(Mat4 value)
    {
        Write(value.XX);
        Write(value.XY);
        Write(value.XZ);
        Write(value.XW);
        Write(value.YX);
        Write(value.YY);
        Write(value.YZ);
        Write(value.YW);
        Write(value.ZX);
        Write(value.ZY);
        Write(value.ZZ);
        Write(value.ZW);
        Write(value.WX);
        Write(value.WY);
        Write(value.WZ);
        Write(value.WW);
    }

    public void Write(Quat value)
    {
        Write(value.X);
        Write(value.Y);
        Write(value.Z);
        Write(value.W);
    }

    public void Write(Rect value)
    {
        Write(value.X);
        Write(value.Y);
        Write(value.X2);
        Write(value.Y2);
    }

    public void Write(Id value)
    {
        WriteIdVersionIfNotWritten();
        WriteIdWithoutVersion(value);
    }

    public void WriteIdAsString(string? value)
    {
        WriteIdVersionIfNotWritten();
        WriteIdAsStringWithoutVersion(value);
    }

    private void WriteIdVersionIfNotWritten()
    {
        if (IdVersion is null)
        {
            IdVersion = IdVersionToWrite;
            Write(IdVersion.Value);
        }
    }

    private void WriteIdWithoutVersion(Id value)
    {
        if (value.Index.HasValue)
        {
            Write(value.Index.Value);
            return;
        }

        WriteIdAsStringWithoutVersion(value.String);
    }

    private void WriteIdAsStringWithoutVersion(string? value)
    {
        if (value is null or "")
        {
            Write(0xFFFFFFFF);
            return;
        }

        if (IdDict.TryGetValue(value, out var index))
        {
            Write(index + 1 + 0x40000000);
            return;
        }

        /*if (tryParseToInt32 && int.TryParse(value, out index))
        {
            Write(index);
            return;
        }*/

        Write(0x40000000);
        Write(value);

        IdDict.Add(value, IdDict.Count);
    }

    public void Write(Ident? value)
    {
        if (value is null)
        {
            WriteIdVersionIfNotWritten();
            Write(0xFFFFFFFF);
            Write(0xFFFFFFFF);
            Write(0xFFFFFFFF);
            return;
        }

        WriteIdAsString(value.Id);
        WriteIdWithoutVersion(value.Collection);
        WriteIdAsStringWithoutVersion(value.Author);
    }

    public void Write(PackDesc? value)
    {
        Write(PackDescVersion);

        if (PackDescVersion >= 3)
        {
#if NET6_0_OR_GREATER
            Write(value?.Checksum ?? stackalloc byte[32]);
#else
            Write(value?.Checksum ?? new byte[32]);
#endif
        }

        Write(value?.FilePath);

        if (value?.FilePath is not null
            && ((value.FilePath.Length > 0 && PackDescVersion >= 1)
                || PackDescVersion >= 3))
        {
            Write(value.LocatorUrl);
        }
    }

    public void WriteNodeRef<T>(T? value) where T : IClass
    {
        throw new NotImplementedException();
    }

    public void WriteNode<T>(T? value) where T : IClass
    {
        throw new NotImplementedException();
    }

    public void Write(TimeInt32 value)
    {
        Write(value.TotalMilliseconds);
    }

    public void WriteTimeInt32Nullable(TimeInt32? value)
    {
        Write(value.HasValue ? value.Value.TotalMilliseconds : -1);
    }

    public void Write(TimeSingle value)
    {
        Write(value.TotalSeconds);
    }

    public void WriteTimeSingleNullable(TimeSingle? value)
    {
        Write(value.HasValue ? value.Value.TotalSeconds : -1);
    }

    public void WriteTimeOfDay(TimeSpan? value)
    {
        if (value is null)
        {
            Write(-1);
            return;
        }

        var maxTime = TimeSpan.FromDays(1) - TimeSpan.FromSeconds(1);
        var maxSecs = maxTime.TotalSeconds;
        var secs = value.Value.TotalSeconds % maxTime.TotalSeconds;

        Write(Convert.ToInt32(secs / maxSecs * ushort.MaxValue));
    }

    public void WriteMarker(string value)
    {
        Write(value, StringLengthPrefix.None);
    }

    public override void Write(byte[]? value)
    {
        if (value is not null)
        {
            base.Write(value);
        }
    }

    /*internal void WriteArray<T>(T[]? array, bool noPrefix = false) where T : struct
    {
        if (!noPrefix)
        {
            if (array is null)
            {
                Write(0);
                return;
            }

            Write(array.Length);
        }
        else if (array is null)
        {
            return;
        }

        if (array.Length > 0x10000000) // ~268MB
        {
            throw new LengthLimitException(array.Length);
        }

#if NET6_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
        Write(MemoryMarshal.Cast<T, byte>(array));
#else
        Write(MemoryMarshal.Cast<T, byte>(array).ToArray());
#endif
    }*/

    private void WriteDeprecVersion()
    {
        Write(DeprecVersion);
    }

    /*internal void WriteArray_deprec<T>(T[]? array, bool lengthInBytes = false) where T : struct
    {
        WriteDeprecVersion();
        WriteArray(array, lengthInBytes);
    }*/

    public void ResetIdState()
    {
        IdVersion = null;
        IdDict.Clear();
    }

    public void WriteData(byte[]? value, int length)
    {
        if (value is null)
        {
#if NET6_0_OR_GREATER
            Write(stackalloc byte[length]);
#else
            Write(new byte[length]);
#endif
            return;
        }

        if (value.Length > length)
        {
            Write(value, 0, length);
            return;
        }

        Write(value);

        if (value.Length == length)
        {
            return;
        }

#if NET6_0_OR_GREATER
        Write(stackalloc byte[length - value.Length]);
#else
        Write(new byte[length - value.Length]);
#endif
    }

    public void WriteData(byte[]? value)
    {
        if (value is null)
        {
            Write(0);
            return;
        }

        Write(value.Length);
        Write(value);
    }

    public void WriteArray<T>(T[]? value, bool lengthInBytes = false) where T : struct
    {
        throw new NotImplementedException();
    }

    public void WriteArray<T>(T[]? value, int length, bool lengthInBytes = false) where T : struct
    {
        throw new NotImplementedException();
    }

    public void WriteArray_deprec<T>(T[]? value, bool lengthInBytes = false) where T : struct
    {
        throw new NotImplementedException();
    }

    public void WriteArray_deprec<T>(T[]? value, int length, bool lengthInBytes = false) where T : struct
    {
        throw new NotImplementedException();
    }

    public void WriteList<T>(IList<T>? value, bool lengthInBytes = false) where T : struct
    {
        throw new NotImplementedException();
    }

    public void WriteList<T>(IList<T>? value, int length, bool lengthInBytes = false) where T : struct
    {
        throw new NotImplementedException();
    }

    public void WriteList_deprec<T>(IList<T>? value, bool lengthInBytes = false) where T : struct
    {
        throw new NotImplementedException();
    }

    public void WriteList_deprec<T>(IList<T>? value, int length, bool lengthInBytes = false) where T : struct
    {
        throw new NotImplementedException();
    }

    public void WriteArrayNodeRef<T>(T?[]? value) where T : IClass
    {
        throw new NotImplementedException();
    }

    public void WriteArrayNodeRef<T>(T?[]? value, int length) where T : IClass
    {
        throw new NotImplementedException();
    }

    public void WriteArrayNodeRef_deprec<T>(T?[]? value) where T : IClass
    {
        throw new NotImplementedException();
    }

    public void WriteListNodeRef<T>(IList<T?>? value) where T : IClass
    {
        throw new NotImplementedException();
    }

    public void WriteListNodeRef<T>(IList<T?>? value, int length) where T : IClass
    {
        throw new NotImplementedException();
    }

    public void WriteListNodeRef_deprec<T>(IList<T?>? value) where T : IClass
    {
        throw new NotImplementedException();
    }

    public void WriteArrayId(string[]? value)
    {
        if (value is null)
        {
            Write(0);
            return;
        }

        Write(value.Length);

        foreach (var item in value)
        {
            WriteIdAsString(item);
        }
    }

    public void WriteArrayId(string[]? value, int length)
    {
        if (value is null) return;

        for (var i = 0; i < length; i++)
        {
            WriteIdAsString(value[i]);

            if (i >= value.Length)
            {
                WriteIdAsString(default);
            }
        }
    }

    public void WriteArrayId_deprec(string[]? value)
    {
        WriteDeprecVersion();
        WriteArrayId(value);
    }

    public void WriteListId(IList<string>? value)
    {
        if (value is null)
        {
            Write(0);
            return;
        }

        Write(value.Count);

        foreach (var item in value)
        {
            WriteIdAsString(item);
        }
    }

    public void WriteListId(IList<string>? value, int length)
    {
        if (value is null) return;

        for (var i = 0; i < length; i++)
        {
            WriteIdAsString(value[i]);

            if (i >= value.Count)
            {
                WriteIdAsString(default);
            }
        }
    }

    public void WriteListId_deprec(IList<string>? value)
    {
        WriteDeprecVersion();
        WriteListId(value);
    }
}
