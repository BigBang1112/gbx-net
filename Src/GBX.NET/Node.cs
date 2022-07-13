using System.Diagnostics;

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

    public uint Id => GetStoredId();

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
        return NodeCacheManager.GetClassIdByType(GetType()) ?? throw new ThisShouldNotHappenException();
    }

    /// <summary>
    /// Returns the name of the class formatted as <c>[engine]::[class]</c>.
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        var type = GetType();

        if (NodeCacheManager.Names.TryGetValue(Id, out string? name))
        {
            return name;
        }

        return type.FullName?.Substring("GBX.NET.Engines".Length + 1).Replace(".", "::") ?? string.Empty;
    }

    protected internal Node? GetNodeFromRefTable(Node? nodeAtTheMoment, GameBoxRefTable.File? nodeFile)
    {
        if (nodeAtTheMoment is not null || nodeFile is null || gbxForRefTable is null)
        {
            return nodeAtTheMoment;
        }

        var refTable = gbxForRefTable.RefTable;

        if (refTable is null)
        {
            return nodeAtTheMoment;
        }

        var fileName = gbxForRefTable.PakFileName ?? gbxForRefTable.FileName;

        return refTable.GetNode(nodeAtTheMoment, nodeFile, fileName, gbxForRefTable?.ExternalGameData);
    }

    internal static T? Parse<T>(GameBoxReader r, uint? classId, IProgress<GameBoxReadProgress>? progress, ILogger? logger, bool ignoreZeroIdChunk = false) where T : Node
    {
        return Parse(r, classId, progress, logger, ignoreZeroIdChunk) as T;
    }

    /// <exception cref="NodeNotImplementedException">Auxiliary node is not implemented and is not parseable.</exception>
    /// <exception cref="ChunkReadNotImplementedException">Chunk does not support reading.</exception>
    /// <exception cref="IgnoredUnskippableChunkException">Chunk is known but its content is unknown to read.</exception>
    internal static Node? Parse(GameBoxReader r,
                                uint? classId,
                                IProgress<GameBoxReadProgress>? progress,
                                ILogger? logger,
                                bool ignoreZeroIdChunk = false)
    {
        GetNodeInfoFromClassId(r, classId, out Node? node, out Type nodeType);

        if (node is null)
        {
            return null;
        }

        Parse(node, nodeType, r, progress, logger, ignoreZeroIdChunk);

        return node;
    }

    private static uint GetNodeInfoFromClassId(GameBoxReader r, uint? classId, out Node? node, out Type nodeType)
    {
        if (classId is null)
        {
            classId = r.ReadUInt32();
        }

        if (classId == uint.MaxValue)
        {
            node = null;
            nodeType = null!;

            return classId.Value;
        }

        classId = RemapToLatest(classId.Value);

        var id = classId.Value;

        nodeType = NodeCacheManager.GetClassTypeById(id)!;

        if (nodeType is null)
        {
            throw new NodeNotImplementedException(id);
        }

        var constructor = NodeCacheManager.GetClassConstructor(id);

        node = constructor();

        return classId.Value;
    }

    /// <exception cref="NodeNotImplementedException">Auxiliary node is not implemented and is not parseable.</exception>
    /// <exception cref="ChunkReadNotImplementedException">Chunk does not support reading.</exception>
    /// <exception cref="IgnoredUnskippableChunkException">Chunk is known but its content is unknown to read.</exception>
    internal static void Parse(Node node,
                               Type nodeType,
                               GameBoxReader r,
                               IProgress<GameBoxReadProgress>? progress,
                               ILogger? logger,
                               bool ignoreZeroIdChunk = false)
    {
        node.gbxForRefTable = r.Settings.Gbx;
        
        var stopwatch = Stopwatch.StartNew();

        using var scope = logger?.BeginScope("{name}", nodeType.Name);

        var previousChunkId = default(uint?);

        if (r.BaseStream is IXorTrickStream cryptedStream)
        {
            var baseType = nodeType.BaseType ?? throw new ThisShouldNotHappenException();
                
            var parentClassId = NodeCacheManager.GetClassIdByType(baseType) ?? throw new ThisShouldNotHappenException();

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

        while (IterateChunks(node, nodeType, r, progress, logger, ref previousChunkId, ignoreZeroIdChunk))
        {
            // Iterates through chunks until false is returned
        }

        stopwatch.Stop();

        logger?.LogNodeComplete(time: stopwatch.Elapsed.TotalMilliseconds);
    }

    /// <exception cref="NodeNotImplementedException">Auxiliary node is not implemented and is not parseable.</exception>
    /// <exception cref="ChunkReadNotImplementedException">Chunk does not support reading.</exception>
    /// <exception cref="IgnoredUnskippableChunkException">Chunk is known but its content is unknown to read.</exception>
    internal static async Task<Node?> ParseAsync(GameBoxReader r, uint? classId, ILogger? logger, CancellationToken cancellationToken = default)
    {
        var realClassId = GetNodeInfoFromClassId(r, classId, out Node? node, out Type nodeType);

        if (node is null)
        {
            return null;
        }

        r.Settings.AsyncAction?.OnReadNode?.Invoke(r, realClassId);

        await ParseAsync(node, nodeType, r, logger, cancellationToken);

        return node;
    }

    internal static async Task ParseAsync(Node node, Type nodeType, GameBoxReader r, ILogger? logger, CancellationToken cancellationToken = default)
    {
        var stopwatch = Stopwatch.StartNew();

        using var scope = logger?.BeginScope("{name} (async)", nodeType.Name);

        cancellationToken.ThrowIfCancellationRequested();

        var previousChunkId = default(uint?);

        while (true)
        {
            // Iterates through chunks until false is returned
            (var moreChunks, previousChunkId, var chunk) = await IterateChunksAsync(node, nodeType, r, logger, previousChunkId, cancellationToken);

            if (!moreChunks)
            {
                break;
            }

            var asyncAction = r.Settings.AsyncAction;

            if (asyncAction is not null && asyncAction.AfterChunkIteration is not null)
            {
                await asyncAction.AfterChunkIteration(node, chunk);
            }

            cancellationToken.ThrowIfCancellationRequested();
        }

        stopwatch.Stop();

        logger?.LogNodeComplete(time: stopwatch.Elapsed.TotalMilliseconds);
    }

    private static bool IterateChunks(Node node,
                                      Type nodeType,
                                      GameBoxReader r,
                                      IProgress<GameBoxReadProgress>? progress,
                                      ILogger? logger,
                                      ref uint? previousChunkId,
                                      bool ignoreZeroIdChunk = false)
    {
        var stream = r.BaseStream;
        var canSeek = stream.CanSeek;

        if (canSeek && stream.Position + 4 > stream.Length)
        {
            logger?.LogUnexpectedEndOfStream(stream.Position, stream.Length);
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

        LogChunkProgress(logger, stream, originalChunkId);

        var chunkId = Chunk.Remap(originalChunkId);

        var chunkClass = NodeCacheManager.GetChunkTypeById(nodeType, chunkId);

        var reflected = chunkClass is not null;
        var skippable = reflected && NodeCacheManager.SkippableChunks.ContainsKey(chunkClass!);

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

            chunk = ReadSkippableChunk(node, nodeType, r, chunkId, reflected, logger);
        }
        else // Known or unskippable chunk
        {
            chunk = ReadUnskippableChunk(node, r, logger, chunkId);
        }

        node.Chunks.Add(chunk);

        progress?.Report(new GameBoxReadProgress(GameBoxReadProgressStage.Body, (float)stream.Position / stream.Length, gbx: null, chunk));

        previousChunkId = chunkId;

        return true;
    }

    private static Chunk ReadUnskippableChunk(Node node, GameBoxReader r, ILogger? logger, uint chunkId)
    {
        if (!NodeCacheManager.ChunkConstructors.TryGetValue(chunkId, out Func<Chunk>? constructor))
        {
            throw new ThisShouldNotHappenException();
        }

        var chunk = constructor();
        chunk.OnLoad(); // The chunk is immediately activated, but is not parsed at this state

        var gbxrw = new GameBoxReaderWriter(r);

        TryGetChunkAttributes(chunkId, out _,
            out IgnoreChunkAttribute? ignoreChunkAttribute,
            out AutoReadWriteChunkAttribute? autoReadWriteChunkAttribute);

        if (ignoreChunkAttribute is not null)
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
            if (autoReadWriteChunkAttribute is null)
            {
                // Why does it have to be IChunk?
                ((IReadableWritableChunk)chunk).ReadWrite(node, gbxrw);
            }
            else
            {
                var unknown = new GameBoxWriter(chunk.Unknown, logger: logger);
                var unknownData = r.ReadUntilFacade().ToArray();
                unknown.Write(unknownData);
            }
        }
        catch (EndOfStreamException) // May not be needed
        {
            logger?.LogDebug("Unexpected end of the stream while reading the chunk.");
        }

        if (GameBox.SeekForRawChunkData)
        {
            SetChunkRawData(r.BaseStream, chunk, streamPos);
        }

        return chunk;
    }

    private static void TryGetChunkAttributes(uint chunkId,
                                              out ChunkAttribute? chunkAttribute,
                                              out IgnoreChunkAttribute? ignoreChunkAttribute,
                                              out AutoReadWriteChunkAttribute? autoReadWriteChunkAttribute)
    {
        if (!NodeCacheManager.ChunkAttributesById.TryGetValue(chunkId, out IEnumerable<Attribute>? attributes))
        {
            throw new ThisShouldNotHappenException();
        }

        chunkAttribute = null;
        ignoreChunkAttribute = null;
        autoReadWriteChunkAttribute = null;

        foreach (var att in attributes)
        {
            switch (att)
            {
                case ChunkAttribute chunkAtt:
                    chunkAttribute = chunkAtt;
                    break;
                case IgnoreChunkAttribute ignoreChunkAtt:
                    ignoreChunkAttribute = ignoreChunkAtt;
                    break;
                case AutoReadWriteChunkAttribute autoReadWriteChunkAtt:
                    autoReadWriteChunkAttribute = autoReadWriteChunkAtt;
                    break;
            }
        }
    }

    private static async Task<(bool moreChunks, uint? previousChunkId, Chunk? chunk)> IterateChunksAsync(
        Node node,
        Type nodeType,
        GameBoxReader r,
        ILogger? logger,
        uint? previousChunkId,
        CancellationToken cancellationToken)
    {
        var stream = r.BaseStream;
        var canSeek = stream.CanSeek;

        if (canSeek && stream.Position + 4 > stream.Length)
        {
            logger?.LogUnexpectedEndOfStream(stream.Position, stream.Length);
            var bytes = r.ReadBytes((int)(stream.Length - stream.Position));
            return (false, previousChunkId, null);
        }

        var chunkId = r.ReadUInt32();

        if (chunkId == 0xFACADE01) // no more chunks
        {
            return (false, previousChunkId, null);
        }

        LogChunkProgress(logger, stream, chunkId);

        chunkId = Chunk.Remap(chunkId);

        var chunkClass = NodeCacheManager.GetChunkTypeById(nodeType, chunkId);

        var reflected = chunkClass is not null;
        var skippable = reflected && NodeCacheManager.SkippableChunks.ContainsKey(chunkClass!);

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

            chunk = ReadSkippableChunk(node, nodeType, r, chunkId, reflected, logger);
        }
        else // Known or unskippable chunk
        {
            chunk = await ReadUnskippableChunkAsync(node, r, logger, chunkId, cancellationToken);
        }

        node.Chunks.Add(chunk);

        return (true, chunkId, chunk);
    }

    private static async Task<Chunk> ReadUnskippableChunkAsync(
        Node node,
        GameBoxReader r,
        ILogger? logger,
        uint chunkId,
        CancellationToken cancellationToken)
    {
        if (!NodeCacheManager.ChunkConstructors.TryGetValue(chunkId, out Func<Chunk>? constructor))
        {
            throw new ThisShouldNotHappenException();
        }

        var chunk = constructor();
        chunk.OnLoad(); // The chunk is immediately activated, but may not be yet parsed at this state

        var gbxrw = new GameBoxReaderWriter(r);

        TryGetChunkAttributes(chunkId, out _,
            out IgnoreChunkAttribute? ignoreChunkAttribute,
            out AutoReadWriteChunkAttribute? autoReadWriteChunkAttribute);

        if (ignoreChunkAttribute is not null)
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
            if (autoReadWriteChunkAttribute is null)
            {
                await WriteChunkWithReadWriteAsync(node, (IReadableWritableChunk)chunk, gbxrw, cancellationToken);
            }
            else
            {
                var unknown = new GameBoxWriter(chunk.Unknown, logger: logger);
                var unknownData = r.ReadUntilFacade().ToArray();
                unknown.Write(unknownData);
            }
        }
        catch (EndOfStreamException) // May not be needed
        {
            logger?.LogDebug("Unexpected end of the stream while reading the chunk.");
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
                                            Type nodeType,
                                            GameBoxReader r,
                                            uint chunkId,
                                            bool reflected,
                                            ILogger? logger)
    {
        var chunkData = r.ReadBytes();

        return reflected
            ? CreateKnownSkippableChunk(node, chunkId, chunkData, r.Settings, logger)
            : CreateUnknownSkippableChunk(node, nodeType, chunkId, chunkData, logger);
    }

    private static Chunk CreateUnknownSkippableChunk(Node node,
                                                     Type nodeType,
                                                     uint chunkId,
                                                     byte[] chunkData,
                                                     ILogger? logger)
    {
        logger?.LogChunkUnknownSkippable(chunkId.ToString("X8"), chunkData.Length);

        var chunk = (Chunk)Activator.CreateInstance(typeof(SkippableChunk<>).MakeGenericType(nodeType), new object?[] { node, chunkData, chunkId })!;

        if (GameBox.SeekForRawChunkData)
        {
            chunk.RawData = chunkData;
        }

        return chunk;
    }

    private static Chunk CreateKnownSkippableChunk(Node node, uint chunkId, byte[] chunkData, GameBoxReaderSettings readerSettings, ILogger? logger)
    {
        TryGetChunkAttributes(chunkId,
            out ChunkAttribute? chunkAttribute,
            out IgnoreChunkAttribute? ignoreChunkAttribute,
            out AutoReadWriteChunkAttribute? autoReadWriteChunkAttribute);

        if (chunkAttribute is null)
        {
            throw new ThisShouldNotHappenException();
        }

        if (!NodeCacheManager.ChunkConstructors.TryGetValue(chunkId, out Func<Chunk>? constructor))
        {
            throw new ThisShouldNotHappenException();
        }

        var chunk = constructor();

        var skippableChunk = (ISkippableChunk)chunk;
        skippableChunk.Data = chunkData;
        skippableChunk.Gbx = readerSettings.Gbx;
        skippableChunk.Node = node; //

        if (chunkData.Length == 0)
        {
            skippableChunk.Discovered = true;
        }

        // If the chunk should be taken as "activated" (not necessarily to be immediately parsed)
        if (ignoreChunkAttribute is null)
        {
            chunk.OnLoad();

            if (chunkAttribute.ProcessSync)
            {
                using var ms = new MemoryStream(chunkData);
                using var r = new GameBoxReader(ms, readerSettings, logger);
                var rw = new GameBoxReaderWriter(r);

                skippableChunk.ReadWrite(node, rw);
                skippableChunk.Discovered = true;
                
                if (ms.Position != ms.Length)
                {
                    logger?.LogWarning("Skippable chunk 0x{chunkId} has {chunkSize} bytes left.", chunkId.ToString("X8"), ms.Length - ms.Position);
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

    internal void Write(GameBoxWriter w, ILogger? logger)
    {
        var stopwatch = Stopwatch.StartNew();

        var nodeType = GetType();

        ThrowIfWritingIsNotSupported(nodeType);

        using var scope = logger?.BeginScope("{name}", nodeType.Name);

        var counter = 0;

        foreach (IReadableWritableChunk chunk in Chunks)
        {
            counter++;
            PrepareToWriteChunk(logger, counter, chunk);
            WriteChunk(chunk, w, logger);
        }

        WriteFacade(w);

        logger?.LogNodeComplete(stopwatch.Elapsed.TotalMilliseconds);
    }

    public async Task WriteAsync(GameBoxWriter w, ILogger? logger = null, CancellationToken cancellationToken = default)
    {
        var stopwatch = Stopwatch.StartNew();

        var nodeType = GetType();

        ThrowIfWritingIsNotSupported(nodeType);

        cancellationToken.ThrowIfCancellationRequested();

        using var scope = logger?.BeginScope("{name}", nodeType.Name);

        var counter = 0;

        foreach (IReadableWritableChunk chunk in Chunks)
        {
            counter++;
            PrepareToWriteChunk(logger, counter, chunk);
            await WriteChunkAsync(chunk, w, logger, cancellationToken);

            var asyncAction = w.Settings.AsyncAction;

            if (asyncAction is not null && asyncAction.AfterChunkIteration is not null)
            {
                await asyncAction.AfterChunkIteration(this, chunk as Chunk);
            }

            cancellationToken.ThrowIfCancellationRequested();
        }

        WriteFacade(w);

        logger?.LogNodeComplete(stopwatch.Elapsed.TotalMilliseconds);
    }

    private static void WriteFacade(GameBoxWriter w)
    {
        w.Write(0xFACADE01);
    }

    private static void ThrowIfWritingIsNotSupported(Type nodeType)
    {
        if (!NodeCacheManager.ClassAttributesByType.TryGetValue(nodeType, out var classAttributes))
        {
            return;
        }

        foreach (var attribute in classAttributes)
        {
            if (attribute is WritingNotSupportedAttribute)
            {
                throw new NotSupportedException($"Writing of {nodeType.Name} is not supported.");
            }
        }
    }

    private void WriteChunk(IReadableWritableChunk chunk, GameBoxWriter w, ILogger? logger)
    {
        using var ms = new MemoryStream();

        // Writer doesn't have using to not dispose nested IdSubStates. Hopefully this will be fine
        var msW = new GameBoxWriter(ms, w.Settings, logger);
        var rw = new GameBoxReaderWriter(msW);

        w.Write(Chunk.Remap(chunk.Id, w.Settings.Remap));

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

    private async Task WriteChunkAsync(IReadableWritableChunk chunk, GameBoxWriter w, ILogger? logger, CancellationToken cancellationToken)
    {
        using var ms = new MemoryStream();
        var msW = new GameBoxWriter(ms, w.Settings, logger);
        var rw = new GameBoxReaderWriter(msW);

        w.Write(Chunk.Remap(chunk.Id, w.Settings.Remap));

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
        if (IsIgnorableChunk(chunk.GetType()))
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
        if (IsIgnorableChunk(chunk.GetType()))
        {
            await msW.WriteBytesAsync(chunk.Unknown.ToArray(), cancellationToken);
            return;
        }

        await WriteChunkWithReadWriteAsync(chunk, rw, cancellationToken);
    }

    private static async Task WriteChunkWithReadWriteAsync(Node node,
                                                           IReadableWritableChunk chunk,
                                                           GameBoxReaderWriter rw,
                                                           CancellationToken cancellationToken)
    {
        if (NodeCacheManager.AsyncChunksById.ContainsKey(chunk.Id))
        {
            if (NodeCacheManager.ReadWriteAsyncChunksById.ContainsKey(chunk.Id) || NodeCacheManager.ReadAsyncChunksById.ContainsKey(chunk.Id))
            {
                await chunk.ReadWriteAsync(node, rw, cancellationToken);
                return;
            }

            throw new ThisShouldNotHappenException();
        }

        chunk.ReadWrite(node, rw);
    }

    private async Task WriteChunkWithReadWriteAsync(IReadableWritableChunk chunk,
                                                    GameBoxReaderWriter rw,
                                                    CancellationToken cancellationToken)
    {
        await WriteChunkWithReadWriteAsync(this, chunk, rw, cancellationToken);
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

        var type = skippableChunk.GetType();

        if (IsIgnorableChunk(type))
        {
            chunkWriter.Write(skippableChunk.Data);
            return;
        }

        var ownIdState = HasChunkOwnIdState(type);

        if (ownIdState)
        {
            gbx?.ResetIdState();
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

    private static bool HasChunkOwnIdState(Type type)
    {
        if (!NodeCacheManager.ChunkAttributesByType.TryGetValue(type, out var attributes))
        {
            return false;
        }

        foreach (var attribute in attributes)
        {
            if (attribute is ChunkWithOwnIdStateAttribute)
            {
                return true;
            }
        }

        return false;
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

        var type = skippableChunk.GetType();

        if (IsIgnorableChunk(type))
        {
            await chunkWriter.WriteBytesAsync(skippableChunk.Data, cancellationToken);
            return;
        }

        var ownIdState = HasChunkOwnIdState(type);

        if (ownIdState)
        {
            GetGbx()?.ResetIdState();
        }

        try
        {
            await WriteChunkWithReadWriteAsync(skippableChunk, rw, cancellationToken);
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

    private static bool IsIgnorableChunk(Type chunkType)
    {
        if (!NodeCacheManager.ChunkAttributesByType.TryGetValue(chunkType, out IEnumerable<Attribute>? attributes))
        {
            return false;
        }

        foreach (var attribute in attributes)
        {
            if (attribute is AutoReadWriteChunkAttribute || attribute is IgnoreChunkAttribute)
            {
                return true;
            }
        }

        return false;
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

    public bool TryGetChunk<T>(out T? chunk) where T : Chunk
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
    /// Saves the serialized node on a disk in a GBX form.
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
    /// Saves the serialized node to a stream in a GBX form.
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
    /// Saves the serialized node on a disk in a GBX form.
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
        while (NodeCacheManager.Mappings.TryGetValue(id, out var newId))
        {
            id = newId;
        }

        return id;
    }
}
