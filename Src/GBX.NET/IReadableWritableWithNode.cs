namespace GBX.NET;

public interface IReadableWritableWithNode<T> : IReadableWritable where T : Node
{
    void ReadWrite(GameBoxReaderWriter rw, T node, int version = 0);
}