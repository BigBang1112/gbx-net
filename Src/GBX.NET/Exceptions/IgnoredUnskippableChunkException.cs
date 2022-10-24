namespace GBX.NET.Exceptions;

public class IgnoredUnskippableChunkException : Exception
{
    public IgnoredUnskippableChunkException(Node node, uint chunkId) : base(GetMessage(node, chunkId))
    {

    }

    private static string GetMessage(Node node, uint chunkId)
    {
        return $"Chunk 0x{chunkId & 0xFFF:X3} from class {node.GetType().Name} is known but its content is unknown to read.";
    }
}
