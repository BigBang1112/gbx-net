namespace GBX.NET.Engines.Plug;

/// <remarks>ID: 0x09159000</remarks>
[Node(0x09159000)]
public class CPlugStaticObjectModel : CMwNod
{
    public int U01 { get; set; }

    [NodeMember(ExactlyNamed = true)]
    public CPlugSolid2Model? Mesh { get; set; }
    
    public byte U02 { get; set; }

    internal CPlugStaticObjectModel()
    {

    }

    protected override void ReadChunkData(GameBoxReader r, IProgress<GameBoxReadProgress>? progress, bool ignoreZeroIdChunk)
    {
        U01 = r.ReadInt32();
        Mesh = r.ReadNodeRef<CPlugSolid2Model>();
        U02 = r.ReadByte();
    }

    protected override Task ReadChunkDataAsync(GameBoxReader r, CancellationToken cancellationToken)
    {
        ReadChunkData(r, null, false);
        return Task.CompletedTask;
    }

    protected override void WriteChunkData(GameBoxWriter w)
    {
        w.Write(U01);
        w.Write(Mesh);
        w.Write(U02);
    }

    protected override Task WriteChunkDataAsync(GameBoxWriter w, CancellationToken cancellationToken)
    {
        WriteChunkData(w);
        return Task.CompletedTask;
    }
}