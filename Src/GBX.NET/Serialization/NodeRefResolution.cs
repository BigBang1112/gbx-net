using GBX.NET.Components;
using System.Runtime.ExceptionServices;

namespace GBX.NET.Serialization;

internal sealed class NodeRefResolution
{
    private static readonly AsyncLocal<HashSet<GbxRefTableNode>?> resolving = new();
#if NET9_0_OR_GREATER
    private readonly Lock sync = new();
#else
    private readonly object sync = new();
#endif
    private Task<CMwNod?>? task;
    private CMwNod? node;
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

    internal CMwNod? CachedNode
    {
        get
        {
            lock (sync)
            {
                return node;
            }
        }
    }

    internal NodeRefResolutionSnapshot Snapshot()
    {
        lock (sync)
        {
            return new(generation, state, node, error?.SourceException);
        }
    }

    internal void Seed(CMwNod node)
    {
        lock (sync)
        {
            ResetCore();
            task = Task.FromResult<CMwNod?>(node);
            this.node = node;
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
                Complete(completion, descriptor.ResolveNode());
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
            throw new InvalidOperationException("An external reference was accessed while its node was still being parsed.");
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
            Complete(completion, await descriptor.ResolveNodeAsync().ConfigureAwait(false));
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
            this.node = node;
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
        lock (sync)
        {
            ResetCore();
        }
    }

    internal void Update<T>(ref T field, T value)
    {
        lock (sync)
        {
            if (EqualityComparer<T>.Default.Equals(field, value))
            {
                return;
            }

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
        node = null;
        generation++;
        error = null;
        state = NodeRefState.Unresolved;
    }
}
