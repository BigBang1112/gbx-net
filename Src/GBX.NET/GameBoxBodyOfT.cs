using System.Diagnostics;

namespace GBX.NET;

public class GameBoxBody<T> : GameBoxBody where T : Node
{
    public byte[]? Rest { get; set; }

    public new GameBox<T> GBX => (GameBox<T>)base.GBX;

    /// <summary>
    /// Body with uncompressed data with compression parameters.
    /// </summary>
    /// <param name="gbx">Owner of the GBX body.</param>
    public GameBoxBody(GameBox<T> gbx) : base(gbx)
    {

    }

    private void DecompressData(byte[] input, byte[] output)
    {
        Lzo.Decompress(input, output);

        if (GameBox.Debug)
        {
            Debugger ??= new();
            Debugger.CompressedData = input;
            Debugger.UncompressedData = output;
        }
    }

    /// <exception cref="MissingLzoException"></exception>
    /// <exception cref="NodeNotImplementedException">Auxiliary node is not implemented and is not parseable.</exception>
    /// <exception cref="ChunkReadNotImplementedException">Chunk does not support reading.</exception>
    /// <exception cref="IgnoredUnskippableChunkException">Chunk is known but its content is unknown to read.</exception>
    internal void Read(byte[] data, int uncompressedSize, Guid? contextGuid, IProgress<GameBoxReadProgress>? progress, ILogger? logger)
    {
        var buffer = new byte[uncompressedSize];

        DecompressData(data, buffer);

        Read(buffer, contextGuid, progress, logger);
    }

    /// <exception cref="NodeNotImplementedException">Auxiliary node is not implemented and is not parseable.</exception>
    /// <exception cref="ChunkReadNotImplementedException">Chunk does not support reading.</exception>
    /// <exception cref="IgnoredUnskippableChunkException">Chunk is known but its content is unknown to read.</exception>
    internal void Read(byte[] data, Guid? contextGuid, IProgress<GameBoxReadProgress>? progress, ILogger? logger)
    {
        using var ms = new MemoryStream(data);
        Read(ms, contextGuid, progress, logger);
    }

    /// <exception cref="NodeNotImplementedException">Auxiliary node is not implemented and is not parseable.</exception>
    /// <exception cref="ChunkReadNotImplementedException">Chunk does not support reading.</exception>
    /// <exception cref="IgnoredUnskippableChunkException">Chunk is known but its content is unknown to read.</exception>
    internal void Read(Stream stream, Guid? contextGuid, IProgress<GameBoxReadProgress>? progress, ILogger? logger)
    {
        using var gbxr = new GameBoxReader(stream, contextGuid, logger: logger);
        Read(gbxr, progress, logger);
    }

    /// <exception cref="NodeNotImplementedException">Auxiliary node is not implemented and is not parseable.</exception>
    /// <exception cref="ChunkReadNotImplementedException">Chunk does not support reading.</exception>
    /// <exception cref="IgnoredUnskippableChunkException">Chunk is known but its content is unknown to read.</exception>
    internal void Read(GameBoxReader reader, IProgress<GameBoxReadProgress>? progress, ILogger? logger)
    {
        var node = GBX.Node;

        Node.Parse(node, node.GetType(), reader, progress, logger);

        IsParsed = true;

        using var ms = new MemoryStream();
        var s = reader.BaseStream;
        s.CopyTo(ms);
        Rest = ms.ToArray();
        Debug.WriteLine("Amount read: " + (s.Position / (float)(s.Position + Rest.Length)).ToString("P"));
    }

