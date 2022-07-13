using GBX.NET.Debugging;

namespace GBX.NET;

public class GameBoxBody
{
    public int? UncompressedSize { get; init; }

    /// <summary>
    /// Pure body data usually in compressed form. This property is used if GameBox's ParseHeader methods are used, otherwise null.
    /// </summary>
    public byte[]? RawData { get; init; }

    /// <exception cref="MissingLzoException"></exception>
    /// <exception cref="NodeNotImplementedException">Auxiliary node is not implemented and is not parseable.</exception>
    /// <exception cref="ChunkReadNotImplementedException">Chunk does not support reading.</exception>
    /// <exception cref="IgnoredUnskippableChunkException">Chunk is known but its content is unknown to read.</exception>
    internal static bool Read(Node node,
                              GameBoxHeader header,
                              GameBoxReader reader,
                              IProgress<GameBoxReadProgress>? progress,
                              bool readUncompressedBodyDirectly,
                              ILogger? logger)
    {
        reader.Settings.GetGbxOrThrow().ResetIdState();

        logger?.LogDebug("Reading the body...");

        switch (header.CompressionOfBody)
        {
            case GameBoxCompression.Compressed:
                ReadCompressed(node, reader, progress, logger);
                break;
            case GameBoxCompression.Uncompressed:
                ReadUncompressed(node, reader, progress, readUncompressedBodyDirectly, logger);
                break;
            default:
                logger?.LogError("Body can't be read! Compression type is unknown.");
                return false;
        }

        logger?.LogDebug("Body chunks parsed without major exceptions.");

        return true;
    }

    private static void ReadCompressed(Node node,
                                        GameBoxReader reader,
                                        IProgress<GameBoxReadProgress>? progress,
                                        ILogger? logger)
    {
        var uncompressedSize = reader.ReadInt32();
        var compressedSize = reader.ReadInt32();

        var data = reader.ReadBytes(compressedSize);
        ReadMainNode(node, data, uncompressedSize, progress, logger);
    }

    private static void ReadUncompressed(Node node,
                                            GameBoxReader reader,
                                            IProgress<GameBoxReadProgress>? progress,
                                            bool readUncompressedBodyDirectly,
                                            ILogger? logger)
    {
        if (readUncompressedBodyDirectly)
        {
            ReadMainNode(node, reader, progress, logger);
            return;
        }

        var uncompressedData = reader.ReadToEnd();
        ReadMainNode(node, uncompressedData, progress, logger);
    }



    /// <exception cref="MissingLzoException"></exception>
    /// <exception cref="NodeNotImplementedException">Auxiliary node is not implemented and is not parseable.</exception>
    /// <exception cref="ChunkReadNotImplementedException">Chunk does not support reading.</exception>
    /// <exception cref="IgnoredUnskippableChunkException">Chunk is known but its content is unknown to read.</exception>
    internal static async Task<bool> ReadAsync(Node node,
                                                GameBoxHeader header,
                                                GameBoxReader reader,
                                                bool readUncompressedBodyDirectly,
                                                ILogger? logger,
                                                GameBoxAsyncReadAction? asyncAction,
                                                CancellationToken cancellationToken)
    {
        logger?.LogDebug("Reading the body...");

        switch (header.CompressionOfBody)
        {
            case GameBoxCompression.Compressed:
                await ReadCompressedAsync(node, reader, logger, asyncAction, cancellationToken);
                break;
            case GameBoxCompression.Uncompressed:
                await ReadUncompressedAsync(node, reader, readUncompressedBodyDirectly, logger, asyncAction, cancellationToken);
                break;
            default:
                logger?.LogError("Body can't be read! Compression type is unknown.");
                return false;
        }

        logger?.LogDebug("Body chunks parsed without major exceptions.");

        return true;
    }

    private static async Task ReadCompressedAsync(Node node,
                                                    GameBoxReader reader,
                                                    ILogger? logger,
                                                    GameBoxAsyncReadAction? asyncAction,
                                                    CancellationToken cancellationToken)
    {
        var uncompressedSize = reader.ReadInt32();
        var compressedSize = reader.ReadInt32();

        var data = reader.ReadBytes(compressedSize);
        await ReadMainNodeAsync(node, data,
                                uncompressedSize,
                                logger,
                                asyncAction,
                                cancellationToken);
    }

