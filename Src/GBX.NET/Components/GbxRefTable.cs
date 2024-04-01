namespace GBX.NET.Components;

public sealed class GbxRefTable
{
    public int AncestorLevel { get; set; }

    internal static GbxRefTable? Parse(GbxReader reader, GbxHeader header, GbxReadSettings settings)
    {
        return new GbxRefTableReader(reader, header, settings).Parse();
    }

    internal bool Write(GbxWriter writer, GbxWriteSettings settings)
    {
        return new GbxRefTableWriter(this, writer, settings).Write();
    }

    public GbxRefTable DeepClone()
    {
        return new GbxRefTable(); // WRONG, TODO
    }
}