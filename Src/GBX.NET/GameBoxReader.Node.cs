namespace GBX.NET;

public partial class GameBoxReader
{
    /// <summary>
    /// Reads an <see cref="int"/> containing the node reference index, then the node using the <see cref="Node"/>'s Parse method. The index is also checked if it isn't a part of a reference table, which currently returns null.
    /// </summary>
    /// <param name="nodeRefIndex">Index of the node reference.</param>
    /// <returns>A node, or null if the index is -1 or the node is from reference table.</returns>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="PropertyNullException"><see cref="GameBoxReaderSettings.Gbx"/> is null.</exception>
    public Node? ReadNodeRef(out int nodeRefIndex)
    {
        var index = ReadInt32() - 1; // GBX seems to start the index at 1

        nodeRefIndex = index;

        // If aux node index is below 0 or the node index is part of the reference table
        if (index < 0)
        {
            return null;
        }

        var gbx = Settings.GetGbxOrThrow();

        if (IsRefTableNode(gbx, index))
        {
            return null;
        }

        var node = default(Node?);

        if (NodeShouldBeParsed(gbx, index))
        {
            node = Node.Parse(this, classId: null, progress: null, Logger)!;
        }

        TryGetNodeIfNullOrAssignExistingNode(gbx, index, ref node);

        if (node is null)
        {
            // Sometimes it indexes the node reference that is further in the expected indexes
            // So it grabs the last one instead, needs to be further tested
            return gbx.AuxNodes.Values.Last(); 
        }

        return node;
    }

    /// <summary>
    /// Reads an <see cref="int"/> containing the node reference index, then the node using the <see cref="Node"/>'s Parse method. The index is also checked if it isn't a part of a reference table, which currently returns null.
    /// </summary>
    /// <returns>A node, or null if the index is -1 or the node is from reference table.</returns>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="PropertyNullException"><see cref="GameBoxReaderSettings.Gbx"/> is null.</exception>
    public Node? ReadNodeRef() => ReadNodeRef(out int _);

    /// <summary>
    /// Reads an <see cref="int"/> containing the node reference index, then the node using the <see cref="Node"/>'s Parse method. The index is also checked if it isn't a part of a reference table, which currently returns null.
    /// </summary>
    /// <typeparam name="T">Type of node.</typeparam>
    /// <param name="nodeRefIndex">Index of the node reference.</param>
    /// <returns>A node, or null if the index is -1 or the node is from reference table.</returns>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="PropertyNullException"><see cref="GameBoxReaderSettings.Gbx"/> is null.</exception>
    public T? ReadNodeRef<T>(out int nodeRefIndex) where T : Node
    {
        return ReadNodeRef(out nodeRefIndex) as T;
    }

    /// <summary>
    /// Reads an <see cref="int"/> containing the node reference index, then the node using the <see cref="Node"/>'s Parse method. The index is also checked if it isn't a part of a reference table, which currently returns null.
    /// </summary>
    /// <typeparam name="T">Type of node.</typeparam>
    /// <returns>A node, or null if the index is -1 or the node is from reference table.</returns>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="PropertyNullException"><see cref="GameBoxReaderSettings.Gbx"/> is null.</exception>
    public T? ReadNodeRef<T>() where T : Node
    {
        return ReadNodeRef() as T;
    }

    /// <summary>
    /// Reads an <see cref="int"/> containing the node reference index, then the node using the <see cref="Node"/>'s ParseAsync method. The index is also checked if it isn't a part of a reference table, which currently returns null.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A node, or null if the index is -1 or the node is from reference table.</returns>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="PropertyNullException"><see cref="GameBoxReaderSettings.Gbx"/> is null.</exception>
    public async Task<Node?> ReadNodeRefAsync(CancellationToken cancellationToken = default)
    {
        var gbx = Settings.GetGbxOrThrow();

        var index = ReadInt32() - 1; // GBX seems to start the index at 1

        // If aux node index is below 0 or the node index is part of the reference table
        if (index < 0 || IsRefTableNode(gbx, index))
        {
            return null;
        }

        var node = default(Node?);

        if (NodeShouldBeParsed(gbx, index))
        {
            node = await Node.ParseAsync(this, classId: null, Logger, cancellationToken);
        }

        TryGetNodeIfNullOrAssignExistingNode(gbx, index, ref node);

        if (node is null)
        {
            // Sometimes it indexes the node reference that is further in the expected indexes
            // So it grabs the last one instead, needs to be further tested
            return gbx.AuxNodes.Values.Last();
        }

        return node;
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
    /// <exception cref="PropertyNullException"><see cref="GameBoxReaderSettings.Gbx"/> is null.</exception>
    public async Task<T?> ReadNodeRefAsync<T>(CancellationToken cancellationToken = default) where T : Node
    {
        return await ReadNodeRefAsync(cancellationToken) as T;
    }

    private static bool NodeShouldBeParsed(GameBox gbx, int index)
    {
        var containsNodeIndex = gbx.AuxNodes.ContainsKey(index);
        _ = gbx.AuxNodes.TryGetValue(index, out Node? node);

        // If index is 0 or bigger and the node wasn't read yet, or is null
        return index >= 0 && (!containsNodeIndex || node is null);
    }

    private static void TryGetNodeIfNullOrAssignExistingNode(GameBox gbx, int index, ref Node? node)
    {
        if (node is null)
        {
            gbx.AuxNodes.TryGetValue(index, out node); // Tries to get the available node from index
        }
        else
        {
            gbx.AuxNodes[index] = node;
        }
    }

    private static bool IsRefTableNode(GameBox gbx, int index)
    {
        var refTable = gbx.GetRefTable();

        // First checks if reference table is used
        if (refTable is null || refTable.Files.Count <= 0 && refTable.Folders.Count <= 0)
        {
            // Reference table isn't used so it's a nested object
            return false;
        }

        var allFiles = refTable.Files; // Returns available external references

        if (!allFiles.Any()) // If there's none
        {
            return false;
        }

        // Tries to get the one with this node index
        var refTableNode = allFiles.FirstOrDefault(x => x.NodeIndex == index);

        if (refTableNode is not null)
        {
            return true;
        }

        return false;
    }
}
