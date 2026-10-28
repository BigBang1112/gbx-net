using GBX.NET.Components;
using System.Runtime.ExceptionServices;
using System.Diagnostics.CodeAnalysis;

namespace GBX.NET;

/// <summary>
/// Stores an embedded node or an external descriptor independently of resolution.
/// </summary>
public sealed class NodeRef<T> : NodeRef, IDeepCloneable where T : CMwNod?
{
#if NET9_0_OR_GREATER
    private Lock? sync;

    private Lock Sync => LazyInitializer.EnsureInitialized(ref sync);
#else
    private object? sync;

    private object Sync => LazyInitializer.EnsureInitialized(ref sync)!;
#endif
    private GbxRefTableNode? descriptor;
    private T? node;
    private ExceptionDispatchInfo? error;
    private long revision;
    private int validatedGeneration = -1;

    public NodeRef()
    {
    }

    internal NodeRef(T? node)
    {
        // The handle is not published yet, so its initial embedded node needs no lock.
        this.node = node;
    }

    public override NodeRefKind Kind
    {
        get
        {
            lock (Sync)
            {
                return descriptor switch
                {
                    GbxRefTableFile => NodeRefKind.ExternalFile,
                    GbxRefTableResource => NodeRefKind.ExternalResource,
                    _ => node is null ? NodeRefKind.None : NodeRefKind.Embedded
                };
            }
        }
    }

    public override NodeRefState State
    {
        get
        {
            lock (Sync)
            {
                if (descriptor is not { } source)
                {
                    return node is null ? NodeRefState.Unresolved : NodeRefState.Resolved;
                }

                var snapshot = source.Resolution.Snapshot();

                if (error is not null && validatedGeneration == snapshot.Generation)
                {
                    return NodeRefState.Failed;
                }

                return snapshot.State == NodeRefState.Resolved && validatedGeneration != snapshot.Generation
                    ? NodeRefState.Unresolved
                    : snapshot.State;
            }
        }
    }

    /// <inheritdoc cref="NodeRef.Node"/>
    public new T? Node
    {
        get => Resolve();
        set => SetStorage(value, null);
    }

    internal new T? CachedNode
    {
        get
        {
            lock (Sync)
            {
                if (descriptor is not { } source)
                {
                    return node;
                }

                var snapshot = source.Resolution.Snapshot();
                return validatedGeneration == snapshot.Generation && error is null ? (T?)snapshot.Node : null;
            }
        }
    }

    internal override CMwNod? CachedNodeCore => CachedNode;

    public override Exception? Exception
    {
        get
        {
            lock (Sync)
            {
                if (descriptor is not { } source)
                {
                    return error?.SourceException;
                }

                var snapshot = source.Resolution.Snapshot();
                return validatedGeneration == snapshot.Generation ? error?.SourceException ?? snapshot.Exception : snapshot.Exception;
            }
        }
    }

    public override GbxRefTableNode? Descriptor
    {
        get
        {
            lock (Sync)
            {
                return descriptor;
            }
        }

        set => SetStorage(null, value);
    }

    private void SetStorage(T? node, GbxRefTableNode? descriptor)
    {
        lock (Sync)
        {
            revision++;
            this.descriptor = descriptor;
            this.node = node;
            error = null;
            validatedGeneration = -1;
        }
    }

    /// <summary>
    /// Copies stored reference metadata or an embedded node without resolving it. This handle retains its identity.
    /// </summary>
    public void CopyFrom(NodeRef<T> reference)
    {
        if (reference is null)
        {
            throw new ArgumentNullException(nameof(reference));
        }

        if (ReferenceEquals(reference, this))
        {
            return;
        }

        GbxRefTableNode? source;
        T? embedded;

        lock (reference.Sync)
        {
            source = reference.descriptor;
            embedded = reference.node;
        }

        SetStorage(embedded, source);
    }

    internal override void SetNodeCore(CMwNod? value)
    {
        if (value is null || value is T)
        {
            Node = (T?)value;
        }
        else
        {
            throw new InvalidCastException($"Expected {typeof(T).Name}, received Gbx class 0x{value.GetId():X8}.");
        }
    }

