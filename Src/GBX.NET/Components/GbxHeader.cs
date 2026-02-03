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

    internal static GbxHeader Parse(GbxReader reader, out IClass? node)
    {
        return new GbxHeaderReader(reader).Parse(out node);
    }

    internal static GbxHeader Parse(Stream stream, GbxReadSettings settings, out IClass? node)
    {
        using var reader = new GbxReader(stream, settings);
        return Parse(reader, out node);
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