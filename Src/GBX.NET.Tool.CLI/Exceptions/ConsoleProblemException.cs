namespace GBX.NET.Tool.CLI.Exceptions;

public class ConsoleProblemException : Exception
{
    public ConsoleProblemException()
    {
    }

    public ConsoleProblemException(string? message) : base(message)
    {
    }

    public ConsoleProblemException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}
