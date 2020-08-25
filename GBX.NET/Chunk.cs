using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;

namespace GBX.NET
{
    public abstract class Chunk
    {
        public Node Node { get; }
        public virtual uint ID => GetType().GetCustomAttribute<ChunkAttribute>().ID;
        public int Progress { get; internal set; }
        /// <summary>
        /// Stream of unknown bytes
        /// </summary>
        [IgnoreDataMember]
        public MemoryStream Unknown { get; }
        public bool IsHeavy { get; set; }

        public bool IsHeader => Node.Lookbackable is GameBoxHeader;
        public bool IsBody => Node.Lookbackable is IGameBoxBody;
        public bool Skippable => this is SkippableChunk;

        public Chunk(Node node)
        {
            Node = node;
            Unknown = new MemoryStream();
        }

        [Obsolete]
        public ILookbackable GetLookbackable()
        {
            return Node.Lookbackable;
        }

        public override int GetHashCode()
        {
            return (int)ID;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Chunk);
        }

        public bool Equals(Chunk chunk) => chunk != null && chunk.ID == ID;

        public virtual void Parse(Stream stream)
        {
            throw new NotImplementedException(ID + " This chunk doesn't support Parse.");
        }

        public virtual object[] GetUnknownObjects()
        {
            throw new NotImplementedException($"Chunk 0x{ID & 0xFFF:x3} from class {Node.ClassName} doesn't support GetUnknownObjects.");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="r"></param>
        /// <param name="unknownW">Writer of the <see cref="Unknown"/> stream. This parameter mustn't be used inside loops - it can cause writing order problems whenever the loop changes!</param>
        public virtual void Read(GameBoxReader r, GameBoxWriter unknownW)
        {
            throw new NotImplementedException($"Chunk 0x{ID & 0xFFF:x3} from class {Node.ClassName} doesn't support Read.");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="w"></param>
        /// <param name="unknownR">Reader of the <see cref="Unknown"/> stream. This parameter mustn't be used inside loops - it can cause writing order problems whenever the loop changes!</param>
        public virtual void Write(GameBoxWriter w, GameBoxReader unknownR)
        {
            throw new NotImplementedException($"Chunk 0x{ID & 0xFFF:x3} from class {Node.ClassName} doesn't support Write.");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rw"></param>
        public virtual void ReadWrite(GameBoxReaderWriter rw)
        {
            ILookbackable lb = Node.Lookbackable;
            if (this is ILookbackable l) lb = l;

            if (rw.Reader != null)
            {
                var unknownW = new GameBoxWriter(Unknown, lb);
                Read(rw.Reader, unknownW);
            }

            if (rw.Writer != null)
            {
                var unknownR = new GameBoxReader(Unknown, lb);
                Write(rw.Writer, unknownR);
            }
        }

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

        public void FromData(byte[] data)
        {
            var lookbackable = Node.Lookbackable;

            if (this is ILookbackable l)
            {
                l.LookbackVersion = null;
                lookbackable = l;
            }

            using var ms = new MemoryStream(data);
            using var r = new GameBoxReader(ms, lookbackable);
            var rw = new GameBoxReaderWriter(r);
            ReadWrite(rw);
        }

        public byte[] ToByteArray()
        {
            var lookbackable = Node.Lookbackable;

            if (this is ILookbackable l)
            {
                l.LookbackWritten = false;
                l.LookbackStrings.Clear();
                lookbackable = l;
            }

            using var ms = new MemoryStream();
            using var w = new GameBoxWriter(ms, lookbackable);
            var rw = new GameBoxReaderWriter(w);
            ReadWrite(rw);
            return ms.ToArray();
        }

        public void Cast<TChunk>() where TChunk : Chunk
        {
            var chunk = (TChunk)this;
            Node.Chunks.Add(chunk);
            Node.Chunks.Remove(this);
        }
    }
}
