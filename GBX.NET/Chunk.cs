using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;

namespace GBX.NET
{
    public abstract class Chunk
    {
        public virtual uint ID => GetType().GetCustomAttribute<ChunkAttribute>().ID;
        public int Progress { get; internal set; }
        /// <summary>
        /// Stream of unknown bytes
        /// </summary>
        [IgnoreDataMember]
        public MemoryStream Unknown { get; } = new MemoryStream();

        public Chunk()
        {

        }

        [Obsolete]
        public Chunk(Node node)
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

        [Obsolete]
        public virtual object[] GetUnknownObjects()
        {
            throw new NotImplementedException($"Chunk 0x{ID & 0xFFF:x3} doesn't support GetUnknownObjects.");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="r"></param>
        /// <param name="unknownW">Writer of the <see cref="Unknown"/> stream. This parameter mustn't be used inside loops - it can cause writing order problems whenever the loop changes!</param>
        [Obsolete]
        public virtual void Read(GameBoxReader r, GameBoxWriter unknownW)
        {
            throw new NotImplementedException($"Chunk 0x{ID & 0xFFF:x3} doesn't support Read.");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="w"></param>
        /// <param name="unknownR">Reader of the <see cref="Unknown"/> stream. This parameter mustn't be used inside loops - it can cause writing order problems whenever the loop changes!</param>
        [Obsolete]
        public virtual void Write(GameBoxWriter w, GameBoxReader unknownR)
        {
            throw new NotImplementedException($"Chunk 0x{ID & 0xFFF:x3} doesn't support Write.");
        }

        [Obsolete]
        public virtual void ReadWrite(GameBoxReaderWriter rw)
        {
            Debug.WriteLine("ReadWrite on Chunk is no longer supported. " + GetType().FullName);
        }
    }

    public abstract class Chunk<T> : Chunk where T : Node
    {
        public new T Node { get; internal set; }

        public bool IsHeader => Node.Lookbackable is GameBoxHeader;
        public bool IsBody => Node.Lookbackable is GameBoxBody;

        public Chunk()
        {

        }

        public Chunk(T node)
        {
            Node = node;
        }

        public new virtual object[] GetUnknownObjects()
        {
            throw new NotImplementedException($"Chunk 0x{ID & 0xFFF:x3} from class {Node.ClassName} doesn't support GetUnknownObjects.");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="r"></param>
        /// <param name="unknownW">Writer of the <see cref="Unknown"/> stream. This parameter mustn't be used inside loops - it can cause writing order problems whenever the loop changes!</param>
        public virtual void Read(T n, GameBoxReader r, GameBoxWriter unknownW)
        {
            throw new NotImplementedException($"Chunk 0x{ID & 0xFFF:x3} from class {Node.ClassName} doesn't support Read.");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="w"></param>
        /// <param name="unknownR">Reader of the <see cref="Unknown"/> stream. This parameter mustn't be used inside loops - it can cause writing order problems whenever the loop changes!</param>
        public virtual void Write(T n, GameBoxWriter w, GameBoxReader unknownR)
        {
            throw new NotImplementedException($"Chunk 0x{ID & 0xFFF:x3} from class {Node.ClassName} doesn't support Write.");
        }

        [Obsolete]
        public new virtual void ReadWrite(GameBoxReaderWriter rw)
        {
            ReadWrite(null, rw);
        }

        public virtual void ReadWrite(T n, GameBoxReaderWriter rw)
        {
            ILookbackable lb = Node.Lookbackable;
            if (this is ILookbackable l) lb = l;

            if (rw.Reader != null)
            {
                var unknownW = new GameBoxWriter(Unknown, lb);
                if (n == null) Read(null, rw.Reader, unknownW);
                else Read(n, rw.Reader, unknownW);
            }

            if (rw.Writer != null)
            {
                var unknownR = new GameBoxReader(Unknown, lb);
                if (n == null) Write(null, rw.Writer, unknownR);
                else Write(n, rw.Writer, unknownR);
            }
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

        [Obsolete]
        public void Cast<TChunk>() where TChunk : Chunk<T>
        {
            var chunk = (TChunk)this;
            //Node.Chunks.Add(chunk);
            //Node.Chunks.Remove(this);
        }
    }
}
