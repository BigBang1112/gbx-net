using GBX.NET.Components;
using System.Runtime.ExceptionServices;

namespace GBX.NET.Serialization;

internal sealed class NodeRefResolution
{
    private static readonly AsyncLocal<HashSet<GbxRefTableNode>?> resolving = new();
    private readonly object sync = new();
    private Task<CMwNod?>? task;
    private ExceptionDispatchInfo? error;
    private NodeRefState state;
    private int generation;

    public NodeRefState State { get { lock (sync) return state; } }
    public int Generation { get { lock (sync) return generation; } }
    public Exception? Error { get { lock (sync) return error?.SourceException; } }
    public CMwNod? CachedTarget { get { lock (sync) return state == NodeRefState.Resolved ? task!.GetAwaiter().GetResult() : null; } }

    public void Seed(CMwNod target)
    {
        lock (sync)
        {
            task = Task.FromResult<CMwNod?>(target);
            generation++;
            state = NodeRefState.Resolved;
        }
    }

    public CMwNod? Resolve(GbxRefTableNode descriptor)
    {
        var (pending, completion) = Begin(descriptor);

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

        return pending.GetAwaiter().GetResult();
    }

    public async Task<CMwNod?> ResolveAsync(GbxRefTableNode descriptor, CancellationToken cancellationToken)
    {
        var (pending, completion) = Begin(descriptor);

        if (completion is not null)
        {
            _ = LoadAsync(descriptor, completion);
        }

        if (!cancellationToken.CanBeCanceled)
        {
            return await pending.ConfigureAwait(false);
        }

        var canceled = new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously);
        
        using (cancellationToken.Register(() => canceled.TrySetResult(true)))
        {
            if (await Task.WhenAny(pending, canceled.Task).ConfigureAwait(false) != pending)
            {
                cancellationToken.ThrowIfCancellationRequested();
            }
        }

        return await pending.ConfigureAwait(false);
    }

    private (Task<CMwNod?> Task, TaskCompletionSource<CMwNod?>? Completion) Begin(GbxRefTableNode descriptor)
    {
        if (resolving.Value?.Contains(descriptor) == true)
        {
            throw new InvalidOperationException("An external reference was accessed while its target was still being parsed.");
        }

        lock (sync)
        {
            if (task is not null)
            {
                return (task, null);
            }

            var completion = new TaskCompletionSource<CMwNod?>(TaskCreationOptions.RunContinuationsAsynchronously);
            
            task = completion.Task;
            state = NodeRefState.Resolving;

            return (task, completion);
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
            state = node is null ? NodeRefState.Missing : NodeRefState.Resolved;
        }

        completion.SetResult(node);
    }

    private void Fail(TaskCompletionSource<CMwNod?> completion, Exception exception)
    {
        lock (sync)
        {
            error = ExceptionDispatchInfo.Capture(exception);
            state = NodeRefState.Failed;
        }

        completion.SetException(exception);
    }

    public void Reset()
    {
        lock (sync)
        {
            if (state == NodeRefState.Resolving)
            {
                throw new InvalidOperationException("Cannot retry a reference while it is resolving.");
            }

            task = null;
            generation++;
            error = null;
            state = NodeRefState.Unresolved;
        }
    }
}
