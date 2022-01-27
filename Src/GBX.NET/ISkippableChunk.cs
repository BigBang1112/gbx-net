namespace GBX.NET;

public interface ISkippableChunk : IReadableWritableChunk
{
    bool Discovered { get; set; }
    byte[] Data { get; set; }

    void Write(GameBoxWriter w);
    void Discover();
}
