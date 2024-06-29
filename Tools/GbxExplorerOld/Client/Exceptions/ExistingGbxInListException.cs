using System.Runtime.Serialization;

namespace GbxExplorerOld.Client.Exceptions;

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
}
