namespace GBX.NET.Engines.Plug;

/// <remarks>ID: 0x09067000</remarks>
[Class(0x09067000)]
public partial class CPlugShaderPass : CPlug, IClass
{
    [Hexadecimal] public static new uint Id => 0x09067000;




    private CPlugBitmapSampler[]? vertexTextures;
    [AppliedWithChunk<Chunk09067006>]
    public CPlugBitmapSampler[]? VertexTextures { get => vertexTextures; set => vertexTextures = value; }

    private CPlugFileGPU? vHlslGpu;
    [AppliedWithChunk<Chunk09067008>]
    public CPlugFileGPU? VHlslGpu { get => vHlslGpuFile?.GetNode(ref vHlslGpu) ?? vHlslGpu; set => vHlslGpu = value; }
    private Components.GbxRefTableFile? vHlslGpuFile;
    public Components.GbxRefTableFile? VHlslGpuFile { get => vHlslGpuFile; set => vHlslGpuFile = value; }
    public CPlugFileGPU? GetVHlslGpu(GbxReadSettings settings = default, bool exceptions = false) => vHlslGpuFile?.GetNode(ref vHlslGpu, settings, exceptions) ?? vHlslGpu;

    private GpuLoadFx[]? vHlslGpuLoadFxs1;
    [AppliedWithChunk<Chunk09067008>]
    public GpuLoadFx[]? VHlslGpuLoadFxs1 { get => vHlslGpuLoadFxs1; set => vHlslGpuLoadFxs1 = value; }

    private Vec4[]? vHlslU01;
    [AppliedWithChunk<Chunk09067008>]
    public Vec4[]? VHlslU01 { get => vHlslU01; set => vHlslU01 = value; }

    private CPlugFileGPU? pHlslGpu;
    [AppliedWithChunk<Chunk09067008>]
    public CPlugFileGPU? PHlslGpu { get => pHlslGpuFile?.GetNode(ref pHlslGpu) ?? pHlslGpu; set => pHlslGpu = value; }
    private Components.GbxRefTableFile? pHlslGpuFile;
    public Components.GbxRefTableFile? PHlslGpuFile { get => pHlslGpuFile; set => pHlslGpuFile = value; }
    public CPlugFileGPU? GetPHlslGpu(GbxReadSettings settings = default, bool exceptions = false) => pHlslGpuFile?.GetNode(ref pHlslGpu, settings, exceptions) ?? pHlslGpu;

    private GpuLoadFx[]? pHlslGpuLoadFxs2;
    [AppliedWithChunk<Chunk09067008>]
    public GpuLoadFx[]? PHlslGpuLoadFxs2 { get => pHlslGpuLoadFxs2; set => pHlslGpuLoadFxs2 = value; }

    private Vec4[]? pHlslU01;
    [AppliedWithChunk<Chunk09067008>]
    public Vec4[]? PHlslU01 { get => pHlslU01; set => pHlslU01 = value; }

    private PipelineGpu? vHlsl;
    [AppliedWithChunk<Chunk0906700A>]
    public PipelineGpu? VHlsl { get => vHlsl; set => vHlsl = value; }

    private PipelineGpu? pHlsl;
    [AppliedWithChunk<Chunk0906700A>]
    public PipelineGpu? PHlsl { get => pHlsl; set => pHlsl = value; }

    /// <summary>
    /// Creates a new instance of <see cref="CPlugShaderPass"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public CPlugShaderPass() { }


    /// <summary>
    /// CPlugShaderPass 0x006 chunk
    /// </summary>
    [Chunk(0x09067006)]
    public partial class Chunk09067006 : Chunk<CPlugShaderPass>
    {
        /// <inheritdoc />
        public override uint Id => 0x09067006;


        public override void ReadWrite(CPlugShaderPass n, GbxReaderWriter rw)
        {
            rw.ArrayNodeRef<CPlugBitmapSampler>(ref n.vertexTextures!);
        }
    }

    /// <summary>
    /// CPlugShaderPass 0x007 chunk
    /// </summary>
    [Chunk(0x09067007)]
    public partial class Chunk09067007 : Chunk<CPlugShaderPass>
    {
        /// <inheritdoc />
        public override uint Id => 0x09067007;

        public uint U01;

        public override void ReadWrite(CPlugShaderPass n, GbxReaderWriter rw)
        {
            rw.UInt32(ref U01);
        }
    }

    /// <summary>
    /// CPlugShaderPass 0x008 chunk
    /// </summary>
    [Chunk(0x09067008)]
    public partial class Chunk09067008 : Chunk<CPlugShaderPass>
    {
        /// <inheritdoc />
        public override uint Id => 0x09067008;


        public override void ReadWrite(CPlugShaderPass n, GbxReaderWriter rw)
        {
            rw.NodeRef<CPlugFileGPU>(ref n.vHlslGpu, ref n.vHlslGpuFile);
            if (n.VHlslGpu!=null||n.VHlslGpuFile!=null)
            {
                rw.ArrayReadableWritable<GpuLoadFx>(ref n.vHlslGpuLoadFxs1!);
                rw.Array<Vec4>(ref n.vHlslU01!);
            }
            rw.NodeRef<CPlugFileGPU>(ref n.pHlslGpu, ref n.pHlslGpuFile);
            if (n.PHlslGpu!=null||n.PHlslGpuFile!=null)
            {
                rw.ArrayReadableWritable<GpuLoadFx>(ref n.pHlslGpuLoadFxs2!);
                rw.Array<Vec4>(ref n.pHlslU01!);
            }
        }
    }

    /// <summary>
    /// CPlugShaderPass 0x00A chunk
    /// </summary>
    [Chunk(0x0906700A)]
    public partial class Chunk0906700A : Chunk<CPlugShaderPass>
    {
        /// <inheritdoc />
        public override uint Id => 0x0906700A;

