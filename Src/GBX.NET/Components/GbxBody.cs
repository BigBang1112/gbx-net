using System.Collections.Immutable;
using System.Text;

namespace GBX.NET.Components;

public sealed partial class GbxBody
{
    private readonly ImmutableArray<byte> rawData;

    public int UncompressedSize { get; init; }
    public int? CompressedSize { get; init; }

    public float? CompressionRatio => CompressedSize.HasValue ? (float)CompressedSize / UncompressedSize : null;

    /// <summary>
    /// Pure body data usually in compressed form. This property is used if GameBox's ParseHeader methods are used, otherwise null.
    /// </summary>
    public ImmutableArray<byte> RawData
    {
        get => rawData;
        init
        {
            rawData = value;

            if (!value.IsDefaultOrEmpty)
            {
                UncompressedSize = value.Length;
            }
        }
    }

    public Exception? Exception { get; internal set; }

    public override string ToString()
    {
        if (CompressedSize is null)
        {
            return "GbxBody (uncompressed)";
        }

        var sb = new StringBuilder();
        sb.Append("GbxBody (compressed, ");
        sb.Append(CompressedSize.Value.ToString("N0"));
        sb.Append('/');
        sb.Append(UncompressedSize.ToString("N0"));
        sb.Append(" bytes");

        if (CompressionRatio.HasValue)
        {
            sb.Append(", ");
            sb.Append(CompressionRatio.Value.ToString("P2"));
            sb.Append(" ratio");
        }

        sb.Append(')');
        return sb.ToString();
    }

    [Zomp.SyncMethodGenerator.CreateSyncVersion]
    internal static async Task<GbxBody> ParseAsync(GbxReader reader, GbxCompression compression, GbxReadSettings settings, CancellationToken cancellationToken)
    {
        return await GbxBodyReader.ParseAsync(reader, compression, settings, cancellationToken);
    }

    [Zomp.SyncMethodGenerator.CreateSyncVersion]
    internal static async Task<GbxBody> ParseAsync(IClass node, GbxReader reader, GbxCompression compression, GbxReadSettings settings, CancellationToken cancellationToken)
    {
        using var rw = new GbxReaderWriter(reader, settings.LeaveOpen);
        return await new GbxBodyReader(rw, settings, compression).ParseAsync(node, cancellationToken);
    }

    [Zomp.SyncMethodGenerator.CreateSyncVersion]
    internal static async Task<GbxBody> ParseAsync<T>(T node, GbxReader reader, GbxCompression compression, GbxReadSettings settings, CancellationToken cancellationToken) where T : IClass
    {
        using var rw = new GbxReaderWriter(reader, settings.LeaveOpen);
        return await new GbxBodyReader(rw, settings, compression).ParseAsync(node, cancellationToken);
    }

    internal bool Write(IClass? node, GbxWriter writer, GbxCompression compression, GbxWriteSettings settings)
    {
        return new GbxBodyWriter(this, writer, settings, compression).Write(node);
    }
}
