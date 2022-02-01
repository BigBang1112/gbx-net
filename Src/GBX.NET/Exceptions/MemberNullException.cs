namespace GBX.NET.Exceptions;

/// <summary>
/// The exception that is thrown when a node member is null when it shouldn't be.
/// </summary>
public class MemberNullException : Exception
{
    public MemberNullException(string memberName) : base($"Member '{memberName}' is null.")
    {

    }
}
