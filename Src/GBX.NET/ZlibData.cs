namespace GBX.NET;

public sealed class ZlibData(int uncompressedSize, byte[] data, Exception? exception) : IDeepCloneable
{
    public int UncompressedSize { get; } = uncompressedSize;
    public byte[] Data { get; } = data;
    public Exception? Exception { get; set; } = exception;

    public bool Parsed { get; set; }

    object IDeepCloneable.DeepClone(DeepCloneContext context)
    {
        var clone = new ZlibData(UncompressedSize, context.CloneArray(Data)!, Exception) { Parsed = Parsed };
        context.Register(this, clone);
        return clone;
    }

    internal static GbxReader OpenDecompressedReader(int uncompressedSize, byte[] data, GbxReader? referenceReader = null)
    {
        using var compressedStream = new MemoryStream(data);
        var uncompressedStream = new MemoryStream(uncompressedSize);

        Gbx.ZLib.Decompress(compressedStream, uncompressedStream);

        var rBuffer = new GbxReader(uncompressedStream);

        if (referenceReader is not null)
        {
            rBuffer.LoadFrom(referenceReader);
        }

        uncompressedStream.Position = 0;
        return rBuffer;
    }

    public GbxReader OpenDecompressedReader()
    {
        return OpenDecompressedReader(UncompressedSize, Data);
    }

    public override string ToString()
    {
        return $"ZlibData ({ByteHelper.ToByteSize(Data.Length)} / {ByteHelper.ToByteSize(UncompressedSize)}, ratio: {(double)Data.Length / UncompressedSize:P2}, parsed: {Parsed})";
    }
}
