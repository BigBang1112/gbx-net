namespace GBX.NET.Engines.Game;

/// <remarks>ID: 0x03095000</remarks>
[Class(0x03095000)]
public partial class CGamePlayerOfficialScores : CMwNod, IClass
{
    [Hexadecimal] public static new uint Id => 0x03095000;




    private TransactionalNatural? globalScore;
    [AppliedWithChunk<Chunk03095000>]
    [AppliedWithChunk<Chunk03095001>]
    public TransactionalNatural? GlobalScore { get => globalScore; set => globalScore = value; }

    private FilteredPlayerRank[]? filteredPlayerRanks;
    [AppliedWithChunk<Chunk03095000>]
    [AppliedWithChunk<Chunk03095001>]
    public FilteredPlayerRank[]? FilteredPlayerRanks { get => filteredPlayerRanks; set => filteredPlayerRanks = value; }

    /// <summary>
    /// Creates a new instance of <see cref="CGamePlayerOfficialScores"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public CGamePlayerOfficialScores() { }


    /// <summary>
    /// CGamePlayerOfficialScores 0x000 chunk
    /// </summary>
    [Chunk(0x03095000)]
    public partial class Chunk03095000 : Chunk<CGamePlayerOfficialScores>
    {
        /// <inheritdoc />
        public override uint Id => 0x03095000;


        public override void ReadWrite(CGamePlayerOfficialScores n, GbxReaderWriter rw)
        {
            rw.ReadableWritable<TransactionalNatural>(ref n.globalScore);
            rw.ArrayReadableWritable<FilteredPlayerRank>(ref n.filteredPlayerRanks!);
        }
    }

    /// <summary>
    /// CGamePlayerOfficialScores 0x001 chunk
    /// </summary>
    [Chunk(0x03095001)]
    public partial class Chunk03095001 : Chunk<CGamePlayerOfficialScores>
    {
        /// <inheritdoc />
        public override uint Id => 0x03095001;

        public bool U02;
        public DateTime? U03;
        public byte U04;
        public DateTime? U05;

        public override void ReadWrite(CGamePlayerOfficialScores n, GbxReaderWriter rw)
        {
            rw.ReadableWritable<TransactionalNatural>(ref n.globalScore);
            rw.Boolean(ref U02, asByte: true);
            if (U02)
            {
                rw.SystemTime(ref U03);
                rw.Byte(ref U04);
            }
            rw.SystemTime(ref U05);
            rw.ArrayReadableWritable<FilteredPlayerRank>(ref n.filteredPlayerRanks!);
        }
    }


    public sealed partial class FilteredPlayerRank : IReadableWritable
    {

        private string? zone;
        public string? Zone { get => zone; set => zone = value; }

        private bool u01;
        public bool U01 { get => u01; set => u01 = value; }

        private CGameHighScore? score;
        public CGameHighScore? Score { get => score; set => score = value; }

        public void ReadWrite(GbxReaderWriter rw, int v = 0)
        {
            rw.String(ref zone);
            rw.Boolean(ref u01);
            rw.NodeRef<CGameHighScore>(ref score);
        }
    }

    public sealed partial class TransactionalNatural : IReadableWritable
    {

        private bool u01;
        public bool U01 { get => u01; set => u01 = value; }

        private int u02;
        public int U02 { get => u02; set => u02 = value; }

        private int value;
        public int Value { get => value; set => value = value; }

        private int u03;
        public int U03 { get => u03; set => u03 = value; }

        private bool u04;
        public bool U04 { get => u04; set => u04 = value; }

        public void ReadWrite(GbxReaderWriter rw, int v = 0)
        {
            rw.Boolean(ref u01, asByte: true);
            if (U01)
            {
                rw.Int32(ref u02);
                rw.Int32(ref value);
                rw.Int32(ref u03);
                rw.Boolean(ref u04);
            }
        }
    }



    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x03095000 => new Chunk03095000(),
        0x03095001 => new Chunk03095001(),
        _ => base.NewChunk(chunkId),
    };
}
