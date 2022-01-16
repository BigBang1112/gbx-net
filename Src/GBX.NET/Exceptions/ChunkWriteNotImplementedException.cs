namespace GBX.NET.Exceptions;

public class ChunkWriteNotImplementedException : Exception
{
    public ChunkWriteNotImplementedException(uint id, Node node)
        : this($"Chunk 0x{id & 0xFFF:x3} from class {node} doesn't support Write.")
    {
    }

    public ChunkWriteNotImplementedException(string? message) : base(message)
    {
    }

    public ChunkWriteNotImplementedException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}
