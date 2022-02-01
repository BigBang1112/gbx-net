namespace GBX.NET.Exceptions;

/// <summary>
/// The exception that is thrown when a string length is outside of a valid range.
/// </summary>
public class StringLengthOutOfRangeException : Exception
{
    public int Length { get; }

    public StringLengthOutOfRangeException(int length)
        : base($"The string was outside of the valid range: {length}")
    {
        Length = length;
    }

    public StringLengthOutOfRangeException(string? message) : base(message)
    {

    }

    public StringLengthOutOfRangeException(string? message, Exception? innerException) : base(message, innerException)
    {

    }
}
