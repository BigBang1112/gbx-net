namespace GBX.NET.Serialization;

public interface IReadable
{
    void Read(IGbxReader r, int version = 0);
}
