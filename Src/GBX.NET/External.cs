using GBX.NET.Components;

namespace GBX.NET;

[Obsolete("Use NodeRef<T> or NodeRefList<T> to preserve reference metadata.")]
public sealed record External<T> : IDeepCloneable where T : CMwNod?
{
    private NodeRef<T> reference = new();

    internal T? CachedNode => reference.CachedNode;

    public T? Node
    {
        get => reference.Node;
        set => reference.Node = value;
    }

    public GbxRefTableFile? File
    {
        get => reference.File;
        set
        {
            if (ReferenceEquals(value, reference.File))
            {
                return;
            }

            if (value is null)
            {
                reference.Node = reference.CachedNode;
            }
            else
            {
                reference.Descriptor = value;
            }
        }
    }

    public External(T? node, GbxRefTableFile? file)
    {
        Node = node;
        File = file;

        if (node is not null && file is not null)
        {
            file.Resolution.Seed(node);
            reference.Resolve();
        }
    }

    private External(External<T> original)
    {
        reference = new();
        reference.CopyFrom(original.reference);
    }

    internal NodeRef<T> ToNodeRef() => reference;

    object IDeepCloneable.DeepClone(DeepCloneContext context)
    {
        var clone = new External<T>(null, null);
        context.Register(this, clone);
        clone.reference = context.Clone(reference)!;
        return clone;
    }
}
