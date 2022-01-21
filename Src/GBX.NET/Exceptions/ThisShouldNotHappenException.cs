namespace GBX.NET.Exceptions;

/// <summary>
/// The exception that is thrown when a hypothetically impossible state is detected.
/// </summary>
public class ThisShouldNotHappenException : Exception
{
    public override string Message => "This should not happen.";

    public ThisShouldNotHappenException()
    {

    }

    public ThisShouldNotHappenException(string? message) : base(message)
    {

    }

    public ThisShouldNotHappenException(string? message, Exception? innerException) : base(message, innerException)
    {

    }
}
