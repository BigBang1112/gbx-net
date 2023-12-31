namespace GBX.NET.Engines.Plug;

/// <remarks>ID: 0x0902F000</remarks>
[Node(0x0902F000)]
public class CPlugFileGen : CPlugFileImg
{
    [NodeMember(ExactlyNamed = true)]
    public int Version { get; set; }

    [NodeMember(ExactlyNamed = true)]
    public int? GenKind { get; set; }

    [NodeMember(ExactlyNamed = true)]
    public int[]? GenNatural { get; set; }

    [NodeMember]
    public Vec4[]? GenColor { get; set; } // GxColor

    public float[]? U05 { get; set; }

    internal CPlugFileGen()
    {

    }

    protected override void ReadChunkData(GameBoxReader r, IProgress<GameBoxReadProgress>? progress, bool ignoreZeroIdChunk)
    {
        Version = r.ReadInt32();

        if (Version < 0)
        {
            Version &= 0x7fffffff;

            GenKind = r.ReadInt32();
            GenNatural = r.ReadArray<int>();
            GenColor = r.ReadArray<Vec4>();
            U05 = r.ReadArray<float>();
        }
        else
        {
            GenNatural = r.ReadArray<int>();
            GenColor = r.ReadArray<Vec4>();
        }
    }

    protected override Task ReadChunkDataAsync(GameBoxReader r, CancellationToken cancellationToken)
    {
        ReadChunkData(r, progress: null, ignoreZeroIdChunk: false);
        return Task.CompletedTask;
    }
}
