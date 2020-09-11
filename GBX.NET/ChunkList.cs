using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.IO;
using System.Runtime.Serialization;

namespace GBX.NET
{
    public class ChunkList : SortedSet<Chunk>
    {
        [IgnoreDataMember]
        public Node Node { get; set; }

        public ChunkList() : base()
        {

        }

        public ChunkList(IEnumerable<Chunk> collection) : base(collection)
        {

        }

        public bool Remove<T>() where T : Chunk
        {
            return RemoveWhere(x => x.ID == typeof(T).GetCustomAttribute<ChunkAttribute>().ID) > 0;
        }

        public T Create<T>(byte[] data) where T : Chunk
        {
            var chunkId = typeof(T).GetCustomAttribute<ChunkAttribute>().ID;

            var c = this.FirstOrDefault(x => x.ID == chunkId);
            if (c != null)
                return (T)c;

            dynamic chunk;
            if (typeof(T).BaseType.GetGenericTypeDefinition() == typeof(SkippableChunk<>))
            {
                chunk = (T)Activator.CreateInstance(typeof(T));
                chunk.Stream = new MemoryStream(data, 0, data.Length, false);
                chunk.Node = (dynamic)Node;
                if (data == null || data.Length == 0)
                    chunk.Discovered = true;

                chunk.OnLoad();
            }
            else
            {
                chunk = (T)Activator.CreateInstance(typeof(T));
                chunk.Node = (dynamic)Node;
                if (data.Length > 0) chunk.FromData(data);
            }

            chunk.Part = Node?.Body;

            if (chunk is ILookbackable l) l.LookbackVersion = 3;
            Add(chunk);
            return chunk;
        }

        public T Create<T>() where T : Chunk
        {
            return Create<T>(new byte[0]);
        }

        public T Get<T>() where T : Chunk
        {
            foreach (var chunk in this)
            {
                if (chunk is T t)
                {
                    if (chunk is ISkippableChunk s) s.Discover();
                    return t;
                }
            }
            return default;
        }

        public bool TryGet<T>(out T chunk) where T : Chunk
        {
            chunk = Get<T>();
            return chunk != default;
        }

        public void Discover<TChunk1>() where TChunk1 : ISkippableChunk
        {
            foreach (var chunk in this)
                if (chunk is TChunk1 c)
                    c.Discover();
        }

        public void Discover<TChunk1, TChunk2>() where TChunk1 : ISkippableChunk where TChunk2 : ISkippableChunk
        {
            foreach (var chunk in this)
            {
                if (chunk is TChunk1 c1) c1.Discover();
                if (chunk is TChunk2 c2) c2.Discover();
            }
        }

        public void Discover<TChunk1, TChunk2, TChunk3>()
            where TChunk1 : ISkippableChunk
            where TChunk2 : ISkippableChunk
            where TChunk3 : ISkippableChunk
        {
            foreach (var chunk in this)
            {
                if (chunk is TChunk1 c1) c1.Discover();
                if (chunk is TChunk2 c2) c2.Discover();
                if (chunk is TChunk3 c3) c3.Discover();
            }
        }

        public void Discover<TChunk1, TChunk2, TChunk3, TChunk4>()
            where TChunk1 : ISkippableChunk
            where TChunk2 : ISkippableChunk
            where TChunk3 : ISkippableChunk
            where TChunk4 : ISkippableChunk
        {
            foreach (var chunk in this)
            {
                if (chunk is TChunk1 c1) c1.Discover();
                if (chunk is TChunk2 c2) c2.Discover();
                if (chunk is TChunk3 c3) c3.Discover();
                if (chunk is TChunk4 c4) c4.Discover();
            }
        }

        public void Discover<TChunk1, TChunk2, TChunk3, TChunk4, TChunk5>()
            where TChunk1 : ISkippableChunk
            where TChunk2 : ISkippableChunk
            where TChunk3 : ISkippableChunk
            where TChunk4 : ISkippableChunk
            where TChunk5 : ISkippableChunk
        {
            foreach (var chunk in this)
            {
                if (chunk is TChunk1 c1) c1.Discover();
                if (chunk is TChunk2 c2) c2.Discover();
                if (chunk is TChunk3 c3) c3.Discover();
                if (chunk is TChunk4 c4) c4.Discover();
                if (chunk is TChunk5 c5) c5.Discover();
            }
        }

        public void Discover<TChunk1, TChunk2, TChunk3, TChunk4, TChunk5, TChunk6>()
            where TChunk1 : ISkippableChunk
            where TChunk2 : ISkippableChunk
            where TChunk3 : ISkippableChunk
            where TChunk4 : ISkippableChunk
            where TChunk5 : ISkippableChunk
            where TChunk6 : ISkippableChunk
        {
            foreach (var chunk in this)
            {
                if (chunk is TChunk1 c1) c1.Discover();
                if (chunk is TChunk2 c2) c2.Discover();
                if (chunk is TChunk3 c3) c3.Discover();
                if (chunk is TChunk4 c4) c4.Discover();
                if (chunk is TChunk5 c5) c5.Discover();
                if (chunk is TChunk6 c6) c6.Discover();
            }
        }

        public void DiscoverAll()
        {
            foreach (var chunk in this)
                if (chunk is ISkippableChunk s)
                    s.Discover();
        }
    }
}
