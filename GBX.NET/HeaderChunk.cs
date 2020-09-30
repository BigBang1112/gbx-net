using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace GBX.NET
{
    public class HeaderChunk<T> : SkippableChunk<T>, IHeaderChunk where T : Node
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

        public virtual void ReadWrite(GameBoxReaderWriter rw)
        {
            if (rw.Reader != null)
            {
                var unknownW = new GameBoxWriter(Unknown, rw.Reader.Lookbackable);
                Read(rw.Reader, unknownW);
            }

            if (rw.Writer != null)
            {
                var unknownR = new GameBoxReader(Unknown, rw.Writer.Lookbackable);
                Write(rw.Writer, unknownR);
            }
        }

        public override void ReadWrite(T n, GameBoxReaderWriter rw)
        {
            ReadWrite(rw);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="r"></param>
        /// <param name="unknownW">Writer of the <see cref="Unknown"/> stream. This parameter mustn't be used inside loops - it can cause writing order problems whenever the loop changes!</param>
        public virtual void Read(GameBoxReader r, GameBoxWriter unknownW)
        {
            throw new NotImplementedException($"Header chunk 0x{ID & 0xFFF:x3} doesn't support Read.");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="w"></param>
        /// <param name="unknownR">Reader of the <see cref="Unknown"/> stream. This parameter mustn't be used inside loops - it can cause writing order problems whenever the loop changes!</param>
        public virtual void Write(GameBoxWriter w, GameBoxReader unknownR)
        {
            throw new NotImplementedException($"Header chunk 0x{ID & 0xFFF:x3} doesn't support Write.");
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

        public MemoryStream Stream { get; set; }
        public Node Node { get => throw new NotSupportedException(); set => throw new NotSupportedException(); }

        public int Progress
        {
            get => 0;
            set => throw new NotSupportedException("Cannot progress reading of an unknown header chunk.");
        }

        public bool IsHeavy { get; set; }

        public HeaderChunk(uint id, byte[] data)
        {
            this.id = id;
            Stream = new MemoryStream(data);
        }

        public override uint ID => id;

        public void Discover() => throw new NotSupportedException("Cannot discover an unknown header chunk.");

        public void ReadWrite(Node n, GameBoxReaderWriter rw) => ReadWrite(rw);

        public void ReadWrite(GameBoxReaderWriter rw)
        {
            if (rw.Writer != null)
                Write(rw.Writer);
        }

        public void Write(GameBoxWriter w) => w.Write(Stream.ToArray(), 0, (int)Stream.Length);
    }
}
