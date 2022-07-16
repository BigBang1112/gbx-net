namespace GBX.NET.Exceptions;

public class BooleanOutOfRangeException : Exception
{
    public BooleanOutOfRangeException(uint value) : base($"Boolean value is out of range: {value}")
    {

    }
}