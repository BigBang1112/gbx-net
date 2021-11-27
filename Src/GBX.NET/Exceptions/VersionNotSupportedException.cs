namespace GBX.NET.Exceptions;

public class VersionNotSupportedException : Exception
{
    public VersionNotSupportedException(int version) : base($"Version {version} is not supported.")
    {
    }
}
