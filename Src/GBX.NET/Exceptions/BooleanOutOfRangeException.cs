namespace GBX.NET.Exceptions;

public class BooleanOutOfRangeException : Exception
{
    public BooleanOutOfRangeException(int value) : base($"Boolean value is out of range: {value}")
    {

    }
}