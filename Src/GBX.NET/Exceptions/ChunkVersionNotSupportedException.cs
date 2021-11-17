namespace GBX.NET.Exceptions;

public class ChunkVersionNotSupportedException : Exception
{
    public ChunkVersionNotSupportedException(Chunk chunk, int version) : base($"Chunk version {version} is not yet supported.")
    {
    }
}
