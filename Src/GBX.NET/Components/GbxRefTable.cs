using System.Collections.Immutable;

namespace GBX.NET.Components;

public sealed class GbxRefTable
{
    public int AncestorLevel { get; set; }

    /// <summary>
    /// Directories in the reference table at point when the Gbx was read. This could be different from reality if the nodes were modified.
    /// </summary>
    public ImmutableList<ImmutableGbxRefTableDirectory> Directories { get; internal set; } = ImmutableList<ImmutableGbxRefTableDirectory>.Empty;

    /// <summary>
    /// Files in the root directory of the reference table at point when the Gbx was read. This could be different from reality if the nodes were modified.
    /// </summary>
    public ImmutableList<ImmutableGbxRefTableFile> Files { get; internal set; } = ImmutableList<ImmutableGbxRefTableFile>.Empty;

    /// <summary>
    /// Resources in the reference table at point when the Gbx was read. This could be different from reality if the nodes were modified.
    /// </summary>
    public ImmutableList<ImmutableGbxRefTableResource> Resources { get; internal set; } = ImmutableList<ImmutableGbxRefTableResource>.Empty;

    internal static GbxRefTable? Parse(GbxReader reader, GbxHeader header, GbxReadSettings settings)
    {
        return new GbxRefTableReader(reader, header, settings).Parse();
    }

    internal bool Write(GbxWriter writer, GbxHeader header, GbxWriteSettings settings)
    {
        return new GbxRefTableWriter(this, header, writer, settings).Write();
    }

    public GbxRefTable DeepClone()
    {
        return new GbxRefTable(); // WRONG, TODO
    }
}