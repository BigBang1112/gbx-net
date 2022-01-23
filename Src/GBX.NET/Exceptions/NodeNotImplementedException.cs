namespace GBX.NET.Exceptions;

public class NodeNotImplementedException : Exception
{
    public uint ClassId { get; }

    public string ClassName => NodeCacheManager.Names
        .Where(x => x.Key == ClassId)
        .Select(x => x.Value)
        .FirstOrDefault() ?? "unknown class";

    public override string Message => $"Node with ID 0x{ClassId:X8} is not implemented. ({ClassName})";

    public NodeNotImplementedException(uint classId)
    {
        ClassId = classId;
    }
}
