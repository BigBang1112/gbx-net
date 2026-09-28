namespace GBX.NET.Engines.Plug;

/// <remarks>ID: 0x09047000</remarks>
[Class(0x09047000)]
public partial class CPlugBitmapAddress : CPlugBitmapSampler, IClass
{
    [Hexadecimal] public static new uint Id => 0x09047000;




    private BoxAligned? uVTransfoIso3;
    [AppliedWithChunk<Chunk09047007>]
    public BoxAligned? UVTransfoIso3 { get => uVTransfoIso3; set => uVTransfoIso3 = value; }

    private Mat4? uVTransfoMat4;
    [AppliedWithChunk<Chunk09047007>]
    public Mat4? UVTransfoMat4 { get => uVTransfoMat4; set => uVTransfoMat4 = value; }

    private float bumpEnvScale;
    [AppliedWithChunk<Chunk09047009>]
    public float BumpEnvScale { get => bumpEnvScale; set => bumpEnvScale = value; }

    /// <summary>
    /// Creates a new instance of <see cref="CPlugBitmapAddress"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public CPlugBitmapAddress() { }


    /// <summary>
    /// CPlugBitmapAddress 0x002 chunk
    /// </summary>
    [Chunk(0x09047002)]
    public partial class Chunk09047002 : Chunk<CPlugBitmapAddress>
    {
        /// <inheritdoc />
        public override uint Id => 0x09047002;

        /// <summary>
        /// SBitmapElemToPack array?
        /// </summary>
        public int U01;

        public override void ReadWrite(CPlugBitmapAddress n, GbxReaderWriter rw)
        {
            rw.Int32(ref U01); // SBitmapElemToPack array?
        }
    }

    /// <summary>
    /// CPlugBitmapAddress 0x004 chunk
    /// </summary>
    [Chunk(0x09047004)]
    public partial class Chunk09047004 : Chunk<CPlugBitmapAddress>
    {
        /// <inheritdoc />
        public override uint Id => 0x09047004;

        public int U01;
        public byte U02;
        public int U03;
        public bool U04;
        public bool U05;
        public bool U06;

        public override void ReadWrite(CPlugBitmapAddress n, GbxReaderWriter rw)
        {
            rw.Int32(ref U01);
            rw.Byte(ref U02);
            rw.Int32(ref U03);
            rw.Boolean(ref U04);
            rw.Boolean(ref U05);
            rw.Boolean(ref U06);
        }
    }

    /// <summary>
    /// CPlugBitmapAddress 0x005 chunk
    /// </summary>
    [Chunk(0x09047005)]
    public partial class Chunk09047005 : Chunk09047004
    {
        /// <inheritdoc />
        public override uint Id => 0x09047005;

    }

    /// <summary>
    /// CPlugBitmapAddress 0x007 chunk
    /// </summary>
    [Chunk(0x09047007)]
    public partial class Chunk09047007 : Chunk<CPlugBitmapAddress>
    {
        /// <inheritdoc />
        public override uint Id => 0x09047007;

        public uint U01;
        public int U02;
        public byte U03;

        public override void ReadWrite(CPlugBitmapAddress n, GbxReaderWriter rw)
        {
            rw.UInt32(ref U01);
            rw.Int32(ref U02);
            rw.Byte(ref U03);
            if (U03==1)
            {
                rw.BoxAligned(ref n.uVTransfoIso3);
            }
            if (U03==2)
            {
                rw.Mat4(ref n.uVTransfoMat4);
            }
        }
    }

    /// <summary>
    /// CPlugBitmapAddress 0x008 chunk
    /// </summary>
    [Chunk(0x09047008)]
    public partial class Chunk09047008 : Chunk<CPlugBitmapAddress>
    {
        /// <inheritdoc />
        public override uint Id => 0x09047008;

        public int U01;
        public float U02;

        public override void ReadWrite(CPlugBitmapAddress n, GbxReaderWriter rw)
        {
            rw.Int32(ref U01);
            rw.Single(ref U02);
        }
    }

    /// <summary>
    /// CPlugBitmapAddress 0x009 chunk
    /// </summary>
    [Chunk(0x09047009)]
    public partial class Chunk09047009 : Chunk<CPlugBitmapAddress>
    {
        /// <inheritdoc />
        public override uint Id => 0x09047009;


        public override void ReadWrite(CPlugBitmapAddress n, GbxReaderWriter rw)
        {
            rw.Single(ref n.bumpEnvScale);
        }
    }




    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x09047002 => new Chunk09047002(),
        0x09047004 => new Chunk09047004(),
        0x09047005 => new Chunk09047005(),
        0x09047007 => new Chunk09047007(),
        0x09047008 => new Chunk09047008(),
        0x09047009 => new Chunk09047009(),
        _ => base.NewChunk(chunkId),
    };
}
