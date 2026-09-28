namespace GBX.NET.Engines.Game;

/// <remarks>ID: 0x030EA000</remarks>
[Class(0x030EA000)]
public partial class CGameCampaignPlayerScores : CMwNod, IClass
{
    [Hexadecimal] public static new uint Id => 0x030EA000;




    private string? campaignId;
    [AppliedWithChunk<Chunk030EA001>]
    public string? CampaignId { get => campaignId; set => campaignId = value; }

    private SGameModeScores[]? gameModeScores;
    [AppliedWithChunk<Chunk030EA001>]
    public SGameModeScores[]? GameModeScores { get => gameModeScores; set => gameModeScores = value; }

    /// <summary>
    /// Creates a new instance of <see cref="CGameCampaignPlayerScores"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public CGameCampaignPlayerScores() { }


    /// <summary>
    /// CGameCampaignPlayerScores 0x001 chunk
    /// </summary>
    [Chunk(0x030EA001)]
    public partial class Chunk030EA001 : Chunk<CGameCampaignPlayerScores>
    {
        /// <inheritdoc />
        public override uint Id => 0x030EA001;

        public bool U01;
        public DateTime? U02;
        public byte U03;
        public DateTime? U04;
        public TransactionalNatural? U05;

        public override void ReadWrite(CGameCampaignPlayerScores n, GbxReaderWriter rw)
        {
            rw.Id(ref n.campaignId);
            rw.Boolean(ref U01, asByte: true);
            if (U01)
            {
                rw.SystemTime(ref U02);
                rw.Byte(ref U03);
            }
            rw.SystemTime(ref U04);
            rw.ReadableWritable<TransactionalNatural>(ref U05);
            rw.ArrayReadableWritable<SGameModeScores>(ref n.gameModeScores!);
        }
    }


    public sealed partial class SGameModeScores : IReadableWritable
    {

        private int u01;
        public int U01 { get => u01; set => u01 = value; }

        private TransactionalNatural? u02;
        public TransactionalNatural? U02 { get => u02; set => u02 = value; }

        public void ReadWrite(GbxReaderWriter rw, int v = 0)
        {
            rw.Int32(ref u01);
            rw.ReadableWritable<TransactionalNatural>(ref u02);
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
        0x030EA001 => new Chunk030EA001(),
        _ => base.NewChunk(chunkId),
    };
}
