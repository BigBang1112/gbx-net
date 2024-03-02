namespace GBX.NET.Exceptions;

[Serializable]
public class VersionNotSupportedException : Exception
{
	public int Version { get; init; }

	public VersionNotSupportedException(int version) : base($"Version {version} is not supported.")
	{
        Version = version;
    }

	public VersionNotSupportedException(string message) : base(message) { }
	public VersionNotSupportedException(string message, Exception? innerException) : base(message, innerException) { }
}
