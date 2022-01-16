namespace GBX.NET.Exceptions;

public class ChunkReadNotImplementedException : Exception
{
    public ChunkReadNotImplementedException(uint id, Node node)
        : this($"Chunk 0x{id & 0xFFF:x3} from class {node} doesn't support Read.")
    {
    }

    public ChunkReadNotImplementedException(string? message) : base(message)
    {
    }

    public ChunkReadNotImplementedException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}
