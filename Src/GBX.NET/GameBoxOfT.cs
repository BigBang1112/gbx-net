using System.Collections;
using System.Reflection;

namespace GBX.NET;

/// <summary>
/// A known serialized GameBox node with additional attributes. This class can represent deserialized .Gbx file.
/// </summary>
/// <typeparam name="T">The main node of the GBX. Nodes to use are located in the GBX.NET.Engines namespace.</typeparam>
public class GameBox<T> : GameBox where T : Node
{
    /// <summary>
    /// Deserialized node from GBX.
    /// </summary>
    public new T Node
    {
        get => (T)base.Node!;
    }

    public GameBox(T node) : base(node)
    {

    }

    public GameBox(GameBoxHeader header, GameBoxRefTable? refTable, string? fileName = null) : base(header, refTable, fileName)
    {
        
    }

    /// <summary>
    /// Creates a header chunk based on the attributes from <typeparamref name="TChunk"/>.
    /// </summary>
    /// <typeparam name="TChunk">A chunk type compatible with <see cref="IHeaderChunk"/>.</typeparam>
    /// <returns>A newly created chunk.</returns>
    public TChunk? CreateHeaderChunk<TChunk>() where TChunk : Chunk, IHeaderChunk
    {
        if (Node is not INodeHeader nodeHeader)
        {
            return null;
        }

        return nodeHeader.HeaderChunks.Create<TChunk>();
    }

    /// <summary>
    /// Removes all header chunks.
    /// </summary>
    public bool RemoveAllHeaderChunks()
    {
        if (Node is not INodeHeader nodeHeader)
        {
            return false;
        }

        nodeHeader.HeaderChunks.Clear();

        return true;
    }

    /// <summary>
    /// Removes a header chunk based on the attributes from <typeparamref name="TChunk"/>.
    /// </summary>
    /// <typeparam name="TChunk">A chunk type compatible with <see cref="IHeaderChunk"/>.</typeparam>
    /// <returns>True, if the chunk was removed, otherwise false.</returns>
    public bool RemoveHeaderChunk<TChunk>() where TChunk : Chunk, IHeaderChunk
    {
        if (Node is not INodeHeader nodeHeader)
        {
            return false;
        }

        return nodeHeader.HeaderChunks.RemoveWhere(x => x is T) > 0;
    }

    /// <summary>
    /// Creates a body chunk based on the attributes from <typeparamref name="TChunk"/>.
    /// </summary>
    /// <typeparam name="TChunk">A chunk type that isn't a header chunk.</typeparam>
    /// <param name="data">If the chunk is <see cref="ISkippableChunk"/>, the bytes to initiate the chunks with, unparsed. If it's not a skippable chunk, data is parsed immediately.</param>
    /// <returns>A newly created chunk.</returns>
    public TChunk CreateBodyChunk<TChunk>(byte[] data) where TChunk : Chunk
    {
        var chunk = Node.Chunks.Create<TChunk>();

        if (chunk is ISkippableChunk s)
        {
            s.Data = data;

            if (data is null || data.Length == 0)
            {
                s.Discovered = true;
            }

            chunk.OnLoad();
        }
        else
        {
            using var ms = new MemoryStream(data);
            using var r = new GameBoxReader(ms);

            var rw = new GameBoxReaderWriter(r);
            ((IReadableWritableChunk)chunk).ReadWrite(Node, rw);
        }

        return chunk;
    }

    /// <summary>
    /// Creates a body chunk based on the attributes from <typeparamref name="TChunk"/> with no inner data provided.
    /// </summary>
    /// <typeparam name="TChunk">A chunk type that isn't a header chunk.</typeparam>
    /// <returns>A newly created chunk.</returns>
    public TChunk CreateBodyChunk<TChunk>() where TChunk : Chunk
    {
        return CreateBodyChunk<TChunk>(Array.Empty<byte>());
    }

    /// <summary>
    /// Removes all body chunks.
    /// </summary>
    public void RemoveAllBodyChunks()
    {
        Node.Chunks.Clear();
    }

    /// <summary>
    /// Removes a body chunk based on the attributes from <typeparamref name="TChunk"/>.
    /// </summary>
    /// <typeparam name="TChunk">A chunk type that isn't a header chunk.</typeparam>
    /// <returns>True, if the chunk was removed, otherwise false.</returns>
    public bool RemoveBodyChunk<TChunk>() where TChunk : Chunk
    {
        return Node.Chunks.Remove<TChunk>();
    }

    /// <summary>
    /// Discovers all chunks in the GBX.
    /// </summary>
    /// <exception cref="AggregateException"/>
    public void DiscoverAllChunks()
    {
        Node.DiscoverAllChunks();
    }

    /// <summary>
    /// Implicitly casts <see cref="GameBox{T}"/> to its <see cref="GameBox{T}.Node"/>.
    /// </summary>
    /// <param name="gbx"></param>
    public static implicit operator T(GameBox<T> gbx) => gbx.Node;
}