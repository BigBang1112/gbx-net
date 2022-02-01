namespace GBX.NET;

public partial class GameBoxReaderWriter
{
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="PropertyNullException"><see cref="GameBoxReaderSettings.StateGuid"/> or <see cref="GameBoxWriterSettings.StateGuid"/> is null.</exception>
    public Node? NodeRef(Node? variable = default)
    {
        if (Reader is not null) return Reader.ReadNodeRef();
        if (Writer is not null) Writer.Write(variable);
        return variable;
    }

    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="PropertyNullException"><see cref="GameBoxReaderSettings.StateGuid"/> or <see cref="GameBoxWriterSettings.StateGuid"/> is null.</exception>
    public void NodeRef(ref Node? variable) => variable = NodeRef(variable);

    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="PropertyNullException"><see cref="GameBoxReaderSettings.StateGuid"/> or <see cref="GameBoxWriterSettings.StateGuid"/> is null.</exception>
    public Node? NodeRef(Node? variable, ref int? nodeRefIndex)
    {
        if (Reader is not null)
        {
            var node = Reader.ReadNodeRef(out int index);
            nodeRefIndex = index < 0 ? null : index;
            return node;
        }

        if (Writer is not null)
        {
            Writer.Write(variable);
        }

        return variable;
    }

    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="PropertyNullException"><see cref="GameBoxReaderSettings.StateGuid"/> or <see cref="GameBoxWriterSettings.StateGuid"/> is null.</exception>
    public void NodeRef(ref Node? variable, ref int? nodeRefIndex)
    {
        variable = NodeRef(variable, ref nodeRefIndex);
    }

    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="PropertyNullException"><see cref="GameBoxReaderSettings.StateGuid"/> or <see cref="GameBoxWriterSettings.StateGuid"/> is null.</exception>
    public T? NodeRef<T>(T? variable = default) where T : Node
    {
        if (Reader is not null) return Reader.ReadNodeRef<T>();
        if (Writer is not null) Writer.Write(variable);
        return variable;
    }

    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="PropertyNullException"><see cref="GameBoxReaderSettings.StateGuid"/> or <see cref="GameBoxWriterSettings.StateGuid"/> is null.</exception>
    public void NodeRef<T>(ref T? variable) where T : Node => variable = NodeRef(variable);

    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="PropertyNullException"><see cref="GameBoxReaderSettings.StateGuid"/> or <see cref="GameBoxWriterSettings.StateGuid"/> is null.</exception>
    public T? NodeRef<T>(T? variable, ref int? nodeRefIndex) where T : Node
    {
        if (Reader is not null)
        {
            var node = Reader.ReadNodeRef<T>(out int index);
            nodeRefIndex = index < 0 ? null : index;
            return node;
        }

        if (Writer is not null)
        {
            Writer.Write(variable);
        }

        return variable;
    }

    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="PropertyNullException"><see cref="GameBoxReaderSettings.StateGuid"/> or <see cref="GameBoxWriterSettings.StateGuid"/> is null.</exception>
    public void NodeRef<T>(ref T? variable, ref int? nodeRefIndex) where T : Node
    {
        variable = NodeRef(variable, ref nodeRefIndex);
    }

    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="PropertyNullException"><see cref="GameBoxReaderSettings.StateGuid"/> or <see cref="GameBoxWriterSettings.StateGuid"/> is null.</exception>
    public async Task<Node?> NodeRefAsync(Node? variable = default, CancellationToken cancellationToken = default)
    {
        if (Reader is not null) return await Reader.ReadNodeRefAsync(cancellationToken);
        if (Writer is not null) Writer.Write(variable);
        return variable;
    }

    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="PropertyNullException"><see cref="GameBoxReaderSettings.StateGuid"/> or <see cref="GameBoxWriterSettings.StateGuid"/> is null.</exception>
    public async Task<T?> NodeRefAsync<T>(T? variable = default, CancellationToken cancellationToken = default) where T : Node
    {
        if (Reader is not null) return await Reader.ReadNodeRefAsync<T>(cancellationToken);
        if (Writer is not null) Writer.Write(variable);
        return variable;
    }
}
