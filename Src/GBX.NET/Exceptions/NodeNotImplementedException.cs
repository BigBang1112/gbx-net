namespace GBX.NET.Exceptions;

public class NodeNotImplementedException : Exception
{
    public uint ClassId { get; }

    public NodeNotImplementedException(uint classId) : base(GetMessage(classId))
    {
        ClassId = classId;
    }

    private static string GetMessage(uint classId)
    {
        return $"Node with ID 0x{classId:X8} is not implemented. ({GetClassName(classId)})";
    }

    private static string GetClassName(uint classId)
    {
        NodeCacheManager.Names.TryGetValue(classId, out var className);
        return className ?? "unknown class";
    }
}
