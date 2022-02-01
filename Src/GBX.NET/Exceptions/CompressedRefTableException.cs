namespace GBX.NET.Exceptions;

public class CompressedRefTableException : Exception
{
    public CompressedRefTableException() : base("Compressed reference table is not known to read.")
    {

    }
}
