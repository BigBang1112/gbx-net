namespace GBX.NET.Extensions;

public interface IZLib
{
    void Compress(Stream input, Stream output);
    void Decompress(Stream input, Stream output);
    Stream Decompress(Stream input);
}
