using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace GBX.NET;

/// <summary>
/// The skeleton of the Gbx object and representation of the <see cref="CMwNod"/>.
/// </summary>
/// <remarks>You shouldn't inherit this class unless <see cref="CMwNod"/> cannot be inherited instead.</remarks>
public abstract class Node
{
    private uint? id;
    private ChunkSet? chunks;
    private GameBox? gbx;
    private GameBox? gbxForRefTable;

    [Hexadecimal]
    public virtual uint Id => GetStoredId();

    public ChunkSet Chunks
    {
        get
        {
            chunks ??= new ChunkSet();
            return chunks;
        }
    }

    protected Node()
    {

    }

    /// <summary>
    /// Gets the <see cref="GameBox"/> object holding the main node.
    /// </summary>
    /// <returns>The holding <see cref="GameBox"/> object, if THIS node is the main node, otherwise null.</returns>
    public GameBox? GetGbx()
    {
        return gbx;
    }

    internal void SetGbx(GameBox gbx)
    {
        this.gbx = gbx;
    }

    private uint GetStoredId()
    {
        id ??= GetId();
        return id.Value;
    }

    private uint GetId()
    {
        return NodeManager.ClassIdsByType[GetType()];
    }

    /// <summary>
    /// Returns the name of the class formatted as <c>[engine]::[class]</c>.
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        var type = GetType();

        if (NodeManager.TryGetName(Id, out string? name))
        {
            return name!;
        }

