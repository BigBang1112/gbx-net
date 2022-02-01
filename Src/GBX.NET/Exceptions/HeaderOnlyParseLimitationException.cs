namespace GBX.NET.Exceptions;

public class HeaderOnlyParseLimitationException : Exception
{
    public HeaderOnlyParseLimitationException() : base("This action is forbidden in GameBox where only the header was parsed.")
    {

    }

    public HeaderOnlyParseLimitationException(string? message) : base(message)
    {

    }

    public HeaderOnlyParseLimitationException(string? message, Exception? innerException) : base(message, innerException)
    {

    }
}
