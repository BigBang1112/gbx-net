namespace GBX.NET;

public partial class GameBoxReaderWriter
{
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="body"/> is null.</exception>
    public Node? NodeRef(Node? variable, GameBoxBody body)
    {
        if (Reader is not null) return Reader.ReadNodeRef(body);
        if (Writer is not null) Writer.Write(variable, body);
        return variable;
    }

    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="body"/> is null.</exception>
    public void NodeRef(ref Node? variable, GameBoxBody body)
    {
        variable = NodeRef(variable, body);
    }

    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="PropertyNullException">Body of <see cref="Reader"/> or <see cref="Writer"/> is null.</exception>
    public Node? NodeRef(Node? variable = default)
    {
        if (Reader is not null)
        {
            if (Reader.Body is null)
            {
                throw new PropertyNullException(nameof(Reader.Body));
            }

            return NodeRef(variable, Reader.Body);
        }

        if (Writer is not null)
        {
            if (Writer.Body is null)
            {
                throw new PropertyNullException(nameof(Writer.Body));
            }

            return NodeRef(variable, Writer.Body);
        }

        throw new ThisShouldNotHappenException();
    }

    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="PropertyNullException">Body of <see cref="Reader"/> or <see cref="Writer"/> is null.</exception>
    public void NodeRef(ref Node? variable) => variable = NodeRef(variable);

    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="body"/> is null.</exception>
    public Node? NodeRef(Node? variable, ref int? nodeRefIndex, GameBoxBody body)
    {
        if (Reader is not null)
        {
            var node = Reader.ReadNodeRef(out int index, body);
            nodeRefIndex = index < 0 ? null : index;
            return node;
        }

        if (Writer is not null)
        {
            Writer.Write(variable, body);
        }

        return variable;
    }

    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="body"/> is null.</exception>
    public void NodeRef(ref Node? variable, ref int? nodeRefIndex, GameBoxBody body)
    {
        variable = NodeRef(variable, ref nodeRefIndex, body);
    }

    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="PropertyNullException">Body of <see cref="Reader"/> or <see cref="Writer"/> is null.</exception>
    public Node? NodeRef(Node? variable, ref int? nodeRefIndex)
    {
        if (Reader is not null)
        {
            if (Reader.Body is null)
            {
                throw new PropertyNullException(nameof(Reader.Body));
            }

            return NodeRef(variable, ref nodeRefIndex, Reader.Body);
        }

        if (Writer is not null)
        {
            if (Writer.Body is null)
            {
                throw new PropertyNullException(nameof(Writer.Body));
            }

            return NodeRef(variable, ref nodeRefIndex, Writer.Body);
        }

        throw new ThisShouldNotHappenException();
    }

    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="PropertyNullException">Body of <see cref="Reader"/> or <see cref="Writer"/> is null.</exception>
    public void NodeRef(ref Node? variable, ref int? nodeRefIndex)
    {
        variable = NodeRef(variable, ref nodeRefIndex);
    }

    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="body"/> is null.</exception>
    public T? NodeRef<T>(T? variable, GameBoxBody body) where T : Node
    {
        if (Reader is not null) return Reader.ReadNodeRef<T>(body);
        if (Writer is not null) Writer.Write(variable, body);
        return variable;
    }

    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="body"/> is null.</exception>
    public void NodeRef<T>(ref T? variable, GameBoxBody body) where T : Node
    {
        variable = NodeRef(variable, body);
    }

    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="PropertyNullException">Body of <see cref="Reader"/> or <see cref="Writer"/> is null.</exception>
    public T? NodeRef<T>(T? variable = default) where T : Node
    {
        if (Reader is not null)
        {
            if (Reader.Body is null)
            {
                throw new PropertyNullException(nameof(Reader.Body));
            }

            return NodeRef(variable, Reader.Body);
        }

        if (Writer is not null)
        {
            if (Writer.Body is null)
            {
                throw new PropertyNullException(nameof(Writer.Body));
            }

            return NodeRef(variable, Writer.Body);
        }

        throw new ThisShouldNotHappenException();
    }

    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="PropertyNullException">Body of <see cref="Reader"/> or <see cref="Writer"/> is null.</exception>
    public void NodeRef<T>(ref T? variable) where T : Node
    {
        variable = NodeRef(variable);
    }

    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="body"/> is null.</exception>
    public T? NodeRef<T>(T? variable, ref int? nodeRefIndex, GameBoxBody body) where T : Node
    {
        if (Reader is not null)
        {
            var node = Reader.ReadNodeRef<T>(out int index, body);
            nodeRefIndex = index < 0 ? null : index;
            return node;
        }

        if (Writer is not null)
        {
            Writer.Write(variable, body);
        }

        return variable;
    }

    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="body"/> is null.</exception>
    public void NodeRef<T>(ref T? variable, ref int? nodeRefIndex, GameBoxBody body) where T : Node
    {
        variable = NodeRef(variable, ref nodeRefIndex, body);
    }

    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="PropertyNullException">Body of <see cref="Reader"/> or <see cref="Writer"/> is null.</exception>
    public T? NodeRef<T>(T? variable, ref int? nodeRefIndex) where T : Node
    {
        if (Reader is not null)
        {
            if (Reader.Body is null)
            {
                throw new PropertyNullException(nameof(Reader.Body));
            }

            return NodeRef(variable, ref nodeRefIndex, Reader.Body);
        }

        if (Writer is not null)
        {
            if (Writer.Body is null)
            {
                throw new PropertyNullException(nameof(Writer.Body));
            }

            return NodeRef(variable, ref nodeRefIndex, Writer.Body);
        }

        throw new ThisShouldNotHappenException();
    }

    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="PropertyNullException">Body of <see cref="Reader"/> or <see cref="Writer"/> is null.</exception>
    public void NodeRef<T>(ref T? variable, ref int? nodeRefIndex) where T : Node
    {
        variable = NodeRef(variable, ref nodeRefIndex);
    }

    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="body"/> is null.</exception>
    public async Task<Node?> NodeRefAsync(Node? variable, GameBoxBody body)
    {
        if (Reader is not null) return await Reader.ReadNodeRefAsync(body);
        if (Writer is not null) Writer.Write(variable, body);
        return variable;
    }

    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="PropertyNullException">Body of <see cref="Reader"/> or <see cref="Writer"/> is null.</exception>
    public async Task<Node?> NodeRefAsync(Node? variable = default)
    {
        if (Reader is not null)
        {
            if (Reader.Body is null)
            {
                throw new PropertyNullException(nameof(Reader.Body));
            }

            return await NodeRefAsync(variable, Reader.Body);
        }

        if (Writer is not null)
        {
            if (Writer.Body is null)
            {
                throw new PropertyNullException(nameof(Writer.Body));
            }

            return NodeRef(variable, Writer.Body);
        }

        throw new ThisShouldNotHappenException();
    }

    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="body"/> is null.</exception>
    public async Task<T?> NodeRefAsync<T>(T? variable, GameBoxBody body) where T : Node
    {
        if (Reader is not null) return await Reader.ReadNodeRefAsync<T>(body);
        if (Writer is not null) Writer.Write(variable, body);
        return variable;
    }

    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="PropertyNullException">Body of <see cref="Reader"/> or <see cref="Writer"/> is null.</exception>
    public async Task<T?> NodeRefAsync<T>(T? variable = default) where T : Node
    {
        if (Reader is not null)
        {
            if (Reader.Body is null)
            {
                throw new PropertyNullException(nameof(Reader.Body));
            }

            return await NodeRefAsync(variable, Reader.Body);
        }

        if (Writer is not null)
        {
            if (Writer.Body is null)
            {
                throw new PropertyNullException(nameof(Writer.Body));
            }

            return NodeRef(variable, Writer.Body);
        }

        throw new ThisShouldNotHappenException();
    }
}
