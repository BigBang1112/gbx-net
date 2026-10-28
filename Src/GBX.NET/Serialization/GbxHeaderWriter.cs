using GBX.NET.Components;
using GBX.NET.Managers;
using Microsoft.Extensions.Logging;

namespace GBX.NET.Serialization;

internal sealed class GbxHeaderWriter(GbxHeader header, GbxWriter writer)
{
    private readonly ILogger? logger = writer.Logger;

    public bool Write(IClass? node)
    {
        logger?.LogDebug("Writing header... (IMPLICIT, basic, UserData)");

        using var _ = logger?.BeginScope("Header");

        var basic = header.Basic;
        basic.Write(writer);
        logger?.LogDebug("Basic: {Version} {Format} {RefTableCompression} {BodyCompression} {Mode}", basic.Version, basic.Format, basic.CompressionOfRefTable, basic.CompressionOfBody, basic.Mode);

        if (node?.IsWriteSupported == false)
        {
            throw new ClassWriteNotSupportedException(header.ClassId);
        }

        WriteClassId();

        if (header is GbxHeaderUnknown unknownHeader)
        {
            logger?.LogInformation("Unknown class, writing GbxHeaderUnknown...");
            WriteUnknownHeaderUserData(unknownHeader);
        }
        else
        {
            logger?.LogInformation("Known class!");
            WriteKnownHeaderUserData(node ?? throw new Exception("Node cannot be null for a known header, as it contains the header chunk instances."));
        }

        // NumNodes is handled elsewhere as unknown header doesnt change it while known header needs to first count it when writing body

        return true;
    }

    private void WriteClassId()
    {
        var classId = header.ClassId;
        var rawClassId = writer.WriteClassId(classId);

        if (rawClassId == classId)
        {
            logger?.LogInformation("Class ID: 0x{ClassId:X8} ({ClassName})", classId, ClassManager.GetName(classId) ?? "unknown class");
        }
        else
        {
            logger?.LogInformation("Class ID: 0x{ClassId:X8} (raw: 0x{RawClassId:X8}, {ClassName} -> {RawClassName})",
                classId, rawClassId,
                ClassManager.GetName(classId) ?? "unknown class",
                ClassManager.GetName(rawClassId) ?? "unknown class");
        }
    }

    private void WriteUnknownHeaderUserData(GbxHeaderUnknown unknownHeader)
    {
        if (unknownHeader.UserData.Count == 0)
        {
            logger?.LogDebug("UserData: Empty");
            writer.Write(0);
            return;
        }

        var isAllUnknownHeaderChunk = unknownHeader.UserData.All(x => x is HeaderChunk);

        if (!isAllUnknownHeaderChunk)
        {
            WritePartiallyKnownHeaderUserData(unknownHeader);
            return;
        }

        var headerChunks = unknownHeader.UserData.OfType<HeaderChunk>();

        var concatenatedDataLength = headerChunks.Sum(x => x.Data.Length);

        var infosLength = unknownHeader.UserData.Count * sizeof(int) * 2 + sizeof(int); // +4 for the numHeaderChunks int
        var userDataLength = concatenatedDataLength + infosLength;

        writer.Write(userDataLength);
        writer.Write(unknownHeader.UserData.Count);

        logger?.LogDebug("UserData: {Length} bytes, {NumChunks} header chunks", userDataLength, unknownHeader.UserData.Count);

        foreach (var chunk in headerChunks)
        {
            var length = chunk.IsHeavy
                ? (uint)chunk.Data.Length + 0x80000000
                : (uint)chunk.Data.Length;

            var rawChunkId = writer.WriteChunkId(chunk.Id);
            writer.Write(length);

            LogChunk(chunk.Id, rawChunkId, chunk.Data.Length, chunk.IsHeavy);
        }

        foreach (var chunk in headerChunks)
        {
            writer.Write(chunk.Data);
        }
    }

