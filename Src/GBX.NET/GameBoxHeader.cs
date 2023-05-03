namespace GBX.NET;

// since 10. 4. 2023, header is no longer immutable
public class GameBoxHeader
{
    public short Version { get; init; }
    public GameBoxFormat Format { get; init; }
    public GameBoxCompression CompressionOfRefTable { get; init; }
    public GameBoxCompression CompressionOfBody { get; internal set; }
    public char? UnknownByte { get; init; }
    public uint Id { get; init; }
    public byte[] UserData { get; init; }
    public int NumNodes { get; init; }

    /// <summary>
    /// Header chunks that are part of an unknown node. For known node header chunks, see <see cref="GameBox.Node"/> -> <see cref="INodeHeader.HeaderChunks"/>.
    /// </summary>
    public HeaderChunkSet HeaderChunks { get; init; }

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
        HeaderChunks = new();
    }

    internal void Write(Node node, GameBoxWriter w)
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

        w.Write(Chunk.Remap(Id, w.Remap));

        if (Version >= 6)
        {
            WriteVersion6(node, w);
        }
    }

    private void WriteVersion6(Node node, GameBoxWriter w)
    {
        var headerChunks = (node as INodeHeader)?.HeaderChunks ?? HeaderChunks;

        if (headerChunks is null || headerChunks.Count == 0)
        {
            w.Write(0);
            return;
        }

        using var userDataStream = new MemoryStream();
        using var userDataWriter = new GameBoxWriter(userDataStream, w);
        var userDataReaderWriter = new GameBoxReaderWriter(userDataWriter);

        var table = new Dictionary<uint, int>();

        foreach (IHeaderChunk chunk in headerChunks)
        {
            chunk.Unknown.Position = 0;

            var pos = userDataStream.Position;

            chunk.ReadWrite(node, userDataReaderWriter);

            table[chunk.Id] = (int)(userDataStream.Position - pos);
        }

        // Actual data size plus the class id (4 bytes) and each length (4 bytes) plus the number of chunks integer
        w.Write((int)userDataStream.Length + headerChunks.Count * 8 + 4);

        // Write number of header chunks integer
        w.Write(headerChunks.Count);

        foreach (IHeaderChunk chunk in headerChunks)
        {
            w.Write(Chunk.Remap(chunk.Id, w.Remap));

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
    /// <returns>An immutable <see cref="GameBoxHeader"/> object.</returns>
    /// <exception cref="NotAGbxException">The stream is not a Gbx stream nor starting at the correct position.</exception>
    /// <exception cref="VersionNotSupportedException">Version of Gbx below 3 is not supported.</exception>
    /// <exception cref="TextFormatNotSupportedException">Text-formatted Gbx is not supported.</exception>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    public static GameBoxHeader Parse(GameBoxReader r)
    {
        ParseUntilClassId(r,
            out short version,
            out GameBoxFormat format,
            out GameBoxCompression compressionOfRefTable,
            out GameBoxCompression compressionOfBody,
            out char? unknownByte,
            out uint id);

        var userData = Array.Empty<byte>();

        if (version >= 6)
        {
            userData = GameBox.OpenPlanetHookExtractMode ? new byte[r.ReadInt32()] : r.ReadBytes();

            r.Logger?.LogUserDataSize(userData.Length / 1024f);
        }

        ParseRestOfHeader(r, out int numNodes);

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

    internal static async Task<GameBoxHeader> ParseAsync(GameBoxReader r, GameBoxAsyncReadAction? asyncAction, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        
        ParseUntilClassId(r,
            out short version,
            out GameBoxFormat format,
            out GameBoxCompression compressionOfRefTable,
            out GameBoxCompression compressionOfBody,
            out char? unknownByte,
            out uint id);

        if (asyncAction?.AfterClassId is not null)
        {
            await asyncAction.AfterClassId(id, cancellationToken);
        }

        var userData = Array.Empty<byte>();

        if (version >= 6)
        {
            userData = new byte[r.ReadInt32()];

            if (!GameBox.OpenPlanetHookExtractMode)
            {
                var read = await r.BaseStream.ReadAsync(userData, 0, userData.Length, cancellationToken);

                if (read != userData.Length)
                {
                    throw new EndOfStreamException();
                }
            }

            r.Logger?.LogUserDataSize(userData.Length / 1024f);
        }

        ParseRestOfHeader(r, out int numNodes);

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

    private static void ParseUntilClassId(GameBoxReader r, out short version, out GameBoxFormat format, out GameBoxCompression compressionOfRefTable, out GameBoxCompression compressionOfBody, out char? unknownByte, out uint id)
    {
        if (!r.HasMagic(GameBox.Magic))
        {
            throw new NotAGbxException();
        }

        r.Logger?.LogDebug("GBX magic found");

        version = r.ReadInt16();
        r.Logger?.LogVersion(version);

        if (version < 3 || version >= 7)
        {
            throw new VersionNotSupportedException(version);
        }

        format = (GameBoxFormat)r.ReadByte();
        r.Logger?.LogFormat(format);

        switch (format)
        {
            case GameBoxFormat.Byte:
                break;
            case GameBoxFormat.Text:
                throw new TextFormatNotSupportedException();
            default:
                throw new InvalidDataException($"Unknown format: {format}");
        }

        compressionOfRefTable = (GameBoxCompression)r.ReadByte();
        r.Logger?.LogRefTableCompression(compressionOfRefTable);

        switch (compressionOfRefTable)
        {
            case GameBoxCompression.Uncompressed:
            case GameBoxCompression.Compressed:
                break;
            default:
                throw new InvalidDataException($"Unknown ref. table compression: {compressionOfRefTable}");
        }

        compressionOfBody = (GameBoxCompression)r.ReadByte();
        r.Logger?.LogBodyCompression(compressionOfBody);

        switch (compressionOfBody)
        {
            case GameBoxCompression.Uncompressed:
            case GameBoxCompression.Compressed:
                break;
            default:
                throw new InvalidDataException($"Unknown body compression: {compressionOfBody}");
        }

        unknownByte = default;
        
        if (version >= 4)
        {
            unknownByte = (char)r.ReadByte();
            r.Logger?.LogUnknownByte(unknownByte.Value);
        }

        id = Node.RemapToLatest(r.ReadUInt32());
        r.Logger?.LogClassId(id.ToString("X8"));
    }

    private static void ParseRestOfHeader(GameBoxReader r, out int numNodes)
    {
        numNodes = r.ReadInt32();
        r.Logger?.LogNumberOfNodes(numNodes);

        r.Logger?.LogDebug("Header complete");
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

    internal void ProcessUserData(Node node, Type nodeType, GameBoxReader r, ILogger? logger)
    {
        var chunkList = GetChunkList(r);
        var chunks = ProcessChunks(node, nodeType, r, logger, chunkList);

        var headerChunks = (node as INodeHeader)?.HeaderChunks ?? HeaderChunks;
        
        foreach (var chunk in chunks)
        {
            headerChunks.Add(chunk);
        }
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

    private static Chunk ProcessChunk(Node node, Type nodeType, GameBoxReader r, ILogger? logger, uint chunkId, int size, bool isHeavy)
    {
        var classId = chunkId & 0xFFFFF000;

        if (isHeavy)
        {
            logger?.LogHeaderChunkHeavy(chunkId.ToString("X8"), size);
        }
        else
        {
            logger?.LogHeaderChunk(chunkId.ToString("X8"), size);
        }

        // Chunk data can be always read
        var chunkData = r.ReadBytes(size);

        var chunkNodeType = NodeManager.GetClassTypeById(classId);

        if (chunkNodeType is null)
        {
            logger?.LogHeaderChunkNodeNotImplemented(classId.ToString("X8"), NodeManager.GetName(classId) ?? "unknown class");

            return new HeaderChunk(chunkId, chunkData, isHeavy);
        }

        var headerChunk = NodeManager.GetNewHeaderChunk(chunkId);

        if (headerChunk is null)
        {
            var genericHeaderChunkType = typeof(HeaderChunk<>).MakeGenericType(chunkNodeType);

            var args = new object?[] { chunkData, chunkId, isHeavy };

            if (Activator.CreateInstance(genericHeaderChunkType, args) is not Chunk chunk)
            {
                throw new ThisShouldNotHappenException();
            }

            return chunk;
        }

        headerChunk.Data = chunkData;
        headerChunk.IsHeavy = isHeavy;

        if (chunkData.Length == 0)
        {
            return (Chunk)headerChunk;
        }

        using var chunkStream = new MemoryStream(chunkData);
        using var chunkReader = new GameBoxReader(chunkStream, r);
        var rw = new GameBoxReaderWriter(chunkReader);

        if (nodeType != chunkNodeType && !nodeType.IsSubclassOf(chunkNodeType))
        {
            // There are cast-related problems when one of the header chunks is not part of inheritance
            // For example, CGameCtnDecoration has a header chunk of type CPlugGameSkin that is not
            // inherited by CGameCtnDecoration. A ReadWrite without a node is called.

            try
            {
                headerChunk.ReadWrite(rw);
            }
            catch (NotSupportedException ex)
            {
                logger?.LogWarning(ex, "Exception with chunk 0x{chunkId}:", chunkId.ToString("X8"));
            }
        }
        else
        {
            headerChunk.ReadWrite(node, rw);
        }

        if (chunkStream.Position != chunkStream.Length)
        {
            logger?.LogWarning("Header chunk 0x{chunkId} has {chunkSize} bytes left.", chunkId.ToString("X8"), chunkStream.Length - chunkStream.Position);
        }

        if (GameBox.SeekForRawChunkData)
        {
            ((Chunk)headerChunk).RawData = chunkData; //
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
        using var r = new GameBoxReader(stream, logger);
        return Parse(r);
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
