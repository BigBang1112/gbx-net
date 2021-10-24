using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GBX.NET.Exceptions
{
    public class ChunkParseException : Exception
    {
        public ChunkParseException(uint chunkId, uint? previousChunkId)
            : base($"Wrong chunk format or unskippable chunk: 0x{chunkId:X8} (" +
                   $"{NodeCacheManager.Names.Where(x => x.Key == Chunk.Remap(chunkId & 0xFFFFF000)).Select(x => x.Value).FirstOrDefault() ?? "unknown class"})" +
                   $"\nPrevious chunk: 0x{previousChunkId ?? 0:X8} (" +
                   $"{(previousChunkId.HasValue ? (NodeCacheManager.Names.Where(x => x.Key == Chunk.Remap(previousChunkId.Value & 0xFFFFF000)).Select(x => x.Value).FirstOrDefault() ?? "unknown class") : "not a class")})")
        {

        }
    }
}
