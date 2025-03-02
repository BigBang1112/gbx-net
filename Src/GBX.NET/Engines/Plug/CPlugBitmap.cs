namespace GBX.NET.Engines.Plug;

public partial class CPlugBitmap
{
    public EUsage Usage => (EGxUVGenerate)(Flags & 0xFF);
    public bool NormalAreSigned => (Flags >> 62 & 1) != 0;
    public bool NormalCanBeSigned => (Flags >> 61 & 1) != 0;
    public EColorDepth WantedColorDepth => (EColorDepth)(Flags >> 16 & 3);
    public bool IsOneBitAlpha => (Flags >> 21 & 1) != 0;
    public bool IgnoreImageAlpha01 => (Flags >> 33 & 1) != 0;
    public bool ShadowCasterIgnoreAlpha => (Flags >> 34 & 1) != 0;
    public bool AlphaToCoverage => (Flags2 >> 24 & 1) != 0;
    public bool IsNonPow2Conditional => (Flags >> 31 & 1) != 0;
    public bool IsCubeMap => (Flags >> 19 & 1) != 0;
    public bool IsOriginTop => (Flags2 >> 10 & 1) != 0;
    public ECubeFace CubeMapAuto2dFace => (ECubeFace)(Flags >> 46 & 7);
    public EGxTexFilter TexFilter => (EGxTexFilter)(Flags >> 53 & 3);
    public EGxTexAddress TexAddressU => (EGxTexAddress)(Flags >> 25 & 3);
    public EGxTexAddress TexAddressV => (EGxTexAddress)(Flags >> 27 & 3);
    public EGxTexAddress TexAddressW => (EGxTexAddress)(Flags2 >> 8 & 3);
    public bool RenderRequireBlending => (Flags >> 50 & 1) != 0;
    public bool ForceShaderGenerateUV => (Flags2 >> 15 & 1) != 0;
    public EGxUVGenerate GenerateUV => (EGxUVGenerate)(Flags2 >> 16 & 0xFF);
}
