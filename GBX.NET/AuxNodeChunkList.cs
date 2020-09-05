using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace GBX.NET
{
    public class AuxNodeChunkList : SortedList<uint, Chunk>
    {
        public void Add(Chunk chunk)
        {
            Add(chunk.ID, chunk);
        }

        public bool Remove(Chunk chunk)
        {
            return Remove(chunk.ID);
        }

        public bool Remove<T>() where T : Chunk
        {
            return Remove(typeof(T).GetCustomAttribute<ChunkAttribute>().ID);
        }

        public T Create<T>(byte[] data) where T : Chunk
        {
            var chunkId = typeof(T).GetCustomAttribute<ChunkAttribute>().ID;

            if (TryGetValue(chunkId, out Chunk c))
                return (T)c;

            dynamic chunk;
            if (typeof(T).BaseType == typeof(SkippableChunk<>))
                chunk = (T)Activator.CreateInstance(typeof(T), this, data);
            else
            {
                chunk = (T)Activator.CreateInstance(typeof(T), this);
                if (data.Length > 0) chunk.FromData(data);
            }

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
            foreach (var chunk in Values)
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
    }
}
