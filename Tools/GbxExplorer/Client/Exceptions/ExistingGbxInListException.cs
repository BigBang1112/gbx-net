using System.Runtime.Serialization;

namespace GbxExplorer.Client.Exceptions;

public class ExistingGbxInListException : Exception
{
    public ExistingGbxInListException()
    {
    }

    public ExistingGbxInListException(string? message) : base(message)
    {
    }

    public ExistingGbxInListException(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    protected ExistingGbxInListException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}
