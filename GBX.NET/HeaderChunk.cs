using System;
using System.Reflection;

using GBX.NET.Engines.MwFoundations;

namespace GBX.NET
{
    public class HeaderChunk<T> : SkippableChunk<T>, IHeaderChunk where T : CMwNod
    {
        public bool IsHeavy { get; set; }

        public HeaderChunk()
        {

        }

        public HeaderChunk(T node, byte[] data) : base(node, data)
        {

        }

        public HeaderChunk(T node, uint id, byte[] data) : base(node, id, data)
        {

        }

        public override void ReadWrite(T n, GameBoxReaderWriter rw)
        {
            if (rw.Reader != null)
                Read(n, rw.Reader);

            if (rw.Writer != null)
                Write(n, rw.Writer);
        }

        public override string ToString()
        {
            var desc = GetType().GetCustomAttribute<ChunkAttribute>().Description;
            return $"{typeof(T).Name} header chunk 0x{ID:X8}{(string.IsNullOrEmpty(desc) ? "" : $" ({desc})")}";
        }
    }

    public sealed class HeaderChunk : Chunk, ISkippableChunk, IHeaderChunk
    {
        readonly uint id;

        public bool Discovered
        {
            get => false;
            set => throw new NotSupportedException("Cannot discover an unknown header chunk.");
        }

        public byte[] Data { get; set; }
        public CMwNod Node { get => throw new NotSupportedException(); set => throw new NotSupportedException(); }

        public int Progress
        {
            get => 0;
            set => throw new NotSupportedException("Cannot progress reading of an unknown header chunk.");
        }

        public bool IsHeavy { get; set; }

        public HeaderChunk(uint id, byte[] data)
        {
            this.id = id;
            Data = data;
        }

        public override uint ID => id;

        public void Discover() => throw new NotSupportedException("Cannot discover an unknown header chunk.");

        public void ReadWrite(GameBoxReaderWriter rw)
        {
            if (rw.Writer != null)
                Write(rw.Writer);
        }

        public void Write(GameBoxWriter w) => w.WriteBytes(Data);

        public override string ToString()
        {
            return $"Header chunk 0x{ID:X8}";
        }

        public override void Read(CMwNod n, GameBoxReader r)
        {
            throw new NotSupportedException();
        }

        public override void Write(CMwNod n, GameBoxWriter w)
        {
            throw new NotSupportedException();
        }

        public override void ReadWrite(CMwNod n, GameBoxReaderWriter rw)
        {
            throw new NotSupportedException();
        }
    }
}
