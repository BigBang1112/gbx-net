using GBX.NET.Components;

namespace GBX.NET.Engines.Plug;

public partial class CPlugShaderPass
{
    public int BlendSrc
    {
        get => (int)(Flags & 0xF);
        set => Flags = (Flags & 0xFFFFFFF0) | ((uint)value & 0xF);
    }

    public int BlendDst
    {
        get => (int)((Flags >> 4) & 0xF);
        set => Flags = (Flags & 0xFFFFFF0F) | (((uint)value & 0xF) << 4);
    }

    public int CullMode
    {
        get => (int)((Flags >> 8) & 3);
        set => Flags = (Flags & 0xFFFFFCFF) | (((uint)value & 3) << 8);
    }

    [Obsolete("Use VertexShader instead.")]
    public CPlugFileGPU? VHlslGpu { get => VertexShader; set => VertexShader = value; }

    [Obsolete("Use PixelShader instead.")]
    public CPlugFileGPU? PHlslGpu { get => PixelShader; set => PixelShader = value; }

    [Obsolete("Use VertexLoadFxs instead.")]
    public GpuLoadFx[]? VHlslGpuLoadFxs1 { get => VertexLoadFxs; set => VertexLoadFxs = value!; }

    [Obsolete("Use PixelLoadFxs instead.")]
    public GpuLoadFx[]? PHlslGpuLoadFxs2 { get => PixelLoadFxs; set => PixelLoadFxs = value!; }

    [Obsolete("Use VertexFxValues instead.")]
    public Vec4[]? VHlslU01 { get => VertexFxValues; set => VertexFxValues = value!; }

    [Obsolete("Use PixelFxValues instead.")]
    public Vec4[]? PHlslU01 { get => PixelFxValues; set => PixelFxValues = value!; }

    [Obsolete("Use VertexPipeline instead.")]
    public PipelineGpu? VHlsl { get => VertexPipeline; set => VertexPipeline = value; }

    [Obsolete("Use PixelPipeline instead.")]
    public PipelineGpu? PHlsl { get => PixelPipeline; set => PixelPipeline = value; }

    public partial class PipelineGpu
    {
        [Obsolete("Use HasExternalFile instead.")]
        public bool U01 { get => HasExternalFile; set => HasExternalFile = value; }

        [Obsolete("Use FileGpu instead.")]
        public CMwNod? Script { get => FileGpu; set => FileGpu = (CPlugFileGPU?)value; }

        [Obsolete("Use FxValues instead.")]
        public Vec4[]? U02 { get => FxValues; set => FxValues = value!; }
    }

    public partial class GpuLoadFx
    {
        [Obsolete("Use Name instead.")]
        public string? U01 { get => Name; set => Name = value; }

        [Obsolete("Use RegistersPerElement instead.")]
        public int U03 { get => RegistersPerElement; set => RegistersPerElement = value; }

        [Obsolete("Use ElementCount instead.")]
        public int U04 { get => ElementCount; set => ElementCount = value; }

        [Obsolete("Use ValueIndex instead.")]
        public int U05 { get => ValueIndex; set => ValueIndex = value; }
    }

    [Obsolete("Use Sampler instead.")]
    public partial class UnknownStruct : Sampler
    {
        [Obsolete("Use Bitmap instead.")]
        public CPlugBitmap? U01 { get => Bitmap; set => Bitmap = value; }

        [Obsolete("Use Name instead.")]
        public string? U02 { get => Name; set => Name = value; }
    }
}
