using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;

using GBX.NET.Engines.MwFoundations;

namespace GBX.NET
{
    public abstract class Chunk : IComparable<Chunk>
    {
        public virtual uint ID
        {
            get => ((ChunkAttribute)NodeCacheManager.AvailableChunkAttributesByType[GetType()]
                .First(x => x is ChunkAttribute)).ID;
        }
        /// <summary>
        /// Stream of unknown bytes
        /// </summary>
        [IgnoreDataMember]
        public MemoryStream Unknown { get; } = new MemoryStream();

        [IgnoreDataMember]
        public GameBox? GBX { get; set; }

        public CMwNod Node { get; internal set; }

        public int Progress { get; set; }

        protected Chunk(CMwNod node)
        {
            Node = node;
        }

        public override int GetHashCode()
        {
            return (int)ID;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Chunk);
        }

        public override string ToString()
        {
            return $"Chunk 0x{ID:X8}";
        }

        public bool Equals(Chunk? chunk) => chunk is not null && chunk.ID == ID;

        protected internal GameBoxReader CreateReader(Stream input)
        {
            if (this is ILookbackable l)
                return new GameBoxReader(input, l);
            return new GameBoxReader(input, GBX?.Body);
        }

        protected internal GameBoxWriter CreateWriter(Stream input)
        {
            if (this is ILookbackable l)
                return new GameBoxWriter(input, l);
            return new GameBoxWriter(input, GBX?.Body);
        }

        public static uint Remap(uint chunkID, IDRemap remap = IDRemap.Latest)
        {
            var classPart = chunkID & 0xFFFFF000;
            var chunkPart = chunkID & 0xFFF;

            switch(remap)
            {
                case IDRemap.Latest:
                    {
                        uint newClassPart = classPart;
                        while (NodeCacheManager.Mappings.TryGetValue(newClassPart, out uint newID))
                            newClassPart = newID;
                        return newClassPart + chunkPart;
                    }
                case IDRemap.TrackMania2006:
                    {
                        if (classPart == 0x03078000) // Not ideal solution
                            return 0x24061000 + chunkPart;
                        return NodeCacheManager.Mappings.LastOrDefault(x => x.Value == classPart).Key + chunkPart;
                    }
                default:
                    return chunkID;
            }
        }

        public virtual void OnLoad() { }

        public abstract void Read(CMwNod n, GameBoxReader r);
        public abstract void Write(CMwNod n, GameBoxWriter w);
        public abstract void ReadWrite(CMwNod n, GameBoxReaderWriter rw);

        public int CompareTo(Chunk other)
        {
            return ID.CompareTo(other.ID);
        }
    }

    public abstract class Chunk<T> : Chunk where T : CMwNod
    {
        [IgnoreDataMember]
        public new T Node
        {
            get => (T)base.Node;
            internal set => base.Node = value;
        }

        public bool IsHeader => this is IHeaderChunk;
        public bool IsBody => !IsHeader;

        protected Chunk() : base(null!)
        {

        }

        protected Chunk(T node) : base(node)
        {
            
        }

        public override void Read(CMwNod n, GameBoxReader r)
        {
            Read((T)n, r);
        }

        public override void Write(CMwNod n, GameBoxWriter w)
        {
            Write((T)n, w);
        }

        public override void ReadWrite(CMwNod n, GameBoxReaderWriter rw)
        {
            ReadWrite((T)n, rw);
        }

        public virtual void Read(T n, GameBoxReader r)
        {
            throw new NotImplementedException($"Chunk 0x{ID & 0xFFF:x3} from class {Node.ClassName} doesn't support Read.");
        }

        public virtual void Write(T n, GameBoxWriter w)
        {
            throw new NotImplementedException($"Chunk 0x{ID & 0xFFF:x3} from class {Node.ClassName} doesn't support Write.");
        }

        public virtual void ReadWrite(T n, GameBoxReaderWriter rw)
        {
            if (rw.Reader != null)
                Read(n, rw.Reader);
            else if (rw.Writer != null)
                Write(n, rw.Writer);
        }

        public byte[] ToByteArray()
        {
            if (this is ILookbackable l)
            {
                l.IdWritten = false;
                l.IdStrings.Clear();
            }

            using (var ms = new MemoryStream())
            using (var w = CreateWriter(ms))
            {
                var rw = new GameBoxReaderWriter(w);
                ReadWrite(Node, rw);
                return ms.ToArray();
            }
        }

        public override string ToString()
        {
            var desc = GetType().GetCustomAttribute<ChunkAttribute>().Description;
            return $"{typeof(T).Name} chunk 0x{ID:X8}{(string.IsNullOrEmpty(desc) ? "" : $" ({desc})")}";
        }
    }
}
