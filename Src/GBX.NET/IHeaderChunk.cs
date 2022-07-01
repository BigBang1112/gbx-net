namespace GBX.NET;

public interface IHeaderChunk : IReadableWritableChunk
{
    byte[] Data { get; set; }
    bool IsHeavy { get; set; }
    
    void Write(GameBoxWriter w);
    Task WriteAsync(GameBoxWriter w, CancellationToken cancellationToken);
}
