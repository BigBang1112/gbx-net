namespace GBX.NET.Engines.Plug;

/// <remarks>ID: 0x0902F000</remarks>
[Node(0x0902F000)]
public class CPlugFileGen : CPlugFileImg
{
    public int U01 { get; set; }
    public int? U02 { get; set; }
    public int[]? U03 { get; set; }
    public Vec4[]? U04 { get; set; }
    public float[]? U05 { get; set; }

    internal CPlugFileGen()
    {

    }

    protected override void ReadChunkData(GameBoxReader r, IProgress<GameBoxReadProgress>? progress, bool ignoreZeroIdChunk)
    {
        U01 = r.ReadInt32();

        if (U01 < 0)
        {
            U01 &= 0x7fffffff;

            U02 = r.ReadInt32();
            U03 = r.ReadArray<int>();
            U04 = r.ReadArray<Vec4>();
            U05 = r.ReadArray<float>();
        }
        else
        {
            U03 = r.ReadArray<int>();
            U04 = r.ReadArray<Vec4>();
        }
    }

    protected override Task ReadChunkDataAsync(GameBoxReader r, CancellationToken cancellationToken)
    {
        ReadChunkData(r, progress: null, ignoreZeroIdChunk: false);
        return Task.CompletedTask;
    }
}
