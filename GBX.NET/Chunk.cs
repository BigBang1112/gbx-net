using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;

namespace GBX.NET
{
    public abstract class Chunk : IComparable<Chunk>
    {
        public virtual uint ID => GetType().GetCustomAttribute<ChunkAttribute>().ID;
        /// <summary>
        /// Stream of unknown bytes
        /// </summary>
        [IgnoreDataMember]
        public UnknownStream Unknown { get; } = new UnknownStream();

        /// <summary>
        /// A virtual property usable to parse unknown data from the <see cref="Unknown"/> stream.
        /// </summary>
        [IgnoreDataMember]
        public virtual object[] UnknownValues
        {
            get => throw new NotImplementedException($"Chunk 0x{ID & 0xFFF:x3} doesn't support UnknownValues.");
        }

        [IgnoreDataMember]
        public virtual ILookbackable Lookbackable
        {
            get
            {
                if (this is ILookbackable l)
                    return l;
                return Part;
            }
        }

        [IgnoreDataMember]
        public GameBoxPart Part { get; set; }

        public Chunk()
        {

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

        public bool Equals(Chunk chunk) => chunk != null && chunk.ID == ID;

        public static uint Remap(uint chunkID, ClassIDRemap remap = ClassIDRemap.Latest)
        {
            var classPart = chunkID & 0xFFFFF000;
            var chunkPart = chunkID & 0xFFF;

            switch(remap)
            {
                case ClassIDRemap.ManiaPlanet:
                    if (Node.Mappings.TryGetValue(classPart, out uint newID))
                        return newID + chunkPart;
                    return chunkID;
                case ClassIDRemap.TrackMania:
                    if (classPart == 0x03078000) // Not ideal solution
                        return 0x24061000 + chunkPart;
                    return Node.Mappings.LastOrDefault(x => x.Value == classPart).Key + chunkPart;
                default:
                    return chunkID;
            }
        }

        public virtual GameBoxReader OpenUnknownStream()
        {
            return new GameBoxReader(Unknown, null);
        }

        public virtual void OnLoad() { }

        public int CompareTo(Chunk other)
        {
            return ID.CompareTo(other.ID);
        }
    }

    public abstract class Chunk<T> : Chunk, IChunk where T : Node
    {
        [IgnoreDataMember]
        public T Node { get; internal set; }
        public int Progress { get; set; }

        Node IChunk.Node
        {
            get => Node;
            set => Node = (T)value;
        }

        public bool IsHeader => Lookbackable is GameBoxHeader;
        public bool IsBody => Lookbackable is IGameBoxBody;

        [IgnoreDataMember]
        public override ILookbackable Lookbackable
        {
            get
            {
                if (Node?.ParentChunk is ILookbackable l)
                    return l;
                return base.Lookbackable;
            }
        }

        [IgnoreDataMember]
        public override object[] UnknownValues
        {
            get => throw new NotImplementedException($"Chunk 0x{ID & 0xFFF:x3} from class {Node.ClassName} doesn't support UnknownValues.");
        }

        public Chunk()
        {
            
        }

        public Chunk(T node)
        {
            Node = node;
        }

        public override GameBoxReader OpenUnknownStream()
        {
            return new GameBoxReader(Unknown, Lookbackable);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="n"></param>
        /// <param name="r"></param>
        /// <param name="unknownW">Writer of the <see cref="Chunk.Unknown"/> stream. This parameter mustn't be used inside loops - it can cause writing order problems whenever the loop changes!</param>
        public virtual void Read(T n, GameBoxReader r, GameBoxWriter unknownW)
        {
            throw new NotImplementedException($"Chunk 0x{ID & 0xFFF:x3} from class {Node.ClassName} doesn't support Read.");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="n"></param>
        /// <param name="w"></param>
        /// <param name="unknownR">Reader of the <see cref="Chunk.Unknown"/> stream. This parameter mustn't be used inside loops - it can cause writing order problems whenever the loop changes!</param>
        public virtual void Write(T n, GameBoxWriter w, GameBoxReader unknownR)
        {
            throw new NotImplementedException($"Chunk 0x{ID & 0xFFF:x3} from class {Node.ClassName} doesn't support Write.");
        }

        public virtual void ReadWrite(T n, GameBoxReaderWriter rw)
        {
            if (rw.Reader != null)
            {
                var unknownW = new GameBoxWriter(Unknown, rw.Reader.Lookbackable);
                if (n == null) Read(null, rw.Reader, unknownW);
                else Read(n, rw.Reader, unknownW);
            }

            if (rw.Writer != null)
            {
                var unknownR = new GameBoxReader(Unknown, rw.Writer.Lookbackable);
                if (n == null) Write(null, rw.Writer, unknownR);
                else Write(n, rw.Writer, unknownR);
            }
        }

        void IChunk.ReadWrite(Node n, GameBoxReaderWriter rw) => ReadWrite((T)n, rw);

        public byte[] ToByteArray()
        {
            var lookbackable = Lookbackable;

            if (this is ILookbackable l)
            {
                l.LookbackWritten = false;
                l.LookbackStrings.Clear();
                lookbackable = l;
            }

            using (var ms = new MemoryStream())
            using (var w = new GameBoxWriter(ms, lookbackable))
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
