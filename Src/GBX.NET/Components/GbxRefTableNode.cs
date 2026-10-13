namespace GBX.NET.Components;

public abstract class GbxRefTableNode : IDeepCloneable
{
    protected internal GbxRefTable RefTable { get; }
    internal NodeRefResolution Resolution { get; } = new();

    public int Flags { get; }
    public bool UseFile { get; }

    protected GbxRefTableNode(GbxRefTable refTable, int flags, bool useFile)
    {
        RefTable = refTable ?? throw new ArgumentNullException(nameof(refTable));
        Flags = flags;
        UseFile = useFile;
    }

    internal abstract CMwNod? ResolveTarget();
    internal virtual Task<CMwNod?> ResolveTargetAsync() => Task.FromResult(ResolveTarget());

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

        if (Resolution.CachedTarget is not null) 
        {
            clone.Resolution.Seed(context.Clone(Resolution.CachedTarget)!);
        }
        
        return clone;
    }
}
