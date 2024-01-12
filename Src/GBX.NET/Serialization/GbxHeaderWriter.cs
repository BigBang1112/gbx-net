﻿using GBX.NET.Components;
using GBX.NET.Managers;

namespace GBX.NET.Serialization;

internal sealed class GbxHeaderWriter(GbxHeader header, GbxReaderWriter readerWriter, GbxWriteSettings settings)
{
    private readonly GbxWriter writer = readerWriter.Writer ?? throw new Exception("Writer is required but not available.");

    public bool Write(IClass node)
    {
        var w = writer;

        _ = header.Basic.Write(w);

        writer.Write(ClassManager.Remap(header.ClassId, settings.ClassIdRemapMode));

        // WRONG
        if (header is GbxHeaderUnknown unknownHeader)
        {
            foreach (var chunk in unknownHeader.UserData)
            {
                WriteChunk(node, chunk);
            }
        }
        else
        {
            foreach (var chunk in node.Chunks.OfType<IHeaderChunk>())
            {
                WriteChunk(node, chunk);
            }
        }

        return true;
    }

    private void WriteChunk(IClass node, IHeaderChunk chunk)
    {
        switch (chunk)
        {
            case IReadableWritableChunk readableWritable:
                readableWritable.ReadWrite(node, readerWriter);
                break;
            case IWritableChunk readable:
                readable.Write(node, writer);
                break;
            default:
                throw new Exception($"Unwritable chunk: {chunk.GetType().Name}");
        }
    }
}