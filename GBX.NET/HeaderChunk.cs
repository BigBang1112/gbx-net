using System;
using System.Collections.Generic;
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
}
