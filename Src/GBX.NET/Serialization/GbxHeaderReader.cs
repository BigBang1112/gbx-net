using GBX.NET.Components;
using GBX.NET.Managers;
using Microsoft.Extensions.Logging;

namespace GBX.NET.Serialization;

internal sealed class GbxHeaderReader(GbxReader reader, GbxReadSettings settings)
{
    private readonly ILogger? logger = settings.Logger;

    public GbxHeader<T> Parse<T>(out T node) where T : notnull, IClass, new()
    {
        logger?.LogDebug("Parsing header... (EXPLICIT, basic, UserData, number of nodes)");

        using var _ = logger?.BeginScope("Header");

        var basic = GbxHeaderBasic.Parse(reader);
        logger?.LogDebug("Basic: {Version} {Format} {RefTableCompression} {BodyCompression} {UnknownByte}", basic.Version, basic.Format, basic.CompressionOfRefTable, basic.CompressionOfBody, basic.UnknownByte);

        var rawClassId = reader.ReadHexUInt32();
        var classId = ClassManager.Wrap(rawClassId);
        logger?.LogDebug("Class ID: 0x{ClassId:X8} ({ClassName}, raw: 0x{RawClassId:X8})", classId, ClassManager.GetName(classId), rawClassId);

        var expectedClassId = ClassManager.GetClassId<T>();

        if (classId != expectedClassId)
        {
            throw new InvalidCastException($"Class ID 0x{classId:X8} ({ClassManager.GetName(classId) ?? "unknown class name"}) does not match expected class ID 0x{expectedClassId:X8} ({typeof(T).Name}).");
        }

        node = new T();

        logger?.LogInformation("Instantiated EXPLICIT node: {Node}", node);

        if (basic.Version >= 6)
        {
            ReadUserData(node);
        }

        var numNodes = reader.ReadInt32();
        logger?.LogDebug("Number of nodes: {NumNodes}", numNodes);

        return new GbxHeader<T>(basic)
        {
            NumNodes = numNodes
        };
    }

    public GbxHeader Parse(out IClass? node)
    {
        logger?.LogDebug("Parsing header... (IMPLICIT, basic, UserData, number of nodes)");

        using var _ = logger?.BeginScope("Header");

        var basic = GbxHeaderBasic.Parse(reader);
        logger?.LogDebug("Basic: {Version} {Format} {RefTableCompression} {BodyCompression} {UnknownByte}", basic.Version, basic.Format, basic.CompressionOfRefTable, basic.CompressionOfBody, basic.UnknownByte);

        var rawClassId = reader.ReadHexUInt32();
        var classId = ClassManager.Wrap(rawClassId);
        logger?.LogDebug("Class ID: 0x{ClassId:X8} ({ClassName}, raw: 0x{RawClassId:X8})", classId, ClassManager.GetName(classId), rawClassId);

        node = ClassManager.New(classId);

        GbxHeader header;

        if (node is null)
        {
            header = new GbxHeaderUnknown(basic, classId);
            logger?.LogInformation("Unknown class, using GbxHeaderUnknown...");
        }
        else
        {
            header = ClassManager.NewHeader(basic, classId) ?? new GbxHeaderUnknown(basic, classId);
            logger?.LogInformation("Known class!");
        }

        if (basic.Version >= 6)
        {
            ReadUserData(node, header as GbxHeaderUnknown);
        }

        header.NumNodes = reader.ReadInt32();
        logger?.LogDebug("Number of nodes: {NumNodes}", header.NumNodes);

        return header;
    }

    internal bool ReadUserData<T>(T node) where T : notnull, IClass
    {
        var userDataNums = ValidateUserDataNumbers();

        if (userDataNums.Length == 0)
        {
            return false;
        }

        if (userDataNums.NumChunks == 0)
        {
            if (userDataNums.Length > sizeof(int))
            {
                // Corrupted header extract scenarios
                logger?.LogWarning("UserData is zeroed, possibly corrupted header. (EXPLICIT)");
                reader.SkipData(userDataNums.Length - sizeof(int));
            }

            return false;
        }

        using var readerWriter = new GbxReaderWriter(reader, leaveOpen: true);

        Span<HeaderChunkInfo> headerChunkInfos = stackalloc HeaderChunkInfo[userDataNums.NumChunks];

        FillHeaderChunkInfo(headerChunkInfos, userDataNums);

        foreach (var desc in headerChunkInfos)
        {
            reader.Limit(desc.Size);

            var chunk = node.CreateHeaderChunk(desc.Id);

            if (chunk is null)
            {
                ReadAndAddUnknownHeaderChunk(node, unknownHeader: null, desc);
            }
            else
            {
                ReadKnownHeaderChunk(chunk, node, readerWriter, desc);
            }

            reader.Unlimit(skipToLimitWhenUnreached: settings.SkipUnclearedHeaderChunkBuffers);
        }

        return true;
    }

