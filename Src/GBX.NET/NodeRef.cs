using GBX.NET.Components;
using System.Runtime.ExceptionServices;

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
    public abstract NodeRefKind Kind { get; }
    public abstract NodeRefState State { get; }
    public abstract GbxRefTableFile? File { get; }
    public abstract GbxRefTableResource? Resource { get; }
    public CMwNod? CachedTarget => CachedTargetCore;
    internal abstract CMwNod? CachedTargetCore { get; }
    public abstract Exception? Error { get; }
    internal abstract GbxRefTableNode? Descriptor { get; }
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
                if (error is not null && validatedGeneration == source.Resolution.Generation) return NodeRefState.Failed;
                var state = source.Resolution.State;
                return state == NodeRefState.Resolved && validatedGeneration != source.Resolution.Generation ? NodeRefState.Unresolved : state;
            }
        }
    }
    public override GbxRefTableFile? File { get { lock (sync) return descriptor as GbxRefTableFile; } }
    public override GbxRefTableResource? Resource { get { lock (sync) return descriptor as GbxRefTableResource; } }
    public new T? CachedTarget { get { lock (sync) return descriptor is null || validatedGeneration == descriptor.Resolution.Generation ? target : null; } }
    internal override CMwNod? CachedTargetCore => CachedTarget;
    public override Exception? Error { get { lock (sync) return descriptor is { } source && validatedGeneration != source.Resolution.Generation ? source.Resolution.Error : error?.SourceException ?? descriptor?.Resolution.Error; } }
    internal override GbxRefTableNode? Descriptor { get { lock (sync) return descriptor; } }

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

    public void SetExternal(GbxRefTableFile file) => SetDescriptor(file ?? throw new ArgumentNullException(nameof(file)));
    public void SetExternal(GbxRefTableResource resource) => SetDescriptor(resource ?? throw new ArgumentNullException(nameof(resource)));

    public void CopyFrom(NodeRef<T> reference)
    {
        if (reference is null) throw new ArgumentNullException(nameof(reference));
        if (ReferenceEquals(reference, this)) return;
        if (reference.Descriptor is { } source) SetDescriptor(source);
        else SetEmbedded(reference.CachedTarget);
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

    public T? Resolve()
    {
        var (source, version, generation, embedded) = Snapshot();
        if (source is null) return embedded;
        return Validate(source, source.Resolution.Resolve(source), version, generation);
    }

    public bool TryResolve(out T? value)
    {
        value = Resolve();
        return value is not null;
    }

    public async Task<T?> ResolveAsync(CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var (source, version, generation, embedded) = Snapshot();
        if (source is null) return embedded;
        var value = await source.Resolution.ResolveAsync(source, cancellationToken).ConfigureAwait(false);
        return Validate(source, value, version, generation);
    }

    public async Task<(bool Success, T? Target)> TryResolveAsync(CancellationToken cancellationToken = default)
    {
        var value = await ResolveAsync(cancellationToken).ConfigureAwait(false);
        return (value is not null, value);
    }

    public T? Retry()
    {
        Reset();
        return Resolve();
    }

    public async Task<T?> RetryAsync(CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        Reset();
        return await ResolveAsync(cancellationToken).ConfigureAwait(false);
    }

    private void Reset()
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

    private (GbxRefTableNode? Source, long Version, int Generation, T? Embedded) Snapshot()
    {
        lock (sync)
        {
            var generation = descriptor?.Resolution.Generation ?? -1;
            if (validatedGeneration == generation) error?.Throw();
            return (descriptor, revision, generation, target);
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
                target = typed;
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
        if (descriptor is null || State == NodeRefState.Resolved) clone.target = context.Clone(target);
        if (clone.target is not null && clone.descriptor is not null) clone.validatedGeneration = clone.descriptor.Resolution.Generation;
        return clone;
    }
}
