using GBX.NET.Components;
using System.Runtime.ExceptionServices;
using System.Diagnostics.CodeAnalysis;

namespace GBX.NET;

public enum NodeRefKind
{
    None,
    Embedded,
    ExternalFile,
    ExternalResource
}

public enum NodeRefState
{
    Unresolved,
    Resolving,
    Resolved,
    Missing,
    Failed
}

/// <summary>A stored node reference. Inspecting its metadata never resolves its target.</summary>
public abstract class NodeRef
{
    private protected NodeRef() { }

    internal static NodeRef<T> GetOrCreate<T>(ref NodeRef<T>? reference) where T : CMwNod
    {
        var existing = Volatile.Read(ref reference);
        if (existing is not null) return existing;
        var created = new NodeRef<T>();
        return Interlocked.CompareExchange(ref reference, created, null) ?? created;
    }

    internal static void SetEmbedded<T>([NotNullIfNotNull(nameof(value))] ref NodeRef<T>? reference, T? value) where T : CMwNod
    {
        if (value is null) reference?.Clear();
        else GetOrCreate(ref reference).SetEmbedded(value);
    }

    internal static void CopyFrom<T>(ref NodeRef<T>? reference, NodeRef<T>? source) where T : CMwNod
    {
        if (source is null || source.Kind == NodeRefKind.None) reference?.Clear();
        else GetOrCreate(ref reference).CopyFrom(source);
    }

    /// <summary>How the reference is stored, independently of whether its target has been resolved.</summary>
    public abstract NodeRefKind Kind { get; }
    /// <summary>The current resolution state. Inspecting it does not load the target.</summary>
    public abstract NodeRefState State { get; }
    /// <summary>The external descriptor, or null for an embedded or absent reference.</summary>
    public abstract GbxRefTableNode? Descriptor { get; }
    /// <summary>The external file descriptor, if this reference points to a file.</summary>
    public GbxRefTableFile? File => Descriptor as GbxRefTableFile;
    /// <summary>The external resource descriptor, if this reference points to a resource.</summary>
    public GbxRefTableResource? Resource => Descriptor as GbxRefTableResource;
    /// <summary>The already validated target, or null. This property never starts resolution.</summary>
    public CMwNod? CachedTarget => CachedTargetCore;
    internal abstract CMwNod? CachedTargetCore { get; }
    /// <summary>The cached resolver or target-type error, if any.</summary>
    public abstract Exception? Error { get; }

    /// <summary>Stores a file or resource descriptor without loading its target. Handles share the descriptor's resolution cache.</summary>
    public abstract void SetExternal(GbxRefTableNode descriptor);

    /// <summary>Resolves the target, returning null for an absent or missing target. Other errors propagate.</summary>
    public CMwNod? Resolve() => ResolveCore();
    internal abstract CMwNod? ResolveCore();

    /// <summary>Resolves the target asynchronously. Cancellation stops this waiter without canceling a shared load.</summary>
    public Task<CMwNod?> ResolveAsync(CancellationToken cancellationToken = default) => ResolveAsyncCore(cancellationToken);
    internal abstract Task<CMwNod?> ResolveAsyncCore(CancellationToken cancellationToken);

    /// <summary>Returns whether a target was found. Resolver and type errors still propagate.</summary>
#if NET8_0_OR_GREATER
    public bool TryResolve([NotNullWhen(true)] out CMwNod? value)
#else
    public bool TryResolve(out CMwNod? value)
#endif
    {
        value = Resolve();
        return value is not null;
    }

    /// <summary>Returns a named outcome for asynchronous resolution. Resolver and type errors still propagate.</summary>
    public async Task<NodeRefResolveResult<CMwNod>> TryResolveAsync(CancellationToken cancellationToken = default)
        => new(await ResolveAsync(cancellationToken).ConfigureAwait(false));

    /// <summary>Clears this handle without changing other handles that share its external descriptor.</summary>
    public abstract void Clear();

    /// <summary>Invalidates the descriptor's shared cache without loading its target. Active loads cannot be reset.</summary>
    public abstract void Reset();

    /// <summary>Invalidates the shared cache and resolves the target again.</summary>
    public CMwNod? Retry()
    {
        Reset();
        return Resolve();
    }

    /// <summary>Invalidates the shared cache and resolves the target again asynchronously.</summary>
    public async Task<CMwNod?> RetryAsync(CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        Reset();
        return await ResolveAsync(cancellationToken).ConfigureAwait(false);
    }
}

/// <summary>Stores an embedded target or an external descriptor independently of resolution.</summary>
public sealed class NodeRef<T> : NodeRef, IDeepCloneable where T : CMwNod
{
    private readonly object sync = new();
    private GbxRefTableNode? descriptor;
    private T? target;
    private ExceptionDispatchInfo? error;
    private long revision;
    private int validatedGeneration = -1;

    public override NodeRefKind Kind { get { lock (sync) return descriptor switch
    {
        GbxRefTableFile => NodeRefKind.ExternalFile,
        GbxRefTableResource => NodeRefKind.ExternalResource,
        _ => target is null ? NodeRefKind.None : NodeRefKind.Embedded
    }; } }

