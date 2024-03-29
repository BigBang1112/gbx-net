namespace GBX.NET.Serialization;

public interface IReadable
{
    void Read(GbxReader r, int v = 0);
}
