using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace GBX.NET
{
    /// <summary>
    /// A list of chunks sorted by the chunk ID.
    /// </summary>
    public class ChunkList<T> : ChunkListBase<T, Chunk<T>> where T : Node
    {
        public ChunkList() : base()
        {

        }

        public ChunkList(IEnumerable<Chunk<T>> chunks) : base(chunks.ToDictionary(x => x.ID))
        {
            
        }

        public ChunkList(IEnumerable<KeyValuePair<uint, Chunk<T>>> chunks) : base(chunks.ToDictionary(x => x.Key, x => x.Value))
        {
            
        }

        public bool Remove<TChunk>() where TChunk : Chunk<T>
        {
            return Remove(typeof(TChunk).GetCustomAttribute<ChunkAttribute>().ID);
        }
    }
}
