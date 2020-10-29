namespace GBX.NET
{
    public interface IHeaderChunk : ISkippableChunk
    {
        bool IsHeavy { get; set; }
    }
}
