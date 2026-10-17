using GBX.NET.Components;
using GBX.NET.Managers;
using Microsoft.Extensions.Logging;

namespace GBX.NET.Serialization;

internal sealed class GbxHeaderReader(GbxReader reader)
{
    private const int MaxUserDataSize = 0x1000000; // ~16MB

    private readonly ILogger? logger = reader.Logger;
    private GbxReadSettings Settings => reader.Settings;

    public GbxHeader Parse(out IClass? node)
    {
        logger?.LogDebug("Parsing header... (IMPLICIT, basic, UserData, number of nodes)");

        using var _ = logger?.BeginScope("Header");

        var basic = GbxHeaderBasic.Parse(reader);
        logger?.LogDebug("Basic: {Version} {Format} {RefTableCompression} {BodyCompression} {Mode}", basic.Version, basic.Format, basic.CompressionOfRefTable, basic.CompressionOfBody, basic.Mode);

        var classId = ReadClassId(reader);

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

        if (header.NumNodes < 0)
        {
            throw new InvalidDataException($"Number of nodes {header.NumNodes} is negative.");
        }

        if (header.NumNodes > short.MaxValue)
        {
            logger?.LogWarning("Number of nodes {NumNodes} exceeds 32767, which may cause the game to ignore the Gbx.", header.NumNodes);
        }

        return header;
    }

    private uint ReadClassId(GbxReader reader)
    {
        var rawClassId = reader.ReadHexUInt32();
        var classId = ClassManager.Wrap(rawClassId);

        if (rawClassId == classId)
        {
            logger?.LogInformation("Class ID: 0x{ClassId:X8} ({ClassName})", classId, ClassManager.GetName(classId) ?? "unknown class");
        }
        else
        {
            logger?.LogInformation("Class ID: 0x{ClassId:X8} (raw: 0x{RawClassId:X8}, {RawClassName} -> {ClassName})",
                classId, rawClassId,
                ClassManager.GetName(rawClassId) ?? "unknown class",
                ClassManager.GetName(classId) ?? "unknown class");

            reader.ClassIdRemapMode = rawClassId is 0x0301A000 // CGameCtnCollector in TMF
                ? ClassIdRemapMode.Id2008
                : ClassIdRemapMode.Id2006;
        }

        return classId;
    }

    internal bool ReadUserData(IClass? node, GbxHeaderUnknown? unknownHeader)
    {
        var userDataLength = reader.ReadInt32();

        if (userDataLength == 0)
        {
            logger?.LogDebug("UserData: Empty");
            return false;
        }

        var maxUserDataSize = Settings.MaxUserDataSize ?? MaxUserDataSize;

        if (userDataLength > maxUserDataSize)
        {
            throw new LengthLimitException($"User data size {userDataLength} exceeds maximum allowed size {maxUserDataSize}.");
        }

        if (Settings.OpenPlanetHookExtractMode)
        {
            logger?.LogDebug("UserData: {Length} bytes (read skipped - OpenPlanetHookExtractMode)", userDataLength);
            return false;
        }
        
        if (Settings.SkipUserData)
        {
            logger?.LogDebug("UserData: {Length} bytes (read skipped)", userDataLength);
            reader.SkipData(userDataLength);
            return false;
        }

        using var boundedStream = new BoundedStream(reader.BaseStream, userDataLength);
        using var r = new GbxReader(boundedStream);

        var numHeaderChunks = r.ReadInt32();

        if (numHeaderChunks < 0)
        {
            throw new InvalidDataException($"Number of header chunks {numHeaderChunks} is negative.");
        }

        if (numHeaderChunks > 255)
        {
            throw new InvalidDataException($"Number of header chunks {numHeaderChunks} exceeds maximum allowed (255).");
        }

        logger?.LogDebug("UserData: {Length} bytes, {NumChunks} header chunks", userDataLength, numHeaderChunks);

        if (numHeaderChunks == 0)
        {
            if (userDataLength > sizeof(int))
            {
                // Corrupted header extract scenarios
                logger?.LogWarning("UserData is zeroed, possibly corrupted header. (IMPLICIT)");
                r.SkipData(userDataLength - sizeof(int));
            }

            return false;
        }

        Span<HeaderChunkInfo> headerChunkInfos = stackalloc HeaderChunkInfo[numHeaderChunks];

        FillHeaderChunkInfo(headerChunkInfos, r);

        var nodeDict = default(Dictionary<uint, CMwNod>);

        foreach (var desc in headerChunkInfos)
        {
            var chunk = (node as CMwNod)?.NewHeaderChunk(desc.Id) ?? ClassManager.NewHeaderChunk(desc.Id);

            if (chunk is null)
            {
                logger?.LogWarning("Unknown header chunk 0x{ChunkId:X8} ({ClassName})!", desc.Id, ClassManager.GetName(desc.Id & 0xFFFFF000) ?? "unknown class");

                ReadAndAddUnknownHeaderChunk(r, node, unknownHeader, desc);
            }
            else
            {
                // If the class is unknown but the chunk ID is known (chunk ID collision here is near impossible)
                // Single node is shared for all chunks of the same class, except if it's abstract
                node ??= GetOrCreateNodeFromHeaderChunkInfo(desc, ref nodeDict);

                if (node.Chunks is ChunkSet chunkSet)
                {
                    chunkSet.AddInternal(chunk);
                }
                else
                {
                    node.Chunks.Add(chunk);
                }

                ReadKnownHeaderChunk(chunk, node, boundedStream, desc);

                // On unknown classes, add the chunk and reset this temporary node state
                if (unknownHeader is not null)
                {
                    chunk.Node = node;
                    unknownHeader.UserData.Add(chunk);
                    node.Chunks.Add(chunk);
                    node = null;
                }
            }
        }

        if (boundedStream.Remaining > 0)
        {
            throw new Exception($"Not all data was read from user data. {boundedStream.Remaining} bytes remaining.");
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
            throw new Exception($"Header chunk 0x{desc.Id:X8} requires a non-abstract CMwNod to read into.");
        }

        nodeDict.Add(classId, nod);
        return nod;
    }

