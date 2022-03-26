namespace GBX.NET;

public interface IXorTrickStream
{
    void InitializeXorTrick(byte[] bytes, uint offset, uint count);
}