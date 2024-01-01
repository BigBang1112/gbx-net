namespace GBX.NET;

public interface IHeaderChunk : IChunk
{
    bool IsHeavy { get; set; }
}

public sealed class HeaderChunk(uint id) : IHeaderChunk
{
    public uint Id { get; } = id;

    public bool IsHeavy { get; set; }
    public byte[] Data { get; set; } = [];
}