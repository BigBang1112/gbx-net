namespace GBX.NET.Exceptions;

public class ChunkReadNotImplementedException : Exception
{
    public uint? Id { get; }
    public Node? Node { get; }

    public ChunkReadNotImplementedException(uint id, Node node) : this(GetMessage(id, node))
    {
        Id = id;
        Node = node;
    }

    private static string GetMessage(uint id, Node node)
    {
        return $"Chunk 0x{id & 0xFFF:x3} from class {node} doesn't support Read.";
    }

    public ChunkReadNotImplementedException(string? message) : base(message)
    {

    }

    public ChunkReadNotImplementedException(string? message, Exception? innerException) : base(message, innerException)
    {

    }
}
