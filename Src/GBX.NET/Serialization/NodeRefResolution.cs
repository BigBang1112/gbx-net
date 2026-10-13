using GBX.NET.Components;
using System.Runtime.ExceptionServices;

namespace GBX.NET.Serialization;

internal sealed class NodeRefResolution
{
    private static readonly AsyncLocal<HashSet<GbxRefTableNode>?> resolving = new();
    private readonly object sync = new();
    private Task<CMwNod?>? task;
    private CMwNod? target;
    private ExceptionDispatchInfo? error;
    private NodeRefState state;
    private int generation;

    internal int Generation
    {
        get
        { 
            lock (sync)
            {
                return generation;
            }
        }
    }

    internal CMwNod? CachedTarget
    {
        get
        {
            lock (sync)
            {
                return target;
            }
        }
    }

    internal NodeRefResolutionSnapshot Snapshot()
    {
        lock (sync) return new(generation, state, target, error?.SourceException);
    }

    internal void Seed(CMwNod target)
    {
        lock (sync)
        {
            ResetCore();
            task = Task.FromResult<CMwNod?>(target);
            this.target = target;
            state = NodeRefState.Resolved;
        }
    }

    internal NodeRefResolutionResult Resolve(GbxRefTableNode descriptor)
    {
        var (pending, completion, generation) = Begin(descriptor);

        if (completion is not null)
        {
            var previous = Enter(descriptor);

            try
            {
                Complete(completion, descriptor.ResolveTarget());
            }
            catch (Exception ex)
            {
                Fail(completion, ex);
            }
            finally
            {
                resolving.Value = previous;
            }
        }

        return new(pending.GetAwaiter().GetResult(), generation);
    }

    internal async Task<NodeRefResolutionResult> ResolveAsync(GbxRefTableNode descriptor, CancellationToken cancellationToken)
    {
        var (pending, completion, generation) = Begin(descriptor);

        if (completion is not null)
        {
            _ = LoadAsync(descriptor, completion);
        }

        if (!cancellationToken.CanBeCanceled)
        {
            return new(await pending.ConfigureAwait(false), generation);
        }

        var canceled = new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously);
        
        using (cancellationToken.Register(() => canceled.TrySetResult(true)))
        {
            if (await Task.WhenAny(pending, canceled.Task).ConfigureAwait(false) != pending)
            {
                cancellationToken.ThrowIfCancellationRequested();
            }
        }

        return new(await pending.ConfigureAwait(false), generation);
    }

    private (Task<CMwNod?> Task, TaskCompletionSource<CMwNod?>? Completion, int Generation) Begin(GbxRefTableNode descriptor)
    {
        if (resolving.Value?.Contains(descriptor) == true)
        {
            throw new InvalidOperationException("An external reference was accessed while its target was still being parsed.");
        }

        lock (sync)
        {
            if (task is not null)
            {
                return (task, null, generation);
            }

            var completion = new TaskCompletionSource<CMwNod?>(TaskCreationOptions.RunContinuationsAsynchronously);
            
            task = completion.Task;
            state = NodeRefState.Resolving;

            return (task, completion, generation);
        }
    }

    private static HashSet<GbxRefTableNode>? Enter(GbxRefTableNode descriptor)
    {
        var previous = resolving.Value;
        resolving.Value = previous is null ? [descriptor] : new HashSet<GbxRefTableNode>(previous) { descriptor };
        return previous;
    }

    private async Task LoadAsync(GbxRefTableNode descriptor, TaskCompletionSource<CMwNod?> completion)
    {
        var previous = Enter(descriptor);

        try
        {
            Complete(completion, await descriptor.ResolveTargetAsync().ConfigureAwait(false));
        }
        catch (Exception ex)
        {
            Fail(completion, ex);
        }
        finally
        {
            resolving.Value = previous;
        }
    }

    private void Complete(TaskCompletionSource<CMwNod?> completion, CMwNod? node)
    {
        lock (sync)
        {
            target = node;
            state = node is null ? NodeRefState.Missing : NodeRefState.Resolved;
            completion.SetResult(node);
        }
    }

    private void Fail(TaskCompletionSource<CMwNod?> completion, Exception exception)
    {
        lock (sync)
        {
            error = ExceptionDispatchInfo.Capture(exception);
            state = NodeRefState.Failed;
            completion.SetException(exception);
        }
    }

    internal void Reset()
    {
        lock (sync) ResetCore();
    }

    internal void Update<T>(ref T field, T value)
    {
        lock (sync)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return;
            // Keep metadata changes atomic with cache invalidation and the start of a load.
            ResetCore();
            field = value;
        }
    }

    private void ResetCore()
    {
        if (state == NodeRefState.Resolving)
        {
            throw new InvalidOperationException("Cannot change or retry a reference while it is resolving.");
        }

        task = null;
        target = null;
        generation++;
        error = null;
        state = NodeRefState.Unresolved;
    }
}
