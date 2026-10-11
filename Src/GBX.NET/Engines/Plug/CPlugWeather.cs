using GBX.NET.Attributes;

namespace GBX.NET.Engines.Plug;

public partial class CPlugWeather
{
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
                n.clouds = new CPlugClouds();
                n.cloudsFile = null;
                // Keep the inline data when clouds are later saved as a node reference.
                n.clouds.CreateChunk<CPlugClouds.Chunk09180000>();
            }

            var clouds = n.clouds ?? throw new InvalidOperationException("Inline clouds are missing.");
            clouds.SolidFids = rw.ArrayNodeRef(clouds.SolidFids);
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
            var clouds = n.clouds!;
            clouds.BottomFarZ = rw.Single(clouds.BottomFarZ);
            var minFile = clouds.ImageColorMinFile;
            var maxFile = clouds.ImageColorMaxFile;
            clouds.ImageColorMin = rw.NodeRef(minFile is null ? clouds.ImageColorMin : null, ref minFile);
            clouds.ImageColorMax = rw.NodeRef(maxFile is null ? clouds.ImageColorMax : null, ref maxFile);
            clouds.ImageColorMinFile = minFile;
            clouds.ImageColorMaxFile = maxFile;
        }
    }

    public partial class Chunk0917E015
    {
        public override void ReadWrite(CPlugWeather n, GbxReaderWriter rw)
        {
            base.ReadWrite(n, rw);
            n.clouds!.Lighting = rw.Int32(n.clouds.Lighting);
        }
    }
}
