namespace GBX.NET;

public interface IReadableWritable
{
    void ReadWrite(IGbxReaderWriter rw, int version = 0);
}
