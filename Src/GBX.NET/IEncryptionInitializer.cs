namespace GBX.NET;

public interface IEncryptionInitializer
{
    void Initialize(byte[] data, uint offset, uint count);
}
