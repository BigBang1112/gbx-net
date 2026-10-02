namespace GBX.NET;

public sealed class RawData(byte[] data, Exception? exception) : IDeepCloneable
{
    public byte[] Data { get; } = data;
    public Exception? Exception { get; set; } = exception;

    public bool Parsed { get; set; }

    object IDeepCloneable.DeepClone(DeepCloneContext context)
    {
        var clone = new RawData(context.CloneArray(Data)!, Exception) { Parsed = Parsed };
        context.Register(this, clone);
        return clone;
    }

    public override string ToString()
    {
        return $"RawData ({ByteHelper.ToByteSize(Data.Length)}, parsed: {Parsed})";
    }
}
