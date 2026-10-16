using System.Diagnostics.CodeAnalysis;

namespace GBX.NET.Serialization;

public partial class GbxReaderWriter
{
    public NodeRefList<T>? EmbeddedNodeRefList<T>(
        NodeRefList<T>? references,
        int? length = null,
        int version = 0,
        bool deprec = false)
        where T : CMwNod?, IReadable, IWritable, new()
    {
        if (Reader is not null)
        {
            if (deprec)
            {
                Reader.ReadDeprecVersion();
            }

            var values = length.HasValue
                ? Reader.ReadListReadable<T>(length.Value, version: version)
                : Reader.ReadListReadable<T>(version: version);
            references = new(values);
        }

        if (Writer is not null)
        {
            if (deprec)
            {
                Writer.WriteDeprecVersion();
            }

            var values = references?.GetEmbeddedNodes().ToList();

            if (length.HasValue)
            {
                for (var i = 0; i < length.Value; i++)
                {
                    Writer.WriteWritable(values is not null && i < values.Count ? values[i] : default, version);
                }
            }
            else
            {
                Writer.WriteListWritable(values, version: version);
            }
        }

        return references;
    }

    public void NodeRef<T>(NodeRef<T> reference) where T : CMwNod?
    {
        if (reference is null)
        {
            throw new ArgumentNullException(nameof(reference));
        }

        Reader?.ReadNodeRef(reference);
        Writer?.WriteNodeRef(reference);
    }

    /// <summary>
    /// Serializes a stored reference without allocating a handle for an absent node.
    /// </summary>
    public void NodeRef<T>([NotNullIfNotNull(nameof(reference))] ref NodeRef<T>? reference) where T : CMwNod?
    {
        Reader?.ReadNodeRef(ref reference);
        Writer?.WriteNodeRef(reference);
    }

    public NodeRefList<T>? ListNodeRef<T>(NodeRefList<T>? references, int? length = null) where T : CMwNod?
    {
        if (Reader is not null)
        {
            references = Reader.ReadListNodeRef<T>(length);
        }

        Writer?.WriteListNodeRef(references, length);
        return references;
    }

    public void ListNodeRef<T>(ref NodeRefList<T>? references, int? length = null) where T : CMwNod?
        => references = ListNodeRef(references, length);

    public NodeRefList<T>? ListNodeRef_deprec<T>(NodeRefList<T>? references, int? length = null) where T : CMwNod?
    {
        if (Reader is not null)
        {
            references = Reader.ReadListNodeRef_deprec<T>(length);
        }

        Writer?.WriteListNodeRef_deprec(references, length);
        return references;
    }

    public void ListNodeRef_deprec<T>(ref NodeRefList<T>? references, int? length = null) where T : CMwNod?
        => references = ListNodeRef_deprec(references, length);

    public NodeRefList<T>[]? JaggedArrayNodeRef<T>(
        NodeRefList<T>[]? references,
        int? innerLength = null,
        int? outerLength = null) where T : CMwNod?
    {
        if (Reader is not null)
        {
            references = Reader.ReadJaggedListNodeRef<T>(innerLength, outerLength);
        }

        Writer?.WriteJaggedArrayNodeRef(references, innerLength, outerLength);
        return references;
    }

    public void JaggedArrayNodeRef<T>(
        ref NodeRefList<T>[]? references,
        int? innerLength = null,
        int? outerLength = null) where T : CMwNod?
        => references = JaggedArrayNodeRef(references, innerLength, outerLength);
}
