namespace GBX.NET.Exceptions;

public class ChunkWriteNotImplementedException : Exception
{
    public uint? Id { get; }
    public Node? Node { get; }

    public ChunkWriteNotImplementedException(uint id, Node node) : this(GetMessage(id, node))
    {

    }

    private static string GetMessage(uint id, Node node)
    {
        return $"Chunk 0x{id & 0xFFF:X3} from class {node.GetType().Name} doesn't support Write.";
    }

    public ChunkWriteNotImplementedException(string? message) : base(message)
    {

    }

    public ChunkWriteNotImplementedException(string? message, Exception? innerException) : base(message, innerException)
    {

    }
}
