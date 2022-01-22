namespace GBX.NET;

public class GameBoxHeader<T> : GameBoxPart where T : CMwNod
{
    public ChunkSet Chunks { get; }

    public short Version
    {
        get => GBX.Header.Version;
        set => GBX.Header.Version = value;
    }

    public GameBoxByteFormat ByteFormat
    {
        get => GBX.Header.ByteFormat;
        set => GBX.Header.ByteFormat = value;
    }

    public GameBoxCompression CompressionOfRefTable
    {
        get => GBX.Header.CompressionOfRefTable;
        set => GBX.Header.CompressionOfRefTable = value;
    }

    /// <summary>
    /// Compression type of the body part.
    /// </summary>
    /// <exception cref="HeaderOnlyParseLimitationException">Setting the compression is forbidden in <see cref="GameBox"/> where only the header was parsed (with raw body being read).</exception>
    public GameBoxCompression CompressionOfBody
    {
        get => GBX.Header.CompressionOfBody;
        set
        {
            if (GBX.Body?.RawData is not null)
                throw new HeaderOnlyParseLimitationException();
            GBX.Header.CompressionOfBody = value;
        }
    }

    public char? UnknownByte
    {
        get => GBX.Header.UnknownByte;
        set => GBX.Header.UnknownByte = value;
    }

    public uint? ID
    {
        get => GBX.Header.ID;
        internal set => GBX.Header.ID = value;
    }

    public byte[] UserData
    {
        get => GBX.Header.UserData;
    }

    public int NumNodes
    {
        get => GBX.Header.NumNodes;
    }

    public GameBoxHeader(GameBox<T> gbx) : base(gbx)
    {
        Chunks = new ChunkSet(gbx.Node);
    }

    public void Read(byte[] userData, IProgress<GameBoxReadProgress>? progress, ILogger? logger)
    {
        var gbx = (GameBox<T>)GBX;

        if (Version < 6)
            return;

        if (userData is null || userData.Length == 0)
            return;

        using var ms = new MemoryStream(userData);
        using var r = new GameBoxReader(ms, lookbackable: this, logger: logger);

        var numHeaderChunks = r.ReadInt32();

        var chunkList = new Dictionary<uint, (int Size, bool IsHeavy)>();

        for (var i = 0; i < numHeaderChunks; i++)
        {
            var chunkID = r.ReadUInt32();
            var chunkSize = r.ReadUInt32();

            var chId = chunkID & 0xFFF;
            var clId = chunkID & 0xFFFFF000;

            chunkList[clId + chId] = ((int)(chunkSize & ~0x80000000), (chunkSize & (1 << 31)) != 0);
        }

        if (logger?.IsEnabled(LogLevel.Debug) == true)
        {
            logger?.LogDebug("Header data chunk list:");

            foreach (var c in chunkList)
            {
                if (c.Value.IsHeavy)
                    logger?.LogDebug("| 0x{classId} | {size} B (Heavy)", c.Key.ToString("X8"), c.Value.Size);
                else
                    logger?.LogDebug("| 0x{classId} | {size} B", c.Key.ToString("X8"), c.Value.Size);
            }
        }

        foreach (var chunkInfo in chunkList)
        {
            var chunkId = Chunk.Remap(chunkInfo.Key);
            var nodeId = chunkId & 0xFFFFF000;

            var nodeType = NodeCacheManager.GetClassTypeById(nodeId);

            if (nodeType is null)
            {
                logger?.LogWarning("Node ID 0x{nodeId} is not implemented. This occurs only in the header therefore it's not a fatal problem (it actually is). ({nodeName})",
                    nodeId.ToString("X8"),
                    NodeCacheManager.Names.Where(x => x.Key == nodeId).Select(x => x.Value).FirstOrDefault() ?? "unknown class");
                throw new Exception();
            }

            var chunkTypes = new Dictionary<uint, Type>();

            var d = r.ReadBytes(chunkInfo.Value.Size);

            Chunk chunk;

            var type = NodeCacheManager.GetHeaderChunkTypeById(nodeType, chunkId);

            if (type is not null)
            {
                NodeCacheManager.HeaderChunkConstructors.TryGetValue(chunkId,
                    out Func<Chunk>? constructor);

                if (constructor is null)
                    throw new ThisShouldNotHappenException();

                Chunk headerChunk = constructor();
                headerChunk.Node = gbx.Node;
                ((IHeaderChunk)headerChunk).Data = d;
                if (d == null || d.Length == 0)
                    ((IHeaderChunk)headerChunk).Discovered = true;
                chunk = (Chunk)headerChunk;

                if (GameBox.Debug)
                {
                    chunk.Debugger ??= new();
                    chunk.Debugger.RawData = d;
                }

                if (d is not null)
                {
                    using var msChunk = new MemoryStream(d);
                    using var rChunk = new GameBoxReader(msChunk, lookbackable: this, logger: logger);
                    var rw = new GameBoxReaderWriter(rChunk);
                    ((IChunk)chunk).ReadWrite(gbx.Node, rw);
                    ((ISkippableChunk)chunk).Discovered = true;
                }

                ((IHeaderChunk)chunk).IsHeavy = chunkInfo.Value.IsHeavy;
            }
            else if (nodeType is not null)
                chunk = (Chunk)Activator.CreateInstance(typeof(HeaderChunk<>).MakeGenericType(nodeType), gbx.Node, d, chunkId)!;
            else
                chunk = new HeaderChunk(chunkId, d) { IsHeavy = chunkInfo.Value.IsHeavy };

            Chunks.Add(chunk);

            progress?.Report(new GameBoxReadProgress(
                GameBoxReadProgressStage.HeaderUserData,
                r.BaseStream.Position / (float)r.BaseStream.Length,
                gbx,
                chunk));
        }
    }

