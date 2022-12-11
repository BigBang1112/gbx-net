namespace GBX.NET.Engines.Game;

/// <summary>
/// A list of puzzle pieces.
/// </summary>
/// <remarks>ID: 0x0301B000</remarks>
[Node(0x0301B000)]
public partial class CGameCtnCollectorList : CMwNod
{
    private IList<Collector> collectorStock;

    [NodeMember]
    [AppliedWithChunk<Chunk0301B000>]
    public IList<Collector> CollectorStock { get => collectorStock; set => collectorStock = value; }

    internal CGameCtnCollectorList()
    {
        collectorStock = Array.Empty<Collector>();
    }

    /// <summary>
    /// CGameCtnCollectorList 0x000 chunk
    /// </summary>
    [Chunk(0x0301B000)]
    public class Chunk0301B000 : Chunk<CGameCtnCollectorList>
    {
        public override void ReadWrite(CGameCtnCollectorList n, GameBoxReaderWriter rw)
        {
            rw.List(ref n.collectorStock!, r => new Collector
            {
                Ident = r.ReadIdent(),
                Count = r.ReadInt32()
            },
            (x, w) =>
            {
                w.Write(x.Ident);
                w.Write(x.Count);
            });
        }
    }
}
