namespace GBX.NET.Exceptions;

/// <summary>
/// The exception that is thrown when a version of certain data wasn't yet verified and could cause parsing exceptions.
/// </summary>
public class VersionNotSupportedException : Exception
{
    private static string GetMessage(int version) => $"Version {version} is not supported.";

    public VersionNotSupportedException(int version) : base(GetMessage(version))
    {

    }

    public VersionNotSupportedException(string? message) : base(message)
    {
        
    }

    public VersionNotSupportedException(int version, Exception? innerException) : base(GetMessage(version), innerException)
    {

    }

    public VersionNotSupportedException(string? message, Exception? innerException) : base(message, innerException)
    {

    }
}
