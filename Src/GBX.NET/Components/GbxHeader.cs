namespace GBX.NET.Components;

public abstract class GbxHeader
{
    public GbxHeaderBasic Basic { get; set; }

    public int NumNodes { get; internal set; }

    public abstract uint ClassId { get; }

    protected GbxHeader(GbxHeaderBasic basic)
    {
        Basic = basic;
    }

    internal static GbxHeader<T> Parse<T>(GbxReader reader, GbxReadSettings settings, out T node) where T : IClass, new()
    {
        return new GbxHeaderReader(reader, settings).Parse(out node);
    }

    internal static GbxHeader<T> Parse<T>(Stream stream, GbxReadSettings settings, out T node) where T : IClass, new()
    {
        using var reader = new GbxReader(stream);
        return Parse(reader, settings, out node);
    }

    internal static GbxHeader<T> Parse<T>(string fileName, GbxReadSettings settings, out T node) where T : IClass, new()
    {
        using var fs = File.OpenRead(fileName);
        return Parse(fs, settings, out node);
    }

    internal static GbxHeader Parse(GbxReader reader, GbxReadSettings settings, out IClass? node)
    {
        return new GbxHeaderReader(reader, settings).Parse(out node);
    }

    internal static GbxHeader Parse(Stream stream, GbxReadSettings settings, out IClass? node)
    {
        using var reader = new GbxReader(stream);
        return Parse(reader, settings, out node);
    }

    internal static GbxHeader Parse(string fileName, GbxReadSettings settings, out IClass? node)
    {
        using var fs = File.OpenRead(fileName);
        return Parse(fs, settings, out node);
    }

    internal bool Write(GbxWriter writer, IClass node, GbxWriteSettings settings)
    {
        using var rw = new GbxReaderWriter(writer);
        return new GbxHeaderWriter(this, rw, settings).Write(node);
    }
}