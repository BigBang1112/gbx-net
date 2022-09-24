namespace GBX.NET.Exceptions;

public class NodeNotImplementedException : Exception
{
    public uint ClassId { get; }
    public string ClassName { get; }

    public NodeNotImplementedException(uint classId) : base(GetMessage(classId))
    {
        ClassId = classId;
        ClassName = GetClassName(classId);
    }

    private static string GetMessage(uint classId)
    {
        return $"Node with ID 0x{classId:X8} is not implemented. ({GetClassName(classId)})";
    }

    private static string GetClassName(uint classId)
    {
        return NodeManager.GetName(classId) ?? "unknown class";
    }
}
