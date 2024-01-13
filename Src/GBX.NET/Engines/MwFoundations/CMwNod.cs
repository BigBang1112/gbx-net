using GBX.NET.Managers;

namespace GBX.NET.Engines.MwFoundations;

/// <remarks>ID: 0x01001000</remarks>
[Class(0x01001000)]
public partial class CMwNod : IClass
{
    public static uint Id => 0x01001000;

    private IChunkSet? chunks;
    public IChunkSet Chunks => chunks ??= new ChunkSet();

#if NET8_0_OR_GREATER
    static void IClass.Read<T>(T node, GbxReaderWriter rw)
    {
        var r = rw.Reader ?? throw new Exception("Reader is required but not available.");

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

                    throw new Exception($"Chunk ID 0x{chunkId:X8} cannot be processed");
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
                    default:
                        // TODO: possibility to skip
                        var chunkData = r.ReadBytes(chunkSize);
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
                    throw new Exception("Chunk cannot be processed");
            }
        }
    }
#endif

    internal virtual void Read(GbxReaderWriter rw)
    {
        var r = rw.Reader ?? throw new Exception("Reader is required but not available.");

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

                    throw new Exception("Chunk cannot be processed");
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
                    default:
                        // TODO: possibility to skip
                        var chunkData = r.ReadBytes(chunkSize);
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
                    throw new Exception("Chunk cannot be processed");
            }
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
                    throw new Exception("Chunk cannot be processed");
            }

            // Memory stream is not null only if chunk is skippable
            if (ms is not null)
            {
                w.Write((uint)ms.Length);
                ms.WriteTo(w.BaseStream);
            }
        }
    }

    internal virtual void ReadWrite(GbxReaderWriter rw)
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

    /// <inheritdoc />
    public virtual void ReadWrite(IGbxReaderWriter rw) => ReadWrite((GbxReaderWriter)rw);

    [Chunk(0x01001000)]
    public sealed class Chunk01001000 : Chunk<CMwNod>
    {
        public override uint Id => 0x01001000;

        public string? U01;

        internal override void Read(CMwNod n, GbxReader r)
        {
            U01 = r.ReadString();
        }

        internal override void Write(CMwNod n, GbxWriter w)
        {
            w.Write(U01);
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
