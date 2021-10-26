using GBX.NET.Engines.MwFoundations;
using System;
using System.Collections.Generic;
using System.Text;

namespace GBX.NET.Exceptions
{
    public class IgnoredUnskippableChunkException : Exception
    {
        public IgnoredUnskippableChunkException(CMwNod node, uint chunkId)
            : base($"Chunk 0x{chunkId & 0xFFF:x3} from class {node.ClassName} is known but its content is unknown to read.")
        {

        }
    }
}
