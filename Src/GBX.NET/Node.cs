using System.Diagnostics;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;

namespace GBX.NET;

public abstract class Node
{
    private ChunkSet? chunks;

    [IgnoreDataMember]
    public GameBox? GBX { get; internal set; }

    public ChunkSet Chunks
    {
        get
        {
            chunks ??= new ChunkSet(this);
            return chunks;
        }
    }

    /// <summary>
    /// Name of the class. The format is <c>Engine::Class</c>.
    /// </summary>
    public string ClassName
    {
        get
        {
            if (NodeCacheManager.TypeWithClassId.TryGetValue(GetType(), out uint id))
                if (NodeCacheManager.Names.TryGetValue(id, out string? name))
                    return name;
            return GetType().FullName?.Substring("GBX.NET.Engines".Length + 1).Replace(".", "::") ?? string.Empty;
        }
    }

    protected Node()
    {

    }

    protected Node(params Chunk[] chunks) : this()
    {
        foreach (var chunk in chunks)
        {
            GetType()
                .GetMethod("CreateChunk")!
                .MakeGenericMethod(chunk.GetType())
                .Invoke(this, Array.Empty<object>());
        }
    }

    // Now only chunks
    internal void SetIDAndChunks()
    {
        chunks = new ChunkSet(this);
    }

    protected CMwNod? GetNodeFromRefTable(CMwNod? nodeAtTheMoment, int? nodeIndex)
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
    internal static T?[] ParseArray<T>(GameBoxReader r, IProgress<GameBoxReadProgress>? progress, ILogger? logger) where T : CMwNod
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
    internal static IEnumerable<T?> ParseEnumerable<T>(GameBoxReader r, int count, IProgress<GameBoxReadProgress>? progress, ILogger? logger) where T : CMwNod
    {
        for (var i = 0; i < count; i++)
            yield return Parse<T>(r, classId: null, progress, logger);
    }

    /// <exception cref="NodeNotImplementedException">Auxiliary node is not implemented and is not parseable.</exception>
    /// <exception cref="ChunkReadNotImplementedException">Chunk does not support reading.</exception>
    /// <exception cref="IgnoredUnskippableChunkException">Chunk is known but its content is unknown to read.</exception>
    internal static IEnumerable<T?> ParseEnumerable<T>(GameBoxReader r, IProgress<GameBoxReadProgress>? progress, ILogger? logger) where T : CMwNod
    {
        return ParseEnumerable<T>(r, count: r.ReadInt32(), progress, logger);
    }

    /// <exception cref="NodeNotImplementedException">Auxiliary node is not implemented and is not parseable.</exception>
    /// <exception cref="ChunkReadNotImplementedException">Chunk does not support reading.</exception>
    /// <exception cref="IgnoredUnskippableChunkException">Chunk is known but its content is unknown to read.</exception>
    internal static IList<T?> ParseList<T>(GameBoxReader r, IProgress<GameBoxReadProgress>? progress, ILogger? logger) where T : CMwNod
    {
        var count = r.ReadInt32();
        return ParseEnumerable<T>(r, count, progress, logger).ToList(count);
    }

    /// <exception cref="NodeNotImplementedException">Auxiliary node is not implemented and is not parseable.</exception>
    /// <exception cref="ChunkReadNotImplementedException">Chunk does not support reading.</exception>
    /// <exception cref="IgnoredUnskippableChunkException">Chunk is known but its content is unknown to read.</exception>
    internal static T? Parse<T>(GameBoxReader r, uint? classId, IProgress<GameBoxReadProgress>? progress, ILogger? logger) where T : CMwNod
    {
        if (!classId.HasValue)
            classId = r.ReadUInt32();

        if (classId == uint.MaxValue)
            return null;

        classId = Remap(classId.Value);

        var id = classId.Value;

        // Duplicate check
        var type = NodeCacheManager.GetClassTypeById(id);

        if (type is null)
        {
            throw new NodeNotImplementedException(id);
        }

        var constructor = NodeCacheManager.GetClassConstructor(id);

        var node = (T)constructor();
        node.SetIDAndChunks();

        Parse(node, r, progress, logger);

        return node;
    }

