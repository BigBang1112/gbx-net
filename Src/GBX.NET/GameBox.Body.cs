namespace GBX.NET;

public partial class GameBox
{
    public class Body
    {
        public int? UncompressedSize { get; init; }
        public byte[]? RawData { get; init; }

        public static Body ParseRaw(GameBoxCompression bodyCompression, GameBoxReader r)
        {
            byte[]? rawData;

            if (bodyCompression == GameBoxCompression.Uncompressed)
            {
                rawData = r.ReadToEnd();

                return new Body { RawData = rawData };
            }

            var uncompressedSize = r.ReadInt32();
            rawData = r.ReadBytes();

            return new Body
            {
                UncompressedSize = uncompressedSize,
                RawData = rawData
            };
        }
    }

    internal bool BodyIsParsed { get; set; }
    internal int UncompressedBodySize { get; set; }

    /// <summary>
    /// Pure body data usually in compressed form. This property is used if GameBox's ParseHeader methods are used, otherwise null.
    /// </summary>
    internal byte[]? BodyRawData { get; set; }

    public GameBoxBodyDebugger? BodyDebugger { get; protected set; }

    private void ReadRawBody(GameBoxReader reader)
    {
        if (Header.CompressionOfBody == GameBoxCompression.Uncompressed)
        {
            BodyRawData = reader.ReadToEnd();
            return;
        }

        UncompressedBodySize = reader.ReadInt32();
        BodyRawData = reader.ReadBytes();
    }

    /// <exception cref="MissingLzoException"></exception>
    /// <exception cref="NodeNotImplementedException">Auxiliary node is not implemented and is not parseable.</exception>
    /// <exception cref="ChunkReadNotImplementedException">Chunk does not support reading.</exception>
    /// <exception cref="IgnoredUnskippableChunkException">Chunk is known but its content is unknown to read.</exception>
    internal bool ReadBody(GameBoxReader reader, IProgress<GameBoxReadProgress>? progress, bool readUncompressedBodyDirectly, ILogger? logger)
    {
        var stateGuid = reader.Settings.StateGuid;

        if (stateGuid is not null)
        {
            StateManager.Shared.ResetIdState(stateGuid.Value);
        }

        logger?.LogDebug("Reading the body...");

        switch (header.CompressionOfBody)
        {
            case GameBoxCompression.Compressed:
                ReadCompressedBody(reader, progress, logger);
                break;
            case GameBoxCompression.Uncompressed:
                ReadUncompressedBody(reader, progress, readUncompressedBodyDirectly, logger);
                break;
            default:
                logger?.LogError("Body can't be read! Compression type is unknown.");
                return false;
        }

        logger?.LogDebug("Body chunks parsed without major exceptions.");

        return true;
    }

    private void ReadCompressedBody(GameBoxReader reader, IProgress<GameBoxReadProgress>? progress, ILogger? logger)
    {
        var uncompressedSize = reader.ReadInt32();
        var compressedSize = reader.ReadInt32();

        var data = reader.ReadBytes(compressedSize);
        ReadBody(data, uncompressedSize, reader.Settings.StateGuid, progress, logger);
    }

    private void ReadUncompressedBody(GameBoxReader reader, IProgress<GameBoxReadProgress>? progress, bool readUncompressedBodyDirectly, ILogger? logger)
    {
        if (readUncompressedBodyDirectly)
        {
            ReadBody(reader, progress, logger);
            return;
        }

        var uncompressedData = reader.ReadToEnd();
        ReadBody(uncompressedData, reader.Settings.StateGuid, progress, logger);
    }

    /// <exception cref="MissingLzoException"></exception>
    /// <exception cref="NodeNotImplementedException">Auxiliary node is not implemented and is not parseable.</exception>
    /// <exception cref="ChunkReadNotImplementedException">Chunk does not support reading.</exception>
    /// <exception cref="IgnoredUnskippableChunkException">Chunk is known but its content is unknown to read.</exception>
    protected internal async Task<bool> ReadBodyAsync(GameBoxReader reader,
                                                               bool readUncompressedBodyDirectly,
                                                               ILogger? logger,
                                                               GameBoxAsyncReadAction? asyncAction,
                                                               CancellationToken cancellationToken)
    {
        var stateGuid = reader.Settings.StateGuid;

        if (stateGuid is not null)
        {
            StateManager.Shared.ResetIdState(stateGuid.Value);
        }

        logger?.LogDebug("Reading the body...");

        switch (Header.CompressionOfBody)
        {
            case GameBoxCompression.Compressed:
                await ReadCompressedBodyAsync(reader, logger, asyncAction, cancellationToken);
                break;
            case GameBoxCompression.Uncompressed:
                await ReadUncompressedBodyAsync(reader, readUncompressedBodyDirectly, logger, asyncAction, cancellationToken);
                break;
            default:
                logger?.LogError("Body can't be read! Compression type is unknown.");
                return false;
        }

        logger?.LogDebug("Body chunks parsed without major exceptions.");

        return true;
    }

