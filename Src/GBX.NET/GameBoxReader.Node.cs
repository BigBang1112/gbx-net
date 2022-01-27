namespace GBX.NET;

public partial class GameBoxReader
{
    /// <summary>
    /// Reads an <see cref="int"/> containing the node reference index, then the node using the <see cref="Node"/>'s Parse method. The index is also checked if it isn't a part of a reference table, which currently returns null.
    /// </summary>
    /// <param name="nodeRefIndex">Index of the node reference.</param>
    /// <param name="body">Body used to store node references.</param>
    /// <returns>A node, or null if the index is -1 or the node is from reference table.</returns>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="body"/> is null.</exception>
    public Node? ReadNodeRef(out int nodeRefIndex, GameBoxBody body)
    {
        if (body is null)
        {
            throw new ArgumentNullException(nameof(body));
        }

        var index = ReadInt32() - 1; // GBX seems to start the index at 1

        nodeRefIndex = index;

        // If aux node index is below 0 or the node index is part of the reference table
        if (index < 0 || IsRefTableNode(body, index))
        {
            return null;
        }

        var node = default(Node?);

        if (NodeShouldBeParsed(body, index))
        {
            node = Node.Parse(this, classId: null, progress: null, logger)!;
        }

        TryGetNodeIfNullOrAssignExistingNode(body, index, ref node);

        if (node is null)
        {
            // Sometimes it indexes the node reference that is further in the expected indexes
            return body.AuxilaryNodes.Last().Value; // So it grabs the last one instead, needs to be further tested
        }

        return node;
    }

    /// <summary>
    /// Reads an <see cref="int"/> containing the node reference index, then the node using the <see cref="Node"/>'s Parse method. The index is also checked if it isn't a part of a reference table, which currently returns null.
    /// </summary>
    /// <param name="nodeRefIndex">Index of the node reference.</param>
    /// <returns>A node, or null if the index is -1 or the node is from reference table.</returns>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="PropertyNullException"><see cref="Body"/> is null.</exception>
    public Node? ReadNodeRef(out int nodeRefIndex)
    {
        if (Body is null)
        {
            throw new PropertyNullException(nameof(Body));
        }

        return ReadNodeRef(out nodeRefIndex, Body);
    }

    /// <summary>
    /// Reads an <see cref="int"/> containing the node reference index, then the node using the <see cref="Node"/>'s Parse method. The index is also checked if it isn't a part of a reference table, which currently returns null.
    /// </summary>
    /// <param name="body">Body used to store node references.</param>
    /// <returns>A node, or null if the index is -1 or the node is from reference table.</returns>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="body"/> is null.</exception>
    public Node? ReadNodeRef(GameBoxBody body) => ReadNodeRef(out int _, body);

    /// <summary>
    /// Reads an <see cref="int"/> containing the node reference index, then the node using the <see cref="Node"/>'s Parse method. The index is also checked if it isn't a part of a reference table, which currently returns null.
    /// </summary>
    /// <returns>A node, or null if the index is -1 or the node is from reference table.</returns>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="PropertyNullException"><see cref="Body"/> is null.</exception>
    public Node? ReadNodeRef() => ReadNodeRef(out int _);

    /// <summary>
    /// Reads an <see cref="int"/> containing the node reference index, then the node using the <see cref="Node"/>'s Parse method. The index is also checked if it isn't a part of a reference table, which currently returns null.
    /// </summary>
    /// <typeparam name="T">Type of node.</typeparam>
    /// <param name="nodeRefIndex">Index of the node reference.</param>
    /// <param name="body">Body used to store node references.</param>
    /// <returns>A node, or null if the index is -1 or the node is from reference table.</returns>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="body"/> is null.</exception>
    public T? ReadNodeRef<T>(out int nodeRefIndex, GameBoxBody body) where T : Node
    {
        return ReadNodeRef(out nodeRefIndex, body) as T;
    }

    /// <summary>
    /// Reads an <see cref="int"/> containing the node reference index, then the node using the <see cref="Node"/>'s Parse method. The index is also checked if it isn't a part of a reference table, which currently returns null.
    /// </summary>
    /// <typeparam name="T">Type of node.</typeparam>
    /// <param name="nodeRefIndex">Index of the node reference.</param>
    /// <returns>A node, or null if the index is -1 or the node is from reference table.</returns>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="PropertyNullException"><see cref="Body"/> is null.</exception>
    public T? ReadNodeRef<T>(out int nodeRefIndex) where T : Node
    {
        return ReadNodeRef(out nodeRefIndex) as T;
    }

    /// <summary>
    /// Reads an <see cref="int"/> containing the node reference index, then the node using the <see cref="Node"/>'s Parse method. The index is also checked if it isn't a part of a reference table, which currently returns null.
    /// </summary>
    /// <typeparam name="T">Type of node.</typeparam>
    /// <param name="body">Body used to store node references.</param>
    /// <returns>A node, or null if the index is -1 or the node is from reference table.</returns>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="body"/> is null.</exception>
    public T? ReadNodeRef<T>(GameBoxBody body) where T : Node
    {
        return ReadNodeRef(body) as T;
    }

