namespace GBX.NET.Engines.Plug;

/// <remarks>ID: 0x090C6000</remarks>
[Class(0x090C6000)]
public partial class CPlugParticleGpuModel : CMwNod, IClass
{
    [Hexadecimal] public static new uint Id => 0x090C6000;




    private CPlugBitmap? bitmap;
    [AppliedWithChunk<Chunk090C6002>]
    [AppliedWithChunk<Chunk090C6002>]
    public CPlugBitmap? Bitmap { get => bitmapFile?.GetNode(ref bitmap) ?? bitmap; set => bitmap = value; }
    private Components.GbxRefTableFile? bitmapFile;
    public Components.GbxRefTableFile? BitmapFile { get => bitmapFile; set => bitmapFile = value; }
    public CPlugBitmap? GetBitmap(GbxReadSettings settings = default, bool exceptions = false) => bitmapFile?.GetNode(ref bitmap, settings, exceptions) ?? bitmap;

    /// <summary>
    /// Creates a new instance of <see cref="CPlugParticleGpuModel"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public CPlugParticleGpuModel() { }


    /// <summary>
    /// CPlugParticleGpuModel 0x000 chunk
    /// </summary>
    [Chunk(0x090C6000)]
    public partial class Chunk090C6000 : Chunk<CPlugParticleGpuModel>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x090C6000;

        public int Version { get; set; }

        public float U01;
        public float U02;
        public float U03;
        public float U04;
        public float U05;
        public float U06;
        public float U07;
        public bool U08;
        public bool U09;

        public override void ReadWrite(CPlugParticleGpuModel n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.Single(ref U01);
            rw.Single(ref U02);
            rw.Single(ref U03);
            rw.Single(ref U04);
            if (Version >= 2)
            {
                rw.Single(ref U05);
                rw.Single(ref U06);
                if (Version >= 3)
                {
                    rw.Single(ref U07);
                    if (Version >= 5)
                    {
                        rw.Boolean(ref U08);
                        if (Version >= 6)
                        {
                            rw.Boolean(ref U09);
                        }
                    }
                }
            }
        }
    }

    /// <summary>
    /// CPlugParticleGpuModel 0x001 chunk
    /// </summary>
    [Chunk(0x090C6001)]
    public partial class Chunk090C6001 : Chunk<CPlugParticleGpuModel>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x090C6001;

        public int Version { get; set; }

        public float U01;
        public float U02;
        public bool U03;
        public bool U04;
        public bool U05;

        public override void ReadWrite(CPlugParticleGpuModel n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.Single(ref U01);
            rw.Single(ref U02);
            if (Version >= 1)
            {
                rw.Boolean(ref U03);
                rw.Boolean(ref U04);
                if (Version >= 2)
                {
                    rw.Boolean(ref U05);
                }
            }
        }
    }

    /// <summary>
    /// CPlugParticleGpuModel 0x002 chunk
    /// </summary>
    [Chunk(0x090C6002)]
    public partial class Chunk090C6002 : Chunk<CPlugParticleGpuModel>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x090C6002;

        public int Version { get; set; }

        public float U01;
        public bool U02;
        public string? U03;
        public bool U04;
        public float U05;
        public float U06;
        public float U07;
        public int U08;
        public int U09;
        public float U10;
        public float U11;
        public float U12;
        public float U13;
        public float U14;
        public float U15;
        public bool U16;
        public bool U17;
        public bool U18;
        public bool U19;
        public float U20;
        public CPlugBitmap? U21;
        public bool U22;
        public bool U23;
        public bool U24;
        public bool U25;
        public float U26;
        public float U27;
        public bool U28;
        public float U29;
        public bool U30;

        public override void ReadWrite(CPlugParticleGpuModel n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.Single(ref U01);
            if (Version <= 6)
            {
                rw.NodeRef<CPlugBitmap>(ref n.bitmap, ref n.bitmapFile);
            }
            if (Version >= 7)
            {
                rw.Boolean(ref U02);
                if (!U02)
                {
                    rw.NodeRef<CPlugBitmap>(ref n.bitmap, ref n.bitmapFile);
                }
                if (U02)
                {
                    rw.String(ref U03);
                }
                if (Version >= 8)
                {
                    rw.Boolean(ref U04);
                    rw.Single(ref U05);
                }
            }
            rw.Single(ref U06);
            rw.Single(ref U07);
            rw.Int32(ref U08);
            rw.Int32(ref U09);
            if (Version <= 2)
            {
                rw.Single(ref U10);
            }
            if (Version >= 3)
            {
                rw.Single(ref U11);
                rw.Single(ref U12);
                if (Version >= 4)
                {
                    rw.Single(ref U13);
                }
            }
            rw.Single(ref U14);
            rw.Single(ref U15);
            rw.Boolean(ref U16);
            if (Version >= 2)
            {
                rw.Boolean(ref U17);
                if (Version >= 5)
                {
                    rw.Boolean(ref U18);
                    if (Version >= 6)
                    {
                        rw.Boolean(ref U19);
                        rw.Single(ref U20);
                        if (Version >= 9)
                        {
                            rw.NodeRef<CPlugBitmap>(ref U21);
                            rw.Boolean(ref U22);
                            rw.Boolean(ref U23);
                            rw.Boolean(ref U24);
                            rw.Boolean(ref U25);
                            rw.Single(ref U26);
                            if (Version >= 10)
                            {
                                rw.Single(ref U27);
                                rw.Boolean(ref U28);
                                if (Version >= 11)
                                {
                                    rw.Single(ref U29);
                                    rw.Boolean(ref U30);
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    /// <summary>
    /// CPlugParticleGpuModel 0x003 chunk
    /// </summary>
    [Chunk(0x090C6003)]
    public partial class Chunk090C6003 : Chunk<CPlugParticleGpuModel>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x090C6003;

        public int Version { get; set; }

        public float U01;
        public float U02;

        public override void ReadWrite(CPlugParticleGpuModel n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.Single(ref U01);
            rw.Single(ref U02);
        }
    }




    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x090C6000 => new Chunk090C6000(),
        0x090C6001 => new Chunk090C6001(),
        0x090C6002 => new Chunk090C6002(),
        0x090C6003 => new Chunk090C6003(),
        _ => base.NewChunk(chunkId),
    };
}