        public string[]? U01;

        public override void ReadWrite(CPlugShaderPass n, GbxReaderWriter rw)
        {
            rw.ArrayId(ref U01!);
            rw.ReadableWritable<PipelineGpu>(ref n.vHlsl);
            rw.ReadableWritable<PipelineGpu>(ref n.pHlsl);
        }
    }

    /// <summary>
    /// CPlugShaderPass 0x00B chunk
    /// </summary>
    [Chunk(0x0906700B)]
    public partial class Chunk0906700B : Chunk<CPlugShaderPass>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x0906700B;

        public int Version { get; set; }

        public PipelineGpu? U01;
        public PipelineGpu? U02;
        public PipelineGpu? U03;

        public override void ReadWrite(CPlugShaderPass n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.ReadableWritable<PipelineGpu>(ref U01);
            rw.ReadableWritable<PipelineGpu>(ref U02);
            if (Version >= 1)
            {
                rw.ReadableWritable<PipelineGpu>(ref U03);
            }
        }
    }

    /// <summary>
    /// CPlugShaderPass 0x00C chunk
    /// </summary>
    [Chunk(0x0906700C)]
    public partial class Chunk0906700C : Chunk<CPlugShaderPass>
    {
        /// <inheritdoc />
        public override uint Id => 0x0906700C;

        public uint U01;

        public override void ReadWrite(CPlugShaderPass n, GbxReaderWriter rw)
        {
            rw.UInt32(ref U01);
        }
    }

    /// <summary>
    /// CPlugShaderPass 0x00D chunk
    /// </summary>
    [Chunk(0x0906700D)]
    public partial class Chunk0906700D : Chunk<CPlugShaderPass>
    {
        /// <inheritdoc />
        public override uint Id => 0x0906700D;

        public UnknownStruct[]? U01;
        public UnknownStruct[]? U02;
        public UnknownStruct[]? U03;
        public UnknownStruct[]? U04;
        public UnknownStruct[]? U05;

        public override void ReadWrite(CPlugShaderPass n, GbxReaderWriter rw)
        {
            rw.ArrayReadableWritable<UnknownStruct>(ref U01!);
            rw.ArrayReadableWritable<UnknownStruct>(ref U02!);
            rw.ArrayReadableWritable<UnknownStruct>(ref U03!);
            rw.ArrayReadableWritable<UnknownStruct>(ref U04!);
            rw.ArrayReadableWritable<UnknownStruct>(ref U05!);
        }
    }


    public sealed partial class PipelineGpu : IReadableWritable
    {

        private bool u01;
        public bool U01 { get => u01; set => u01 = value; }

        private CMwNod? script;
        public CMwNod? Script { get => scriptFile?.GetNode(ref script) ?? script; set => script = value; }
        private Components.GbxRefTableFile? scriptFile;
        public Components.GbxRefTableFile? ScriptFile { get => scriptFile; set => scriptFile = value; }
        public CMwNod? GetScript(GbxReadSettings settings = default, bool exceptions = false) => scriptFile?.GetNode(ref script, settings, exceptions) ?? script;

        private GpuLoadFx[]? gpuLoadFxs;
        public GpuLoadFx[]? GpuLoadFxs { get => gpuLoadFxs; set => gpuLoadFxs = value; }

        private Vec4[]? u02;
        public Vec4[]? U02 { get => u02; set => u02 = value; }

        public void ReadWrite(GbxReaderWriter rw, int v = 0)
        {
            rw.Boolean(ref u01);
            rw.NodeRef<CMwNod>(ref script, ref scriptFile);
            if (!U01)
            {
                return;
            }
            rw.ArrayReadableWritable<GpuLoadFx>(ref gpuLoadFxs!);
            rw.Array<Vec4>(ref u02!);
        }
    }

    public sealed partial class UnknownStruct : IReadableWritable
    {

        private CPlugBitmap? u01;
        public CPlugBitmap? U01 { get => u01File?.GetNode(ref u01) ?? u01; set => u01 = value; }
        private Components.GbxRefTableFile? u01File;
        public Components.GbxRefTableFile? U01File { get => u01File; set => u01File = value; }
        public CPlugBitmap? GetU01(GbxReadSettings settings = default, bool exceptions = false) => u01File?.GetNode(ref u01, settings, exceptions) ?? u01;

        private string? u02;
        public string? U02 { get => u02; set => u02 = value; }

        public void ReadWrite(GbxReaderWriter rw, int v = 0)
        {
            rw.NodeRef<CPlugBitmap>(ref u01, ref u01File);
            rw.Id(ref u02);
        }
    }

    public sealed partial class GpuLoadFx : IReadableWritable
    {

        private string? u01;
        public string? U01 { get => u01; set => u01 = value; }

        private int u02;
        public int U02 { get => u02; set => u02 = value; }

        private int u03;
        public int U03 { get => u03; set => u03 = value; }

        private int u04;
        public int U04 { get => u04; set => u04 = value; }

        private int u05;
        public int U05 { get => u05; set => u05 = value; }

        public void ReadWrite(GbxReaderWriter rw, int v = 0)
        {
            rw.Id(ref u01);
            rw.Int32(ref u02);
            rw.Int32(ref u03);
            rw.Int32(ref u04);
            rw.Int32(ref u05);
        }
    }



    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x09067006 => new Chunk09067006(),
        0x09067007 => new Chunk09067007(),
        0x09067008 => new Chunk09067008(),
        0x0906700A => new Chunk0906700A(),
        0x0906700B => new Chunk0906700B(),
        0x0906700C => new Chunk0906700C(),
        0x0906700D => new Chunk0906700D(),
        _ => base.NewChunk(chunkId),
    };
}
