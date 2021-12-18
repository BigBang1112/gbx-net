using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace GBX.NET.Exceptions;

public class ChunkReadNotImplementedException : Exception
{
    public ChunkReadNotImplementedException(uint id, string name)
        : this($"Chunk 0x{id & 0xFFF:x3} from class {name} doesn't support Read.")
    {
    }

    public ChunkReadNotImplementedException(string? message) : base(message)
    {
    }

    public ChunkReadNotImplementedException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}
