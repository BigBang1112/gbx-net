using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace GBX.NET
{
    public abstract class ChunkListBase<TNode, TChunk> : SortedList<uint, TChunk> where TNode : Node where TChunk : Chunk<TNode>
    {
        public ChunkListBase() : base()
        {

        }

        public ChunkListBase(IDictionary<uint, TChunk> dictionary) : base(dictionary)
        {
            
        }

        public void Add(TChunk chunk)
        {
            Add(chunk.ID, chunk);
        }

        public bool Remove(TChunk chunk)
        {
            return Remove(chunk.ID);
        }

        public T Create<T>(byte[] data) where T : TChunk
        {
            var chunkId = typeof(T).GetCustomAttribute<ChunkAttribute>().ID;

            if (TryGetValue(chunkId, out TChunk c))
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

        public TChunk Create<T>() where T : TChunk
        {
            return Create<T>(new byte[0]);
        }

        public static T FromGBX<T>(GameBox<T> loadedGbx) where T : Node
        {
            return loadedGbx.MainNode;
        }

        public void Discover<TChunk1>() where TChunk1 : SkippableChunk<TNode>
        {
            foreach (var chunk in Values)
                if (chunk is TChunk1 c)
                    c.Discover();
        }

        public void Discover<TChunk1, TChunk2>() where TChunk1 : SkippableChunk<TNode> where TChunk2 : SkippableChunk<TNode>
        {
            foreach (var chunk in Values)
            {
                if (chunk is TChunk1 c1) c1.Discover();
                if (chunk is TChunk2 c2) c2.Discover();
            }
        }

        public void Discover<TChunk1, TChunk2, TChunk3>()
            where TChunk1 : SkippableChunk<TNode>
            where TChunk2 : SkippableChunk<TNode>
            where TChunk3 : SkippableChunk<TNode>
        {
            foreach (var chunk in Values)
            {
                if (chunk is TChunk1 c1) c1.Discover();
                if (chunk is TChunk2 c2) c2.Discover();
                if (chunk is TChunk3 c3) c3.Discover();
            }
        }

        public void Discover<TChunk1, TChunk2, TChunk3, TChunk4>()
            where TChunk1 : SkippableChunk<TNode>
            where TChunk2 : SkippableChunk<TNode>
            where TChunk3 : SkippableChunk<TNode>
            where TChunk4 : SkippableChunk<TNode>
        {
            foreach (var chunk in Values)
            {
                if (chunk is TChunk1 c1) c1.Discover();
                if (chunk is TChunk2 c2) c2.Discover();
                if (chunk is TChunk3 c3) c3.Discover();
                if (chunk is TChunk4 c4) c4.Discover();
            }
        }

        public void Discover<TChunk1, TChunk2, TChunk3, TChunk4, TChunk5>()
            where TChunk1 : SkippableChunk<TNode>
            where TChunk2 : SkippableChunk<TNode>
            where TChunk3 : SkippableChunk<TNode>
            where TChunk4 : SkippableChunk<TNode>
            where TChunk5 : SkippableChunk<TNode>
        {
            foreach (var chunk in Values)
            {
                if (chunk is TChunk1 c1) c1.Discover();
                if (chunk is TChunk2 c2) c2.Discover();
                if (chunk is TChunk3 c3) c3.Discover();
                if (chunk is TChunk4 c4) c4.Discover();
                if (chunk is TChunk5 c5) c5.Discover();
            }
        }

        public void Discover<TChunk1, TChunk2, TChunk3, TChunk4, TChunk5, TChunk6>()
            where TChunk1 : SkippableChunk<TNode>
            where TChunk2 : SkippableChunk<TNode>
            where TChunk3 : SkippableChunk<TNode>
            where TChunk4 : SkippableChunk<TNode>
            where TChunk5 : SkippableChunk<TNode>
            where TChunk6 : SkippableChunk<TNode>
        {
            foreach (var chunk in Values)
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
            foreach (var chunk in Values)
                if (chunk is SkippableChunk<TNode> s)
                    s.Discover();
        }

        public T Get<T>() where T : Chunk
        {
            foreach (var chunk in Values)
            {
                if (chunk is T t)
                {
                    if (chunk is SkippableChunk<TNode> s) s.Discover();
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

        public void FromAuxNodeChunkList(AuxNodeChunkList chunkList)
        {
            Clear();
            foreach (var c in chunkList)
                Add((TChunk)c.Value);
        }
    }
}