    internal void Write(GameBoxWriter w, int numNodes, IDRemap remap, ILogger? logger)
    {
        w.Write(GameBox.Magic, StringLengthPrefix.None);
        w.Write(Version);

        if (Version < 3)
        {
            return;
        }

        w.Write((byte)ByteFormat);
        w.Write((byte)CompressionOfRefTable);
        w.Write((byte)CompressionOfBody);

        if (Version >= 4)
        {
            w.Write((byte)UnknownByte.GetValueOrDefault());
        }

        w.Write(Chunk.Remap(GBX.ID.GetValueOrDefault(), remap));

        if (Version >= 6)
        {
            WriteVersion6Header(w, remap, logger);
        }

        w.Write(numNodes);
    }

    private void WriteVersion6Header(GameBoxWriter w, IDRemap remap, ILogger? logger)
    {
        if (Chunks is null)
        {
            w.Write(0);
            return;
        }

        using var userData = new MemoryStream();
        using var gbxw = new GameBoxWriter(userData, w.Body, w.Lookbackable, logger);

        var gbxrw = new GameBoxReaderWriter(gbxw);

        var lengths = new Dictionary<uint, int>();

        foreach (var chunk in Chunks)
        {
            chunk.Unknown.Position = 0;

            var pos = userData.Position;
            if (((ISkippableChunk)chunk).Discovered)
                ((IChunk)chunk).ReadWrite(((GameBox<T>)GBX).Node, gbxrw);
            else
                ((ISkippableChunk)chunk).Write(gbxw);

            lengths[chunk.Id] = (int)(userData.Position - pos);
        }

        // Actual data size plus the class id (4 bytes) and each length (4 bytes) plus the number of chunks integer
        w.Write((int)userData.Length + Chunks.Count * 8 + 4);

        // Write number of header chunks integer
        w.Write(Chunks.Count);

        foreach (Chunk chunk in Chunks)
        {
            w.Write(Chunk.Remap(chunk.Id, remap));
            var length = lengths[chunk.Id];
            if (((IHeaderChunk)chunk).IsHeavy)
                length |= 1 << 31;
            w.Write(length);
        }

        w.Write(userData.ToArray(), 0, (int)userData.Length);
    }

    internal void Write(GameBoxWriter w, int numNodes, ILogger? logger)
    {
        Write(w, numNodes, IDRemap.Latest, logger);
    }

    public TChunk CreateChunk<TChunk>(byte[] data) where TChunk : Chunk
    {
        return Chunks.Create<TChunk>(data);
    }

    public TChunk CreateChunk<TChunk>() where TChunk : Chunk
    {
        return CreateChunk<TChunk>(Array.Empty<byte>());
    }

