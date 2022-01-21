namespace GBX.NET.Exceptions;

public static class ChunkVersionNotSeenExceptionHelper
{
    public static void ThrowIfVersionNotSeen(int version)
    {
        if (!GameBox.IgnoreUnseenVersions)
        {
            throw new ChunkVersionNotSeenException(version);
        }
    }
}
