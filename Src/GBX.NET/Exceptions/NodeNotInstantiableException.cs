namespace GBX.NET.Exceptions;

public class NodeNotInstantiableException : Exception
{
    public uint ClassId { get; }

    public NodeNotInstantiableException(uint classId) : base(GetMessage(classId))
    {
        ClassId = classId;
    }

    private static string GetMessage(uint classId)
    {
        return $"Instance of a node with a class ID 0x{classId:X8} cannot be created.";
    }
}