    private void FillHeaderChunkInfo(Span<HeaderChunkInfo> headerChunkDescs, GbxReader reader)
    {
        var maxUserDataSize = Settings.MaxUserDataSize ?? MaxUserDataSize;

        var userDataSize = reader.BaseStream.Length;
        var totalSize = 4; // Includes the number of header chunks

        for (var i = 0; i < headerChunkDescs.Length; i++)
        {
            var rawChunkId = reader.ReadHexUInt32();
            var chunkSize = reader.ReadInt32();
            var actualChunkSize = (int)(chunkSize & ~0x80000000);
            var isHeavy = (chunkSize & 0x80000000) != 0;

            var chunkId = ClassManager.Wrap(rawChunkId);

            if (logger is not null)
            {
                var rawClassId = rawChunkId & 0xFFFFF000;
                var classId = chunkId & 0xFFFFF000;

                if (rawClassId == classId)
                {
                    logger.LogDebug("Chunk 0x{ChunkId:X8} ({ClassName}) (size: {Size}, heavy: {Heavy})", chunkId, ClassManager.GetName(classId) ?? "unknown class", actualChunkSize, isHeavy);
                }
                else
                {
                    logger.LogDebug("Chunk 0x{ChunkId:X8} (raw: 0x{RawChunkId:X8}, {RawClassName} -> {ClassName}) (size: {Size}, heavy: {Heavy})", 
                        chunkId, 
                        rawChunkId, 
                        ClassManager.GetName(rawClassId) ?? "unknown class", 
                        ClassManager.GetName(classId) ?? "unknown class", 
                        actualChunkSize, 
                        isHeavy);
                }
            }

            if (actualChunkSize > maxUserDataSize)
            {
                throw new LengthLimitException($"Header chunk size {actualChunkSize} exceeds maximum user data size {maxUserDataSize}.");
            }

            headerChunkDescs[i] = new HeaderChunkInfo(chunkId, actualChunkSize, isHeavy);

            // sizeof(uint) + sizeof(int) + actualChunkSize
            totalSize += 8 + actualChunkSize;

            if (totalSize > userDataSize)
            {
                throw new InvalidDataException($"Header chunk 0x{chunkId:X8} (size {actualChunkSize}) exceeds user data length ({totalSize} > {userDataSize}).");
            }
        }

        // Non-matching user data length will throw
        if (totalSize != userDataSize)
        {
            throw new InvalidDataException($"User data length {userDataSize} does not match actual data length {totalSize}.");
        }
    }

    private static void ReadAndAddUnknownHeaderChunk(GbxReader reader, IClass? node, GbxHeaderUnknown? unknownHeader, HeaderChunkInfo desc)
    {
        var chunk = new HeaderChunk(desc.Id)
        {
            IsHeavy = desc.IsHeavy,
            Data = reader.ReadBytes(desc.Size)
        };

        if (node is not null)
        {
            if (node.Chunks is ChunkSet chunkSet)
            {
                chunkSet.AddInternal(chunk);
            }
            else
            {
                node.Chunks.Add(chunk);
            }
        }
        else if (unknownHeader is not null)
        {
            if (unknownHeader.UserData is HeaderChunkSet headerChunkSet)
            {
                headerChunkSet.AddInternal(chunk);
            }
            else
            {
                unknownHeader.UserData.Add(chunk);
            }
        }
        else
        {
            throw new Exception($"Header chunk 0x{desc.Id:X8} cannot be stored anywhere.");
        }
    }

    private static void ReadKnownHeaderChunk(IHeaderChunk chunk, IClass node, BoundedStream stream, HeaderChunkInfo desc)
    {
        chunk.IsHeavy = desc.IsHeavy;

        using var boundedStream = new BoundedStream(stream, desc.Size);
        using var r = new GbxReader(boundedStream);

        switch (chunk)
        {
            case IReadableWritableChunk readableWritable:
                using (var rw = new GbxReaderWriter(r, leaveOpen: true))
                {
                    readableWritable.ReadWrite(chunk.Node ?? node, rw);
                }
                break;
            case IReadableChunk readable:
                readable.Read(chunk.Node ?? node, r);
                break;
            default:
                r.SkipData(desc.Size); // maybe let know?
                break;
        }

        if (boundedStream.Remaining > 0)
        {
            throw new Exception($"Not all data was read from header chunk 0x{chunk.Id:X8} ({ClassManager.GetName(chunk.Id)}). {boundedStream.Remaining} bytes remaining.");
        }
    }
}