    internal bool ReadUserData(IClass? node, GbxHeaderUnknown? unknownHeader)
    {
        var userDataNums = ValidateUserDataNumbers();

        if (userDataNums.Length == 0)
        {
            return false;
        }

        if (userDataNums.NumChunks == 0)
        {
            if (userDataNums.Length > sizeof(int))
            {
                // Corrupted header extract scenarios
                logger?.LogWarning("UserData is zeroed, possibly corrupted header. (IMPLICIT)");
                reader.SkipData(userDataNums.Length - sizeof(int));
            }

            return false;
        }

        using var readerWriter = new GbxReaderWriter(reader, leaveOpen: true);

        Span<HeaderChunkInfo> headerChunkInfos = stackalloc HeaderChunkInfo[userDataNums.NumChunks];

        FillHeaderChunkInfo(headerChunkInfos, userDataNums);

        var nodeDict = default(Dictionary<uint, CMwNod>);

        foreach (var desc in headerChunkInfos)
        {
            reader.Limit(desc.Size);

            var chunk = node?.CreateHeaderChunk(desc.Id) ?? ClassManager.NewHeaderChunk(desc.Id);

            if (chunk is null)
            {
                ReadAndAddUnknownHeaderChunk(node, unknownHeader, desc);
            }
            else
            {
                // If the class is unknown but the chunk ID is known (chunk ID collision here is near impossible)
                // Single node is shared for all chunks of the same class, except if it's abstract
                node ??= GetOrCreateNodeFromHeaderChunkInfo(desc, ref nodeDict);

                ReadKnownHeaderChunk(chunk, node, readerWriter, desc);

                // On unknown classes, add the chunk and reset this temporary node state
                if (unknownHeader is not null)
                {
                    chunk.Node = node;
                    unknownHeader.UserData.Add(chunk);
                    node.Chunks.Add(chunk);
                    node = null;
                }
            }

            reader.Unlimit(skipToLimitWhenUnreached: settings.SkipUnclearedHeaderChunkBuffers);
        }

        return true;
    }

    private static CMwNod GetOrCreateNodeFromHeaderChunkInfo(HeaderChunkInfo desc, ref Dictionary<uint, CMwNod>? nodeDict)
    {
        nodeDict ??= new(capacity: 3);

        var classId = desc.Id & 0xFFFFF000;

        if (nodeDict.TryGetValue(classId, out var existingNod))
        {
            return existingNod;
        }

        if (ClassManager.New(classId) is not CMwNod nod)
        {
            throw new Exception($"Chunk 0x{desc.Id:X8} requires a non-abstract CMwNod to read into.");
        }

        nodeDict.Add(classId, nod);
        return nod;
    }

    internal void FillHeaderChunkInfo(Span<HeaderChunkInfo> headerChunkDescs, UserDataNumbers userDataNums)
    {
        var totalSize = 4; // Includes the number of header chunks

        for (var i = 0; i < userDataNums.NumChunks; i++)
        {
            var chunkId = reader.ReadHexUInt32();
            var chunkSize = reader.ReadInt32();
            var actualChunkSize = (int)(chunkSize & ~0x80000000);
            var isHeavy = (chunkSize & 0x80000000) != 0;

            logger?.LogDebug("Chunk 0x{ChunkId:X8} (Size: {Size}, Heavy: {Heavy})", chunkId, actualChunkSize, isHeavy);

            if (actualChunkSize > GbxReader.MaxDataSize)
            {
                throw new LengthLimitException($"Header chunk size {actualChunkSize} exceeds maximum data size {GbxReader.MaxDataSize}.");
            }

            var mappedChunkId = ClassManager.Wrap(chunkId);

            headerChunkDescs[i] = new HeaderChunkInfo(mappedChunkId, actualChunkSize, isHeavy);

            // sizeof(uint) + sizeof(int) + actualChunkSize
            totalSize += 8 + actualChunkSize;

            if (totalSize > userDataNums.Length)
            {
                throw new InvalidDataException($"Header chunk 0x{mappedChunkId:X8} (size {actualChunkSize}) exceeds user data length ({totalSize} > {userDataNums.Length}).");
            }
        }

        // Non-matching user data length will throw
        if (totalSize != userDataNums.Length)
        {
            throw new InvalidDataException($"User data length {userDataNums.Length} does not match actual data length {totalSize}.");
        }
    }

