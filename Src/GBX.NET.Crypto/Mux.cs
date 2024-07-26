namespace GBX.NET.Crypto;

public sealed class Mux
{
    public byte Version { get; }
    public int KeySalt { get; }
    public byte[] Data { get; }

    public Mux(byte version, int keySalt, byte[] data)
    {
        Version = version;
        KeySalt = keySalt;
        Data = data;
    }
}
