namespace GBX.NET.Exceptions;

public class NotAGbxException : Exception
{
    public NotAGbxException() : base("Gbx data stream was not identified.")
    {

    }
}
