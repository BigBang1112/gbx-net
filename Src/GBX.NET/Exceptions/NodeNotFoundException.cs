namespace GBX.NET.Exceptions;

public class NodeNotFoundException : Exception
{
    public NodeNotFoundException(uint classId) : base($"Node with a class ID {classId:X8} was not found.")
    {

    }
}
