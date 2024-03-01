
namespace GBX.NET.Components;

public sealed partial class GbxBody
{
    private byte[]? rawData;

    public int UncompressedSize { get; init; }
    public int? CompressedSize { get; init; }

    /// <summary>
    /// Pure body data usually in compressed form. This property is used if GameBox's ParseHeader methods are used, otherwise null.
    /// </summary>
    public byte[]? RawData
    {
        get => rawData;
        init
        {
            rawData = value;

            if (value is not null)
            {
                UncompressedSize = value.Length;
            }
        }
    }

    public Exception? Exception { get; internal set; }

    [Zomp.SyncMethodGenerator.CreateSyncVersion]
    internal static async Task<GbxBody> ParseAsync(GbxReader reader, GbxCompression compression, GbxReadSettings settings, CancellationToken cancellationToken)
    {
        return await GbxBodyReader.ParseAsync(reader, compression, settings, cancellationToken);
    }

    [Zomp.SyncMethodGenerator.CreateSyncVersion]
    internal static async Task<GbxBody> ParseAsync(IClass node, GbxReader reader, GbxCompression compression, GbxReadSettings settings, CancellationToken cancellationToken)
    {
        using var rw = new GbxReaderWriter(reader, leaveOpen: true);
        return await new GbxBodyReader(rw, settings, compression).ParseAsync(node, cancellationToken);
    }

    [Zomp.SyncMethodGenerator.CreateSyncVersion]
    internal static async Task<GbxBody> ParseAsync<T>(T node, GbxReader reader, GbxCompression compression, GbxReadSettings settings, CancellationToken cancellationToken) where T : IClass
    {
        using var rw = new GbxReaderWriter(reader, leaveOpen: true);
        return await new GbxBodyReader(rw, settings, compression).ParseAsync(node, cancellationToken);
    }

    internal bool Write(IClass? node, GbxWriter writer, GbxCompression compression, GbxWriteSettings settings)
    {
        return new GbxBodyWriter(this, writer, settings, compression).Write(node);
    }
}
