using System.Diagnostics.CodeAnalysis;

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
#if NET6_0_OR_GREATER
    [RequiresUnreferencedCode(Lzo.TrimWarningIfDynamic)]
#endif
    internal static bool Read(Node node,
                              GameBoxHeader header,
                              GameBoxReader reader,
                              IProgress<GameBoxReadProgress>? progress,
                              bool readUncompressedBodyDirectly)
    {
        reader.Logger?.LogDebug("Reading the body...");

        switch (header.CompressionOfBody)
        {
            case GameBoxCompression.Compressed:
                ReadCompressed(node, reader, progress);
                break;
            case GameBoxCompression.Uncompressed:
                ReadUncompressed(node, reader, progress, readUncompressedBodyDirectly);
                break;
            default:
                reader.Logger?.LogError("Body can't be read! Compression type is unknown.");
                return false;
        }

        reader.Logger?.LogDebug("Body chunks parsed without major exceptions.");

        return true;
    }
   
#if NET6_0_OR_GREATER
    [RequiresUnreferencedCode(Lzo.TrimWarningIfDynamic)]
#endif
    private static void ReadCompressed(Node node, GameBoxReader reader, IProgress<GameBoxReadProgress>? progress)
    {
        var uncompressedSize = reader.ReadInt32();
        var compressedSize = reader.ReadInt32();

        var data = reader.ReadBytes(compressedSize);
        ReadMainNode(node, data, uncompressedSize, progress, reader.Logger, reader.State);
    }

    private static void ReadUncompressed(Node node,
                                         GameBoxReader reader,
                                         IProgress<GameBoxReadProgress>? progress,
                                         bool readUncompressedBodyDirectly)
    {
        if (readUncompressedBodyDirectly)
        {
            ReadMainNode(node, reader, progress);
            return;
        }

        var uncompressedData = reader.ReadToEnd();
        ReadMainNode(node, uncompressedData, progress, reader.Logger, reader.State);
    }



    /// <exception cref="MissingLzoException"></exception>
    /// <exception cref="NodeNotImplementedException">Auxiliary node is not implemented and is not parseable.</exception>
    /// <exception cref="ChunkReadNotImplementedException">Chunk does not support reading.</exception>
    /// <exception cref="IgnoredUnskippableChunkException">Chunk is known but its content is unknown to read.</exception>
#if NET6_0_OR_GREATER
    [RequiresUnreferencedCode(Lzo.TrimWarningIfDynamic)]
#endif
    internal static async Task<bool> ReadAsync(Node node,
                                               GameBoxHeader header,
                                               GameBoxReader reader,
                                               bool readUncompressedBodyDirectly,
                                               GameBoxAsyncReadAction? asyncAction,
                                               CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        reader.Logger?.LogDebug("Reading the body...");

        switch (header.CompressionOfBody)
        {
            case GameBoxCompression.Compressed:
                await ReadCompressedAsync(node, reader, asyncAction, cancellationToken);
                break;
            case GameBoxCompression.Uncompressed:
                await ReadUncompressedAsync(node, reader, readUncompressedBodyDirectly, asyncAction, cancellationToken);
                break;
            default:
                reader.Logger?.LogError("Body can't be read! Compression type is unknown.");
                return false;
        }

        reader.Logger?.LogDebug("Body chunks parsed without major exceptions.");

        return true;
    }
    
#if NET6_0_OR_GREATER
    [RequiresUnreferencedCode(Lzo.TrimWarningIfDynamic)]
#endif
    private static async Task ReadCompressedAsync(Node node,
                                                  GameBoxReader reader,
                                                  GameBoxAsyncReadAction? asyncAction,
                                                  CancellationToken cancellationToken)
    {
        var uncompressedSize = reader.ReadInt32();
        var compressedSize = reader.ReadInt32();

        var data = reader.ReadBytes(compressedSize);
        await ReadMainNodeAsync(node, data, uncompressedSize, reader.Logger, asyncAction, reader.State, cancellationToken);
    }

    private static async Task ReadUncompressedAsync(Node node,
                                                    GameBoxReader reader,
                                                    bool readUncompressedBodyDirectly,
                                                    GameBoxAsyncReadAction? asyncAction,
                                                    CancellationToken cancellationToken)
    {
        if (readUncompressedBodyDirectly)
        {
            await ReadMainNodeAsync(node, reader, cancellationToken);
            return;
        }

        var uncompressedData = reader.ReadToEnd();
        await ReadMainNodeAsync(node, uncompressedData, reader.Logger, asyncAction, reader.State, cancellationToken);
    }

    /// <exception cref="MissingLzoException"></exception>
    /// <exception cref="NodeNotImplementedException">Auxiliary node is not implemented and is not parseable.</exception>
    /// <exception cref="ChunkReadNotImplementedException">Chunk does not support reading.</exception>
    /// <exception cref="IgnoredUnskippableChunkException">Chunk is known but its content is unknown to read.</exception>
