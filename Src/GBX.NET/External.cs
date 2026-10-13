using GBX.NET.Components;

namespace GBX.NET;

[Obsolete("Use NodeRef<T> or NodeRefList<T> to preserve reference metadata.")]
public sealed record External<T> : IDeepCloneable where T : CMwNod
{
    private T? node;
    internal T? CachedTarget => node;

    public T? Node
    {
        get => File?.GetNode(ref node) ?? node;
        set => node = value;
    }

    public GbxRefTableFile? File { get; set; }

    public External(T? node, GbxRefTableFile? file)
    {
        Node = node;
        File = file;
    }

    public T? GetNode(GbxReadSettings settings = default) => File?.GetNode(ref node, settings) ?? node;

    internal NodeRef<T> ToNodeRef()
    {
        var reference = new NodeRef<T>();
        if (File is { } file) reference.SetExternal(file);
        else reference.SetEmbedded(node);
        return reference;
    }

    object IDeepCloneable.DeepClone(DeepCloneContext context)
    {
        var clone = new External<T>(null, null);
        context.Register(this, clone);
        clone.node = context.Clone(node);
        clone.File = context.Clone(File);
        return clone;
    }
}
