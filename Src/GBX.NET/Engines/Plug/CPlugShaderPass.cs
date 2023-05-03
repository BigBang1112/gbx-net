namespace GBX.NET.Engines.Plug;

[Node(0x09067000)]
public class CPlugShaderPass : CPlug
{
    private CPlugBitmapSampler?[]? vertexTextures;
    private PipelineGpu? vhlsl;
    private PipelineGpu? phlsl;

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk<Chunk09067006>]
    public CPlugBitmapSampler?[]? VertexTextures { get => vertexTextures; set => vertexTextures = value; }

    [NodeMember]
    [AppliedWithChunk<Chunk0906700A>]
    public PipelineGpu? VHlsl { get => vhlsl; set => vhlsl = value; }

    [NodeMember]
    [AppliedWithChunk<Chunk0906700A>]
    public PipelineGpu? PHlsl { get => phlsl; set => phlsl = value; }

    internal CPlugShaderPass()
	{

	}

    #region 0x006 chunk

    /// <summary>
    /// CPlugShaderPass 0x006 chunk
    /// </summary>
    [Chunk(0x09067006)]
    public class Chunk09067006 : Chunk<CPlugShaderPass>
    {
        public override void ReadWrite(CPlugShaderPass n, GameBoxReaderWriter rw)
        {
            rw.ArrayNode<CPlugBitmapSampler>(ref n.vertexTextures);
        }
    }

    #endregion

    #region 0x007 chunk

    /// <summary>
    /// CPlugShaderPass 0x007 chunk
    /// </summary>
    [Chunk(0x09067007)]
    public class Chunk09067007 : Chunk<CPlugShaderPass>
    {
        public uint U01;

        public override void ReadWrite(CPlugShaderPass n, GameBoxReaderWriter rw)
        {
            rw.UInt32(ref U01); // DoData
        }
    }

    #endregion

    #region 0x008 chunk

    /// <summary>
    /// CPlugShaderPass 0x008 chunk
    /// </summary>
    [Chunk(0x09067008)]
    public class Chunk09067008 : Chunk<CPlugShaderPass>
    {
        public Node? U01;
        public GameBoxRefTable.File? U01file;
        public GpuLoadFx[]? GpuLoadFxs1;
        public Vec4[]? U02;

        public Node? U03;
        public GameBoxRefTable.File? U03file;
        public GpuLoadFx[]? GpuLoadFxs2;
        public Vec4[]? U04;

        public override void ReadWrite(CPlugShaderPass n, GameBoxReaderWriter rw)
        {
            rw.NodeRef(ref U01, ref U01file);

            if (U01 is not null || U01file is not null)
            {
                rw.ArrayArchive<GpuLoadFx>(ref GpuLoadFxs1);
                rw.Array<Vec4>(ref U02);
            }

            rw.NodeRef(ref U03, ref U03file);

            if (U03 is not null || U03file is not null)
            {
                rw.ArrayArchive<GpuLoadFx>(ref GpuLoadFxs2);
                rw.Array<Vec4>(ref U04);
            }
        }
    }

    #endregion

    #region 0x00A chunk

    /// <summary>
    /// CPlugShaderPass 0x00A chunk
    /// </summary>
    [Chunk(0x0906700A)]
    public class Chunk0906700A : Chunk<CPlugShaderPass>
    {
        public string[]? U01;

        public override void ReadWrite(CPlugShaderPass n, GameBoxReaderWriter rw)
        {
            rw.ArrayId(ref U01);
            rw.Archive<PipelineGpu>(ref n.vhlsl);
            rw.Archive<PipelineGpu>(ref n.phlsl);          
        }
    }

    #endregion

    public class GpuLoadFx : IReadableWritable
    {
        private string u01 = "";
        private int u02;
        private int u03;
        private int u04;
        private int u05;

        public string U01 { get => u01; set => u01 = value; }
        public int U02 { get => u02; set => u02 = value; }
        public int U03 { get => u03; set => u03 = value; }
        public int U04 { get => u04; set => u04 = value; }
        public int U05 { get => u05; set => u05 = value; }

        public void ReadWrite(GameBoxReaderWriter rw, int version = 0)
        {
            rw.Id(ref u01!);
            rw.Int32(ref u02);
            rw.Int32(ref u03);
            rw.Int32(ref u04);
            rw.Int32(ref u05);
        }
    }

    public class PipelineGpu : IReadableWritable
    {
        private bool u01;
        private Node? script;
        private GameBoxRefTable.File? scriptFile;
        private GpuLoadFx[] gpuLoadFxs = Array.Empty<GpuLoadFx>();
        private Vec4[] u02 = Array.Empty<Vec4>();

        public bool U01 { get => u01; set => u01 = value; }
        public GameBoxRefTable.File? ScriptFile { get => scriptFile; set => scriptFile = value; }
        public GpuLoadFx[] GpuLoadFxs { get => gpuLoadFxs; set => gpuLoadFxs = value; }
        public Vec4[] U02 { get => u02; set => u02 = value; }

        public void ReadWrite(GameBoxReaderWriter rw, int version = 0)
        {
            rw.Boolean(ref u01);
            rw.NodeRef(ref script, ref scriptFile); // VHlsl txt?

            if (!u01)
            {
                return;
            }

            rw.ArrayArchive<GpuLoadFx>(ref gpuLoadFxs!);
            rw.Array<Vec4>(ref u02!);
        }
    }
}
