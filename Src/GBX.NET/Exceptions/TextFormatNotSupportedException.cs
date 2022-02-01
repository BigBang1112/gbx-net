namespace GBX.NET.Exceptions;

public class TextFormatNotSupportedException : Exception
{
    public TextFormatNotSupportedException() : base("Text-formatted GBX files are not supported.")
    {

    }

    public TextFormatNotSupportedException(string? message) : base(message)
    {

    }

    public TextFormatNotSupportedException(string? message, Exception? innerException) : base(message, innerException)
    {

    }
}
