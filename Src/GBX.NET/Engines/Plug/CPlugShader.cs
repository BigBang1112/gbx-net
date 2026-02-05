using static GBX.NET.BitHelper;

namespace GBX.NET.Engines.Plug;

public partial class CPlugShader
{
    public bool IsDoubleSided
    {
        get => GetBit(RequirementFlags, 0xA);
        set => RequirementFlags = SetBit(RequirementFlags, 0xA, value);
    }

    public int BiasZ
    {
        get => GetBitRange(RequirementFlags, 0x2, 5);
        set => RequirementFlags = SetBitRange(RequirementFlags, 0x2, 5, value);
    }

    public EFillMode FillMode
    {
        get => (EFillMode)GetBitRange(RequirementFlags, 0, 2);
        set => RequirementFlags = SetBitRange(RequirementFlags, 0, 2, (int)value);
    }

    public bool ShadowRecvGrp0
    {
        get => GetBit(RequirementFlags, 0x24);
        set => RequirementFlags = SetBit(RequirementFlags, 0x24, value);
    }

    public bool ShadowRecvGrp1
    {
        get => GetBit(RequirementFlags, 0x25);
        set => RequirementFlags = SetBit(RequirementFlags, 0x25, value);
    }

    public bool ShadowRecvGrp2
    {
        get => GetBit(RequirementFlags, 0x26);
        set => RequirementFlags = SetBit(RequirementFlags, 0x26, value);
    }

    public bool ShadowRecvGrp3
    {
        get => GetBit(RequirementFlags, 0x27);
        set => RequirementFlags = SetBit(RequirementFlags, 0x27, value);
    }

    public bool ShadowMPassEnable
    {
        get => GetBit(RequirementFlags, 0xB);
        set => RequirementFlags = SetBit(RequirementFlags, 0xB, value);
    }

    public bool ShadowCasterDisable
    {
        get => GetBit(RequirementFlags, 0x32);
        set => RequirementFlags = SetBit(RequirementFlags, 0x32, value);
    }

    public bool ShadowImageSpaceDisable
    {
        get => GetBit(RequirementFlags, 0x35);
        set => RequirementFlags = SetBit(RequirementFlags, 0x35, value);
    }

    public bool ProjectorReceiver
    {
        get => GetBit(RequirementFlags, 0xC);
        set => RequirementFlags = SetBit(RequirementFlags, 0xC, value);
    }

    public bool StaticLighting
    {
        get => GetBit(RequirementFlags, 0xF);
        set => RequirementFlags = SetBit(RequirementFlags, 0xF, value);
    }

    public bool IsFogEyeZEnable
    {
        get => GetBit(RequirementFlags, 0xD);
        set => RequirementFlags = SetBit(RequirementFlags, 0xD, value);
    }

    public bool TweakFogColorBlack
    {
        get => GetBit(RequirementFlags, 0x33);
        set => RequirementFlags = SetBit(RequirementFlags, 0x33, value);
    }

    public bool IsFogPlaneEnable
    {
        get => GetBit(RequirementFlags, 0xE);
        set => RequirementFlags = SetBit(RequirementFlags, 0xE, value);
    }

    public bool TransTreeMip
    {
        get => GetBit(RequirementFlags, 0x9);
        set => RequirementFlags = SetBit(RequirementFlags, 0x9, value);
    }

    public int TexCoordCount
    {
        get => GetBitRange(RequirementFlags, 0x20, 4);
        set => RequirementFlags = SetBitRange(RequirementFlags, 0x20, 4, value);
    }

    public bool GReqColor0
    {
        get => GetBit(RequirementFlags, 0x13);
        set => RequirementFlags = SetBit(RequirementFlags, 0x13, value);
    }

    public bool IsTranslucent
    {
        get => GetBit(RequirementFlags, 0x7);
        set => RequirementFlags = SetBit(RequirementFlags, 0x7, value);
    }

    public bool IsAlphaBlended
    {
        get => GetBit(RequirementFlags, 0x8);
        set => RequirementFlags = SetBit(RequirementFlags, 0x8, value);
    }

    public bool Alpha01SoftEdges
    {
        get => GetBit(RequirementFlags, 0x1F);
        set => RequirementFlags = SetBit(RequirementFlags, 0x1F, value);
    }

    public EVertexSpace VertexSpace
    {
        get => (EVertexSpace)GetBitRange(RequirementFlags, 0x36, 2);
        set => RequirementFlags = SetBitRange(RequirementFlags, 0x36, 2, (int)value);
    }