        return type.FullName?.Substring("GBX.NET.Engines".Length + 1).Replace(".", "::") ?? string.Empty;
    }

    protected internal ExternalNode<T>[]? GetNodesFromRefTable<T>(ExternalNode<T>[]? nodesAtTheMoment) where T : Node
    {
        if (nodesAtTheMoment is null)
        {
            return null;
        }

        for (var i = 0; i < nodesAtTheMoment.Length; i++)
        {
            try
            {
                nodesAtTheMoment[i] = GetNodeFromRefTable(nodesAtTheMoment[i]);
            }
            catch
            {
                
            }
        }

        return nodesAtTheMoment;
    }

    protected internal ExternalNode<T> GetNodeFromRefTable<T>(ExternalNode<T> nodeAtTheMoment) where T : Node
    {
        return nodeAtTheMoment with { Node = GetNodeFromRefTable(nodeAtTheMoment.Node, nodeAtTheMoment.File) as T };
    }

    protected internal Node? GetNodeFromRefTable(Node? nodeAtTheMoment, GameBoxRefTable.File? nodeFile)
    {
        return GetNodeFromRefTable(gbxForRefTable, nodeAtTheMoment, nodeFile);
    }

    internal static Node? GetNodeFromRefTable(GameBox? gbx, Node? nodeAtTheMoment, GameBoxRefTable.File? nodeFile)
    {
        if (nodeAtTheMoment is not null || nodeFile is null || gbx is null)
        {
            return nodeAtTheMoment;
        }

        var refTable = gbx.RefTable;

        if (refTable is null)
        {
            return nodeAtTheMoment;
        }

        var fileName = gbx.PakFileName ?? gbx.FileName;

        return refTable.GetNode(nodeAtTheMoment, nodeFile, fileName, gbx?.ExternalGameData);
    }

    internal static T? Parse<T>(GameBoxReader r, uint? classId, IProgress<GameBoxReadProgress>? progress, bool ignoreZeroIdChunk = false) where T : Node
    {
        return Parse(r, classId, progress, ignoreZeroIdChunk) as T;
    }

    /// <exception cref="NodeNotImplementedException">Auxiliary node is not implemented and is not parseable.</exception>
    /// <exception cref="ChunkReadNotImplementedException">Chunk does not support reading.</exception>
    /// <exception cref="IgnoredUnskippableChunkException">Chunk is known but its content is unknown to read.</exception>
    internal static Node? Parse(GameBoxReader r,
                                uint? classId,
                                IProgress<GameBoxReadProgress>? progress,
                                bool ignoreZeroIdChunk = false)
    {
        _ = GetNodeInfoFromClassId(r, classId, out Node? node);

        if (node is null)
        {
            return null;
        }

        Parse(node, r, progress, ignoreZeroIdChunk);

        return node;
    }

    private static uint GetNodeInfoFromClassId(GameBoxReader r, uint? classId, out Node? node)
    {
        classId ??= r.ReadUInt32();

        if (classId == uint.MaxValue)
        {
            node = null;

            return classId.Value;
        }

        classId = RemapToLatest(classId.Value);

        var id = classId.Value;
        
        node = NodeManager.GetNewNode(id);

        if (node is null)
        {
            throw new NodeNotImplementedException(id);
        }

        return classId.Value;
    }

    /// <exception cref="NodeNotImplementedException">Auxiliary node is not implemented and is not parseable.</exception>
    /// <exception cref="ChunkReadNotImplementedException">Chunk does not support reading.</exception>
    /// <exception cref="IgnoredUnskippableChunkException">Chunk is known but its content is unknown to read.</exception>
    internal static void Parse(Node node,
                               GameBoxReader r,
                               IProgress<GameBoxReadProgress>? progress,
                               bool ignoreZeroIdChunk = false)
    {
        node.gbxForRefTable = r.Gbx;

        var stopwatch = Stopwatch.StartNew();

        using var scope = r.Logger?.BeginScope("{name}", node.GetType().Name);

        if (r.BaseStream is IXorTrickStream cryptedStream)
        {
            var baseType = node.GetType().BaseType ?? throw new ThisShouldNotHappenException();

            var parentClassId = NodeManager.ClassIdsByType[baseType];

            if (parentClassId == 0x07031000)
            {
                parentClassId = 0x07001000;
            }

            if (node is CPlugSurfaceGeom)
            {
                parentClassId = 0x0902B000;
            }

            if (baseType == typeof(CGameCtnBlockInfo))
            {
                parentClassId = 0x24005000;
            }

            var parentClassIDBytes = BitConverter.GetBytes(parentClassId);

            cryptedStream.InitializeXorTrick(parentClassIDBytes, 0, 4);
        }

        node.ReadChunkData(r, progress, ignoreZeroIdChunk);

        stopwatch.Stop();

        r.Logger?.LogNodeComplete(time: stopwatch.Elapsed.TotalMilliseconds);
    }

    protected virtual void ReadChunkData(GameBoxReader r, IProgress<GameBoxReadProgress>? progress, bool ignoreZeroIdChunk)
    {
        var previousChunkId = default(uint?);

        while (IterateChunks(this, r, progress, ref previousChunkId, ignoreZeroIdChunk))
        {
            // Iterates through chunks until false is returned
        }
    }

    /// <exception cref="NodeNotImplementedException">Auxiliary node is not implemented and is not parseable.</exception>
    /// <exception cref="ChunkReadNotImplementedException">Chunk does not support reading.</exception>
    /// <exception cref="IgnoredUnskippableChunkException">Chunk is known but its content is unknown to read.</exception>
    internal static async Task<Node?> ParseAsync(GameBoxReader r, uint? classId, CancellationToken cancellationToken = default)
    {
        var realClassId = GetNodeInfoFromClassId(r, classId, out Node? node);

        if (node is null)
        {
            return null;
        }

        r.AsyncAction?.OnReadNode?.Invoke(r, realClassId);

        await ParseAsync(node, r, cancellationToken);

        return node;
    }

    internal static async Task ParseAsync(Node node,
                                          GameBoxReader r,
                                          CancellationToken cancellationToken = default)
    {
        var stopwatch = Stopwatch.StartNew();

        using var scope = r.Logger?.BeginScope("{name} (async)", node.GetType().Name);

        cancellationToken.ThrowIfCancellationRequested();
        await node.ReadChunkDataAsync(r, cancellationToken);

        stopwatch.Stop();

        r.Logger?.LogNodeComplete(time: stopwatch.Elapsed.TotalMilliseconds);
    }

    protected virtual async Task ReadChunkDataAsync(GameBoxReader r, CancellationToken cancellationToken)
    {
        var previousChunkId = default(uint?);

        while (true)
        {
            // Iterates through chunks until false is returned
            (var moreChunks, previousChunkId, var chunk) = await IterateChunksAsync(this, r, previousChunkId, cancellationToken);

            if (!moreChunks)
            {
                break;
            }

            var asyncAction = r.AsyncAction;

            if (asyncAction is not null && asyncAction.AfterChunkIteration is not null)
            {
                await asyncAction.AfterChunkIteration(this, chunk);
            }

            cancellationToken.ThrowIfCancellationRequested();
        }
    }

    private static bool IterateChunks(Node node,
                                      GameBoxReader r,
                                      IProgress<GameBoxReadProgress>? progress,
                                      ref uint? previousChunkId,
                                      bool ignoreZeroIdChunk = false)
    {
        var stream = r.BaseStream;
        var canSeek = stream.CanSeek;

        if (canSeek && stream.Position + 4 > stream.Length)
        {
            r.Logger?.LogUnexpectedEndOfStream(stream.Position, stream.Length);
            var bytes = r.ReadBytes((int)(stream.Length - stream.Position));
            return false;
        }

        var originalChunkId = r.ReadUInt32();

        if (originalChunkId == 0xFACADE01) // no more chunks
        {
            return false;
        }

        if (previousChunkId is null && ignoreZeroIdChunk && originalChunkId == 0)
        {
            return false;
        }

        LogChunkProgress(r.Logger, stream, originalChunkId);

        var chunkId = Chunk.Remap(originalChunkId);

        var chunkClass = NodeManager.GetChunkTypeById(chunkId);

        var reflected = chunkClass is not null;
        var skippable = reflected && NodeManager.IsSkippableChunk(chunkId);

        Chunk chunk;

        // Unknown or skippable chunk
        if (!reflected || skippable)
        {
            var skip = r.ReadUInt32();

            if (skip != 0x534B4950)
            {
                if (reflected)
                {
                    return false;
                }

                BeforeChunkParseException(r);

                throw new ChunkParseException(chunkId, previousChunkId);
            }

            chunk = ReadSkippableChunk(node, r, chunkId, reflected);
        }
        else // Known or unskippable chunk
        {
            chunk = ReadUnskippableChunk(node, r, chunkId);
        }

        node.Chunks.Add(chunk);

        progress?.Report(new GameBoxReadProgress(GameBoxReadProgressStage.Body, (float)stream.Position / stream.Length, gbx: null, chunk));

        previousChunkId = chunkId;

        return true;
    }

    private static Chunk ReadUnskippableChunk(Node node, GameBoxReader r, uint chunkId)
    {
        var chunk = NodeManager.GetNewChunk(chunkId) ?? throw new ThisShouldNotHappenException();
        chunk.OnLoad(); // The chunk is immediately activated, but is not parsed at this state

        var gbxrw = new GameBoxReaderWriter(r);

        var chunkAttributes = NodeManager.ChunkAttributesById[chunkId];

        if (chunkAttributes.Ignore)
        {
            throw new IgnoredUnskippableChunkException(node, chunkId);
        }

        var streamPos = default(long);

        if (GameBox.SeekForRawChunkData)
        {
            streamPos = r.BaseStream.Position;
        }

        try
        {
            if (chunkAttributes.AutoReadWrite)
            {
                var unknown = new GameBoxWriter(chunk.Unknown);
                var unknownData = r.ReadUntilFacade().ToArray();
                unknown.Write(unknownData);
            }
            else
            {
                // Why does it have to be IChunk?
                ((IReadableWritableChunk)chunk).ReadWrite(node, gbxrw);
            }
        }
        catch (EndOfStreamException) // May not be needed
        {
            r.Logger?.LogDebug("Unexpected end of the stream while reading the chunk.");
        }

        if (GameBox.SeekForRawChunkData)
        {
            SetChunkRawData(r.BaseStream, chunk, streamPos);
        }

        return chunk;
    }

    private static async Task<(bool moreChunks, uint? previousChunkId, Chunk? chunk)> IterateChunksAsync(
        Node node,
        GameBoxReader r,
        uint? previousChunkId,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var stream = r.BaseStream;
        var canSeek = stream.CanSeek;

        if (canSeek && stream.Position + 4 > stream.Length)
        {
            r.Logger?.LogUnexpectedEndOfStream(stream.Position, stream.Length);
            var bytes = r.ReadBytes((int)(stream.Length - stream.Position));
            return (false, previousChunkId, null);
        }

        var chunkId = r.ReadUInt32();

        if (chunkId == 0xFACADE01) // no more chunks
        {
            return (false, previousChunkId, null);
        }

        LogChunkProgress(r.Logger, stream, chunkId);

        chunkId = Chunk.Remap(chunkId);

        var chunkClass = NodeManager.GetChunkTypeById(chunkId);

        var reflected = chunkClass is not null;
        var skippable = reflected && NodeManager.IsSkippableChunk(chunkId);

        Chunk chunk;

        // Unknown or skippable chunk
        if (!reflected || skippable)
        {
            var skip = r.ReadUInt32();

            if (skip != 0x534B4950)
            {
                if (reflected)
                {
                    return (false, previousChunkId, null);
                }

                BeforeChunkParseException(r);

                throw new ChunkParseException(chunkId, previousChunkId);
            }

            chunk = await ReadSkippableChunkAsync(node, r, chunkId, reflected, cancellationToken);
        }
        else // Known or unskippable chunk
        {
            chunk = await ReadUnskippableChunkAsync(node, r, chunkId, cancellationToken);
        }

        node.Chunks.Add(chunk);

        return (true, chunkId, chunk);
    }

    private static async Task<Chunk> ReadUnskippableChunkAsync(
        Node node,
        GameBoxReader r,
        uint chunkId,
        CancellationToken cancellationToken)
    {
        var chunk = NodeManager.GetNewChunk(chunkId) ?? throw new ThisShouldNotHappenException();
        chunk.OnLoad(); // The chunk is immediately activated, but may not be yet parsed at this state

        var gbxrw = new GameBoxReaderWriter(r);

        var chunkAttributes = NodeManager.ChunkAttributesById[chunkId];

        if (chunkAttributes.Ignore)
        {
            throw new IgnoredUnskippableChunkException(node, chunkId);
        }

        var streamPos = default(long);

        if (GameBox.SeekForRawChunkData)
        {
            streamPos = r.BaseStream.Position;
        }

        try
        {
            if (chunkAttributes.AutoReadWrite)
            {
                var unknown = new GameBoxWriter(chunk.Unknown);
                var unknownData = r.ReadUntilFacade().ToArray();
                unknown.Write(unknownData);
            }
            else
            {
                await ReadWriteChunkAsync(node, (IReadableWritableChunk)chunk, gbxrw, cancellationToken);
            }
        }
        catch (EndOfStreamException) // May not be needed
        {
            r.Logger?.LogWarning("Unexpected end of the stream while reading the chunk.");
        }

        if (GameBox.SeekForRawChunkData)
        {
            await SetChunkRawDataAsync(r.BaseStream, chunk, streamPos, cancellationToken);
        }

        return chunk;
    }

    private static void SetChunkRawData(Stream stream, Chunk chunk, long streamPos)
    {
        if (!stream.CanSeek)
        {
            return;
        }

        var chunkLength = (int)(stream.Position - streamPos);
        stream.Position = streamPos;

        var rawData = new byte[chunkLength];

        _ = stream.Read(rawData, 0, chunkLength);

        chunk.RawData = rawData;
    }

    private static async Task SetChunkRawDataAsync(Stream stream, Chunk chunk, long streamPos, CancellationToken cancellationToken)
    {
        if (!stream.CanSeek)
        {
            return;
        }

        var chunkLength = (int)(stream.Position - streamPos);
        stream.Position = streamPos;

        var rawData = new byte[chunkLength];

#if NET6_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
        _ = await stream.ReadAsync(rawData, cancellationToken);
#else
        _ = await stream.ReadAsync(rawData, 0, rawData.Length, cancellationToken);
#endif

        chunk.RawData = rawData;
    }

    private static void BeforeChunkParseException(GameBoxReader r)
    {
        /*if (GameBox.Debug && r.BaseStream.CanSeek) // I don't get it
        {
            // Read the rest of the body

            var streamPos = r.BaseStream.Position;
            var uncontrollableData = r.ReadToEnd();
            r.BaseStream.Position = streamPos;
        }*/
    }

    private static Chunk ReadSkippableChunk(Node node,
                                            GameBoxReader r,
                                            uint chunkId,
                                            bool reflected)
    {
        var chunkData = r.ReadBytes();

        return reflected
            ? CreateKnownSkippableChunk(node, chunkId, chunkData, r)
            : CreateUnknownSkippableChunk(node, chunkId, chunkData, r.Logger);
    }

    private static async Task<Chunk> ReadSkippableChunkAsync(Node node,
                                                             GameBoxReader r,
                                                             uint chunkId,
                                                             bool reflected,
                                                             CancellationToken cancellationToken)
    {
        var chunkData = new byte[r.ReadInt32()];
        var read = await r.BaseStream.ReadAsync(chunkData, 0, chunkData.Length, cancellationToken);

        if (read != chunkData.Length)
        {
            throw new EndOfStreamException();
        }

        return reflected
            ? CreateKnownSkippableChunk(node, chunkId, chunkData, r)
            : CreateUnknownSkippableChunk(node, chunkId, chunkData, r.Logger);
    }

    private static Chunk CreateUnknownSkippableChunk(Node node,
                                                     uint chunkId,
                                                     byte[] chunkData,
                                                     ILogger? logger)
    {
        logger?.LogChunkUnknownSkippable(chunkId.ToString("X8"), chunkData.Length);

        var chunk = (Chunk)Activator.CreateInstance(typeof(SkippableChunk<>).MakeGenericType(node.GetType()), new object?[] { node, chunkData, chunkId })!;

        if (GameBox.SeekForRawChunkData)
        {
            chunk.RawData = chunkData;
        }

        return chunk;
    }

    private static Chunk CreateKnownSkippableChunk(Node node, uint chunkId, byte[] chunkData, GameBoxReader reader)
    {
        var chunkAttributes = NodeManager.ChunkAttributesById[chunkId];
        var chunk = NodeManager.GetNewChunk(chunkId) ?? throw new ThisShouldNotHappenException();

        var skippableChunk = (ISkippableChunk)chunk;
        skippableChunk.Data = chunkData;
        skippableChunk.Gbx = reader.Gbx;
        skippableChunk.Node = node; //

        if (chunkData.Length == 0)
        {
            skippableChunk.Discovered = true;
        }

        // If the chunk should be taken as "activated" (not necessarily to be immediately parsed)
        if (!chunkAttributes.Ignore)
        {
            chunk.OnLoad();

            if (chunkAttributes.ProcessSync)
            {
                using var ms = new MemoryStream(chunkData);
                using var r = new GameBoxReader(ms, reader);
                var rw = new GameBoxReaderWriter(r);

                skippableChunk.ReadWrite(node, rw);
                skippableChunk.Discovered = true;
                
                if (ms.Position != ms.Length)
                {
                    reader.Logger?.LogWarning("Skippable chunk 0x{chunkId} has {chunkSize} bytes left.", chunkId.ToString("X8"), ms.Length - ms.Position);
                }
            }
        }

        if (GameBox.SeekForRawChunkData)
        {
            chunk.RawData = chunkData;
        }

        return chunk;
    }

    private static void LogChunkProgress(ILogger? logger, Stream stream, uint chunkId)
    {
        if (stream.CanSeek)
        {
            var progressPercentage = (float)stream.Position / stream.Length;
            logger?.LogChunkProgress(chunkHex: chunkId.ToString("X8"), progressPercentage * 100);

            return;
        }

        logger?.LogChunkProgressSeekless(chunkHex: chunkId.ToString("X8"));
    }

    internal void Write(GameBoxWriter w)
    {
        var stopwatch = Stopwatch.StartNew();

        var nodeType = GetType();

        ThrowIfWritingIsNotSupported(nodeType);

        using var scope = w.Logger?.BeginScope("{name}", nodeType.Name);

        WriteChunkData(w);

        w.Logger?.LogNodeComplete(stopwatch.Elapsed.TotalMilliseconds);
    }

    protected virtual void WriteChunkData(GameBoxWriter w)
    {
        var counter = 0;

        foreach (IReadableWritableChunk chunk in Chunks)
        {
            counter++;
            PrepareToWriteChunk(w.Logger, counter, chunk);
            WriteChunk(chunk, w);
        }

        WriteFacade(w);
    }

    internal async Task WriteAsync(GameBoxWriter w, CancellationToken cancellationToken)
    {
        var stopwatch = Stopwatch.StartNew();

        var nodeType = GetType();

        ThrowIfWritingIsNotSupported(nodeType);

        cancellationToken.ThrowIfCancellationRequested();

        using var scope = w.Logger?.BeginScope("{name}", nodeType.Name);

        await WriteChunkDataAsync(w, cancellationToken);

        w.Logger?.LogNodeComplete(stopwatch.Elapsed.TotalMilliseconds);
    }

    protected virtual async Task WriteChunkDataAsync(GameBoxWriter w, CancellationToken cancellationToken)
    {
        var counter = 0;
        
        foreach (IReadableWritableChunk chunk in Chunks)
        {
            counter++;
            PrepareToWriteChunk(w.Logger, counter, chunk);
            await WriteChunkAsync(chunk, w, cancellationToken);

            var asyncAction = w.AsyncAction;

            if (asyncAction is not null && asyncAction.AfterChunkIteration is not null)
            {
                await asyncAction.AfterChunkIteration(this, chunk as Chunk);
            }

            cancellationToken.ThrowIfCancellationRequested();
        }

        WriteFacade(w);
    }

    private static void WriteFacade(GameBoxWriter w)
    {
        w.Write(0xFACADE01);
    }

    private static void ThrowIfWritingIsNotSupported(Type nodeType)
    {
        if (NodeManager.WritingNotSupportedClassTypes.Contains(nodeType))
        {
            throw new NotSupportedException($"Writing is not supported for {nodeType.Name}.");
        }
    }

    private void WriteChunk(IReadableWritableChunk chunk, GameBoxWriter w)
    {
        using var ms = new MemoryStream();

        using var msW = new GameBoxWriter(ms, w);
        var rw = new GameBoxReaderWriter(msW);

        w.Write(Chunk.Remap(chunk.Id, w.Remap));

        switch (chunk)
        {
            case ISkippableChunk skippableChunk:
                WriteSkippableChunk(skippableChunk, w, msW, rw);
                break;
            default:
                WriteUnskippableChunk(chunk, msW, rw);
                break;
        }

        w.Write(ms.ToArray());
    }

    private async Task WriteChunkAsync(IReadableWritableChunk chunk, GameBoxWriter w, CancellationToken cancellationToken)
    {
        using var ms = new MemoryStream();
        var msW = new GameBoxWriter(ms, w);
        var rw = new GameBoxReaderWriter(msW);

        w.Write(Chunk.Remap(chunk.Id, w.Remap));

        switch (chunk)
        {
            case ISkippableChunk skippableChunk:
                await WriteSkippableChunkAsync(skippableChunk, w, msW, rw, cancellationToken);
                break;
            default:
                await WriteUnskippableChunkAsync(chunk, msW, rw, cancellationToken);
                break;
        }

        await w.WriteBytesAsync(ms.ToArray(), cancellationToken);
    }

    private void WriteUnskippableChunk(IReadableWritableChunk chunk, GameBoxWriter msW, GameBoxReaderWriter rw)
    {
        if (IsIgnorableChunk(chunk.Id))
        {
            msW.Write(chunk.Unknown.ToArray());
        }
        else
        {
            chunk.ReadWrite(this, rw);
        }
    }

    private async Task WriteUnskippableChunkAsync(IReadableWritableChunk chunk,
                                                  GameBoxWriter msW,
                                                  GameBoxReaderWriter rw,
                                                  CancellationToken cancellationToken)
    {
        if (IsIgnorableChunk(chunk.Id))
        {
            await msW.WriteBytesAsync(chunk.Unknown.ToArray(), cancellationToken);
            return;
        }

        await ReadWriteChunkAsync(chunk, rw, cancellationToken);
    }

    private static async Task ReadWriteChunkAsync(Node node,
                                                  IReadableWritableChunk chunk,
                                                  GameBoxReaderWriter rw,
                                                  CancellationToken cancellationToken)
    {
        if (NodeManager.IsAsyncChunk(chunk.Id))
        {
            if (NodeManager.IsReadWriteAsyncChunk(chunk.Id) || NodeManager.IsReadAsyncChunk(chunk.Id))
            {
                await chunk.ReadWriteAsync(node, rw, cancellationToken);
                return;
            }

            throw new ThisShouldNotHappenException();
        }

        chunk.ReadWrite(node, rw);
    }

    private async Task ReadWriteChunkAsync(IReadableWritableChunk chunk,
                                           GameBoxReaderWriter rw,
                                           CancellationToken cancellationToken)
    {
        await ReadWriteChunkAsync(this, chunk, rw, cancellationToken);
    }

    private void WriteSkippableChunk(ISkippableChunk skippableChunk,
                                     GameBoxWriter mainWriter,
                                     GameBoxWriter chunkWriter,
                                     GameBoxReaderWriter rw)
    {
        WriteSkippableChunkBody(skippableChunk, chunkWriter, rw);

        mainWriter.Write(0x534B4950);
        mainWriter.Write((uint)chunkWriter.BaseStream.Length);
    }

    private void WriteSkippableChunkBody(ISkippableChunk skippableChunk, GameBoxWriter chunkWriter, GameBoxReaderWriter rw)
    {
        if (!skippableChunk.Discovered)
        {
            skippableChunk.Write(chunkWriter);
            return;
        }

        if (IsIgnorableChunk(skippableChunk.Id))
        {
            chunkWriter.Write(skippableChunk.Data);
            return;
        }

        try
        {
            skippableChunk.ReadWrite(this, rw);
        }
        catch (ChunkWriteNotImplementedException)
        {
            chunkWriter.Write(skippableChunk.Data);
        }
    }

    private async Task WriteSkippableChunkAsync(ISkippableChunk skippableChunk,
                                                GameBoxWriter mainWriter,
                                                GameBoxWriter chunkWriter,
                                                GameBoxReaderWriter rw,
                                                CancellationToken cancellationToken)
    {
        await WriteSkippableChunkBodyAsync(skippableChunk, chunkWriter, rw, cancellationToken);

        mainWriter.Write(0x534B4950);
        mainWriter.Write((uint)chunkWriter.BaseStream.Length);
    }

    private async Task WriteSkippableChunkBodyAsync(ISkippableChunk skippableChunk, GameBoxWriter chunkWriter, GameBoxReaderWriter rw, CancellationToken cancellationToken)
    {
        if (!skippableChunk.Discovered)
        {
            await skippableChunk.WriteAsync(chunkWriter, cancellationToken);
            return;
        }

        if (IsIgnorableChunk(skippableChunk.Id))
        {
            await chunkWriter.WriteBytesAsync(skippableChunk.Data, cancellationToken);
            return;
        }

        try
        {
            await ReadWriteChunkAsync(skippableChunk, rw, cancellationToken);
        }
        catch (ChunkWriteNotImplementedException)
        {
            await chunkWriter.WriteBytesAsync(skippableChunk.Data, cancellationToken);
        }
    }

    private void PrepareToWriteChunk(ILogger? logger, int counter, IReadableWritableChunk chunk)
    {
        logger?.LogChunkProgress(chunk.Id.ToString("X8"), (float)counter / Chunks.Count * 100);

        chunk.Unknown.Position = 0;
    }

    private static bool IsIgnorableChunk(uint chunkId)
    {
        return NodeManager.ChunkAttributesById[chunkId].Ignore;
    }

    public T? GetChunk<T>() where T : Chunk
    {
        return Chunks.Get<T>();
    }

    public T? GetChunkAndDiscover<T>() where T : Chunk, ISkippableChunk
    {
        var chunk = Chunks.Get<T>();

        if (chunk is not null)
        {
            chunk.Discover();
        }

        return chunk;
    }

    public T CreateChunk<T>() where T : Chunk
    {
        return Chunks.Create<T>();
    }

    public bool RemoveChunk<T>() where T : Chunk
    {
        return Chunks.Remove<T>();
    }

    public bool RemoveChunk(uint chunkID)
    {
        return Chunks.Remove(chunkID);
    }

