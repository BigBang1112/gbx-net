namespace GBX.NET;

public interface IReadableWritableWithGbx : IReadableWritable
{
    void ReadWrite(GameBoxReaderWriter rw, GameBox? gbx, int version = 0);
}