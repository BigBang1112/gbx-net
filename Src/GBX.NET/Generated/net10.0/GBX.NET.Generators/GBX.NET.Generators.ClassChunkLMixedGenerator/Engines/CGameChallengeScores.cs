namespace GBX.NET.Engines.Game;

/// <remarks>ID: 0x03064000</remarks>
[Class(0x03064000)]
public partial class CGameChallengeScores : CMwNod, IClass
{
    [Hexadecimal] public static new uint Id => 0x03064000;




    /// <summary>
    /// Creates a new instance of <see cref="CGameChallengeScores"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public CGameChallengeScores() { }


    /// <summary>
    /// CGameChallengeScores 0x001 chunk
    /// </summary>
    [Chunk(0x03064001)]
    public partial class Chunk03064001 : Chunk<CGameChallengeScores>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x03064001;

        public int Version { get; set; }

        public string? U01;
        public string? U02;
        public bool U03;
        public bool U04;
        public int U05;
        public DateTime? U06;
        public Unknown[]? U07;

        public override void ReadWrite(CGameChallengeScores n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.Id(ref U01);
            rw.String(ref U02);
            rw.Boolean(ref U03);
            rw.Boolean(ref U04);
            rw.Int32(ref U05);
            rw.SystemTime(ref U06);
            rw.ArrayReadableWritable<Unknown>(ref U07!, version: Version);
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

        private string? u01;
        public string? U01 { get => u01; set => u01 = value; }

        private string? u02;
        public string? U02 { get => u02; set => u02 = value; }

        public void ReadWrite(GbxReaderWriter rw, int v = 0)
        {
            rw.Int32(ref rank);
            rw.Int32(ref score);
            rw.String(ref login);
            rw.String(ref nickname);
            if (v >= 2)
            {
                rw.String(ref u01);
                rw.String(ref u02);
            }
        }
    }

    public sealed partial class Unknown : IReadableWritable
    {

        private string? u01;
        public string? U01 { get => u01; set => u01 = value; }

        private bool u02;
        public bool U02 { get => u02; set => u02 = value; }

        private RecordsBuffer? u03;
        public RecordsBuffer? U03 { get => u03; set => u03 = value; }

        private int scoreCount;
        public int ScoreCount { get => scoreCount; set => scoreCount = value; }

        private byte u04;
        public byte U04 { get => u04; set => u04 = value; }

        private DateTime? u05;
        public DateTime? U05 { get => u05; set => u05 = value; }

        private byte u06;
        public byte U06 { get => u06; set => u06 = value; }

        private DateTime? u07;
        public DateTime? U07 { get => u07; set => u07 = value; }

        private ChampionRanking[]? u08;
        public ChampionRanking[]? U08 { get => u08; set => u08 = value; }

        public void ReadWrite(GbxReaderWriter rw, int v = 0)
        {
            rw.String(ref u01);
            rw.Boolean(ref u02);
            rw.ReadableWritable<RecordsBuffer>(ref u03);
            rw.Int32(ref scoreCount);
            rw.Byte(ref u04);
            rw.SystemTime(ref u05);
            rw.Byte(ref u06);
            rw.SystemTime(ref u07);
            rw.ArrayReadableWritable<ChampionRanking>(ref u08!, ScoreCount, version: v);
        }
    }



    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x03064001 => new Chunk03064001(),
        _ => base.NewChunk(chunkId),
    };
}
