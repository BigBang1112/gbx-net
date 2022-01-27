using System.Diagnostics;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;

namespace GBX.NET;

/// <summary>
/// The skeleton of the Gbx object and representation of the <see cref="CMwNod"/>.
/// </summary>
/// <remarks>You shouldn't inherit this class unless <see cref="CMwNod"/> cannot be inherited instead, at all costs.</remarks>
public abstract class Node
{
    private uint? id;
    private ChunkSet? chunks;

    // Should be removed for causing recursive behaviour, problematic writing etc.
    // Responsibility should be thrown on readers/writers
    [IgnoreDataMember]
    public GameBox? GBX { get; internal set; }

    public uint Id => GetStoredId();

    public ChunkSet Chunks
    {
        get
        {
            chunks ??= new ChunkSet(this); // Maybe improve this
            return chunks;
        }
    }

    protected Node()
    {

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

    protected Node? GetNodeFromRefTable(Node? nodeAtTheMoment, int? nodeIndex)
    {
        if (nodeAtTheMoment is not null || nodeIndex is null)
            return nodeAtTheMoment;

        if (GBX is null)
            return nodeAtTheMoment;

        var refTable = GBX.RefTable;

        if (refTable is null)
            return nodeAtTheMoment;

        var fileName = GBX.FileName;

        return refTable.GetNode(nodeAtTheMoment, nodeIndex, fileName);
    }

    /// <exception cref="NodeNotImplementedException">Auxiliary node is not implemented and is not parseable.</exception>
    /// <exception cref="ChunkReadNotImplementedException">Chunk does not support reading.</exception>
    /// <exception cref="IgnoredUnskippableChunkException">Chunk is known but its content is unknown to read.</exception>
    internal static T?[] ParseArray<T>(GameBoxReader r, IProgress<GameBoxReadProgress>? progress, ILogger? logger) where T : Node
    {
        var count = r.ReadInt32();
        var array = new T?[count];

        for (var i = 0; i < count; i++)
            array[i] = Parse<T>(r, classId: null, progress, logger);

        return array;
    }

    /// <exception cref="NodeNotImplementedException">Auxiliary node is not implemented and is not parseable.</exception>
    /// <exception cref="ChunkReadNotImplementedException">Chunk does not support reading.</exception>
    /// <exception cref="IgnoredUnskippableChunkException">Chunk is known but its content is unknown to read.</exception>
    internal static IEnumerable<T?> ParseEnumerable<T>(GameBoxReader r, int count, IProgress<GameBoxReadProgress>? progress, ILogger? logger) where T : Node
    {
        for (var i = 0; i < count; i++)
            yield return Parse<T>(r, classId: null, progress, logger);
    }

    /// <exception cref="NodeNotImplementedException">Auxiliary node is not implemented and is not parseable.</exception>
    /// <exception cref="ChunkReadNotImplementedException">Chunk does not support reading.</exception>
    /// <exception cref="IgnoredUnskippableChunkException">Chunk is known but its content is unknown to read.</exception>
    internal static IEnumerable<T?> ParseEnumerable<T>(GameBoxReader r, IProgress<GameBoxReadProgress>? progress, ILogger? logger) where T : Node
    {
        return ParseEnumerable<T>(r, count: r.ReadInt32(), progress, logger);
    }

    /// <exception cref="NodeNotImplementedException">Auxiliary node is not implemented and is not parseable.</exception>
    /// <exception cref="ChunkReadNotImplementedException">Chunk does not support reading.</exception>
    /// <exception cref="IgnoredUnskippableChunkException">Chunk is known but its content is unknown to read.</exception>
    internal static IList<T?> ParseList<T>(GameBoxReader r, IProgress<GameBoxReadProgress>? progress, ILogger? logger) where T : Node
    {
        var count = r.ReadInt32();
        return ParseEnumerable<T>(r, count, progress, logger).ToList(count);
    }

    internal static T? Parse<T>(GameBoxReader r, uint? classId, IProgress<GameBoxReadProgress>? progress, ILogger? logger) where T : Node
    {
        return Parse(r, classId, progress, logger) as T;
    }

    /// <exception cref="NodeNotImplementedException">Auxiliary node is not implemented and is not parseable.</exception>
    /// <exception cref="ChunkReadNotImplementedException">Chunk does not support reading.</exception>
    /// <exception cref="IgnoredUnskippableChunkException">Chunk is known but its content is unknown to read.</exception>
    internal static Node? Parse(GameBoxReader r, uint? classId, IProgress<GameBoxReadProgress>? progress, ILogger? logger)
    {
        GetNodeInfoFromClassId(r, classId, out Node? node, out Type nodeType);

        if (node is null)
        {
            return null;
        }

        Parse(node, nodeType, r, progress, logger);

        return node;
    }

    private static void GetNodeInfoFromClassId(GameBoxReader r, uint? classId, out Node? node, out Type nodeType)
    {
        if (classId is null)
        {
            classId = r.ReadUInt32();
        }

        if (classId == uint.MaxValue)
        {
            node = null;
            nodeType = null!;
            return;
        }

        classId = Remap(classId.Value);

        var id = classId.Value;

        nodeType = NodeCacheManager.GetClassTypeById(id)!;

        if (nodeType is null)
        {
            throw new NodeNotImplementedException(id);
        }

        var constructor = NodeCacheManager.GetClassConstructor(id);

        node = constructor();
    }

    /// <exception cref="NodeNotImplementedException">Auxiliary node is not implemented and is not parseable.</exception>
    /// <exception cref="ChunkReadNotImplementedException">Chunk does not support reading.</exception>
    /// <exception cref="IgnoredUnskippableChunkException">Chunk is known but its content is unknown to read.</exception>
    internal static void Parse(Node node, Type nodeType, GameBoxReader r, IProgress<GameBoxReadProgress>? progress, ILogger? logger)
    {
        var stopwatch = Stopwatch.StartNew();

        node.GBX = r.Body?.GBX;

        using var scope = logger?.BeginScope("{name}", nodeType.Name);

        var previousChunkId = default(uint?);

        while (IterateChunks(node, nodeType, r, progress, logger, ref previousChunkId))
        {
            // Iterates through chunks until false is returned
        }

        stopwatch.Stop();

        logger?.LogNodeComplete(time: stopwatch.Elapsed.TotalMilliseconds);
    }

    /// <exception cref="NodeNotImplementedException">Auxiliary node is not implemented and is not parseable.</exception>
    /// <exception cref="ChunkReadNotImplementedException">Chunk does not support reading.</exception>
    /// <exception cref="IgnoredUnskippableChunkException">Chunk is known but its content is unknown to read.</exception>
    internal static async Task<Node?> ParseAsync(GameBoxReader r, uint? classId, ILogger? logger, GameBoxAsyncAction? asyncAction, CancellationToken cancellationToken = default)
    {
        GetNodeInfoFromClassId(r, classId, out Node? node, out Type nodeType);

        if (node is null)
        {
            return null;
        }

        await ParseAsync(node, nodeType, r, logger, asyncAction, cancellationToken);

        return node;
    }

    internal static async Task ParseAsync(Node node, Type nodeType, GameBoxReader r, ILogger? logger, GameBoxAsyncAction? asyncAction, CancellationToken cancellationToken = default)
    {
        var stopwatch = Stopwatch.StartNew();

        node.GBX = r.Body?.GBX;

        using var scope = logger?.BeginScope("{name} (async)", nodeType.Name);

        cancellationToken.ThrowIfCancellationRequested();

        var previousChunkId = default(uint?);

        while (true)
        {
            // Iterates through chunks until false is returned
            (var moreChunks, previousChunkId) = await IterateChunksAsync(node, nodeType, r, logger, previousChunkId, cancellationToken);

            if (!moreChunks)
            {
                break;
            }

            if (asyncAction is not null && asyncAction.AfterChunkIteration is not null)
            {
                await asyncAction.AfterChunkIteration();
            }

            cancellationToken.ThrowIfCancellationRequested();
        }

        stopwatch.Stop();

        logger?.LogNodeComplete(time: stopwatch.Elapsed.TotalMilliseconds);
    }

    private static bool IterateChunks(Node node, Type nodeType, GameBoxReader r, IProgress<GameBoxReadProgress>? progress, ILogger? logger, ref uint? previousChunkId)
    {
        var stream = r.BaseStream;
        var canSeek = stream.CanSeek;

        if (canSeek && stream.Position + 4 > stream.Length)
        {
            logger?.LogUnexpectedEndOfStream(stream.Position, stream.Length);
            var bytes = r.ReadBytes((int)(stream.Length - stream.Position));
            return false;
        }

        var chunkId = r.ReadUInt32();

        if (chunkId == 0xFACADE01) // no more chunks
        {
            return false;
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

        progress?.Report(new GameBoxReadProgress(GameBoxReadProgressStage.Body, (float)stream.Position / stream.Length, node.GBX, chunk));

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
        chunk.Node = node; //
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

        if (GameBox.Debug)
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
                unknown.WriteBytes(unknownData);
            }
        }
        catch (EndOfStreamException) // May not be needed
        {
            logger?.LogDebug("Unexpected end of the stream while reading the chunk.");
        }

        if (GameBox.Debug)
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

    private static async Task<(bool moreChunks, uint? previousChunkId)> IterateChunksAsync(
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
            return (false, previousChunkId);
        }

        var chunkId = r.ReadUInt32();

        if (chunkId == 0xFACADE01) // no more chunks
        {
            return (false, previousChunkId);
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
                    return (false, previousChunkId);
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

        return (true, chunkId);
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
        chunk.Node = node; //
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

        if (GameBox.Debug)
        {
            streamPos = r.BaseStream.Position;
        }

        try
        {
            if (autoReadWriteChunkAttribute is null)
            {
                if (NodeCacheManager.AsyncChunksById.ContainsKey(chunkId))
                {
                    if (NodeCacheManager.ReadWriteAsyncChunksById.ContainsKey(chunkId) || NodeCacheManager.ReadAsyncChunksById.ContainsKey(chunkId))
                    {
                        await ((IReadableWritableChunk)chunk).ReadWriteAsync(node, gbxrw, cancellationToken);
                    }
                }
                else
                {
                    ((IReadableWritableChunk)chunk).ReadWrite(node, gbxrw);
                }
            }
            else
            {
                var unknown = new GameBoxWriter(chunk.Unknown, logger: logger);
                var unknownData = r.ReadUntilFacade().ToArray();
                unknown.WriteBytes(unknownData);
            }
        }
        catch (EndOfStreamException) // May not be needed
        {
            logger?.LogDebug("Unexpected end of the stream while reading the chunk.");
        }

        if (GameBox.Debug)
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

        chunk.Debugger ??= new();
        chunk.Debugger.RawData = rawData;
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

        _ = await stream.ReadAsync(rawData, 0, chunkLength, cancellationToken);

        chunk.Debugger ??= new();
        chunk.Debugger.RawData = rawData;
    }

    private static void BeforeChunkParseException(GameBoxReader r)
    {
        if (GameBox.Debug && r.BaseStream.CanSeek) // I don't get it
        {
            // Read the rest of the body

            var streamPos = r.BaseStream.Position;
            var uncontrollableData = r.ReadToEnd();
            r.BaseStream.Position = streamPos;
        }
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
            ? CreateKnownSkippableChunk(node, chunkId, chunkData)
            : CreateUnknownSkippableChunk(node, nodeType, chunkId, chunkData, logger);
    }

    private static Chunk CreateUnknownSkippableChunk(Node node,
                                                     Type nodeType,
                                                     uint chunkId,
                                                     byte[] chunkData,
                                                     ILogger? logger)
    {
        logger?.LogChunkUnknownSkippable(chunkId.ToString("X"));

        var chunk = (Chunk)Activator.CreateInstance(typeof(SkippableChunk<>).MakeGenericType(nodeType), new object?[] { node, chunkData, chunkId })!;

        if (GameBox.Debug)
        {
            chunk.Debugger ??= new();
            chunk.Debugger.RawData = chunkData;
        }

        return chunk;
    }

    private static Chunk CreateKnownSkippableChunk(Node node, uint chunkId, byte[] chunkData)
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
        chunk.Node = node; //

        var skippableChunk = (ISkippableChunk)chunk;
        skippableChunk.Data = chunkData;

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
                skippableChunk.Discover();
            }
        }

        if (GameBox.Debug)
        {
            chunk.Debugger ??= new();
            chunk.Debugger!.RawData = chunkData;
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

    public void Read(GameBoxReader r)
    {
        throw new NotImplementedException($"Node doesn't support Read.");
    }

    public void Write(GameBoxWriter w, IDRemap remap = default, ILogger? logger = null)
    {
        var stopwatch = Stopwatch.StartNew();

        int counter = 0;

        var type = GetType();
        var writingNotSupported = type.GetCustomAttribute<WritingNotSupportedAttribute>() != null;
        if (writingNotSupported)
            throw new NotSupportedException($"Writing of {type.Name} is not supported.");

        var className = logger?.IsEnabled(LogLevel.Debug) == true ? ToString() : null;

        foreach (Chunk chunk in Chunks)
        {
            counter++;

            if (logger?.IsEnabled(LogLevel.Debug) == true)
            {
                logger?.LogDebug("[{className}] 0x{chunkId} ({progressPercentage})",
                    className,
                    chunk.Id.ToString("X8"),
                    ((float)counter / Chunks.Count).ToString("0.00%"));
            }

            chunk.Node = this;
            chunk.Unknown.Position = 0;

            if (chunk is ILookbackable l)
            {
                l.IdWritten = false;
                l.IdStrings.Clear();
            }

            using var ms = new MemoryStream();
            using var msW = new GameBoxWriter(ms, w.Body, w.Lookbackable, logger);
            var rw = new GameBoxReaderWriter(msW);

            try
            {
                if (chunk is ISkippableChunk s && !s.Discovered)
                    s.Write(msW);
                else if (!IsIgnorableChunk(chunk))
                    ((IReadableWritableChunk)chunk).ReadWrite(this, rw);
                else if (chunk is ISkippableChunk sIgnored)
                    msW.WriteBytes(sIgnored.Data);
                else
                    msW.WriteBytes(chunk.Unknown.ToArray());

                w.Write(Chunk.Remap(chunk.Id, remap));

                if (chunk is ISkippableChunk)
                {
                    w.Write(0x534B4950);
                    w.Write((int)ms.Length);
                }

                w.Write(ms.ToArray(), 0, (int)ms.Length);
            }
            catch (NotImplementedException e)
            {
                if (chunk is not ISkippableChunk)
                    throw e; // Unskippable chunk must have a Write implementation
                
                Debug.WriteLine(e.Message);
                Debug.WriteLine("Ignoring the skippable chunk from writing.");
            }
        }

        w.Write(0xFACADE01);

        stopwatch.Stop();

        logger?.LogDebug("[{className}] DONE! ({time}ms)", className, stopwatch.Elapsed.TotalMilliseconds);
    }

    private static bool IsIgnorableChunk(Chunk chunk)
    {
        var chunkType = chunk.GetType();

        return Attribute.IsDefined(chunkType, typeof(AutoReadWriteChunkAttribute))
            || Attribute.IsDefined(chunkType, typeof(IgnoreChunkAttribute));
    }

    public T? GetChunk<T>() where T : Chunk
    {
        return Chunks.Get<T>();
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

    public void DiscoverChunk<T>() where T : ISkippableChunk
    {
        Chunks.Discover<T>();
    }

    public void DiscoverChunks<T1, T2>() where T1 : ISkippableChunk where T2 : ISkippableChunk
    {
        Chunks.Discover<T1, T2>();
    }

    public void DiscoverChunks<T1, T2, T3>() where T1 : ISkippableChunk where T2 : ISkippableChunk where T3 : ISkippableChunk
    {
        Chunks.Discover<T1, T2, T3>();
    }

    public void DiscoverChunks<T1, T2, T3, T4>()
        where T1 : ISkippableChunk
        where T2 : ISkippableChunk
        where T3 : ISkippableChunk
        where T4 : ISkippableChunk
    {
        Chunks.Discover<T1, T2, T3, T4>();
    }

    public void DiscoverChunks<T1, T2, T3, T4, T5>()
        where T1 : ISkippableChunk
        where T2 : ISkippableChunk
        where T3 : ISkippableChunk
        where T4 : ISkippableChunk
        where T5 : ISkippableChunk
    {
        Chunks.Discover<T1, T2, T3, T4, T5>();
    }

    public void DiscoverChunks<T1, T2, T3, T4, T5, T6>()
        where T1 : ISkippableChunk
        where T2 : ISkippableChunk
        where T3 : ISkippableChunk
        where T4 : ISkippableChunk
        where T5 : ISkippableChunk
        where T6 : ISkippableChunk
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
    /// Makes a <see cref="GameBox"/> from this node. NOTE: Non-generic <see cref="GameBox"/> doesn't have a Save method.
    /// </summary>
    /// <param name="headerInfo"></param>
    /// <returns></returns>
    public GameBox ToGBX(GameBoxHeaderInfo headerInfo)
    {
        return (GameBox)Activator.CreateInstance(typeof(GameBox<>).MakeGenericType(GetType()), this, headerInfo)!;
    }

    /// <summary>
    /// Makes a <see cref="GameBox"/> from this node. You can explicitly cast it to <see cref="GameBox{T}"/> depending on the <see cref="Node"/>. NOTE: Non-generic <see cref="GameBox"/> doesn't have a Save method.
    /// </summary>
    /// <returns></returns>
    public GameBox ToGBX()
    {
        return (GameBox)Activator.CreateInstance(typeof(GameBox<>).MakeGenericType(GetType()), this)!;
    }

    private void Save(Type[] types, object?[] parameters)
    {
        var type = GetType();
        var gbxType = GBX?.GetType();
        var gbxOfType = typeof(GameBox<>).MakeGenericType(type);

        GameBox gbx;

        if (GBX is not null && gbxOfType == gbxType)
        {
            gbx = GBX;
        }
        else
        {
            gbx = (GameBox)Activator.CreateInstance(gbxOfType, this, null)!;
            gbx.Body!.IsParsed = true;
        }

        _ = gbxOfType.GetMethod("Save", types)!.Invoke(gbx, parameters);
    }

    /// <summary>
    /// Saves the serialized node on a disk in a GBX form.
    /// </summary>
    /// <param name="fileName">Relative or absolute file path. Null will pick the <see cref="GameBox.FileName"/> value from <see cref="GBX"/> object instead.</param>
    /// <param name="remap">What to remap the newest node IDs to. Used for older games.</param>
    /// <param name="logger">Logger.</param>
    /// <exception cref="NotSupportedException"/>
    public void Save(string? fileName, IDRemap remap = default, ILogger? logger = null)
    {
        Save(new Type[] { typeof(string), typeof(IDRemap), typeof(ILogger) }, new object?[] { fileName, remap, logger });
    }

    /// <summary>
    /// Saves the serialized node to a stream in a GBX form.
    /// </summary>
    /// <param name="stream">Any kind of stream that supports writing.</param>
    /// <param name="remap">What to remap the newest node IDs to. Used for older games.</param>
    /// <param name="logger">Logger.</param>
    /// <exception cref="NotSupportedException"/>
    public void Save(Stream stream, IDRemap remap = default, ILogger? logger = null)
    {
        Save(new Type[] { typeof(Stream), typeof(IDRemap), typeof(ILogger) }, new object?[] { stream, remap, logger });
    }

    internal static uint Remap(uint id)
    {
        if (NodeCacheManager.Mappings.TryGetValue(id, out uint newerClassID))
            return newerClassID;
        return id;
    }
}
