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
    public class HeaderChunkList<T> : ChunkListBase<T, HeaderChunk<T>> where T : Node
    {
        public HeaderChunkList() : base()
        {

        }

        public HeaderChunkList(IEnumerable<HeaderChunk<T>> chunks) : base(chunks.ToDictionary(x => x.ID))
        {

        }

        public HeaderChunkList(IEnumerable<KeyValuePair<uint, HeaderChunk<T>>> chunks) : base(chunks.ToDictionary(x => x.Key, x => x.Value))
        {

        }

        public bool Remove<TChunk>() where TChunk : HeaderChunk<T>
        {
            return Remove(typeof(TChunk).GetCustomAttribute<ChunkAttribute>().ID);
        }
    }
}
