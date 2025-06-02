namespace GBX.NET.PAK;

public class LZ4Exception : Exception
{
    public LZ4Exception() { }
    public LZ4Exception(string message) : base("[LZ4] " + message) { }
    public LZ4Exception(string message, Exception innerException) : base(message, innerException) { }
}
