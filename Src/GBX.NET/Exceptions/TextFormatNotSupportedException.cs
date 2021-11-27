namespace GBX.NET.Exceptions;

public class TextFormatNotSupportedException : Exception
{
    public TextFormatNotSupportedException() : base("Text-formatted GBX files are not supported.")
    {

    }
}
