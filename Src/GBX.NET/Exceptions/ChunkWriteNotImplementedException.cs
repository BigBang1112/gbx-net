using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace GBX.NET.Exceptions;

public class ChunkWriteNotImplementedException : Exception
{
    public ChunkWriteNotImplementedException(uint id, string name)
        : this($"Chunk 0x{id & 0xFFF:x3} from class {name} doesn't support Write.")
    {
    }

    public ChunkWriteNotImplementedException(string? message) : base(message)
    {
    }

    public ChunkWriteNotImplementedException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}
