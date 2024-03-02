using GBX.NET.Managers;

namespace GBX.NET.Engines.MwFoundations;

/// <remarks>ID: 0x01001000</remarks>
[Class(0x01001000)]
public partial class CMwNod : IClass
{
    private IChunkSet? chunks;
    public IChunkSet Chunks => chunks ??= new ChunkSet();

#if NET8_0_OR_GREATER
    static void IClass.Read<T>(T node, GbxReaderWriter rw)
    {
        var r = rw.Reader ?? throw new Exception("Reader is required but not available.");

        var prevChunkId = default(uint?);

        while (true)
        {
            var originalChunkId = r.ReadHexUInt32();

            if (originalChunkId == 0xFACADE01)
            {
                return;
            }

            var chunkId = ClassManager.Wrap(originalChunkId);

            var chunk = node.CreateChunk(chunkId);

            // Unknown or skippable chunk
            if (chunk is null or ISkippableChunk)
            {
                var skip = r.ReadHexUInt32();

                if (skip != 0x534B4950)
                {
                    if (chunk is not null)
                    {
                        return;
                    }

                    throw new ChunkReadException(chunkId, prevChunkId, known: false);
                }

                var chunkSize = r.ReadInt32();

                switch (chunk)
                {
                    case IReadableWritableChunk<T> readableWritableT:
                        readableWritableT.ReadWrite(node, rw);
                        break;
                    case IReadableChunk<T> readableT:
                        readableT.Read(node, r);
                        break;
                    case IReadableWritableChunk readableWritable:
                        readableWritable.ReadWrite(node, rw);
                        break;
                    case IReadableChunk readable:
                        readable.Read(node, r);
                        break;
                    case ISkippableChunk skippable: // Known skippable but does not include reading/writing logic
                        // TODO: possibility to skip (not read into memory)
                        skippable.Data = r.ReadBytes(chunkSize);
                        break;
                    default: // Unknown skippable

                        // TODO: possibility to skip (not read into memory), maybe create typed skippable chunk in the future?
                        var skippableChunk = new SkippableChunk(chunkId)
                        {
                            Data = r.ReadBytes(chunkSize)
                        };

                        node.Chunks.Add(skippableChunk); // as its an unknown chunk, its not implicitly added by CreateChunk

                        break;
                }

                continue;
            }

            // Unskippable chunk
            switch (chunk)
            {
                case IReadableWritableChunk<T> readableWritableT:
                    readableWritableT.ReadWrite(node, rw);
                    break;
                case IReadableChunk<T> readableT:
                    readableT.Read(node, r);
                    break;
                case IReadableWritableChunk readableWritable:
                    readableWritable.ReadWrite(node, rw);
                    break;
                case IReadableChunk readable:
                    readable.Read(node, r);
                    break;
                default:
                    throw new ChunkReadException(chunkId, prevChunkId, known: true);
            }

            prevChunkId = chunkId;
        }
    }
#endif

    internal virtual void Read(GbxReaderWriter rw)
    {
        var r = rw.Reader ?? throw new Exception("Reader is required but not available.");

        var prevChunkId = default(uint?);

        while (true)
        {
            var originalChunkId = r.ReadHexUInt32();

            if (originalChunkId == 0xFACADE01)
            {
                return;
            }

            var chunkId = ClassManager.Wrap(originalChunkId);

            var chunk = CreateChunk(chunkId);

            // Unknown or skippable chunk
            if (chunk is null or ISkippableChunk)
            {
                var skip = r.ReadHexUInt32();

                if (skip != 0x534B4950)
                {
                    if (chunk is not null)
                    {
                        return;
                    }

                    throw new ChunkReadException(chunkId, prevChunkId, known: false);
                }

                var chunkSize = r.ReadInt32();

                switch (chunk)
                {
                    case IReadableWritableChunk readableWritable:
                        readableWritable.ReadWrite(this, rw);
                        // TODO: validate chunk size
                        break;
                    case IReadableChunk readable:
                        readable.Read(this, r);
                        // TODO: validate chunk size
                        break;
                    case ISkippableChunk skippable: // Known skippable but does not include reading/writing logic
                        // TODO: possibility to skip (not read into memory)
                        skippable.Data = r.ReadBytes(chunkSize);
                        break;
                    default: // Unknown skippable

                        // TODO: possibility to skip (not read into memory), maybe create typed skippable chunk in the future?
                        var skippableChunk = new SkippableChunk(chunkId)
                        {
                            Data = r.ReadBytes(chunkSize)
                        };

                        Chunks.Add(skippableChunk); // as its an unknown chunk, its not implicitly added by CreateChunk

                        break;
                }

                continue;
            }

            // Unskippable chunk
            switch (chunk)
            {
                case IReadableWritableChunk readableWritable:
                    readableWritable.ReadWrite(this, rw);
                    break;
                case IReadableChunk readable:
                    readable.Read(this, r);
                    break;
                default:
                    throw new ChunkReadException(chunkId, prevChunkId, known: true);
            }

            prevChunkId = chunkId;
        }
    }

    internal virtual void Write(GbxReaderWriter rw)
    {
        var w = rw.Writer ?? throw new Exception("Writer is required but not available.");

        foreach (var chunk in Chunks)
        {
            w.WriteHexUInt32(chunk.Id);

            var chunkW = w;
            var chunkRw = rw;

            var ms = default(MemoryStream);

            if (chunk is ISkippableChunk)
            {
                w.WriteHexUInt32(0x534B4950);

                ms = new MemoryStream();
                chunkW = new GbxWriter(ms);
                chunkRw = new GbxReaderWriter(chunkW);
            }

            switch (chunk)
            {
                case IReadableWritableChunk readableWritable:
                    readableWritable.ReadWrite(this, chunkRw);
                    break;
                case IWritableChunk writable:
                    writable.Write(this, chunkW);
                    break;
                default:
                    throw new Exception($"Chunk (ID 0x{chunk.Id:X8}, {ClassManager.GetName(chunk.Id & 0xFFFFF000)}) cannot be processed");
            }

            // Memory stream is not null only if chunk is skippable
            if (ms is not null)
            {
                w.Write((uint)ms.Length);
                ms.WriteTo(w.BaseStream);
            }
        }
    }

    /// <inheritdoc />
    public virtual void ReadWrite(GbxReaderWriter rw)
    {
        if (rw.Reader is not null)
        {
            Read(rw);
        }

        if (rw.Writer is not null)
        {
            Write(rw);
        }
    }

    public virtual IHeaderChunk? CreateHeaderChunk(uint chunkId)
    {
        return null;
    }

    public virtual IChunk? CreateChunk(uint chunkId)
    {
        var chunk = chunkId switch
        {
            0x01001000 => new Chunk01001000(),
            _ => null
        };

        if (chunk is not null)
        {
            Chunks.Add(chunk);
        }

        return chunk;
    }

    public virtual IClass DeepClone()
    {
        var clone = new CMwNod();
        DeepCloneChunks(clone);
        return clone;
    }

    protected void DeepCloneChunks(CMwNod dest)
    {
        if (chunks is null)
        {
            return;
        }
        
        foreach (var chunk in chunks)
        {
            var chunkClone = chunk.DeepClone();
            dest.Chunks.Add(chunkClone);
        }
    }
}
