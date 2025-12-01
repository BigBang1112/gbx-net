namespace GBX.NET.Engines.Plug;

public partial class CPlugShader
{
    public bool IsDoubleSided
    {
        get => (RequirementFlags >> 0xA & 0x1) != 0;
        set => RequirementFlags = value ? (RequirementFlags | 0x400) : (RequirementFlags & ~0x400ul);
    }

    public int BiasZ
    {
        get => (int)(RequirementFlags >> 0x2 & 0x1F);
        set => RequirementFlags = (RequirementFlags & ~0x7Cul) | ((ulong)(value & 0x1F) << 0x2);
    }

    public EFillMode FillMode
    {
        get => (EFillMode)(RequirementFlags & 0x3);
        set => RequirementFlags = (RequirementFlags & ~0x3ul) | ((ulong)value & 0x3);
    }

    public int TexCoordCount
    {
        get => (int)(RequirementFlags >> 0x20 & 0xF);
        set => RequirementFlags = (RequirementFlags & ~0xF00000000ul) | ((ulong)(value & 0xF) << 0x20);
    }


    public bool ShadowRecvGrp0
    {
        get => (RequirementFlags >> 0x24 & 0x1) != 0;
        set => RequirementFlags = value ? (RequirementFlags | 0x1000000000UL) : (RequirementFlags & ~0x1000000000UL);
    }

    public bool ShadowRecvGrp1
    {
        get => (RequirementFlags >> 0x25 & 0x1) != 0;
        set => RequirementFlags = value ? (RequirementFlags | 0x2000000000UL) : (RequirementFlags & ~0x2000000000UL);
    }

    public bool ShadowRecvGrp2
    {
        get => (RequirementFlags >> 0x26 & 0x1) != 0;
        set => RequirementFlags = value ? (RequirementFlags | 0x4000000000UL) : (RequirementFlags & ~0x4000000000UL);
    }

    public bool ShadowRecvGrp3
    {
        get => (RequirementFlags >> 0x27 & 0x1) != 0;
        set => RequirementFlags = value ? (RequirementFlags | 0x8000000000UL) : (RequirementFlags & ~0x8000000000UL);
    }

    public bool ShadowMPassEnable
    {
        get => (RequirementFlags >> 0xB & 0x1) != 0;
        set => RequirementFlags = value ? (RequirementFlags | 0x800UL) : (RequirementFlags & ~0x800UL);
    }

    public bool ShadowCasterDisable
    {
        get => (RequirementFlags >> 0x32 & 0x1) != 0;
        set => RequirementFlags = value ? (RequirementFlags | 0x4000000000000UL) : (RequirementFlags & ~0x4000000000000UL);
    }

    public bool ShadowImageSpaceDisable
    {
        get => (RequirementFlags >> 0x35 & 0x1) != 0;
        set => RequirementFlags = value ? (RequirementFlags | 0x20000000000000UL) : (RequirementFlags & ~0x20000000000000UL);
    }

    public bool ProjectorReceiver
    {
        get => (RequirementFlags >> 0xC & 0x1) != 0;
        set => RequirementFlags = value ? (RequirementFlags | 0x1000UL) : (RequirementFlags & ~0x1000UL);
    }

    public bool StaticLighting
    {
        get => (RequirementFlags >> 0xF & 0x1) != 0;
        set => RequirementFlags = value ? (RequirementFlags | 0x8000UL) : (RequirementFlags & ~0x8000UL);
    }

    public bool IsFogEyeZEnable
    {
        get => (RequirementFlags >> 0xD & 0x1) != 0;
        set => RequirementFlags = value ? (RequirementFlags | 0x2000UL) : (RequirementFlags & ~0x2000UL);
    }

    public bool TweakFogColorBlack
    {
        get => (RequirementFlags >> 0x33 & 0x1) != 0;
        set => RequirementFlags = value ? (RequirementFlags | 0x8000000000000UL) : (RequirementFlags & ~0x8000000000000UL);
    }

    public bool IsFogPlaneEnable
    {
        get => (RequirementFlags >> 0xE & 0x1) != 0;
        set => RequirementFlags = value ? (RequirementFlags | 0x4000UL) : (RequirementFlags & ~0x4000UL);
    }

    public bool TransTreeMip
    {
        get => (RequirementFlags >> 0x9 & 0x1) != 0;
        set => RequirementFlags = value ? (RequirementFlags | 0x200UL) : (RequirementFlags & ~0x200UL);
    }

    public bool IsTranslucent
    {
        get => (RequirementFlags >> 0x7 & 0x1) != 0;
        set => RequirementFlags = value ? (RequirementFlags | 0x80UL) : (RequirementFlags & ~0x80UL);
    }

    public bool IsAlphaBlended
    {
        get => (RequirementFlags >> 0x8 & 0x1) != 0;
        set => RequirementFlags = value ? (RequirementFlags | 0x100UL) : (RequirementFlags & ~0x100UL);
    }

    public bool Alpha01SoftEdges
    {
        get => (RequirementFlags >> 0x1F & 0x1) != 0;
        set => RequirementFlags = value ? (RequirementFlags | 0x80000000UL) : (RequirementFlags & ~0x80000000UL);
    }

