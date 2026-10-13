namespace GBX.NET.Components;

public abstract class GbxRefTableNode : IDeepCloneable
{
    /// <summary>
    /// The table that owns this descriptor and supplies its read settings and external resolvers.
    /// </summary>
    public GbxRefTable RefTable { get; }

    internal NodeRefResolution Resolution { get; } = new();

    public int Flags { get; }
    public bool UseFile { get; }

    protected GbxRefTableNode(GbxRefTable refTable, int flags, bool useFile)
    {
        RefTable = refTable ?? throw new ArgumentNullException(nameof(refTable));
        Flags = flags;
        UseFile = useFile;
    }

    internal abstract CMwNod? ResolveNode();

    internal virtual Task<CMwNod?> ResolveNodeAsync() => Task.FromResult(ResolveNode());

    object IDeepCloneable.DeepClone(DeepCloneContext context)
    {
        var table = context.Clone(RefTable)!;

        GbxRefTableNode clone = this switch
        {
            GbxRefTableFile file => new GbxRefTableFile(table, Flags, UseFile, file.FilePath),
            GbxRefTableResource resource => new GbxRefTableResource(table, Flags, UseFile, resource.ResourceIndex),
            _ => throw new NotSupportedException("Unknown reference descriptor.")
        };

        context.Register(this, clone);

        if (Resolution.CachedNode is not null)
        {
            clone.Resolution.Seed(context.Clone(Resolution.CachedNode)!);
        }

        return clone;
    }
}
