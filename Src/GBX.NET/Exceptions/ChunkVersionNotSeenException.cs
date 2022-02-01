namespace GBX.NET.Exceptions;

/// <summary>
/// The exception that is thrown when a chunk version wasn't yet verified but shouldn't cause exceptions.
/// </summary>
public class ChunkVersionNotSeenException : Exception
{
    public int Version { get; }

    public ChunkVersionNotSeenException(int version) : base(GetMessage(version))
    {
        Version = version;
    }

    private static string GetMessage(int version)
    {
        return $"Chunk version {version} hasn't been seen yet, but reading and writing should work just fine. You can disable GameBox.IgnoreUnseenVersions to not throw this again.";
    }
}
