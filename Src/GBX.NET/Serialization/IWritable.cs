namespace GBX.NET.Serialization;

public interface IWritable
{
    void Write(GbxWriter w, int version = 0);
}
