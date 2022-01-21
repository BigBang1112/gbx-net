namespace GBX.NET.Exceptions;

public class HeaderOnlyParseLimitationException : Exception
{
    public override string Message => "This action is forbidden in GameBox where only the header was parsed.";

    public HeaderOnlyParseLimitationException()
    {

    }

    public HeaderOnlyParseLimitationException(string? message) : base(message)
    {

    }

    public HeaderOnlyParseLimitationException(string? message, Exception? innerException) : base(message, innerException)
    {

    }
}
