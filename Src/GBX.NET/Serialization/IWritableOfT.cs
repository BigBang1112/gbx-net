namespace GBX.NET.Serialization;

public interface IWritable<T> where T : IClass
{
    void Write(GbxWriter w, T n, int v = 0);
}
