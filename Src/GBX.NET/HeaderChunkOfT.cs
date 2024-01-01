namespace GBX.NET;

public interface IHeaderChunk<T> : IReadableChunk<T>, IWritableChunk<T>, IHeaderChunk where T : IClass
{

}

public abstract class HeaderChunk<T> : Chunk<T>, IHeaderChunk<T> where T : IClass
{
    public bool IsHeavy { get; set; }
}