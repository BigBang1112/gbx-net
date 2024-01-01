namespace GBX.NET.Exceptions;

[Serializable]
public class BooleanOutOfRangeException : Exception
{
    public BooleanOutOfRangeException(uint value, Exception? innerException = null) : base($"Boolean value is out of range: {value}", innerException)
    {
    }
}
