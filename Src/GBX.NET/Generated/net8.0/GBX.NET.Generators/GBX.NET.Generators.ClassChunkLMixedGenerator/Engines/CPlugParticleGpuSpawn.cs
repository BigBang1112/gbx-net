namespace GBX.NET.Engines.Plug;

/// <remarks>ID: 0x090C5000</remarks>
[Class(0x090C5000)]
public partial class CPlugParticleGpuSpawn : CMwNod, IClass
{
    [Hexadecimal] public static new uint Id => 0x090C5000;




    private ParticleGpuSpawn? spawn;
    [AppliedWithChunk<Chunk090C5000>]
    public ParticleGpuSpawn? Spawn { get => spawn; set => spawn = value; }

    /// <summary>
    /// Creates a new instance of <see cref="CPlugParticleGpuSpawn"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public CPlugParticleGpuSpawn() { }


    /// <summary>
    /// CPlugParticleGpuSpawn 0x000 chunk
    /// </summary>
    [Chunk(0x090C5000)]
    public partial class Chunk090C5000 : Chunk<CPlugParticleGpuSpawn>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x090C5000;

        public int Version { get; set; }

        public int U01;

        public override void ReadWrite(CPlugParticleGpuSpawn n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            if (Version == 0)
            {
                rw.Int32(ref U01);
            }
            if (Version >= 1)
            {
                rw.ReadableWritable<ParticleGpuSpawn>(ref n.spawn);
            }
        }
    }


    public sealed partial class ParticleGpuSpawn : IReadableWritable
    {

        private int version = 4;
        public int Version { get => version; set => version = value; }

        private int u01;
        public int U01 { get => u01; set => u01 = value; }

        private int u02;
        public int U02 { get => u02; set => u02 = value; }

        private int u03;
        public int U03 { get => u03; set => u03 = value; }

        private float u04;
        public float U04 { get => u04; set => u04 = value; }

        private float u05;
        public float U05 { get => u05; set => u05 = value; }

        private bool u06;
        public bool U06 { get => u06; set => u06 = value; }

        private float u07;
        public float U07 { get => u07; set => u07 = value; }

        private float u08;
        public float U08 { get => u08; set => u08 = value; }

        private float u09;
        public float U09 { get => u09; set => u09 = value; }

        private float u10;
        public float U10 { get => u10; set => u10 = value; }

        private float u11;
        public float U11 { get => u11; set => u11 = value; }

        private float u12;
        public float U12 { get => u12; set => u12 = value; }

        private float u13;
        public float U13 { get => u13; set => u13 = value; }

        private float u14;
        public float U14 { get => u14; set => u14 = value; }

        private int u15;
        public int U15 { get => u15; set => u15 = value; }

        private float u16;
        public float U16 { get => u16; set => u16 = value; }

        private float u17;
        public float U17 { get => u17; set => u17 = value; }

        private float u18;
        public float U18 { get => u18; set => u18 = value; }

        private float u19;
        public float U19 { get => u19; set => u19 = value; }

        private float u20;
        public float U20 { get => u20; set => u20 = value; }

        private float u21;
        public float U21 { get => u21; set => u21 = value; }

        private float u22;
        public float U22 { get => u22; set => u22 = value; }

        private float u23;
        public float U23 { get => u23; set => u23 = value; }

        private float u24;
        public float U24 { get => u24; set => u24 = value; }

        private float u25;
        public float U25 { get => u25; set => u25 = value; }

        private float u26;
        public float U26 { get => u26; set => u26 = value; }

        private float u27;
        public float U27 { get => u27; set => u27 = value; }

        private BoxAligned u28;
        public BoxAligned U28 { get => u28; set => u28 = value; }

        private float u29;
        public float U29 { get => u29; set => u29 = value; }

        private float u30;
        public float U30 { get => u30; set => u30 = value; }

        private float u31;
        public float U31 { get => u31; set => u31 = value; }

        private float u32;
        public float U32 { get => u32; set => u32 = value; }

        public void ReadWrite(GbxReaderWriter rw, int v = 0)
        {
            rw.Int32(ref this.version);
            rw.Int32(ref u01);
            if (Version>=2)
            {
                rw.Int32(ref u02);
            }
            rw.Int32(ref u03);
            rw.Single(ref u04);
            if (Version>=3)
            {
                rw.Single(ref u05);
            }
            if (Version>=4)
            {
                rw.Boolean(ref u06);
                rw.Single(ref u07);
                rw.Single(ref u08);
                rw.Single(ref u09);
            }
            rw.Single(ref u10);
            rw.Single(ref u11);
            rw.Single(ref u12);
            rw.Single(ref u13);
            rw.Single(ref u14);
            rw.Int32(ref u15);
            if (U15==1)
            {
                rw.Single(ref u16);
                rw.Single(ref u17);
                rw.Single(ref u18);
                rw.Single(ref u19);
            }
            if (U15==2)
            {
                rw.Single(ref u20);
                rw.Single(ref u21);
                rw.Single(ref u22);
                rw.Single(ref u23);
                if (Version>=1)
                {
                    rw.Single(ref u24);
                    rw.Single(ref u25);
                    rw.Single(ref u26);
                    rw.Single(ref u27);
                }
            }
            if (U15==3)
            {
                rw.BoxAligned(ref u28);
            }
            if (U15==4)
            {
                rw.Single(ref u29);
                rw.Single(ref u30);
                rw.Single(ref u31);
                rw.Single(ref u32);
            }
        }
    }



    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x090C5000 => new Chunk090C5000(),
        _ => base.NewChunk(chunkId),
    };
}
