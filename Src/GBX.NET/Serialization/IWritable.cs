namespace GBX.NET.Serialization;

public interface IWritable
{
    void Write(IGbxWriter w, int version = 0);
}
