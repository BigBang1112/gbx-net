#if NET5_0_OR_GREATER
namespace GBX.NET.Crypto;

public sealed class EncryptionInitializer : IEncryptionInitializer
{
    private readonly BlowfishStream stream;

    public EncryptionInitializer(BlowfishStream stream)
    {
        this.stream = stream;
    }

    public void Initialize(byte[] data, uint offset, uint count)
    {
        stream.Initialize(data, offset, count);
    }
}
#endif