    /// <exception cref="NodeNotImplementedException">Auxiliary node is not implemented and is not parseable.</exception>
    /// <exception cref="ChunkReadNotImplementedException">Chunk does not support reading.</exception>
    /// <exception cref="IgnoredUnskippableChunkException">Chunk is known but its content is unknown to read.</exception>
    internal static void Parse<T>(T node, GameBoxReader r, IProgress<GameBoxReadProgress>? progress, ILogger? logger) where T : CMwNod
    {
        var stopwatch = Stopwatch.StartNew();

        node.GBX = r.Body?.GBX;

        var type = node.GetType();

        using var scope = logger?.BeginScope("{name}", type.Name);

        uint? previousChunkId = null;

        var stream = r.BaseStream;
        var canSeek = stream.CanSeek;

        while (!canSeek || stream.Position < stream.Length)
        {
            if (canSeek && stream.Position + 4 > stream.Length)
            {
                logger?.LogDebug("Unexpected end of the stream: {position}/{length}", stream.Position, stream.Length);
                var bytes = r.ReadBytes((int)(stream.Length - stream.Position));
                break;
            }

            var chunkId = r.ReadUInt32();

            if (chunkId == 0xFACADE01) // no more chunks
            {
                break;
            }

            var progressPercentage = (float)stream.Position / stream.Length;

            if (logger?.IsEnabled(LogLevel.Debug) == true)
            {
                logger?.LogDebug("0x{chunk} ({progress})",
                    chunkId.ToString("X8"),
                    progressPercentage.ToString("0.00%"));
            }

            Chunk chunk;

            chunkId = Chunk.Remap(chunkId);

            var chunkClass = NodeCacheManager.GetChunkTypeById(type, chunkId);

            var reflected = chunkClass is not null;
            var skippable = reflected && NodeCacheManager.SkippableChunks.Contains(chunkClass!);

            // Unknown or skippable chunk
            if (!reflected || skippable)
            {
                var skip = r.ReadUInt32();

                if (skip != 0x534B4950)
                {
                    if (reflected)
                    {
                        break;
                    }

                    if (GameBox.Debug) // I don't get it
                    {
                        // Read the rest of the body

                        var streamPos = stream.Position;
                        var uncontrollableData = r.ReadToEnd();
                        stream.Position = streamPos;
                    }

                    throw new ChunkParseException(chunkId, previousChunkId);
                }

                var chunkDataSize = r.ReadInt32();
                var chunkData = new byte[chunkDataSize];
                if (chunkDataSize > 0)
                    r.Read(chunkData, 0, chunkDataSize);

                if (reflected)
                {
                    var attributesAvailable = NodeCacheManager.AvailableChunkAttributes.TryGetValue(
                        chunkId, out IEnumerable<Attribute>? attributes);

                    if (!attributesAvailable)
                        throw new ThisShouldNotHappenException();

                    if (attributes is null)
                        throw new ThisShouldNotHappenException();

                    var ignoreChunkAttribute = default(IgnoreChunkAttribute);
                    var chunkAttribute = default(ChunkAttribute);

                    foreach (var att in attributes)
                    {
                        if (att is IgnoreChunkAttribute ignoreChunkAtt)
                            ignoreChunkAttribute = ignoreChunkAtt;
                        if (att is ChunkAttribute chunkAtt)
                            chunkAttribute = chunkAtt;
                    }

                    if (chunkAttribute is null)
                        throw new ThisShouldNotHappenException();

                    NodeCacheManager.AvailableChunkConstructors.TryGetValue(chunkId,
                        out Func<Chunk>? constructor);

                    if (constructor is null)
                        throw new ThisShouldNotHappenException();

                    var c = constructor();
                    c.Node = node;
                    if (GameBox.Debug) c.Debugger ??= new();
                    ((ISkippableChunk)c).Data = chunkData;
                    if (chunkData == null || chunkData.Length == 0)
                        ((ISkippableChunk)c).Discovered = true;
                    node.Chunks.Add(c);

                    if (GameBox.Debug)
                    {
                        c.Debugger!.RawData = chunkData;
                    }

                    if (ignoreChunkAttribute == null)
                    {
                        c.OnLoad();

                        if (chunkAttribute.ProcessSync)
                            ((ISkippableChunk)c).Discover();
                    }

                    chunk = c;
                }
                else
                {
                    var debugLine = new StringBuilder("Unknown skippable chunk: ")
                        .Append(chunkId.ToString("X"))
                        .ToString();
                    Debug.WriteLine(debugLine);

                    chunk = (Chunk)Activator.CreateInstance(typeof(SkippableChunk<>).MakeGenericType(type), node, chunkId, chunkData)!;

                    if (GameBox.Debug)
                    {
                        chunk.Debugger ??= new();
                        chunk.Debugger.RawData = chunkData;
                    }

                    node.Chunks.Add(chunk);
                }
            }
            else // Known or unskippable chunk
            {
                // Faster than caching
                NodeCacheManager.AvailableChunkConstructors.TryGetValue(chunkId,
                    out Func<Chunk>? constructor);

                if (constructor is null)
                    throw new ThisShouldNotHappenException();

                var c = constructor();
                c.Node = node;

                if (GameBox.Debug) c.Debugger ??= new();

                c.OnLoad();

                node.Chunks.Add(c);

                //r.Chunk = (Chunk)c; // Set chunk temporarily for reading

                var posBefore = stream.Position;

                var gbxrw = new GameBoxReaderWriter(r);

                var attributesAvailable = NodeCacheManager.AvailableChunkAttributes.TryGetValue(
                    chunkId, out IEnumerable<Attribute>? attributes);

                if (!attributesAvailable)
                    throw new ThisShouldNotHappenException();

                if (attributes is null)
                    throw new ThisShouldNotHappenException();

                var ignoreChunkAttribute = default(IgnoreChunkAttribute);
                var autoReadWriteChunkAttribute = default(AutoReadWriteChunkAttribute);

                foreach (var att in attributes)
                {
                    if (att is IgnoreChunkAttribute ignoreChunkAtt)
                        ignoreChunkAttribute = ignoreChunkAtt;
                    if (att is AutoReadWriteChunkAttribute autoReadWriteChunkAtt)
                        autoReadWriteChunkAttribute = autoReadWriteChunkAtt;
                }

                if (ignoreChunkAttribute is not null)
                {
                    throw new IgnoredUnskippableChunkException(node, chunkId);
                }

                try
                {
                    var streamPos = default(long);

                    if (GameBox.Debug)
                    {
                        streamPos = stream.Position;
                    }

                    if (autoReadWriteChunkAttribute == null)
                    {
                        ((IChunk)c).ReadWrite(node, gbxrw);
                    }
                    else
                    {
                        var unknown = new GameBoxWriter(c.Unknown, logger: logger);
                        var unknownData = r.ReadUntilFacade().ToArray();
                        unknown.WriteBytes(unknownData);
                    }

                    if (GameBox.Debug)
                    {
                        var chunkLength = (int)(stream.Position - streamPos);

                        stream.Position = streamPos;

                        var rawData = r.ReadBytes(chunkLength);

                        c.Debugger!.RawData = rawData;
                    }
                }
                catch (EndOfStreamException)
                {
                    Debug.WriteLine($"Unexpected end of the stream while reading the chunk. 0x" + chunkId.ToString("X"));
                }

                c.Progress = (int)(stream.Position - posBefore);

                chunk = c;
            }

            progress?.Report(new GameBoxReadProgress(GameBoxReadProgressStage.Body, (float)stream.Position / stream.Length, node.GBX, chunk));

            previousChunkId = chunkId;
        }

        stopwatch.Stop();

        logger?.LogDebug("DONE! ({time}ms)", stopwatch.Elapsed.TotalMilliseconds);
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

        var className = logger?.IsEnabled(LogLevel.Debug) == true ? ClassName : null;

        foreach (Chunk chunk in Chunks)
        {
            counter++;

            if (logger?.IsEnabled(LogLevel.Debug) == true)
            {
                logger?.LogDebug("[{className}] 0x{chunkId} ({progressPercentage})",
                    className,
                    chunk.ID.ToString("X8"),
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
                    ((IChunk)chunk).ReadWrite(this, rw);
                else if (chunk is ISkippableChunk sIgnored)
                    msW.WriteBytes(sIgnored.Data);
                else
                    msW.WriteBytes(chunk.Unknown.ToArray());

                w.Write(Chunk.Remap(chunk.ID, remap));

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
    /// Makes a <see cref="GameBox"/> from this node. You can explicitly cast it to <see cref="GameBox{T}"/> depending on the <see cref="CMwNod"/>. NOTE: Non-generic <see cref="GameBox"/> doesn't have a Save method.
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