    private async Task ReadCompressedBodyAsync(GameBoxReader reader,
                                               ILogger? logger,
                                               GameBoxAsyncReadAction? asyncAction,
                                               CancellationToken cancellationToken)
    {
        var uncompressedSize = reader.ReadInt32();
        var compressedSize = reader.ReadInt32();

        var data = reader.ReadBytes(compressedSize);
        await ReadBodyAsync(data,
                              uncompressedSize,
                              reader.Settings.StateGuid.GetValueOrDefault(),
                              logger,
                              asyncAction,
                              cancellationToken);
    }

    private async Task ReadUncompressedBodyAsync(GameBoxReader reader,
                                                 bool readUncompressedBodyDirectly,
                                                 ILogger? logger,
                                                 GameBoxAsyncReadAction? asyncAction,
                                                 CancellationToken cancellationToken)
    {
        if (readUncompressedBodyDirectly)
        {
            await ReadBodyAsync(reader, logger, cancellationToken);
            return;
        }

        var uncompressedData = reader.ReadToEnd();
        await ReadBodyAsync(uncompressedData,
                              reader.Settings.StateGuid.GetValueOrDefault(),
                              logger,
                              asyncAction,
                              cancellationToken);
    }

    private void WriteBody(GameBoxWriter bodyW, ILogger? logger)
    {
        if (BodyRawData is null)
        {
            if (!BodyIsParsed)
            {
                throw new HeaderOnlyParseLimitationException();
            }

            WriteParsedBody(bodyW, logger); // Body is written first so that the aux node count is determined properly

            return;
        }

        if (Header.CompressionOfBody == GameBoxCompression.Compressed)
        {
            bodyW.Write(UncompressedBodySize);
            bodyW.Write(BodyRawData.Length);
        }

        bodyW.WriteBytes(BodyRawData);
    }

    private async Task WriteBodyAsync(GameBoxWriter bodyW, ILogger? logger, CancellationToken cancellationToken)
    {
        if (BodyRawData is null)
        {
            if (!BodyIsParsed)
            {
                throw new HeaderOnlyParseLimitationException();
            }

            await WriteParsedBodyAsync(bodyW, logger, cancellationToken); // Body is written first so that the aux node count is determined properly

            return;
        }

        if (Header.CompressionOfBody == GameBoxCompression.Compressed)
        {
            bodyW.Write(UncompressedBodySize);
            bodyW.Write(BodyRawData.Length);
        }

        await bodyW.WriteBytesAsync(BodyRawData, cancellationToken);
    }

    private void DecompressData(byte[] input, byte[] output)
    {
        Lzo.Decompress(input, output);

        /*if (Debug)
        {
            BodyDebugger ??= new();
            BodyDebugger.CompressedData = input;
            BodyDebugger.UncompressedData = output;
        }*/
    }

    /// <exception cref="MissingLzoException"></exception>
    /// <exception cref="NodeNotImplementedException">Auxiliary node is not implemented and is not parseable.</exception>
    /// <exception cref="ChunkReadNotImplementedException">Chunk does not support reading.</exception>
    /// <exception cref="IgnoredUnskippableChunkException">Chunk is known but its content is unknown to read.</exception>
    internal void ReadBody(byte[] data, int uncompressedSize, Guid? contextGuid, IProgress<GameBoxReadProgress>? progress, ILogger? logger)
    {
        var buffer = new byte[uncompressedSize];

        DecompressData(data, buffer);

        ReadBody(buffer, contextGuid, progress, logger);
    }

    /// <exception cref="NodeNotImplementedException">Auxiliary node is not implemented and is not parseable.</exception>
    /// <exception cref="ChunkReadNotImplementedException">Chunk does not support reading.</exception>
    /// <exception cref="IgnoredUnskippableChunkException">Chunk is known but its content is unknown to read.</exception>
    internal void ReadBody(byte[] data, Guid? contextGuid, IProgress<GameBoxReadProgress>? progress, ILogger? logger)
    {
        using var ms = new MemoryStream(data);
        ReadBody(ms, contextGuid, progress, logger);
    }

    /// <exception cref="NodeNotImplementedException">Auxiliary node is not implemented and is not parseable.</exception>
    /// <exception cref="ChunkReadNotImplementedException">Chunk does not support reading.</exception>
    /// <exception cref="IgnoredUnskippableChunkException">Chunk is known but its content is unknown to read.</exception>
    internal void ReadBody(Stream stream, Guid? contextGuid, IProgress<GameBoxReadProgress>? progress, ILogger? logger)
    {
        using var gbxr = new GameBoxReader(stream, contextGuid, logger: logger);
        ReadBody(gbxr, progress, logger);
    }

