using GBX.NET.Components;
using System.Diagnostics.CodeAnalysis;

namespace GBX.NET;

/// <summary>
/// A stored node reference. Inspecting its metadata never resolves its node.
/// </summary>
public abstract class NodeRef
{
    private protected NodeRef()
    {
    }

    internal static NodeRef<T> GetOrCreate<T>(ref NodeRef<T>? reference) where T : CMwNod?
    {
        var existing = Volatile.Read(ref reference);
        if (existing is not null)
        {
            return existing;
        }

        var created = new NodeRef<T>();
        return Interlocked.CompareExchange(ref reference, created, null) ?? created;
    }

    internal static void SetEmbedded<T>(
        [NotNullIfNotNull(nameof(value))] ref NodeRef<T>? reference,
        T? value) where T : CMwNod?
    {
        if (value is null)
        {
            reference?.Clear();
        }
        else
        {
            GetOrCreate(ref reference).Node = value;
        }
    }

    internal static void CopyFrom<T>(ref NodeRef<T>? reference, NodeRef<T>? source) where T : CMwNod?
    {
        if (source is null || source.Kind == NodeRefKind.None)
        {
            reference?.Clear();
        }
        else
        {
            GetOrCreate(ref reference).CopyFrom(source);
        }
    }

    /// <summary>
    /// How the reference is stored, independently of whether its node has been resolved.
    /// </summary>
    public abstract NodeRefKind Kind { get; }

    /// <summary>
    /// The current resolution state. Inspecting it does not load the node.
    /// </summary>
    public abstract NodeRefState State { get; }

    /// <summary>
    /// The external descriptor, or null for an embedded or absent reference. Assigning it replaces stored reference data without loading the node. Null clears the reference.
    /// </summary>
    public abstract GbxRefTableNode? Descriptor { get; set; }

    /// <summary>
    /// The external file descriptor, if this reference points to a file.
    /// </summary>
    public GbxRefTableFile? File => Descriptor as GbxRefTableFile;

    /// <summary>
    /// The external resource descriptor, if this reference points to a resource.
    /// </summary>
    public GbxRefTableResource? Resource => Descriptor as GbxRefTableResource;

    /// <summary>
    /// Resolves the node on access, with the same behavior as <see cref="Resolve"/>.
    /// </summary>
    /// <remarks>
    /// Assigning a node stores it as embedded and clears the external descriptor. Null clears the reference. The node must match the handle's declared type.
    /// </remarks>
    public CMwNod? Node
    {
        get => Resolve();
        set => SetNodeCore(value);
    }

    internal abstract void SetNodeCore(CMwNod? value);

    internal CMwNod? CachedNode => CachedNodeCore;

    internal abstract CMwNod? CachedNodeCore { get; }

    /// <summary>
    /// The cached resolver or node-type exception, if any.
    /// </summary>
    public abstract Exception? Exception { get; }

    /// <summary>
    /// Resolves the node, returning null for an absent or missing node. Other errors propagate.
    /// </summary>
    public CMwNod? Resolve() => ResolveCore();

    internal abstract CMwNod? ResolveCore();

    /// <summary>
    /// Resolves the node asynchronously. Cancellation stops this waiter without canceling a shared load.
    /// </summary>
    public Task<CMwNod?> ResolveAsync(CancellationToken cancellationToken = default) => ResolveAsyncCore(cancellationToken);

    internal abstract Task<CMwNod?> ResolveAsyncCore(CancellationToken cancellationToken);

    /// <summary>
    /// Returns whether a node was found. Resolver and type errors still propagate.
    /// </summary>
#if NET8_0_OR_GREATER
    public bool TryResolve([NotNullWhen(true)] out CMwNod? value)
#else
    public bool TryResolve(out CMwNod? value)
#endif
    {
        value = Resolve();
        return value is not null;
    }

    /// <summary>
    /// Clears this handle without changing other handles that share its external descriptor.
    /// </summary>
    public abstract void Clear();

    /// <summary>
    /// Invalidates the descriptor's shared cache without loading its node. Active loads cannot be reset.
    /// </summary>
    public abstract void Reset();

    /// <summary>
    /// Invalidates the shared cache and resolves the node again.
    /// </summary>
    public CMwNod? Retry()
    {
        Reset();
        return Resolve();
    }

    /// <summary>
    /// Invalidates the shared cache and resolves the node again asynchronously.
    /// </summary>
    public async Task<CMwNod?> RetryAsync(CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        Reset();
        return await ResolveAsync(cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Describes the external descriptor or embedded node without resolving it. An absent reference returns "null".
    /// </summary>
    public override string ToString() => Descriptor?.ToString() ?? CachedNode?.ToString() ?? "None";
}