    private static async Task ReadUncompressedAsync(Node node,
                                                    GameBoxReader reader,
                                                    bool readUncompressedBodyDirectly,
                                                    ILogger? logger,
                                                    GameBoxAsyncReadAction? asyncAction,
                                                    CancellationToken cancellationToken)
    {
        if (readUncompressedBodyDirectly)
        {
            await ReadMainNodeAsync(node, reader, logger, cancellationToken);
            return;
        }

        var uncompressedData = reader.ReadToEnd();
        await ReadMainNodeAsync(node, uncompressedData,
                                logger,
                                asyncAction,
                                cancellationToken);
    }

    /// <exception cref="MissingLzoException"></exception>
    /// <exception cref="NodeNotImplementedException">Auxiliary node is not implemented and is not parseable.</exception>
    /// <exception cref="ChunkReadNotImplementedException">Chunk does not support reading.</exception>
    /// <exception cref="IgnoredUnskippableChunkException">Chunk is known but its content is unknown to read.</exception>
    private static void ReadMainNode(Node node,
                                        byte[] data,
                                        int uncompressedSize,
                                        IProgress<GameBoxReadProgress>? progress,
                                        ILogger? logger)
    {
        var buffer = new byte[uncompressedSize];

        DecompressData(data, buffer);

        ReadMainNode(node, buffer, progress, logger);
    }

    /// <exception cref="NodeNotImplementedException">Auxiliary node is not implemented and is not parseable.</exception>
    /// <exception cref="ChunkReadNotImplementedException">Chunk does not support reading.</exception>
    /// <exception cref="IgnoredUnskippableChunkException">Chunk is known but its content is unknown to read.</exception>
    private static void ReadMainNode(Node node,
                                        byte[] data,
                                        IProgress<GameBoxReadProgress>? progress,
                                        ILogger? logger)
    {
        using var ms = new MemoryStream(data);
        ReadMainNode(node, ms, progress, logger);
    }

    /// <exception cref="NodeNotImplementedException">Auxiliary node is not implemented and is not parseable.</exception>
    /// <exception cref="ChunkReadNotImplementedException">Chunk does not support reading.</exception>
    /// <exception cref="IgnoredUnskippableChunkException">Chunk is known but its content is unknown to read.</exception>
    private static void ReadMainNode(Node node, Stream stream, IProgress<GameBoxReadProgress>? progress, ILogger? logger)
    {
        using var gbxr = new GameBoxReader(stream, node.GetGbx(), logger: logger);
        ReadMainNode(node, gbxr, progress, logger);
    }

