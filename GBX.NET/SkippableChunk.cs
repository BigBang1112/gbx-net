using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;

using GBX.NET.Engines.MwFoundations;

namespace GBX.NET
{
    public class SkippableChunk<T> : Chunk<T>, ISkippableChunk where T : CMwNod
    {
        private readonly uint chunkID;

        public override uint ID => chunkID;

        public bool Discovered { get; set; }
        public byte[] Data { get; set; }

        public SkippableChunk()
        {
            chunkID = GetType().GetCustomAttribute<ChunkAttribute>().ID;
        }

        public SkippableChunk(T node, uint id, byte[] data) : base(node)
        {
            chunkID = id;

            Data = data;

            if (data == null || data.Length == 0)
                Discovered = true;
        }

        public SkippableChunk(T node, byte[] data) : base(node)
        {
            chunkID = GetType().GetCustomAttribute<ChunkAttribute>().ID;

            Data = data;

            if (data == null || data.Length == 0)
                Discovered = true;
        }

        public void Discover()
        {
            if (Discovered) return;
            Discovered = true;

            using (var ms = new MemoryStream(Data))
            using (var gbxr = new GameBoxReader(ms, Lookbackable))
            {
                gbxr.Chunk = this;

                GameBoxReaderWriter gbxrw = new GameBoxReaderWriter(gbxr);

                try
                {
                    ReadWrite(Node, gbxrw);
                }
                catch (NotImplementedException)
                {
                    var unknownGbxw = new GameBoxWriter(Unknown, Lookbackable);

                    try
                    {
                        Read(Node, gbxr, unknownGbxw);
                    }
                    catch (NotImplementedException e)
                    {
                        Debug.WriteLine(e.Message);
                    }
                }

                Progress = (int)ms.Position;
            }
        }

        public override void Write(T n, GameBoxWriter w, GameBoxReader unknownR)
        {
            w.WriteBytes(Data);
        }

        public void Write(GameBoxWriter w)
        {
            w.WriteBytes(Data);
        }

        public override string ToString()
        {
            if(GetType().GetCustomAttribute<ChunkAttribute>() == null)
                return $"{typeof(T).Name} unknown skippable chunk 0x{ID:X8}";
            var desc = GetType().GetCustomAttribute<ChunkAttribute>().Description;
            return $"{typeof(T).Name} skippable chunk 0x{ID:X8}{(string.IsNullOrEmpty(desc) ? "" : $" ({desc})")}";
        }
    }
}
