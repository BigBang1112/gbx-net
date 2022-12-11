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

    [NodeMember]
    [AppliedWithChunk<Chunk0903A006>]
    public Bitmap[]? Textures { get => textures; set => textures = value; }

    [NodeMember]
    [AppliedWithChunk<Chunk0903A006>]
    public GpuFx[]? GpuFxs { get => gpuFxs; set => gpuFxs = value; }

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
        public (string, bool)[]? GpuParamSkipSamplers;

        public override void ReadWrite(CPlugMaterialCustom n, GameBoxReaderWriter rw)
        {
            // array of SPlugGpuParamSkipSampler
            rw.Array(ref GpuParamSkipSamplers,
                r => (r.ReadId(), r.ReadBoolean()),
                (x, w) =>
                {
                    w.WriteId(x.Item1);
                    w.Write(x.Item2);
                });
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

    public class Bitmap : IReadableWritableWithGbx
    {
        private Node? node;

        private string name = "";
        private int u01;
        private CPlugBitmap? texture;
        private GameBoxRefTable.File? textureFile;

        public string Name { get => name; set => name = value; }
        public int U01 { get => u01; set => u01 = value; }

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
}
