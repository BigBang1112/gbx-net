namespace GBX.NET.Engines.Game;

/// <remarks>ID: 0x03063000</remarks>
[Class(0x03063000)]
public partial class CGameCampaignScores : CMwNod, IClass
{
    [Hexadecimal] public static new uint Id => 0x03063000;




    /// <summary>
    /// Creates a new instance of <see cref="CGameCampaignScores"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public CGameCampaignScores() { }


    /// <summary>
    /// CGameCampaignScores 0x001 chunk
    /// </summary>
    [Chunk(0x03063001)]
    public partial class Chunk03063001 : Chunk<CGameCampaignScores>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x03063001;

        public int Version { get; set; }

        public string? U01;
        public Unknown1[]? U02;
        public Unknown2[]? U03;

        public override void ReadWrite(CGameCampaignScores n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.Id(ref U01);
            rw.ArrayReadableWritable<Unknown1>(ref U02!);
            rw.ArrayReadableWritable<Unknown2>(ref U03!);
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

    public sealed partial class Unknown2 : IReadableWritable
    {

        private int u01;
        public int U01 { get => u01; set => u01 = value; }

        private Unknown3[]? u02;
        public Unknown3[]? U02 { get => u02; set => u02 = value; }

        public void ReadWrite(GbxReaderWriter rw, int v = 0)
        {
            rw.Int32(ref u01);
            rw.ArrayReadableWritable<Unknown3>(ref u02!);
        }
    }

    public sealed partial class Unknown1 : IReadableWritable
    {

        private string? u01;
        public string? U01 { get => u01; set => u01 = value; }

        private byte u02;
        public byte U02 { get => u02; set => u02 = value; }

        private DateTime? u03;
        public DateTime? U03 { get => u03; set => u03 = value; }

        private byte u04;
        public byte U04 { get => u04; set => u04 = value; }

        private DateTime? u05;
        public DateTime? U05 { get => u05; set => u05 = value; }

        public void ReadWrite(GbxReaderWriter rw, int v = 0)
        {
            rw.String(ref u01);
            rw.Byte(ref u02);
            rw.SystemTime(ref u03);
            rw.Byte(ref u04);
            rw.SystemTime(ref u05);
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

    public sealed partial class Unknown3 : IReadableWritable
    {

        private string? u01;
        public string? U01 { get => u01; set => u01 = value; }

        private bool u02;
        public bool U02 { get => u02; set => u02 = value; }

        private RecordsBuffer? u03;
        public RecordsBuffer? U03 { get => u03; set => u03 = value; }

        public void ReadWrite(GbxReaderWriter rw, int v = 0)
        {
            rw.String(ref u01);
            rw.Boolean(ref u02);
            rw.ReadableWritable<RecordsBuffer>(ref u03);
        }
    }



    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x03063001 => new Chunk03063001(),
        _ => base.NewChunk(chunkId),
    };
}
