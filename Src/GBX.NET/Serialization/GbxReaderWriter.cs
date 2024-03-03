using System.Diagnostics.CodeAnalysis;

namespace GBX.NET.Serialization;

public partial interface IGbxReaderWriter : IDisposable
{
    GbxReader? Reader { get; }
    GbxWriter? Writer { get; }

    void VersionInt32(IVersionable versionable);
    void VersionByte(IVersionable versionable);

    [return: NotNullIfNotNull(nameof(value))]
    string? Id(string? value = default);
    void Id([NotNullIfNotNull(nameof(value))] ref string? value);

    Int3 Byte3(Int3 value = default);
    [return: NotNullIfNotNull(nameof(value))]
    Int3? Byte3(Int3? value, Int3 defaultValue = default);
    void Byte3(ref Int3 value);
    void Byte3([NotNullIfNotNull(nameof(value))] ref Int3? value, Int3 defaultValue = default);

    T EnumInt32<T>(T value) where T : struct, Enum;
    void EnumInt32<T>(ref T value) where T : struct, Enum;
    T EnumByte<T>(T value) where T : struct, Enum;
    void EnumByte<T>(ref T value) where T : struct, Enum;

    void Marker(string value);

    [return: NotNullIfNotNull(nameof(value))]
    T? ReadableWritable<T>(T? value, int version = 0) where T : IReadableWritable, new();
    void ReadableWritable<T>([NotNullIfNotNull(nameof(value))] ref T? value, int version = 0) where T : IReadableWritable, new();

    [return: NotNullIfNotNull(nameof(value))]
    T[]? ArrayReadableWritable<T>(T[]? value, int length, int version = 0) where T : IReadableWritable, new();
    void ArrayReadableWritable<T>([NotNullIfNotNull(nameof(value))] ref T[]? value, int length, int version = 0) where T : IReadableWritable, new();
    [return: NotNullIfNotNull(nameof(value))]
    T[]? ArrayReadableWritable<T>(T[]? value = default, int version = 0) where T : IReadableWritable, new();
    void ArrayReadableWritable<T>([NotNullIfNotNull(nameof(value))] ref T[]? value, int version = 0) where T : IReadableWritable, new();
    [return: NotNullIfNotNull(nameof(value))]
    T[]? ArrayReadableWritable_deprec<T>(T[]? value = default, int version = 0) where T : IReadableWritable, new();
    void ArrayReadableWritable_deprec<T>([NotNullIfNotNull(nameof(value))] ref T[]? value, int version = 0) where T : IReadableWritable, new();
    [return: NotNullIfNotNull(nameof(value))]
    IList<T>? ListReadableWritable<T>(IList<T>? value, int length, int version = 0) where T : IReadableWritable, new();
    void ListReadableWritable<T>([NotNullIfNotNull(nameof(value))] ref IList<T>? value, int length, int version = 0) where T : IReadableWritable, new();
    [return: NotNullIfNotNull(nameof(value))]
    IList<T>? ListReadableWritable<T>(IList<T>? value = default, int version = 0) where T : IReadableWritable, new();
    void ListReadableWritable<T>([NotNullIfNotNull(nameof(value))] ref IList<T>? value, int version = 0) where T : IReadableWritable, new();
    [return: NotNullIfNotNull(nameof(value))]
    IList<T>? ListReadableWritable_deprec<T>(IList<T>? value = default, int version = 0) where T : IReadableWritable, new();
    void ListReadableWritable_deprec<T>([NotNullIfNotNull(nameof(value))] ref IList<T>? value, int version = 0) where T : IReadableWritable, new();
}

public sealed partial class GbxReaderWriter : IGbxReaderWriter
{
    private readonly bool leaveOpen;

    public GbxReader? Reader { get; }
    public GbxWriter? Writer { get; }

    public GbxReaderWriter(GbxReader reader, bool leaveOpen = false)
    {
        Reader = reader;

        this.leaveOpen = leaveOpen;
    }

    public GbxReaderWriter(GbxWriter writer, bool leaveOpen = false)
    {
        Writer = writer;

        this.leaveOpen = leaveOpen;
    }

