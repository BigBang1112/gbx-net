namespace GBX.NET.Exceptions;

public class NodeNotInstantiableException : Exception
{
    private static string GetMessage(uint classId)
    {
        return $"Instance of a node with a class ID 0x{classId:X8} cannot be created.";
    }

    public NodeNotInstantiableException(uint classId) : base(GetMessage(classId))
    {

    }

    public NodeNotInstantiableException(string? message) : base(message)
    {

    }

    public NodeNotInstantiableException(uint classId, Exception? innerException) : base(GetMessage(classId), innerException)
    {

    }

    public NodeNotInstantiableException(string? message, Exception? innerException) : base(message, innerException)
    {

    }
}
