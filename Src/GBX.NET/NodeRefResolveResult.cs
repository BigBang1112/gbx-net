#if NET8_0_OR_GREATER
using System.Diagnostics.CodeAnalysis;
#endif

namespace GBX.NET;

/// <summary>The outcome of trying to resolve a node reference. Resolution errors still propagate to the caller.</summary>
public readonly struct NodeRefResolveResult<T> where T : CMwNod
{
    /// <summary>Whether an embedded or external target was found.</summary>
#if NET8_0_OR_GREATER
    [MemberNotNullWhen(true, nameof(Target))]
#endif
    public bool Success => Target is not null;

    /// <summary>The resolved target, or null when the reference is absent or its target is missing.</summary>
    public T? Target { get; }

    internal NodeRefResolveResult(T? target) => Target = target;
}