    public GbxReaderWriter(GbxReader reader, GbxWriter writer, bool leaveOpen = false)
    {
        Reader = reader;
        Writer = writer;

        this.leaveOpen = leaveOpen;
    }

    public int Byte(int value) => Byte((byte)value);

    public void Byte(ref int value) => value = Byte(value);

    public void VersionInt32(IVersionable versionable)
    {
        _ = versionable ?? throw new ArgumentNullException(nameof(versionable));
        versionable.Version = Int32(versionable.Version);
    }

    public void VersionByte(IVersionable versionable)
    {
        _ = versionable ?? throw new ArgumentNullException(nameof(versionable));
        versionable.Version = Byte(versionable.Version);
    }

    public void Dispose()
    {
        if (leaveOpen)
        {
            return;
        }

        Reader?.Dispose();
        Writer?.Dispose();
    }

    [return: NotNullIfNotNull(nameof(value))]
    public string? Id(string? value = null) => IdAsString(value);

    public void Id([NotNullIfNotNull(nameof(value))] ref string? value) => value = Id(value);

    public Int3 Byte3(Int3 value) => Byte3((Byte3)value);

    [return: NotNullIfNotNull(nameof(value))]
    public Int3? Byte3(Int3? value, Int3 defaultValue = default) => Byte3((Byte3?)value, (Byte3)defaultValue);

    public void Byte3(ref Int3 value) => value = Byte3(value);

    public void Byte3([NotNullIfNotNull(nameof(value))] ref Int3? value, Int3 defaultValue = default)
        => value = Byte3(value, defaultValue);

    public T EnumInt32<T>(T value) where T : struct, Enum
    {
        if (Reader is not null)
        {
            value = (T)Enum.ToObject(typeof(T), Reader.ReadInt32()); // CastTo<T>.From(Reader.ReadInt32());
        }

        Writer?.Write((int)(object)value /* CastTo<int>.From(value) */);

        return value;
    }

    public void EnumInt32<T>(ref T value) where T : struct, Enum => value = EnumInt32(value);

    public T EnumByte<T>(T value) where T : struct, Enum
    {
        if (Reader is not null)
        {
            value = (T)Enum.ToObject(typeof(T), Reader.ReadByte()); // CastTo<T>.From(Reader.ReadByte());
        }

        Writer?.Write((byte)(object)value /* CastTo<byte>.From(value) */);

        return value;
    }

    public void EnumByte<T>(ref T value) where T : struct, Enum => value = EnumByte(value);

    public void Marker(string value)
    {
        Reader?.ReadMarker(value);
        Writer?.WriteMarker(value);
    }

    [return: NotNullIfNotNull(nameof(value))]
    public T? ReadableWritable<T>(T? value, int version = 0) where T : IReadableWritable, new()
    {
        if (Reader is not null)
        {
            value ??= new T();
        }

        (value ?? new T()).ReadWrite(this, version);
        
        return value;
    }

    public void ReadableWritable<T>([NotNullIfNotNull(nameof(value))] ref T? value, int version = 0)
        where T : IReadableWritable, new() => value = ReadableWritable(value, version);

    [return: NotNullIfNotNull(nameof(value))]
    public T[]? ArrayReadableWritable<T>(T[]? value, int length, int version = 0) where T : IReadableWritable, new()
    {
        if (Reader is not null && length != 0)
        {
            switch (length)
            {
                case < 0:
                    throw new ArgumentOutOfRangeException(nameof(length), "Length is not valid.");
                case < 0 or > 0x10000000: // ~268MB
                    throw new Exception($"Length is too big to handle ({length}).");
            }

            var array = new T[length];

            for (int i = 0; i < length; i++)
            {
                array[i] = ReadableWritable<T>(default, version)!;
            }

            value = array;
        }

        if (Writer is not null)
        {
            if (value is not null)
            {
                foreach (var item in value)
                {
                    ReadableWritable(item, version);
                }
            }

            if (value is null || length > value.Length)
            {
                for (var i = value?.Length ?? 0; i < length; i++)
                {
                    ReadableWritable<T>(new(), version);
                }
            }
        }

        return value;
    }

