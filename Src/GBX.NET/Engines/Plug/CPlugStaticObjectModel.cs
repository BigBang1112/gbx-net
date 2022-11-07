namespace GBX.NET.Engines.Plug;

/// <remarks>ID: 0x09159000</remarks>
[Node(0x09159000)]
public class CPlugStaticObjectModel : CMwNod
{
    public int U01 { get; set; }

    [NodeMember(ExactlyNamed = true)]
    public CPlugSolid2Model? Mesh { get; set; }
    
    public byte U02 { get; set; }
    public int U03 { get; set; }
    public Iso4 U04 { get; set; }
    public int U05 { get; set; }
    public int U06 { get; set; }
    public int U07 { get; set; }
    public int U08 { get; set; }
    public int U09 { get; set; }
    public int U10 { get; set; }
    public int U11 { get; set; }
    public int U12 { get; set; }
    public Iso4 U13 { get; set; }
    public int U14 { get; set; }

    internal CPlugStaticObjectModel()
    {

    }

    protected override void ReadChunkData(GameBoxReader r, IProgress<GameBoxReadProgress>? progress, bool ignoreZeroIdChunk)
    {
        U01 = r.ReadInt32();
        Mesh = r.ReadNodeRef<CPlugSolid2Model>();
        U02 = r.ReadByte();
        U03 = r.ReadInt32();
        U04 = r.ReadIso4();
        U05 = r.ReadInt32();
        U06 = r.ReadInt32();
        U07 = r.ReadInt32();
        U08 = r.ReadInt32();
        U09 = r.ReadInt32();
        U10 = r.ReadInt32();
        U11 = r.ReadInt32();
        U12 = r.ReadInt32();
        U13 = r.ReadIso4();
        U14 = r.ReadInt32();
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
        w.Write(U03);
        w.Write(U04);
        w.Write(U05);
        w.Write(U06);
        w.Write(U07);
        w.Write(U08);
        w.Write(U09);
        w.Write(U10);
        w.Write(U11);
        w.Write(U12);
        w.Write(U13);
        w.Write(U14);
    }

    protected override Task WriteChunkDataAsync(GameBoxWriter w, CancellationToken cancellationToken)
    {
        WriteChunkData(w);
        return Task.CompletedTask;
    }
}