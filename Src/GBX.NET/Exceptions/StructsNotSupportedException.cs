using System.Runtime.Serialization;

namespace GBX.NET.Exceptions;

public class StructsNotSupportedException : Exception
{
    public StructsNotSupportedException() : base("Structs are not supported in version 3 and lower.")
    {
    }

    public StructsNotSupportedException(string? message) : base(message)
    {
    }

    public StructsNotSupportedException(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    protected StructsNotSupportedException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}
