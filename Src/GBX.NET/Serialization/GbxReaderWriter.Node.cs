using System.Diagnostics.CodeAnalysis;

namespace GBX.NET.Serialization;

public partial interface IGbxReaderWriter
{
    /// <summary>
    /// Serializes a direct node through nullable reference storage without resolving external dependencies.
    /// </summary>
    void Node<T>([NotNullIfNotNull(nameof(reference))] ref NodeRef<T>? reference) where T : CMwNod, new();
}

public partial class GbxReaderWriter
{
    /// <inheritdoc/>
    public void Node<T>([NotNullIfNotNull(nameof(reference))] ref NodeRef<T>? reference) where T : CMwNod, new()
    {
        if (Reader is not null)
        {
            GBX.NET.NodeRef.SetEmbedded(ref reference, Reader.ReadNode<T>());
        }

        Writer?.WriteNode(reference?.CachedNode);
    }
}