    public void InsertChunk(IHeaderChunk chunk)
    {
        Chunks.Add((Chunk)chunk);
    }

    public void DiscoverChunk<TChunk>() where TChunk : IHeaderChunk
    {
        foreach (var chunk in Chunks)
            if (chunk is TChunk c)
                c.Discover();
    }

    public void DiscoverChunks<TChunk1, TChunk2>() where TChunk1 : IHeaderChunk where TChunk2 : IHeaderChunk
    {
        foreach (var chunk in Chunks)
        {
            if (chunk is TChunk1 c1)
                c1.Discover();
            if (chunk is TChunk2 c2)
                c2.Discover();
        }
    }

    public void DiscoverChunks<TChunk1, TChunk2, TChunk3>()
        where TChunk1 : IHeaderChunk
        where TChunk2 : IHeaderChunk
        where TChunk3 : IHeaderChunk
    {
        foreach (var chunk in Chunks)
        {
            if (chunk is TChunk1 c1)
                c1.Discover();
            if (chunk is TChunk2 c2)
                c2.Discover();
            if (chunk is TChunk3 c3)
                c3.Discover();
        }
    }

    public void DiscoverChunks<TChunk1, TChunk2, TChunk3, TChunk4>()
        where TChunk1 : IHeaderChunk
        where TChunk2 : IHeaderChunk
        where TChunk3 : IHeaderChunk
        where TChunk4 : IHeaderChunk
    {
        foreach (var chunk in Chunks)
        {
            if (chunk is TChunk1 c1)
                c1.Discover();
            if (chunk is TChunk2 c2)
                c2.Discover();
            if (chunk is TChunk3 c3)
                c3.Discover();
            if (chunk is TChunk4 c4)
                c4.Discover();
        }
    }

    public void DiscoverChunks<TChunk1, TChunk2, TChunk3, TChunk4, TChunk5>()
        where TChunk1 : IHeaderChunk
        where TChunk2 : IHeaderChunk
        where TChunk3 : IHeaderChunk
        where TChunk4 : IHeaderChunk
        where TChunk5 : IHeaderChunk
    {
        foreach (var chunk in Chunks)
        {
            if (chunk is TChunk1 c1)
                c1.Discover();
            if (chunk is TChunk2 c2)
                c2.Discover();
            if (chunk is TChunk3 c3)
                c3.Discover();
            if (chunk is TChunk4 c4)
                c4.Discover();
            if (chunk is TChunk5 c5)
                c5.Discover();
        }
    }

    public void DiscoverChunks<TChunk1, TChunk2, TChunk3, TChunk4, TChunk5, TChunk6>()
        where TChunk1 : IHeaderChunk
        where TChunk2 : IHeaderChunk
        where TChunk3 : IHeaderChunk
        where TChunk4 : IHeaderChunk
        where TChunk5 : IHeaderChunk
        where TChunk6 : IHeaderChunk
    {
        foreach (var chunk in Chunks)
        {
            if (chunk is TChunk1 c1)
                c1.Discover();
            if (chunk is TChunk2 c2)
                c2.Discover();
            if (chunk is TChunk3 c3)
                c3.Discover();
            if (chunk is TChunk4 c4)
                c4.Discover();
            if (chunk is TChunk5 c5)
                c5.Discover();
            if (chunk is TChunk6 c6)
                c6.Discover();
        }
    }

    public void DiscoverAllChunks()
    {
        foreach (var chunk in Chunks)
            if (chunk is IHeaderChunk s)
                s.Discover();
    }

    public TChunk? GetChunk<TChunk>() where TChunk : IHeaderChunk
    {
        foreach (var chunk in Chunks)
        {
            if (chunk is TChunk t)
            {
                t.Discover();
                return t;
            }
        }

        return default;
    }

    public bool TryGetChunk<TChunk>(out TChunk? chunk) where TChunk : IHeaderChunk
    {
        chunk = GetChunk<TChunk>();
        return chunk != null;
    }

    public void RemoveAllChunks()
    {
        Chunks.Clear();
    }

    public bool RemoveChunk<TChunk>() where TChunk : Chunk
    {
        return Chunks.Remove<TChunk>();
    }
}