#if NET6_0_OR_GREATER
    [RequiresUnreferencedCode(Lzo.TrimWarningIfDynamic)]
#endif
    private static void ReadMainNode(Node node,
                                     byte[] data,
                                     int uncompressedSize,
                                     IProgress<GameBoxReadProgress>? progress,
                                     ILogger? logger,
                                     GbxState state)
    {
        var buffer = new byte[uncompressedSize];

        DecompressData(data, buffer);

        ReadMainNode(node, buffer, progress, logger, state);
    }

    /// <exception cref="NodeNotImplementedException">Auxiliary node is not implemented and is not parseable.</exception>
    /// <exception cref="ChunkReadNotImplementedException">Chunk does not support reading.</exception>
    /// <exception cref="IgnoredUnskippableChunkException">Chunk is known but its content is unknown to read.</exception>
    private static void ReadMainNode(Node node,
                                     byte[] data,
                                     IProgress<GameBoxReadProgress>? progress,
                                     ILogger? logger,
                                     GbxState state)
    {
        using var ms = new MemoryStream(data);
        ReadMainNode(node, ms, progress, logger, state);
    }

    /// <exception cref="NodeNotImplementedException">Auxiliary node is not implemented and is not parseable.</exception>
    /// <exception cref="ChunkReadNotImplementedException">Chunk does not support reading.</exception>
    /// <exception cref="IgnoredUnskippableChunkException">Chunk is known but its content is unknown to read.</exception>
    private static void ReadMainNode(Node node, Stream stream, IProgress<GameBoxReadProgress>? progress, ILogger? logger, GbxState state)
    {
        using var r = new GameBoxReader(
            stream,
            node.GetGbx() ?? throw new ThisShouldNotHappenException(),
            asyncAction: null,
            logger,
            state);
        ReadMainNode(node, r, progress);
    }

    /// <exception cref="NodeNotImplementedException">Auxiliary node is not implemented and is not parseable.</exception>
    /// <exception cref="ChunkReadNotImplementedException">Chunk does not support reading.</exception>
    /// <exception cref="IgnoredUnskippableChunkException">Chunk is known but its content is unknown to read.</exception>
    private static void ReadMainNode(Node node, GameBoxReader reader, IProgress<GameBoxReadProgress>? progress)
    {
        Node.Parse(node, reader, progress);

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
#if NET6_0_OR_GREATER
    [RequiresUnreferencedCode(Lzo.TrimWarningIfDynamic)]
#endif
    private static async Task ReadMainNodeAsync(Node node,
                                                byte[] data,
                                                int uncompressedSize,
                                                ILogger? logger,
                                                GameBoxAsyncReadAction? asyncAction,
                                                GbxState state,
                                                CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        
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

        await ReadMainNodeAsync(node, buffer, logger, asyncAction, state, cancellationToken);
    }

    private static async Task ReadMainNodeAsync(Node node,
                                                byte[] data,
                                                ILogger? logger,
                                                GameBoxAsyncReadAction? asyncAction,
                                                GbxState state,
                                                CancellationToken cancellationToken)
    {
        using var ms = new MemoryStream(data);
        await ReadMainNodeAsync(node, ms, logger, asyncAction, state, cancellationToken);
    }

    /// <exception cref="NodeNotImplementedException">Auxiliary node is not implemented and is not parseable.</exception>
    /// <exception cref="ChunkReadNotImplementedException">Chunk does not support reading.</exception>
    /// <exception cref="IgnoredUnskippableChunkException">Chunk is known but its content is unknown to read.</exception>
    private static async Task ReadMainNodeAsync(Node node,
                                                Stream stream,
                                                ILogger? logger,
                                                GameBoxAsyncReadAction? asyncAction,
                                                GbxState state,
                                                CancellationToken cancellationToken)
    {
        using var r = new GameBoxReader(
            stream, 
            node.GetGbx() ?? throw new ThisShouldNotHappenException(),
            asyncAction,
            logger,
            state);
        await ReadMainNodeAsync(node, r, cancellationToken);
    }

    /// <exception cref="NodeNotImplementedException">Auxiliary node is not implemented and is not parseable.</exception>
    /// <exception cref="ChunkReadNotImplementedException">Chunk does not support reading.</exception>
    /// <exception cref="IgnoredUnskippableChunkException">Chunk is known but its content is unknown to read.</exception>
    private static async Task ReadMainNodeAsync(Node node,
                                                GameBoxReader reader,
                                                CancellationToken cancellationToken)
    {        
        await Node.ParseAsync(node, reader, cancellationToken);

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
        if (bodyCompression == GameBoxCompression.Uncompressed)
        {
            return new GameBoxBody { RawData = r.ReadToEnd() };
        }

        var uncompressedSize = r.ReadInt32();
        var rawData = r.ReadBytes();

        return new GameBoxBody
        {
            UncompressedSize = uncompressedSize,
            RawData = rawData
        };
    }

#if NET6_0_OR_GREATER
    [RequiresUnreferencedCode(Lzo.TrimWarningIfDynamic)]
#endif
    internal void Write(GameBox gbx, GameBoxWriter bodyW)
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

            WriteMainNode(gbx.Node, gbx.Header, bodyW); // Body is written first so that the aux node count is determined properly

            return;
        }

        if (gbx.Header.CompressionOfBody == GameBoxCompression.Compressed)
        {
            bodyW.Write(UncompressedSize.GetValueOrDefault());
            bodyW.Write(RawData.Length);
        }

        bodyW.Write(RawData);
    }
    
