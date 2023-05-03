namespace GBX.NET.Engines.Plug;

/// <summary>
/// Custom material.
/// </summary>
/// <remarks>ID: 0x0903A000</remarks>
[Node(0x0903A000)]
public class CPlugMaterialCustom : CPlug
{
    private Bitmap[]? textures;
    private GpuFx[]? gpuFxs;
    private BitmapSkip[]? skipSamplers;
    private CBuffer[]? cBuffers;

    [NodeMember]
    [AppliedWithChunk<Chunk0903A006>]
    public Bitmap[]? Textures { get => textures; set => textures = value; }

    [NodeMember]
    [AppliedWithChunk<Chunk0903A006>]
    public GpuFx[]? GpuFxs { get => gpuFxs; set => gpuFxs = value; }

    [NodeMember]
    [AppliedWithChunk<Chunk0903A00C>]
    public BitmapSkip[]? SkipSamplers { get => skipSamplers; set => skipSamplers = value; }
    
    [NodeMember]
    [AppliedWithChunk<Chunk0903A00C>]
    public CBuffer[]? CBuffers { get => cBuffers; set => cBuffers = value; }

    internal CPlugMaterialCustom()
    {

    }

    /// <summary>
    /// CPlugMaterialCustom 0x004 chunk
    /// </summary>
    [Chunk(0x0903A004)]
    public class Chunk0903A004 : Chunk<CPlugMaterialCustom>
    {
        public int[]? U01;

        public override void ReadWrite(CPlugMaterialCustom n, GameBoxReaderWriter rw)
        {
            rw.Array<int>(ref U01);
        }
    }

    /// <summary>
    /// CPlugMaterialCustom 0x006 chunk
    /// </summary>
    [Chunk(0x0903A006)]
    public class Chunk0903A006 : Chunk<CPlugMaterialCustom>
    {
        public override void ReadWrite(CPlugMaterialCustom n, GameBoxReaderWriter rw)
        {
            rw.ArrayArchiveWithGbx<Bitmap>(ref n.textures);
        }
    }

    /// <summary>
    /// CPlugMaterialCustom 0x00A chunk
    /// </summary>
    [Chunk(0x0903A00A)]
    public class Chunk0903A00A : Chunk<CPlugMaterialCustom>
    {
        public override void ReadWrite(CPlugMaterialCustom n, GameBoxReaderWriter rw)
        {
            for (var i = 0; i < 2; i++)
            {
                rw.ArrayArchive<GpuFx>(ref n.gpuFxs);
            }
        }
    }

    /// <summary>
    /// CPlugMaterialCustom 0x00B chunk
    /// </summary>
    [Chunk(0x0903A00B)]
    public class Chunk0903A00B : Chunk<CPlugMaterialCustom>
    {
        public uint Flags;
        public ulong U01;
        public short? U02;
        public short? U03;

        public override void ReadWrite(CPlugMaterialCustom n, GameBoxReaderWriter rw)
        {
            rw.UInt32(ref Flags);
            rw.UInt64(ref U01);

            if ((Flags & 1) != 0) // SPlugVisibleFilter
            {
                rw.Int16(ref U02);
                rw.Int16(ref U03);
            }
        }
    }

    /// <summary>
    /// CPlugMaterialCustom 0x00C chunk
    /// </summary>
    [Chunk(0x0903A00C)]
    public class Chunk0903A00C : Chunk<CPlugMaterialCustom>
    {
        public override void ReadWrite(CPlugMaterialCustom n, GameBoxReaderWriter rw)
        {
            // array of SPlugGpuParamSkipSampler/SBitmapSkip
            rw.ArrayArchive(ref n.skipSamplers);
        }
    }

    /// <summary>
    /// CPlugMaterialCustom 0x00D chunk
    /// </summary>
    [Chunk(0x0903A00D)]
    public class Chunk0903A00D : Chunk<CPlugMaterialCustom>
    {
        public ulong U01;
        public ulong U02;
        public short? U03;
        public short? U04;

        public override void ReadWrite(CPlugMaterialCustom n, GameBoxReaderWriter rw)
        {
            rw.UInt64(ref U01);
            rw.UInt64(ref U02);

            if ((U01 & 1) != 0)
            {
                // SPlugVisibleFilter
                rw.Int16(ref U03);
                rw.Int16(ref U04);
                //
            }
        }
    }

    #region 0x00F skippable chunk

    /// <summary>
    /// CPlugMaterialCustom 0x00F skippable chunk
    /// </summary>
    [Chunk(0x0903A00F), IgnoreChunk]
    public class Chunk0903A00F : SkippableChunk<CPlugMaterialCustom>
    {

    }

    #endregion

    #region 0x010 chunk

    /// <summary>
    /// CPlugMaterialCustom 0x010 chunk
    /// </summary>
    [Chunk(0x0903A010)]
    public class Chunk0903A010 : Chunk<CPlugMaterialCustom>
    {
        public CPlugBitmap? U01;
        public GameBoxRefTable.File? U01File;

        public override void ReadWrite(CPlugMaterialCustom n, GameBoxReaderWriter rw)
        {
            rw.NodeRef<CPlugBitmap>(ref U01, ref U01File);
        }
    }

    #endregion

    #region 0x011 skippable chunk

    /// <summary>
    /// CPlugMaterialCustom 0x011 skippable chunk
    /// </summary>
    [Chunk(0x0903A011), IgnoreChunk]
    public class Chunk0903A011 : SkippableChunk<CPlugMaterialCustom>
    {
        
    }

