using GBX.NET.Managers;

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

    public override string ToString()
    {
        return $"GbxHeader ({ClassManager.GetName(ClassId)}, 0x{ClassId:X8})";
    }

    public abstract GbxHeader DeepClone();

    internal static GbxHeader<T> Parse<T>(GbxReader reader, GbxReadSettings settings, out T node) where T : IClass, new()
    {
        return new GbxHeaderReader(reader, settings).Parse(out node);
    }

    internal static GbxHeader<T> Parse<T>(Stream stream, GbxReadSettings settings, out T node) where T : IClass, new()
    {
        using var reader = new GbxReader(stream, settings.Logger);
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
        using var reader = new GbxReader(stream, settings.Logger);
        return Parse(reader, settings, out node);
    }

    internal static GbxHeader Parse(string fileName, GbxReadSettings settings, out IClass? node)
    {
        using var fs = File.OpenRead(fileName);
        return Parse(fs, settings, out node);
    }

    internal bool Write(GbxWriter writer, IClass? node, GbxWriteSettings settings)
    {
        return new GbxHeaderWriter(this, writer, settings).Write(node);
    }
}