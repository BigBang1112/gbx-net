using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;

namespace GBX.NET
{
    public class SkippableChunk<T> : Chunk<T>, ISkippableChunk where T : Node
    {
        readonly uint chunkID;

        public override uint ID => chunkID;
        [IgnoreDataMember]
        public MemoryStream Stream { get; set; }

        public int Length => (int)Stream.Length;
        public bool Discovered { get; set; }
        public byte[] Data => Stream.ToArray();

        public SkippableChunk()
        {
            chunkID = GetType().GetCustomAttribute<ChunkAttribute>().ID;
        }

        public SkippableChunk(T node, uint id, byte[] data) : base(node)
        {
            chunkID = id;
            Stream = new MemoryStream(data, 0, data.Length, false);

            if (data == null || data.Length == 0)
                Discovered = true;
        }

        public SkippableChunk(T node, byte[] data) : base(node)
        {
            chunkID = GetType().GetCustomAttribute<ChunkAttribute>().ID;
            Stream = new MemoryStream(data, 0, data.Length, false);

            if (data == null || data.Length == 0)
                Discovered = true;
        }

        public void Discover()
        {
            if (Discovered) return;
            Discovered = true;

            ILookbackable lb = Node.Lookbackable;
            if (this is ILookbackable l) lb = l;

            using var gbxr = new GameBoxReader(Stream, lb);

            GameBoxReaderWriter gbxrw = new GameBoxReaderWriter(gbxr);

            try
            {
                ReadWrite(Node, gbxrw);
            }
            catch (NotImplementedException)
            {
                var unknownGbxw = new GameBoxWriter(Unknown, lb);

                try
                {
                    Read(Node, gbxr, unknownGbxw);
                }
                catch (NotImplementedException e)
                {
                    Debug.WriteLine(e.Message);
                }
            }

            Progress = (int)Stream.Position;
        }

        public override void Write(T n, GameBoxWriter w, GameBoxReader unknownR)
        {
            w.Write(Stream.ToArray(), 0, (int)Stream.Length);
        }

        public void Write(GameBoxWriter w)
        {
            w.Write(Stream.ToArray(), 0, (int)Stream.Length);
        }
    }
}
