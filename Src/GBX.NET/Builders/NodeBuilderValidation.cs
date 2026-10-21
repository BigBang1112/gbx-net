using System.Runtime.CompilerServices;

namespace GBX.NET.Builders;

internal static class NodeBuilderValidation
{
    public static void Validate(CMwNod node, GameVersion game)
    {
        var visited = new HashSet<CMwNod>(NodeIdentityComparer.Instance);
        var pending = new Stack<(CMwNod Node, string Path)>();
        pending.Push((node, node.GetType().Name));

        while (pending.Count > 0)
        {
            var entry = pending.Pop();
            if (!visited.Add(entry.Node)) continue;

            foreach (var chunk in entry.Node.Chunks)
            {
                if (chunk.GameVersion != GameVersion.Unspecified && (chunk.GameVersion & game) == 0)
                {
                    throw new InvalidOperationException($"'{entry.Path}' contains chunk 0x{chunk.Id:X8}, which is incompatible with {game}.");
                }
            }

            foreach (var reference in entry.Node.EnumerateReferences())
            {
                // External references retain deferred compatibility. Inspecting a
                // cached node must never initiate resolution or filesystem I/O.
                if (reference.Value.CachedNode is { } child)
                {
                    pending.Push((child, entry.Path + "." + reference.Key));
                }
            }
        }
    }

    private sealed class NodeIdentityComparer : IEqualityComparer<CMwNod>
    {
        public static readonly NodeIdentityComparer Instance = new();

        public bool Equals(CMwNod? x, CMwNod? y) => ReferenceEquals(x, y);
        public int GetHashCode(CMwNod obj) => RuntimeHelpers.GetHashCode(obj);
    }
}