#if NETSTANDARD2_1_OR_GREATER || NET6_0_OR_GREATER
    public bool TryGetChunk<T>([NotNullWhen(true)] out T? chunk) where T : Chunk
#else
    public bool TryGetChunk<T>(out T? chunk) where T : Chunk
#endif
    {
        return Chunks.TryGet(out chunk);
    }

    public void DiscoverChunk<T>() where T : Chunk, ISkippableChunk
    {
        Chunks.Discover<T>();
    }

    public void DiscoverChunks<T1, T2>() where T1 : Chunk, ISkippableChunk where T2 : Chunk, ISkippableChunk
    {
        Chunks.Discover<T1, T2>();
    }

    public void DiscoverChunks<T1, T2, T3>() where T1 : Chunk, ISkippableChunk where T2 : Chunk, ISkippableChunk where T3 : Chunk, ISkippableChunk
    {
        Chunks.Discover<T1, T2, T3>();
    }

    public void DiscoverChunks<T1, T2, T3, T4>()
        where T1 : Chunk, ISkippableChunk
        where T2 : Chunk, ISkippableChunk
        where T3 : Chunk, ISkippableChunk
        where T4 : Chunk, ISkippableChunk
    {
        Chunks.Discover<T1, T2, T3, T4>();
    }

    public void DiscoverChunks<T1, T2, T3, T4, T5>()
        where T1 : Chunk, ISkippableChunk
        where T2 : Chunk, ISkippableChunk
        where T3 : Chunk, ISkippableChunk
        where T4 : Chunk, ISkippableChunk
        where T5 : Chunk, ISkippableChunk
    {
        Chunks.Discover<T1, T2, T3, T4, T5>();
    }

    public void DiscoverChunks<T1, T2, T3, T4, T5, T6>()
        where T1 : Chunk, ISkippableChunk
        where T2 : Chunk, ISkippableChunk
        where T3 : Chunk, ISkippableChunk
        where T4 : Chunk, ISkippableChunk
        where T5 : Chunk, ISkippableChunk
        where T6 : Chunk, ISkippableChunk
    {
        Chunks.Discover<T1, T2, T3, T4, T5, T6>();
    }

    /// <summary>
    /// Discovers all chunks in the node.
    /// </summary>
    /// <exception cref="AggregateException"/>
    public void DiscoverAllChunks()
    {
        Chunks.DiscoverAll();

        foreach (var nodeProperty in GetType().GetProperties())
        {
            if (!nodeProperty.PropertyType.IsSubclassOf(typeof(Node)))
                continue;

            var node = nodeProperty.GetValue(this) as Node;
            node?.DiscoverAllChunks();
        }
    }

    /// <summary>
    /// Wraps this node into <see cref="GameBox"/> object.
    /// </summary>
    /// <param name="nonGeneric">If to instantiate a <see cref="GameBox"/> object instead of the generic <see cref="GameBox{T}"/>, where comparing of the node type has to be done on <see cref="GameBox.Node"/> level, but the method executes faster.</param>
    /// <returns>A <see cref="GameBox"/> or <see cref="GameBox{T}"/> depending on the <paramref name="nonGeneric"/> parameter.</returns>
    public GameBox ToGbx(bool nonGeneric = false)
    {
        if (nonGeneric)
        {
            return new GameBox(this);
        }

        if (Activator.CreateInstance(typeof(GameBox<>).MakeGenericType(GetType()), this) is not GameBox gbx)
        {
            throw new ThisShouldNotHappenException();
        }

        return gbx;
    }

    /// <summary>
    /// Wraps this node into <see cref="GameBox{T}"/> object with explicit conversion.
    /// </summary>
    /// <typeparam name="T">Type of the node to use on <see cref="GameBox{T}"/>.</typeparam>
    /// <returns>A <see cref="GameBox{T}"/>.</returns>
    public GameBox<T> ToGbx<T>() where T : Node
    {
        return new GameBox<T>((T)this);
    }

    /// <summary>
    /// Saves the serialized node on a disk in a Gbx form.
    /// </summary>
    /// <param name="fileName">Relative or absolute file path. Null will pick the <see cref="GameBox.FileName"/> value from <see cref="GBX"/> object instead.</param>
    /// <param name="remap">What to remap the newest node IDs to. Used for older games.</param>
    /// <param name="logger">Logger.</param>
    public void Save(string? fileName = default, IDRemap remap = default, ILogger? logger = null)
    {
        var gbx = GetGbx();

        if (gbx is null)
        {
            ToGbx(nonGeneric: true).Save(fileName, remap, logger);
            return;
        }

        gbx.Save(fileName, remap, logger);
    }

    /// <summary>
    /// Saves the serialized node to a stream in a Gbx form.
    /// </summary>
    /// <param name="stream">Any kind of stream that supports writing.</param>
    /// <param name="remap">What to remap the newest node IDs to. Used for older games.</param>
    /// <param name="logger">Logger.</param>
    public void Save(Stream stream, IDRemap remap = default, ILogger? logger = null)
    {
        var gbx = GetGbx();

        if (gbx is null)
        {
            ToGbx(nonGeneric: true).Save(stream, remap, logger);
            return;
        }

        gbx.Save(stream, remap, logger);
    }

    /// <summary>
    /// Saves the serialized node on a disk in a Gbx form.
    /// </summary>
    /// <param name="fileName">Relative or absolute file path. Null will pick the <see cref="GameBox.FileName"/> value from <see cref="GBX"/> object instead.</param>
    /// <param name="remap">What to remap the newest node IDs to. Used for older games.</param>
    /// <param name="logger">Logger.</param>
    /// <param name="asyncAction">Specialized executions during asynchronous writing.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    public async Task SaveAsync(string? fileName = default,
                                IDRemap remap = default,
                                ILogger? logger = null,
                                GameBoxAsyncWriteAction? asyncAction = null,
                                CancellationToken cancellationToken = default)
    {
        var gbx = GetGbx();

        if (gbx is null)
        {
            await ToGbx(nonGeneric: true).SaveAsync(fileName, remap, logger, asyncAction, cancellationToken);
            return;
        }

        await gbx.SaveAsync(fileName, remap, logger, asyncAction, cancellationToken);
    }

    /// <summary>
    /// Saves the serialized node to a stream in a GBX form.
    /// </summary>
    /// <param name="stream">Any kind of stream that supports writing.</param>
    /// <param name="remap">What to remap the newest node IDs to. Used for older games.</param>
    /// <param name="logger">Logger.</param>
    /// <param name="asyncAction">Specialized executions during asynchronous writing.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    public async Task SaveAsync(Stream stream,
                                IDRemap remap = default,
                                ILogger? logger = null,
                                GameBoxAsyncWriteAction? asyncAction = null,
                                CancellationToken cancellationToken = default)
    {
        var gbx = GetGbx();

        if (gbx is null)
        {
            await ToGbx(nonGeneric: true).SaveAsync(stream, remap, logger, asyncAction, cancellationToken);
            return;
        }

        await gbx.SaveAsync(stream, remap, logger, asyncAction, cancellationToken);
    }

    public static uint RemapToLatest(uint id)
    {
        while (NodeManager.TryGetMapping(id, out var newId))
        {
            id = newId;
        }

        return id;
    }
}
