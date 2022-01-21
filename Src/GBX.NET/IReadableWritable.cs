namespace GBX.NET;

public interface IReadableWritable
{
    void ReadWrite(GameBoxReaderWriter rw, int version = 0);
}