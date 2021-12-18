namespace GBX.NET.Exceptions;

public class NodeNotInstantiableException : Exception
{
    public NodeNotInstantiableException(uint classId) : base($"Instance of a node with a class ID {classId:X8} cannot be created.")
    {

    }
}
