namespace GBX.NET.Exceptions;

[Serializable]
public class FormatNotSupportedException : Exception
{
    public FormatNotSupportedException(GbxFormat format) : base($"Unsupported format: {format}") { }
    public FormatNotSupportedException() : base("Unsupported format.") { }
    public FormatNotSupportedException(string message) : base(message) { }
    public FormatNotSupportedException(string message, Exception? innerException) : base(message, innerException) { }
}