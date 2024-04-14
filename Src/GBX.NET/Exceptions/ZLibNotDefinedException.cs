namespace GBX.NET.Exceptions;

[Serializable]
public class ZLibNotDefinedException : Exception
{
    public ZLibNotDefinedException() : base("ZLib compression is not defined. Include GBX.NET.ZLib and set 'Gbx.ZLib = new ZLib()' to fix this problem.") { }
    public ZLibNotDefinedException(string message) : base(message) { }
    public ZLibNotDefinedException(string message, Exception? innerException) : base(message, innerException) { }
}