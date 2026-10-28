using static GBX.NET.BitHelper;

namespace GBX.NET.Engines.Plug;

public partial class CPlugShaderApply
{
    public int BlendSrc
    {
        get => GetBitRange(ApplyField, 0, 5);
        set => ApplyField = SetBitRange(ApplyField, 0, 5, value);
    }

    public int BlendDst
    {
        get => GetBitRange(ApplyField, 5, 5);
        set => ApplyField = SetBitRange(ApplyField, 5, 5, value);
    }

    public int BlendOp
    {
        get => GetBitRange(ApplyField, 10, 3);
        set => ApplyField = SetBitRange(ApplyField, 10, 3, value);
    }

    public int AlphaRef
    {
        get => GetBitRange(ApplyField, 14, 8);
        set => ApplyField = SetBitRange(ApplyField, 14, 8, value);
    }

    public bool BlendFromOpacityMap
    {
        get => GetBit(ApplyField, 22);
        set => ApplyField = SetBit(ApplyField, 22, value);
    }

    public bool StencilToFillOnce
    {
        get => GetBit(ApplyField, 23);
        set => ApplyField = SetBit(ApplyField, 23, value);
    }

    public int AlphaCmpPass
    {
        get => GetBitRange(ApplyField, 24, 3);
        set => ApplyField = SetBitRange(ApplyField, 24, 3, value);
    }

    public bool AlphaToCoverage
    {
        get => GetBit(ApplyField, 27);
        set => ApplyField = SetBit(ApplyField, 27, value);
    }

    public bool ForceIsAlphaBlend
    {
        get => GetBit(ApplyField, 28);
        set => ApplyField = SetBit(ApplyField, 28, value);
    }

    public int ShadingSpace
    {
        get => GetBitRange(ApplyField, 29, 1);
        set => ApplyField = SetBitRange(ApplyField, 29, 1, value);
    }

    public bool ForceAlphaToCoverage
    {
        get => GetBit(ApplyField, 30);
        set => ApplyField = SetBit(ApplyField, 30, value);
    }

    public int BlendDisableRTMask
    {
        get => GetBitRange(ApplyField3, 14, 4);
        set => ApplyField3 = SetBitRange(ApplyField3, 14, 4, value);
    }

    public bool IsFrameAdditive
    {
        get => GetBit(LegacyMaterialFlags, 0);
        set => LegacyMaterialFlags = SetBit(LegacyMaterialFlags, 0, value);
    }

    public bool HasOpacityInVertexColors
    {
        get => GetBit(LegacyMaterialFlags, 1);
        set => LegacyMaterialFlags = SetBit(LegacyMaterialFlags, 1, value);
    }

    public bool HasOpacityInDiffuse
    {
        get => GetBit(LegacyMaterialFlags, 2);
        set => LegacyMaterialFlags = SetBit(LegacyMaterialFlags, 2, value);
    }

    public bool HasShinynessInDiffuse
    {
        get => GetBit(LegacyMaterialFlags, 3);
        set => LegacyMaterialFlags = SetBit(LegacyMaterialFlags, 3, value);
    }

    public bool DoesEnvMapUseShinyness
    {
        get => GetBit(LegacyMaterialFlags, 4);
        set => LegacyMaterialFlags = SetBit(LegacyMaterialFlags, 4, value);
    }

    public bool DoesSpecularUseShinyness
    {
        get => GetBit(LegacyMaterialFlags, 5);
        set => LegacyMaterialFlags = SetBit(LegacyMaterialFlags, 5, value);
    }
}