    private void WritePartiallyKnownHeaderUserData(GbxHeaderUnknown unknownHeader)
    {
        using var concatenatedDataMs = new MemoryStream();
        using var concatenatedDataW = new GbxWriter(concatenatedDataMs, writer.Settings with { CloseStream = false });
        using var concatenatedDataRw = new GbxReaderWriter(concatenatedDataW);

        var infos = new List<HeaderChunkInfo>();

        foreach (var chunk in unknownHeader.UserData)
        {
            if (chunk is HeaderChunk unknownHeaderChunk)
            {
                concatenatedDataW.Write(unknownHeaderChunk.Data);

                infos.Add(new HeaderChunkInfo
                {
                    Id = chunk.Id,
                    Size = unknownHeaderChunk.Data.Length,
                    IsHeavy = chunk.IsHeavy
                });

                continue;
            }

            if (chunk.Node is null)
            {
                throw new Exception("Node cannot be null for an unknown header on a known header chunk.");
            }

            var chunkStartPos = concatenatedDataMs.Position;

            WriteChunk(chunk.Node, chunk, concatenatedDataRw);

            infos.Add(new HeaderChunkInfo
            {
                Id = chunk.Id,
                Size = (int)(concatenatedDataMs.Position - chunkStartPos),
                IsHeavy = chunk.IsHeavy
            });
        }

        concatenatedDataMs.Position = 0;

        WriteUserData(infos, concatenatedDataMs);
    }

    private void WriteKnownHeaderUserData(IClass node)
    {
        if (!node.Chunks.OfType<IHeaderChunk>().Any())
        {
            logger?.LogDebug("UserData: Empty");
            writer.Write(0);
            return;
        }

        using var concatenatedDataMs = new MemoryStream();
        using var concatenatedDataW = new GbxWriter(concatenatedDataMs, writer.Settings with { CloseStream = false });
        using var concatenatedDataRw = new GbxReaderWriter(concatenatedDataW);

        var infos = new List<HeaderChunkInfo>();

        foreach (var chunk in node.Chunks.OfType<IHeaderChunk>())
        {
            int size;

            if (chunk is HeaderChunk unknownHeaderChunk)
            {
                concatenatedDataW.Write(unknownHeaderChunk.Data);
                size = unknownHeaderChunk.Data.Length;
            }
            else
            {
                var chunkStartPos = concatenatedDataMs.Position;
                WriteChunk(chunk.Node ?? node, chunk, concatenatedDataRw);
                size = (int)(concatenatedDataMs.Position - chunkStartPos);
            }

            infos.Add(new HeaderChunkInfo
            {
                Id = chunk.Id,
                Size = size,
                IsHeavy = chunk.IsHeavy
            });
        }

        concatenatedDataMs.Position = 0;

        WriteUserData(infos, concatenatedDataMs);
    }

    private void WriteUserData(List<HeaderChunkInfo> chunkInfos, MemoryStream concatenatedDataStream)
    {
        var infosLength = chunkInfos.Count * sizeof(int) * 2 + sizeof(int);
        var userDataLength = (int)concatenatedDataStream.Length + infosLength;

        writer.Write(userDataLength);
        writer.Write(chunkInfos.Count);

        logger?.LogDebug("UserData: {Length} bytes, {NumChunks} header chunks", userDataLength, chunkInfos.Count);

        foreach (var chunk in chunkInfos)
        {
            var size = chunk.IsHeavy
                ? (uint)chunk.Size + 0x80000000
                : (uint)chunk.Size;

            var rawChunkId = writer.WriteChunkId(chunk.Id);
            writer.Write(size);

            LogChunk(chunk.Id, rawChunkId, chunk.Size, chunk.IsHeavy);
        }

        concatenatedDataStream.CopyTo(writer.BaseStream);
    }

    private void LogChunk(uint chunkId, uint rawChunkId, int size, bool isHeavy)
    {
        if (logger is null)
        {
            return;
        }

        var classId = chunkId & 0xFFFFF000;
        var rawClassId = rawChunkId & 0xFFFFF000;

        if (rawClassId == classId)
        {
            logger.LogDebug("Chunk 0x{ChunkId:X8} ({ClassName}) (size: {Size}, heavy: {Heavy})", chunkId, ClassManager.GetName(classId) ?? "unknown class", size, isHeavy);
        }
        else
        {
            logger.LogDebug("Chunk 0x{ChunkId:X8} (raw: 0x{RawChunkId:X8}, {ClassName} -> {RawClassName}) (size: {Size}, heavy: {Heavy})",
                chunkId,
                rawChunkId,
                ClassManager.GetName(classId) ?? "unknown class",
                ClassManager.GetName(rawClassId) ?? "unknown class",
                size,
                isHeavy);
        }
    }

    private static void WriteChunk(IClass node, IHeaderChunk chunk, GbxReaderWriter rw)
    {
        switch (chunk)
        {
            case IReadableWritableChunk readableWritable:
                readableWritable.ReadWrite(node, rw);
                break;
            case IWritableChunk writable:
                writable.Write(node, rw.Writer!);
                break;
            default:
                throw new Exception($"Unwritable chunk: {chunk.GetType().Name}");
        }
    }
}
