namespace GBX.NET;

public class GameBoxHeader
{
    public short Version { get; init; }
    public GameBoxFormat Format { get; init; }
    public GameBoxCompression CompressionOfRefTable { get; init; }
    public GameBoxCompression CompressionOfBody { get; init; }
    public char? UnknownByte { get; init; }
    public uint Id { get; init; }
    public byte[] UserData { get; init; }
    public int NumNodes { get; init; }

    public GameBoxHeader(uint id)
    {
        Version = 6;
        Format = GameBoxFormat.Byte;
        CompressionOfRefTable = GameBoxCompression.Uncompressed;
        CompressionOfBody = GameBoxCompression.Compressed;
        UnknownByte = 'R';
        Id = id;
        UserData = Array.Empty<byte>();
        NumNodes = 0;
    }

    internal void Write(Node node, GameBoxWriter w, int numNodes, ILogger? logger)
    {
        w.Write(GameBox.Magic, StringLengthPrefix.None);
        w.Write(Version);

        if (Version < 3)
        {
            throw new VersionNotSupportedException(Version);
        }

        w.Write((byte)Format);
        w.Write((byte)CompressionOfRefTable);
        w.Write((byte)CompressionOfBody);

        if (Version >= 4)
        {
            w.Write((byte)UnknownByte.GetValueOrDefault('R'));
        }

        w.Write(Chunk.Remap(Id, w.Settings.Remap));

        if (Version >= 6)
        {
            WriteVersion6(node, w, logger);
        }

        w.Write(numNodes);
    }

    private static void WriteVersion6(Node node, GameBoxWriter w, ILogger? logger)
    {
        if (node.HeaderChunks is null)
        {
            w.Write(0);
            return;
        }

        using var userDataStream = new MemoryStream();
        using var userDataWriter = new GameBoxWriter(userDataStream, w.Settings, logger);
        var userDataReaderWriter = new GameBoxReaderWriter(userDataWriter);

        var table = new Dictionary<uint, int>();

        foreach (IHeaderChunk chunk in node.HeaderChunks)
        {
            chunk.Unknown.Position = 0;

            var pos = userDataStream.Position;

            chunk.ReadWrite(node, userDataReaderWriter);

            table[chunk.Id] = (int)(userDataStream.Position - pos);
        }

        // Actual data size plus the class id (4 bytes) and each length (4 bytes) plus the number of chunks integer
        w.Write((int)userDataStream.Length + node.HeaderChunks.Count * 8 + 4);

        // Write number of header chunks integer
        w.Write(node.HeaderChunks.Count);

        foreach (IHeaderChunk chunk in node.HeaderChunks)
        {
            w.Write(Chunk.Remap(chunk.Id, w.Settings.Remap));

            var length = table[chunk.Id];

            if (chunk.IsHeavy)
            {
                length |= 1 << 31;
            }

            w.Write(length);
        }

        w.Write(userDataStream.ToArray());
    }

    /// <summary>
    /// Parses the common Gbx header.
    /// </summary>
    /// <param name="r">Reader.</param>
    /// <param name="logger">Logger.</param>
    /// <returns>An immutable <see cref="GameBoxHeader"/> object.</returns>
    /// <exception cref="NotAGbxException">The stream is not a Gbx stream nor starting at the correct position.</exception>
    /// <exception cref="VersionNotSupportedException">Version of Gbx below 3 is not supported.</exception>
    /// <exception cref="TextFormatNotSupportedException">Text-formatted Gbx is not supported.</exception>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    public static GameBoxHeader Parse(GameBoxReader r, ILogger? logger = null)
    {
        if (!r.HasMagic(GameBox.Magic))
        {
            throw new NotAGbxException();
        }

        logger?.LogDebug("GBX magic found");

        var version = r.ReadInt16();
        logger?.LogVersion(version);

        if (version < 3 || version >= 7)
        {
            throw new VersionNotSupportedException(version);
        }

        var format = (GameBoxFormat)r.ReadByte();
        logger?.LogFormat(format);

        if (format == GameBoxFormat.Text)
        {
            throw new TextFormatNotSupportedException();
        }

        var compressionOfRefTable = (GameBoxCompression)r.ReadByte();
        logger?.LogRefTableCompression(compressionOfRefTable);

        var compressionOfBody = (GameBoxCompression)r.ReadByte();
        logger?.LogBodyCompression(compressionOfBody);

        var unknownByte = default(char?);

        if (version >= 4)
        {
            unknownByte = (char)r.ReadByte();
            logger?.LogUnknownByte(unknownByte.Value);
        }

        var id = Node.RemapToLatest(r.ReadUInt32());
        logger?.LogClassId(id.ToString("X8"));

        var userData = Array.Empty<byte>();

        if (version >= 6)
        {
            userData = r.ReadBytes();
            logger?.LogUserDataSize(userData.Length / 1024f);
        }

        var numNodes = r.ReadInt32();
        logger?.LogNumberOfNodes(numNodes);

        logger?.LogDebug("Header complete");

        return new GameBoxHeader(id)
        {
            Version = version,
            Format = format,
            CompressionOfRefTable = compressionOfRefTable,
            CompressionOfBody = compressionOfBody,
            UnknownByte = unknownByte,
            UserData = userData,
            NumNodes = numNodes
        };
    }

    public IDictionary<uint, HeaderChunkSize> GetChunkList()
    {
        if (UserData.Length == 0)
        {
            return new Dictionary<uint, HeaderChunkSize>();
        }

        using var ms = new MemoryStream(UserData);
        using var r = new GameBoxReader(ms);

        return GetChunkList(r);
    }

