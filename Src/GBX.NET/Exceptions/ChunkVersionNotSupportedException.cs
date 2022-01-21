namespace GBX.NET.Exceptions;

/// <summary>
/// The exception that is thrown when a chunk version wasn't yet verified and could cause parsing exceptions.
/// </summary>
public class ChunkVersionNotSupportedException : Exception
{
    private static string GetMessage(int version) => $"Chunk version {version} is not supported.";

    public ChunkVersionNotSupportedException(int version) : base(GetMessage(version))
    {

    }

    public ChunkVersionNotSupportedException(string? message) : base(message)
    {
        
    }

    public ChunkVersionNotSupportedException(int version, Exception? innerException) : base(GetMessage(version), innerException)
    {

    }

    public ChunkVersionNotSupportedException(string? message, Exception? innerException) : base(message, innerException)
    {

    }
}
