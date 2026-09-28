namespace GBX.NET.Engines.Plug;

/// <remarks>ID: 0x0903A000</remarks>
[Class(0x0903A000)]
public partial class CPlugMaterialCustom : CPlug, IClass
{
    [Hexadecimal] public static new uint Id => 0x0903A000;




    private Bitmap[]? textures;
    [AppliedWithChunk<Chunk0903A006>]
    [AppliedWithChunk<Chunk0903A013>]
    public Bitmap[]? Textures { get => textures; set => textures = value; }

    private GpuFx[]? gpuFxs1;
    [AppliedWithChunk<Chunk0903A00A>]
    public GpuFx[]? GpuFxs1 { get => gpuFxs1; set => gpuFxs1 = value; }

    private GpuFx[]? gpuFxs2;
    [AppliedWithChunk<Chunk0903A00A>]
    public GpuFx[]? GpuFxs2 { get => gpuFxs2; set => gpuFxs2 = value; }

    private BitmapSkip[]? skipSamplers;
    [AppliedWithChunk<Chunk0903A00C>]
    public BitmapSkip[]? SkipSamplers { get => skipSamplers; set => skipSamplers = value; }


    /// <summary>
    /// CPlugMaterialCustom 0x004 chunk
    /// </summary>
    [Chunk(0x0903A004)]
    public partial class Chunk0903A004 : Chunk<CPlugMaterialCustom>
    {
        /// <inheritdoc />
        public override uint Id => 0x0903A004;

        public int[]? U01;

        public override void ReadWrite(CPlugMaterialCustom n, GbxReaderWriter rw)
        {
            rw.Array<int>(ref U01!);
        }
    }

    /// <summary>
    /// CPlugMaterialCustom 0x006 chunk
    /// </summary>
    [Chunk(0x0903A006)]
    public partial class Chunk0903A006 : Chunk<CPlugMaterialCustom>
    {
        /// <inheritdoc />
        public override uint Id => 0x0903A006;


        public override void ReadWrite(CPlugMaterialCustom n, GbxReaderWriter rw)
        {
            rw.ArrayReadableWritable<Bitmap>(ref n.textures!);
        }
    }

    /// <summary>
    /// CPlugMaterialCustom 0x00A chunk
    /// </summary>
    [Chunk(0x0903A00A)]
    public partial class Chunk0903A00A : Chunk<CPlugMaterialCustom>
    {
        /// <inheritdoc />
        public override uint Id => 0x0903A00A;


        public override void Read(CPlugMaterialCustom n, GbxReader r)
        {
            n.GpuFxs1 = r.ReadArrayReadable<GpuFx>();
            n.GpuFxs2 = r.ReadArrayReadable<GpuFx>();
        }

        public override void Write(CPlugMaterialCustom n, GbxWriter w)
        {
            w.WriteArrayWritable<GpuFx>(n.GpuFxs1);
            w.WriteArrayWritable<GpuFx>(n.GpuFxs2);
        }
    }

    /// <summary>
    /// CPlugMaterialCustom 0x00B chunk
    /// </summary>
    [Chunk(0x0903A00B)]
    public partial class Chunk0903A00B : Chunk<CPlugMaterialCustom>
    {
        /// <inheritdoc />
        public override uint Id => 0x0903A00B;

        public uint U01;
        public ulong U02;
        public short U03;
        public short U04;

        public override void ReadWrite(CPlugMaterialCustom n, GbxReaderWriter rw)
        {
            rw.UInt32(ref U01);
            rw.UInt64(ref U02);
            if ((U01&1)!=0)
            {
                rw.Int16(ref U03);
                rw.Int16(ref U04);
            }
        }
    }

    /// <summary>
    /// CPlugMaterialCustom 0x00C chunk
    /// </summary>
    [Chunk(0x0903A00C)]
    public partial class Chunk0903A00C : Chunk<CPlugMaterialCustom>
    {
        /// <inheritdoc />
        public override uint Id => 0x0903A00C;


        public override void ReadWrite(CPlugMaterialCustom n, GbxReaderWriter rw)
        {
            rw.ArrayReadableWritable<BitmapSkip>(ref n.skipSamplers!);
        }
    }