    private static IDictionary<uint, HeaderChunkSize> GetChunkList(GameBoxReader r)
    {
        var numHeaderChunks = r.ReadInt32();

        var chunkList = new Dictionary<uint, HeaderChunkSize>(numHeaderChunks);

        for (var i = 0; i < numHeaderChunks; i++)
        {
            var chunkId = r.ReadUInt32();
            var chunkSize = r.ReadUInt32();

            chunkId = Chunk.Remap(chunkId);

            chunkList[chunkId] = new((int)(chunkSize & ~0x80000000), (chunkSize & (1 << 31)) != 0);
        }

        return chunkList;
    }

    internal static void ProcessUserData(Node node, Type nodeType, GameBoxReader r, ILogger? logger)
    {
        var chunkList = GetChunkList(r);
        var chunks = ProcessChunks(node, nodeType, r, logger, chunkList);

        node.HeaderChunks = new ChunkSet(node, chunks);
    }

    private static IEnumerable<Chunk> ProcessChunks(Node node,
                                                    Type nodeType,
                                                    GameBoxReader r,
                                                    ILogger? logger,
                                                    IDictionary<uint, HeaderChunkSize> chunkList)
    {
        logger?.LogDebug("Header chunks:");

        foreach (var chunkInfo in chunkList)
        {
            yield return ProcessChunk(node, nodeType, r, logger, chunkInfo.Key, chunkInfo.Value.Size, chunkInfo.Value.IsHeavy);
        }
    }

    private static Chunk ProcessChunk(Node node,
                                        Type nodeType,
                                        GameBoxReader r,
                                        ILogger? logger,
                                        uint chunkId,
                                        int size,
                                        bool isHeavy)
    {
        var classId = chunkId & 0xFFFFF000;

        if (isHeavy)
        {
            logger?.LogHeaderChunkHeavy(classId.ToString("X8"), size);
        }
        else
        {
            logger?.LogHeaderChunk(classId.ToString("X8"), size);
        }

        // Chunk data can be always read
        var chunkData = r.ReadBytes(size);

        var chunkNodeType = NodeCacheManager.GetClassTypeById(classId);

        if (chunkNodeType is null)
        {
            NodeCacheManager.Names.TryGetValue(classId, out var className);

            logger?.LogHeaderChunkNodeNotImplemented(classId.ToString("X8"), className ?? "unknown class");

            return new HeaderChunk(chunkId, chunkData, isHeavy);
        }

        var headerChunkType = NodeCacheManager.GetHeaderChunkTypeById(chunkNodeType, chunkId);

        if (headerChunkType is null)
        {
            var genericHeaderChunkType = typeof(HeaderChunk<>).MakeGenericType(chunkNodeType);

            var args = new object?[] { node, chunkData, chunkId, isHeavy };

            if (Activator.CreateInstance(genericHeaderChunkType, args) is not Chunk chunk)
            {
                throw new ThisShouldNotHappenException();
            }

            return chunk;
        }

        var headerChunk = NodeCacheManager.HeaderChunkConstructors[chunkId]();
        headerChunk.Data = chunkData;
        headerChunk.IsHeavy = isHeavy;

        if (nodeType != chunkNodeType && !nodeType.IsSubclassOf(chunkNodeType))
        {
            // There are cast-related problems when one of the header chunks is not part of inheritance
            // For example, CGameCtnDecoration has a header chunk of type CPlugGameSkin that is not
            // inherited by CGameCtnDecoration. This could be later solved by using special attributes
            // on these kinds of unusual chunks.
            return (Chunk)headerChunk;
        }

        if (chunkData.Length > 0)
        {
            using var chunkStream = new MemoryStream(chunkData);
            using var chunkReader = new GameBoxReader(chunkStream, r.Settings, logger: logger);
            var rw = new GameBoxReaderWriter(chunkReader);

            headerChunk.ReadWrite(node, rw);
        }

        return (Chunk)headerChunk; //
    }

    /// <summary>
    /// Parses the common Gbx header.
    /// </summary>
    /// <param name="stream">Stream.</param>
    /// <param name="logger">Logger.</param>
    /// <returns>An immutable <see cref="GameBoxHeader"/> object.</returns>
    /// <exception cref="NotAGbxException">The stream is not a Gbx stream or is not starting at the correct position.</exception>
    /// <exception cref="VersionNotSupportedException">Version of Gbx below 3 is not supported.</exception>
    /// <exception cref="TextFormatNotSupportedException">Text-formatted Gbx is not supported.</exception>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    public static GameBoxHeader Parse(Stream stream, ILogger? logger = null)
    {
        using var r = new GameBoxReader(stream, logger: logger);
        return Parse(r, logger);
    }

    /// <summary>
    /// Parses the common Gbx header.
    /// </summary>
    /// <param name="fileName">File name.</param>
    /// <param name="logger">Logger.</param>
    /// <returns>An immutable <see cref="GameBoxHeader"/> object.</returns>
    /// <exception cref="NotAGbxException">The stream is not a Gbx stream or is not starting at the correct position.</exception>
    /// <exception cref="VersionNotSupportedException">Version of Gbx below 3 is not supported.</exception>
    /// <exception cref="TextFormatNotSupportedException">Text-formatted Gbx is not supported.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    public static GameBoxHeader Parse(string fileName, ILogger? logger = null)
    {
        using var fs = File.OpenRead(fileName);
        return Parse(fs, logger);
    }
}
