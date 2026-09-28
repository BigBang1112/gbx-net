namespace GBX.NET.Engines.Game;

/// <remarks>ID: 0x03148000</remarks>
[Class(0x03148000)]
public partial class CGamePlayerProfileChunk_ChallengesStats : CGamePlayerProfileChunk, IClass
{
    [Hexadecimal] public static new uint Id => 0x03148000;




    private SChallengeStats[]? challengeStats;
    [AppliedWithChunk<Chunk03148001>]
    public SChallengeStats[]? ChallengeStats { get => challengeStats; set => challengeStats = value; }

    /// <summary>
    /// Creates a new instance of <see cref="CGamePlayerProfileChunk_ChallengesStats"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public CGamePlayerProfileChunk_ChallengesStats() { }


    /// <summary>
    /// CGamePlayerProfileChunk_ChallengesStats 0x000 skippable chunk
    /// </summary>
    [Chunk(0x03148000)]
    public partial class Chunk03148000 : SkippableChunk<CGamePlayerProfileChunk_ChallengesStats>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x03148000;

        public int Version { get; set; }

        public int U01;

        public override void ReadWrite(CGamePlayerProfileChunk_ChallengesStats n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.Int32(ref U01);
        }
    }

    /// <summary>
    /// CGamePlayerProfileChunk_ChallengesStats 0x001 skippable chunk
    /// </summary>
    [Chunk(0x03148001)]
    public partial class Chunk03148001 : SkippableChunk<CGamePlayerProfileChunk_ChallengesStats>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x03148001;

        public int Version { get; set; }


        public override void ReadWrite(CGamePlayerProfileChunk_ChallengesStats n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.ArrayReadableWritable<SChallengeStats>(ref n.challengeStats!, version: Version);
        }
    }


    public sealed partial class SChallengeStats : IReadableWritable
    {

        private Ident? u01;
        public Ident? U01 { get => u01; set => u01 = value; }

        private string? u02;
        public string? U02 { get => u02; set => u02 = value; }

        private int u03;
        public int U03 { get => u03; set => u03 = value; }

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

        private bool needsSynchro;
        public bool NeedsSynchro { get => needsSynchro; set => needsSynchro = value; }

        private int u18;
        public int U18 { get => u18; set => u18 = value; }

        private int u19;
        public int U19 { get => u19; set => u19 = value; }

        private int u20;
        public int U20 { get => u20; set => u20 = value; }

        private int u21;
        public int U21 { get => u21; set => u21 = value; }

        private int u22;
        public int U22 { get => u22; set => u22 = value; }

        private int u23;
        public int U23 { get => u23; set => u23 = value; }

        private int u24;
        public int U24 { get => u24; set => u24 = value; }

        private int u25;
        public int U25 { get => u25; set => u25 = value; }

        private int u26;
        public int U26 { get => u26; set => u26 = value; }

        private int u27;
        public int U27 { get => u27; set => u27 = value; }

        private int u28;
        public int U28 { get => u28; set => u28 = value; }

        private int u29;
        public int U29 { get => u29; set => u29 = value; }

        private int u30;
        public int U30 { get => u30; set => u30 = value; }

        public void ReadWrite(GbxReaderWriter rw, int v = 0)
        {
            rw.Ident(ref u01);
            if (v >= 2)
            {
                rw.String(ref u02);
                rw.Int32(ref u03);
            }
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
            rw.Boolean(ref needsSynchro);
            if (NeedsSynchro)
            {
                rw.Int32(ref u18);
                rw.Int32(ref u19);
                rw.Int32(ref u20);
                rw.Int32(ref u21);
                rw.Int32(ref u22);
                rw.Int32(ref u23);
                rw.Int32(ref u24);
                rw.Int32(ref u25);
                rw.Int32(ref u26);
                rw.Int32(ref u27);
                rw.Int32(ref u28);
                rw.Int32(ref u29);
                rw.Int32(ref u30);
            }
        }
    }



    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x03148000 => new Chunk03148000(),
        0x03148001 => new Chunk03148001(),
        _ => base.NewChunk(chunkId),
    };
}