    /// <exception cref="MissingLzoException"></exception>
    /// <exception cref="NodeNotImplementedException">Auxiliary node is not implemented and is not parseable.</exception>
    /// <exception cref="ChunkReadNotImplementedException">Chunk does not support reading.</exception>
    /// <exception cref="IgnoredUnskippableChunkException">Chunk is known but its content is unknown to read.</exception>
    internal async Task ReadAsync(byte[] data,
                                  int uncompressedSize,
                                  Guid stateGuid,
                                  ILogger? logger,
                                  GameBoxAsyncReadAction? asyncAction,
                                  CancellationToken cancellationToken)
    {
        var buffer = new byte[uncompressedSize];

        if (asyncAction is not null && asyncAction.BeforeLzoDecompression is not null)
        {
            await asyncAction.BeforeLzoDecompression();
        }

        DecompressData(data, buffer);

        if (asyncAction is not null && asyncAction.AfterLzoDecompression is not null)
        {
            await asyncAction.AfterLzoDecompression();
        }

        await ReadAsync(buffer, stateGuid, logger, asyncAction, cancellationToken);
    }

    internal async Task ReadAsync(byte[] data,
                                  Guid stateGuid,
                                  ILogger? logger,
                                  GameBoxAsyncReadAction? asyncAction,
                                  CancellationToken cancellationToken)
    {
        using var ms = new MemoryStream(data);
        await ReadAsync(ms, stateGuid, logger, asyncAction, cancellationToken);
    }

    /// <exception cref="NodeNotImplementedException">Auxiliary node is not implemented and is not parseable.</exception>
    /// <exception cref="ChunkReadNotImplementedException">Chunk does not support reading.</exception>
    /// <exception cref="IgnoredUnskippableChunkException">Chunk is known but its content is unknown to read.</exception>
    internal async Task ReadAsync(Stream stream,
                                  Guid stateGuid,
                                  ILogger? logger,
                                  GameBoxAsyncReadAction? asyncAction,
                                  CancellationToken cancellationToken)
    {
        using var gbxr = new GameBoxReader(stream, stateGuid, logger: logger, asyncAction: asyncAction);
        await ReadAsync(gbxr, logger, cancellationToken);
    }

    /// <exception cref="NodeNotImplementedException">Auxiliary node is not implemented and is not parseable.</exception>
    /// <exception cref="ChunkReadNotImplementedException">Chunk does not support reading.</exception>
    /// <exception cref="IgnoredUnskippableChunkException">Chunk is known but its content is unknown to read.</exception>
    internal async Task ReadAsync(GameBoxReader reader,
                                  ILogger? logger,
                                  CancellationToken cancellationToken)
    {
        var node = GBX.Node;

        await Node.ParseAsync(node, node.GetType(), reader, logger, cancellationToken);

        IsParsed = true;

        // Maybe not needed
        using var ms = new MemoryStream();
        var s = reader.BaseStream;

#if NET6_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
        await s.CopyToAsync(ms, cancellationToken);
#else
        await s.CopyToAsync(ms);
#endif

        Rest = ms.ToArray();
        Debug.WriteLine("Amount read: " + (s.Position / (float)(s.Position + Rest.Length)).ToString("P"));
    }

    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="MissingLzoException"></exception>
    internal void Write(GameBoxWriter w, ILogger? logger)
    {
        if (GBX.Header.CompressionOfBody == GameBoxCompression.Uncompressed)
        {
            GBX.Node.Write(w, logger);
            return;
        }

        using var msBody = new MemoryStream();
        using var gbxwBody = new GameBoxWriter(msBody, w.Settings, logger);

        GBX.Node.Write(gbxwBody, logger);

        var buffer = msBody.ToArray();

        var output = Lzo.Compress(buffer);

        w.Write((int)msBody.Length); // Uncompressed
        w.Write(output.Length); // Compressed
        w.WriteBytes(output); // Compressed body data
    }

    public TChunk CreateChunk<TChunk>(byte[] data) where TChunk : Chunk<T>
    {
        return GBX.Node.Chunks.Create<TChunk>(data);
    }

    public TChunk CreateChunk<TChunk>() where TChunk : Chunk<T>
    {
        return CreateChunk<TChunk>(Array.Empty<byte>());
    }

    public void InsertChunk(Chunk<T> chunk)
    {
        GBX.Node.Chunks.Add(chunk);
    }

    public void DiscoverChunk<TChunk>() where TChunk : SkippableChunk<T>
    {
        foreach (var chunk in GBX.Node.Chunks)
            if (chunk is TChunk c)
                c.Discover();
    }