    /// <summary>
    /// Reads an <see cref="int"/> containing the node reference index, then the node using the <see cref="Node"/>'s Parse method. The index is also checked if it isn't a part of a reference table, which currently returns null.
    /// </summary>
    /// <typeparam name="T">Type of node.</typeparam>
    /// <returns>A node, or null if the index is -1 or the node is from reference table.</returns>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="PropertyNullException"><see cref="Body"/> is null.</exception>
    public T? ReadNodeRef<T>() where T : Node
    {
        return ReadNodeRef() as T;
    }

    /// <summary>
    /// Reads an <see cref="int"/> containing the node reference index, then the node using the <see cref="Node"/>'s ParseAsync method. The index is also checked if it isn't a part of a reference table, which currently returns null.
    /// </summary>
    /// <param name="body">Body used to store node references.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A node, or null if the index is -1 or the node is from reference table.</returns>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="body"/> is null.</exception>
    public async Task<Node?> ReadNodeRefAsync(GameBoxBody body, CancellationToken cancellationToken = default)
    {
        if (body is null)
        {
            throw new ArgumentNullException(nameof(body));
        }

        var index = ReadInt32() - 1; // GBX seems to start the index at 1

        // If aux node index is below 0 or the node index is part of the reference table
        if (index < 0 || IsRefTableNode(body, index))
        {
            return null;
        }

        var node = default(Node?);

        if (NodeShouldBeParsed(body, index))
        {
            node = await Node.ParseAsync(this, classId: null, logger, AsyncAction, cancellationToken);
        }

        TryGetNodeIfNullOrAssignExistingNode(body, index, ref node);

        if (node is null)
        {
            // Sometimes it indexes the node reference that is further in the expected indexes
            return body.AuxilaryNodes.Last().Value; // So it grabs the last one instead, needs to be further tested
        }

        return node;
    }

    /// <summary>
    /// Reads an <see cref="int"/> containing the node reference index, then the node using the <see cref="Node"/>'s ParseAsync method. The index is also checked if it isn't a part of a reference table, which currently returns null.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A node, or null if the index is -1 or the node is from reference table.</returns>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="PropertyNullException"><see cref="Body"/> is null.</exception>
    public async Task<Node?> ReadNodeRefAsync(CancellationToken cancellationToken = default)
    {
        if (Body is null)
        {
            throw new PropertyNullException(nameof(Body));
        }

        return await ReadNodeRefAsync(Body, cancellationToken);
    }

    /// <summary>
    /// Reads an <see cref="int"/> containing the node reference index, then the node using the <see cref="Node"/>'s ParseAsync method. The index is also checked if it isn't a part of a reference table, which currently returns null.
    /// </summary>
    /// <typeparam name="T">Type of node.</typeparam>
    /// <param name="body">Body used to store node references.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A node, or null if the index is -1 or the node is from reference table.</returns>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="body"/> is null.</exception>
    public async Task<T?> ReadNodeRefAsync<T>(GameBoxBody body, CancellationToken cancellationToken = default) where T : Node
    {
        return await ReadNodeRefAsync(body, cancellationToken) as T;
    }

    /// <summary>
    /// Reads an <see cref="int"/> containing the node reference index, then the node using the <see cref="Node"/>'s ParseAsync method. The index is also checked if it isn't a part of a reference table, which currently returns null.
    /// </summary>
    /// <typeparam name="T">Type of node.</typeparam>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A node, or null if the index is -1 or the node is from reference table.</returns>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="PropertyNullException"><see cref="Body"/> is null.</exception>
    public async Task<T?> ReadNodeRefAsync<T>(CancellationToken cancellationToken = default) where T : Node
    {
        return await ReadNodeRefAsync(cancellationToken) as T;
    }

    private static bool NodeShouldBeParsed(GameBoxBody body, int index)
    {
        // If index is 0 or bigger and the node wasn't read yet, or is null
        return index >= 0 && (!body.AuxilaryNodes.ContainsKey(index) || body.AuxilaryNodes[index] is null);
    }

    private static void TryGetNodeIfNullOrAssignExistingNode(GameBoxBody body, int index, ref Node? node)
    {
        if (node is null)
        {
            body.AuxilaryNodes.TryGetValue(index, out node); // Tries to get the available node from index
        }
        else
        {
            body.AuxilaryNodes[index] = node;
        }
    }

    private static bool IsRefTableNode(GameBoxBody body, int index)
    {
        var gbx = body.GBX;
        var refTable = gbx.RefTable;

        // First checks if reference table is used
        if (refTable is null || refTable.Files.Count <= 0 && refTable.Folders.Count <= 0)
        {
            // Reference table isn't used so it's a nested object
            return false;
        }

        var allFiles = refTable.GetAllFiles(); // Returns available external references

        if (allFiles.Any()) // If there's one
        {
            // Tries to get the one with this node index
            var refTableNode = allFiles.FirstOrDefault(x => x.NodeIndex == index);

            if (refTableNode is not null)
            {
                return true;
            }
        }

        return false;
    }
}