    #endregion

    #region 0x012 chunk

    /// <summary>
    /// CPlugMaterialCustom 0x012 chunk
    /// </summary>
    [Chunk(0x0903A012)]
    public class Chunk0903A012 : Chunk<CPlugMaterialCustom>
    {
        public Node? U01;
        public GameBoxRefTable.File? U01File;

        public override void ReadWrite(CPlugMaterialCustom n, GameBoxReaderWriter rw)
        {
            rw.NodeRef(ref U01, ref U01File);
        }
    }

    #endregion

    #region 0x013 chunk

    /// <summary>
    /// CPlugMaterialCustom 0x013 chunk
    /// </summary>
    [Chunk(0x0903A013)]
    public class Chunk0903A013 : Chunk<CPlugMaterialCustom>, IVersionable
    {
        public Node? U01;
        public GameBoxRefTable.File? U01File;

        public int Version { get; set; }

        public override void ReadWrite(CPlugMaterialCustom n, GameBoxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.ArrayArchiveWithGbx<Bitmap>(ref n.textures, Version + 1);
        }
    }

    #endregion

    #region 0x014 chunk

    /// <summary>
    /// CPlugMaterialCustom 0x014 chunk
    /// </summary>
    [Chunk(0x0903A014)]
    public class Chunk0903A014 : Chunk<CPlugMaterialCustom>, IVersionable
    {
        public int Version { get; set; }

        public override void ReadWrite(CPlugMaterialCustom n, GameBoxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.ArrayArchive(ref n.cBuffers);
        }
    }

    #endregion

    #region 0x015 chunk

    /// <summary>
    /// CPlugMaterialCustom 0x015 chunk
    /// </summary>
    [Chunk(0x0903A015)]
    public class Chunk0903A015 : Chunk<CPlugMaterialCustom>, IVersionable
    {
        public int U01;
        public string? U02;
        public string? U03;

        public int Version { get; set; } = 1;

        public override void ReadWrite(CPlugMaterialCustom n, GameBoxReaderWriter rw)
        {
            rw.VersionInt32(this);
            
            if (Version >= 1)
            {
                rw.Int32(ref U01);
            }

            if (U01 == 0) // subnames?
            {
                rw.String(ref U02);
                rw.String(ref U03);
            }
        }
    }

    #endregion

    public class Bitmap : IReadableWritableWithGbx
    {
        private Node? node;

        private string name = "";
        private int u01;
        private CPlugBitmap? texture;
        private GameBoxRefTable.File? textureFile;
        private int? u02;
        private int? u03;

        public string Name { get => name; set => name = value; }
        public int U01 { get => u01; set => u01 = value; }
        public int? U02 { get => u02; set => u02 = value; }
        public int? U03 { get => u03; set => u03 = value; }

        public CPlugBitmap? Texture
        {
            get => texture = node?.GetNodeFromRefTable(texture, textureFile) as CPlugBitmap;
            set => texture = value;
        }

        public void ReadWrite(GameBoxReaderWriter rw, GameBox? gbx, int version = 0)
        {
            node = gbx?.Node;
            ReadWrite(rw, version);
        }

        public void ReadWrite(GameBoxReaderWriter rw, int version = 0)
        {
            rw.Id(ref name!);
            rw.Int32(ref u01);
            rw.NodeRef(ref texture, ref textureFile);

            if (version >= 1)
            {
                rw.Int32(ref u02);
                rw.Int32(ref u03);
            }
        }

        public override string ToString()
        {
            return Name;
        }
    }

    public class GpuFx : IReadableWritable
    {
        private string u01 = "";
        private int count1;
        private int count2;
        private bool u02;

        public string U01 { get => u01; set => u01 = value; }
        public int Count1 { get => count1; set => count1 = value; }
        public int Count2 { get => count2; set => count2 = value; }
        public bool U02 { get => u02; set => u02 = value; }
        public float[][] U03 { get; set; } = Array.Empty<float[]>();

        public void ReadWrite(GameBoxReaderWriter rw, int version = 0)
        {
            rw.Id(ref u01!);
            rw.Int32(ref count1);
            rw.Int32(ref count2);
            rw.Boolean(ref u02);

            if (rw.Reader is not null)
            {
                U03 = rw.Reader.ReadArray(count2, r => r.ReadArray<float>(count1));
            }

            if (rw.Writer is not null)
            {
                for (var i = 0; i < count2; i++)
                {
                    rw.Writer.WriteArray_NoPrefix(U03[i]);
                }
            }
        }
    }

    public class BitmapSkip : IReadableWritable
    {
        private string name = "";
        private bool u01;

        public string Name { get => name; set => name = value; }
        public bool U01 { get => u01; set => u01 = value; }

        public void ReadWrite(GameBoxReaderWriter rw, int version = 0)
        {
            rw.Id(ref name!);
            rw.Boolean(ref u01);
        }

        public override string ToString()
        {
            return $"{name}: {u01}";
        }
    }

    public class CBuffer : IReadableWritable
    {
        private int u01;
        private byte[] data = Array.Empty<byte>();

        public int U01 { get => u01; set => u01 = value; }
        public byte[] Data { get => data; set => data = value; }

        public void ReadWrite(GameBoxReaderWriter rw, int version = 0)
        {
            rw.Int32(ref u01);
            rw.Bytes(ref data!);
        }
    }
}
