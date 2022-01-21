namespace GBX.NET.Exceptions;

/// <summary>
/// The exception that is thrown when a chunk version wasn't yet verified but shouldn't cause exceptions.
/// </summary>
public class ChunkVersionNotSeenException : Exception
{
    private static string GetMessage(int version) => $"Chunk version {version} hasn't been seen yet, but reading and writing should work just fine. You can disable GameBox.IgnoreUnseenVersions to not throw this again.";

    public ChunkVersionNotSeenException(int version) : base(GetMessage(version))
    {

    }
}
