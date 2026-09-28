namespace GBX.NET.Engines.Game;

/// <remarks>ID: 0x03149000</remarks>
[Class(0x03149000)]
public partial class CGamePlayerProfileChunk_ChallengesScores : CGamePlayerProfileChunk, IClass
{
    [Hexadecimal] public static new uint Id => 0x03149000;




    private SContextMapRecordForProfile[]? contextMapRecordsForProfile;
    [AppliedWithChunk<Chunk03149002>]
    public SContextMapRecordForProfile[]? ContextMapRecordsForProfile { get => contextMapRecordsForProfile; set => contextMapRecordsForProfile = value; }

    /// <summary>
    /// Creates a new instance of <see cref="CGamePlayerProfileChunk_ChallengesScores"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public CGamePlayerProfileChunk_ChallengesScores() { }


    /// <summary>
    /// CGamePlayerProfileChunk_ChallengesScores 0x000 skippable chunk
    /// </summary>
    [Chunk(0x03149000)]
    public partial class Chunk03149000 : SkippableChunk<CGamePlayerProfileChunk_ChallengesScores>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x03149000;

        public int Version { get; set; }

        public int U01;

        public override void ReadWrite(CGamePlayerProfileChunk_ChallengesScores n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.Int32(ref U01);
        }
    }

    /// <summary>
    /// CGamePlayerProfileChunk_ChallengesScores 0x002 skippable chunk
    /// </summary>
    [Chunk(0x03149002)]
    public partial class Chunk03149002 : SkippableChunk<CGamePlayerProfileChunk_ChallengesScores>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x03149002;

        public int Version { get; set; }


        public override void ReadWrite(CGamePlayerProfileChunk_ChallengesScores n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.ArrayReadableWritable<SContextMapRecordForProfile>(ref n.contextMapRecordsForProfile!, version: Version);
        }
    }


    public sealed partial class SContextMapRecordForProfile : IReadableWritable
    {

        private string? u01;
        public string? U01 { get => u01; set => u01 = value; }

        private string? u02;
        public string? U02 { get => u02; set => u02 = value; }

        private string? u03;
        public string? U03 { get => u03; set => u03 = value; }

        private int u04;
        public int U04 { get => u04; set => u04 = value; }

        private int u05;
        public int U05 { get => u05; set => u05 = value; }

        private int u06;
        public int U06 { get => u06; set => u06 = value; }

        private int u07;
        public int U07 { get => u07; set => u07 = value; }

        private int u08;
        public int U08 { get => u08; set => u08 = value; }

        private int u09;
        public int U09 { get => u09; set => u09 = value; }

        private int u10;
        public int U10 { get => u10; set => u10 = value; }

        private int u11;
        public int U11 { get => u11; set => u11 = value; }

        private int u12;
        public int U12 { get => u12; set => u12 = value; }

        private int u13;
        public int U13 { get => u13; set => u13 = value; }

        private int u14;
        public int U14 { get => u14; set => u14 = value; }

        private int u15;
        public int U15 { get => u15; set => u15 = value; }

        private int u16;
        public int U16 { get => u16; set => u16 = value; }

        private bool u17;
        public bool U17 { get => u17; set => u17 = value; }

        public void ReadWrite(GbxReaderWriter rw, int v = 0)
        {
            if (v >= 2)
            {
                rw.String(ref u01);
            }
            if (v <= 1)
            {
                rw.Id(ref u02);
            }
            rw.Id(ref u03);
            rw.Int32(ref u04);
            rw.Int32(ref u05);
            rw.Int32(ref u06);
            rw.Int32(ref u07);
            rw.Int32(ref u08);
            rw.Int32(ref u09);
            rw.Int32(ref u10);
            rw.Int32(ref u11);
            rw.Int32(ref u12);
            rw.Int32(ref u13);
            rw.Int32(ref u14);
            rw.Int32(ref u15);
            rw.Int32(ref u16);
            rw.Boolean(ref u17);
        }
    }



    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x03149000 => new Chunk03149000(),
        0x03149002 => new Chunk03149002(),
        _ => base.NewChunk(chunkId),
    };
}