    /// <summary>
    /// CPlugMaterialCustom 0x00D chunk
    /// </summary>
    [Chunk(0x0903A00D)]
    public partial class Chunk0903A00D : Chunk<CPlugMaterialCustom>
    {
        /// <inheritdoc />
        public override uint Id => 0x0903A00D;

        public ulong U01;
        public ulong U02;
        public short U03;
        public short U04;

        public override void ReadWrite(CPlugMaterialCustom n, GbxReaderWriter rw)
        {
            rw.UInt64(ref U01);
            rw.UInt64(ref U02);
            if ((U01&1)!=0)
            {
                rw.Int16(ref U03);
                rw.Int16(ref U04);
            }
        }
    }

    /// <summary>
    /// CPlugMaterialCustom 0x00F skippable chunk
    /// </summary>
    [Chunk(0x0903A00F)]
    public partial class Chunk0903A00F : SkippableChunk<CPlugMaterialCustom>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x0903A00F;

        public int Version { get; set; }

        public float U01;
        public float U02;
        public float U03;
        public DefineNat[]? U04;

        public override void ReadWrite(CPlugMaterialCustom n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.Single(ref U01);
            rw.Single(ref U02);
            if (Version >= 1)
            {
                rw.Single(ref U03);
                if (Version >= 2)
                {
                    rw.ArrayReadableWritable<DefineNat>(ref U04!);
                }
            }
        }
    }

    /// <summary>
    /// CPlugMaterialCustom 0x010 chunk
    /// </summary>
    [Chunk(0x0903A010)]
    public partial class Chunk0903A010 : Chunk<CPlugMaterialCustom>
    {
        /// <inheritdoc />
        public override uint Id => 0x0903A010;

        public CPlugBitmap? U01;
        public Components.GbxRefTableFile? U01File;

        public override void ReadWrite(CPlugMaterialCustom n, GbxReaderWriter rw)
        {
            rw.NodeRef<CPlugBitmap>(ref U01, ref U01File);
        }
    }

    /// <summary>
    /// CPlugMaterialCustom 0x011 skippable chunk
    /// </summary>
    [Chunk(0x0903A011)]
    public partial class Chunk0903A011 : SkippableChunk<CPlugMaterialCustom>
    {
        /// <inheritdoc />
        public override uint Id => 0x0903A011;

        public int U01;

        public override void ReadWrite(CPlugMaterialCustom n, GbxReaderWriter rw)
        {
            rw.Int32(ref U01);
        }
    }

    /// <summary>
    /// CPlugMaterialCustom 0x012 chunk
    /// </summary>
    [Chunk(0x0903A012)]
    public partial class Chunk0903A012 : Chunk<CPlugMaterialCustom>
    {
        /// <inheritdoc />
        public override uint Id => 0x0903A012;

        public CMwNod? U01;

        public override void ReadWrite(CPlugMaterialCustom n, GbxReaderWriter rw)
        {
            rw.NodeRef<CMwNod>(ref U01);
        }
    }

    /// <summary>
    /// CPlugMaterialCustom 0x013 chunk
    /// </summary>
    [Chunk(0x0903A013)]
    public partial class Chunk0903A013 : Chunk<CPlugMaterialCustom>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x0903A013;

        public int Version { get; set; }


        public override void ReadWrite(CPlugMaterialCustom n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.ArrayReadableWritable<Bitmap>(ref n.textures!, version: 1);
        }
    }

    /// <summary>
    /// CPlugMaterialCustom 0x014 chunk
    /// </summary>
    [Chunk(0x0903A014)]
    public partial class Chunk0903A014 : Chunk<CPlugMaterialCustom>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x0903A014;

        public int Version { get; set; }

        public CBuffer[]? U01;

        public override void ReadWrite(CPlugMaterialCustom n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.ArrayReadableWritable<CBuffer>(ref U01!);
        }
    }

    /// <summary>
    /// CPlugMaterialCustom 0x015 chunk
    /// </summary>
    [Chunk(0x0903A015)]
    public partial class Chunk0903A015 : Chunk<CPlugMaterialCustom>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x0903A015;

        public int Version { get; set; }

        public int U01;
        public string? U02;
        public string? U03;
        public string? U04;
        public string? U05;

        public override void ReadWrite(CPlugMaterialCustom n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            if (Version >= 1)
            {
                rw.Int32(ref U01);
            }
            if (U01==0)
            {
                rw.String(ref U02);
                rw.String(ref U03);
                if (Version >= 2)
                {
                    rw.String(ref U04);
                    rw.String(ref U05);
                }
            }
        }
    }

    /// <summary>
    /// CPlugMaterialCustom 0x016 chunk
    /// </summary>
    [Chunk(0x0903A016)]
    public partial class Chunk0903A016 : Chunk<CPlugMaterialCustom>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x0903A016;

        public int Version { get; set; }

        public ulong U01;
        public ulong U02;
        public int U03;
        public short U04;
        public short U05;

        public override void ReadWrite(CPlugMaterialCustom n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.UInt64(ref U01);
            rw.UInt64(ref U02);
            if (Version >= 1)
            {
                rw.Int32(ref U03);
            }
            if ((U01&1)!=0)
            {
                rw.Int16(ref U04);
                rw.Int16(ref U05);
            }
        }
    }


    public sealed partial class CBuffer : IReadableWritable
    {

        private int u01;
        public int U01 { get => u01; set => u01 = value; }

        private byte[]? u02;
        public byte[]? U02 { get => u02; set => u02 = value; }

        public void ReadWrite(GbxReaderWriter rw, int v = 0)
        {
            rw.Int32(ref u01);
            rw.Data(ref u02);
        }
    }

    public sealed partial class GpuFx : IReadable, IWritable
    {
    }

    public sealed partial class BitmapSkip : IReadableWritable
    {

        private string? name;
        public string? Name { get => name; set => name = value; }

        private bool u01;
        public bool U01 { get => u01; set => u01 = value; }

        public void ReadWrite(GbxReaderWriter rw, int v = 0)
        {
            rw.Id(ref name);
            rw.Boolean(ref u01);
        }
    }

    public sealed partial class Bitmap : IReadableWritable
    {

        private string? name;
        public string? Name { get => name; set => name = value; }

        private int u01;
        public int U01 { get => u01; set => u01 = value; }

        private CMwNod? texture;
        public CMwNod? Texture { get => textureFile?.GetNode(ref texture) ?? texture; set => texture = value; }
        private Components.GbxRefTableFile? textureFile;
        public Components.GbxRefTableFile? TextureFile { get => textureFile; set => textureFile = value; }
        public CMwNod? GetTexture(GbxReadSettings settings = default, bool exceptions = false) => textureFile?.GetNode(ref texture, settings, exceptions) ?? texture;

        private int u02;
        public int U02 { get => u02; set => u02 = value; }

        private int u03;
        public int U03 { get => u03; set => u03 = value; }

        public void ReadWrite(GbxReaderWriter rw, int v = 0)
        {
            rw.Id(ref name);
            rw.Int32(ref u01);
            rw.NodeRef<CMwNod>(ref texture, ref textureFile);
            if (v >= 1)
            {
                rw.Int32(ref u02);
                rw.Int32(ref u03);
            }
        }
    }

    public sealed partial class DefineNat : IReadableWritable
    {

        private string? u01;
        public string? U01 { get => u01; set => u01 = value; }

        private int u02;
        public int U02 { get => u02; set => u02 = value; }

        public void ReadWrite(GbxReaderWriter rw, int v = 0)
        {
            rw.Id(ref u01);
            rw.Int32(ref u02);
        }
    }



    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x0903A004 => new Chunk0903A004(),
        0x0903A006 => new Chunk0903A006(),
        0x0903A00A => new Chunk0903A00A(),
        0x0903A00B => new Chunk0903A00B(),
        0x0903A00C => new Chunk0903A00C(),
        0x0903A00D => new Chunk0903A00D(),
        0x0903A00F => new Chunk0903A00F(),
        0x0903A010 => new Chunk0903A010(),
        0x0903A011 => new Chunk0903A011(),
        0x0903A012 => new Chunk0903A012(),
        0x0903A013 => new Chunk0903A013(),
        0x0903A014 => new Chunk0903A014(),
        0x0903A015 => new Chunk0903A015(),
        0x0903A016 => new Chunk0903A016(),
        _ => base.NewChunk(chunkId),
    };
}
