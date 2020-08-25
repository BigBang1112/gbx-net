using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace GBX.NET
{
    /// <summary>
    /// A list of chunks sorted by the chunk ID.
    /// </summary>
    public class ChunkList : SortedList<uint, Chunk>
    {
        public ChunkList() : base()
        {

        }

        public ChunkList(IEnumerable<Chunk> chunks) : base(chunks.ToDictionary(x => x.ID))
        {

        }

        public ChunkList(IEnumerable<KeyValuePair<uint, Chunk>> chunks) : base(chunks.ToDictionary(x => x.Key, x => x.Value))
        {

        }

        public void Add(Chunk chunk)
        {
            Add(chunk.ID, chunk);
        }

        public bool Remove<TChunk>() where TChunk : Chunk
        {
            return Remove(typeof(TChunk).GetCustomAttribute<ChunkAttribute>().ID);
        }

        public bool Remove(Chunk chunk)
        {
            return Remove(chunk.ID);
        }
    }
}
