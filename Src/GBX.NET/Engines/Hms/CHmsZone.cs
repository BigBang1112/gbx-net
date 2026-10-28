using GBX.NET.Attributes;

namespace GBX.NET.Engines.Hms;

public partial class CHmsZone
{
    /// <remarks>Applied with: 0x002 body chunk</remarks>
    [AppliedWithChunk<Chunk06004002>]
    public EGxFogFormula FogFormula
    {
        get => (EGxFogFormula)((FogFlags >> 1) & 3);
        set => FogFlags = (FogFlags & 0xFFFFFFF9) | (((uint)value & 3) << 1);
    }

    /// <remarks>Applied with: 0x002 body chunk</remarks>
    [AppliedWithChunk<Chunk06004002>]
    public EGxFogSpace FogSpace
    {
        get => (EGxFogSpace)((FogFlags >> 3) & 1);
        set => FogFlags = (FogFlags & 0xFFFFFFF7) | (((uint)value & 1) << 3);
    }
}
