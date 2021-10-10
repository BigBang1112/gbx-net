using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.IO;
using System.Runtime.Serialization;
using System.Threading.Tasks;

using GBX.NET.Engines.MwFoundations;

namespace GBX.NET
{
    public class ChunkSet : SortedSet<Chunk>
    {
        [IgnoreDataMember]
        public CMwNod Node { get; set; }

        public ChunkSet() : base()
        {

        }

        public ChunkSet(IEnumerable<Chunk> collection) : base(collection)
        {

        }

        public bool Remove(uint chunkID)
        {
            return RemoveWhere(x => x.ID == chunkID) > 0;
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

            T chunk = (T)Activator.CreateInstance(typeof(T));
            chunk.Node = Node;
            chunk.GBX = Node?.GBX;

            if (chunk is ISkippableChunk s)
            {
                s.Data = data;
                if (data == null || data.Length == 0)
                    s.Discovered = true;

                chunk.OnLoad();
            }
            else if (data.Length > 0)
            {
                using var ms = new MemoryStream(data);
                using var r = new GameBoxReader(ms);

                var rw = new GameBoxReaderWriter(r);
                chunk.ReadWrite(Node, rw);
            }

            if (chunk is ILookbackable l) l.IdVersion = 3;
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

        /// <summary>
        /// Discovers all chunks in the chunk set.
        /// </summary>
        public void DiscoverAll()
        {
            foreach (var chunk in this)
                if (chunk is ISkippableChunk s)
                    s.Discover();
        }

        /// <summary>
        /// Discovers all chunks in the chunk set in parallel, if <paramref name="parallel"/> is true.
        /// </summary>
        public void DiscoverAll(bool parallel)
        {
            if (parallel)
            {
                Parallel.ForEach(this, chunk =>
                {
                    if (chunk is ISkippableChunk s)
                        s.Discover();
                });
            }
            else
            {
                DiscoverAll();
            }
        }
    }
}
