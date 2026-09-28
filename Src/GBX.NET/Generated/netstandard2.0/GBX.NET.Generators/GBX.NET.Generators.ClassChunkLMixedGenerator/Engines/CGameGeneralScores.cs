namespace GBX.NET.Engines.Game;

/// <remarks>ID: 0x03065000</remarks>
[Class(0x03065000)]
public partial class CGameGeneralScores : CMwNod, IClass
{
    [Hexadecimal] public static new uint Id => 0x03065000;




    /// <summary>
    /// Creates a new instance of <see cref="CGameGeneralScores"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public CGameGeneralScores() { }


    /// <summary>
    /// CGameGeneralScores 0x001 chunk
    /// </summary>
    [Chunk(0x03065001)]
    public partial class Chunk03065001 : Chunk<CGameGeneralScores>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x03065001;

        public int Version { get; set; }

        public Unknown[]? U01;

        public override void ReadWrite(CGameGeneralScores n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.ArrayReadableWritable<Unknown>(ref U01!);
        }
    }


    public sealed partial class RecordsBuffer : IReadableWritable
    {

        private int version;
        public int Version { get => version; set => version = value; }

        private int u01;
        public int U01 { get => u01; set => u01 = value; }

        private bool u02;
        public bool U02 { get => u02; set => u02 = value; }

        private OldShowTime[]? u03;
        public OldShowTime[]? U03 { get => u03; set => u03 = value; }

        public void ReadWrite(GbxReaderWriter rw, int v = 0)
        {
            rw.Int32(ref this.version);
            rw.Int32(ref u01);
            rw.Boolean(ref u02);
            rw.ArrayReadableWritable<OldShowTime>(ref u03!);
        }
    }

    public sealed partial class OldShowTime : IReadableWritable
    {

        private int u01;
        public int U01 { get => u01; set => u01 = value; }

        private int u02;
        public int U02 { get => u02; set => u02 = value; }

        public void ReadWrite(GbxReaderWriter rw, int v = 0)
        {
            rw.Int32(ref u01);
            rw.Int32(ref u02);
        }
    }

    public sealed partial class ChampionRanking : IReadableWritable
    {

        private int rank;
        public int Rank { get => rank; set => rank = value; }

        private int score;
        public int Score { get => score; set => score = value; }

        private string? login;
        public string? Login { get => login; set => login = value; }

        private string? nickname;
        public string? Nickname { get => nickname; set => nickname = value; }

        public void ReadWrite(GbxReaderWriter rw, int v = 0)
        {
            rw.Int32(ref rank);
            rw.Int32(ref score);
            rw.String(ref login);
            rw.String(ref nickname);
        }
    }

    public sealed partial class Unknown : IReadableWritable
    {

        private string? u01;
        public string? U01 { get => u01; set => u01 = value; }

        private bool u02;
        public bool U02 { get => u02; set => u02 = value; }

        private byte u03;
        public byte U03 { get => u03; set => u03 = value; }

        private DateTime? u04;
        public DateTime? U04 { get => u04; set => u04 = value; }

        private byte u05;
        public byte U05 { get => u05; set => u05 = value; }

        private DateTime? u06;
        public DateTime? U06 { get => u06; set => u06 = value; }

        private RecordsBuffer? u07;
        public RecordsBuffer? U07 { get => u07; set => u07 = value; }

        private ChampionRanking[]? u08;
        public ChampionRanking[]? U08 { get => u08; set => u08 = value; }

        public void ReadWrite(GbxReaderWriter rw, int v = 0)
        {
            rw.String(ref u01);
            rw.Boolean(ref u02);
            rw.Byte(ref u03);
            rw.SystemTime(ref u04);
            rw.Byte(ref u05);
            rw.SystemTime(ref u06);
            rw.ReadableWritable<RecordsBuffer>(ref u07);
            rw.ArrayReadableWritable<ChampionRanking>(ref u08!);
        }
    }



    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x03065001 => new Chunk03065001(),
        _ => base.NewChunk(chunkId),
    };
}
