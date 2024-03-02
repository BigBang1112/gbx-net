namespace GBX.NET.Exceptions;

/// <summary>
/// The exception that is thrown when a chunk version wasn't yet verified and could cause parsing exceptions.
/// </summary>
[Serializable]
public class ChunkVersionNotSupportedException : VersionNotSupportedException
{
    public ChunkVersionNotSupportedException(int version) : base($"Chunk version {version} is not supported.")
    {
        Version = version;
    }
}