namespace GBX.NET.Exceptions;

/// <summary>
/// The exception that is thrown when a node member is <see langword="null"/> when it shouldn't be.
/// </summary>
public class MemberNullException : Exception
{
    public MemberNullException()
    {
    }

    public MemberNullException(string memberName) : base($"Member '{memberName}' is null.")
    {

    }

    public MemberNullException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}