    public EVertexSpace VertexSpace
    {
        get => (EVertexSpace)(RequirementFlags >> 0x36 & 0x3);
        set => RequirementFlags = (RequirementFlags & ~0xC0000000000000UL) | (((ulong)value & 0x3) << 0x36);
    }

    public bool SortCustom
    {
        get => (RequirementFlags >> 0x12 & 0x1) != 0;
        set => RequirementFlags = value ? (RequirementFlags | 0x40000UL) : (RequirementFlags & ~0x40000UL);
    }

    public ESortPosition SortPosition
    {
        get => (ESortPosition)(RequirementFlags >> 0x1A & 0x3);
        set => RequirementFlags = (RequirementFlags & ~0xC000000UL) | (((ulong)value & 0x3) << 0x1A);
    }

    public bool SortIndex
    {
        get => (RequirementFlags >> 0x19 & 0x1) != 0;
        set => RequirementFlags = value ? (RequirementFlags | 0x2000000UL) : (RequirementFlags & ~0x2000000UL);
    }

    public bool ComputeFillValue
    {
        get => (RequirementFlags >> 0x31 & 0x1) != 0;
        set => RequirementFlags = value ? (RequirementFlags | 0x2000000000000UL) : (RequirementFlags & ~0x2000000000000UL);
    }

    public bool VIdReflected
    {
        get => (VisibleIdFlags & 0x1) != 0;
        set => VisibleIdFlags = (ushort)(value ? (VisibleIdFlags | 0x1) : (VisibleIdFlags & ~0x1));
    }

    public bool VIdReflectMirror
    {
        get => (VisibleIdFlags >> 0x1 & 0x1) != 0;
        set => VisibleIdFlags = (ushort)(value ? (VisibleIdFlags | 0x2) : (VisibleIdFlags & ~0x2));
    }

    public bool VIdRefracted
    {
        get => (VisibleIdFlags >> 0x2 & 0x1) != 0;
        set => VisibleIdFlags = (ushort)(value ? (VisibleIdFlags | 0x4) : (VisibleIdFlags & ~0x4));
    }

    public bool VIdViewDepBump
    {
        get => (VisibleIdFlags >> 0x3 & 0x1) != 0;
        set => VisibleIdFlags = (ushort)(value ? (VisibleIdFlags | 0x8) : (VisibleIdFlags & ~0x8));
    }

    public bool VIdViewDepOcclusion
    {
        get => (VisibleIdFlags >> 0x4 & 0x1) != 0;
        set => VisibleIdFlags = (ushort)(value ? (VisibleIdFlags | 0x10) : (VisibleIdFlags & ~0x10));
    }

    public bool VIdOnlyRefracted
    {
        get => (VisibleIdFlags >> 0x5 & 0x1) != 0;
        set => VisibleIdFlags = (ushort)(value ? (VisibleIdFlags | 0x20) : (VisibleIdFlags & ~0x20));
    }

    public bool VIdHideWhenUnderground
    {
        get => (VisibleIdFlags >> 0x6 & 0x1) != 0;
        set => VisibleIdFlags = (ushort)(value ? (VisibleIdFlags | 0x40) : (VisibleIdFlags & ~0x40));
    }

    public bool VIdHideWhenOverground
    {
        get => (VisibleIdFlags >> 0x7 & 0x1) != 0;
        set => VisibleIdFlags = (ushort)(value ? (VisibleIdFlags | 0x80) : (VisibleIdFlags & ~0x80));
    }

    public bool VIdViewDepWindIntens
    {
        get => (VisibleIdFlags >> 0x9 & 0x1) != 0;
        set => VisibleIdFlags = (ushort)(value ? (VisibleIdFlags | 0x200) : (VisibleIdFlags & ~0x200));
    }

    public bool VIdBackground
    {
        get => (VisibleIdFlags >> 0xA & 0x1) != 0;
        set => VisibleIdFlags = (ushort)(value ? (VisibleIdFlags | 0x400) : (VisibleIdFlags & ~0x400));
    }

    public bool VIdGrassRGB
    {
        get => (VisibleIdFlags >> 0xB & 0x1) != 0;
        set => VisibleIdFlags = (ushort)(value ? (VisibleIdFlags | 0x800) : (VisibleIdFlags & ~0x800));
    }

    public bool VIdLightGenP
    {
        get => (VisibleIdFlags >> 0xC & 0x1) != 0;
        set => VisibleIdFlags = (ushort)(value ? (VisibleIdFlags | 0x1000) : (VisibleIdFlags & ~0x1000));
    }

    public bool VIdVehicle
    {
        get => (VisibleIdFlags >> 0xD & 0x1) != 0;
        set => VisibleIdFlags = (ushort)(value ? (VisibleIdFlags | 0x2000) : (VisibleIdFlags & ~0x2000));
    }

    public bool VIdHideOnlyDirect
    {
        get => (VisibleIdFlags >> 0xE & 0x1) != 0;
        set => VisibleIdFlags = (ushort)(value ? (VisibleIdFlags | 0x4000) : (VisibleIdFlags & ~0x4000));
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
