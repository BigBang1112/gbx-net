using GBX.NET.Components;

namespace GBX.NET.Serialization;

internal sealed class GbxHeaderWriter(GbxHeader header, GbxWriter writer, GbxWriteSettings settings)
{
    public bool Write(IClass? node)
    {
        _ = header.Basic.Write(writer);

        writer.Write(header.ClassId);

        if (header is GbxHeaderUnknown unknownHeader)
        {
            WriteUnknownHeaderUserData(unknownHeader);
        }
        else
        {
            WriteKnownHeaderUserData(node ?? throw new Exception("Node cannot be null for a known header, as it contains the header chunk instances."));
        }

        // NumNodes is handled elsewhere as unknown header doesnt change it while known header needs to first count it when writing body

        return true;
    }

    private void WriteUnknownHeaderUserData(GbxHeaderUnknown unknownHeader)
    {
        var isAllUnknownHeaderChunk = unknownHeader.UserData.All(x => x is HeaderChunk);

        if (!isAllUnknownHeaderChunk)
        {
            WritePartiallyKnownHeaderUserData(unknownHeader);
            return;
        }

        var headerChunks = unknownHeader.UserData.OfType<HeaderChunk>();

        var concatenatedDataLength = headerChunks.Select(x => x.Data.Length).Sum();

        var infosLength = unknownHeader.UserData.Count * sizeof(int) * 2 + sizeof(int); // +4 for the numHeaderChunks int
        var userDataLength = concatenatedDataLength + infosLength;

        writer.Write(userDataLength);
        writer.Write(unknownHeader.UserData.Count);

        foreach (var chunk in headerChunks)
        {
            var length = chunk.IsHeavy
                ? (uint)chunk.Data.Length + 0x80000000
                : (uint)chunk.Data.Length;

            writer.Write(chunk.Id);
            writer.Write(length);
        }

        foreach (var chunk in headerChunks)
        {
            writer.Write(chunk.Data);
        }
    }

    private void WritePartiallyKnownHeaderUserData(GbxHeaderUnknown unknownHeader)
    {
        using var concatenatedDataMs = new MemoryStream();
        using var concatenatedDataW = new GbxWriter(concatenatedDataMs);
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
        using var concatenatedDataMs = new MemoryStream();
        using var concatenatedDataW = new GbxWriter(concatenatedDataMs);
        using var concatenatedDataRw = new GbxReaderWriter(concatenatedDataW);

        var infos = new List<HeaderChunkInfo>();

        foreach (var chunk in node.Chunks.OfType<IHeaderChunk>())
        {
            var chunkStartPos = concatenatedDataMs.Position;

            WriteChunk(node, chunk, concatenatedDataRw);

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

    private void WriteUserData(List<HeaderChunkInfo> infos, MemoryStream concatenatedDataStream)
    {
        var infosLength = infos.Count * sizeof(int) * 2 + sizeof(int);
        var userDataLength = (int)concatenatedDataStream.Length + infosLength;

        writer.Write(userDataLength);
        writer.Write(infos.Count);

        foreach (var info in infos)
        {
            var size = info.IsHeavy
                ? (uint)info.Size + 0x80000000
                : (uint)info.Size;

            writer.Write(info.Id);
            writer.Write(size);
        }

        concatenatedDataStream.CopyTo(writer.BaseStream);
    }

    private void WriteChunk(IClass node, IHeaderChunk chunk, GbxReaderWriter rw)
    {
        switch (chunk)
        {
            case IReadableWritableChunk readableWritable:
                readableWritable.ReadWrite(node, rw);
                break;
            case IWritableChunk writable:
                writable.Write(node, writer);
                break;
            default:
                throw new Exception($"Unwritable chunk: {chunk.GetType().Name}");
        }
    }
}
