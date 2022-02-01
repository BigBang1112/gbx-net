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
    /// <exception cref="PropertyNullException"><see cref="GameBoxReaderSettings.StateGuid"/> is null.</exception>
    public Node? ReadNodeRef(out int nodeRefIndex)
    {
        var index = ReadInt32() - 1; // GBX seems to start the index at 1

        nodeRefIndex = index;

        // If aux node index is below 0 or the node index is part of the reference table
        if (index < 0)
        {
            return null;
        }

        if (Settings.StateGuid is null)
        {
            throw new PropertyNullException(nameof(Settings.StateGuid));
        }

        var stateGuid = Settings.StateGuid.Value;

        if (IsRefTableNode(stateGuid, index))
        {
            return null;
        }

        var node = default(Node?);

        if (NodeShouldBeParsed(stateGuid, index))
        {
            node = Node.Parse(this, classId: null, progress: null, logger)!;
        }

        TryGetNodeIfNullOrAssignExistingNode(stateGuid, index, ref node);

        if (node is null)
        {
            // Sometimes it indexes the node reference that is further in the expected indexes
            // So it grabs the last one instead, needs to be further tested
            return StateManager.Shared.GetLastNode(stateGuid); 
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
    /// <exception cref="PropertyNullException"><see cref="GameBoxReaderSettings.StateGuid"/> is null.</exception>
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
    /// <exception cref="PropertyNullException"><see cref="GameBoxReaderSettings.StateGuid"/> is null.</exception>
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
    /// <exception cref="PropertyNullException"><see cref="GameBoxReaderSettings.StateGuid"/> is null.</exception>
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
    /// <exception cref="PropertyNullException"><see cref="GameBoxReaderSettings.StateGuid"/> is null.</exception>
    public async Task<Node?> ReadNodeRefAsync(CancellationToken cancellationToken = default)
    {
        if (Settings.StateGuid is null)
        {
            throw new PropertyNullException(nameof(Settings.StateGuid));
        }

        var stateGuid = Settings.StateGuid.Value;

        var index = ReadInt32() - 1; // GBX seems to start the index at 1

        // If aux node index is below 0 or the node index is part of the reference table
        if (index < 0 || IsRefTableNode(stateGuid, index))
        {
            return null;
        }

        var node = default(Node?);

        if (NodeShouldBeParsed(stateGuid, index))
        {
            node = await Node.ParseAsync(this, classId: null, logger, cancellationToken);
        }

        TryGetNodeIfNullOrAssignExistingNode(stateGuid, index, ref node);

        if (node is null)
        {
            // Sometimes it indexes the node reference that is further in the expected indexes
            // So it grabs the last one instead, needs to be further tested
            return StateManager.Shared.GetLastNode(stateGuid);
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
    /// <exception cref="PropertyNullException"><see cref="GameBoxReaderSettings.StateGuid"/> is null.</exception>
    public async Task<T?> ReadNodeRefAsync<T>(CancellationToken cancellationToken = default) where T : Node
    {
        return await ReadNodeRefAsync(cancellationToken) as T;
    }

    private static bool NodeShouldBeParsed(Guid stateGuid, int index)
    {
        var containsNodeIndex = StateManager.Shared.ContainsNodeIndex(stateGuid, index);
        var nodeAtIndexIsNull = StateManager.Shared.GetNode(stateGuid, index) is null;

        // If index is 0 or bigger and the node wasn't read yet, or is null
        return index >= 0 && (!containsNodeIndex || nodeAtIndexIsNull);
    }

    private static void TryGetNodeIfNullOrAssignExistingNode(Guid stateGuid, int index, ref Node? node)
    {
        if (node is null)
        {
            StateManager.Shared.TryGetNode(stateGuid, index, out node); // Tries to get the available node from index
        }
        else
        {
            StateManager.Shared.SetNode(stateGuid, index, node);
        }
    }

    private static bool IsRefTableNode(Guid stateGuid, int index)
    {
        var refTable = StateManager.Shared.GetReferenceTable(stateGuid);

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