    public void ArrayReadableWritable<T>([NotNullIfNotNull(nameof(value))] ref T[]? value, int length, int version = 0)
        where T : IReadableWritable, new() => value = ArrayReadableWritable(value, length, version);

    [return: NotNullIfNotNull(nameof(value))]
    public T[]? ArrayReadableWritable<T>(T[]? value = default, int version = 0) where T : IReadableWritable, new()
    {
        if (Reader is not null)
        {
            var length = Reader.ReadInt32();
            value = ArrayReadableWritable(value, length, version);
        }

        if (Writer is not null)
        {
            var length = value?.Length ?? 0;
            Writer.Write(length);

            if (length > 0)
            {
                ArrayReadableWritable(value, length, version);
            }
        }

        return value;
    }

    public void ArrayReadableWritable<T>([NotNullIfNotNull(nameof(value))] ref T[]? value, int version = 0)
        where T : IReadableWritable, new() => value = ArrayReadableWritable(value, version);

    [return: NotNullIfNotNull(nameof(value))]
    public T[]? ArrayReadableWritable_deprec<T>(T[]? value = default, int version = 0) where T : IReadableWritable, new()
    {
        Reader?.ReadDeprecVersion();
        Writer?.WriteDeprecVersion();
        return ArrayReadableWritable(value, version);
    }

    public void ArrayReadableWritable_deprec<T>([NotNullIfNotNull(nameof(value))] ref T[]? value, int version = 0)
        where T : IReadableWritable, new() => value = ArrayReadableWritable_deprec(value, version);

    [return: NotNullIfNotNull(nameof(value))]
    public IList<T>? ListReadableWritable<T>(IList<T>? value, int length, int version = 0) where T : IReadableWritable, new()
    {
        if (Reader is not null && length != 0)
        {
            switch (length)
            {
                case < 0:
                    throw new ArgumentOutOfRangeException(nameof(length), "Length is not valid.");
                case < 0 or > 0x10000000: // ~268MB
                    throw new Exception($"Length is too big to handle ({length}).");
            }

            var list = new List<T>(length);

            for (int i = 0; i < length; i++)
            {
                list.Add(ReadableWritable<T>(default, version)!);
            }

            value = list;
        }

        if (Writer is not null)
        {
            if (value is not null)
            {
                foreach (var item in value)
                {
                    ReadableWritable(item, version);
                }
            }

            if (value is null || length > value.Count)
            {
                for (var i = value?.Count ?? 0; i < length; i++)
                {
                    ReadableWritable<T>(new(), version);
                }
            }
        }

        return value;
    }

    public void ListReadableWritable<T>([NotNullIfNotNull(nameof(value))] ref IList<T>? value, int length, int version = 0)
        where T : IReadableWritable, new() => value = ListReadableWritable(value, length, version);

    [return: NotNullIfNotNull(nameof(value))]
    public IList<T>? ListReadableWritable<T>(IList<T>? value = default, int version = 0) where T : IReadableWritable, new()
    {
        if (Reader is not null)
        {
            var length = Reader.ReadInt32();
            value = ListReadableWritable(value, length, version);
        }

        if (Writer is not null)
        {
            var length = value?.Count ?? 0;
            Writer.Write(length);

            if (length > 0)
            {
                ListReadableWritable(value, length, version);
            }
        }

        return value;
    }

    public void ListReadableWritable<T>([NotNullIfNotNull(nameof(value))] ref IList<T>? value, int version = 0)
        where T : IReadableWritable, new() => value = ListReadableWritable(value, version);

    [return: NotNullIfNotNull(nameof(value))]
    public IList<T>? ListReadableWritable_deprec<T>(IList<T>? value = default, int version = 0) where T : IReadableWritable, new()
    {
        Reader?.ReadDeprecVersion();
        Writer?.WriteDeprecVersion();
        return ListReadableWritable(value, version);
    }

    public void ListReadableWritable_deprec<T>([NotNullIfNotNull(nameof(value))] ref IList<T>? value, int version = 0)
        where T : IReadableWritable, new() => value = ListReadableWritable_deprec(value, version);
}
