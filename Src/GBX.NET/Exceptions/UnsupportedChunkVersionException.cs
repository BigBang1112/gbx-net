using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GBX.NET.Exceptions
{
    public class UnsupportedChunkVersionException : Exception
    {
        public UnsupportedChunkVersionException(IVersionable chunk) : base($"Chunk version {chunk.Version} is not yet supported.")
        {
        }
    }
}
