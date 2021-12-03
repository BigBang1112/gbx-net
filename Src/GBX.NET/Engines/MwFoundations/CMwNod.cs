using System.Diagnostics;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;

namespace GBX.NET.Engines.MwFoundations;

[Node(0x01001000)]
public class CMwNod
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
    /// Chunk where the aux node appeared
    /// </summary>
    public Chunk? ParentChunk { get; set; }

    public virtual uint ID { get; private set; }

    public string[]? Dependencies { get; set; }

    /// <summary>
    /// Name of the class. The format is <c>Engine::Class</c>.
    /// </summary>
    public string ClassName
    {
        get
        {
            if (NodeCacheManager.Names.TryGetValue(ID, out string? name))
                return name;
            return GetType().FullName?.Substring("GBX.NET.Engines".Length + 1).Replace(".", "::") ?? string.Empty;
        }
    }

    protected CMwNod()
    {

    }

    protected CMwNod(params Chunk[] chunks) : this()
    {
        foreach (var chunk in chunks)
        {
            GetType()
                .GetMethod("CreateChunk")!
                .MakeGenericMethod(chunk.GetType())
                .Invoke(this, Array.Empty<object>());
        }
    }

    internal void SetIDAndChunks()
    {
        ID = ((NodeAttribute)NodeCacheManager.AvailableClassAttributes[GetType()]
            .First(x => x is NodeAttribute)).ID;
        chunks = new ChunkSet(this);
    }

    internal static T?[] ParseArray<T>(GameBoxReader r) where T : CMwNod
    {
        var count = r.ReadInt32();
        var array = new T?[count];

        for (var i = 0; i < count; i++)
            array[i] = Parse<T>(r);

        return array;
    }

    internal static IEnumerable<T?> ParseEnumerable<T>(GameBoxReader r, int count) where T : CMwNod
    {
        for (var i = 0; i < count; i++)
            yield return Parse<T>(r);
    }

    internal static IEnumerable<T?> ParseEnumerable<T>(GameBoxReader r) where T : CMwNod
    {
        return ParseEnumerable<T>(r, count: r.ReadInt32());
    }

    internal static IList<T?> ParseList<T>(GameBoxReader r) where T : CMwNod
    {
        var count = r.ReadInt32();
        return ParseEnumerable<T>(r, count).ToList(count);
    }

    internal static T? Parse<T>(GameBoxReader r, uint? classID = null, IProgress<GameBoxReadProgress>? progress = null) where T : CMwNod
    {
        if (!classID.HasValue)
            classID = r.ReadUInt32();

        if (classID == uint.MaxValue) return null;

        classID = Remap(classID.Value);

        if (!NodeCacheManager.AvailableClasses.TryGetValue(classID.Value, out Type? type))
            throw new NotImplementedException($"Node ID 0x{classID.Value:X8} is not implemented. ({NodeCacheManager.Names.Where(x => x.Key == Chunk.Remap(classID.Value)).Select(x => x.Value).FirstOrDefault() ?? "unknown class"})");

        NodeCacheManager.AvailableClassConstructors.TryGetValue(classID.Value, out Func<CMwNod>? constructor);

        if (constructor is null)
            throw new ThisShouldNotHappenException();

        var node = (T)constructor();
        node.SetIDAndChunks();

        Parse(node, r, progress);

        return node;
    }

    [global::System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0071:Zjednodušit interpolaci", Justification = "<Čeká>")]
    internal static void Parse<T>(T node, GameBoxReader r, IProgress<GameBoxReadProgress>? progress = null) where T : CMwNod
    {
        var stopwatch = Stopwatch.StartNew();

        node.GBX = r.Body?.GBX;

        var type = node.GetType();

        uint? previousChunkId = null;

        while (!r.BaseStream.CanSeek || r.BaseStream.Position < r.BaseStream.Length)
        {
            if (r.BaseStream.CanSeek && r.BaseStream.Position + 4 > r.BaseStream.Length)
            {
                Debug.WriteLine($"Unexpected end of the stream: {r.BaseStream.Position.ToString()}/{r.BaseStream.Length.ToString()}");
                var bytes = r.ReadBytes((int)(r.BaseStream.Length - r.BaseStream.Position));
                break;
            }

            var chunkId = r.ReadUInt32();

            if (chunkId == 0xFACADE01) // no more chunks
            {
                break;
            }

            var logChunk = new StringBuilder("[")
                .Append(node.ClassName)
                .Append("] 0x")
                .Append(chunkId.ToString("X8"));

            if (r.BaseStream.CanSeek) // Decompressed body can always seek
            {
                logChunk.Append(" (")
                    .Append(((float)r.BaseStream.Position / r.BaseStream.Length).ToString("0.00%"))
                    .Append(')');
            }

            if (node.GBX is null || !node.GBX.ID.HasValue || Remap(node.GBX.ID.Value) != node.ID)
            {
                logChunk.Insert(0, "~ ");
            }

            Log.Write(logChunk.ToString());

            Chunk chunk;

            chunkId = Chunk.Remap(chunkId);

            Type? chunkClass = null;

            var reflected = (
                   (chunkId & 0xFFFFF000) == node.ID
                || NodeCacheManager.AvailableInheritanceClasses[type].Contains(chunkId & 0xFFFFF000)
              )
              && (
                   NodeCacheManager.AvailableChunkClasses[type].TryGetValue(chunkId, out chunkClass)
            );

            if (reflected && chunkClass is null)
                throw new ThisShouldNotHappenException();

            var skippable = reflected && chunkClass!.BaseType!.GetGenericTypeDefinition() == typeof(SkippableChunk<>);

            // Unknown or skippable chunk
            if (!reflected || skippable)
            {
                var skip = r.ReadUInt32();

                if (skip != 0x534B4950)
                {
                    if (chunkId == 0 || reflected)
                    {
                        break;
                    }

                    var logChunkError = $"[{node.ClassName}] 0x{chunkId.ToString("X8")} ERROR (wrong chunk format or unknown unskippable chunk)";
                    if (node.GBX is not null && node.GBX.ID.HasValue && Remap(node.GBX.ID.Value) == node.ID)
                        Log.Write(logChunkError, ConsoleColor.Red);
                    else
                        Log.Write("~ " + logChunkError, ConsoleColor.Red);

#if DEBUG
                    // Read the rest of the body

                    var streamPos = r.BaseStream.Position;
                    var uncontrollableData = r.ReadToEnd();
                    r.BaseStream.Position = streamPos;
#endif

                    throw new ChunkParseException(chunkId, previousChunkId);

                    /* Usually breaks in the current state and causes confusion
                        * 
                        * var buffer = BitConverter.GetBytes(chunkID);
                    using (var restMs = new MemoryStream(ushort.MaxValue))
                    {
                        restMs.Write(buffer, 0, buffer.Length);

                        while (r.PeekUInt32() != 0xFACADE01)
                            restMs.WriteByte(r.ReadByte());

                        node.Rest = restMs.ToArray();
                    }
                    Debug.WriteLine("FACADE found.");*/
                }

                var chunkDataSize = r.ReadInt32();
                var chunkData = new byte[chunkDataSize];
                if (chunkDataSize > 0)
                    r.Read(chunkData, 0, chunkDataSize);

                if (reflected)
                {
                    var attributesAvailable = NodeCacheManager.AvailableChunkAttributes[type].TryGetValue(
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

                    NodeCacheManager.AvailableChunkConstructors[type].TryGetValue(chunkId,
                        out Func<Chunk>? constructor);

                    if (constructor is null)
                        throw new ThisShouldNotHappenException();

                    var c = constructor();
                    c.Node = node;
                    ((ISkippableChunk)c).Data = chunkData;
                    if (chunkData == null || chunkData.Length == 0)
                        ((ISkippableChunk)c).Discovered = true;
                    node.Chunks.Add(c);

#if DEBUG
                    c.Debugger.RawData = chunkData;
#endif

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
                    Debug.WriteLine("Unknown skippable chunk: " + chunkId.ToString("X"));
                    chunk = (Chunk)Activator.CreateInstance(typeof(SkippableChunk<>).MakeGenericType(type), node, chunkId, chunkData)!;
#if DEBUG
                    chunk.Debugger.RawData = chunkData;
#endif
                    node.Chunks.Add(chunk);
                }
            }
            else // Known or unskippable chunk
            {
                // Faster than caching
                NodeCacheManager.AvailableChunkConstructors[type].TryGetValue(chunkId,
                    out Func<Chunk>? constructor);

                if (constructor is null)
                    throw new ThisShouldNotHappenException();

                var c = constructor();
                c.Node = node;

                c.OnLoad();

                node.Chunks.Add(c);

                //r.Chunk = (Chunk)c; // Set chunk temporarily for reading

                var posBefore = r.BaseStream.Position;

                var gbxrw = new GameBoxReaderWriter(r);

                var attributesAvailable = NodeCacheManager.AvailableChunkAttributes[type].TryGetValue(
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
#if DEBUG
                    var streamPos = r.BaseStream.Position;
#endif
                    if (autoReadWriteChunkAttribute == null)
                    {
                        ((IChunk)c).ReadWrite(node, gbxrw);
                    }
                    else
                    {
                        var unknown = new GameBoxWriter(c.Unknown);
                        var unknownData = r.ReadUntilFacade().ToArray();
                        unknown.WriteBytes(unknownData);
                    }
#if DEBUG
                    var chunkLength = (int)(r.BaseStream.Position - streamPos);

                    r.BaseStream.Position = streamPos;

                    var rawData = r.ReadBytes(chunkLength);

                    c.Debugger.RawData = rawData;
#endif
                }
                catch (EndOfStreamException)
                {
                    Debug.WriteLine($"Unexpected end of the stream while reading the chunk.");
                }

                c.Progress = (int)(r.BaseStream.Position - posBefore);

                chunk = c;
            }

            progress?.Report(new GameBoxReadProgress(GameBoxReadProgressStage.Body, (float)r.BaseStream.Position / r.BaseStream.Length, node.GBX, chunk));

            previousChunkId = chunkId;
        }

        stopwatch.Stop();

        var logNodeCompletion = $"[{node.ClassName}] DONE! ({stopwatch.Elapsed.TotalMilliseconds.ToString()}ms)";
        if (node.GBX is not null && node.GBX.ID.HasValue == true && Remap(node.GBX.ID.Value) == node.ID)
            Log.Write(logNodeCompletion, ConsoleColor.Green);
        else
            Log.Write($"~ {logNodeCompletion}", ConsoleColor.Green);
    }

    public void Read(GameBoxReader r)
    {
        throw new NotImplementedException($"Node doesn't support Read.");
    }

    public void Write(GameBoxWriter w)
    {
        Write(w, IDRemap.Latest);
    }

    public void Write(GameBoxWriter w, IDRemap remap)
    {
        var stopwatch = Stopwatch.StartNew();

        int counter = 0;

        var type = GetType();
        var writingNotSupported = type.GetCustomAttribute<WritingNotSupportedAttribute>() != null;
        if (writingNotSupported)
            throw new NotSupportedException($"Writing of {type.Name} is not supported.");

        foreach (Chunk chunk in Chunks)
        {
            counter++;

            var logChunk = $"[{ClassName}] 0x{chunk.ID:X8} ({(float)counter / Chunks.Count:0.00%})";
            if (GBX is not null && GBX.ID.HasValue == true && GBX.ID.Value == ID)
                Log.Write(logChunk);
            else
                Log.Write($"~ {logChunk}");

            chunk.Node = this;
            chunk.Unknown.Position = 0;

            if (chunk is ILookbackable l)
            {
                l.IdWritten = false;
                l.IdStrings.Clear();
            }

            using var ms = new MemoryStream();
            using var msW = new GameBoxWriter(ms, w.Body, w.Lookbackable);
            var rw = new GameBoxReaderWriter(msW);

            try
            {
                if (chunk is ISkippableChunk s && !s.Discovered)
                    s.Write(msW);
                else if (!Attribute.IsDefined(chunk.GetType(), typeof(AutoReadWriteChunkAttribute)))
                    ((IChunk)chunk).ReadWrite(this, rw);
                else
                    msW.Write(chunk.Unknown.ToArray(), 0, (int)chunk.Unknown.Length);

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

        var logNodeCompletion = $"[{ClassName}] DONE! ({stopwatch.Elapsed.TotalMilliseconds}ms)";
        if (GBX is not null && GBX.ID.HasValue == true && GBX.ID.Value == ID)
            Log.Write(logNodeCompletion, ConsoleColor.Green);
        else
            Log.Write($"~ {logNodeCompletion}", ConsoleColor.Green);
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
            if (!nodeProperty.PropertyType.IsSubclassOf(typeof(CMwNod)))
                continue;

            var node = nodeProperty.GetValue(this) as CMwNod;
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
    /// <exception cref="NotSupportedException"/>
    public void Save(string? fileName = default, IDRemap remap = default)
    {
        Save(new Type[] { typeof(string), typeof(IDRemap) }, new object?[] { fileName, remap });
    }

    /// <summary>
    /// Saves the serialized node to a stream in a GBX form.
    /// </summary>
    /// <param name="stream">Any kind of stream that supports writing.</param>
    /// <param name="remap">What to remap the newest node IDs to. Used for older games.</param>
    /// <exception cref="NotSupportedException"/>
    public void Save(Stream stream, IDRemap remap = default)
    {
        Save(new Type[] { typeof(Stream), typeof(IDRemap) }, new object?[] { stream, remap });
    }

    internal static uint Remap(uint id)
    {
        if (NodeCacheManager.Mappings.TryGetValue(id, out uint newerClassID))
            return newerClassID;
        return id;
    }
}
