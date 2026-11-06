namespace GBX.NET.Engines.Plug;

public partial class CPlugMaterial
{
    [Obsolete("Use MaterialCustom instead.")]
    public CPlugMaterialCustom? CustomMaterial { get => MaterialCustom; set => MaterialCustom = value; }

    [Obsolete("Use MaterialModel instead.")]
    public CPlug? Shader { get => MaterialModel; set => MaterialModel = (CPlugMaterial?)value; }

    public partial class DeviceMat
    {
        [Obsolete("Use RenderDevice instead.")]
        public short U01 { get => RenderDevice; set => RenderDevice = value; }

        [Obsolete("Use SubDevice instead.")]
        public short U02 { get => (short)SubDevice; set => SubDevice = value; }

        [Obsolete("Use IsShaderExternal instead.")]
        public bool U03 { get => IsShaderExternal; set => IsShaderExternal = value; }

        [Obsolete("Use Shader instead.")]
        public CPlugShader? Shader1 { get => Shader; set => Shader = value; }

        [Obsolete("Use ShaderDay instead.")]
        public CPlugShader? Shader2 { get => ShaderDay; set => ShaderDay = value; }

        [Obsolete("Use ShaderNight instead.")]
        public CPlugShader? Shader3 { get => ShaderNight; set => ShaderNight = value; }
    }

    public partial class ShadingDesc
    {
        [Obsolete("Use MaterialModelFileName instead.")]
        public string? U01 { get => MaterialModelFileName; set => MaterialModelFileName = value; }

        [Obsolete("Use CustomMaterialCrc64 instead.")]
        public ulong U02 { get => CustomMaterialCrc64; set => CustomMaterialCrc64 = value; }

        [Obsolete("Use MaterialModelRevision instead.")]
        public int U03 { get => MaterialModelRevision; set => MaterialModelRevision = value; }

        [Obsolete("Use VertexDeclarationFlags instead.")]
        public uint U04 { get => VertexDeclarationFlags; set => VertexDeclarationFlags = value; }

        [Obsolete("Use GeometryExtraFlags instead.")]
        public uint U06 { get => GeometryExtraFlags; set => GeometryExtraFlags = value; }

        [Obsolete("Use Flags instead.")]
        public uint U07 { get => Flags; set => Flags = value; }

        [Obsolete("Use PreLightGenTexCoordIndex instead.")]
        public int U08 { get => PreLightGenTexCoordIndex; set => PreLightGenTexCoordIndex = value; }

        [Obsolete("Use PreLightGenScale instead.")]
        public float U09 { get => PreLightGenScale; set => PreLightGenScale = value; }

        [Obsolete("Use PreLightGenProbeTexCoordIndex instead.")]
        public int U10 { get => PreLightGenProbeTexCoordIndex; set => PreLightGenProbeTexCoordIndex = value; }

        [Obsolete("Use PreLightGenFlags instead.")]
        public uint U11 { get => PreLightGenFlags; set => PreLightGenFlags = value; }
    }
}
