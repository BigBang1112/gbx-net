namespace GBX.NET.PAK.Exceptions;

[Serializable]
public class NotAPakException : Exception
{
    public NotAPakException() : base("Pak data stream was not identified.") { }
    public NotAPakException(string message) : base(message) { }
    public NotAPakException(string message, Exception? innerException) : base(message, innerException) { }
}
