namespace GBX.NET.Engines.Game;

/// <summary>
/// Puzzle piece list (0x0301B000)
/// </summary>
/// <remarks>A list of puzzle pieces.</remarks>
[Node(0x0301B000)]
public class CGameCtnCollectorList : CMwNod
{
    #region Fields

    private Collector[] collectorStock;

    #endregion

    #region Properties

    [NodeMember]
    public Collector[] CollectorStock
    {
        get => collectorStock;
        set => collectorStock = value;
    }

    #endregion

    #region Constructors

    protected CGameCtnCollectorList()
    {
        collectorStock = null!;
    }

    #endregion

    #region Chunks

    #region 0x000 chunk

    /// <summary>
    /// CGameCtnCollectorList 0x000 chunk
    /// </summary>
    [Chunk(0x0301B000)]
    public class Chunk0301B000 : Chunk<CGameCtnCollectorList>
    {
        public override void ReadWrite(CGameCtnCollectorList n, GameBoxReaderWriter rw)
        {
            rw.Array(ref n.collectorStock!, r => new Collector
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

    #endregion

    #endregion

    #region Other classes

    public class Collector
    {
        public Ident Ident { get; set; } = new Ident();
        public int Count { get; set; }

        public override string ToString()
        {
            return $"{Count} {Ident.ID}";
        }
    }

    #endregion
}
