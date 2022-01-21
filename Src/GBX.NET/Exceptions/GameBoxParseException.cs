using System.Runtime.Serialization;

namespace GBX.NET.Exceptions;

/// <summary>
/// The exception that is thrown when generic parse issue happens. This exception is temporary and will be obsolete soon.
/// </summary>
public class GameBoxParseException : Exception
{
    public override string Message => "GBX couldn't be parsed.";

    public GameBoxParseException()
    {

    }

    public GameBoxParseException(string? message) : base(message)
    {

    }

    public GameBoxParseException(string? message, Exception? innerException) : base(message, innerException)
    {

    }
}
