#if NET6_0_OR_GREATER
using System.Diagnostics.CodeAnalysis;
#endif

namespace GBX.NET.Serialization;

public partial interface IGbxReaderWriter : IDisposable
{
    IGbxReader? Reader { get; }
    IGbxWriter? Writer { get; }

    string? Id(string? value = default);
    void Id(ref string? value);
}

internal sealed partial class GbxReaderWriter : IGbxReaderWriter
{
    private readonly bool leaveOpen;

    public GbxReader? Reader { get; }
    public GbxWriter? Writer { get; }

    IGbxReader? IGbxReaderWriter.Reader => Reader;
    IGbxWriter? IGbxReaderWriter.Writer => Writer;

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

    public int Byte(int value = default) => Byte((byte)value);

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

#if NET6_0_OR_GREATER
    [return: NotNullIfNotNull(nameof(value))]
#endif
    public string? Id(string? value = null) => IdAsString(value);

    public void Id(
#if NET6_0_OR_GREATER
        [NotNullIfNotNull(nameof(value))]
#endif
        ref string? value) => value = Id(value);
}