    public bool VIdReflected
    {
        get => GetBit(VisibleIdFlags, 0);
        set => VisibleIdFlags = SetBit(VisibleIdFlags, 0, value);
    }

    public bool VIdReflectMirror
    {
        get => GetBit(VisibleIdFlags, 0x1);
        set => VisibleIdFlags = SetBit(VisibleIdFlags, 0x1, value);
    }

    public bool VIdRefracted
    {
        get => GetBit(VisibleIdFlags, 0x2);
        set => VisibleIdFlags = SetBit(VisibleIdFlags, 0x2, value);
    }

    public bool VIdViewDepBump
    {
        get => GetBit(VisibleIdFlags, 0x3);
        set => VisibleIdFlags = SetBit(VisibleIdFlags, 0x3, value);
    }

    public bool VIdViewDepOcclusion
    {
        get => GetBit(VisibleIdFlags, 0x4);
        set => VisibleIdFlags = SetBit(VisibleIdFlags, 0x4, value);
    }

    public bool VIdOnlyRefracted
    {
        get => GetBit(VisibleIdFlags, 0x5);
        set => VisibleIdFlags = SetBit(VisibleIdFlags, 0x5, value);
    }

    public bool VIdHideWhenUnderground
    {
        get => GetBit(VisibleIdFlags, 0x6);
        set => VisibleIdFlags = SetBit(VisibleIdFlags, 0x6, value);
    }

    public bool VIdHideWhenOverground
    {
        get => GetBit(VisibleIdFlags, 0x7);
        set => VisibleIdFlags = SetBit(VisibleIdFlags, 0x7, value);
    }

    public bool VIdViewDepWindIntens
    {
        get => GetBit(VisibleIdFlags, 0x9);
        set => VisibleIdFlags = SetBit(VisibleIdFlags, 0x9, value);
    }

    public bool VIdBackground
    {
        get => GetBit(VisibleIdFlags, 0xA);
        set => VisibleIdFlags = SetBit(VisibleIdFlags, 0xA, value);
    }

    public bool VIdGrassRGB
    {
        get => GetBit(VisibleIdFlags, 0xB);
        set => VisibleIdFlags = SetBit(VisibleIdFlags, 0xB, value);
    }

    public bool VIdLightGenP
    {
        get => GetBit(VisibleIdFlags, 0xC);
        set => VisibleIdFlags = SetBit(VisibleIdFlags, 0xC, value);
    }

    public bool VIdVehicle
    {
        get => GetBit(VisibleIdFlags, 0xD);
        set => VisibleIdFlags = SetBit(VisibleIdFlags, 0xD, value);
    }

    public bool VIdHideOnlyDirect
    {
        get => GetBit(VisibleIdFlags, 0xE);
        set => VisibleIdFlags = SetBit(VisibleIdFlags, 0xE, value);
    }

    public bool SortCustom
    {
        get => GetBit(RequirementFlags, 0x12);
        set => RequirementFlags = SetBit(RequirementFlags, 0x12, value);
    }

    public ESortPosition SortPosition
    {
        get => (ESortPosition)GetBitRange(RequirementFlags, 0x1A, 2);
        set => RequirementFlags = SetBitRange(RequirementFlags, 0x1A, 2, (int)value);
    }

    public bool SortIndex
    {
        get => GetBit(RequirementFlags, 0x19);
        set => RequirementFlags = SetBit(RequirementFlags, 0x19, value);
    }

    public bool ComputeFillValue
    {
        get => GetBit(RequirementFlags, 0x31);
        set => RequirementFlags = SetBit(RequirementFlags, 0x31, value);
    }

    public partial class Chunk09002014
    {
        public override void ReadWrite(CPlugShader n, GbxReaderWriter rw)
        {
            rw.DataUInt64(ref n.requirementFlags); // SRequirement
            rw.Single(ref U01);
            rw.NodeRef<CMwNod>(ref U02);
            rw.UInt16(ref n.visibleIdFlags);

            n.requirementFlags = SortOldRequirementFlags(n.requirementFlags);
        }
    }

    private static ulong SortOldRequirementFlags(ulong value)
    {
        if ((value & 0x40000UL) == 0)
            return value;

        var mode = (value >> 26) & 0x3UL;

        return mode switch
        {
            0 => value & 0xF3FFFFFFUL,
            1 => (value & 0xFBFFFFFFUL) | 0x08000000UL,
            _ => value | 0x0C000000UL
        };
    }
}
