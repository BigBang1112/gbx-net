using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GBX.NET.Exceptions;

public class UnsupportedChunkVersionException : Exception
{
    public UnsupportedChunkVersionException(Chunk chunk, int version) : base($"Chunk version {version} is not yet supported.")
    {
    }
}