    /// <exception cref="NodeNotImplementedException">Auxiliary node is not implemented and is not parseable.</exception>
    /// <exception cref="ChunkReadNotImplementedException">Chunk does not support reading.</exception>
    /// <exception cref="IgnoredUnskippableChunkException">Chunk is known but its content is unknown to read.</exception>
    internal void ReadBody(GameBoxReader reader, IProgress<GameBoxReadProgress>? progress, ILogger? logger)
    {
        Node.Parse(Node, Node.GetType(), reader, progress, logger);

        BodyIsParsed = true;

        /*using var ms = new MemoryStream();
        var s = reader.BaseStream;
        s.CopyTo(ms);
        Rest = ms.ToArray();
        Debug.WriteLine("Amount read: " + (s.Position / (float)(s.Position + Rest.Length)).ToString("P"));*/
    }

    /// <exception cref="MissingLzoException"></exception>
    /// <exception cref="NodeNotImplementedException">Auxiliary node is not implemented and is not parseable.</exception>
    /// <exception cref="ChunkReadNotImplementedException">Chunk does not support reading.</exception>
    /// <exception cref="IgnoredUnskippableChunkException">Chunk is known but its content is unknown to read.</exception>
    internal async Task ReadBodyAsync(byte[] data,
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

        await ReadBodyAsync(buffer, stateGuid, logger, asyncAction, cancellationToken);
    }

    internal async Task ReadBodyAsync(byte[] data,
                                  Guid stateGuid,
                                  ILogger? logger,
                                  GameBoxAsyncReadAction? asyncAction,
                                  CancellationToken cancellationToken)
    {
        using var ms = new MemoryStream(data);
        await ReadBodyAsync(ms, stateGuid, logger, asyncAction, cancellationToken);
    }

    /// <exception cref="NodeNotImplementedException">Auxiliary node is not implemented and is not parseable.</exception>
    /// <exception cref="ChunkReadNotImplementedException">Chunk does not support reading.</exception>
    /// <exception cref="IgnoredUnskippableChunkException">Chunk is known but its content is unknown to read.</exception>
    internal async Task ReadBodyAsync(Stream stream,
                                  Guid stateGuid,
                                  ILogger? logger,
                                  GameBoxAsyncReadAction? asyncAction,
                                  CancellationToken cancellationToken)
    {
        using var gbxr = new GameBoxReader(stream, stateGuid, logger: logger, asyncAction: asyncAction);
        await ReadBodyAsync(gbxr, logger, cancellationToken);
    }

    /// <exception cref="NodeNotImplementedException">Auxiliary node is not implemented and is not parseable.</exception>
    /// <exception cref="ChunkReadNotImplementedException">Chunk does not support reading.</exception>
    /// <exception cref="IgnoredUnskippableChunkException">Chunk is known but its content is unknown to read.</exception>
    internal async Task ReadBodyAsync(GameBoxReader reader,
                                  ILogger? logger,
                                  CancellationToken cancellationToken)
    {
        await Node.ParseAsync(Node, Node.GetType(), reader, logger, cancellationToken);

        BodyIsParsed = true;

        // Maybe not needed
        /*using var ms = new MemoryStream();
        var s = reader.BaseStream;

#if NET6_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
        await s.CopyToAsync(ms, cancellationToken);
#else
        await s.CopyToAsync(ms);
#endif

        Rest = ms.ToArray();
        Debug.WriteLine("Amount read: " + (s.Position / (float)(s.Position + Rest.Length)).ToString("P"));*/
    }

    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="MissingLzoException"></exception>
    internal void WriteParsedBody(GameBoxWriter w, ILogger? logger)
    {
        if (Header.CompressionOfBody == GameBoxCompression.Uncompressed)
        {
            Node.Write(w, logger);
            return;
        }

        using var msBody = new MemoryStream();
        using var gbxwBody = new GameBoxWriter(msBody, w.Settings, logger);

        Node.Write(gbxwBody, logger);

        var buffer = msBody.ToArray();

        var output = Lzo.Compress(buffer);

        w.Write((int)msBody.Length); // Uncompressed
        w.Write(output.Length); // Compressed
        w.WriteBytes(output); // Compressed body data
    }

    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="MissingLzoException"></exception>
    internal async Task WriteParsedBodyAsync(GameBoxWriter w, ILogger? logger, CancellationToken cancellationToken)
    {
        if (Header.CompressionOfBody == GameBoxCompression.Uncompressed)
        {
            Node.Write(w, logger);
            return;
        }

        using var msBody = new MemoryStream();
        using var gbxwBody = new GameBoxWriter(msBody, w.Settings, logger);

        await Node.WriteAsync(gbxwBody, logger, cancellationToken);

        var buffer = msBody.ToArray();

        var output = Lzo.Compress(buffer);

        w.Write((int)msBody.Length); // Uncompressed
        w.Write(output.Length); // Compressed
        w.WriteBytes(output); // Compressed body data
    }
}
