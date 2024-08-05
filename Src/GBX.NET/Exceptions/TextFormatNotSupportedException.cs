namespace GBX.NET.Exceptions;

[Serializable]
public class TextFormatNotSupportedException : FormatNotSupportedException
{
    public TextFormatNotSupportedException() : base("Text-formatted Gbx files are not YET supported.") { }
    public TextFormatNotSupportedException(string message) : base(message) { }
    public TextFormatNotSupportedException(string message, Exception? innerException) : base(message, innerException) { }
}
