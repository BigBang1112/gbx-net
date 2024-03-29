namespace GBX.NET.Serialization;

public interface IWritable
{
    void Write(GbxWriter w, int v = 0);
}
