namespace GBX.NET;

public interface ISkippableChunk : IReadableWritableChunk
{
    bool Discovered { get; set; }
    byte[] Data { get; set; }

    void Write(GameBoxWriter w);
    Task WriteAsync(GameBoxWriter w, CancellationToken cancellationToken);
    void Discover();
}