#if NET6_0_OR_GREATER
    [RequiresUnreferencedCode(Lzo.TrimWarningIfDynamic)]
#endif
    internal async Task WriteAsync(GameBox gbx, GameBoxWriter bodyW, CancellationToken cancellationToken)
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

            await WriteMainNodeAsync(gbx.Node, gbx.Header, bodyW, cancellationToken); // Body is written first so that the aux node count is determined properly

            return;
        }

        if (gbx.Header.CompressionOfBody == GameBoxCompression.Compressed)
        {
            bodyW.Write(UncompressedSize.GetValueOrDefault());
            bodyW.Write(RawData.Length);
        }

        await bodyW.WriteBytesAsync(RawData, cancellationToken);
    }
    
#if NET6_0_OR_GREATER
    [RequiresUnreferencedCode(Lzo.TrimWarningIfDynamic)]
#endif
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

#if NET6_0_OR_GREATER
    [RequiresUnreferencedCode(Lzo.TrimWarningIfDynamic)]
#endif
    internal static void WriteMainNode(Node node, GameBoxHeader header, GameBoxWriter w)
    {
        if (header.CompressionOfBody == GameBoxCompression.Uncompressed)
        {
            node.Write(w);
            return;
        }

        using var msBody = new MemoryStream();
        using var gbxwBody = new GameBoxWriter(msBody, w);

        node.Write(gbxwBody);

        var buffer = msBody.ToArray();

        var output = Lzo.Compress(buffer);

        w.Write((int)msBody.Length); // Uncompressed
        w.Write(output.Length); // Compressed
        w.Write(output); // Compressed body data
    }

    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="MissingLzoException"></exception>
#if NET6_0_OR_GREATER
    [RequiresUnreferencedCode(Lzo.TrimWarningIfDynamic)]
#endif
    internal static async Task WriteMainNodeAsync(Node node, GameBoxHeader header, GameBoxWriter w, CancellationToken cancellationToken)
    {
        if (header.CompressionOfBody == GameBoxCompression.Uncompressed)
        {
            node.Write(w);
            return;
        }

        using var msBody = new MemoryStream();
        using var gbxwBody = new GameBoxWriter(msBody, w);

        await node.WriteAsync(gbxwBody, cancellationToken);

        var buffer = msBody.ToArray();

        var output = Lzo.Compress(buffer);

        w.Write((int)msBody.Length); // Uncompressed
        w.Write(output.Length); // Compressed
        w.Write(output); // Compressed body data
    }
}