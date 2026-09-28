namespace GBX.NET.Engines.Game;

/// <summary>
/// A list of puzzle pieces.
/// </summary>
/// <remarks>ID: 0x0301B000</remarks>
[Class(0x0301B000)]
public partial class CGameCtnCollectorList : CMwNod, IClass
{
    [Hexadecimal] public static new uint Id => 0x0301B000;




    private List<SCollectorStock>? collectorStock;
    [AppliedWithChunk<Chunk0301B000>]
    public List<SCollectorStock>? CollectorStock { get => collectorStock; set => collectorStock = value; }

    /// <summary>
    /// Creates a new instance of <see cref="CGameCtnCollectorList"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public CGameCtnCollectorList() { }


    /// <summary>
    /// CGameCtnCollectorList 0x000 chunk
    /// </summary>
    [Chunk(0x0301B000)]
    public partial class Chunk0301B000 : Chunk<CGameCtnCollectorList>
    {
        /// <inheritdoc />
        public override uint Id => 0x0301B000;


        public override void ReadWrite(CGameCtnCollectorList n, GbxReaderWriter rw)
        {
            rw.ListReadableWritable<SCollectorStock>(ref n.collectorStock!);
        }
    }


    public sealed partial class SCollectorStock : IReadableWritable
    {

        private Ident? blockModel;
        public Ident? BlockModel { get => blockModel; set => blockModel = value; }

        private int count;
        public int Count { get => count; set => count = value; }

        public void ReadWrite(GbxReaderWriter rw, int v = 0)
        {
            rw.Ident(ref blockModel);
            rw.Int32(ref count);
        }
    }



    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x0301B000 => new Chunk0301B000(),
        _ => base.NewChunk(chunkId),
    };
}
