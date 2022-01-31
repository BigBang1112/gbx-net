namespace GBX.NET.Exceptions;

public class CompressedRefTableException : Exception
{
    public override string Message => "Compressed reference table is not known to read.";
}
