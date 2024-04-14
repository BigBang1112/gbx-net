namespace GBX.NET.Exceptions;

/// <summary>
/// The exception that is thrown when a hypothetically impossible state is detected.
/// </summary>
public class ThisShouldNotHappenException : Exception
{
    public ThisShouldNotHappenException() : base("This should not happen.")
    {

    }

    public ThisShouldNotHappenException(string? message) : base(message)
    {

    }

    public ThisShouldNotHappenException(string? message, Exception? innerException) : base(message, innerException)
    {

    }
}