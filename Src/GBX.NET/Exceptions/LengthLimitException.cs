namespace GBX.NET.Exceptions;

[Serializable]
public class LengthLimitException : Exception
{
    public LengthLimitException(int length) : this($"Length exceeded allowed maximum ({length}).") { }
    public LengthLimitException(string message) : base(message) { }
	public LengthLimitException(string message, Exception inner) : base(message, inner) { }
}