    public void DiscoverChunks<TChunk1, TChunk2>() where TChunk1 : SkippableChunk<T> where TChunk2 : SkippableChunk<T>
    {
        foreach (var chunk in GBX.Node.Chunks)
        {
            switch (chunk)
            {
                case TChunk1 c: c.Discover(); break;
                case TChunk2 c: c.Discover(); break;
            }
        }
    }

    public void DiscoverChunks<TChunk1, TChunk2, TChunk3>()
        where TChunk1 : SkippableChunk<T>
        where TChunk2 : SkippableChunk<T>
        where TChunk3 : SkippableChunk<T>
    {
        foreach (var chunk in GBX.Node.Chunks)
        {
            switch (chunk)
            {
                case TChunk1 c: c.Discover(); break;
                case TChunk2 c: c.Discover(); break;
                case TChunk3 c: c.Discover(); break;
            }
        }
    }

    public void DiscoverChunks<TChunk1, TChunk2, TChunk3, TChunk4>()
        where TChunk1 : SkippableChunk<T>
        where TChunk2 : SkippableChunk<T>
        where TChunk3 : SkippableChunk<T>
        where TChunk4 : SkippableChunk<T>
    {
        foreach (var chunk in GBX.Node.Chunks)
        {
            switch (chunk)
            {
                case TChunk1 c: c.Discover(); break;
                case TChunk2 c: c.Discover(); break;
                case TChunk3 c: c.Discover(); break;
                case TChunk4 c: c.Discover(); break;
            }
        }
    }

    public void DiscoverChunks<TChunk1, TChunk2, TChunk3, TChunk4, TChunk5>()
        where TChunk1 : SkippableChunk<T>
        where TChunk2 : SkippableChunk<T>
        where TChunk3 : SkippableChunk<T>
        where TChunk4 : SkippableChunk<T>
        where TChunk5 : SkippableChunk<T>
    {
        foreach (var chunk in GBX.Node.Chunks)
        {
            switch (chunk)
            {
                case TChunk1 c: c.Discover(); break;
                case TChunk2 c: c.Discover(); break;
                case TChunk3 c: c.Discover(); break;
                case TChunk4 c: c.Discover(); break;
                case TChunk5 c: c.Discover(); break;
            }
        }
    }

    public void DiscoverChunks<TChunk1, TChunk2, TChunk3, TChunk4, TChunk5, TChunk6>()
        where TChunk1 : SkippableChunk<T>
        where TChunk2 : SkippableChunk<T>
        where TChunk3 : SkippableChunk<T>
        where TChunk4 : SkippableChunk<T>
        where TChunk5 : SkippableChunk<T>
        where TChunk6 : SkippableChunk<T>
    {
        foreach (var chunk in GBX.Node.Chunks)
        {
            switch (chunk)
            {
                case TChunk1 c: c.Discover(); break;
                case TChunk2 c: c.Discover(); break;
                case TChunk3 c: c.Discover(); break;
                case TChunk4 c: c.Discover(); break;
                case TChunk5 c: c.Discover(); break;
                case TChunk6 c: c.Discover(); break;
            }
        }
    }

    public void DiscoverAllChunks()
    {
        foreach (var chunk in GBX.Node.Chunks)
            if (chunk is SkippableChunk<T> s)
                s.Discover();
    }

    public TChunk? GetChunk<TChunk>() where TChunk : Chunk<T>
    {
        foreach (var chunk in GBX.Node.Chunks)
        {
            if (chunk is not TChunk t)
                continue;

            if (chunk is SkippableChunk<T> s)
                s.Discover();

            return t;
        }

        return default;
    }

    public bool TryGetChunk<TChunk>(out TChunk? chunk) where TChunk : Chunk<T>
    {
        chunk = GetChunk<TChunk>();
        return chunk != default;
    }

    public void RemoveAllChunks()
    {
        GBX.Node.Chunks.Clear();
    }

    public bool RemoveChunk<TChunk>() where TChunk : Chunk<T>
    {
        return GBX.Node.Chunks.Remove<TChunk>();
    }
}