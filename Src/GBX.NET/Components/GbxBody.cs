
namespace GBX.NET.Components;

public sealed class GbxBody
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

    internal static GbxBody Parse(GbxReader reader, GbxCompression compression, GbxReadSettings settings)
    {
        return GbxBodyReader.Parse(reader, compression, settings);
    }

    internal static GbxBody Parse(IClass node, GbxReader reader, GbxReadSettings settings, GbxCompression compression)
    {
        using var rw = new GbxReaderWriter(reader, leaveOpen: true);
        return new GbxBodyReader(rw, settings, compression).Parse(node);
    }

    internal static GbxBody Parse<T>(T node, GbxReader reader, GbxReadSettings settings, GbxCompression compression) where T : IClass
    {
        using var rw = new GbxReaderWriter(reader, leaveOpen: true);
        return new GbxBodyReader(rw, settings, compression).Parse(node);
    }
}
