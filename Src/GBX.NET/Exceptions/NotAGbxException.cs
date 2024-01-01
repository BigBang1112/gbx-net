namespace GBX.NET.Exceptions;

[Serializable]
public class NotAGbxException : Exception
{
	public NotAGbxException() : base("Gbx data stream was not identified.") { }
	public NotAGbxException(string message) : base(message) { }
	public NotAGbxException(string message, Exception? innerException) : base(message, innerException) { }
}