    /// <exception cref="NodeNotImplementedException">Auxiliary node is not implemented and is not parseable.</exception>
    /// <exception cref="ChunkReadNotImplementedException">Chunk does not support reading.</exception>
    /// <exception cref="IgnoredUnskippableChunkException">Chunk is known but its content is unknown to read.</exception>
    private static void ReadMainNode(Node node,
                                        GameBoxReader reader,
                                        IProgress<GameBoxReadProgress>? progress,
                                        ILogger? logger)
    {
        Node.Parse(node, node.GetType(), reader, progress, logger);

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
    private static async Task ReadMainNodeAsync(Node node,
                                                byte[] data,
                                                int uncompressedSize,
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

        await ReadMainNodeAsync(node, buffer, logger, asyncAction, cancellationToken);
    }

    private static async Task ReadMainNodeAsync(Node node,
                                                byte[] data,
                                                ILogger? logger,
                                                GameBoxAsyncReadAction? asyncAction,
                                                CancellationToken cancellationToken)
    {
        using var ms = new MemoryStream(data);
        await ReadMainNodeAsync(node, ms, logger, asyncAction, cancellationToken);
    }

    /// <exception cref="NodeNotImplementedException">Auxiliary node is not implemented and is not parseable.</exception>
    /// <exception cref="ChunkReadNotImplementedException">Chunk does not support reading.</exception>
    /// <exception cref="IgnoredUnskippableChunkException">Chunk is known but its content is unknown to read.</exception>
    private static async Task ReadMainNodeAsync(Node node,
                                                Stream stream,
                                                ILogger? logger,
                                                GameBoxAsyncReadAction? asyncAction,
                                                CancellationToken cancellationToken)
    {
        using var gbxr = new GameBoxReader(stream, node.GetGbx(), logger: logger, asyncAction: asyncAction);
        await ReadMainNodeAsync(node, gbxr, logger, cancellationToken);
    }

    /// <exception cref="NodeNotImplementedException">Auxiliary node is not implemented and is not parseable.</exception>
    /// <exception cref="ChunkReadNotImplementedException">Chunk does not support reading.</exception>
    /// <exception cref="IgnoredUnskippableChunkException">Chunk is known but its content is unknown to read.</exception>
    private static async Task ReadMainNodeAsync(Node node,
                                                GameBoxReader reader,
                                                ILogger? logger,
                                                CancellationToken cancellationToken)
    {
        await Node.ParseAsync(node, node.GetType(), reader, logger, cancellationToken);

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

    public static GameBoxBody ParseRaw(GameBoxCompression bodyCompression, GameBoxReader r)
    {
        byte[]? rawData;

        if (bodyCompression == GameBoxCompression.Uncompressed)
        {
            rawData = r.ReadToEnd();

            return new GameBoxBody { RawData = rawData };
        }

        var uncompressedSize = r.ReadInt32();
        rawData = r.ReadBytes();

        return new GameBoxBody
        {
            UncompressedSize = uncompressedSize,
            RawData = rawData
        };
    }

    internal void Write(GameBox gbx, GameBoxWriter bodyW, ILogger? logger)
    {
        if (RawData is null)
        {
            if (gbx.Node is null)
            {
                throw new PropertyNullException(nameof(gbx.Node));
            }

            if (gbx.Node.Chunks.Count == 0)
            {
                throw new HeaderOnlyParseLimitationException();
            }

            WriteMainNode(gbx.Node, gbx.Header, bodyW, logger); // Body is written first so that the aux node count is determined properly

            return;
        }

        if (gbx.Header.CompressionOfBody == GameBoxCompression.Compressed)
        {
            bodyW.Write(UncompressedSize.GetValueOrDefault());
            bodyW.Write(RawData.Length);
        }

        bodyW.Write(RawData);
    }

    internal async Task WriteAsync(GameBox gbx,
                                    GameBoxWriter bodyW,
                                    ILogger? logger,
                                    CancellationToken cancellationToken)
    {
        if (RawData is null)
        {
            if (gbx.Node is null)
            {
                throw new PropertyNullException(nameof(gbx.Node));
            }

            if (gbx.Node.Chunks.Count == 0)
            {
                // "Body not possible to write" kind of exception would be more suitable
                throw new HeaderOnlyParseLimitationException();
            }

            await WriteMainNodeAsync(gbx.Node, gbx.Header, bodyW, logger, cancellationToken); // Body is written first so that the aux node count is determined properly

            return;
        }

        if (gbx.Header.CompressionOfBody == GameBoxCompression.Compressed)
        {
            bodyW.Write(UncompressedSize.GetValueOrDefault());
            bodyW.Write(RawData.Length);
        }

        await bodyW.WriteBytesAsync(RawData, cancellationToken);
    }

    private static void DecompressData(byte[] input, byte[] output)
    {
        Lzo.Decompress(input, output);

        /*if (Debug)
        {
            BodyDebugger ??= new();
            BodyDebugger.CompressedData = input;
            BodyDebugger.UncompressedData = output;
        }*/
    }

    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="MissingLzoException"></exception>
    internal static void WriteMainNode(Node node,
                                        GameBoxHeader header,
                                        GameBoxWriter w,
                                        ILogger? logger)
    {
        if (header.CompressionOfBody == GameBoxCompression.Uncompressed)
        {
            node.Write(w, logger);
            return;
        }

        using var msBody = new MemoryStream();
        using var gbxwBody = new GameBoxWriter(msBody, w.Settings, logger);

        node.Write(gbxwBody, logger);

        var buffer = msBody.ToArray();

        var output = Lzo.Compress(buffer);

        w.Write((int)msBody.Length); // Uncompressed
        w.Write(output.Length); // Compressed
        w.Write(output); // Compressed body data
    }

    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="MissingLzoException"></exception>
    internal static async Task WriteMainNodeAsync(Node node,
                                                    GameBoxHeader header,
                                                    GameBoxWriter w,
                                                    ILogger? logger,
                                                    CancellationToken cancellationToken)
    {
        if (header.CompressionOfBody == GameBoxCompression.Uncompressed)
        {
            node.Write(w, logger);
            return;
        }

        using var msBody = new MemoryStream();
        using var gbxwBody = new GameBoxWriter(msBody, w.Settings, logger);

        await node.WriteAsync(gbxwBody, logger, cancellationToken);

        var buffer = msBody.ToArray();

        var output = Lzo.Compress(buffer);

        w.Write((int)msBody.Length); // Uncompressed
        w.Write(output.Length); // Compressed
        w.Write(output); // Compressed body data
    }
}