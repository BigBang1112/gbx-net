namespace GBX.NET.PAK;

public sealed class EncryptionInitializer : IEncryptionInitializer
{
    private readonly BlowfishStream stream;

    internal EncryptionInitializer(BlowfishStream stream)
    {
        this.stream = stream;
    }

    public void Initialize(byte[] data, uint offset, uint count)
    {
        stream.Initialize(data, offset, count);
    }
}