    public override NodeRefState State
    {
        get
        {
            lock (sync)
            {
                if (descriptor is not { } source) return target is null ? NodeRefState.Unresolved : NodeRefState.Resolved;
                var snapshot = source.Resolution.Snapshot();
                if (error is not null && validatedGeneration == snapshot.Generation) return NodeRefState.Failed;
                return snapshot.State == NodeRefState.Resolved && validatedGeneration != snapshot.Generation ? NodeRefState.Unresolved : snapshot.State;
            }
        }
    }
    /// <inheritdoc cref="NodeRef.CachedTarget"/>
    public new T? CachedTarget
    {
        get
        {
            lock (sync)
            {
                if (descriptor is not { } source) return target;
                var snapshot = source.Resolution.Snapshot();
                return validatedGeneration == snapshot.Generation && error is null ? (T?)snapshot.Target : null;
            }
        }
    }
    internal override CMwNod? CachedTargetCore => CachedTarget;
    public override Exception? Error
    {
        get
        {
            lock (sync)
            {
                if (descriptor is not { } source) return error?.SourceException;
                var snapshot = source.Resolution.Snapshot();
                return validatedGeneration == snapshot.Generation ? error?.SourceException ?? snapshot.Error : snapshot.Error;
            }
        }
    }
    public override GbxRefTableNode? Descriptor { get { lock (sync) return descriptor; } }

    /// <summary>Stores an embedded target and clears the external descriptor. Null clears the reference.</summary>
    public void SetEmbedded(T? value)
    {
        lock (sync)
        {
            revision++;
            descriptor = null;
            target = value;
            error = null;
            validatedGeneration = -1;
        }
    }

    /// <inheritdoc/>
    public override void SetExternal(GbxRefTableNode descriptor) => SetDescriptor(descriptor ?? throw new ArgumentNullException(nameof(descriptor)));

    /// <summary>Copies stored reference metadata or an embedded target without resolving it. This handle retains its identity.</summary>
    public void CopyFrom(NodeRef<T> reference)
    {
        if (reference is null) throw new ArgumentNullException(nameof(reference));
        if (ReferenceEquals(reference, this)) return;
        GbxRefTableNode? source;
        T? embedded;
        lock (reference.sync)
        {
            source = reference.descriptor;
            embedded = reference.target;
        }
        if (source is not null) SetDescriptor(source);
        else SetEmbedded(embedded);
    }

    internal void SetDescriptor(GbxRefTableNode value)
    {
        lock (sync)
        {
            revision++;
            descriptor = value;
            target = null;
            error = null;
            validatedGeneration = -1;
        }
    }

    internal void SetReadTarget(CMwNod? value)
    {
        if (value is null || value is T) SetEmbedded((T?)value);
        else throw new InvalidCastException($"Expected {typeof(T).Name}, received Gbx class 0x{value.GetId():X8}.");
    }

    /// <inheritdoc cref="NodeRef.Resolve"/>
    public new T? Resolve()
    {
        var (source, version, embedded) = Snapshot();
        if (source is null) return embedded;
        var result = source.Resolution.Resolve(source);
        return Validate(source, result.Target, version, result.Generation);
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
        var (source, version, embedded) = Snapshot();
        if (source is null) return embedded;
        var result = await source.Resolution.ResolveAsync(source, cancellationToken).ConfigureAwait(false);
        return Validate(source, result.Target, version, result.Generation);
    }

    internal override async Task<CMwNod?> ResolveAsyncCore(CancellationToken cancellationToken)
        => await ResolveAsync(cancellationToken).ConfigureAwait(false);

    /// <inheritdoc cref="NodeRef.TryResolveAsync"/>
    public new async Task<NodeRefResolveResult<T>> TryResolveAsync(CancellationToken cancellationToken = default)
    {
        var value = await ResolveAsync(cancellationToken).ConfigureAwait(false);
        return new(value);
    }

    /// <inheritdoc cref="NodeRef.Retry"/>
    public new T? Retry()
    {
        Reset();
        return Resolve();
    }

    /// <inheritdoc cref="NodeRef.RetryAsync"/>
    public new async Task<T?> RetryAsync(CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        Reset();
        return await ResolveAsync(cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public override void Clear() => SetEmbedded(null);

    /// <inheritdoc/>
    public override void Reset()
    {
        lock (sync)
        {
            if (descriptor is null) return;
            descriptor.Resolution.Reset();
            revision++;
            target = null;
            error = null;
            validatedGeneration = -1;
        }
    }

    private (GbxRefTableNode? Source, long Version, T? Embedded) Snapshot()
    {
        lock (sync)
        {
            var generation = descriptor?.Resolution.Generation ?? -1;
            if (validatedGeneration == generation) error?.Throw();
            return (descriptor, revision, target);
        }
    }

    private T? Validate(GbxRefTableNode source, CMwNod? value, long version, int generation)
    {
        if (value is not null && value is not T)
        {
            var failure = new InvalidCastException($"Expected {typeof(T).Name}, received Gbx class 0x{value.GetId():X8}.");
            lock (sync)
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
        lock (sync)
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
        if (descriptor is not null) clone.descriptor = context.Clone(descriptor);
        if (descriptor is null) clone.target = context.Clone(target);
        else if (State == NodeRefState.Resolved) clone.validatedGeneration = clone.descriptor!.Resolution.Generation;
        return clone;
    }
}