    /// <inheritdoc cref="NodeRef.Resolve"/>
    public new T? Resolve()
    {
        var (source, version, node, resolved) = Snapshot();

        if (source is null || resolved)
        {
            return node;
        }

        var result = source.Resolution.Resolve(source);
        return Validate(source, result.Node, version, result.Generation);
    }

    internal override CMwNod? ResolveCore() => Resolve();

    /// <inheritdoc cref="NodeRef.TryResolve"/>
#if NET8_0_OR_GREATER
    public bool TryResolve([NotNullWhen(true)] out T? value)
#else
    public bool TryResolve(out T? value)
#endif
    {
        value = Resolve();
        return value is not null;
    }

    /// <inheritdoc cref="NodeRef.ResolveAsync"/>
    public new async Task<T?> ResolveAsync(CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var (source, version, node, resolved) = Snapshot();

        if (source is null || resolved)
        {
            return node;
        }

        var result = await source.Resolution.ResolveAsync(source, cancellationToken).ConfigureAwait(false);
        return Validate(source, result.Node, version, result.Generation);
    }

    internal override async Task<CMwNod?> ResolveAsyncCore(CancellationToken cancellationToken)
        => await ResolveAsync(cancellationToken).ConfigureAwait(false);

    /// <inheritdoc cref="NodeRef.Retry"/>
    public new T? Retry() => (T?)base.Retry();

    /// <inheritdoc cref="NodeRef.RetryAsync"/>
    public new async Task<T?> RetryAsync(CancellationToken cancellationToken = default)
        => (T?)await base.RetryAsync(cancellationToken).ConfigureAwait(false);

    /// <inheritdoc/>
    public override void Clear() => SetStorage(null, null);

    /// <inheritdoc/>
    public override void Reset()
    {
        lock (Sync)
        {
            if (descriptor is null)
            {
                return;
            }

            descriptor.Resolution.Reset();
            revision++;
            node = null;
            error = null;
            validatedGeneration = -1;
        }
    }

    private (GbxRefTableNode? Source, long Version, T? Node, bool Resolved) Snapshot()
    {
        lock (Sync)
        {
            if (descriptor is null)
            {
                return (null, revision, node, true);
            }

            var snapshot = descriptor.Resolution.Snapshot();

            if (validatedGeneration == snapshot.Generation)
            {
                error?.Throw();

                if (snapshot.State is NodeRefState.Resolved or NodeRefState.Missing)
                {
                    return (descriptor, revision, (T?)snapshot.Node, true);
                }
            }

            return (descriptor, revision, null, false);
        }
    }

    private T? Validate(GbxRefTableNode source, CMwNod? value, long version, int generation)
    {
        if (value is not null && value is not T)
        {
            var failure = new InvalidCastException($"Expected {typeof(T).Name}, received Gbx class 0x{value.GetId():X8}.");

            lock (Sync)
            {
                if (ReferenceEquals(descriptor, source) && revision == version && source.Resolution.Generation == generation)
                {
                    error = ExceptionDispatchInfo.Capture(failure);
                    validatedGeneration = generation;
                }
            }

            throw failure;
        }

        var typed = (T?)value;

        lock (Sync)
        {
            if (ReferenceEquals(descriptor, source) && revision == version && source.Resolution.Generation == generation)
            {
                error = null;
                validatedGeneration = generation;
            }
        }

        return typed;
    }

    object IDeepCloneable.DeepClone(DeepCloneContext context)
    {
        var clone = new NodeRef<T>();
        context.Register(this, clone);

        if (descriptor is not null)
        {
            clone.descriptor = context.Clone(descriptor);
        }

        if (descriptor is null)
        {
            clone.node = context.Clone(node);
        }
        else if (State == NodeRefState.Resolved)
        {
            clone.validatedGeneration = clone.descriptor!.Resolution.Generation;
        }

        return clone;
    }
}
