using System.Diagnostics.CodeAnalysis;

namespace GBX.NET.Serialization;

public partial interface IGbxReaderWriter : IDisposable
{
    GbxReader? Reader { get; }
    GbxWriter? Writer { get; }

    [return: NotNullIfNotNull(nameof(value))]
    string? Id(string? value = default);
    void Id([NotNullIfNotNull(nameof(value))] ref string? value);

    Int3 Byte3(Int3 value = default);
    [return: NotNullIfNotNull(nameof(value))]
    Int3? Byte3(Int3? value, Int3 defaultValue = default);
    void Byte3(ref Int3 value);
    void Byte3([NotNullIfNotNull(nameof(value))] ref Int3? value, Int3 defaultValue = default);

    void Marker(string value);

    [return: NotNullIfNotNull(nameof(value))]
    T? ReadableWritable<T>(T? value, int version = 0) where T : IReadableWritable, new();
    void ReadableWritable<T>([NotNullIfNotNull(nameof(value))] ref T? value, int version = 0) where T : IReadableWritable, new();
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
        versionable.Version = Int32(versionable.Version);
    }

    public void VersionByte(IVersionable versionable)
    {
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
}
