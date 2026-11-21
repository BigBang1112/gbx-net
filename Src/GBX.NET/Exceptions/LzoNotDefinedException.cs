namespace GBX.NET.Exceptions;

[Serializable]
public class LzoNotDefinedException : InvalidOperationException
{
    public LzoNotDefinedException() : base("LZO compression is not defined. Include GBX.NET.LZO and set 'Gbx.LZO = new Lzo()' to fix this problem.") { }
    public LzoNotDefinedException(string message) : base(message) { }
    public LzoNotDefinedException(string message, Exception? innerException) : base(message, innerException) { }
}