    /// <summary>
    /// Reads the user data length and the number of header chunks, also validating the values and returning them as <see cref="UserDataNumbers"/>.
    /// </summary>
    /// <returns></returns>
    /// <exception cref="LengthLimitException"></exception>
    internal UserDataNumbers ValidateUserDataNumbers()
    {
        var userDataLength = reader.ReadInt32();

        if (userDataLength == 0)
        {
            logger?.LogDebug("UserData: Empty");
            return new UserDataNumbers(0, 0);
        }

        // Maybe should be much stricter... and configurable
        if (userDataLength > GbxReader.MaxDataSize || (settings.MaxUserDataSize.HasValue && userDataLength > settings.MaxUserDataSize.Value))
        {
            throw new LengthLimitException($"User data size {userDataLength} exceeds maximum allowed size {GbxReader.MaxDataSize}.");
        }

        // The idea is to preferably not create sub-buffers to reduce pressure on the GC.
        // If SkipUserData is true, it can be faster to reach the next parse stage (reference table):
        // - if seeking is supported, position moves past the user data
        // - if seeking is NOT supported:
        //   - .NET Standard 2.0: unavoidable byte array allocation with ReadBytes
        //   - .NET 6+: no allocation with Read(stackalloc byte[])

        if (settings.OpenPlanetHookExtractMode)
        {
            logger?.LogDebug("UserData: {Length} bytes (read skipped - OpenPlanetHookExtractMode)", userDataLength);
            return new UserDataNumbers(0, NumChunks: 0);
        }
        else if (settings.SkipUserData)
        {
            logger?.LogDebug("UserData: {Length} bytes (read skipped)", userDataLength);
            reader.SkipData(userDataLength);
            return new UserDataNumbers(userDataLength, NumChunks: 0);
        }

        // Header chunk count
        var numHeaderChunks = reader.ReadInt32();

        if (numHeaderChunks < 0)
        {
            throw new InvalidDataException($"Number of header chunks {numHeaderChunks} is negative.");
        }

        logger?.LogDebug("UserData: {Length} bytes, {NumChunks} header chunks", userDataLength, numHeaderChunks);

        return new UserDataNumbers(userDataLength, numHeaderChunks);
    }

    private void ReadAndAddUnknownHeaderChunk(IClass? node, GbxHeaderUnknown? unknownHeader, HeaderChunkInfo desc)
    {
        var chunk = new HeaderChunk(desc.Id)
        {
            IsHeavy = desc.IsHeavy,
            Data = reader.ReadBytes(desc.Size)
        };

        if (node is not null)
        {
            node.Chunks.Add(chunk);
        }
        else if (unknownHeader is not null)
        {
            unknownHeader.UserData.Add(chunk);
        }
        else
        {
            throw new Exception($"Chunk 0x{desc.Id:X8} cannot be stored anywhere.");
        }
    }

    private void ReadKnownHeaderChunk<T>(IHeaderChunk chunk, T node, GbxReaderWriter rw, HeaderChunkInfo desc)
        where T : notnull, IClass
    {
        chunk.IsHeavy = desc.IsHeavy;

        switch (chunk)
        {
            case IReadableWritableChunk<T> readableWritableT:
                readableWritableT.ReadWrite(node, rw);
                break;
            case IReadableChunk<T> readableT:
                readableT.Read(node, reader);
                break;
            case IReadableWritableChunk readableWritable:
                readableWritable.ReadWrite(chunk.Node ?? node, rw);
                break;
            case IReadableChunk readable:
                readable.Read(chunk.Node ?? node, reader);
                break;
            default:
                reader.SkipData(desc.Size);
                break;
        }
    }

    private void ReadKnownHeaderChunk(IHeaderChunk chunk, IClass node, GbxReaderWriter rw, HeaderChunkInfo desc)
    {
        chunk.IsHeavy = desc.IsHeavy;

        switch (chunk)
        {
            case IReadableWritableChunk readableWritable:
                readableWritable.ReadWrite(chunk.Node ?? node, rw);
                break;
            case IReadableChunk readable:
                readable.Read(chunk.Node ?? node, reader);
                break;
            default:
                reader.SkipData(desc.Size); // maybe let know?
                break;
        }
    }
}
