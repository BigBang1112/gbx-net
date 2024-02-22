namespace GBX.NET.Exceptions;

[Serializable]
public class TextFormatNotSupportedException : Exception
{
    public TextFormatNotSupportedException() : base("Text-formatted Gbx files are not supported.") { }
    public TextFormatNotSupportedException(string message) : base(message) { }
    public TextFormatNotSupportedException(string message, Exception? innerException) : base(message, innerException) { }
}
