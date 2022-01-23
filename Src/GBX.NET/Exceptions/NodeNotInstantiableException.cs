namespace GBX.NET.Exceptions;

public class NodeNotInstantiableException : Exception
{
    public uint ClassId { get; }

    public override string Message => $"Instance of a node with a class ID 0x{ClassId:X8} cannot be created.";

    public NodeNotInstantiableException(uint classId)
    {
        ClassId = classId;
    }
}
