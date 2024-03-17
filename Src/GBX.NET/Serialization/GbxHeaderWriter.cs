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
            WriteUnknownHeaderUserData(writer, unknownHeader);
        }
        else
        {
            WriteKnownHeaderUserData(node ?? throw new Exception("Node cannot be null for a known header, as it contains the header chunk instances."));
        }

        // NumNodes is handled elsewhere as unknown header doesnt change it while known header needs to first count it when writing body

        return true;
    }

    private static void WriteUnknownHeaderUserData(GbxWriter writer, GbxHeaderUnknown unknownHeader)
    {
        var concatenatedDataLength = unknownHeader.UserData.Select(x => x.Data.Length).Sum();
        var infosLength = unknownHeader.UserData.Count * sizeof(int) * 2 + sizeof(int); // +4 for the numHeaderChunks int
        var userDataLength = concatenatedDataLength + infosLength;

        writer.Write(userDataLength);
        writer.Write(unknownHeader.UserData.Count);

        foreach (var chunk in unknownHeader.UserData)
        {
            var length = chunk.IsHeavy
                ? (uint)chunk.Data.Length + 0x80000000
                : (uint)chunk.Data.Length;

            writer.Write(chunk.Id);
            writer.Write(length);
        }

        foreach (var chunk in unknownHeader.UserData)
        {
            writer.Write(chunk.Data);
        }
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

        var infosLength = infos.Count * sizeof(int) * 2 + sizeof(int);
        var userDataLength = (int)concatenatedDataMs.Length + infosLength;

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

        concatenatedDataMs.CopyTo(writer.BaseStream);
    }

    private void WriteChunk(IClass node, IHeaderChunk chunk, GbxReaderWriter rw)
    {
        switch (chunk)
        {
            case IReadableWritableChunk readableWritable:
                readableWritable.ReadWrite(node, rw);
                break;
            case IWritableChunk readable:
                readable.Write(node, writer);
                break;
            default:
                throw new Exception($"Unwritable chunk: {chunk.GetType().Name}");
        }
    }
}
