using GBX.NET.Attributes;

namespace GBX.NET.Engines.Plug;

public partial class CPlugWeather
{
    /// <remarks>Applied with: 0x000, 0x001, 0x002, 0x003, 0x004, 0x005, 0x008, 0x009, 0x00A, 0x00C, 0x00E body chunks</remarks>
    [AppliedWithChunk<Chunk0917E000>]
    [AppliedWithChunk<Chunk0917E001>]
    [AppliedWithChunk<Chunk0917E002>]
    [AppliedWithChunk<Chunk0917E003>]
    [AppliedWithChunk<Chunk0917E004>]
    [AppliedWithChunk<Chunk0917E005>]
    [AppliedWithChunk<Chunk0917E008>]
    [AppliedWithChunk<Chunk0917E009>]
    [AppliedWithChunk<Chunk0917E00A>]
    [AppliedWithChunk<Chunk0917E00C>]
    [AppliedWithChunk<Chunk0917E00E>]
    public EGxFogFormula FogFormula
    {
        get => (EGxFogFormula)((FogFlags >> 1) & 3);
        set => FogFlags = (FogFlags & 0xFFFFFFF9) | (((uint)value & 3) << 1);
    }

    /// <remarks>Applied with: 0x000, 0x001, 0x002, 0x003, 0x004, 0x005, 0x008, 0x009, 0x00A, 0x00C, 0x00E body chunks</remarks>
    [AppliedWithChunk<Chunk0917E000>]
    [AppliedWithChunk<Chunk0917E001>]
    [AppliedWithChunk<Chunk0917E002>]
    [AppliedWithChunk<Chunk0917E003>]
    [AppliedWithChunk<Chunk0917E004>]
    [AppliedWithChunk<Chunk0917E005>]
    [AppliedWithChunk<Chunk0917E008>]
    [AppliedWithChunk<Chunk0917E009>]
    [AppliedWithChunk<Chunk0917E00A>]
    [AppliedWithChunk<Chunk0917E00C>]
    [AppliedWithChunk<Chunk0917E00E>]
    public EGxFogSpace FogSpace
    {
        get => (EGxFogSpace)((FogFlags >> 3) & 1);
        set => FogFlags = (FogFlags & 0xFFFFFFF7) | (((uint)value & 1) << 3);
    }

    public partial class Chunk0917E007
    {
        public override void ReadWrite(CPlugWeather n, GbxReaderWriter rw)
        {
            // Empty
        }
    }

    public partial class Chunk0917E010
    {
        public override void ReadWrite(CPlugWeather n, GbxReaderWriter rw)
        {
            if (rw.Reader is not null)
            {
                var inlineClouds = new CPlugClouds();
                NodeRef.SetEmbedded(ref n.cloudsRef, inlineClouds);

                // Keep the inline data when clouds are later saved as a node reference.
                inlineClouds.CreateChunk<CPlugClouds.Chunk09180000>();
            }

            var clouds = n.cloudsRef?.CachedNode ?? throw new InvalidOperationException("Inline clouds are missing.");
            clouds.SolidFids = rw.ListNodeRef(clouds.SolidFids);
            clouds.BottomNearZ = rw.Single(clouds.BottomNearZ);
            if (rw.Reader is not null)
            {
                clouds.BottomFarZ = clouds.BottomNearZ;
            }
        }
    }

    public partial class Chunk0917E012
    {
        public override void ReadWrite(CPlugWeather n, GbxReaderWriter rw)
        {
            base.ReadWrite(n, rw);
            var clouds = n.cloudsRef?.CachedNode ?? throw new InvalidOperationException("Inline clouds are missing.");
            clouds.BottomFarZ = rw.Single(clouds.BottomFarZ);
            rw.NodeRef(clouds.References.ImageColorMin);
            rw.NodeRef(clouds.References.ImageColorMax);
        }
    }

    public partial class Chunk0917E015
    {
        public override void ReadWrite(CPlugWeather n, GbxReaderWriter rw)
        {
            base.ReadWrite(n, rw);
            var clouds = n.cloudsRef?.CachedNode ?? throw new InvalidOperationException("Inline clouds are missing.");
            clouds.Lighting = rw.Int32(clouds.Lighting);
        }
    }
}
